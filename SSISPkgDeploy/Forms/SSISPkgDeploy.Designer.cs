namespace SSISPkgDeploy
{
    partial class SSISPkgDeploy
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
            this.btnExit = new System.Windows.Forms.Button();
            this.lblErrorMsg = new System.Windows.Forms.Label();
            this.btnDeployPackage = new System.Windows.Forms.Button();
            this.txtEnvType = new System.Windows.Forms.TextBox();
            this.lblEnvType = new System.Windows.Forms.Label();
            this.txtConnStr = new System.Windows.Forms.TextBox();
            this.lblConnStr = new System.Windows.Forms.Label();
            this.txtErrMsg = new System.Windows.Forms.TextBox();
            this.txtSSISCatalog = new System.Windows.Forms.TextBox();
            this.lblSSISCatalog = new System.Windows.Forms.Label();
            this.txtSSISFolderName = new System.Windows.Forms.TextBox();
            this.lblSSISFolderName = new System.Windows.Forms.Label();
            this.txtSSISFolderDescription = new System.Windows.Forms.TextBox();
            this.lblSSISFolderDescription = new System.Windows.Forms.Label();
            this.txtSSISProjectName = new System.Windows.Forms.TextBox();
            this.lblSSISProjectName = new System.Windows.Forms.Label();
            this.txtSSISProjectFilename = new System.Windows.Forms.TextBox();
            this.lblSSISProjectFilename = new System.Windows.Forms.Label();
            this.txtSQLAgentJobScript = new System.Windows.Forms.TextBox();
            this.lblSQLAgentJobScript = new System.Windows.Forms.Label();
            this.txtSQLAgentJobConnStr = new System.Windows.Forms.TextBox();
            this.lblSQLAgentJobConnStr = new System.Windows.Forms.Label();
            this.lblMapProjParamsToEnvVar = new System.Windows.Forms.Label();
            this.cbMultiEnvPerCatalog = new System.Windows.Forms.CheckBox();
            this.lblMultiEnvPerCatalog = new System.Windows.Forms.Label();
            this.txtSSISProxyName = new System.Windows.Forms.TextBox();
            this.lblSSISProxyName = new System.Windows.Forms.Label();
            this.cbMapProjParamsToEnvVar = new System.Windows.Forms.CheckBox();
            this.ofdDeployFiles = new System.Windows.Forms.OpenFileDialog();
            this.txtXMLFile = new System.Windows.Forms.TextBox();
            this.lblXMLFile = new System.Windows.Forms.Label();
            this.txtCredName = new System.Windows.Forms.TextBox();
            this.lblCredName = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(737, 625);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(60, 35);
            this.btnExit.TabIndex = 30;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // lblErrorMsg
            // 
            this.lblErrorMsg.AutoSize = true;
            this.lblErrorMsg.Location = new System.Drawing.Point(8, 431);
            this.lblErrorMsg.Name = "lblErrorMsg";
            this.lblErrorMsg.Size = new System.Drawing.Size(121, 20);
            this.lblErrorMsg.TabIndex = 27;
            this.lblErrorMsg.Text = "Error Messages";
            // 
            // btnDeployPackage
            // 
            this.btnDeployPackage.Location = new System.Drawing.Point(662, 416);
            this.btnDeployPackage.Name = "btnDeployPackage";
            this.btnDeployPackage.Size = new System.Drawing.Size(135, 35);
            this.btnDeployPackage.TabIndex = 28;
            this.btnDeployPackage.Text = "Deploy Package";
            this.btnDeployPackage.UseVisualStyleBackColor = true;
            this.btnDeployPackage.Click += new System.EventHandler(this.btnDeployPackage_Click);
            // 
            // txtEnvType
            // 
            this.txtEnvType.BackColor = System.Drawing.SystemColors.Control;
            this.txtEnvType.Enabled = false;
            this.txtEnvType.Location = new System.Drawing.Point(145, 42);
            this.txtEnvType.Name = "txtEnvType";
            this.txtEnvType.Size = new System.Drawing.Size(135, 26);
            this.txtEnvType.TabIndex = 4;
            // 
            // lblEnvType
            // 
            this.lblEnvType.AutoSize = true;
            this.lblEnvType.Location = new System.Drawing.Point(9, 45);
            this.lblEnvType.Name = "lblEnvType";
            this.lblEnvType.Size = new System.Drawing.Size(140, 20);
            this.lblEnvType.TabIndex = 3;
            this.lblEnvType.Text = "Environment Type:";
            // 
            // txtConnStr
            // 
            this.txtConnStr.BackColor = System.Drawing.SystemColors.Control;
            this.txtConnStr.Enabled = false;
            this.txtConnStr.Location = new System.Drawing.Point(151, 106);
            this.txtConnStr.Name = "txtConnStr";
            this.txtConnStr.Size = new System.Drawing.Size(647, 26);
            this.txtConnStr.TabIndex = 12;
            // 
            // lblConnStr
            // 
            this.lblConnStr.AutoSize = true;
            this.lblConnStr.Location = new System.Drawing.Point(9, 109);
            this.lblConnStr.Name = "lblConnStr";
            this.lblConnStr.Size = new System.Drawing.Size(140, 20);
            this.lblConnStr.TabIndex = 11;
            this.lblConnStr.Text = "Connection String:";
            // 
            // txtErrMsg
            // 
            this.txtErrMsg.Location = new System.Drawing.Point(12, 455);
            this.txtErrMsg.Multiline = true;
            this.txtErrMsg.Name = "txtErrMsg";
            this.txtErrMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtErrMsg.Size = new System.Drawing.Size(785, 164);
            this.txtErrMsg.TabIndex = 29;
            // 
            // txtSSISCatalog
            // 
            this.txtSSISCatalog.BackColor = System.Drawing.SystemColors.Control;
            this.txtSSISCatalog.Enabled = false;
            this.txtSSISCatalog.Location = new System.Drawing.Point(388, 42);
            this.txtSSISCatalog.Name = "txtSSISCatalog";
            this.txtSSISCatalog.Size = new System.Drawing.Size(75, 26);
            this.txtSSISCatalog.TabIndex = 6;
            // 
            // lblSSISCatalog
            // 
            this.lblSSISCatalog.AutoSize = true;
            this.lblSSISCatalog.Location = new System.Drawing.Point(282, 45);
            this.lblSSISCatalog.Name = "lblSSISCatalog";
            this.lblSSISCatalog.Size = new System.Drawing.Size(110, 20);
            this.lblSSISCatalog.TabIndex = 5;
            this.lblSSISCatalog.Text = "SSIS Catalog:";
            // 
            // txtSSISFolderName
            // 
            this.txtSSISFolderName.BackColor = System.Drawing.SystemColors.Control;
            this.txtSSISFolderName.Enabled = false;
            this.txtSSISFolderName.Location = new System.Drawing.Point(151, 138);
            this.txtSSISFolderName.Name = "txtSSISFolderName";
            this.txtSSISFolderName.Size = new System.Drawing.Size(646, 26);
            this.txtSSISFolderName.TabIndex = 14;
            // 
            // lblSSISFolderName
            // 
            this.lblSSISFolderName.AutoSize = true;
            this.lblSSISFolderName.Location = new System.Drawing.Point(8, 141);
            this.lblSSISFolderName.Name = "lblSSISFolderName";
            this.lblSSISFolderName.Size = new System.Drawing.Size(146, 20);
            this.lblSSISFolderName.TabIndex = 13;
            this.lblSSISFolderName.Text = "SSIS Folder Name:";
            // 
            // txtSSISFolderDescription
            // 
            this.txtSSISFolderDescription.BackColor = System.Drawing.SystemColors.Control;
            this.txtSSISFolderDescription.Enabled = false;
            this.txtSSISFolderDescription.Location = new System.Drawing.Point(192, 74);
            this.txtSSISFolderDescription.Name = "txtSSISFolderDescription";
            this.txtSSISFolderDescription.Size = new System.Drawing.Size(606, 26);
            this.txtSSISFolderDescription.TabIndex = 10;
            // 
            // lblSSISFolderDescription
            // 
            this.lblSSISFolderDescription.AutoSize = true;
            this.lblSSISFolderDescription.Location = new System.Drawing.Point(9, 77);
            this.lblSSISFolderDescription.Name = "lblSSISFolderDescription";
            this.lblSSISFolderDescription.Size = new System.Drawing.Size(184, 20);
            this.lblSSISFolderDescription.TabIndex = 9;
            this.lblSSISFolderDescription.Text = "SSIS Folder Description:";
            // 
            // txtSSISProjectName
            // 
            this.txtSSISProjectName.BackColor = System.Drawing.SystemColors.Control;
            this.txtSSISProjectName.Enabled = false;
            this.txtSSISProjectName.Location = new System.Drawing.Point(161, 173);
            this.txtSSISProjectName.Name = "txtSSISProjectName";
            this.txtSSISProjectName.Size = new System.Drawing.Size(637, 26);
            this.txtSSISProjectName.TabIndex = 16;
            // 
            // lblSSISProjectName
            // 
            this.lblSSISProjectName.AutoSize = true;
            this.lblSSISProjectName.Location = new System.Drawing.Point(9, 176);
            this.lblSSISProjectName.Name = "lblSSISProjectName";
            this.lblSSISProjectName.Size = new System.Drawing.Size(150, 20);
            this.lblSSISProjectName.TabIndex = 15;
            this.lblSSISProjectName.Text = "SSIS Project Name:";
            // 
            // txtSSISProjectFilename
            // 
            this.txtSSISProjectFilename.BackColor = System.Drawing.SystemColors.Control;
            this.txtSSISProjectFilename.Enabled = false;
            this.txtSSISProjectFilename.Location = new System.Drawing.Point(183, 275);
            this.txtSSISProjectFilename.Name = "txtSSISProjectFilename";
            this.txtSSISProjectFilename.Size = new System.Drawing.Size(615, 26);
            this.txtSSISProjectFilename.TabIndex = 20;
            // 
            // lblSSISProjectFilename
            // 
            this.lblSSISProjectFilename.AutoSize = true;
            this.lblSSISProjectFilename.Location = new System.Drawing.Point(9, 278);
            this.lblSSISProjectFilename.Name = "lblSSISProjectFilename";
            this.lblSSISProjectFilename.Size = new System.Drawing.Size(172, 20);
            this.lblSSISProjectFilename.TabIndex = 19;
            this.lblSSISProjectFilename.Text = "SSIS ISPAC Filename:";
            // 
            // txtSQLAgentJobScript
            // 
            this.txtSQLAgentJobScript.BackColor = System.Drawing.SystemColors.Control;
            this.txtSQLAgentJobScript.Enabled = false;
            this.txtSQLAgentJobScript.Location = new System.Drawing.Point(175, 346);
            this.txtSQLAgentJobScript.Name = "txtSQLAgentJobScript";
            this.txtSQLAgentJobScript.Size = new System.Drawing.Size(623, 26);
            this.txtSQLAgentJobScript.TabIndex = 24;
            // 
            // lblSQLAgentJobScript
            // 
            this.lblSQLAgentJobScript.AutoSize = true;
            this.lblSQLAgentJobScript.Location = new System.Drawing.Point(9, 349);
            this.lblSQLAgentJobScript.Name = "lblSQLAgentJobScript";
            this.lblSQLAgentJobScript.Size = new System.Drawing.Size(167, 20);
            this.lblSQLAgentJobScript.TabIndex = 23;
            this.lblSQLAgentJobScript.Text = "SQL Agent Job Script:";
            // 
            // txtSQLAgentJobConnStr
            // 
            this.txtSQLAgentJobConnStr.BackColor = System.Drawing.SystemColors.Control;
            this.txtSQLAgentJobConnStr.Enabled = false;
            this.txtSQLAgentJobConnStr.Location = new System.Drawing.Point(213, 380);
            this.txtSQLAgentJobConnStr.Name = "txtSQLAgentJobConnStr";
            this.txtSQLAgentJobConnStr.Size = new System.Drawing.Size(586, 26);
            this.txtSQLAgentJobConnStr.TabIndex = 26;
            // 
            // lblSQLAgentJobConnStr
            // 
            this.lblSQLAgentJobConnStr.AutoSize = true;
            this.lblSQLAgentJobConnStr.Location = new System.Drawing.Point(9, 383);
            this.lblSQLAgentJobConnStr.Name = "lblSQLAgentJobConnStr";
            this.lblSQLAgentJobConnStr.Size = new System.Drawing.Size(206, 20);
            this.lblSQLAgentJobConnStr.TabIndex = 25;
            this.lblSQLAgentJobConnStr.Text = "SQL Job Connection String:";
            // 
            // lblMapProjParamsToEnvVar
            // 
            this.lblMapProjParamsToEnvVar.AutoSize = true;
            this.lblMapProjParamsToEnvVar.Location = new System.Drawing.Point(31, 315);
            this.lblMapProjParamsToEnvVar.Name = "lblMapProjParamsToEnvVar";
            this.lblMapProjParamsToEnvVar.Size = new System.Drawing.Size(507, 20);
            this.lblMapProjParamsToEnvVar.TabIndex = 22;
            this.lblMapProjParamsToEnvVar.Text = "Run Script Used To Map Project Parameters To Environment Variables";
            // 
            // cbMultiEnvPerCatalog
            // 
            this.cbMultiEnvPerCatalog.AutoSize = true;
            this.cbMultiEnvPerCatalog.Enabled = false;
            this.cbMultiEnvPerCatalog.Location = new System.Drawing.Point(469, 49);
            this.cbMultiEnvPerCatalog.Name = "cbMultiEnvPerCatalog";
            this.cbMultiEnvPerCatalog.Size = new System.Drawing.Size(15, 14);
            this.cbMultiEnvPerCatalog.TabIndex = 7;
            this.cbMultiEnvPerCatalog.UseVisualStyleBackColor = true;
            // 
            // lblMultiEnvPerCatalog
            // 
            this.lblMultiEnvPerCatalog.AutoSize = true;
            this.lblMultiEnvPerCatalog.Location = new System.Drawing.Point(483, 45);
            this.lblMultiEnvPerCatalog.Name = "lblMultiEnvPerCatalog";
            this.lblMultiEnvPerCatalog.Size = new System.Drawing.Size(316, 20);
            this.lblMultiEnvPerCatalog.TabIndex = 8;
            this.lblMultiEnvPerCatalog.Text = "Multiple Environments Per SSISDB Catalog";
            // 
            // txtSSISProxyName
            // 
            this.txtSSISProxyName.BackColor = System.Drawing.SystemColors.Control;
            this.txtSSISProxyName.Enabled = false;
            this.txtSSISProxyName.Location = new System.Drawing.Point(148, 240);
            this.txtSSISProxyName.Name = "txtSSISProxyName";
            this.txtSSISProxyName.Size = new System.Drawing.Size(651, 26);
            this.txtSSISProxyName.TabIndex = 18;
            // 
            // lblSSISProxyName
            // 
            this.lblSSISProxyName.AutoSize = true;
            this.lblSSISProxyName.Location = new System.Drawing.Point(10, 243);
            this.lblSSISProxyName.Name = "lblSSISProxyName";
            this.lblSSISProxyName.Size = new System.Drawing.Size(139, 20);
            this.lblSSISProxyName.TabIndex = 17;
            this.lblSSISProxyName.Text = "SSIS Proxy Name:";
            // 
            // cbMapProjParamsToEnvVar
            // 
            this.cbMapProjParamsToEnvVar.AutoSize = true;
            this.cbMapProjParamsToEnvVar.Enabled = false;
            this.cbMapProjParamsToEnvVar.Location = new System.Drawing.Point(14, 319);
            this.cbMapProjParamsToEnvVar.Name = "cbMapProjParamsToEnvVar";
            this.cbMapProjParamsToEnvVar.Size = new System.Drawing.Size(15, 14);
            this.cbMapProjParamsToEnvVar.TabIndex = 21;
            this.cbMapProjParamsToEnvVar.UseVisualStyleBackColor = true;
            // 
            // ofdDeployFiles
            // 
            this.ofdDeployFiles.FileName = "ofdDeployFiles";
            // 
            // txtXMLFile
            // 
            this.txtXMLFile.BackColor = System.Drawing.SystemColors.Control;
            this.txtXMLFile.Enabled = false;
            this.txtXMLFile.Location = new System.Drawing.Point(210, 8);
            this.txtXMLFile.Name = "txtXMLFile";
            this.txtXMLFile.Size = new System.Drawing.Size(589, 26);
            this.txtXMLFile.TabIndex = 2;
            // 
            // lblXMLFile
            // 
            this.lblXMLFile.AutoSize = true;
            this.lblXMLFile.Location = new System.Drawing.Point(8, 11);
            this.lblXMLFile.Name = "lblXMLFile";
            this.lblXMLFile.Size = new System.Drawing.Size(204, 20);
            this.lblXMLFile.TabIndex = 1;
            this.lblXMLFile.Text = "XML Deployment Filename:";
            // 
            // txtCredName
            // 
            this.txtCredName.BackColor = System.Drawing.SystemColors.Control;
            this.txtCredName.Enabled = false;
            this.txtCredName.Location = new System.Drawing.Point(145, 206);
            this.txtCredName.Name = "txtCredName";
            this.txtCredName.Size = new System.Drawing.Size(654, 26);
            this.txtCredName.TabIndex = 32;
            // 
            // lblCredName
            // 
            this.lblCredName.AutoSize = true;
            this.lblCredName.Location = new System.Drawing.Point(10, 209);
            this.lblCredName.Name = "lblCredName";
            this.lblCredName.Size = new System.Drawing.Size(131, 20);
            this.lblCredName.TabIndex = 31;
            this.lblCredName.Text = "Credential Name:";
            // 
            // SSISPkgDeploy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 667);
            this.Controls.Add(this.txtCredName);
            this.Controls.Add(this.lblCredName);
            this.Controls.Add(this.txtXMLFile);
            this.Controls.Add(this.lblXMLFile);
            this.Controls.Add(this.cbMapProjParamsToEnvVar);
            this.Controls.Add(this.txtSSISProxyName);
            this.Controls.Add(this.lblSSISProxyName);
            this.Controls.Add(this.lblMultiEnvPerCatalog);
            this.Controls.Add(this.cbMultiEnvPerCatalog);
            this.Controls.Add(this.lblMapProjParamsToEnvVar);
            this.Controls.Add(this.txtSQLAgentJobConnStr);
            this.Controls.Add(this.lblSQLAgentJobConnStr);
            this.Controls.Add(this.txtSQLAgentJobScript);
            this.Controls.Add(this.lblSQLAgentJobScript);
            this.Controls.Add(this.txtSSISProjectFilename);
            this.Controls.Add(this.lblSSISProjectFilename);
            this.Controls.Add(this.txtSSISProjectName);
            this.Controls.Add(this.lblSSISProjectName);
            this.Controls.Add(this.txtSSISFolderDescription);
            this.Controls.Add(this.lblSSISFolderDescription);
            this.Controls.Add(this.txtSSISFolderName);
            this.Controls.Add(this.lblSSISFolderName);
            this.Controls.Add(this.txtSSISCatalog);
            this.Controls.Add(this.lblSSISCatalog);
            this.Controls.Add(this.txtConnStr);
            this.Controls.Add(this.lblConnStr);
            this.Controls.Add(this.txtEnvType);
            this.Controls.Add(this.lblEnvType);
            this.Controls.Add(this.btnDeployPackage);
            this.Controls.Add(this.lblErrorMsg);
            this.Controls.Add(this.txtErrMsg);
            this.Controls.Add(this.btnExit);
            this.Margin = new System.Windows.Forms.Padding(3);
            this.Name = "SSISPkgDeploy";
            this.Text = "SSIS Package Deployment";
            this.Load += new System.EventHandler(this.SSISPkgDeploy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblErrorMsg;
        private System.Windows.Forms.Button btnDeployPackage;
        private System.Windows.Forms.TextBox txtEnvType;
        private System.Windows.Forms.Label lblEnvType;
        private System.Windows.Forms.TextBox txtConnStr;
        private System.Windows.Forms.Label lblConnStr;
        private System.Windows.Forms.TextBox txtErrMsg;
        private System.Windows.Forms.TextBox txtSSISCatalog;
        private System.Windows.Forms.Label lblSSISCatalog;
        private System.Windows.Forms.TextBox txtSSISFolderName;
        private System.Windows.Forms.Label lblSSISFolderName;
        private System.Windows.Forms.TextBox txtSSISFolderDescription;
        private System.Windows.Forms.Label lblSSISFolderDescription;
        private System.Windows.Forms.TextBox txtSSISProjectName;
        private System.Windows.Forms.Label lblSSISProjectName;
        private System.Windows.Forms.TextBox txtSSISProjectFilename;
        private System.Windows.Forms.Label lblSSISProjectFilename;
        private System.Windows.Forms.TextBox txtSQLAgentJobScript;
        private System.Windows.Forms.Label lblSQLAgentJobScript;
        private System.Windows.Forms.TextBox txtSQLAgentJobConnStr;
        private System.Windows.Forms.Label lblSQLAgentJobConnStr;
        private System.Windows.Forms.Label lblMapProjParamsToEnvVar;
        private System.Windows.Forms.CheckBox cbMultiEnvPerCatalog;
        private System.Windows.Forms.Label lblMultiEnvPerCatalog;
        private System.Windows.Forms.TextBox txtSSISProxyName;
        private System.Windows.Forms.Label lblSSISProxyName;
        private System.Windows.Forms.CheckBox cbMapProjParamsToEnvVar;
        private System.Windows.Forms.OpenFileDialog ofdDeployFiles;
        private System.Windows.Forms.TextBox txtXMLFile;
        private System.Windows.Forms.Label lblXMLFile;
        private System.Windows.Forms.TextBox txtCredName;
        private System.Windows.Forms.Label lblCredName;
    }
}

