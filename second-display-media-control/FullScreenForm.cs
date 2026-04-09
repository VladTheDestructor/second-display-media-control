using System;
using System.IO;
using System.Windows.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;

namespace second_display_media_control
{
    public partial class FullScreenForm : Form
    {
        private VlcControl vlcPlayer;
        private MainWindow mainWindow;
        private string currentUri = "";

        public FullScreenForm(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
            InitializeComponent();
            InitializeVlcPlayer();
        }

        public void SetMainWindow(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        private void InitializeVlcPlayer()
        {
            try
            {
                string vlcPath = MainWindow.FindVlcPathStatic();
                if (string.IsNullOrEmpty(vlcPath)) return;

                // ПРОСТАЯ ИНИЦИАЛИЗАЦИЯ
                vlcPlayer = new VlcControl();
                vlcPlayer.Dock = DockStyle.Fill;

                vlcPlayer.BeginInit();
                vlcPlayer.VlcLibDirectory = new DirectoryInfo(vlcPath);
                vlcPlayer.EndInit();

                videoPanel.Controls.Add(vlcPlayer);
            }
            catch (Exception ex)
            {
                // Без сообщений, чтобы не мешать
            }
        }

        public void PlaySync(string uri, int volume, string backgroundImagePath = null)
        {
            if (vlcPlayer != null)
            {
                if (currentUri != uri || !vlcPlayer.IsPlaying)
                {
                    currentUri = uri;
                    if (vlcPlayer.IsPlaying) vlcPlayer.Stop();
                    System.Threading.Thread.Sleep(50);

                    vlcPlayer.Play(new Uri(uri));
                    vlcPlayer.Audio.Volume = volume;

                    // Управление фоновым изображением
                    SetBackgroundImage(backgroundImagePath);
                }
            }
        }

        private void SetBackgroundImage(string imagePath)
        {
            if (IsAudioFile(currentUri) && !string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                backgroundPictureBox.ImageLocation = imagePath;
                backgroundPictureBox.Load(); // асинхронно, но можно использовать Image.FromFile
                backgroundPictureBox.Visible = true;
            }
            else
            {
                backgroundPictureBox.Visible = false;
                backgroundPictureBox.Image = null;
            }
        }

        public void UpdateBackgroundImage(string imagePath)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateBackgroundImage(imagePath)));
                return;
            }
            SetBackgroundImage(imagePath);
        }

        private bool IsAudioFile(string uri)
        {
            string ext = Path.GetExtension(uri)?.ToLower();
            return ext == ".mp3" || ext == ".wav" || ext == ".flac" || ext == ".ogg" || ext == ".m4a";
        }

        public void SetTime(long time)
        {
            if (vlcPlayer != null && vlcPlayer.IsPlaying)
            {
                vlcPlayer.Time = time;
            }
        }

        public void Play()
        {
            if (vlcPlayer != null && !string.IsNullOrEmpty(currentUri))
            {
                if (!vlcPlayer.IsPlaying)
                    vlcPlayer.Play();
            }
        }

        public void Pause()
        {
            if (vlcPlayer != null && vlcPlayer.IsPlaying)
                vlcPlayer.Pause();
        }

        public void SetVolume(int volume)
        {
            if (vlcPlayer != null)
            {
                // Ограничиваем значение 0-100 (VLC ожидает 0-200)
                int vlcVolume = Math.Max(0, Math.Min(200, volume * 2));
                vlcPlayer.Audio.Volume = vlcVolume;
            }
        }

        public void Stop()
        {
            if (vlcPlayer != null)
            {
                vlcPlayer.Stop();
                currentUri = "";
            }
        }

        public bool IsPlaying => vlcPlayer?.IsPlaying ?? false;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            vlcPlayer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}