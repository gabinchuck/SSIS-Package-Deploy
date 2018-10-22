using System;
using System.Windows.Forms;

namespace SSISPkgDeploy
{
    public partial class CredProxy : BaseForm
    {
        #region "PRIVATE VARIABLES"

        private string errorMsg = string.Empty;
        private string envType = string.Empty;
        private bool createCred = false;

        #endregion

        #region "PUBLIC PROPERTIES"

        public string Identity { get { return txtIdentity.Text; } }
        public string Secret { get { return txtSecret.Text; } }

        #endregion

        public CredProxy(string sEnvType, string sCredentialName, string sSSISProxyName, bool bCreateCred)
        {
            InitializeComponent();
            envType = sEnvType;
            txtCredName.Text = sCredentialName;
            txtSSISProxyName.Text = sSSISProxyName;
            createCred = bCreateCred;
        }

        private void CredProxy_Load(object sender, EventArgs e)
        {
            try
            {
                // Set up the delays for the ToolTip.
                ttCredProxy.AutoPopDelay = 20000;
                ttCredProxy.InitialDelay = 500;
                ttCredProxy.ReshowDelay = 500;
                // Force the ToolTip text to be displayed whether or not the form is active.
                ttCredProxy.ShowAlways = true;

                this.Text = "Enter Identity & Secret For Credential/Proxy Setup (" + envType + ")";
                string ttIdentity = "Identity (User ID) Used To Create The Credential.\r\n" + @"Include Domain And User ID (e.g. 'MYDOMAIN\myuserid').";
                string ttSecret = @"Secret (Password) Used To Create The Credential (e.g. 'vQXU4U%$Vb').";
                ttCredProxy.SetToolTip(this.lblCredName, "Name Of The Credential Used For SSIS Package Execution.");
                ttCredProxy.SetToolTip(this.lblSSISProxyName, "Name Of The Proxy Used For SSIS Package Execution.");
                ttCredProxy.SetToolTip(this.lblIdentity, ttIdentity);
                ttCredProxy.SetToolTip(this.txtIdentity, ttIdentity);
                ttCredProxy.SetToolTip(this.lblSecret, ttSecret);
                ttCredProxy.SetToolTip(this.txtSecret, ttSecret);

                // Only need the secret if creating a credential.
                if (createCred == false)
                {
                    lblSecret.Enabled = false;
                    txtSecret.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message + (ex.InnerException != null ? "\r\n\r\nInner Exception: " + ex.InnerException.Message : "");
                MessageBox.Show(
                    errorMsg,
                    "Error Entering Identity/Secret!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        } // private void CredProxy_Load(object sender, EventArgs e)

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtIdentity.Text.Trim() == string.Empty)
            {
                MessageBox.Show(
                    "A value for Identity (User ID) must be entered.",
                    "Enter a value for Identity (User ID)!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                txtIdentity.Focus();
                return;
            }
            if (createCred == true && txtSecret.Text.Trim() == string.Empty)
            {
                MessageBox.Show(
                    "A value for Secret (Password) must be entered.",
                    "Enter a value for Secret (Password)!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                txtSecret.Focus();
                return;
            }
            this.Close();
        } // private void btnOK_Click(object sender, EventArgs e)

    } // public partial class CredProxy : BaseForm
} // namespace SSISPkgDeploy
