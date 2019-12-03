using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using HtmlAgilityPack;

namespace InstagramVideoScraper {
	public partial class Form1 : Form {

		const int Second = 1000;
		const int Minute = 60 * Second;
		const int MaxVideosToDownload = 3;
		int maxVideosToDownload = 0;
		int maxImagesToDownload = 0;
		const int TimeToSearchForVideos = 3 * Minute;
		const int TimeToWaitForNewPosts = 4 * Second;

		ChromiumWebBrowser chromium;
		ChromiumWebBrowser chromiumTwo;

		DownloadMode downloadMode = DownloadMode.Videos;
		HtmlAgilityPack.HtmlDocument globalDocument = null;
		bool hasSelectedFolder = false;
		string lastDocument = null;
		bool firstLoad = true;
		bool firstLoadTwo = true;
		bool downloadInProgress = false;
		double profilePageScrapeStartTime = 0;
		int sameDocumentCounter = 0;
		int numberOfVideosDownloaded = 0;
		int numberOfImagesDownloaded = 0;
		int depthCounter = 0;
		int counter = -1;
		int lastPostNumber = 0;
		string currentProfileName = null;
		string currentPostPageLink = null;
		bool scraping = false;
		string[] links = null;
		Point depth = new Point(0, 0);

		private void ResetPageInfoValues() {
			lastDocument = null;
			profilePageScrapeStartTime = 0;
			currentProfileName = null;
			currentPostPageLink = null;
			numberOfVideosDownloaded = 0;
			numberOfImagesDownloaded = 0;
			sameDocumentCounter = 0;
			depthCounter = 0;
			depth.X = 0;
			depth.Y = 0;
			lastPostNumber = 0;
		}

		private void Start() {
			links = GetLinksFromTextBox();
			if (links.Length == 0)
				return;
			button2.Text = "Stop";
			downloadMode = GetSelectedDownloadMode();
			scraping = true;
			sameDocumentCounter = 0;
			numberOfVideosDownloaded = 0;
			numberOfImagesDownloaded = 0;
			counter = -1;
			lastDocument = null;
			currentProfileName = null;
			TryLoadNextLink();
		}

		private DownloadMode GetSelectedDownloadMode() {
			if (radioButtonVideos.Checked)
				return DownloadMode.Videos;
			if (radioButtonImages.Checked)
				return DownloadMode.Images;
			return DownloadMode.VideosAndImages;
		}

