namespace NetClient
{
    partial class ReplayForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbTotolPage = new System.Windows.Forms.ComboBox();
            this.btnLastPage = new System.Windows.Forms.Button();
            this.btnFirstPage = new System.Windows.Forms.Button();
            this.butNextPage = new System.Windows.Forms.Button();
            this.btnUpPage = new System.Windows.Forms.Button();
            this.btnContinue = new System.Windows.Forms.Button();
            this.btnPause = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnDownload = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.lbFileNumber = new System.Windows.Forms.Label();
            this.cbQueryMode = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lbDeviceIDFile = new System.Windows.Forms.Label();
            this.lbDeviceIPFile = new System.Windows.Forms.Label();
            this.lvFileData = new System.Windows.Forms.ListView();
            this.ID = new System.Windows.Forms.ColumnHeader();
            this.FileName = new System.Windows.Forms.ColumnHeader();
            this.VideoType = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dtStartTimeFile = new System.Windows.Forms.DateTimePicker();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.tbOtherTerms = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbPassNOFile = new System.Windows.Forms.ComboBox();
            this.dtEndTimeFile = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbInputPort = new System.Windows.Forms.ComboBox();
            this.cbAlarmType = new System.Windows.Forms.ComboBox();
            this.cbVideoType = new System.Windows.Forms.ComboBox();
            this.cbFileType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbDownloadStatusTime = new System.Windows.Forms.Label();
            this.pbDownloadTime = new System.Windows.Forms.ProgressBar();
            this.btnPlayTime = new System.Windows.Forms.Button();
            this.btnStopDownloadTime = new System.Windows.Forms.Button();
            this.btnDownloadTime = new System.Windows.Forms.Button();
            this.cbPassNOTime = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbBitStreamType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dtEndTimeTime = new System.Windows.Forms.DateTimePicker();
            this.dtStartTimeTime = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.lbDeviceIDTime = new System.Windows.Forms.Label();
            this.lbDeviceIPTime = new System.Windows.Forms.Label();
            this.timerFile = new System.Windows.Forms.Timer(this.components);
            this.timerTime = new System.Windows.Forms.Timer(this.components);
            this.cbBreakContinue = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(754, 524);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbBreakContinue);
            this.tabPage1.Controls.Add(this.cbTotolPage);
            this.tabPage1.Controls.Add(this.btnLastPage);
            this.tabPage1.Controls.Add(this.btnFirstPage);
            this.tabPage1.Controls.Add(this.butNextPage);
            this.tabPage1.Controls.Add(this.btnUpPage);
            this.tabPage1.Controls.Add(this.btnContinue);
            this.tabPage1.Controls.Add(this.btnPause);
            this.tabPage1.Controls.Add(this.btnStop);
            this.tabPage1.Controls.Add(this.btnDownload);
            this.tabPage1.Controls.Add(this.btnPlay);
            this.tabPage1.Controls.Add(this.lbFileNumber);
            this.tabPage1.Controls.Add(this.cbQueryMode);
            this.tabPage1.Controls.Add(this.label10);
            this.tabPage1.Controls.Add(this.lbDeviceIDFile);
            this.tabPage1.Controls.Add(this.lbDeviceIPFile);
            this.tabPage1.Controls.Add(this.lvFileData);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(746, 498);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Play by File";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbTotolPage
            // 
            this.cbTotolPage.FormattingEnabled = true;
            this.cbTotolPage.Location = new System.Drawing.Point(672, 459);
            this.cbTotolPage.Name = "cbTotolPage";
            this.cbTotolPage.Size = new System.Drawing.Size(51, 20);
            this.cbTotolPage.TabIndex = 16;
            // 
            // btnLastPage
            // 
            this.btnLastPage.Location = new System.Drawing.Point(592, 458);
            this.btnLastPage.Name = "btnLastPage";
            this.btnLastPage.Size = new System.Drawing.Size(73, 23);
            this.btnLastPage.TabIndex = 15;
            this.btnLastPage.Text = "Last Page";
            this.btnLastPage.UseVisualStyleBackColor = true;
            this.btnLastPage.Click += new System.EventHandler(this.btnLastPage_Click);
            // 
            // btnFirstPage
            // 
            this.btnFirstPage.Location = new System.Drawing.Point(509, 457);
            this.btnFirstPage.Name = "btnFirstPage";
            this.btnFirstPage.Size = new System.Drawing.Size(75, 23);
            this.btnFirstPage.TabIndex = 14;
            this.btnFirstPage.Text = "First Page";
            this.btnFirstPage.UseVisualStyleBackColor = true;
            this.btnFirstPage.Click += new System.EventHandler(this.btnFirstPage_Click);
            // 
            // butNextPage
            // 
            this.butNextPage.Location = new System.Drawing.Point(433, 457);
            this.butNextPage.Name = "butNextPage";
            this.butNextPage.Size = new System.Drawing.Size(71, 23);
            this.butNextPage.TabIndex = 13;
            this.butNextPage.Text = "Next Page";
            this.butNextPage.UseVisualStyleBackColor = true;
            this.butNextPage.Click += new System.EventHandler(this.butNextPage_Click);
            // 
            // btnUpPage
            // 
            this.btnUpPage.Location = new System.Drawing.Point(373, 457);
            this.btnUpPage.Name = "btnUpPage";
            this.btnUpPage.Size = new System.Drawing.Size(57, 23);
            this.btnUpPage.TabIndex = 12;
            this.btnUpPage.Text = "Up Page";
            this.btnUpPage.UseVisualStyleBackColor = true;
            this.btnUpPage.Click += new System.EventHandler(this.btnUpPage_Click);
            // 
            // btnContinue
            // 
            this.btnContinue.Location = new System.Drawing.Point(290, 458);
            this.btnContinue.Name = "btnContinue";
            this.btnContinue.Size = new System.Drawing.Size(69, 23);
            this.btnContinue.TabIndex = 11;
            this.btnContinue.Text = "Continue";
            this.btnContinue.UseVisualStyleBackColor = true;
            this.btnContinue.Click += new System.EventHandler(this.btnContinue_Click);
            // 
            // btnPause
            // 
            this.btnPause.Location = new System.Drawing.Point(225, 459);
            this.btnPause.Name = "btnPause";
            this.btnPause.Size = new System.Drawing.Size(59, 23);
            this.btnPause.TabIndex = 10;
            this.btnPause.Text = "Pause";
            this.btnPause.UseVisualStyleBackColor = true;
            this.btnPause.Click += new System.EventHandler(this.btnPause_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(153, 459);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(66, 23);
            this.btnStop.TabIndex = 9;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnDownload
            // 
            this.btnDownload.Location = new System.Drawing.Point(81, 461);
            this.btnDownload.Name = "btnDownload";
            this.btnDownload.Size = new System.Drawing.Size(66, 23);
            this.btnDownload.TabIndex = 8;
            this.btnDownload.Text = "Download";
            this.btnDownload.UseVisualStyleBackColor = true;
            this.btnDownload.Click += new System.EventHandler(this.btnDownload_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.Location = new System.Drawing.Point(13, 461);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(62, 23);
            this.btnPlay.TabIndex = 7;
            this.btnPlay.Text = "Play";
            this.btnPlay.UseVisualStyleBackColor = true;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // lbFileNumber
            // 
            this.lbFileNumber.AutoSize = true;
            this.lbFileNumber.Location = new System.Drawing.Point(625, 193);
            this.lbFileNumber.Name = "lbFileNumber";
            this.lbFileNumber.Size = new System.Drawing.Size(83, 12);
            this.lbFileNumber.TabIndex = 6;
            this.lbFileNumber.Text = "Totol Number:";
            // 
            // cbQueryMode
            // 
            this.cbQueryMode.FormattingEnabled = true;
            this.cbQueryMode.Items.AddRange(new object[] {
            "流模式",
            "帧模式"});
            this.cbQueryMode.Location = new System.Drawing.Point(419, 190);
            this.cbQueryMode.Name = "cbQueryMode";
            this.cbQueryMode.Size = new System.Drawing.Size(59, 20);
            this.cbQueryMode.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(342, 194);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(71, 12);
            this.label10.TabIndex = 4;
            this.label10.Text = "Query Mode:";
            // 
            // lbDeviceIDFile
            // 
            this.lbDeviceIDFile.AutoSize = true;
            this.lbDeviceIDFile.Location = new System.Drawing.Point(154, 194);
            this.lbDeviceIDFile.Name = "lbDeviceIDFile";
            this.lbDeviceIDFile.Size = new System.Drawing.Size(65, 12);
            this.lbDeviceIDFile.TabIndex = 3;
            this.lbDeviceIDFile.Text = "Device ID:";
            // 
            // lbDeviceIPFile
            // 
            this.lbDeviceIPFile.AutoSize = true;
            this.lbDeviceIPFile.Location = new System.Drawing.Point(11, 194);
            this.lbDeviceIPFile.Name = "lbDeviceIPFile";
            this.lbDeviceIPFile.Size = new System.Drawing.Size(65, 12);
            this.lbDeviceIPFile.TabIndex = 2;
            this.lbDeviceIPFile.Text = "Device IP:";
            // 
            // lvFileData
            // 
            this.lvFileData.CheckBoxes = true;
            this.lvFileData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.ID,
            this.FileName,
            this.VideoType,
            this.columnHeader2,
            this.columnHeader3,
            this.columnHeader1,
            this.columnHeader4});
            this.lvFileData.FullRowSelect = true;
            this.lvFileData.GridLines = true;
            this.lvFileData.Location = new System.Drawing.Point(14, 217);
            this.lvFileData.MultiSelect = false;
            this.lvFileData.Name = "lvFileData";
            this.lvFileData.Size = new System.Drawing.Size(724, 220);
            this.lvFileData.TabIndex = 1;
            this.lvFileData.UseCompatibleStateImageBehavior = false;
            this.lvFileData.View = System.Windows.Forms.View.Details;
            this.lvFileData.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.lvFileData_ItemChecked);
            // 
            // ID
            // 
            this.ID.Text = "ID";
            this.ID.Width = 41;
            // 
            // FileName
            // 
            this.FileName.Text = "Name";
            this.FileName.Width = 176;
            // 
            // VideoType
            // 
            this.VideoType.Text = "Video Type";
            this.VideoType.Width = 74;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "FileSize";
            this.columnHeader2.Width = 63;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Start Time";
            this.columnHeader3.Width = 125;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "End Time";
            this.columnHeader1.Width = 142;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "DownLoad";
            this.columnHeader4.Width = 88;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dtStartTimeFile);
            this.groupBox1.Controls.Add(this.listBox1);
            this.groupBox1.Controls.Add(this.tbOtherTerms);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.cbPassNOFile);
            this.groupBox1.Controls.Add(this.dtEndTimeFile);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cbInputPort);
            this.groupBox1.Controls.Add(this.cbAlarmType);
            this.groupBox1.Controls.Add(this.cbVideoType);
            this.groupBox1.Controls.Add(this.cbFileType);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Location = new System.Drawing.Point(13, 15);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(725, 158);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Query terms";
            // 
            // dtStartTimeFile
            // 
            this.dtStartTimeFile.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtStartTimeFile.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStartTimeFile.Location = new System.Drawing.Point(289, 62);
            this.dtStartTimeFile.Name = "dtStartTimeFile";
            this.dtStartTimeFile.ShowUpDown = true;
            this.dtStartTimeFile.Size = new System.Drawing.Size(140, 21);
            this.dtStartTimeFile.TabIndex = 17;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 12;
            this.listBox1.Location = new System.Drawing.Point(503, 33);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(8, 4);
            this.listBox1.TabIndex = 16;
            // 
            // tbOtherTerms
            // 
            this.tbOtherTerms.Location = new System.Drawing.Point(88, 120);
            this.tbOtherTerms.Name = "tbOtherTerms";
            this.tbOtherTerms.Size = new System.Drawing.Size(260, 21);
            this.tbOtherTerms.TabIndex = 15;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(11, 123);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 14;
            this.label7.Text = "Other terms";
            // 
            // cbPassNOFile
            // 
            this.cbPassNOFile.FormattingEnabled = true;
            this.cbPassNOFile.Location = new System.Drawing.Point(78, 65);
            this.cbPassNOFile.Name = "cbPassNOFile";
            this.cbPassNOFile.Size = new System.Drawing.Size(78, 20);
            this.cbPassNOFile.TabIndex = 13;
            // 
            // dtEndTimeFile
            // 
            this.dtEndTimeFile.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtEndTimeFile.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEndTimeFile.Location = new System.Drawing.Point(490, 62);
            this.dtEndTimeFile.Name = "dtEndTimeFile";
            this.dtEndTimeFile.ShowUpDown = true;
            this.dtEndTimeFile.Size = new System.Drawing.Size(137, 21);
            this.dtEndTimeFile.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(181, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "Time frame:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 68);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "Pass NO.";
            // 
            // cbInputPort
            // 
            this.cbInputPort.FormattingEnabled = true;
            this.cbInputPort.Location = new System.Drawing.Point(633, 29);
            this.cbInputPort.Name = "cbInputPort";
            this.cbInputPort.Size = new System.Drawing.Size(77, 20);
            this.cbInputPort.TabIndex = 8;
            // 
            // cbAlarmType
            // 
            this.cbAlarmType.FormattingEnabled = true;
            this.cbAlarmType.Location = new System.Drawing.Point(454, 28);
            this.cbAlarmType.Name = "cbAlarmType";
            this.cbAlarmType.Size = new System.Drawing.Size(82, 20);
            this.cbAlarmType.TabIndex = 7;
            // 
            // cbVideoType
            // 
            this.cbVideoType.FormattingEnabled = true;
            this.cbVideoType.Items.AddRange(new object[] {
            "All",
            "手动录像",
            "定时录像",
            "报警录像"});
            this.cbVideoType.Location = new System.Drawing.Point(258, 26);
            this.cbVideoType.Name = "cbVideoType";
            this.cbVideoType.Size = new System.Drawing.Size(88, 20);
            this.cbVideoType.TabIndex = 6;
            // 
            // cbFileType
            // 
            this.cbFileType.FormattingEnabled = true;
            this.cbFileType.Items.AddRange(new object[] {
            "All",
            "Video",
            "Picture"});
            this.cbFileType.Location = new System.Drawing.Point(78, 23);
            this.cbFileType.Name = "cbFileType";
            this.cbFileType.Size = new System.Drawing.Size(78, 20);
            this.cbFileType.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(556, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "Input port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(377, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "Alarm type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(181, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "Video type:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "File type:";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(633, 120);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 0;
            this.btnQuery.Text = "Query";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.lbDeviceIDTime);
            this.tabPage2.Controls.Add(this.lbDeviceIPTime);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(746, 498);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Play by Time";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbDownloadStatusTime);
            this.groupBox2.Controls.Add(this.pbDownloadTime);
            this.groupBox2.Controls.Add(this.btnPlayTime);
            this.groupBox2.Controls.Add(this.btnStopDownloadTime);
            this.groupBox2.Controls.Add(this.btnDownloadTime);
            this.groupBox2.Controls.Add(this.cbPassNOTime);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.cbBitStreamType);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.dtEndTimeTime);
            this.groupBox2.Controls.Add(this.dtStartTimeTime);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Location = new System.Drawing.Point(32, 143);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(680, 229);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Download and Play";
            // 
            // lbDownloadStatusTime
            // 
            this.lbDownloadStatusTime.AutoSize = true;
            this.lbDownloadStatusTime.Location = new System.Drawing.Point(267, 203);
            this.lbDownloadStatusTime.Name = "lbDownloadStatusTime";
            this.lbDownloadStatusTime.Size = new System.Drawing.Size(95, 12);
            this.lbDownloadStatusTime.TabIndex = 25;
            this.lbDownloadStatusTime.Text = "Download Status";
            // 
            // pbDownloadTime
            // 
            this.pbDownloadTime.Location = new System.Drawing.Point(24, 163);
            this.pbDownloadTime.Name = "pbDownloadTime";
            this.pbDownloadTime.Size = new System.Drawing.Size(621, 23);
            this.pbDownloadTime.TabIndex = 24;
            // 
            // btnPlayTime
            // 
            this.btnPlayTime.Location = new System.Drawing.Point(570, 113);
            this.btnPlayTime.Name = "btnPlayTime";
            this.btnPlayTime.Size = new System.Drawing.Size(75, 23);
            this.btnPlayTime.TabIndex = 23;
            this.btnPlayTime.Text = "Play";
            this.btnPlayTime.UseVisualStyleBackColor = true;
            this.btnPlayTime.Click += new System.EventHandler(this.btnPlayTime_Click);
            // 
            // btnStopDownloadTime
            // 
            this.btnStopDownloadTime.Location = new System.Drawing.Point(454, 113);
            this.btnStopDownloadTime.Name = "btnStopDownloadTime";
            this.btnStopDownloadTime.Size = new System.Drawing.Size(92, 23);
            this.btnStopDownloadTime.TabIndex = 22;
            this.btnStopDownloadTime.Text = "Stop Download";
            this.btnStopDownloadTime.UseVisualStyleBackColor = true;
            this.btnStopDownloadTime.Click += new System.EventHandler(this.btnStopDownloadTime_Click);
            // 
            // btnDownloadTime
            // 
            this.btnDownloadTime.Location = new System.Drawing.Point(355, 113);
            this.btnDownloadTime.Name = "btnDownloadTime";
            this.btnDownloadTime.Size = new System.Drawing.Size(75, 23);
            this.btnDownloadTime.TabIndex = 21;
            this.btnDownloadTime.Text = "Download";
            this.btnDownloadTime.UseVisualStyleBackColor = true;
            this.btnDownloadTime.Click += new System.EventHandler(this.btnDownloadTime_Click);
            // 
            // cbPassNOTime
            // 
            this.cbPassNOTime.FormattingEnabled = true;
            this.cbPassNOTime.Location = new System.Drawing.Point(486, 65);
            this.cbPassNOTime.Name = "cbPassNOTime";
            this.cbPassNOTime.Size = new System.Drawing.Size(78, 20);
            this.cbPassNOTime.TabIndex = 20;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(365, 68);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 19;
            this.label12.Text = "Pass NO.";
            // 
            // cbBitStreamType
            // 
            this.cbBitStreamType.FormattingEnabled = true;
            this.cbBitStreamType.Items.AddRange(new object[] {
            "主码流",
            "副码流"});
            this.cbBitStreamType.Location = new System.Drawing.Point(486, 27);
            this.cbBitStreamType.Name = "cbBitStreamType";
            this.cbBitStreamType.Size = new System.Drawing.Size(78, 20);
            this.cbBitStreamType.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(364, 35);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(101, 12);
            this.label11.TabIndex = 17;
            this.label11.Text = "Bit Stream Type:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(22, 73);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(59, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "End Time:";
            // 
            // dtEndTimeTime
            // 
            this.dtEndTimeTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtEndTimeTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEndTimeTime.Location = new System.Drawing.Point(114, 67);
            this.dtEndTimeTime.Name = "dtEndTimeTime";
            this.dtEndTimeTime.ShowUpDown = true;
            this.dtEndTimeTime.Size = new System.Drawing.Size(153, 21);
            this.dtEndTimeTime.TabIndex = 15;
            // 
            // dtStartTimeTime
            // 
            this.dtStartTimeTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtStartTimeTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtStartTimeTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dtStartTimeTime.Location = new System.Drawing.Point(114, 31);
            this.dtStartTimeTime.Name = "dtStartTimeTime";
            this.dtStartTimeTime.ShowUpDown = true;
            this.dtStartTimeTime.Size = new System.Drawing.Size(153, 21);
            this.dtStartTimeTime.TabIndex = 14;
            this.dtStartTimeTime.Value = new System.DateTime(2015, 7, 4, 11, 59, 53, 0);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(22, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 12);
            this.label8.TabIndex = 13;
            this.label8.Text = "Start Time:";
            // 
            // lbDeviceIDTime
            // 
            this.lbDeviceIDTime.AutoSize = true;
            this.lbDeviceIDTime.Location = new System.Drawing.Point(281, 105);
            this.lbDeviceIDTime.Name = "lbDeviceIDTime";
            this.lbDeviceIDTime.Size = new System.Drawing.Size(65, 12);
            this.lbDeviceIDTime.TabIndex = 5;
            this.lbDeviceIDTime.Text = "Device ID:";
            // 
            // lbDeviceIPTime
            // 
            this.lbDeviceIPTime.AutoSize = true;
            this.lbDeviceIPTime.Location = new System.Drawing.Point(30, 105);
            this.lbDeviceIPTime.Name = "lbDeviceIPTime";
            this.lbDeviceIPTime.Size = new System.Drawing.Size(65, 12);
            this.lbDeviceIPTime.TabIndex = 4;
            this.lbDeviceIPTime.Text = "Device IP:";
            // 
            // timerFile
            // 
            this.timerFile.Tick += new System.EventHandler(this.timerFile_Tick);
            // 
            // timerTime
            // 
            this.timerTime.Tick += new System.EventHandler(this.timerTime_Tick);
            // 
            // cbBreakContinue
            // 
            this.cbBreakContinue.AutoSize = true;
            this.cbBreakContinue.Location = new System.Drawing.Point(503, 193);
            this.cbBreakContinue.Name = "cbBreakContinue";
            this.cbBreakContinue.Size = new System.Drawing.Size(108, 16);
            this.cbBreakContinue.TabIndex = 17;
            this.cbBreakContinue.Text = "Break Continue";
            this.cbBreakContinue.UseVisualStyleBackColor = true;
            // 
            // ReplayForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 524);
            this.Controls.Add(this.tabControl1);
            this.Name = "ReplayForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Play Back";
            this.Load += new System.EventHandler(this.ReplayForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ReplayForm_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvFileData;
        private System.Windows.Forms.ColumnHeader ID;
        private System.Windows.Forms.ColumnHeader FileName;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.ComboBox cbInputPort;
        private System.Windows.Forms.ComboBox cbAlarmType;
        private System.Windows.Forms.ComboBox cbVideoType;
        private System.Windows.Forms.ComboBox cbFileType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbOtherTerms;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbPassNOFile;
        private System.Windows.Forms.DateTimePicker dtEndTimeFile;
        private System.Windows.Forms.Label lbFileNumber;
        private System.Windows.Forms.ComboBox cbQueryMode;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbDeviceIDFile;
        private System.Windows.Forms.Label lbDeviceIPFile;
        private System.Windows.Forms.ComboBox cbTotolPage;
        private System.Windows.Forms.Button btnLastPage;
        private System.Windows.Forms.Button btnFirstPage;
        private System.Windows.Forms.Button butNextPage;
        private System.Windows.Forms.Button btnUpPage;
        private System.Windows.Forms.Button btnContinue;
        private System.Windows.Forms.Button btnPause;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnDownload;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbDeviceIDTime;
        private System.Windows.Forms.Label lbDeviceIPTime;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtEndTimeTime;
        private System.Windows.Forms.DateTimePicker dtStartTimeTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbPassNOTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbBitStreamType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnPlayTime;
        private System.Windows.Forms.Button btnStopDownloadTime;
        private System.Windows.Forms.Button btnDownloadTime;
        private System.Windows.Forms.Label lbDownloadStatusTime;
        private System.Windows.Forms.ProgressBar pbDownloadTime;
        private System.Windows.Forms.ColumnHeader VideoType;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Timer timerFile;
        private System.Windows.Forms.DateTimePicker dtStartTimeFile;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.Timer timerTime;
        private System.Windows.Forms.CheckBox cbBreakContinue;
    }
}