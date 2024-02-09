namespace ImageResizer
{
	partial class Main
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.FolderBrowser = new System.Windows.Forms.FolderBrowserDialog();
			this.Folder = new System.Windows.Forms.TextBox();
			this.Browse = new System.Windows.Forms.Button();
			this.FolderWatcher = new System.IO.FileSystemWatcher();
			((System.ComponentModel.ISupportInitialize)(this.FolderWatcher)).BeginInit();
			this.SuspendLayout();
			// 
			// Folder
			// 
			this.Folder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.Folder.Location = new System.Drawing.Point(12, 12);
			this.Folder.Name = "Folder";
			this.Folder.Size = new System.Drawing.Size(282, 22);
			this.Folder.TabIndex = 0;
			this.Folder.Enter += new System.EventHandler(this.Folder_Enter);
			this.Folder.Leave += new System.EventHandler(this.Folder_Leave);
			// 
			// Browse
			// 
			this.Browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.Browse.Location = new System.Drawing.Point(300, 12);
			this.Browse.Name = "Browse";
			this.Browse.Size = new System.Drawing.Size(75, 23);
			this.Browse.TabIndex = 1;
			this.Browse.Text = "Browse...";
			this.Browse.UseVisualStyleBackColor = true;
			this.Browse.Click += new System.EventHandler(this.Browse_Click);
			// 
			// FolderWatcher
			// 
			this.FolderWatcher.EnableRaisingEvents = true;
			this.FolderWatcher.IncludeSubdirectories = true;
			this.FolderWatcher.SynchronizingObject = this;
			this.FolderWatcher.Changed += new System.IO.FileSystemEventHandler(this.FolderWatcher_Changed);
			this.FolderWatcher.Created += new System.IO.FileSystemEventHandler(this.FolderWatcher_Created);
			this.FolderWatcher.Renamed += new System.IO.RenamedEventHandler(this.FolderWatcher_Renamed);
			// 
			// Main
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(387, 95);
			this.Controls.Add(this.Browse);
			this.Controls.Add(this.Folder);
			this.Name = "Main";
			this.Text = "Image Resizer";
			((System.ComponentModel.ISupportInitialize)(this.FolderWatcher)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.FolderBrowserDialog FolderBrowser;
		private System.Windows.Forms.TextBox Folder;
		private System.Windows.Forms.Button Browse;
		private System.IO.FileSystemWatcher FolderWatcher;
	}
}

