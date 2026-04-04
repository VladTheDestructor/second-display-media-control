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
            toolStrip1 = new ToolStrip();
            playButton = new ToolStripButton();
            pauseButton = new ToolStripButton();
            stopButton = new ToolStripButton();
            autoplayButton = new ToolStripButton();
            secondScreenButton = new ToolStripButton();
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
            videoPanel = new Panel();
            label1 = new Label();
            volumeTrackBar = new TrackBar();
            volumeLabel = new Label();
            selectBackgroundImageToolStripMenuItem = new ToolStripMenuItem();
            removeBackgroundImageToolStripMenuItem = new ToolStripMenuItem();
            toolStrip1.SuspendLayout();
            menuStrip1.SuspendLayout();
            FileFromListContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)volumeTrackBar).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.Dock = DockStyle.None;
            toolStrip1.Items.AddRange(new ToolStripItem[] { playButton, pauseButton, stopButton, autoplayButton, secondScreenButton });
            toolStrip1.Location = new Point(743, 321);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(127, 25);
            toolStrip1.TabIndex = 2;
            toolStrip1.Text = "toolStrip1";
            // 
            // playButton
            // 
            playButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            playButton.Image = Properties.Resources.play_buttton;
            playButton.ImageTransparentColor = Color.Magenta;
            playButton.Name = "playButton";
            playButton.Size = new Size(23, 22);
            playButton.Text = "toolStripButton1";
            playButton.Click += playButton_Click;
            // 
            // pauseButton
            // 
            pauseButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            pauseButton.Image = Properties.Resources.pause;
            pauseButton.ImageTransparentColor = Color.Magenta;
            pauseButton.Name = "pauseButton";
            pauseButton.Size = new Size(23, 22);
            pauseButton.Text = "toolStripButton2";
            pauseButton.Click += pauseButton_Click;
            // 
            // stopButton
            // 
            stopButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            stopButton.Image = Properties.Resources.stop_button;
            stopButton.ImageTransparentColor = Color.Magenta;
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(23, 22);
            stopButton.Text = "toolStripButton3";
            stopButton.Click += stopButton_Click;
            // 
            // autoplayButton
            // 
            autoplayButton.CheckOnClick = true;
            autoplayButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            autoplayButton.Image = Properties.Resources.arrowhead;
            autoplayButton.ImageTransparentColor = Color.Magenta;
            autoplayButton.Name = "autoplayButton";
            autoplayButton.Size = new Size(23, 22);
            autoplayButton.Text = "toolStripButton1";
            autoplayButton.Click += autoplayButton_Click;
            // 
            // secondScreenButton
            // 
            secondScreenButton.CheckOnClick = true;
            secondScreenButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
            secondScreenButton.Image = Properties.Resources.monitor;
            secondScreenButton.ImageTransparentColor = Color.Magenta;
            secondScreenButton.Name = "secondScreenButton";
            secondScreenButton.Size = new Size(23, 22);
            secondScreenButton.Text = "toolStripButton1";
            secondScreenButton.Click += secondScreenButton_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, viewToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1119, 24);
            menuStrip1.TabIndex = 3;
            menuStrip1.Text = "menuStrip1";
            menuStrip1.ItemClicked += menuStrip1_ItemClicked;
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newProjectToolStripMenuItem, openProjectToolStripMenuItem, saveProjectToolStripMenuItem, importMediaToolStripMenuItem, importFolderToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(48, 20);
            fileToolStripMenuItem.Text = "Файл";
            fileToolStripMenuItem.Click += fileToolStripMenuItem_Click;
            // 
            // newProjectToolStripMenuItem
            // 
            newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            newProjectToolStripMenuItem.Size = new Size(174, 22);
            newProjectToolStripMenuItem.Text = "Новый проект";
            newProjectToolStripMenuItem.Click += newProjectToolStripMenuItem_Click;
            // 
            // openProjectToolStripMenuItem
            // 
            openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            openProjectToolStripMenuItem.Size = new Size(174, 22);
            openProjectToolStripMenuItem.Text = "Открыть проект";
            openProjectToolStripMenuItem.Click += openProjectToolStripMenuItem_Click;
            // 
            // saveProjectToolStripMenuItem
            // 
            saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            saveProjectToolStripMenuItem.Size = new Size(174, 22);
            saveProjectToolStripMenuItem.Text = "Сохранить проект";
            saveProjectToolStripMenuItem.Click += saveProjectToolStripMenuItem_Click;
            // 
            // importMediaToolStripMenuItem
            // 
            importMediaToolStripMenuItem.Name = "importMediaToolStripMenuItem";
            importMediaToolStripMenuItem.Size = new Size(174, 22);
            importMediaToolStripMenuItem.Text = "Импорт медиа";
            importMediaToolStripMenuItem.Click += importMediaToolStripMenuItem_Click;
            // 
            // importFolderToolStripMenuItem
            // 
            importFolderToolStripMenuItem.Name = "importFolderToolStripMenuItem";
            importFolderToolStripMenuItem.Size = new Size(174, 22);
            importFolderToolStripMenuItem.Text = "Импорт папки";
            // 
            // viewToolStripMenuItem
            // 
            viewToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { secondDisplayWidowtempToolStripMenuItem });
            viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            viewToolStripMenuItem.Size = new Size(39, 20);
            viewToolStripMenuItem.Text = "Вид";
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
            listView1.Dock = DockStyle.Left;
            listView1.Location = new Point(0, 24);
            listView1.Name = "listView1";
            listView1.Size = new Size(496, 611);
            listView1.TabIndex = 4;
            listView1.UseCompatibleStateImageBehavior = false;
            listView1.MouseDoubleClick += ListView1_MouseDoubleClick;
            // 
            // imageList1
            // 
            imageList1.ColorDepth = ColorDepth.Depth32Bit;
            imageList1.ImageSize = new Size(32, 32);
            imageList1.TransparentColor = Color.Transparent;
            // 
            // FileFromListContextMenu
            // 
            FileFromListContextMenu.Items.AddRange(new ToolStripItem[] { moveUpToolStripMenuItem, moveDownToolStripMenuItem, selectBackgroundImageToolStripMenuItem, removeBackgroundImageToolStripMenuItem, removeFromPlaylistToolStripMenuItem });
            FileFromListContextMenu.Name = "contextMenuStrip1";
            FileFromListContextMenu.Size = new Size(256, 136);
            // 
            // moveUpToolStripMenuItem
            // 
            moveUpToolStripMenuItem.Name = "moveUpToolStripMenuItem";
            moveUpToolStripMenuItem.Size = new Size(255, 22);
            moveUpToolStripMenuItem.Text = "переместить вверх";
            moveUpToolStripMenuItem.Click += moveUpToolStripMenuItem_Click;
            // 
            // moveDownToolStripMenuItem
            // 
            moveDownToolStripMenuItem.Name = "moveDownToolStripMenuItem";
            moveDownToolStripMenuItem.Size = new Size(255, 22);
            moveDownToolStripMenuItem.Text = "переместить вниз";
            moveDownToolStripMenuItem.Click += moveDownToolStripMenuItem_Click;
            // 
            // removeFromPlaylistToolStripMenuItem
            // 
            removeFromPlaylistToolStripMenuItem.Name = "removeFromPlaylistToolStripMenuItem";
            removeFromPlaylistToolStripMenuItem.Size = new Size(255, 22);
            removeFromPlaylistToolStripMenuItem.Text = "УДАЛИТЬ из плейлиста";
            removeFromPlaylistToolStripMenuItem.Click += removeFromPlaylistToolStripMenuItem_Click;
            // 
            // videoPanel
            // 
            videoPanel.Dock = DockStyle.Top;
            videoPanel.Location = new Point(496, 24);
            videoPanel.Name = "videoPanel";
            videoPanel.Size = new Size(623, 294);
            videoPanel.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(916, 374);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 6;
            label1.Text = "Громкость";
            // 
            // volumeTrackBar
            // 
            volumeTrackBar.Location = new Point(984, 370);
            volumeTrackBar.Maximum = 100;
            volumeTrackBar.Name = "volumeTrackBar";
            volumeTrackBar.Size = new Size(104, 45);
            volumeTrackBar.TabIndex = 7;
            volumeTrackBar.Scroll += volumeTrackBar_Scroll;
            // 
            // volumeLabel
            // 
            volumeLabel.AutoSize = true;
            volumeLabel.Location = new Point(1078, 372);
            volumeLabel.Name = "volumeLabel";
            volumeLabel.Size = new Size(13, 15);
            volumeLabel.TabIndex = 8;
            volumeLabel.Text = "0";
            // 
            // selectBackgroundImageToolStripMenuItem
            // 
            selectBackgroundImageToolStripMenuItem.Name = "selectBackgroundImageToolStripMenuItem";
            selectBackgroundImageToolStripMenuItem.Size = new Size(255, 22);
            selectBackgroundImageToolStripMenuItem.Text = "Добавить фоновое изображение";
            // 
            // removeBackgroundImageToolStripMenuItem
            // 
            removeBackgroundImageToolStripMenuItem.Name = "removeBackgroundImageToolStripMenuItem";
            removeBackgroundImageToolStripMenuItem.Size = new Size(255, 22);
            removeBackgroundImageToolStripMenuItem.Text = "Удалить изображение";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1119, 635);
            Controls.Add(volumeLabel);
            Controls.Add(volumeTrackBar);
            Controls.Add(label1);
            Controls.Add(videoPanel);
            Controls.Add(listView1);
            Controls.Add(toolStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainWindow";
            Text = "SDMC-Live";
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            FileFromListContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)volumeTrackBar).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private ToolStrip toolStrip1;
        private ToolStripButton playButton;
        private ToolStripButton pauseButton;
        private ToolStripButton stopButton;
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
        private Panel videoPanel;
        private ToolStripButton autoplayButton;
        private ToolStripButton secondScreenButton;
        private Label label1;
        private TrackBar volumeTrackBar;
        private Label volumeLabel;
        private ToolStripMenuItem selectBackgroundImageToolStripMenuItem;
        private ToolStripMenuItem removeBackgroundImageToolStripMenuItem;
    }
}
