namespace XWare.ACME.UI
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.menuMain = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.registerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusMain = new System.Windows.Forms.StatusStrip();
            this.labInfo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolMain = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbAccounts = new System.Windows.Forms.ToolStripComboBox();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnEdit = new System.Windows.Forms.ToolStripButton();
            this.btnRegister = new System.Windows.Forms.ToolStripButton();
            this.dgvDomains = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.toolDomains = new System.Windows.Forms.ToolStrip();
            this.btnRefreshDomains = new System.Windows.Forms.ToolStripButton();
            this.btnAddDomains = new System.Windows.Forms.ToolStripButton();
            this.btnAuthorize = new System.Windows.Forms.ToolStripButton();
            this.btnSelectAllDomains = new System.Windows.Forms.ToolStripButton();
            this.btnReverseSelectDomain = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolCertificate = new System.Windows.Forms.ToolStrip();
            this.btnCertificate = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSavePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.radioCSRAutoGen = new System.Windows.Forms.RadioButton();
            this.radioCSRUseCustom = new System.Windows.Forms.RadioButton();
            this.txtCSR = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCSRCommonName = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.menuMain.SuspendLayout();
            this.statusMain.SuspendLayout();
            this.toolMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDomains)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.toolDomains.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.toolCertificate.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuMain
            // 
            this.menuMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuMain.Location = new System.Drawing.Point(0, 0);
            this.menuMain.Name = "menuMain";
            this.menuMain.Size = new System.Drawing.Size(1238, 32);
            this.menuMain.TabIndex = 0;
            this.menuMain.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.registerToolStripMenuItem,
            this.toolStripMenuItem1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(52, 28);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // registerToolStripMenuItem
            // 
            this.registerToolStripMenuItem.Name = "registerToolStripMenuItem";
            this.registerToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.registerToolStripMenuItem.Text = "&Register";
            this.registerToolStripMenuItem.Click += new System.EventHandler(this.registerToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(161, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(164, 30);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // statusMain
            // 
            this.statusMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.labInfo});
            this.statusMain.Location = new System.Drawing.Point(0, 515);
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(1238, 29);
            this.statusMain.TabIndex = 1;
            this.statusMain.Text = "statusStrip1";
            // 
            // labInfo
            // 
            this.labInfo.Name = "labInfo";
            this.labInfo.Size = new System.Drawing.Size(64, 24);
            this.labInfo.Text = "Ready";
            // 
            // toolMain
            // 
            this.toolMain.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cmbAccounts,
            this.btnRefresh,
            this.btnEdit,
            this.btnRegister});
            this.toolMain.Location = new System.Drawing.Point(0, 32);
            this.toolMain.Name = "toolMain";
            this.toolMain.Size = new System.Drawing.Size(1238, 32);
            this.toolMain.TabIndex = 7;
            this.toolMain.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(93, 29);
            this.toolStripLabel1.Text = "Accounts:";
            // 
            // cmbAccounts
            // 
            this.cmbAccounts.AutoSize = false;
            this.cmbAccounts.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAccounts.Name = "cmbAccounts";
            this.cmbAccounts.Size = new System.Drawing.Size(550, 32);
            this.cmbAccounts.SelectedIndexChanged += new System.EventHandler(this.cmbAccounts_SelectedIndexChanged);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(78, 29);
            this.btnRefresh.Text = "&Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnEdit.Image = ((System.Drawing.Image)(resources.GetObject("btnEdit.Image")));
            this.btnEdit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(48, 29);
            this.btnEdit.Text = "&Edit";
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnRegister
            // 
            this.btnRegister.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRegister.Image = ((System.Drawing.Image)(resources.GetObject("btnRegister.Image")));
            this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(85, 29);
            this.btnRegister.Text = "&Register";
            this.btnRegister.Click += new System.EventHandler(this.registerToolStripMenuItem_Click);
            // 
            // dgvDomains
            // 
            this.dgvDomains.AllowUserToAddRows = false;
            this.dgvDomains.AllowUserToDeleteRows = false;
            this.dgvDomains.AllowUserToOrderColumns = true;
            this.dgvDomains.AllowUserToResizeRows = false;
            this.dgvDomains.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDomains.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDomains.Location = new System.Drawing.Point(3, 55);
            this.dgvDomains.Name = "dgvDomains";
            this.dgvDomains.RowHeadersVisible = false;
            this.dgvDomains.RowTemplate.Height = 30;
            this.dgvDomains.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDomains.Size = new System.Drawing.Size(629, 393);
            this.dgvDomains.TabIndex = 8;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvDomains);
            this.groupBox1.Controls.Add(this.toolDomains);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(635, 451);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Domains";
            // 
            // toolDomains
            // 
            this.toolDomains.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolDomains.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefreshDomains,
            this.btnAddDomains,
            this.btnAuthorize,
            this.btnSelectAllDomains,
            this.btnReverseSelectDomain});
            this.toolDomains.Location = new System.Drawing.Point(3, 24);
            this.toolDomains.Name = "toolDomains";
            this.toolDomains.Size = new System.Drawing.Size(629, 31);
            this.toolDomains.TabIndex = 0;
            this.toolDomains.Text = "toolStrip2";
            // 
            // btnRefreshDomains
            // 
            this.btnRefreshDomains.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRefreshDomains.Image = ((System.Drawing.Image)(resources.GetObject("btnRefreshDomains.Image")));
            this.btnRefreshDomains.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshDomains.Name = "btnRefreshDomains";
            this.btnRefreshDomains.Size = new System.Drawing.Size(78, 28);
            this.btnRefreshDomains.Text = "&Refresh";
            this.btnRefreshDomains.Click += new System.EventHandler(this.btnRefreshDomains_Click);
            // 
            // btnAddDomains
            // 
            this.btnAddDomains.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAddDomains.Image = ((System.Drawing.Image)(resources.GetObject("btnAddDomains.Image")));
            this.btnAddDomains.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddDomains.Name = "btnAddDomains";
            this.btnAddDomains.Size = new System.Drawing.Size(51, 28);
            this.btnAddDomains.Text = "&Add";
            this.btnAddDomains.Click += new System.EventHandler(this.btnAddDomains_Click);
            // 
            // btnAuthorize
            // 
            this.btnAuthorize.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnAuthorize.Image = ((System.Drawing.Image)(resources.GetObject("btnAuthorize.Image")));
            this.btnAuthorize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAuthorize.Name = "btnAuthorize";
            this.btnAuthorize.Size = new System.Drawing.Size(98, 28);
            this.btnAuthorize.Text = "&Authorize";
            this.btnAuthorize.Click += new System.EventHandler(this.btnAuthorize_Click);
            // 
            // btnSelectAllDomains
            // 
            this.btnSelectAllDomains.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSelectAllDomains.Image = ((System.Drawing.Image)(resources.GetObject("btnSelectAllDomains.Image")));
            this.btnSelectAllDomains.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectAllDomains.Name = "btnSelectAllDomains";
            this.btnSelectAllDomains.Size = new System.Drawing.Size(93, 28);
            this.btnSelectAllDomains.Text = "Select All";
            this.btnSelectAllDomains.Click += new System.EventHandler(this.btnSelectAllDomains_Click);
            // 
            // btnReverseSelectDomain
            // 
            this.btnReverseSelectDomain.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnReverseSelectDomain.Image = ((System.Drawing.Image)(resources.GetObject("btnReverseSelectDomain.Image")));
            this.btnReverseSelectDomain.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReverseSelectDomain.Name = "btnReverseSelectDomain";
            this.btnReverseSelectDomain.Size = new System.Drawing.Size(136, 28);
            this.btnReverseSelectDomain.Text = "Reverse Select";
            this.btnReverseSelectDomain.Click += new System.EventHandler(this.btnReverseSelectDomain_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.groupBox3);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Controls.Add(this.toolCertificate);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(635, 64);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(603, 451);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Certificate";
            // 
            // toolCertificate
            // 
            this.toolCertificate.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolCertificate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCertificate});
            this.toolCertificate.Location = new System.Drawing.Point(3, 24);
            this.toolCertificate.Name = "toolCertificate";
            this.toolCertificate.Size = new System.Drawing.Size(597, 31);
            this.toolCertificate.TabIndex = 0;
            this.toolCertificate.Text = "toolStrip3";
            // 
            // btnCertificate
            // 
            this.btnCertificate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnCertificate.Image = ((System.Drawing.Image)(resources.GetObject("btnCertificate.Image")));
            this.btnCertificate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCertificate.Name = "btnCertificate";
            this.btnCertificate.Size = new System.Drawing.Size(185, 28);
            this.btnCertificate.Text = "Generate Certificate";
            this.btnCertificate.Click += new System.EventHandler(this.btnCertificate_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Save Path:";
            // 
            // txtSavePath
            // 
            this.txtSavePath.Location = new System.Drawing.Point(17, 38);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new System.Drawing.Size(485, 28);
            this.txtSavePath.TabIndex = 2;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(508, 38);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(80, 28);
            this.btnBrowse.TabIndex = 3;
            this.btnBrowse.Text = "&Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.btnBrowse);
            this.panel1.Controls.Add(this.txtSavePath);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 55);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(597, 93);
            this.panel1.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtCSRCommonName);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtCSR);
            this.groupBox3.Controls.Add(this.radioCSRUseCustom);
            this.groupBox3.Controls.Add(this.radioCSRAutoGen);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 148);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(597, 300);
            this.groupBox3.TabIndex = 5;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "CSR (Certificate Signing Request)";
            // 
            // radioCSRAutoGen
            // 
            this.radioCSRAutoGen.AutoSize = true;
            this.radioCSRAutoGen.Checked = true;
            this.radioCSRAutoGen.Location = new System.Drawing.Point(7, 28);
            this.radioCSRAutoGen.Name = "radioCSRAutoGen";
            this.radioCSRAutoGen.Size = new System.Drawing.Size(240, 22);
            this.radioCSRAutoGen.TabIndex = 0;
            this.radioCSRAutoGen.TabStop = true;
            this.radioCSRAutoGen.Text = "Automatically generate.";
            this.radioCSRAutoGen.UseVisualStyleBackColor = true;
            // 
            // radioCSRUseCustom
            // 
            this.radioCSRUseCustom.AutoSize = true;
            this.radioCSRUseCustom.Location = new System.Drawing.Point(7, 101);
            this.radioCSRUseCustom.Name = "radioCSRUseCustom";
            this.radioCSRUseCustom.Size = new System.Drawing.Size(285, 22);
            this.radioCSRUseCustom.TabIndex = 1;
            this.radioCSRUseCustom.Text = "Use custom CSR (PEM Format).";
            this.radioCSRUseCustom.UseVisualStyleBackColor = true;
            // 
            // txtCSR
            // 
            this.txtCSR.Location = new System.Drawing.Point(30, 129);
            this.txtCSR.Multiline = true;
            this.txtCSR.Name = "txtCSR";
            this.txtCSR.Size = new System.Drawing.Size(558, 153);
            this.txtCSR.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(116, 18);
            this.label2.TabIndex = 3;
            this.label2.Text = "Common Name:";
            // 
            // txtCSRCommonName
            // 
            this.txtCSRCommonName.Location = new System.Drawing.Point(152, 61);
            this.txtCSRCommonName.Name = "txtCSRCommonName";
            this.txtCSRCommonName.Size = new System.Drawing.Size(436, 28);
            this.txtCSRCommonName.TabIndex = 4;
            // 
            // folderBrowserDialog1
            // 
            this.folderBrowserDialog1.RootFolder = System.Environment.SpecialFolder.MyComputer;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1238, 544);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolMain);
            this.Controls.Add(this.statusMain);
            this.Controls.Add(this.menuMain);
            this.MainMenuStrip = this.menuMain;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Let\'s Encrypt with DNSPod";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.menuMain.ResumeLayout(false);
            this.menuMain.PerformLayout();
            this.statusMain.ResumeLayout(false);
            this.statusMain.PerformLayout();
            this.toolMain.ResumeLayout(false);
            this.toolMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDomains)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.toolDomains.ResumeLayout(false);
            this.toolDomains.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolCertificate.ResumeLayout(false);
            this.toolCertificate.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuMain;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem registerToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusMain;
        private System.Windows.Forms.ToolStripStatusLabel labInfo;
        private System.Windows.Forms.ToolStrip toolMain;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmbAccounts;
        private System.Windows.Forms.DataGridView dgvDomains;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolDomains;
        private System.Windows.Forms.ToolStripButton btnRefreshDomains;
        private System.Windows.Forms.ToolStripButton btnAddDomains;
        private System.Windows.Forms.ToolStripButton btnAuthorize;
        private System.Windows.Forms.ToolStripButton btnRegister;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripButton btnSelectAllDomains;
        private System.Windows.Forms.ToolStripButton btnReverseSelectDomain;
        private System.Windows.Forms.ToolStripButton btnEdit;
        private System.Windows.Forms.ToolStrip toolCertificate;
        private System.Windows.Forms.ToolStripButton btnCertificate;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtSavePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtCSR;
        private System.Windows.Forms.RadioButton radioCSRUseCustom;
        private System.Windows.Forms.RadioButton radioCSRAutoGen;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCSRCommonName;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

