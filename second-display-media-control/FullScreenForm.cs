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

        public FullScreenForm()
        {
            InitializeComponent();
            InitializeVlcPlayer();
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

        public void Play(string uri)
        {
            if (vlcPlayer != null)
            {
                if (vlcPlayer.IsPlaying) vlcPlayer.Stop();
                System.Threading.Thread.Sleep(100);
                vlcPlayer.Play(new Uri(uri));
            }
        }

        public void Pause()
        {
            if (vlcPlayer != null && vlcPlayer.IsPlaying)
                vlcPlayer.Pause();
        }

        public void Stop()
        {
            if (vlcPlayer != null) vlcPlayer.Stop();
        }

        public void SetVolume(int volume)
        {
            if (vlcPlayer != null) vlcPlayer.Audio.Volume = volume;
        }

        public bool IsPlaying => vlcPlayer?.IsPlaying ?? false;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            vlcPlayer?.Dispose();
            base.OnFormClosing(e);
        }
    }
}