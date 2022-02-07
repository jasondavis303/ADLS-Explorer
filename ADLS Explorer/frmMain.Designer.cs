namespace ADLS_Explorer
{
    partial class frmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbCurrentContainer = new System.Windows.Forms.ToolStripComboBox();
            this.btnSettings = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tvFolders = new System.Windows.Forms.TreeView();
            this.cmsFolders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRefreshFolders = new System.Windows.Forms.ToolStripMenuItem();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.lvFiles = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader6 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader5 = new System.Windows.Forms.ColumnHeader();
            this.cmsFiles = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.mnuRefreshFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuUpload = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUploadFiles = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuUploadFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuDownload = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuNewFolder = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuMove = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRename = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.dlgLocalFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.dlgFiles = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.cmsFolders.SuspendLayout();
            this.cmsFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cbCurrentContainer,
            this.btnSettings});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(8, 4, 1, 6);
            this.toolStrip1.Size = new System.Drawing.Size(989, 33);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(62, 20);
            this.toolStripLabel1.Text = "Container:";
            // 
            // cbCurrentContainer
            // 
            this.cbCurrentContainer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrentContainer.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cbCurrentContainer.Margin = new System.Windows.Forms.Padding(8, 0, 1, 0);
            this.cbCurrentContainer.Name = "cbCurrentContainer";
            this.cbCurrentContainer.Size = new System.Drawing.Size(260, 23);
            this.cbCurrentContainer.SelectedIndexChanged += new System.EventHandler(this.cbCurrentContainer_SelectedIndexChanged);
            // 
            // btnSettings
            // 
            this.btnSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSettings.Image = ((System.Drawing.Image)(resources.GetObject("btnSettings.Image")));
            this.btnSettings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSettings.Margin = new System.Windows.Forms.Padding(8, 1, 0, 2);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(23, 20);
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 627);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(989, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(974, 17);
            this.lblStatus.Spring = true;
            this.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 33);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tvFolders);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.lvFiles);
            this.splitContainer1.Size = new System.Drawing.Size(989, 594);
            this.splitContainer1.SplitterDistance = 336;
            this.splitContainer1.TabIndex = 2;
            // 
            // tvFolders
            // 
            this.tvFolders.ContextMenuStrip = this.cmsFolders;
            this.tvFolders.Cursor = System.Windows.Forms.Cursors.Default;
            this.tvFolders.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvFolders.HideSelection = false;
            this.tvFolders.ImageIndex = 0;
            this.tvFolders.ImageList = this.ilIcons;
            this.tvFolders.Location = new System.Drawing.Point(0, 0);
            this.tvFolders.Name = "tvFolders";
            this.tvFolders.PathSeparator = "/";
            this.tvFolders.SelectedImageIndex = 0;
            this.tvFolders.ShowRootLines = false;
            this.tvFolders.Size = new System.Drawing.Size(336, 594);
            this.tvFolders.TabIndex = 0;
            this.tvFolders.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvFolders_AfterSelect);
            // 
            // cmsFolders
            // 
            this.cmsFolders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRefreshFolders});
            this.cmsFolders.Name = "cmsFolders";
            this.cmsFolders.Size = new System.Drawing.Size(114, 26);
            // 
            // mnuRefreshFolders
            // 
            this.mnuRefreshFolders.Name = "mnuRefreshFolders";
            this.mnuRefreshFolders.Size = new System.Drawing.Size(113, 22);
            this.mnuRefreshFolders.Text = "Refresh";
            this.mnuRefreshFolders.Click += new System.EventHandler(this.mnuRefreshFolders_Click);
            // 
            // ilIcons
            // 
            this.ilIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ilIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.ilIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lvFiles
            // 
            this.lvFiles.AllowDrop = true;
            this.lvFiles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader4,
            this.columnHeader6,
            this.columnHeader5});
            this.lvFiles.ContextMenuStrip = this.cmsFiles;
            this.lvFiles.Cursor = System.Windows.Forms.Cursors.Default;
            this.lvFiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvFiles.Location = new System.Drawing.Point(0, 0);
            this.lvFiles.Name = "lvFiles";
            this.lvFiles.Size = new System.Drawing.Size(649, 594);
            this.lvFiles.SmallImageList = this.ilIcons;
            this.lvFiles.TabIndex = 0;
            this.lvFiles.UseCompatibleStateImageBehavior = false;
            this.lvFiles.View = System.Windows.Forms.View.Details;
            this.lvFiles.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.lvFiles_ItemDrag);
            this.lvFiles.DragDrop += new System.Windows.Forms.DragEventHandler(this.lvFiles_DragDrop);
            this.lvFiles.DragOver += new System.Windows.Forms.DragEventHandler(this.lvFiles_DragOver);
            this.lvFiles.DoubleClick += new System.EventHandler(this.lvFiles_DoubleClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 240;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 100;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Size";
            this.columnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.columnHeader3.Width = 80;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Modified";
            this.columnHeader4.Width = 200;
            // 
            // columnHeader6
            // 
            this.columnHeader6.DisplayIndex = 5;
            this.columnHeader6.Text = "Permissions";
            // 
            // columnHeader5
            // 
            this.columnHeader5.DisplayIndex = 4;
            this.columnHeader5.Text = "Owner";
            // 
            // cmsFiles
            // 
            this.cmsFiles.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRefreshFiles,
            this.toolStripMenuItem3,
            this.mnuUpload,
            this.mnuDownload,
            this.toolStripMenuItem1,
            this.mnuNewFolder,
            this.mnuMove,
            this.mnuRename,
            this.toolStripMenuItem2,
            this.mnuDelete});
            this.cmsFiles.Name = "cmsFiles";
            this.cmsFiles.Size = new System.Drawing.Size(135, 176);
            this.cmsFiles.Opening += new System.ComponentModel.CancelEventHandler(this.cmsFiles_Opening);
            // 
            // mnuRefreshFiles
            // 
            this.mnuRefreshFiles.Name = "mnuRefreshFiles";
            this.mnuRefreshFiles.Size = new System.Drawing.Size(134, 22);
            this.mnuRefreshFiles.Text = "Refresh";
            this.mnuRefreshFiles.Click += new System.EventHandler(this.mnuRefreshFiles_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(131, 6);
            // 
            // mnuUpload
            // 
            this.mnuUpload.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUploadFiles,
            this.mnuUploadFolder});
            this.mnuUpload.Name = "mnuUpload";
            this.mnuUpload.Size = new System.Drawing.Size(134, 22);
            this.mnuUpload.Text = "Upload";
            // 
            // mnuUploadFiles
            // 
            this.mnuUploadFiles.Name = "mnuUploadFiles";
            this.mnuUploadFiles.Size = new System.Drawing.Size(107, 22);
            this.mnuUploadFiles.Text = "Files";
            this.mnuUploadFiles.Click += new System.EventHandler(this.mnuUploadFiles_Click);
            // 
            // mnuUploadFolder
            // 
            this.mnuUploadFolder.Name = "mnuUploadFolder";
            this.mnuUploadFolder.Size = new System.Drawing.Size(107, 22);
            this.mnuUploadFolder.Text = "Folder";
            this.mnuUploadFolder.Click += new System.EventHandler(this.mnuUploadFolder_Click_1);
            // 
            // mnuDownload
            // 
            this.mnuDownload.Name = "mnuDownload";
            this.mnuDownload.Size = new System.Drawing.Size(134, 22);
            this.mnuDownload.Text = "Download";
            this.mnuDownload.Click += new System.EventHandler(this.mnuDownload_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(131, 6);
            // 
            // mnuNewFolder
            // 
            this.mnuNewFolder.Name = "mnuNewFolder";
            this.mnuNewFolder.Size = new System.Drawing.Size(134, 22);
            this.mnuNewFolder.Text = "New Folder";
            this.mnuNewFolder.Click += new System.EventHandler(this.mnuNewFolder_Click);
            // 
            // mnuMove
            // 
            this.mnuMove.Name = "mnuMove";
            this.mnuMove.Size = new System.Drawing.Size(134, 22);
            this.mnuMove.Text = "Move";
            this.mnuMove.Click += new System.EventHandler(this.mnuMove_Click);
            // 
            // mnuRename
            // 
            this.mnuRename.Name = "mnuRename";
            this.mnuRename.Size = new System.Drawing.Size(134, 22);
            this.mnuRename.Text = "Rename";
            this.mnuRename.Click += new System.EventHandler(this.mnuRename_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(131, 6);
            // 
            // mnuDelete
            // 
            this.mnuDelete.Name = "mnuDelete";
            this.mnuDelete.Size = new System.Drawing.Size(134, 22);
            this.mnuDelete.Text = "Delete";
            this.mnuDelete.Click += new System.EventHandler(this.mnuDelete_Click);
            // 
            // dlgLocalFolder
            // 
            this.dlgLocalFolder.AutoUpgradeEnabled = false;
            // 
            // dlgFiles
            // 
            this.dlgFiles.Multiselect = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(989, 649);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "frmMain";
            this.Text = "ADLS Explorer";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.cmsFolders.ResumeLayout(false);
            this.cmsFiles.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripComboBox cbCurrentContainer;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton btnSettings;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView tvFolders;
        private System.Windows.Forms.ListView lvFiles;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ColumnHeader columnHeader5;
        private System.Windows.Forms.ColumnHeader columnHeader6;
        private System.Windows.Forms.ContextMenuStrip cmsFiles;
        private System.Windows.Forms.ToolStripMenuItem mnuDownload;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem mnuMove;
        private System.Windows.Forms.ToolStripMenuItem mnuRename;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem mnuDelete;
        private System.Windows.Forms.ToolStripStatusLabel lblStatus;
        private System.Windows.Forms.ContextMenuStrip cmsFolders;
        private System.Windows.Forms.ToolStripMenuItem mnuRefreshFolders;
        private System.Windows.Forms.ToolStripMenuItem mnuRefreshFiles;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem mnuNewFolder;
        private System.Windows.Forms.FolderBrowserDialog dlgLocalFolder;
        private System.Windows.Forms.ToolStripMenuItem mnuUpload;
        private System.Windows.Forms.ToolStripMenuItem mnuUploadFiles;
        private System.Windows.Forms.ToolStripMenuItem mnuUploadFolder;
        private System.Windows.Forms.OpenFileDialog dlgFiles;
    }
}