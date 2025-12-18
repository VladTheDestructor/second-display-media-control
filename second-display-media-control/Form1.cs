using System;
using System.IO;
using System.Windows.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;

namespace second_display_media_control
{
    public partial class MainWindow : Form
    {
        private VlcControl vlcPlayer;

        public MainWindow()
        {
            InitializeComponent();
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            listView1.AllowDrop = true;
            listView1.MultiSelect = true;
            listView1.Columns.Add("Preview", 60);
            listView1.Columns.Add("Name", 200);
            listView1.Columns.Add("Path", 400);
            listView1.SmallImageList = imageList1;
            listView1.MouseDoubleClick += ListView1_MouseDoubleClick;
            InitializeVlcPlayer();
            InitializeButtons();
        }

        private void InitializeVlcPlayer()
        {
            try
            {
                string vlcPath = FindVlcPath();
                if (!Directory.Exists(vlcPath) || !File.Exists(Path.Combine(vlcPath, "libvlc.dll")))
                {
                    MessageBox.Show($"Библиотеки VLC не найдены по пути:\n{vlcPath}\n\n1. Установите VLC Player с https://www.videolan.org/vlc/\n2. Или укажите правильный путь к папке с VLC.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                vlcPlayer = new VlcControl();
                vlcPlayer.Dock = DockStyle.Fill;
                vlcPlayer.BeginInit();
                vlcPlayer.VlcLibDirectory = new DirectoryInfo(vlcPath);
                vlcPlayer.EndInit();

                if (videoPanel != null)
                {
                    videoPanel.Controls.Add(vlcPlayer);
                }
                else
                {
                    Panel panel = new Panel();
                    panel.Dock = DockStyle.Fill;
                    panel.Name = "videoPanel";
                    this.Controls.Add(panel);
                    panel.Controls.Add(vlcPlayer);
                    panel.BringToFront();
                }

                MessageBox.Show($"VLC успешно инициализирован!\nПуть: {vlcPath}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка VLC: {ex.Message}\n\nУбедитесь, что установлен VLC Player или пакет Vlc.DotNet.WinForms.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FindVlcPath()
        {
            string[] searchPaths =
            {
                Application.StartupPath,
                Path.Combine(Directory.GetCurrentDirectory(), "packages"),
                Directory.GetCurrentDirectory(),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "VideoLAN", "VLC"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "VideoLAN", "VLC")
            };

            foreach (var path in searchPaths)
            {
                if (Directory.Exists(path))
                {
                    if (File.Exists(Path.Combine(path, "libvlc.dll")))
                        return path;
                    var dllFiles = Directory.GetFiles(path, "libvlc.dll", SearchOption.AllDirectories);
                    if (dllFiles.Length > 0)
                        return Path.GetDirectoryName(dllFiles[0]);
                }
            }
            return null;
        }

        private void InitializeButtons()
        {
            playButton.Enabled = true;
            pauseButton.Enabled = false;
            stopButton.Enabled = false;
            playButton.ToolTipText = "Play (Space)";
            pauseButton.ToolTipText = "Pause (Space)";
            stopButton.ToolTipText = "Stop (Ctrl+S)";
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitInfo = listView1.HitTest(e.Location);
            if (hitInfo.Item != null && vlcPlayer != null)
            {
                PlaySelectedFile(hitInfo.Item);
            }
        }

        private void PlaySelectedFile(ListViewItem item)
        {
            try
            {
                string filePath = item.Tag.ToString();
                if (File.Exists(filePath))
                {
                    vlcPlayer.Play(new Uri(filePath));
                    vlcPlayer.Audio.Volume = 50;
                    playButton.Enabled = false;
                    pauseButton.Enabled = true;
                    stopButton.Enabled = true;
                    item.Selected = true;
                    item.EnsureVisible();
                }
                else
                {
                    MessageBox.Show($"Файл не найден:\n{filePath}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка воспроизведения: {ex.Message}", "Ошибка");
            }
        }

        private void importMediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMediaDialog.Filter = "Media files|*.mp4;*.avi;*.mov;*.mkv;*.jpg;*.jpeg;*.png;*.bmp";
            AddMediaDialog.Multiselect = true;
            if (AddMediaDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in AddMediaDialog.FileNames)
                {
                    Icon fileIcon = Icon.ExtractAssociatedIcon(filePath);
                    imageList1.Images.Add(filePath, fileIcon);
                    var item = new ListViewItem("", imageList1.Images.Count - 1);
                    item.SubItems.Add(Path.GetFileName(filePath));
                    item.SubItems.Add(filePath);
                    item.Tag = filePath;
                    listView1.Items.Add(item);
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if (vlcPlayer == null) return;
            var button = sender as ToolStripButton;
            switch (button?.Name)
            {
                case "playButton":
                    if (!vlcPlayer.IsPlaying)
                    {
                        if (listView1.SelectedItems.Count > 0)
                        {
                            PlaySelectedFile(listView1.SelectedItems[0]);
                        }
                        else if (listView1.Items.Count > 0)
                        {
                            listView1.Items[0].Selected = true;
                            PlaySelectedFile(listView1.Items[0]);
                        }
                        else
                        {
                            vlcPlayer.Play();
                        }
                    }
                    playButton.Enabled = false;
                    pauseButton.Enabled = true;
                    stopButton.Enabled = true;
                    break;
                case "pauseButton":
                    vlcPlayer.Pause();
                    playButton.Enabled = true;
                    pauseButton.Enabled = false;
                    stopButton.Enabled = true;
                    break;
                case "stopButton":
                    vlcPlayer.Stop();
                    playButton.Enabled = true;
                    pauseButton.Enabled = false;
                    stopButton.Enabled = false;
                    this.Text = "Media Player";
                    break;
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (vlcPlayer != null)
            {
                if (keyData == Keys.Space)
                {
                    if (vlcPlayer.IsPlaying)
                        toolStripButton1_Click(pauseButton, EventArgs.Empty);
                    else
                        toolStripButton1_Click(playButton, EventArgs.Empty);
                    return true;
                }
                else if (keyData == (Keys.Control | Keys.S))
                {
                    toolStripButton1_Click(stopButton, EventArgs.Empty);
                    return true;
                }
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e) { }
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) { vlcPlayer?.Dispose(); }
        private void MainWindow_FormClosing_1(object sender, FormClosingEventArgs e) { }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e) { }
    }
}