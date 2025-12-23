using System;
using System.IO;
using System.Windows.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;
using System.Threading.Tasks;
using System.Timers;

namespace second_display_media_control
{
    public partial class MainWindow : Form
    {
        private FullScreenForm fullScreenForm;
        private VlcControl vlcPlayer;
        private bool autoplayEnabled = false;
        private int currentPlayingIndex = -1;
        private string currentPlayingUri = "";
        private bool isPlaying = false;
        private System.Timers.Timer syncTimer;

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
            InitializeSyncTimer();

            fullScreenForm = new FullScreenForm(this);
            fullScreenForm.SetMainWindow(this);

            if (Screen.AllScreens.Length > 1)
            {
                Screen secondScreen = Screen.AllScreens[1];
                fullScreenForm.StartPosition = FormStartPosition.Manual;
                fullScreenForm.Location = secondScreen.WorkingArea.Location;
                fullScreenForm.Size = secondScreen.WorkingArea.Size;
                fullScreenForm.FormBorderStyle = FormBorderStyle.None;
            }
            else
            {
                fullScreenForm.StartPosition = FormStartPosition.CenterScreen;
                fullScreenForm.WindowState = FormWindowState.Maximized;
                fullScreenForm.FormBorderStyle = FormBorderStyle.None;
            }
        }

        private void InitializeSyncTimer()
        {
            syncTimer = new System.Timers.Timer(1000); // Синхронизация каждую секунду
            syncTimer.Elapsed += SyncTimer_Elapsed;
            syncTimer.AutoReset = true;
            syncTimer.Enabled = false;
        }

        private void SyncTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (isPlaying && vlcPlayer != null && vlcPlayer.IsPlaying)
            {
                // Синхронизация состояния и позиции
                fullScreenForm?.SyncWithMain(vlcPlayer.IsPlaying, vlcPlayer.Time);
            }
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

                // ОТКЛЮЧАЕМ ЗВУК У ПРЕВЬЮ-ПЛЕЕРА
                vlcPlayer.Audio.IsMute = true;

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
            secondScreenButton.Checked = false;
            secondScreenButton.Text = "Second Screen: OFF";
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

        public void PlaySelectedFile(ListViewItem item)
        {
            try
            {
                string filePath = item.Tag.ToString();
                if (File.Exists(filePath))
                {
                    currentPlayingUri = filePath;

                    // Останавливаем если играет
                    if (vlcPlayer.IsPlaying) vlcPlayer.Stop();
                    System.Threading.Thread.Sleep(100);

                    // Запускаем в основном плеере (без звука)
                    vlcPlayer.Play(new Uri(filePath));
                    vlcPlayer.Audio.IsMute = true; // Убеждаемся что звук отключён

                    // Запускаем в полноэкранной форме если она видима (со звуком)
                    if (fullScreenForm != null && fullScreenForm.Visible)
                    {
                        fullScreenForm.PlaySync(filePath, 50); // Здесь звук на 50%
                    }

                    isPlaying = true;
                    syncTimer.Enabled = true;

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
                isPlaying = false;
                syncTimer.Enabled = false;
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
        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            syncTimer?.Stop();
            syncTimer?.Dispose();
            vlcPlayer?.Dispose();
        }
        private void MainWindow_FormClosing_1(object sender, FormClosingEventArgs e) { }
        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e) { }

        public void PlayBoth()
        {
            if (vlcPlayer == null) return;

            if (!string.IsNullOrEmpty(currentPlayingUri))
            {
                // Если есть текущий файл, продолжаем его
                if (!vlcPlayer.IsPlaying)
                {
                    vlcPlayer.Play();
                }

                if (fullScreenForm != null && fullScreenForm.Visible)
                {
                    fullScreenForm.Play();
                }
            }
            else
            {
                // Если нет текущего файла, воспроизводим выбранный или первый
                if (listView1.SelectedItems.Count > 0)
                {
                    PlaySelectedFile(listView1.SelectedItems[0]);
                }
                else if (listView1.Items.Count > 0)
                {
                    listView1.Items[0].Selected = true;
                    PlaySelectedFile(listView1.Items[0]);
                }
            }

            isPlaying = true;
            syncTimer.Enabled = true;
            playButton.Enabled = false;
            pauseButton.Enabled = true;
            stopButton.Enabled = true;
        }

        private void playButton_Click(object sender, EventArgs e)
        {
            PlayBoth();
        }

        public void PauseBoth()
        {
            if (vlcPlayer == null) return;

            if (vlcPlayer.IsPlaying)
            {
                vlcPlayer.Pause();
            }

            if (fullScreenForm != null && fullScreenForm.Visible)
            {
                fullScreenForm.Pause();
            }

            isPlaying = false;
            syncTimer.Enabled = false;
            playButton.Enabled = true;
            pauseButton.Enabled = false;
            stopButton.Enabled = true;
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            PauseBoth();
        }

        public void StopBoth()
        {
            if (vlcPlayer == null) return;

            vlcPlayer.Stop();

            if (fullScreenForm != null && fullScreenForm.Visible)
            {
                fullScreenForm.Stop();
            }

            isPlaying = false;
            syncTimer.Enabled = false;
            currentPlayingUri = "";
            playButton.Enabled = true;
            pauseButton.Enabled = false;
            stopButton.Enabled = false;
            currentPlayingIndex = -1;
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            StopBoth();
        }

        private void autoplayButton_Click(object sender, EventArgs e)
        {
            autoplayEnabled = autoplayButton.Checked;
        }

        private void secondScreenButton_Click(object sender, EventArgs e)
        {
            if (secondScreenButton.Checked)
            {
                fullScreenForm.Show();
                secondScreenButton.Text = "Second Screen: ON";

                // Если есть текущее видео, запускаем его на втором экране
                if (!string.IsNullOrEmpty(currentPlayingUri) && vlcPlayer != null && vlcPlayer.IsPlaying)
                {
                    // Передаём текущую позицию
                    long currentTime = vlcPlayer.Time;
                    fullScreenForm.PlaySync(currentPlayingUri, vlcPlayer.Audio.Volume);
                    fullScreenForm.SetTime(currentTime);
                }
            }
            else
            {
                fullScreenForm.Hide();
                secondScreenButton.Text = "Second Screen: OFF";
            }
        }

        private async void VlcPlayer_EndReached(object sender, VlcMediaPlayerEndReachedEventArgs e)
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
                    await Task.Delay(500);

                    int nextIndex = currentPlayingIndex + 1;
                    if (nextIndex < listView1.Items.Count)
                    {
                        ListViewItem nextItem = listView1.Items[nextIndex];
                        PlaySelectedFile(nextItem);
                    }
                    else
                    {
                        currentPlayingIndex = -1;
                        isPlaying = false;
                        syncTimer.Enabled = false;
                    }
                }
                else
                {
                    currentPlayingIndex = -1;
                    isPlaying = false;
                    syncTimer.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка автовоспроизведения: {ex.Message}");
                currentPlayingIndex = -1;
                isPlaying = false;
                syncTimer.Enabled = false;
            }
        }

        public static string FindVlcPathStatic()
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

        public bool IsPlaying => isPlaying;
        public string CurrentUri => currentPlayingUri;
        public long GetCurrentTime() => vlcPlayer?.Time ?? 0;
    }
}