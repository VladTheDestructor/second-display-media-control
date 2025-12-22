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
            SuspendLayout();
            videoPanel.BackColor = System.Drawing.Color.Black;
            videoPanel.Dock = DockStyle.Fill;
            videoPanel.Location = new System.Drawing.Point(0, 0);
            videoPanel.Name = "videoPanel";
            videoPanel.Size = new System.Drawing.Size(800, 450);
            videoPanel.TabIndex = 0;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(videoPanel);
            Name = "FullScreenForm";
            Text = "Form2";
            ResumeLayout(false);
        }

        #endregion

        private Panel videoPanel;
    }
}