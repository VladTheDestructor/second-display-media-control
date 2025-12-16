namespace second_display_media_control
{
    partial class MainWindow
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            PreviewBox = new PictureBox();
            toolStrip1 = new ToolStrip();
            toolStripButton1 = new ToolStripButton();
            toolStripButton2 = new ToolStripButton();
            toolStripButton3 = new ToolStripButton();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newProjectToolStripMenuItem = new ToolStripMenuItem();
            openProjectToolStripMenuItem = new ToolStripMenuItem();
            saveProjectToolStripMenuItem = new ToolStripMenuItem();
            importMediaToolStripMenuItem = new ToolStripMenuItem();
            importFolderToolStripMenuItem = new ToolStripMenuItem();
            viewToolStripMenuItem = new ToolStripMenuItem();
            secondDisplayWidowtempToolStripMenuItem = new ToolStripMenuItem();
            AddMediaDialog = new OpenFileDialog();
            listView1 = new ListView();
            imageList1 = new ImageList(components);
            FileFromListContextMenu = new ContextMenuStrip(components);
            moveUpToolStripMenuItem = new ToolStripMenuItem();
            moveDownToolStripMenuItem = new ToolStripMenuItem();
            removeFromPlaylistToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)PreviewBox).BeginInit();
            toolStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            FileFromListContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // PreviewBox
            // 
            PreviewBox.Location = new Point(630, 67);
            PreviewBox.Name = "PreviewBox";
            PreviewBox.Size = new Size(382, 230);
            PreviewBox.TabIndex = 0;
            PreviewBox.TabStop = false;
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = DockStyle.None;
            toolStrip1.Items.AddRange(new ToolStripItem[] { toolStripButton1, toolStripButton2, toolStripButton3 });
            toolStrip1.Location = new Point(769, 312);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(81, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton1.Image = (Image)resources.GetObject("toolStripButton1.Image");
            toolStripButton1.ImageTransparentColor = Color.Magenta;
            toolStripButton1.Name = "toolStripButton1";
            toolStripButton1.Size = new Size(23, 22);
            toolStripButton1.Text = "toolStripButton1";
            // 
            // toolStripButton2
            // 
            toolStripButton2.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton2.Image = (Image)resources.GetObject("toolStripButton2.Image");
            toolStripButton2.ImageTransparentColor = Color.Magenta;
            toolStripButton2.Name = "toolStripButton2";
            toolStripButton2.Size = new Size(23, 22);
            toolStripButton2.Text = "toolStripButton2";
            // 
            // toolStripButton3
            // 
            toolStripButton3.DisplayStyle = ToolStripItemDisplayStyle.Image;
            toolStripButton3.Image = (Image)resources.GetObject("toolStripButton3.Image");
            toolStripButton3.ImageTransparentColor = Color.Magenta;
            toolStripButton3.Name = "toolStripButton3";
            toolStripButton3.Size = new Size(23, 22);
            toolStripButton3.Text = "toolStripButton3";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1119, 24);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newProjectToolStripMenuItem, openProjectToolStripMenuItem, saveProjectToolStripMenuItem, importMediaToolStripMenuItem, importFolderToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newProjectToolStripMenuItem
            // 
            newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            newProjectToolStripMenuItem.Size = new Size(146, 22);
            newProjectToolStripMenuItem.Text = "New project";
            // 
            // openProjectToolStripMenuItem
            // 
            openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            openProjectToolStripMenuItem.Size = new Size(146, 22);
            openProjectToolStripMenuItem.Text = "Open project";
            // 
            // saveProjectToolStripMenuItem
            // 
            saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            saveProjectToolStripMenuItem.Size = new Size(146, 22);
            saveProjectToolStripMenuItem.Text = "Save project";
            // 
            // importMediaToolStripMenuItem
            // 
            importMediaToolStripMenuItem.Name = "importMediaToolStripMenuItem";
            importMediaToolStripMenuItem.Size = new Size(146, 22);
            importMediaToolStripMenuItem.Text = "Import media";
            importMediaToolStripMenuItem.Click += importMediaToolStripMenuItem_Click;
            // 
            // importFolderToolStripMenuItem
            // 
            importFolderToolStripMenuItem.Name = "importFolderToolStripMenuItem";
            importFolderToolStripMenuItem.Size = new Size(146, 22);
            importFolderToolStripMenuItem.Text = "Import folder";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { secondDisplayWidowtempToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(44, 20);
            viewToolStripMenuItem.Text = "View";
            // 
            // secondDisplayWidowtempToolStripMenuItem
            // 
            secondDisplayWidowtempToolStripMenuItem.Name = "secondDisplayWidowtempToolStripMenuItem";
            secondDisplayWidowtempToolStripMenuItem.Size = new Size(227, 22);
            secondDisplayWidowtempToolStripMenuItem.Text = "Second display widow(temp)";
            // 
            // AddMediaDialog
            // 
            AddMediaDialog.FileName = "AddMediaDialog";
            // 
            // listView1
            // 
            listView1.Location = new Point(47, 63);
            listView1.Name = "listView1";
            listView1.Size = new Size(247, 489);
            listView1.TabIndex = 4;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(32, 32);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // FileFromListContextMenu
            // 
            FileFromListContextMenu.Items.AddRange(new ToolStripItem[] { moveUpToolStripMenuItem, moveDownToolStripMenuItem, removeFromPlaylistToolStripMenuItem });
            FileFromListContextMenu.Name = "contextMenuStrip1";
            FileFromListContextMenu.Size = new Size(184, 70);
            // 
            // moveUpToolStripMenuItem
            // 
            moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            moveUpToolStripMenuItem.Size = new Size(183, 22);
            moveUpToolStripMenuItem.Text = "move up";
            // 
            // moveDownToolStripMenuItem
            // 
            moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            moveDownToolStripMenuItem.Size = new Size(183, 22);
            moveDownToolStripMenuItem.Text = "move down";
            // 
            // removeFromPlaylistToolStripMenuItem
            // 
            removeFromPlaylistToolStripMenuItem.Name = "removeFromPlaylistToolStripMenuItem";
            removeFromPlaylistToolStripMenuItem.Size = new Size(183, 22);
            removeFromPlaylistToolStripMenuItem.Text = "remove from playlist";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1119, 635);
            Controls.Add(listView1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            Controls.Add(PreviewBox);
            MainMenuStrip = menuStrip1;
            Name = "MainWindow";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)PreviewBox).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            FileFromListContextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox PreviewBox;
        private ToolStrip toolStrip1;
        private ToolStripButton toolStripButton1;
        private ToolStripButton toolStripButton2;
        private ToolStripButton toolStripButton3;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem newProjectToolStripMenuItem;
        private ToolStripMenuItem openProjectToolStripMenuItem;
        private ToolStripMenuItem saveProjectToolStripMenuItem;
        private ToolStripMenuItem importMediaToolStripMenuItem;
        private ToolStripMenuItem importFolderToolStripMenuItem;
        private ToolStripMenuItem viewToolStripMenuItem;
        private ToolStripMenuItem secondDisplayWidowtempToolStripMenuItem;
        private OpenFileDialog AddMediaDialog;
        private ListView listView1;
        private ImageList imageList1;
        private ContextMenuStrip FileFromListContextMenu;
        private ToolStripMenuItem moveUpToolStripMenuItem;
        private ToolStripMenuItem moveDownToolStripMenuItem;
        private ToolStripMenuItem removeFromPlaylistToolStripMenuItem;
    }
}
