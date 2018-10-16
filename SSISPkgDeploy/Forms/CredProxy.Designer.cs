namespace SSISPkgDeploy
{
    partial class CredProxy
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
            this.txtCredName = new System.Windows.Forms.TextBox();
            this.lblCredName = new System.Windows.Forms.Label();
            this.txtSSISProxyName = new System.Windows.Forms.TextBox();
            this.lblSSISProxyName = new System.Windows.Forms.Label();
            this.ttCredProxy = new System.Windows.Forms.ToolTip(this.components);
            this.txtIdentity = new System.Windows.Forms.TextBox();
            this.lblIdentity = new System.Windows.Forms.Label();
            this.txtSecret = new System.Windows.Forms.TextBox();
            this.lblSecret = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtCredName
            // 
            this.txtCredName.BackColor = System.Drawing.SystemColors.Control;
            this.txtCredName.Enabled = false;
            this.txtCredName.Location = new System.Drawing.Point(137, 7);
            this.txtCredName.Name = "txtCredName";
            this.txtCredName.Size = new System.Drawing.Size(340, 26);
            this.txtCredName.TabIndex = 36;
            // 
            // lblCredName
            // 
            this.lblCredName.AutoSize = true;
            this.lblCredName.Location = new System.Drawing.Point(7, 10);
            this.lblCredName.Name = "lblCredName";
            this.lblCredName.Size = new System.Drawing.Size(131, 20);
            this.lblCredName.TabIndex = 35;
            this.lblCredName.Text = "Credential Name:";
            // 
            // txtSSISProxyName
            // 
            this.txtSSISProxyName.BackColor = System.Drawing.SystemColors.Control;
            this.txtSSISProxyName.Enabled = false;
            this.txtSSISProxyName.Location = new System.Drawing.Point(144, 41);
            this.txtSSISProxyName.Name = "txtSSISProxyName";
            this.txtSSISProxyName.Size = new System.Drawing.Size(333, 26);
            this.txtSSISProxyName.TabIndex = 34;
            // 
            // lblSSISProxyName
            // 
            this.lblSSISProxyName.AutoSize = true;
            this.lblSSISProxyName.Location = new System.Drawing.Point(7, 44);
            this.lblSSISProxyName.Name = "lblSSISProxyName";
            this.lblSSISProxyName.Size = new System.Drawing.Size(139, 20);
            this.lblSSISProxyName.TabIndex = 33;
            this.lblSSISProxyName.Text = "SSIS Proxy Name:";
            // 
            // txtIdentity
            // 
            this.txtIdentity.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtIdentity.Location = new System.Drawing.Point(70, 77);
            this.txtIdentity.Name = "txtIdentity";
            this.txtIdentity.Size = new System.Drawing.Size(407, 26);
            this.txtIdentity.TabIndex = 38;
            // 
            // lblIdentity
            // 
            this.lblIdentity.AutoSize = true;
            this.lblIdentity.Location = new System.Drawing.Point(7, 80);
            this.lblIdentity.Name = "lblIdentity";
            this.lblIdentity.Size = new System.Drawing.Size(65, 20);
            this.lblIdentity.TabIndex = 37;
            this.lblIdentity.Text = "Identity:";
            // 
            // txtSecret
            // 
            this.txtSecret.BackColor = System.Drawing.SystemColors.HighlightText;
            this.txtSecret.Location = new System.Drawing.Point(69, 112);
            this.txtSecret.Name = "txtSecret";
            this.txtSecret.Size = new System.Drawing.Size(407, 26);
            this.txtSecret.TabIndex = 40;
            // 
            // lblSecret
            // 
            this.lblSecret.AutoSize = true;
            this.lblSecret.Location = new System.Drawing.Point(6, 115);
            this.lblSecret.Name = "lblSecret";
            this.lblSecret.Size = new System.Drawing.Size(60, 20);
            this.lblSecret.TabIndex = 39;
            this.lblSecret.Text = "Secret:";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(437, 144);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(40, 35);
            this.btnOK.TabIndex = 41;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // CredProxy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 182);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtSecret);
            this.Controls.Add(this.lblSecret);
            this.Controls.Add(this.txtIdentity);
            this.Controls.Add(this.lblIdentity);
            this.Controls.Add(this.txtCredName);
            this.Controls.Add(this.lblCredName);
            this.Controls.Add(this.txtSSISProxyName);
            this.Controls.Add(this.lblSSISProxyName);
            this.Name = "CredProxy";
            this.Text = "Enter Identity & Secret For Credential Setup";
            this.Load += new System.EventHandler(this.CredProxy_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtCredName;
        private System.Windows.Forms.Label lblCredName;
        private System.Windows.Forms.TextBox txtSSISProxyName;
        private System.Windows.Forms.Label lblSSISProxyName;
        private System.Windows.Forms.ToolTip ttCredProxy;
        private System.Windows.Forms.TextBox txtIdentity;
        private System.Windows.Forms.Label lblIdentity;
        private System.Windows.Forms.TextBox txtSecret;
        private System.Windows.Forms.Label lblSecret;
        private System.Windows.Forms.Button btnOK;
    }
}