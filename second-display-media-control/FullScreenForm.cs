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
        private bool isPaused = false;

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

                vlcPlayer = new VlcControl();
                vlcPlayer.Dock = DockStyle.Fill;
                vlcPlayer.BeginInit();
                vlcPlayer.VlcLibDirectory = new DirectoryInfo(vlcPath);
                vlcPlayer.EndInit();

                videoPanel.Controls.Add(vlcPlayer);
            }
            catch { }
        }

        public void PlaySync(string uri, int volume)
        {
            if (vlcPlayer != null)
            {
                if (currentUri != uri || !vlcPlayer.IsPlaying || isPaused)
                {
                    currentUri = uri;
                    isPaused = false;

                    if (vlcPlayer.IsPlaying) vlcPlayer.Stop();
                    System.Threading.Thread.Sleep(50);
                    vlcPlayer.Play(new Uri(uri));
                    vlcPlayer.Audio.Volume = volume;
                }
            }
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
                if (isPaused)
                {
                    // Возобновляем если было на паузе
                    vlcPlayer.Play();
                    isPaused = false;
                }
                else if (!vlcPlayer.IsPlaying)
                {
                    // Запускаем заново если остановлено
                    vlcPlayer.Play(new Uri(currentUri));
                }
            }
        }

        public void Pause()
        {
            if (vlcPlayer != null && vlcPlayer.IsPlaying)
            {
                vlcPlayer.Pause();
                isPaused = true;
            }
        }

        public void Stop()
        {
            if (vlcPlayer != null)
            {
                vlcPlayer.Stop();
                currentUri = "";
                isPaused = false;
            }
        }

        public void SetVolume(int volume)
        {
            if (vlcPlayer != null) vlcPlayer.Audio.Volume = volume;
        }

        public bool IsPlaying => vlcPlayer?.IsPlaying ?? false;

        public void SyncWithMain(bool isPlaying, long time)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SyncWithMain(isPlaying, time)));
                return;
            }

            if (vlcPlayer != null && isPlaying && this.Visible)
            {
                // Синхронизируем позицию если расхождение больше 1 секунды
                long currentTime = vlcPlayer.Time;
                if (Math.Abs(currentTime - time) > 1000) // 1 секунда
                {
                    vlcPlayer.Time = time;
                }

                // Синхронизируем состояние воспроизведения
                if (!vlcPlayer.IsPlaying && !isPaused)
                {
                    vlcPlayer.Play();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            vlcPlayer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}