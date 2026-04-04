using System;
using System.IO;
using System.Windows.Forms;
using Vlc.DotNet.Core;
using Vlc.DotNet.Forms;
using System.Threading.Tasks;
using System.Timers;
using second_display_media_control;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

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
            listView1.DragEnter += ListView1_DragEnter;
            listView1.DragDrop += ListView1_DragDrop;
            listView1.DragOver += ListView1_DragOver;
            listView1.DragLeave += ListView1_DragLeave;
            listView1.ItemDrag += ListView1_ItemDrag;
            listView1.MultiSelect = true;
            listView1.Columns.Add("Preview", 60);
            listView1.Columns.Add("Name", 200);
            listView1.Columns.Add("Path", 400);
            listView1.SmallImageList = imageList1;
            listView1.MouseDoubleClick += ListView1_MouseDoubleClick;
            listView1.MouseClick += listView1_MouseClick;
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
            selectBackgroundImageToolStripMenuItem.Click += selectBackgroundImageToolStripMenuItem_Click;
            removeBackgroundImageToolStripMenuItem.Click += removeBackgroundImageToolStripMenuItem_Click;
        }

        
        private void InitializeSyncTimer()
        {
            syncTimer = new System.Timers.Timer(1000);
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

                // ПРОСТАЯ ИНИЦИАЛИЗАЦИЯ БЕЗ ПАРАМЕТРОВ
                vlcPlayer = new VlcControl();
                vlcPlayer.Dock = DockStyle.Fill;

                vlcPlayer.BeginInit();
                vlcPlayer.VlcLibDirectory = new DirectoryInfo(vlcPath);
                vlcPlayer.EndInit();
                vlcPlayer.EndReached += VlcPlayer_EndReached;

                // Отключаем звук ПОСЛЕ инициализации
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка VLC: {ex.Message}\n\nУбедитесь, что установлен VLC Player или пакет Vlc.DotNet.WinForms.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string FindVlcPath()
        {
            string appVlcPath = Path.Combine(Application.StartupPath, "vlc");
            if (Directory.Exists(appVlcPath) && File.Exists(Path.Combine(appVlcPath, "libvlc.dll")))
                return appVlcPath;
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
            autoplayButton.Text = "Автовоспроизведение";
            secondScreenButton.Checked = false;
            secondScreenButton.Text = "Второй экран: Выкл";
            playButton.ToolTipText = "Воспроизвести (Space)";
            pauseButton.ToolTipText = "Пауза (Space)";
            stopButton.ToolTipText = "Остановить";
        }

        private void ListView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitInfo = listView1.HitTest(e.Location);
            if (hitInfo.Item != null && vlcPlayer != null)
            {
                PlaySelectedFile(hitInfo.Item);
            }
        }

        public void PlaySelectedFile(ListViewItem listItem)
        {
            var playlistItem = (PlaylistItem)listItem.Tag;
            string filePath = playlistItem.FilePath;
            if (File.Exists(filePath))
            {
                currentPlayingUri = filePath;
                currentPlayingIndex = listItem.Index;

                // Останавливаем текущее воспроизведение
                if (vlcPlayer.IsPlaying) vlcPlayer.Stop();
                System.Threading.Thread.Sleep(100);

                // Воспроизведение на главном плеере (без звука)
                vlcPlayer.Play(new Uri(filePath), ":no-audio", ":audio-track-id=-1");
                vlcPlayer.Audio.IsMute = true;

                // Воспроизведение на втором экране с передачей фонового изображения
                if (fullScreenForm != null && fullScreenForm.Visible)
                {
                    fullScreenForm.PlaySync(filePath, 50, playlistItem.BackgroundImagePath);
                }

                isPlaying = true;
                syncTimer.Enabled = true;

                playButton.Enabled = false;
                pauseButton.Enabled = true;
                stopButton.Enabled = true;
                listItem.Selected = true;
                listItem.EnsureVisible();
            }
            else
            {
                MessageBox.Show($"Файл не найден:\n{filePath}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void importMediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMediaDialog.Filter = "Media files|*.mp4;*.avi;*.mov;*.mkv;*.mp3;*.wav;*.flac;*.jpg;*.jpeg;*.png;*.bmp";
            AddMediaDialog.Multiselect = true;
            if (AddMediaDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in AddMediaDialog.FileNames)
                {
                    AddFileToPlaylist(filePath);
                }
            }
        }
        private void AddFileToPlaylist(string filePath, string backgroundImagePath = null)
        {
            var item = new PlaylistItem
            {
                FilePath = filePath,
                BackgroundImagePath = backgroundImagePath
            };
            AddFileToPlaylist(item); // calls the existing method that takes PlaylistItem
        }
        private void AddFileToPlaylist(PlaylistItem item)
        {
            // Создаём миниатюру
            Image thumbnail;
            if (!string.IsNullOrEmpty(item.BackgroundImagePath) && File.Exists(item.BackgroundImagePath))
            {
                using (var img = Image.FromFile(item.BackgroundImagePath))
                    thumbnail = img.GetThumbnailImage(32, 32, null, IntPtr.Zero);
            }
            else
            {
                item.ThumbnailIcon = Icon.ExtractAssociatedIcon(item.FilePath);
                thumbnail = item.ThumbnailIcon?.ToBitmap();
            }

            // Генерируем уникальный ключ, чтобы избежать конфликтов
            string imageKey = Guid.NewGuid().ToString();
            imageList1.Images.Add(imageKey, thumbnail);

            var listItem = new ListViewItem("", imageList1.Images.Count - 1);
            listItem.SubItems.Add(Path.GetFileName(item.FilePath));
            listItem.SubItems.Add(item.FilePath);
            listItem.Tag = item;
            listView1.Items.Add(listItem);
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
                secondScreenButton.Text = "Второй экран: Вкл";

                // Если есть текущее видео, запускаем его на втором экране
                if (!string.IsNullOrEmpty(currentPlayingUri) && vlcPlayer != null && vlcPlayer.IsPlaying)
                {
                    long currentTime = vlcPlayer.Time;
                    // Находим элемент плейлиста по currentPlayingUri
                    PlaylistItem currentItem = null;
                    foreach (ListViewItem item in listView1.Items)
                    {
                        var pi = (PlaylistItem)item.Tag;
                        if (pi.FilePath == currentPlayingUri)
                        {
                            currentItem = pi;
                            break;
                        }
                    }
                    string bgImage = currentItem?.BackgroundImagePath;
                    fullScreenForm.PlaySync(currentPlayingUri, vlcPlayer.Audio.Volume, bgImage);
                    fullScreenForm.SetTime(currentTime);
                }
            }
            else
            {
                fullScreenForm.Hide();
                secondScreenButton.Text = "Второй экран: Выкл";
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

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "Media Project (*.sdmc)|*.sdmc";
            saveDialog.DefaultExt = "sdmc";
            saveDialog.InitialDirectory = ProjectManager.GetDefaultProjectPath();

            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                ProjectData project = CreateProjectData();

                if (ProjectManager.SaveProject(saveDialog.FileName, project))
                {
                    MessageBox.Show("Project saved successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = "Media Project (*.sdmc)|*.sdmc";
            openDialog.InitialDirectory = ProjectManager.GetDefaultProjectPath();

            if (openDialog.ShowDialog() == DialogResult.OK)
            {
                ProjectData project = ProjectManager.LoadProject(openDialog.FileName);

                if (project != null)
                {
                    LoadProjectData(project);
                    MessageBox.Show("Project loaded successfully!", "Success",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Create new project? Current playlist will be cleared.",
                "New Project", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                listView1.Items.Clear();
                imageList1.Images.Clear();
                StopBoth();
                currentPlayingIndex = -1;
                currentPlayingUri = "";
            }
        }

        // Создание данных проекта из текущего состояния
        private ProjectData CreateProjectData()
        {
            ProjectData project = new ProjectData
            {
                AutoplayEnabled = autoplayEnabled,
                CurrentPlayingIndex = currentPlayingIndex,
                CurrentPlayingUri = currentPlayingUri,
                SecondScreenEnabled = (fullScreenForm != null && fullScreenForm.Visible),
                Volume = volumeTrackBar.Value
            };
            foreach (ListViewItem item in listView1.Items)
            {
                var playlistItem = (PlaylistItem)item.Tag;
                project.Files.Add(new ProjectFileInfo
                {
                    FilePath = playlistItem.FilePath,
                    BackgroundImagePath = playlistItem.BackgroundImagePath
                });
            }
            return project;
        }

        // Загрузка данных проекта
        private void LoadProjectData(ProjectData project)
        {
            listView1.Items.Clear();
            imageList1.Images.Clear();

            foreach (var fileInfo in project.Files)
            {
                if (File.Exists(fileInfo.FilePath))
                {
                    var playlistItem = new PlaylistItem
                    {
                        FilePath = fileInfo.FilePath,
                        BackgroundImagePath = fileInfo.BackgroundImagePath
                    };
                    AddFileToPlaylist(playlistItem); // новый метод, который принимает PlaylistItem
                }
            }
            autoplayEnabled = project.AutoplayEnabled;
            autoplayButton.Checked = autoplayEnabled;
            volumeTrackBar.Value = project.Volume;
            volumeLabel.Text = $"{project.Volume}%";
            fullScreenForm?.SetVolume(project.Volume);

            if (project.SecondScreenEnabled && !fullScreenForm.Visible)
                secondScreenButton.Checked = true; // вызовет secondScreenButton_Click

            if (!string.IsNullOrEmpty(project.CurrentPlayingUri) && File.Exists(project.CurrentPlayingUri))
            {
                int index = project.CurrentPlayingIndex;
                if (index >= 0 && index < listView1.Items.Count)
                {
                    listView1.Items[index].Selected = true;
                    PlaySelectedFile(listView1.Items[index]);
                }
            }
            // восстановление остальных настроек...
        }

        // Методы контекстного меню
        private void moveUpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                int currentIndex = selectedItem.Index;

                if (currentIndex > 0)
                {
                    listView1.Items.RemoveAt(currentIndex);
                    listView1.Items.Insert(currentIndex - 1, selectedItem);
                    selectedItem.Selected = true;

                    UpdateCurrentPlayingIndexAfterMove(currentIndex, -1);
                }
            }
        }

        private void moveDownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                int currentIndex = selectedItem.Index;

                if (currentIndex < listView1.Items.Count - 1)
                {
                    listView1.Items.RemoveAt(currentIndex);
                    listView1.Items.Insert(currentIndex + 1, selectedItem);
                    selectedItem.Selected = true;

                    UpdateCurrentPlayingIndexAfterMove(currentIndex, 1);
                }
            }
        }

        private void UpdateCurrentPlayingIndexAfterMove(int movedIndex, int direction)
        {
            if (currentPlayingIndex == movedIndex)
            {
                currentPlayingIndex += direction;
            }
            else if (currentPlayingIndex == movedIndex + direction)
            {
                currentPlayingIndex = movedIndex;
            }
        }

        private void removeFromPlaylistToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                int removedIndex = selectedItem.Index;

                if (removedIndex == currentPlayingIndex)
                {
                    StopBoth();
                    currentPlayingIndex = -1;
                }
                else if (currentPlayingIndex > removedIndex)
                {
                    currentPlayingIndex--;
                }

                listView1.Items.Remove(selectedItem);
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitInfo = listView1.HitTest(e.Location);
                if (hitInfo.Item != null)
                {
                    hitInfo.Item.Selected = true;
                    FileFromListContextMenu.Show(listView1, e.Location);
                }
            }
        }

        public bool IsPlaying => isPlaying;
        public string CurrentUri => currentPlayingUri;
        public long GetCurrentTime() => vlcPlayer?.Time ?? 0;

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void volumeTrackBar_Scroll(object sender, EventArgs e)
        {
            int volume = volumeTrackBar.Value;
            volumeLabel.Text = $"{volume}%";

            if (fullScreenForm != null)
            {
                fullScreenForm.SetVolume(volume);
            }
        }

        // Drag & Drop
        private void ListView1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
            else if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
                e.Effect = DragDropEffects.Move;
            else
                e.Effect = DragDropEffects.None;
        }

        private void ListView1_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left && listView1.SelectedItems.Count > 0)
            {
                listView1.DoDragDrop(listView1.SelectedItems, DragDropEffects.Move);
            }
        }

        private void ListView1_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                e.Effect = DragDropEffects.Move;
                // mouse tracking
                Point clientPoint = listView1.PointToClient(new Point(e.X, e.Y));
                ListViewItem hoverItem = listView1.GetItemAt(clientPoint.X, clientPoint.Y);
                int targetIndex;
                if (hoverItem != null)
                {
                    targetIndex = hoverItem.Index;
                    Rectangle itemBounds = hoverItem.GetBounds(ItemBoundsPortion.Entire);
                    // If mouse is in lower half of the item, insert after it
                    if (clientPoint.Y > itemBounds.Y + itemBounds.Height / 2)
                        targetIndex++;
                }
                else
                {
                    targetIndex = listView1.Items.Count;
                }

                // place line
                listView1.InsertionMark.Index = targetIndex;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void ListView1_DragLeave(object sender, EventArgs e)
        {
            listView1.InsertionMark.Index = -1;
        }

        private void ListView1_DragDrop(object sender, DragEventArgs e)
        {
            listView1.InsertionMark.Index = -1;

            //INTERNAL REORDER
            if (e.Data.GetDataPresent(typeof(ListView.SelectedListViewItemCollection)))
            {
                Point clientPoint = listView1.PointToClient(new Point(e.X, e.Y));
                ListViewItem targetItem = listView1.GetItemAt(clientPoint.X, clientPoint.Y);
                int targetIndex = (targetItem != null) ? targetItem.Index : listView1.Items.Count;

                // Adjust for drop after item
                if (targetItem != null)
                {
                    Rectangle itemBounds = targetItem.GetBounds(ItemBoundsPortion.Entire);
                    if (clientPoint.Y > itemBounds.Y + itemBounds.Height / 2)
                        targetIndex++;
                }

                targetIndex = Math.Max(0, Math.Min(targetIndex, listView1.Items.Count));

                var selectedItems = (ListView.SelectedListViewItemCollection)e.Data.GetData(typeof(ListView.SelectedListViewItemCollection));
                if (selectedItems.Count == 0) return;

                List<int> originalIndices = new List<int>();
                foreach (ListViewItem item in selectedItems)
                {
                    originalIndices.Add(item.Index);
                }
                originalIndices.Sort();

                // Save the items to move
                List<ListViewItem> itemsToMove = new List<ListViewItem>();
                foreach (int idx in originalIndices)
                {
                    itemsToMove.Add(listView1.Items[idx]);
                }

                for (int i = originalIndices.Count - 1; i >= 0; i--)
                {
                    listView1.Items.RemoveAt(originalIndices[i]);
                }

                int removedCount = originalIndices.Count;
                int lastRemovedIndex = originalIndices[originalIndices.Count - 1];
                if (targetIndex > lastRemovedIndex)
                    targetIndex -= removedCount;
                else if (targetIndex > originalIndices[0] && targetIndex <= lastRemovedIndex)
                    targetIndex = originalIndices[0];

                for (int i = 0; i < itemsToMove.Count; i++)
                {
                    listView1.Items.Insert(targetIndex + i, itemsToMove[i]);
                }
                foreach (ListViewItem item in itemsToMove)
                {
                    item.Selected = true;
                }
                UpdateCurrentPlayingIndexAfterReorder(originalIndices, targetIndex, removedCount);

                listView1.Refresh();
            }
            //EXTERNAL FILE DROP
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files == null) return;

                foreach (string filePath in files)
                {
                    if (File.Exists(filePath))
                    {
                        AddFileToPlaylist(filePath);
                    }
                }
            }
        }

        private void UpdateCurrentPlayingIndexAfterReorder(List<int> originalIndices, int targetIndex, int movedCount)
        {
            if (currentPlayingIndex < 0) return;
            bool playingWasMoved = originalIndices.Contains(currentPlayingIndex);
            if (playingWasMoved)
            {
                int offset = originalIndices.IndexOf(currentPlayingIndex);
                currentPlayingIndex = targetIndex + offset;
            }
            else
            {
                int removedCount = movedCount;
                int lastRemovedIndex = originalIndices[originalIndices.Count - 1];

                if (currentPlayingIndex > lastRemovedIndex)
                {
                    currentPlayingIndex -= removedCount;
                }
                else if (currentPlayingIndex >= targetIndex && currentPlayingIndex < targetIndex + removedCount)
                {
                    if (targetIndex <= currentPlayingIndex)
                        currentPlayingIndex += removedCount;
                }
                else if (currentPlayingIndex >= originalIndices[0] && currentPlayingIndex <= lastRemovedIndex)
                {
                    //just in case
                }
                else if (currentPlayingIndex >= targetIndex && !playingWasMoved)
                {
                    currentPlayingIndex += removedCount;
                }
            }

            if (currentPlayingIndex >= listView1.Items.Count)
                currentPlayingIndex = listView1.Items.Count - 1;

            if (currentPlayingIndex >= 0 && currentPlayingIndex < listView1.Items.Count)
            {
                currentPlayingUri = ((PlaylistItem)listView1.Items[currentPlayingIndex].Tag).FilePath;
            }
            else
            {
                currentPlayingUri = "";
            }
        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void selectBackgroundImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var item = (PlaylistItem)listView1.SelectedItems[0].Tag;
            if (!item.IsAudioFile())
            {
                MessageBox.Show("Фоновое изображение можно установить только для аудиофайлов.", "Информация");
                return;
            }

            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp";
                ofd.Title = "Выберите фоновое изображение";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Копируем изображение в папку проекта (или в AppData)
                    string targetFolder = Path.Combine(ProjectManager.GetDefaultProjectPath(), "Backgrounds");
                    if (!Directory.Exists(targetFolder)) Directory.CreateDirectory(targetFolder);
                    string destFile = Path.Combine(targetFolder, Guid.NewGuid().ToString() + Path.GetExtension(ofd.FileName));
                    File.Copy(ofd.FileName, destFile, true);
                    item.BackgroundImagePath = destFile;

                    // Обновляем иконку Preview (миниатюра выбранного изображения)
                    UpdateThumbnailForItem(listView1.SelectedItems[0], item);

                    // Если этот файл сейчас воспроизводится и это аудио – обновляем фон на втором экране
                    if (currentPlayingUri == item.FilePath && fullScreenForm != null && fullScreenForm.Visible)
                    {
                        fullScreenForm.UpdateBackgroundImage(item.BackgroundImagePath);
                    }
                }
            }
        }

        private void removeBackgroundImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0) return;
            var item = (PlaylistItem)listView1.SelectedItems[0].Tag;
            if (item.BackgroundImagePath != null)
            {
                // Можно удалить файл изображения (опционально)
                // File.Delete(item.BackgroundImagePath);
                item.BackgroundImagePath = null;
                // Восстанавливаем стандартную иконку
                item.ThumbnailIcon = Icon.ExtractAssociatedIcon(item.FilePath);
                UpdateThumbnailForItem(listView1.SelectedItems[0], item);

                if (currentPlayingUri == item.FilePath && fullScreenForm != null && fullScreenForm.Visible)
                {
                    fullScreenForm.UpdateBackgroundImage(null);
                }
            }
        }

        private void UpdateThumbnailForItem(ListViewItem listItem, PlaylistItem playlistItem)
        {
            Image thumbnail = null;
            try
            {
                if (!string.IsNullOrEmpty(playlistItem.BackgroundImagePath) && File.Exists(playlistItem.BackgroundImagePath))
                {
                    using (var img = Image.FromFile(playlistItem.BackgroundImagePath))
                        thumbnail = img.GetThumbnailImage(32, 32, null, IntPtr.Zero);
                }
                else
                {
                    playlistItem.ThumbnailIcon = Icon.ExtractAssociatedIcon(playlistItem.FilePath);
                    thumbnail = playlistItem.ThumbnailIcon?.ToBitmap();
                }

                if (thumbnail != null)
                {
                    string key = playlistItem.FilePath;
                    // Удаляем старое изображение, если оно есть
                    if (imageList1.Images.ContainsKey(key))
                        imageList1.Images.RemoveByKey(key);
                    // Добавляем новое
                    imageList1.Images.Add(key, thumbnail);
                    // Обновляем ImageIndex у элемента списка
                    listItem.ImageIndex = imageList1.Images.IndexOfKey(key);
                }
            }
            catch (Exception ex)
            {
                // Логирование при необходимости
                Console.WriteLine($"Ошибка обновления миниатюры: {ex.Message}");
            }
        }
    }
}