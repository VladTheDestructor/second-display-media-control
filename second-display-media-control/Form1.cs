using System;
using System.IO;
using System.Windows.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;
using System.Threading.Tasks;

namespace second_display_media_control
{
    public partial class MainWindow : Form
    {
        private VlcControl vlcPlayer;
        private bool autoplayEnabled = false;
        private int currentPlayingIndex = -1;
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
                vlcPlayer.EndReached += VlcPlayer_EndReached;

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
            autoplayButton.Checked = false;
            autoplayButton.Text = "Autoplay: OFF";
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
                    // Сначала останавливаем текущее воспроизведение
                    if (vlcPlayer.IsPlaying)
                        vlcPlayer.Stop();

                    // Небольшая пауза
                    System.Threading.Thread.Sleep(100);

                    // Запускаем новое видео
                    vlcPlayer.Play(new Uri(filePath));
                    vlcPlayer.Audio.Volume = 50;

                    playButton.Enabled = false;
                    pauseButton.Enabled = true;
                    stopButton.Enabled = true;
                    item.Selected = true;
                    item.EnsureVisible();
                    currentPlayingIndex = listView1.Items.IndexOf(item);
                }
                else
                {
                    MessageBox.Show($"Файл не найден:\n{filePath}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка воспроизведения: {ex.Message}", "Ошибка");
                currentPlayingIndex = -1;
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

        private void listView1_SelectedIndexChanged(object sender, EventArgs e) { }
        private void listView1_SelectedIndexChanged_1(object sender, EventArgs e) { }
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e) { vlcPlayer?.Dispose(); }
        private void MainWindow_FormClosing_1(object sender, FormClosingEventArgs e) { }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e) { }

        private void playButton_Click(object sender, EventArgs e)
        {
            if (vlcPlayer == null) return;
            if (!vlcPlayer.IsPlaying) vlcPlayer.Play();
            playButton.Enabled = false;
            pauseButton.Enabled = true;
            stopButton.Enabled = true;
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (vlcPlayer == null) return;
            if (vlcPlayer.IsPlaying) vlcPlayer.Pause();
            playButton.Enabled = true;
            pauseButton.Enabled = false;
            stopButton.Enabled = true;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            if (vlcPlayer == null) return;
            vlcPlayer.Stop();
            playButton.Enabled = true;
            pauseButton.Enabled = false;
            stopButton.Enabled = false;
            currentPlayingIndex = -1;
        }


        private void autoplayButton_Click(object sender, EventArgs e)
        {
            autoplayEnabled = autoplayButton.Checked;
        }
        private async void VlcPlayer_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => VlcPlayer_EndReached(sender, e)));
                return;
            }

            try
            {
                if (autoplayEnabled && currentPlayingIndex >= 0)
                {
                    // Небольшая задержка перед следующим файлом
                    await Task.Delay(500);

                    int nextIndex = currentPlayingIndex + 1;
                    if (nextIndex < listView1.Items.Count)
                    {
                        ListViewItem nextItem = listView1.Items[nextIndex];
                        PlaySelectedFile(nextItem);
                    }
                    else
                    {
                        // Если файлы закончились, сбрасываем индекс
                        currentPlayingIndex = -1;
                    }
                }
                else
                {
                    currentPlayingIndex = -1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка автовоспроизведения: {ex.Message}");
                currentPlayingIndex = -1;
            }
        }
    }
}