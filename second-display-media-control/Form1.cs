using System;
using System.IO;
using System.Windows.Forms;

namespace second_display_media_control
{
    public partial class MainWindow : Form
    {
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
            listView1.SmallImageList = imageList1;//connects listview thing with list of actual images
        }

        private void importMediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddMediaDialog.Filter = "Media files|*.mp4;*.avi;*.mov;*.mkv;*.jpg;*.jpeg;*.png;*.bmp";
            AddMediaDialog.Multiselect = true;
            if (AddMediaDialog.ShowDialog() == DialogResult.OK)
            {
                foreach (string filePath in AddMediaDialog.FileNames)
                {
                    Icon fileIcon = Icon.ExtractAssociatedIcon(filePath);//temporary thing, will change to actual thumbnail after conneccting ffmpeg/mpv
                    imageList1.Images.Add(filePath, fileIcon);
                    var item = new ListViewItem("", imageList1.Images.Count - 1);
                    item.SubItems.Add(Path.GetFileName(filePath));
                    item.SubItems.Add(filePath);
                    item.Tag = filePath;
                    listView1.Items.Add(item);
                }
            }
        }
    }
}
    