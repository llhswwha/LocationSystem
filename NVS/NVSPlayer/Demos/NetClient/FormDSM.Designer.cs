namespace NetClient
{
    partial class FormDSM
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle21 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle22 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle23 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle24 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabDSM = new System.Windows.Forms.TabControl();
            this.pageNVS = new System.Windows.Forms.TabPage();
            this.btnNVSRefresh = new System.Windows.Forms.Button();
            this.textNVSCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnNVSQuery = new System.Windows.Forms.Button();
            this.textNVSID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgvNVS = new System.Windows.Forms.DataGridView();
            this.NVSIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NVSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProxyIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProxyPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pageDNS = new System.Windows.Forms.TabPage();
            this.gpRefresh = new System.Windows.Forms.GroupBox();
            this.cboDNSPage = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textDNSCount = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDNSRefresh = new System.Windows.Forms.Button();
            this.gpQuery = new System.Windows.Forms.GroupBox();
            this.rdoDNSDomainName = new System.Windows.Forms.RadioButton();
            this.rdoDNSID = new System.Windows.Forms.RadioButton();
            this.textDNSDomainName = new System.Windows.Forms.TextBox();
            this.btnDNSQuery = new System.Windows.Forms.Button();
            this.textDNSID = new System.Windows.Forms.TextBox();
            this.dgvDNS = new System.Windows.Forms.DataGridView();
            this.DNSName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DNSID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LANIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WANIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Port = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DNSType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Chan = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pwd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DevUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DevPwd = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabDSM.SuspendLayout();
            this.pageNVS.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNVS)).BeginInit();
            this.pageDNS.SuspendLayout();
            this.gpRefresh.SuspendLayout();
            this.gpQuery.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDNS)).BeginInit();
            this.SuspendLayout();
            // 
            // tabDSM
            // 
            this.tabDSM.Controls.Add(this.pageNVS);
            this.tabDSM.Controls.Add(this.pageDNS);
            this.tabDSM.Location = new System.Drawing.Point(6, 8);
            this.tabDSM.Name = "tabDSM";
            this.tabDSM.SelectedIndex = 0;
            this.tabDSM.Size = new System.Drawing.Size(620, 386);
            this.tabDSM.TabIndex = 0;
            // 
            // pageNVS
            // 
            this.pageNVS.BackColor = System.Drawing.SystemColors.Control;
            this.pageNVS.Controls.Add(this.btnNVSRefresh);
            this.pageNVS.Controls.Add(this.textNVSCount);
            this.pageNVS.Controls.Add(this.label2);
            this.pageNVS.Controls.Add(this.btnNVSQuery);
            this.pageNVS.Controls.Add(this.textNVSID);
            this.pageNVS.Controls.Add(this.label1);
            this.pageNVS.Controls.Add(this.dgvNVS);
            this.pageNVS.Location = new System.Drawing.Point(4, 21);
            this.pageNVS.Name = "pageNVS";
            this.pageNVS.Padding = new System.Windows.Forms.Padding(3);
            this.pageNVS.Size = new System.Drawing.Size(612, 361);
            this.pageNVS.TabIndex = 0;
            this.pageNVS.Text = "NVS";
            // 
            // btnNVSRefresh
            // 
            this.btnNVSRefresh.Location = new System.Drawing.Point(376, 300);
            this.btnNVSRefresh.Name = "btnNVSRefresh";
            this.btnNVSRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnNVSRefresh.TabIndex = 6;
            this.btnNVSRefresh.Text = "Refresh";
            this.btnNVSRefresh.UseVisualStyleBackColor = true;
            this.btnNVSRefresh.Click += new System.EventHandler(this.btnNVSRefresh_Click);
            // 
            // textNVSCount
            // 
            this.textNVSCount.Location = new System.Drawing.Point(212, 300);
            this.textNVSCount.Name = "textNVSCount";
            this.textNVSCount.ReadOnly = true;
            this.textNVSCount.Size = new System.Drawing.Size(157, 21);
            this.textNVSCount.TabIndex = 5;
            this.textNVSCount.Text = "0";
            this.textNVSCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(148, 303);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "NVS Count:";
            // 
            // btnNVSQuery
            // 
            this.btnNVSQuery.Location = new System.Drawing.Point(376, 271);
            this.btnNVSQuery.Name = "btnNVSQuery";
            this.btnNVSQuery.Size = new System.Drawing.Size(75, 23);
            this.btnNVSQuery.TabIndex = 3;
            this.btnNVSQuery.Text = "Query";
            this.btnNVSQuery.UseVisualStyleBackColor = true;
            this.btnNVSQuery.Click += new System.EventHandler(this.btnNVSQuery_Click);
            // 
            // textNVSID
            // 
            this.textNVSID.Location = new System.Drawing.Point(212, 271);
            this.textNVSID.Name = "textNVSID";
            this.textNVSID.Size = new System.Drawing.Size(157, 21);
            this.textNVSID.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 274);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "NVS ID:";
            // 
            // dgvNVS
            // 
            this.dgvNVS.AllowUserToAddRows = false;
            this.dgvNVS.AllowUserToDeleteRows = false;
            this.dgvNVS.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvNVS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle19.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle19.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle19.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle19.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle19.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle19.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNVS.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle19;
            this.dgvNVS.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NVSIP,
            this.NVSName,
            this.Type,
            this.ID,
            this.ProxyIP,
            this.ProxyPort});
            this.dgvNVS.Location = new System.Drawing.Point(3, 5);
            this.dgvNVS.Name = "dgvNVS";
            this.dgvNVS.ReadOnly = true;
            this.dgvNVS.RowHeadersVisible = false;
            this.dgvNVS.RowHeadersWidth = 25;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvNVS.RowsDefaultCellStyle = dataGridViewCellStyle20;
            this.dgvNVS.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvNVS.RowTemplate.Height = 23;
            this.dgvNVS.RowTemplate.ReadOnly = true;
            this.dgvNVS.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNVS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvNVS.Size = new System.Drawing.Size(604, 242);
            this.dgvNVS.TabIndex = 0;
            this.dgvNVS.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNVS_CellDoubleClick);
            this.dgvNVS.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNVS_CellClick);
            // 
            // NVSIP
            // 
            this.NVSIP.HeaderText = "NVS IP";
            this.NVSIP.Name = "NVSIP";
            this.NVSIP.ReadOnly = true;
            // 
            // NVSName
            // 
            this.NVSName.HeaderText = "NVS Name";
            this.NVSName.Name = "NVSName";
            this.NVSName.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            // 
            // ProxyIP
            // 
            this.ProxyIP.HeaderText = "Proxy IP";
            this.ProxyIP.Name = "ProxyIP";
            this.ProxyIP.ReadOnly = true;
            // 
            // ProxyPort
            // 
            this.ProxyPort.HeaderText = "Proxy Port";
            this.ProxyPort.Name = "ProxyPort";
            this.ProxyPort.ReadOnly = true;
            // 
            // pageDNS
            // 
            this.pageDNS.BackColor = System.Drawing.SystemColors.Control;
            this.pageDNS.Controls.Add(this.gpRefresh);
            this.pageDNS.Controls.Add(this.gpQuery);
            this.pageDNS.Controls.Add(this.dgvDNS);
            this.pageDNS.Location = new System.Drawing.Point(4, 21);
            this.pageDNS.Name = "pageDNS";
            this.pageDNS.Padding = new System.Windows.Forms.Padding(3);
            this.pageDNS.Size = new System.Drawing.Size(612, 361);
            this.pageDNS.TabIndex = 1;
            this.pageDNS.Text = "DNS";
            // 
            // gpRefresh
            // 
            this.gpRefresh.Controls.Add(this.cboDNSPage);
            this.gpRefresh.Controls.Add(this.label6);
            this.gpRefresh.Controls.Add(this.textDNSCount);
            this.gpRefresh.Controls.Add(this.label3);
            this.gpRefresh.Controls.Add(this.btnDNSRefresh);
            this.gpRefresh.Location = new System.Drawing.Point(321, 248);
            this.gpRefresh.Name = "gpRefresh";
            this.gpRefresh.Size = new System.Drawing.Size(279, 105);
            this.gpRefresh.TabIndex = 14;
            this.gpRefresh.TabStop = false;
            // 
            // cboDNSPage
            // 
            this.cboDNSPage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDNSPage.Location = new System.Drawing.Point(95, 47);
            this.cboDNSPage.Name = "cboDNSPage";
            this.cboDNSPage.Size = new System.Drawing.Size(100, 20);
            this.cboDNSPage.TabIndex = 14;
            this.cboDNSPage.SelectedIndexChanged += new System.EventHandler(this.cboDNSPage_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 53);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "Page:";
            // 
            // textDNSCount
            // 
            this.textDNSCount.Location = new System.Drawing.Point(95, 21);
            this.textDNSCount.Name = "textDNSCount";
            this.textDNSCount.ReadOnly = true;
            this.textDNSCount.Size = new System.Drawing.Size(100, 21);
            this.textDNSCount.TabIndex = 11;
            this.textDNSCount.Text = "0";
            this.textDNSCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "DNS Count:";
            // 
            // btnDNSRefresh
            // 
            this.btnDNSRefresh.Location = new System.Drawing.Point(95, 76);
            this.btnDNSRefresh.Name = "btnDNSRefresh";
            this.btnDNSRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnDNSRefresh.TabIndex = 12;
            this.btnDNSRefresh.Text = "Refresh";
            this.btnDNSRefresh.UseVisualStyleBackColor = true;
            this.btnDNSRefresh.Click += new System.EventHandler(this.btnDNSRefresh_Click);
            // 
            // gpQuery
            // 
            this.gpQuery.Controls.Add(this.rdoDNSDomainName);
            this.gpQuery.Controls.Add(this.rdoDNSID);
            this.gpQuery.Controls.Add(this.textDNSDomainName);
            this.gpQuery.Controls.Add(this.btnDNSQuery);
            this.gpQuery.Controls.Add(this.textDNSID);
            this.gpQuery.Location = new System.Drawing.Point(7, 248);
            this.gpQuery.Name = "gpQuery";
            this.gpQuery.Size = new System.Drawing.Size(299, 105);
            this.gpQuery.TabIndex = 13;
            this.gpQuery.TabStop = false;
            // 
            // rdoDNSDomainName
            // 
            this.rdoDNSDomainName.AutoSize = true;
            this.rdoDNSDomainName.Location = new System.Drawing.Point(183, 52);
            this.rdoDNSDomainName.Name = "rdoDNSDomainName";
            this.rdoDNSDomainName.Size = new System.Drawing.Size(101, 16);
            this.rdoDNSDomainName.TabIndex = 15;
            this.rdoDNSDomainName.Text = "By DomainName";
            this.rdoDNSDomainName.UseVisualStyleBackColor = true;
            this.rdoDNSDomainName.CheckedChanged += new System.EventHandler(this.rdoDNSDomainName_CheckedChanged);
            // 
            // rdoDNSID
            // 
            this.rdoDNSID.AccessibleDescription = "rdoDNSDomainName";
            this.rdoDNSID.AutoSize = true;
            this.rdoDNSID.Checked = true;
            this.rdoDNSID.Location = new System.Drawing.Point(183, 25);
            this.rdoDNSID.Name = "rdoDNSID";
            this.rdoDNSID.Size = new System.Drawing.Size(53, 16);
            this.rdoDNSID.TabIndex = 14;
            this.rdoDNSID.TabStop = true;
            this.rdoDNSID.Text = "By ID";
            this.rdoDNSID.UseVisualStyleBackColor = true;
            this.rdoDNSID.CheckedChanged += new System.EventHandler(this.rdoDNSID_CheckedChanged);
            // 
            // textDNSDomainName
            // 
            this.textDNSDomainName.Location = new System.Drawing.Point(14, 47);
            this.textDNSDomainName.Name = "textDNSDomainName";
            this.textDNSDomainName.ReadOnly = true;
            this.textDNSDomainName.Size = new System.Drawing.Size(157, 21);
            this.textDNSDomainName.TabIndex = 12;
            // 
            // btnDNSQuery
            // 
            this.btnDNSQuery.Location = new System.Drawing.Point(115, 76);
            this.btnDNSQuery.Name = "btnDNSQuery";
            this.btnDNSQuery.Size = new System.Drawing.Size(75, 23);
            this.btnDNSQuery.TabIndex = 9;
            this.btnDNSQuery.Text = "Query";
            this.btnDNSQuery.UseVisualStyleBackColor = true;
            this.btnDNSQuery.Click += new System.EventHandler(this.btnDNSQuery_Click);
            // 
            // textDNSID
            // 
            this.textDNSID.Location = new System.Drawing.Point(14, 21);
            this.textDNSID.Name = "textDNSID";
            this.textDNSID.Size = new System.Drawing.Size(157, 21);
            this.textDNSID.TabIndex = 8;
            // 
            // dgvDNS
            // 
            this.dgvDNS.AllowUserToAddRows = false;
            this.dgvDNS.AllowUserToDeleteRows = false;
            this.dgvDNS.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dgvDNS.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle21.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle21.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle21.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle21.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle21.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle21.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle21.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvDNS.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle21;
            this.dgvDNS.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DNSName,
            this.DNSID,
            this.LANIP,
            this.WANIP,
            this.Port,
            this.DNSType,
            this.Chan,
            this.Account,
            this.Pwd,
            this.Time,
            this.DevUser,
            this.DevPwd});
            dataGridViewCellStyle22.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle22.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle22.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle22.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle22.NullValue = null;
            dataGridViewCellStyle22.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle22.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle22.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDNS.DefaultCellStyle = dataGridViewCellStyle22;
            this.dgvDNS.Location = new System.Drawing.Point(3, 5);
            this.dgvDNS.MultiSelect = false;
            this.dgvDNS.Name = "dgvDNS";
            this.dgvDNS.ReadOnly = true;
            dataGridViewCellStyle23.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle23.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle23.Font = new System.Drawing.Font("SimSun", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle23.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle23.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle23.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            this.dgvDNS.RowHeadersDefaultCellStyle = dataGridViewCellStyle23;
            this.dgvDNS.RowHeadersVisible = false;
            this.dgvDNS.RowHeadersWidth = 25;
            dataGridViewCellStyle24.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvDNS.RowsDefaultCellStyle = dataGridViewCellStyle24;
            this.dgvDNS.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.dgvDNS.RowTemplate.Height = 23;
            this.dgvDNS.RowTemplate.ReadOnly = true;
            this.dgvDNS.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvDNS.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDNS.Size = new System.Drawing.Size(604, 242);
            this.dgvDNS.TabIndex = 1;
            this.dgvDNS.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDNS_CellDoubleClick);
            this.dgvDNS.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvDNS_CellClick);
            // 
            // DNSName
            // 
            this.DNSName.HeaderText = "NVS Name";
            this.DNSName.Name = "DNSName";
            this.DNSName.ReadOnly = true;
            // 
            // DNSID
            // 
            this.DNSID.HeaderText = "ID";
            this.DNSID.Name = "DNSID";
            this.DNSID.ReadOnly = true;
            // 
            // LANIP
            // 
            this.LANIP.HeaderText = "LAN IP";
            this.LANIP.Name = "LANIP";
            this.LANIP.ReadOnly = true;
            // 
            // WANIP
            // 
            this.WANIP.HeaderText = "WAN IP";
            this.WANIP.Name = "WANIP";
            this.WANIP.ReadOnly = true;
            // 
            // Port
            // 
            this.Port.HeaderText = "Port";
            this.Port.Name = "Port";
            this.Port.ReadOnly = true;
            // 
            // DNSType
            // 
            this.DNSType.HeaderText = "Type";
            this.DNSType.Name = "DNSType";
            this.DNSType.ReadOnly = true;
            // 
            // Chan
            // 
            this.Chan.HeaderText = "Chan";
            this.Chan.Name = "Chan";
            this.Chan.ReadOnly = true;
            // 
            // Account
            // 
            this.Account.HeaderText = "Account";
            this.Account.Name = "Account";
            this.Account.ReadOnly = true;
            // 
            // Pwd
            // 
            this.Pwd.HeaderText = "Pwd";
            this.Pwd.Name = "Pwd";
            this.Pwd.ReadOnly = true;
            // 
            // Time
            // 
            this.Time.HeaderText = "Time";
            this.Time.Name = "Time";
            this.Time.ReadOnly = true;
            // 
            // DevUser
            // 
            this.DevUser.HeaderText = "Dev User";
            this.DevUser.Name = "DevUser";
            this.DevUser.ReadOnly = true;
            // 
            // DevPwd
            // 
            this.DevPwd.HeaderText = "Dev Pwd";
            this.DevPwd.Name = "DevPwd";
            this.DevPwd.ReadOnly = true;
            // 
            // FormDSM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 400);
            this.Controls.Add(this.tabDSM);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "FormDSM";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormDSM";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormDSM_FormClosed);
            this.tabDSM.ResumeLayout(false);
            this.pageNVS.ResumeLayout(false);
            this.pageNVS.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNVS)).EndInit();
            this.pageDNS.ResumeLayout(false);
            this.gpRefresh.ResumeLayout(false);
            this.gpRefresh.PerformLayout();
            this.gpQuery.ResumeLayout(false);
            this.gpQuery.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDNS)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabDSM;
        private System.Windows.Forms.TabPage pageNVS;
        private System.Windows.Forms.TabPage pageDNS;
        private System.Windows.Forms.DataGridView dgvNVS;
        private System.Windows.Forms.Button btnNVSRefresh;
        private System.Windows.Forms.TextBox textNVSCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnNVSQuery;
        private System.Windows.Forms.TextBox textNVSID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgvDNS;
        private System.Windows.Forms.Button btnDNSRefresh;
        private System.Windows.Forms.TextBox textDNSCount;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnDNSQuery;
        private System.Windows.Forms.TextBox textDNSID;
        private System.Windows.Forms.GroupBox gpQuery;
        private System.Windows.Forms.RadioButton rdoDNSDomainName;
        private System.Windows.Forms.RadioButton rdoDNSID;
        private System.Windows.Forms.TextBox textDNSDomainName;
        private System.Windows.Forms.GroupBox gpRefresh;
        private System.Windows.Forms.ComboBox cboDNSPage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn NVSIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn NVSName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProxyIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProxyPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn DNSName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DNSID;
        private System.Windows.Forms.DataGridViewTextBoxColumn LANIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn WANIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Port;
        private System.Windows.Forms.DataGridViewTextBoxColumn DNSType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Chan;
        private System.Windows.Forms.DataGridViewTextBoxColumn Account;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pwd;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn DevUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn DevPwd;
    }
}