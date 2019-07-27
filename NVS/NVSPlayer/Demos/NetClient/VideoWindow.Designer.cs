namespace NetClient
{
    partial class VideoWindow
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.picVideo = new System.Windows.Forms.PictureBox();
            this.pnlVideo = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.picVideo)).BeginInit();
            this.SuspendLayout();
            // 
            // picVideo
            // 
            this.picVideo.BackColor = System.Drawing.SystemColors.Control;
            this.picVideo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picVideo.Location = new System.Drawing.Point(0, 0);
            this.picVideo.Name = "picVideo";
            this.picVideo.Size = new System.Drawing.Size(231, 224);
            this.picVideo.TabIndex = 0;
            this.picVideo.TabStop = false;
            // 
            // pnlVideo
            // 
            this.pnlVideo.Location = new System.Drawing.Point(8, 9);
            this.pnlVideo.Name = "pnlVideo";
            this.pnlVideo.Size = new System.Drawing.Size(214, 202);
            this.pnlVideo.TabIndex = 1;
            // 
            // VideoWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.pnlVideo);
            this.Controls.Add(this.picVideo);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "VideoWindow";
            this.Size = new System.Drawing.Size(231, 224);
            this.Resize += new System.EventHandler(this.VideoWindow_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.picVideo)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.PictureBox picVideo;
        public System.Windows.Forms.Panel pnlVideo;



    }
}