		private double GetUnixTimestamp() {
			double unixTimestamp = (DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
			return unixTimestamp;
		}

		private string GetCurrentLink() {
			return links[counter];
		}

		public Form1() {
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e) {
			CheckForIllegalCrossThreadCalls = false;
			InitialiseBrowsers();
		}

		private void InitialiseBrowsers() {
			chromium = new ChromiumWebBrowser("https://google.com/");
			chromium.FrameLoadEnd += MainFrameLoad;
			chromium.Location = new Point(450, 10);
			chromium.Size = new Size(400, 500);
			chromium.Dock = DockStyle.None;
			Controls.Add(chromium);
			chromium.Visible = false;

			chromiumTwo = new ChromiumWebBrowser("https://google.com/");
			chromiumTwo.FrameLoadEnd += SecondFrameLoad;
			chromiumTwo.Location = new Point(860, 10);
			chromiumTwo.Size = new Size(400, 500);
			chromiumTwo.Dock = DockStyle.None;
			Controls.Add(chromiumTwo);
			chromiumTwo.Visible = false;
		}

		private string GetCurrentProfileName() {
			string currentLink = GetCurrentLink();
			string[] parts = currentLink.Split('/');
			int numberOfParts = parts.Length;
			if (currentLink.LastIndexOf('/') == currentLink.Length - 1) {
				return parts[numberOfParts - 2].ToLower();
			}
			return parts[numberOfParts - 1].ToLower();
		}

		private void CreateDirectory(string name) {
			Directory.CreateDirectory(textBox1.Text + "\\" + name);
		}

		private void MoveCounterToNextValidLink() {
			if (ReachedLastLink())
				return;
			counter++;
			while (!ReachedLastLink() && !IsValidURL(GetCurrentLink())) {
				counter++;
			}
		}

		private bool IsValidURL(string url) {
			url = url.Trim();
			Uri uriResult;
			bool result = Uri.TryCreate(url, UriKind.Absolute, out uriResult)
				&& (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
			return result;
		}

		private void TryLoadNextLink() {
			Log("Entered load link");
			MoveCounterToNextValidLink();
			Log(counter.ToString());
			if (ReachedLastLink()) {
				Log("Reached last link");
				Stop();
				return;
			}
			ResetPageInfoValues();
			currentProfileName = GetCurrentProfileName();
			CreateDirectory(currentProfileName);
			profilePageScrapeStartTime = GetUnixTimestamp();
			textBox3.Text = GetCurrentLink();
			Log("Current link: " + GetCurrentLink());
			chromium.Load(GetCurrentLink());
		}

		private string GetCurrentDownloadPath() {
			return textBox1.Text + "\\" + currentProfileName;
		}

		private bool ReachedLastLink() {
			return counter == links.Length;
		}

		private void MainFrameLoad(object sender, FrameLoadEndEventArgs e) {
			if (e.Frame.IsMain)
				Log("Main frame loaded");
			if (e.Frame.IsMain && !scraping)
				Log("Not scraping");
			if (!ShouldRunFrameLoadHandler(e))
				return;
			Task.Run(TryDownloadMedia);
		}

		private string[] GetLinksFromTextBox() {
			string[] lines = textBox2.Lines;
			return lines;
		}

		private void SecondFrameLoad(object sender, FrameLoadEndEventArgs e) {
			if (!ShouldRunFrameLoadHandlerTwo(e))
				return;
			if (IsCurrentPostValidVideo()) {
				Task.Run(DownloadCurrentVideo);
			}
			else if (ShouldDownloadImages()) {
				Task.Run(DownloadCurrentImage);
			}
		}

		private async void TryDownloadMedia() {
			while (!DownloadedAllMediaOnPage() && scraping) {
				Log("Trying to download media; current number downloaded: " + numberOfVideosDownloaded.ToString());
				string document = await chromium.GetSourceAsync();
				if (HasDocumentChanged(document)) {
					globalDocument = new HtmlAgilityPack.HtmlDocument();
					globalDocument.LoadHtml(document);
					if (!IsValidHtmlDocument(globalDocument)) {
						TryLoadNextLink();
						return;
					}
					ExtractLinksAndDownloadMedia(globalDocument);
				}
				else if (ReachedEndOfDocument()) {
					TryLoadNextLink();
				}
				Log("TryDownloadMedia() Looping: " + scraping.ToString());
			}
		}

		private void ExtractLinksAndDownloadMedia(HtmlAgilityPack.HtmlDocument htmlDocument) {
			HtmlNodeCollection htmlNodes = htmlDocument.DocumentNode.SelectNodes(RootDivXPath);
			foreach (HtmlNode node in htmlNodes) {
				if (DownloadedAllMediaOnPage()) {
					Log("Downloaded all media; Last post number: " + lastPostNumber);
					Task.Delay(1000).ContinueWith((x) => TryLoadNextLink());
					return;
				}
				if (lastPostNumber >= GetCurrentPostNumberFromDepth()) {
					UpdateDepth();
					continue;
				}
				lastPostNumber++;
				HtmlNode anchorNode = node.SelectSingleNode(GetCurrentAnchorXPath());
				if (anchorNode != null) {
					currentPostPageLink = "https://instagram.com" + anchorNode.Attributes["href"].Value;
					if (ShouldDownloadVideos() && IsCurrentPostValidVideo()) {
						TryDownloadCurrentMedia();
					}
					else if (ShouldDownloadImages() && IsCurrentPostImage()) {
						TryDownloadCurrentMedia();
					}
				}
				LoopWhileDownloadInProgress();
				UpdateDepth();
			}
			ScrollToBottom();
			Log("ExtractLinks() Looping...");
			Thread.Sleep(TimeToWaitForNewPosts);
		}

		private bool ShouldDownloadVideos() {
			return (downloadMode == DownloadMode.Videos || downloadMode == DownloadMode.VideosAndImages) && numberOfVideosDownloaded < maxVideosToDownload;
		}

		private bool ShouldDownloadImages() {
			return (downloadMode == DownloadMode.Images || downloadMode == DownloadMode.VideosAndImages) && numberOfImagesDownloaded < maxImagesToDownload;
		}

		private bool testSwitch = false;
		private bool IsCurrentPostValidVideo() {
			HtmlNode videoSpanNode = globalDocument.DocumentNode.SelectSingleNode(GetCurrentVideoSpanXPath());
			if (videoSpanNode == null)
				return false;
			string ariaLabel = videoSpanNode.Attributes["aria-label"].Value;
			bool postIsVideo = ariaLabel == AriaLabelVideo;
			return postIsVideo;
		}

		private bool IsCurrentPostImage() {
			HtmlNode imageSpanNode = globalDocument.DocumentNode.SelectSingleNode(GetCurrentVideoSpanXPath());
			if (imageSpanNode == null)
				return true;
			string ariaLabel = imageSpanNode.Attributes["aria-label"].Value;
			bool postIsImage = ariaLabel == AriaLabelImage;
			return postIsImage;
		}

		private bool IsCurrentPostVideo() {
			HtmlNode videoSpanNode = globalDocument.DocumentNode.SelectSingleNode(GetCurrentVideoSpanXPath());
			if (videoSpanNode == null)
				return false;
			return true;
		}

		private int GetCurrentPostNumberFromDepth() {
			int currentPostNumber = depth.X * 3;
			currentPostNumber += (depth.Y % 3) + 1;
			return currentPostNumber;
		}

		private void LoopWhileDownloadInProgress() {
			while (downloadInProgress) { }
		}

		private void TryDownloadCurrentMedia() {
			downloadInProgress = true;
			Log("Loading: " + currentPostPageLink);
			chromiumTwo.Load(currentPostPageLink);
		}

		const string AriaLabelVideo = "Video";

		private async void DownloadCurrentVideo() {
			Log("Downloading video");
			string document = await chromiumTwo.GetSourceAsync();
			HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
			htmlDocument.LoadHtml(document);
			HtmlNode videoNode = htmlDocument.DocumentNode.SelectSingleNode(VideoXPath);
			if (videoNode == null) {
				document = await chromiumTwo.GetSourceAsync();
				htmlDocument = new HtmlAgilityPack.HtmlDocument();
				htmlDocument.LoadHtml(document);
				videoNode = htmlDocument.DocumentNode.SelectSingleNode(AltVideoXPath);
				if (videoNode == null) {
					Log("Download failed");
					downloadInProgress = false;
					return;
				}
			}
			string videoDownloadLink = videoNode.Attributes["src"].Value;
			WebClient client = new WebClient();
			string videoName = (numberOfVideosDownloaded + 1).ToString();
			try {
				client.DownloadFile(new Uri(videoDownloadLink),
					GetCurrentDownloadPath() + "\\" + videoName + ".mp4");
				numberOfVideosDownloaded++;
			}
			catch (Exception e) {
				client.Dispose();
			}
			downloadInProgress = false;
			Log("Finished downloading video");
		}

		private async void DownloadCurrentImage() {
			Log("Downloading image");
			string document = await chromiumTwo.GetSourceAsync();
			HtmlAgilityPack.HtmlDocument htmlDocument = new HtmlAgilityPack.HtmlDocument();
			htmlDocument.LoadHtml(document);
			HtmlNode imageNode = htmlDocument.DocumentNode.SelectSingleNode(ImageXPath);
			if (imageNode == null) {
				imageNode = htmlDocument.DocumentNode.SelectSingleNode(AltImageXPath);
				if (imageNode == null) {
					Log("Image download failed");
					downloadInProgress = false;
					return;
				}
			}
			string imageDownloadLink = imageNode.Attributes["src"].Value;
			WebClient client = new WebClient();
			string imageName = (numberOfImagesDownloaded + 1).ToString();
			try {
				client.DownloadFile(new Uri(imageDownloadLink),
					GetCurrentDownloadPath() + "\\" + imageName + ".jpg");
				numberOfImagesDownloaded++;
			}
			catch (Exception e) {
				client.Dispose();
			}
			Thread.Sleep(500);
			downloadInProgress = false;
			Log("Finished downloading image");
		}

		private void Log(string message) {
			Console.WriteLine(message);
		}

		const string RootDivXPath   = "/html/body/span/section/main/div/div/article/div[1]/div/div/div";
		const string VideoXPath     = "/html/body/span/section/main/div/div/article/div[1]/div/div/div/div[1]/div/div/video";
		const string AltVideoXPath  = "/html/body/span/section/main/div/div/article/div[1]/div/div/div/div[1]/div/video";
		const string ImageXPath     = "/html/body/span/section/main/div/div/article/div[1]/div/div/div[1]/div[1]/img";
		const string AltImageXPath  = "/html/body/span/section/main/div/div/article/div[1]/div/div/div/div[2]/div/div/div/ul/li[1]/div/div/div/div[1]/img";
		const string AriaLabelImage = "Carousel";

		private async void Test() {
			Log("Current TStamp: " + GetUnixTimestamp().ToString());
			Thread.Sleep(1000);
			Log("Current TStamp: " + GetUnixTimestamp().ToString());
		}

		private void RemoveSuggestionsDivFromProfilePage() {
			chromium.GetMainFrame().ExecuteJavaScriptAsync("suggestionsDiv = document.getElementsByClassName('_4bSq7')[0]; if (suggestionsDiv !== undefined) { suggestionsDiv.parentNode.removeChild(suggestionsDiv); }");
		}

		private string GetCurrentAnchorXPath() {
			string xPath = String.Format("/html/body/span/section/main/div/div/article/div[1]/div/div[{0}]/div[{1}]/a", depth.X + 1, (depth.Y % 3) + 1);
			return xPath;
		}

		private void UpdateDepth() {
			depthCounter++;
			if (depthCounter == 3) {
				depthCounter = 0;
				depth.X++;
			}
			depth.Y++;
		}

		private string GetCurrentVideoSpanXPath() {
			string xPath = GetCurrentAnchorXPath() + "/div[2]/span";
			return xPath;
		}

		private bool IsValidHtmlDocument(HtmlAgilityPack.HtmlDocument htmlDocument) {
			HtmlNodeCollection htmlNodes = htmlDocument.DocumentNode.SelectNodes(RootDivXPath);
			bool isValidHtmlDocument = htmlNodes != null;
			return isValidHtmlDocument;
		}

		private bool DownloadedAllMediaOnPage() {
			if (ReachedEndOfDocument() || (profilePageScrapeStartTime + TimeToSearchForVideos < GetUnixTimestamp()))
				return true;
			if (downloadMode == DownloadMode.Videos)
				return numberOfVideosDownloaded >= maxVideosToDownload;
			if (downloadMode == DownloadMode.Images)
				return numberOfImagesDownloaded >= maxImagesToDownload;
			return (numberOfVideosDownloaded >= maxVideosToDownload && numberOfImagesDownloaded >= maxImagesToDownload);
		}

		private void Stop() {
			scraping = false;
			button2.Text = "Scrape";
			textBox3.Text = "";
		}

		private void ScrollToBottom() {
			chromium.GetMainFrame().ExecuteJavaScriptAsync("window.scrollTo(0, document.body.scrollHeight);");
		}

		private bool ReachedEndOfDocument() {
			if (sameDocumentCounter == 2) {
				sameDocumentCounter = 0;
				return true;
			}
			return false;
		}

		private bool ShouldRunFrameLoadHandler(FrameLoadEndEventArgs e) {
			if (!e.Frame.IsMain)
				return false;
			if (!scraping)
				return false;
			return true;
		}

		private bool ShouldRunFrameLoadHandlerTwo(FrameLoadEndEventArgs e) {
			if (!e.Frame.IsMain)
				return false;
			if (!scraping)
				return false;
			return true;
		}

		private bool HasDocumentChanged(string document) {
			if (lastDocument == null) {
				lastDocument = document;
				sameDocumentCounter = 0;
				return true;
			}
			else if (document == lastDocument) {
				sameDocumentCounter = 0;
				return false;
			}
			sameDocumentCounter++;
			lastDocument = document;
			return true;
		}

		// Open folder browser
		private void Button1_Click(object sender, EventArgs e) {
			hasSelectedFolder = true;
			FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
			DialogResult dialogResult = folderBrowser.ShowDialog();
			if (dialogResult == DialogResult.OK) {
				textBox1.Text = folderBrowser.SelectedPath;
			}
		}

		// Start scraping
		private void Button2_Click(object sender, EventArgs e) {
			if (!hasSelectedFolder)
				return;
			if (scraping) {
				Stop();
				return;
			}
			if (!HasInputtedValidNumberOfMediaToDownload()) {
				MessageBox.Show("Please enter a valid number of media to download.");
				return;
			}
			Start();
		}

		private bool HasInputtedValidNumberOfMediaToDownload() {
			maxVideosToDownload = 0;
			maxImagesToDownload = 0;
			if (GetSelectedDownloadMode() == DownloadMode.Videos) {
				Int32.TryParse(textBox4.Text, out maxVideosToDownload);
				return maxVideosToDownload >= 1;
			}
			else if (GetSelectedDownloadMode() == DownloadMode.Images) {
				Int32.TryParse(textBox5.Text, out maxImagesToDownload);
				return maxImagesToDownload >= 1;
			}
			else {
				Int32.TryParse(textBox4.Text, out maxVideosToDownload);
				Int32.TryParse(textBox5.Text, out maxImagesToDownload);
				return maxVideosToDownload >= 1 && maxImagesToDownload >= 1;
			}
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
			Cef.Shutdown();
		}

		private void Button3_Click_1(object sender, EventArgs e) {
			Test();
		}

		private enum DownloadMode {
			Videos,
			Images,
			VideosAndImages
		}

		private void Label4_Click(object sender, EventArgs e) {

		}

		private void TextBox4_TextChanged(object sender, EventArgs e) {

		}

		private void Label5_Click(object sender, EventArgs e) {

		}

		private void TextBox5_TextChanged(object sender, EventArgs e) {

		}
	}
}
