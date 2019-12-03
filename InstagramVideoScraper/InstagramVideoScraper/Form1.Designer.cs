namespace InstagramVideoScraper {
	partial class Form1 {
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing) {
			if (disposing && (components != null)) {
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent() {
			this.label1 = new System.Windows.Forms.Label();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.button2 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.textBox3 = new System.Windows.Forms.TextBox();
			this.directorySearcher1 = new System.DirectoryServices.DirectorySearcher();
			this.label4 = new System.Windows.Forms.Label();
			this.textBox4 = new System.Windows.Forms.TextBox();
			this.textBox5 = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.radioButtonVideos = new System.Windows.Forms.RadioButton();
			this.radioButtonImages = new System.Windows.Forms.RadioButton();
			this.radioButtonVideosImages = new System.Windows.Forms.RadioButton();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(83, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Save Videos To";
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(15, 26);
			this.textBox1.Name = "textBox1";
			this.textBox1.ReadOnly = true;
			this.textBox1.Size = new System.Drawing.Size(303, 20);
			this.textBox1.TabIndex = 1;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(324, 24);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 2;
			this.button1.Text = "Browse";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1_Click);
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(15, 153);
			this.textBox2.Multiline = true;
			this.textBox2.Name = "textBox2";
			this.textBox2.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textBox2.Size = new System.Drawing.Size(303, 120);
			this.textBox2.TabIndex = 3;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 137);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(32, 13);
			this.label2.TabIndex = 4;
			this.label2.Text = "Links";
			// 
			// button2
			// 
			this.button2.Location = new System.Drawing.Point(324, 250);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(75, 23);
			this.button2.TabIndex = 5;
			this.button2.Text = "Scrape";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new System.EventHandler(this.Button2_Click);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 276);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(64, 13);
			this.label3.TabIndex = 6;
			this.label3.Text = "Current Link";
			// 
			// textBox3
			// 
			this.textBox3.Location = new System.Drawing.Point(15, 292);
			this.textBox3.Name = "textBox3";
			this.textBox3.ReadOnly = true;
			this.textBox3.Size = new System.Drawing.Size(303, 20);
			this.textBox3.TabIndex = 7;
			// 
			// directorySearcher1
			// 
			this.directorySearcher1.ClientTimeout = System.TimeSpan.Parse("-00:00:01");
			this.directorySearcher1.ServerPageTimeLimit = System.TimeSpan.Parse("-00:00:01");
			this.directorySearcher1.ServerTimeLimit = System.TimeSpan.Parse("-00:00:01");
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(12, 94);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(39, 13);
			this.label4.TabIndex = 8;
			this.label4.Text = "Videos";
			this.label4.Click += new System.EventHandler(this.Label4_Click);
			// 
			// textBox4
			// 
			this.textBox4.Location = new System.Drawing.Point(15, 110);
			this.textBox4.Name = "textBox4";
			this.textBox4.Size = new System.Drawing.Size(150, 20);
			this.textBox4.TabIndex = 9;
			this.textBox4.Text = "3";
			this.textBox4.TextChanged += new System.EventHandler(this.TextBox4_TextChanged);
			// 
			// textBox5
			// 
			this.textBox5.Location = new System.Drawing.Point(168, 110);
			this.textBox5.Name = "textBox5";
			this.textBox5.Size = new System.Drawing.Size(150, 20);
			this.textBox5.TabIndex = 11;
			this.textBox5.Text = "3";
			this.textBox5.TextChanged += new System.EventHandler(this.TextBox5_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(165, 94);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(41, 13);
			this.label5.TabIndex = 10;
			this.label5.Text = "Images";
			this.label5.Click += new System.EventHandler(this.Label5_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 49);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(55, 13);
			this.label6.TabIndex = 12;
			this.label6.Text = "Download";
			// 
			// radioButtonVideos
			// 
			this.radioButtonVideos.AutoSize = true;
			this.radioButtonVideos.Checked = true;
			this.radioButtonVideos.Location = new System.Drawing.Point(15, 69);
			this.radioButtonVideos.Name = "radioButtonVideos";
			this.radioButtonVideos.Size = new System.Drawing.Size(57, 17);
			this.radioButtonVideos.TabIndex = 13;
			this.radioButtonVideos.TabStop = true;
			this.radioButtonVideos.Text = "Videos";
			this.radioButtonVideos.UseVisualStyleBackColor = true;
			// 
			// radioButtonImages
			// 
			this.radioButtonImages.AutoSize = true;
			this.radioButtonImages.Location = new System.Drawing.Point(78, 69);
			this.radioButtonImages.Name = "radioButtonImages";
			this.radioButtonImages.Size = new System.Drawing.Size(59, 17);
			this.radioButtonImages.TabIndex = 14;
			this.radioButtonImages.Text = "Images";
			this.radioButtonImages.UseVisualStyleBackColor = true;
			// 
			// radioButtonVideosImages
			// 
			this.radioButtonVideosImages.AutoSize = true;
			this.radioButtonVideosImages.Location = new System.Drawing.Point(143, 69);
			this.radioButtonVideosImages.Name = "radioButtonVideosImages";
			this.radioButtonVideosImages.Size = new System.Drawing.Size(116, 17);
			this.radioButtonVideosImages.TabIndex = 15;
			this.radioButtonVideosImages.Text = "Videos And Images";
			this.radioButtonVideosImages.UseVisualStyleBackColor = true;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(409, 319);
			this.Controls.Add(this.radioButtonVideosImages);
			this.Controls.Add(this.radioButtonImages);
			this.Controls.Add(this.radioButtonVideos);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.textBox5);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.textBox4);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.textBox3);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.button2);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Instagram Video Scraper";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
			this.Load += new System.EventHandler(this.Form1_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textBox1;
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button button2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox textBox3;
		private System.DirectoryServices.DirectorySearcher directorySearcher1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox textBox4;
		private System.Windows.Forms.TextBox textBox5;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RadioButton radioButtonVideos;
		private System.Windows.Forms.RadioButton radioButtonImages;
		private System.Windows.Forms.RadioButton radioButtonVideosImages;
	}
}

