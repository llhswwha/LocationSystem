namespace NetClient
{
    partial class PlayForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.pnlVideoPlay = new System.Windows.Forms.Panel();
            this.timerPlay = new System.Windows.Forms.Timer(this.components);
            this.pgbDownload = new System.Windows.Forms.ProgressBar();
            this.pgbPlay = new System.Windows.Forms.ProgressBar();
            this.lbDownloadProgress = new System.Windows.Forms.Label();
            this.lbPlayProgress = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPlaySpeed = new System.Windows.Forms.TextBox();
            this.tbVDdecryption = new System.Windows.Forms.TextBox();
            this.tbDLSpeed = new System.Windows.Forms.TextBox();
            this.tbDLLocation = new System.Windows.Forms.TextBox();
            this.btnPlaySpeedChange = new System.Windows.Forms.Button();
            this.btnVDderyption = new System.Windows.Forms.Button();
            this.btnDLSpeedChange = new System.Windows.Forms.Button();
            this.btnDLLocation = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlVideoPlay
            // 
            this.pnlVideoPlay.Location = new System.Drawing.Point(12, 12);
            this.pnlVideoPlay.Name = "pnlVideoPlay";
            this.pnlVideoPlay.Size = new System.Drawing.Size(648, 305);
            this.pnlVideoPlay.TabIndex = 0;
            // 
            // timerPlay
            // 
            this.timerPlay.Enabled = true;
            this.timerPlay.Tick += new System.EventHandler(this.timerPlay_Tick);
            // 
            // pgbDownload
            // 
            this.pgbDownload.Location = new System.Drawing.Point(15, 346);
            this.pgbDownload.Name = "pgbDownload";
            this.pgbDownload.Size = new System.Drawing.Size(645, 23);
            this.pgbDownload.TabIndex = 1;
            // 
            // pgbPlay
            // 
            this.pgbPlay.Location = new System.Drawing.Point(15, 395);
            this.pgbPlay.Name = "pgbPlay";
            this.pgbPlay.Size = new System.Drawing.Size(645, 23);
            this.pgbPlay.TabIndex = 2;
            // 
            // lbDownloadProgress
            // 
            this.lbDownloadProgress.AutoSize = true;
            this.lbDownloadProgress.Location = new System.Drawing.Point(278, 372);
            this.lbDownloadProgress.Name = "lbDownloadProgress";
            this.lbDownloadProgress.Size = new System.Drawing.Size(77, 12);
            this.lbDownloadProgress.TabIndex = 3;
            this.lbDownloadProgress.Text = "***0%/0dB***";
            // 
            // lbPlayProgress
            // 
            this.lbPlayProgress.AutoSize = true;
            this.lbPlayProgress.Location = new System.Drawing.Point(287, 421);
            this.lbPlayProgress.Name = "lbPlayProgress";
            this.lbPlayProgress.Size = new System.Drawing.Size(53, 12);
            this.lbPlayProgress.TabIndex = 4;
            this.lbPlayProgress.Text = "***0%***";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbPlaySpeed);
            this.groupBox1.Controls.Add(this.tbVDdecryption);
            this.groupBox1.Controls.Add(this.tbDLSpeed);
            this.groupBox1.Controls.Add(this.tbDLLocation);
            this.groupBox1.Controls.Add(this.btnPlaySpeedChange);
            this.groupBox1.Controls.Add(this.btnVDderyption);
            this.groupBox1.Controls.Add(this.btnDLSpeedChange);
            this.groupBox1.Controls.Add(this.btnDLLocation);
            this.groupBox1.Location = new System.Drawing.Point(12, 453);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(648, 96);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Operation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(318, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "Play Speed(-4~4)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(315, 27);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(125, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "Download Speed(0~32)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(101, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "Video Decryption";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 12);
            this.label1.TabIndex = 8;
            this.label1.Text = "Downdload Location";
            // 
            // tbPlaySpeed
            // 
            this.tbPlaySpeed.Location = new System.Drawing.Point(446, 65);
            this.tbPlaySpeed.Name = "tbPlaySpeed";
            this.tbPlaySpeed.Size = new System.Drawing.Size(61, 21);
            this.tbPlaySpeed.TabIndex = 7;
            // 
            // tbVDdecryption
            // 
            this.tbVDdecryption.Location = new System.Drawing.Point(135, 67);
            this.tbVDdecryption.Name = "tbVDdecryption";
            this.tbVDdecryption.Size = new System.Drawing.Size(54, 21);
            this.tbVDdecryption.TabIndex = 6;
            // 
            // tbDLSpeed
            // 
            this.tbDLSpeed.Location = new System.Drawing.Point(446, 23);
            this.tbDLSpeed.Name = "tbDLSpeed";
            this.tbDLSpeed.Size = new System.Drawing.Size(61, 21);
            this.tbDLSpeed.TabIndex = 5;
            // 
            // tbDLLocation
            // 
            this.tbDLLocation.Location = new System.Drawing.Point(135, 23);
            this.tbDLLocation.Name = "tbDLLocation";
            this.tbDLLocation.Size = new System.Drawing.Size(54, 21);
            this.tbDLLocation.TabIndex = 4;
            this.tbDLLocation.Text = "1";
            // 
            // btnPlaySpeedChange
            // 
            this.btnPlaySpeedChange.Location = new System.Drawing.Point(535, 65);
            this.btnPlaySpeedChange.Name = "btnPlaySpeedChange";
            this.btnPlaySpeedChange.Size = new System.Drawing.Size(75, 23);
            this.btnPlaySpeedChange.TabIndex = 3;
            this.btnPlaySpeedChange.Text = "Change";
            this.btnPlaySpeedChange.UseVisualStyleBackColor = true;
            this.btnPlaySpeedChange.Click += new System.EventHandler(this.btnPlaySpeedChange_Click);
            // 
            // btnVDderyption
            // 
            this.btnVDderyption.Location = new System.Drawing.Point(195, 65);
            this.btnVDderyption.Name = "btnVDderyption";
            this.btnVDderyption.Size = new System.Drawing.Size(75, 23);
            this.btnVDderyption.TabIndex = 2;
            this.btnVDderyption.Text = "Decryption";
            this.btnVDderyption.UseVisualStyleBackColor = true;
            this.btnVDderyption.Click += new System.EventHandler(this.btnVDderyption_Click);
            // 
            // btnDLSpeedChange
            // 
            this.btnDLSpeedChange.Location = new System.Drawing.Point(535, 21);
            this.btnDLSpeedChange.Name = "btnDLSpeedChange";
            this.btnDLSpeedChange.Size = new System.Drawing.Size(75, 23);
            this.btnDLSpeedChange.TabIndex = 1;
            this.btnDLSpeedChange.Text = "Change";
            this.btnDLSpeedChange.UseVisualStyleBackColor = true;
            this.btnDLSpeedChange.Click += new System.EventHandler(this.btnDLSpeedChange_Click);
            // 
            // btnDLLocation
            // 
            this.btnDLLocation.Location = new System.Drawing.Point(195, 21);
            this.btnDLLocation.Name = "btnDLLocation";
            this.btnDLLocation.Size = new System.Drawing.Size(75, 23);
            this.btnDLLocation.TabIndex = 0;
            this.btnDLLocation.Text = "Location";
            this.btnDLLocation.UseVisualStyleBackColor = true;
            this.btnDLLocation.Click += new System.EventHandler(this.btnDLLocation_Click);
            // 
            // PlayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(675, 563);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbPlayProgress);
            this.Controls.Add(this.lbDownloadProgress);
            this.Controls.Add(this.pgbPlay);
            this.Controls.Add(this.pgbDownload);
            this.Controls.Add(this.pnlVideoPlay);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PlayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Play";
            this.Shown += new System.EventHandler(this.PlayForm_Shown);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PlayForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer timerPlay;
        private System.Windows.Forms.ProgressBar pgbDownload;
        private System.Windows.Forms.ProgressBar pgbPlay;
        private System.Windows.Forms.Label lbDownloadProgress;
        private System.Windows.Forms.Label lbPlayProgress;
        public System.Windows.Forms.Panel pnlVideoPlay;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbPlaySpeed;
        private System.Windows.Forms.TextBox tbVDdecryption;
        private System.Windows.Forms.TextBox tbDLSpeed;
        private System.Windows.Forms.TextBox tbDLLocation;
        private System.Windows.Forms.Button btnPlaySpeedChange;
        private System.Windows.Forms.Button btnVDderyption;
        private System.Windows.Forms.Button btnDLSpeedChange;
        private System.Windows.Forms.Button btnDLLocation;
    }
}