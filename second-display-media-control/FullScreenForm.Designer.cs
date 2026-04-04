namespace second_display_media_control
{
    partial class FullScreenForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            videoPanel = new Panel();
            backgroundPictureBox = new PictureBox();
            videoPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).BeginInit();
            SuspendLayout();
            // 
            // videoPanel
            // 
            videoPanel.BackColor = Color.Black;
            videoPanel.Controls.Add(backgroundPictureBox);
            videoPanel.Dock = DockStyle.Fill;
            videoPanel.Location = new Point(0, 0);
            videoPanel.Name = "videoPanel";
            videoPanel.Size = new Size(800, 450);
            videoPanel.TabIndex = 0;
            // 
            // backgroundPictureBox
            // 
            backgroundPictureBox.Dock = DockStyle.Fill;
            backgroundPictureBox.Location = new Point(0, 0);
            backgroundPictureBox.Name = "backgroundPictureBox";
            backgroundPictureBox.Size = new Size(800, 450);
            backgroundPictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            backgroundPictureBox.TabIndex = 0;
            backgroundPictureBox.TabStop = false;
            backgroundPictureBox.Visible = false;
            // 
            // FullScreenForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(videoPanel);
            Name = "FullScreenForm";
            Text = "Form2";
            videoPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)backgroundPictureBox).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private Panel videoPanel;
        private PictureBox backgroundPictureBox;
    }
}