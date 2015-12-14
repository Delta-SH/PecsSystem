namespace Delta.PECS.WebCSC.Licensing {
    partial class Keygen {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Keygen));
            this.authFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.MainTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.authCodeBox = new System.Windows.Forms.GroupBox();
            this.ContentTableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.authCodeLbl = new System.Windows.Forms.Label();
            this.userName = new System.Windows.Forms.TextBox();
            this.userNameLbl = new System.Windows.Forms.Label();
            this.companyNameLbl = new System.Windows.Forms.Label();
            this.companyName = new System.Windows.Forms.TextBox();
            this.periodTime = new System.Windows.Forms.DateTimePicker();
            this.periodTimeLbl = new System.Windows.Forms.Label();
            this.neverTimeCK = new System.Windows.Forms.CheckBox();
            this.machineCodeLbl = new System.Windows.Forms.Label();
            this.machineCode = new System.Windows.Forms.TextBox();
            this.authCode = new System.Windows.Forms.TextBox();
            this.userCntLbl = new System.Windows.Forms.Label();
            this.userCntNUD = new System.Windows.Forms.NumericUpDown();
            this.BottomPanel = new System.Windows.Forms.Panel();
            this.createFileBtn = new System.Windows.Forms.Button();
            this.generateBtn = new System.Windows.Forms.Button();
            this.copyBtn = new System.Windows.Forms.Button();
            this.MainTableLayoutPanel.SuspendLayout();
            this.authCodeBox.SuspendLayout();
            this.ContentTableLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userCntNUD)).BeginInit();
            this.BottomPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // authFileDialog
            // 
            this.authFileDialog.DefaultExt = "key";
            this.authFileDialog.FileName = "register.key";
            this.authFileDialog.Filter = "授权文件|*.key|所有文件|*.*";
            this.authFileDialog.Title = "文件另存为";
            // 
            // MainTableLayoutPanel
            // 
            this.MainTableLayoutPanel.ColumnCount = 1;
            this.MainTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.Controls.Add(this.authCodeBox, 0, 0);
            this.MainTableLayoutPanel.Controls.Add(this.BottomPanel, 0, 1);
            this.MainTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainTableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.MainTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.MainTableLayoutPanel.Name = "MainTableLayoutPanel";
            this.MainTableLayoutPanel.Padding = new System.Windows.Forms.Padding(10, 10, 10, 0);
            this.MainTableLayoutPanel.RowCount = 2;
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.MainTableLayoutPanel.Size = new System.Drawing.Size(414, 422);
            this.MainTableLayoutPanel.TabIndex = 1;
            // 
            // authCodeBox
            // 
            this.authCodeBox.Controls.Add(this.ContentTableLayoutPanel);
            this.authCodeBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.authCodeBox.Location = new System.Drawing.Point(10, 10);
            this.authCodeBox.Margin = new System.Windows.Forms.Padding(0);
            this.authCodeBox.Name = "authCodeBox";
            this.authCodeBox.Size = new System.Drawing.Size(394, 362);
            this.authCodeBox.TabIndex = 1;
            this.authCodeBox.TabStop = false;
            this.authCodeBox.Text = "注册码生成器";
            // 
            // ContentTableLayoutPanel
            // 
            this.ContentTableLayoutPanel.ColumnCount = 3;
            this.ContentTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 80F));
            this.ContentTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ContentTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.ContentTableLayoutPanel.Controls.Add(this.authCodeLbl, 0, 10);
            this.ContentTableLayoutPanel.Controls.Add(this.userName, 1, 0);
            this.ContentTableLayoutPanel.Controls.Add(this.userNameLbl, 0, 0);
            this.ContentTableLayoutPanel.Controls.Add(this.companyNameLbl, 0, 2);
            this.ContentTableLayoutPanel.Controls.Add(this.companyName, 1, 2);
            this.ContentTableLayoutPanel.Controls.Add(this.periodTime, 1, 4);
            this.ContentTableLayoutPanel.Controls.Add(this.periodTimeLbl, 0, 4);
            this.ContentTableLayoutPanel.Controls.Add(this.neverTimeCK, 2, 4);
            this.ContentTableLayoutPanel.Controls.Add(this.machineCodeLbl, 0, 8);
            this.ContentTableLayoutPanel.Controls.Add(this.machineCode, 1, 8);
            this.ContentTableLayoutPanel.Controls.Add(this.authCode, 1, 10);
            this.ContentTableLayoutPanel.Controls.Add(this.userCntLbl, 0, 6);
            this.ContentTableLayoutPanel.Controls.Add(this.userCntNUD, 1, 6);
            this.ContentTableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ContentTableLayoutPanel.Location = new System.Drawing.Point(3, 19);
            this.ContentTableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.ContentTableLayoutPanel.Name = "ContentTableLayoutPanel";
            this.ContentTableLayoutPanel.Padding = new System.Windows.Forms.Padding(10);
            this.ContentTableLayoutPanel.RowCount = 11;
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10F));
            this.ContentTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.ContentTableLayoutPanel.Size = new System.Drawing.Size(388, 340);
            this.ContentTableLayoutPanel.TabIndex = 0;
            // 
            // authCodeLbl
            // 
            this.authCodeLbl.Dock = System.Windows.Forms.DockStyle.Top;
            this.authCodeLbl.Location = new System.Drawing.Point(10, 185);
            this.authCodeLbl.Margin = new System.Windows.Forms.Padding(0);
            this.authCodeLbl.Name = "authCodeLbl";
            this.authCodeLbl.Size = new System.Drawing.Size(80, 22);
            this.authCodeLbl.TabIndex = 9;
            this.authCodeLbl.Text = "注册码(&R)";
            this.authCodeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // userName
            // 
            this.ContentTableLayoutPanel.SetColumnSpan(this.userName, 2);
            this.userName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userName.Location = new System.Drawing.Point(90, 10);
            this.userName.Margin = new System.Windows.Forms.Padding(0);
            this.userName.MaxLength = 50;
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(288, 23);
            this.userName.TabIndex = 2;
            this.userName.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // userNameLbl
            // 
            this.userNameLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userNameLbl.Location = new System.Drawing.Point(10, 10);
            this.userNameLbl.Margin = new System.Windows.Forms.Padding(0);
            this.userNameLbl.Name = "userNameLbl";
            this.userNameLbl.Size = new System.Drawing.Size(80, 25);
            this.userNameLbl.TabIndex = 1;
            this.userNameLbl.Text = "用户姓名(&U)";
            this.userNameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // companyNameLbl
            // 
            this.companyNameLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.companyNameLbl.Location = new System.Drawing.Point(10, 45);
            this.companyNameLbl.Margin = new System.Windows.Forms.Padding(0);
            this.companyNameLbl.Name = "companyNameLbl";
            this.companyNameLbl.Size = new System.Drawing.Size(80, 25);
            this.companyNameLbl.TabIndex = 3;
            this.companyNameLbl.Text = "所属公司(&L)";
            this.companyNameLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // companyName
            // 
            this.ContentTableLayoutPanel.SetColumnSpan(this.companyName, 2);
            this.companyName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.companyName.Location = new System.Drawing.Point(90, 45);
            this.companyName.Margin = new System.Windows.Forms.Padding(0);
            this.companyName.MaxLength = 50;
            this.companyName.Name = "companyName";
            this.companyName.Size = new System.Drawing.Size(288, 23);
            this.companyName.TabIndex = 4;
            this.companyName.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // periodTime
            // 
            this.periodTime.CustomFormat = "yyyy/MM/dd";
            this.periodTime.Dock = System.Windows.Forms.DockStyle.Fill;
            this.periodTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.periodTime.Location = new System.Drawing.Point(90, 80);
            this.periodTime.Margin = new System.Windows.Forms.Padding(0);
            this.periodTime.Name = "periodTime";
            this.periodTime.Size = new System.Drawing.Size(188, 23);
            this.periodTime.TabIndex = 6;
            this.periodTime.Value = new System.DateTime(2013, 1, 1, 0, 0, 0, 0);
            this.periodTime.ValueChanged += new System.EventHandler(this.periodTime_ValueChanged);
            // 
            // periodTimeLbl
            // 
            this.periodTimeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.periodTimeLbl.Location = new System.Drawing.Point(10, 80);
            this.periodTimeLbl.Margin = new System.Windows.Forms.Padding(0);
            this.periodTimeLbl.Name = "periodTimeLbl";
            this.periodTimeLbl.Size = new System.Drawing.Size(80, 25);
            this.periodTimeLbl.TabIndex = 5;
            this.periodTimeLbl.Text = "有效期(&P)";
            this.periodTimeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // neverTimeCK
            // 
            this.neverTimeCK.AutoSize = true;
            this.neverTimeCK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.neverTimeCK.Location = new System.Drawing.Point(283, 80);
            this.neverTimeCK.Margin = new System.Windows.Forms.Padding(5, 0, 0, 0);
            this.neverTimeCK.Name = "neverTimeCK";
            this.neverTimeCK.Size = new System.Drawing.Size(95, 25);
            this.neverTimeCK.TabIndex = 7;
            this.neverTimeCK.Text = "永不过期(&N)";
            this.neverTimeCK.UseVisualStyleBackColor = true;
            this.neverTimeCK.CheckedChanged += new System.EventHandler(this.neverTimeCK_CheckedChanged);
            // 
            // machineCodeLbl
            // 
            this.machineCodeLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.machineCodeLbl.Location = new System.Drawing.Point(10, 150);
            this.machineCodeLbl.Margin = new System.Windows.Forms.Padding(0);
            this.machineCodeLbl.Name = "machineCodeLbl";
            this.machineCodeLbl.Size = new System.Drawing.Size(80, 25);
            this.machineCodeLbl.TabIndex = 8;
            this.machineCodeLbl.Text = "机器码(&M)";
            this.machineCodeLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // machineCode
            // 
            this.ContentTableLayoutPanel.SetColumnSpan(this.machineCode, 2);
            this.machineCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.machineCode.Location = new System.Drawing.Point(90, 150);
            this.machineCode.Margin = new System.Windows.Forms.Padding(0);
            this.machineCode.MaxLength = 32;
            this.machineCode.Name = "machineCode";
            this.machineCode.Size = new System.Drawing.Size(288, 23);
            this.machineCode.TabIndex = 14;
            this.machineCode.TextChanged += new System.EventHandler(this.TextBox_TextChanged);
            // 
            // authCode
            // 
            this.ContentTableLayoutPanel.SetColumnSpan(this.authCode, 2);
            this.authCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.authCode.Location = new System.Drawing.Point(90, 185);
            this.authCode.Margin = new System.Windows.Forms.Padding(0);
            this.authCode.MaxLength = 50000;
            this.authCode.Multiline = true;
            this.authCode.Name = "authCode";
            this.authCode.ReadOnly = true;
            this.authCode.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.authCode.Size = new System.Drawing.Size(288, 145);
            this.authCode.TabIndex = 10;
            this.authCode.TextChanged += new System.EventHandler(this.authCode_TextChanged);
            // 
            // userCntLbl
            // 
            this.userCntLbl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userCntLbl.Location = new System.Drawing.Point(10, 115);
            this.userCntLbl.Margin = new System.Windows.Forms.Padding(0);
            this.userCntLbl.Name = "userCntLbl";
            this.userCntLbl.Size = new System.Drawing.Size(80, 25);
            this.userCntLbl.TabIndex = 5;
            this.userCntLbl.Text = "用户数量(&N)";
            this.userCntLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // userCntNUD
            // 
            this.userCntNUD.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userCntNUD.Location = new System.Drawing.Point(90, 115);
            this.userCntNUD.Margin = new System.Windows.Forms.Padding(0);
            this.userCntNUD.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.userCntNUD.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.userCntNUD.Name = "userCntNUD";
            this.userCntNUD.Size = new System.Drawing.Size(188, 23);
            this.userCntNUD.TabIndex = 15;
            this.userCntNUD.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.userCntNUD.ValueChanged += new System.EventHandler(this.userCntNUD_ValueChanged);
            // 
            // BottomPanel
            // 
            this.BottomPanel.Controls.Add(this.createFileBtn);
            this.BottomPanel.Controls.Add(this.generateBtn);
            this.BottomPanel.Controls.Add(this.copyBtn);
            this.BottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.BottomPanel.Location = new System.Drawing.Point(10, 372);
            this.BottomPanel.Margin = new System.Windows.Forms.Padding(0);
            this.BottomPanel.Name = "BottomPanel";
            this.BottomPanel.Size = new System.Drawing.Size(394, 50);
            this.BottomPanel.TabIndex = 2;
            // 
            // createFileBtn
            // 
            this.createFileBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.createFileBtn.Location = new System.Drawing.Point(184, 10);
            this.createFileBtn.Margin = new System.Windows.Forms.Padding(0);
            this.createFileBtn.Name = "createFileBtn";
            this.createFileBtn.Size = new System.Drawing.Size(100, 30);
            this.createFileBtn.TabIndex = 2;
            this.createFileBtn.Text = "导出授权(&E)";
            this.createFileBtn.UseVisualStyleBackColor = true;
            this.createFileBtn.Click += new System.EventHandler(this.createFileBtn_Click);
            // 
            // generateBtn
            // 
            this.generateBtn.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.generateBtn.Location = new System.Drawing.Point(294, 10);
            this.generateBtn.Margin = new System.Windows.Forms.Padding(0);
            this.generateBtn.Name = "generateBtn";
            this.generateBtn.Size = new System.Drawing.Size(100, 30);
            this.generateBtn.TabIndex = 3;
            this.generateBtn.Text = "生成注册码(&G)";
            this.generateBtn.UseVisualStyleBackColor = true;
            this.generateBtn.Click += new System.EventHandler(this.generateBtn_Click);
            // 
            // copyBtn
            // 
            this.copyBtn.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.copyBtn.Enabled = false;
            this.copyBtn.Location = new System.Drawing.Point(0, 10);
            this.copyBtn.Margin = new System.Windows.Forms.Padding(0);
            this.copyBtn.Name = "copyBtn";
            this.copyBtn.Size = new System.Drawing.Size(100, 30);
            this.copyBtn.TabIndex = 1;
            this.copyBtn.Text = "复制注册码(C)";
            this.copyBtn.UseVisualStyleBackColor = true;
            this.copyBtn.Click += new System.EventHandler(this.copyBtn_Click);
            // 
            // Keygen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(414, 422);
            this.Controls.Add(this.MainTableLayoutPanel);
            this.Font = new System.Drawing.Font("微软雅黑", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Keygen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "动力环境监控中心系统";
            this.Load += new System.EventHandler(this.Keygen_Load);
            this.MainTableLayoutPanel.ResumeLayout(false);
            this.authCodeBox.ResumeLayout(false);
            this.ContentTableLayoutPanel.ResumeLayout(false);
            this.ContentTableLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.userCntNUD)).EndInit();
            this.BottomPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog authFileDialog;
        private System.Windows.Forms.TableLayoutPanel MainTableLayoutPanel;
        private System.Windows.Forms.GroupBox authCodeBox;
        private System.Windows.Forms.TableLayoutPanel ContentTableLayoutPanel;
        private System.Windows.Forms.Label authCodeLbl;
        private System.Windows.Forms.TextBox userName;
        private System.Windows.Forms.Label userNameLbl;
        private System.Windows.Forms.Label companyNameLbl;
        private System.Windows.Forms.TextBox companyName;
        private System.Windows.Forms.DateTimePicker periodTime;
        private System.Windows.Forms.Label periodTimeLbl;
        private System.Windows.Forms.CheckBox neverTimeCK;
        private System.Windows.Forms.Label machineCodeLbl;
        private System.Windows.Forms.TextBox machineCode;
        private System.Windows.Forms.TextBox authCode;
        private System.Windows.Forms.Label userCntLbl;
        private System.Windows.Forms.NumericUpDown userCntNUD;
        private System.Windows.Forms.Panel BottomPanel;
        private System.Windows.Forms.Button createFileBtn;
        private System.Windows.Forms.Button generateBtn;
        private System.Windows.Forms.Button copyBtn;
    }
}