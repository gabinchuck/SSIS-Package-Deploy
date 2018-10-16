using System;
using System.Windows.Forms;
using System.Text;
using System.Drawing;
using System.Configuration;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Xml.Schema;
using Microsoft.SqlServer.Management.IntegrationServices;

namespace SSISPkgDeploy
{
    public partial class SSISPkgDeploy : BaseForm
    {
        public SSISPkgDeploy()
        {
            InitializeComponent();
        }

        #region "PRIVATE VARIABLES"

        private StringBuilder sbMsg = new StringBuilder();
        private string sErrorMsg = string.Empty;
        private string sEnvironmentType = string.Empty;
        private string sEnvironmentList = string.Empty;
        private string[] sEnvironmentTypes = new String[0];
        private string sEnvironmentName = string.Empty;
        private string sEnvironmentServerName = string.Empty;
        private string sSSISCatalog = string.Empty;
        private bool bMultiEnvPerCatalog = false;
        private string sSSISFolderName = string.Empty;
        private string sSSISFolderDescription = string.Empty;
        private string sSSISProjectName = string.Empty;
        private string sCredentialName = string.Empty;
        private string sSSISProxyName = string.Empty;
        private string sIdentity = string.Empty;
        private string sSecret = string.Empty;
        private string sSSISProjectFilename = string.Empty;
        private string sAppPath = string.Empty;
        private string sInitialDirectory = string.Empty;
        private string sDeployFilePath = string.Empty;
        private bool bMapProjParamsToEnvVar = false;
        private string sSQLAgentJobScript = string.Empty;
        private string[] sSQLAgentJobScriptList = new String[0];
        private StringBuilder sbSQLAgentJobScriptText;
        private string sqlConnPkgDeply = string.Empty;
        private string sqlConnJobScript = string.Empty;
        private string sLogFileName = string.Empty;
        private DeploymentXML deployXML;
        private string sXSDValidationFile = string.Empty;
        private int index;
        private int envCnt;

        private string sDeployXMLFile = string.Empty;

        // Create the ToolTip and associate with the Form container.
        private ToolTip ttPkgDeploy = new ToolTip();

        #endregion

        private void SSISPkgDeploy_Load(object sender, EventArgs e)
        {
            try
            {
                // Set up the delays for the ToolTip.
                ttPkgDeploy.AutoPopDelay = 20000;
                ttPkgDeploy.InitialDelay = 500;
                ttPkgDeploy.ReshowDelay = 500;
                // Force the ToolTip text to be displayed whether or not the form is active.
                ttPkgDeploy.ShowAlways = true;

                sAppPath = System.IO.Path.GetDirectoryName(Application.ExecutablePath) + @"\";
                sDeployXMLFile = ConfigurationManager.AppSettings["DeployXMLFile"].Trim();
                sInitialDirectory = ConfigurationManager.AppSettings["InitialDirectory"].Trim();
                sDeployFilePath = sInitialDirectory;

                if (File.Exists(sDeployFilePath + sDeployXMLFile) == false)
                {
                    SelectDeployXML();
                }

                sXSDValidationFile = ConfigurationManager.AppSettings["XSDValidationFile"].Trim();
                // Check to see if the XSD Valication File exists.
                if (File.Exists(sAppPath + sXSDValidationFile) == false)
                {
                    throw new FileNotFoundException("XSD Validation File: " + sAppPath + sXSDValidationFile + " does NOT exist!");
                }

                XmlDocument doc = new XmlDocument();
                doc.Load(sDeployFilePath + sDeployXMLFile);
                doc.Schemas.Add(null, sAppPath + sXSDValidationFile);
                try
                {
                    doc.Validate(null);
                }
                catch (XmlSchemaValidationException ex)
                {
                    throw new XmlSchemaValidationException("Error validating XML schema for file: " + sAppPath + sXSDValidationFile + "./r/n" + ex.Message + (ex.InnerException != null ? "\r\n\r\nInner Exception: " + ex.InnerException.Message : ""));
                }
                deployXML = new DeploymentXML(doc);

                StringBuilder sbEnvServList = new StringBuilder();
                sEnvironmentList = deployXML.EnvironmentList.Trim();
                sEnvironmentTypes = sEnvironmentList.Split('|');
                sSSISCatalog = deployXML.SSISCatalog.Trim();
                bMultiEnvPerCatalog = deployXML.MultiEnvPerCatalog;
                sSSISFolderName = deployXML.SSISFolderName.Trim();
                sSSISFolderDescription = deployXML.SSISFolderDescription.Trim();
                sSSISProjectName = deployXML.SSISProjectName.Trim();
                sCredentialName = deployXML.CreateCredential == true ? deployXML.CredentialName.Trim() : string.Empty;
                sSSISProxyName = deployXML.UseSSISProxyName == true ? deployXML.SSISProxyName.Trim() : string.Empty;
                sSSISProjectFilename = deployXML.UseSSISProjectFilename == true ? deployXML.SSISProjectFilename.Trim() : string.Empty;
                bMapProjParamsToEnvVar = deployXML.MapProjParamsToEnvVar;
                sSQLAgentJobScript = deployXML.UseSQLAgentJobScript == true ? deployXML.SQLAgentJobScript.Trim() : string.Empty;
                if (sSQLAgentJobScript != string.Empty)
                    sSQLAgentJobScriptList = sSQLAgentJobScript.Split('|');
                sLogFileName = ConfigurationManager.AppSettings["LogFileName"].Trim();

                // Validate that all the Environment Types exist in deployXML.EnvironmentServerNames List<>:
                foreach (string env in sEnvironmentTypes)
                {
                    index = deployXML.EnvironmentServerNames.FindIndex(x => x.Name == env);
                    if (index >= 0)
                    {
                        sbEnvServList.Append(env + ", " + deployXML.EnvironmentServerNames[index].Server + " | ");
                    }
                    else
                    {
                        throw new Exception("Environment Type: " + env + " does NOT exist in the EnvironmentServerNames XML node!");
                    }
                }

                // Populate text controls from the config file:
                txtXMLFile.Text = sDeployFilePath + sDeployXMLFile;
                txtEnvType.Text = sEnvironmentList;
                txtSSISCatalog.Text = sSSISCatalog;
                cbMultiEnvPerCatalog.Checked = bMultiEnvPerCatalog;
                txtSSISFolderName.Text = sSSISFolderName;
                txtConnStr.Text = sbEnvServList.ToString().Substring(0, sbEnvServList.ToString().Length - 3);
                txtSSISFolderDescription.Text = sSSISFolderDescription;
                txtSSISProjectName.Text = sSSISProjectName;
                txtCredName.Text = sCredentialName;
                txtSSISProxyName.Text = sSSISProxyName;
                txtSSISProjectFilename.Text = sSSISProjectFilename;
                cbMapProjParamsToEnvVar.Checked = bMapProjParamsToEnvVar;
                txtSQLAgentJobScript.Text = sSQLAgentJobScript;

                // Check to see if the ISPAC file exists for the deployment.
                if (deployXML.UseSSISProjectFilename == true && File.Exists(sDeployFilePath + sSSISProjectFilename) == false)
                {
                    throw new FileNotFoundException("SSIS Project (ISPAC) File: " + sDeployFilePath + sSSISProjectFilename + " does NOT exist!");
                }

                // Check to see if SQL Agent Script file(s) exists for the deployment.
                // Read the text into a variable if it does exist.
                foreach (string jobScriptFile in sSQLAgentJobScriptList)
                {
                    if (File.Exists(sDeployFilePath + jobScriptFile) == false)
                    {
                        throw new FileNotFoundException("SQL Agent Job Script File: " + sDeployFilePath + jobScriptFile + " does NOT exist!");
                    }
                }

                // Setup Tool Tip text for controls:
                ttPkgDeploy.SetToolTip(this.lblXMLFile, "Deployment XML Filename:\r\n" + sDeployFilePath + sDeployXMLFile);
                ttPkgDeploy.SetToolTip(this.lblEnvType, "Deploy To These Environments:\r\n" + sEnvironmentList);
                ttPkgDeploy.SetToolTip(this.lblSSISCatalog, "Default Database Name For The Integration Services Catalog.\r\nWould Normally Be Setup As SSISDB.");
                ttPkgDeploy.SetToolTip(this.lblMultiEnvPerCatalog, "When Checked, You Can Deploy Multiple Versions Of Packages And Agent Jobs\r\nTo A Single Integration Services Catalog. Allows Running Different Versions\r\nOf The SSIS Packages Against Different Environments. The Folder Names Will\r\nBe Appended With The Environment Name. SQL Agent Jobs Can Be Run Against\r\nIndividual Environments. Would Normally Only Deploy To A Single Environment\r\nAt A Time When Using This Deployment Model.");
                ttPkgDeploy.SetToolTip(this.lblConnStr, "Building Connection Strings For The Following Environments And Servers (Initial Catalog=master):\r\n" + txtConnStr.Text);
                ttPkgDeploy.SetToolTip(this.lblSSISFolderDescription, "Description For The Folder In The Integration Services\r\nCatalog Where The Project Files Are Installed.\r\nFolder Description: " + txtSSISFolderDescription.Text);
                ttPkgDeploy.SetToolTip(this.lblSSISFolderName, "Name Of The Folder In The Integration Services Catalog\r\nWhere The Project Files Are Installed. The Folder Name Is\r\nTypically The Same As The Project Name, But Can Be Different.\r\nFolder Name: " + txtSSISFolderName.Text);
                ttPkgDeploy.SetToolTip(this.lblSSISProjectName, "Name Of The Project Folder In The Integration Services Catalog.\r\nThis Folder Needs To Have The Same Name As The Visual Studio\r\nProject Containing The SSIS Packages.\r\nProject Name: " + txtSSISFolderName.Text);
                ttPkgDeploy.SetToolTip(this.lblCredName, "Name Of The Credential (if applicable) Used For SSIS Package Execution. Leave\r\nThis Field Blank If A Credential Is Not Needed When Running The SQL Agent Job.\r\nIf the value of 'CreateCredential' is false, Then The Credential Will Not Be Created As Well.\r\nCredential Name: " + txtCredName.Text + "\r\nCreateCredential: " + (deployXML.CreateCredential == true ? "True" : "False"));
                ttPkgDeploy.SetToolTip(this.lblSSISProxyName, "Name Of The Proxy (if applicable) Used For SSIS Package Execution. Leave\r\nThis Field Blank If A Proxy Account Is Not Needed When Running The SQL\r\nAgent Job. If 'use=false', Then The Proxy Will Not Be Used As Well. The\r\nProxy Name Is Used By The Script That Creates The SQL Agent Jobs.\r\nProxy Name: " + txtSSISProxyName.Text + "\r\nCreateProxy: " + (deployXML.CreateProxy == true ? "True" : "False"));
                ttPkgDeploy.SetToolTip(this.lblSSISProjectFilename, "Name Of The Project ISPAC Filename (if applicable). This Is The Main Deployment\r\nFile That Contains The SSIS Package DTSX Files And Project Parameter Definitions.\r\nLeave This Field Blank If Packages Are Not Being Deployed. If 'use=false',\r\nThen The Packages Will Not Be Deployed As Well.\r\nProject ISPAC Filename: " + txtSSISProjectFilename.Text);
                ttPkgDeploy.SetToolTip(this.lblMapProjParamsToEnvVar, "Runs A Script That Maps Environment Variable Definitions To\r\nProject Parameters. Should Typically Run This Script To Ensure\r\nThe Jobs Are Running Using The Correct Project Variable Values.");
                ttPkgDeploy.SetToolTip(this.lblSQLAgentJobScript, "Pipe Delimited List Of Scripts Used To Create SQL Agent Jobs.\r\nIf 'use=false',Then The Scripts Will Not Be Run.\r\nSQL Agent Job Script(s): " + txtSQLAgentJobScript.Text);
                ttPkgDeploy.SetToolTip(this.lblSQLAgentJobConnStr, "Shows Connection Strings Used When Running SQL Agent Job Scripts (Initial Catalog=msdb).");
                ttPkgDeploy.SetToolTip(this.btnDeployPackage, "Deploy To The Specified Environments:\r\n" + sEnvironmentList);
                ttPkgDeploy.SetToolTip(this.txtErrMsg, "Shows Deployment Status And Any Error Messages Generated During Deployment.\r\nText Color Turns Red When An Error Occurs!");
                ttPkgDeploy.SetToolTip(this.btnExit, "Exit Deploy Tool");

                btnDeployPackage.Focus();
            }
            catch (Exception ex)
            {
                btnDeployPackage.Enabled = false;
                txtErrMsg.Text = "";
                txtErrMsg.Text = ex.Message + (ex.InnerException != null ? "\r\n\r\nInner Exception: " + ex.InnerException.Message : "");
                txtErrMsg.ForeColor = Color.Red;
                txtErrMsg.Refresh();
            }
        } // private void SSISPkgDeploy_Load(object sender, EventArgs e)

        private void SelectDeployXML()
        {
                OpenFileDialog ofdDeployFiles = new OpenFileDialog();

                ofdDeployFiles.InitialDirectory = sInitialDirectory;
                ofdDeployFiles.Title = "Select Deployment XML File.";
                ofdDeployFiles.CheckFileExists = true;
                ofdDeployFiles.CheckPathExists = true;
                ofdDeployFiles.DefaultExt = "xml";
                ofdDeployFiles.Filter = "XML files (*.xml)|*.xml";
                ofdDeployFiles.FilterIndex = 1;
                ofdDeployFiles.Multiselect = false;
                ofdDeployFiles.RestoreDirectory = true;
                if (ofdDeployFiles.ShowDialog() == DialogResult.OK)
                {
                    sDeployFilePath = Path.GetDirectoryName(ofdDeployFiles.FileName) + @"\";
                    sDeployXMLFile = Path.GetFileName(ofdDeployFiles.FileName);
                }
        } // private void SelectDeployXML()

        private void btnDeployPackage_Click(object sender, EventArgs e)
        {
            try
            {
                btnDeployPackage.Enabled = false;
                envCnt = 0;

                txtErrMsg.ForeColor = Color.Black;
                txtErrMsg.Refresh();
                sbMsg = new StringBuilder();
                Catalog catalog = null;
                CatalogFolder catalogFolder = null;
                string varName = string.Empty;
                bool varSensitive = false;
                TypeCode typeCode = TypeCode.String;

                foreach (string env in sEnvironmentTypes)
                {
                    envCnt++;
                    sEnvironmentType = env;
                    index = deployXML.EnvironmentServerNames.FindIndex(x => x.Name == env);
                    sEnvironmentServerName = deployXML.EnvironmentServerNames[index].Server;
                    deployXML.SetEnvironmentVariablesList(sEnvironmentType);
                    txtEnvType.Text = sEnvironmentType;
                    txtEnvType.Refresh();
                    switch (sEnvironmentType)
                    {
                        case "Test":
                            sEnvironmentName = "Test";
                            break;
                        case "Dev":
                            sEnvironmentName = "Dev";
                            break;
                        case "QA":
                            sEnvironmentName = "QA";
                            break;
                        case "UATPri":
                            sEnvironmentName = "UAT";
                            break;
                        case "UATSec":
                            sEnvironmentName = "UAT";
                            break;
                        case "PRDPri":
                            sEnvironmentName = "PRD";
                            break;
                        case "PRDSec":
                            sEnvironmentName = "PRD";
                            break;
                        default:
                            throw new Exception("Unknown Deployment Type Specified In The Config File: " + sEnvironmentType);
                    }

                    // Get the connection strings for the deployment servers, replacing text #EnvironmentServerName#, for the physical server name.
                    sqlConnPkgDeply = ConfigurationManager.ConnectionStrings["SQLConnPkgDeply"].ConnectionString.Replace("#EnvironmentServerName#", sEnvironmentServerName);
                    sqlConnJobScript = ConfigurationManager.ConnectionStrings["SQLConnJobScript"].ConnectionString.Replace("#EnvironmentServerName#", sEnvironmentServerName);
                    txtConnStr.Text = sqlConnPkgDeply;
                    txtConnStr.Refresh();
                    if (sSQLAgentJobScript.Length > 0)
                    {
                        txtSQLAgentJobConnStr.Text = sqlConnJobScript;
                        txtSQLAgentJobConnStr.Refresh();
                    }

                    // Adjust the Folder Name if deploying to an SSISDB Catalog where you have multiple environments running.
                    // Meaning you're deploying to a server where you want to maintain separate Dev/QA/UAT environments.
                    // Append the Environment Type to the Folder Name to isolate package code for the environments.
                    if (bMultiEnvPerCatalog == true)
                    {
                        sSSISFolderName = deployXML.SSISFolderName + sEnvironmentName;
                        sSSISFolderDescription = deployXML.SSISFolderDescription + " (" + sEnvironmentName + ")";
                        txtSSISFolderName.Text = sSSISFolderName;
                        txtSSISFolderName.Refresh();
                        txtSSISFolderDescription.Text = sSSISFolderDescription;
                        txtSSISFolderDescription.Refresh();
                    }

                    sbMsg.Append("Deployment for Environment Type: '" + sEnvironmentType + "', Server Name: " + sEnvironmentServerName + ".\r\n");
                    txtErrMsg.Text = sbMsg.ToString();
                    txtErrMsg.Refresh();

                    sbMsg.Append("Multiple Environments Per SSISDB Catalog: " + (bMultiEnvPerCatalog == true ? "True" : "False") + ".\r\n");
                    txtErrMsg.Text = sbMsg.ToString();
                    txtErrMsg.Refresh();

                    // Create the SSIS object:
                    SqlConnection oConnection = new SqlConnection(sqlConnPkgDeply);
                    IntegrationServices integrationServices = new IntegrationServices(oConnection);

                    // Verify there is an SSIS Catalog on the deployment server:
                    if (integrationServices.Catalogs.Count == 0)
                    {
                        throw new Exception("There are no SSIS Catalogs associated with connection string: " + sqlConnPkgDeply + "  The default Integration Services Catalog is assumed to be " + sSSISCatalog + ".");
                    }
                    else
                    {
                        catalog = integrationServices.Catalogs[sSSISCatalog];
                    }

                    // Check to see if the Project folder exists:
                    catalogFolder = catalog.Folders[sSSISFolderName];
                    if (catalogFolder == null)
                    {
                        // Create a catalog folder and assign description.
                        catalogFolder = new CatalogFolder(catalog, sSSISFolderName, sSSISFolderDescription);
                        catalogFolder.Create();
                        sbMsg.Append("Folder:" + sSSISFolderName + " has been created in the SSIS Catalog.\r\n");
                        txtErrMsg.Text = sbMsg.ToString();
                        txtErrMsg.Refresh();
                    }

                    if (deployXML.UseSSISProjectFilename == true && sSSISProjectFilename.Length > 0)
                    {
                        // Deploy the project packages:
                        sbMsg.Append("Deploying " + sDeployFilePath + sSSISProjectFilename + " project ISPAC file.\r\n");
                        txtErrMsg.Text = sbMsg.ToString();
                        txtErrMsg.Refresh();

                        // Think you can only deploy the entire project, not just individual dtsx files.
                        byte[] projectFile = File.ReadAllBytes(sDeployFilePath + sSSISProjectFilename);
                        catalogFolder.DeployProject(sSSISProjectName, projectFile);

                        sbMsg.Append("SSIS Project (" + sSSISProjectFilename + ") has been successfully deployed!\r\n");
                        txtErrMsg.Text = sbMsg.ToString();
                        txtErrMsg.Refresh();
                    }

                    // Create an Environment for the SSIS project:
                    if (deployXML.EnvironmentVariables.Count > 0)
                    {
                        if (catalogFolder.Environments[sEnvironmentName] != null)
                            catalogFolder.Environments[sEnvironmentName].Drop();
                        EnvironmentInfo environment = new EnvironmentInfo(catalogFolder, sEnvironmentName, sSSISFolderName + " Environment Variables (" + sEnvironmentName + ")");
                        environment.Create();

                        sbMsg.Append("SSIS '" + sEnvironmentType + "' Environment has been successfully created!\r\n");
                        txtErrMsg.Text = sbMsg.ToString();
                        txtErrMsg.Refresh();

                        // Add variables to the environment:
                        foreach (var projVar in deployXML.EnvironmentVariables)
                        {
                            varName = projVar.Name;
                            varSensitive = projVar.Sensitive.ToLower() == "false" || projVar.Sensitive.ToLower() == "no" ? false : true;
                            switch (projVar.Type.ToUpper())
                            {
                                case "BOOLEAN":
                                    typeCode = TypeCode.Boolean;
                                    break;
                                case "BYTE":
                                    typeCode = TypeCode.Byte;
                                    break;
                                case "CHAR":
                                    typeCode = TypeCode.Char;
                                    break;
                                case "DATETIME":
                                    typeCode = TypeCode.DateTime;
                                    break;
                                case "DBNULL":
                                    typeCode = TypeCode.DBNull;
                                    break;
                                case "DECIMAL":
                                    typeCode = TypeCode.Decimal;
                                    break;
                                case "DOUBLE":
                                    typeCode = TypeCode.Double;
                                    break;
                                case "EMPTY":
                                    typeCode = TypeCode.Empty;
                                    break;
                                case "INT16":
                                    typeCode = TypeCode.Int16;
                                    break;
                                case "INT32":
                                    typeCode = TypeCode.Int32;
                                    break;
                                case "INT64":
                                    typeCode = TypeCode.Int64;
                                    break;
                                case "OBJECT":
                                    typeCode = TypeCode.Object;
                                    break;
                                case "SBYTE":
                                    typeCode = TypeCode.SByte;
                                    break;
                                case "SINGLE":
                                    typeCode = TypeCode.Single;
                                    break;
                                case "STRING":
                                    typeCode = TypeCode.String;
                                    break;
                                case "UINT16":
                                    typeCode = TypeCode.UInt16;
                                    break;
                                case "UINT32":
                                    typeCode = TypeCode.UInt32;
                                    break;
                                case "UINT64":
                                    typeCode = TypeCode.UInt64;
                                    break;
                                default:
                                    throw new Exception("Unknown Type Code Specified In The Environment Variable Config File Section: " + projVar.Type);
                            }
                            environment.Variables.Add(varName, typeCode, projVar.Value, varSensitive, projVar.Description);
                            sbMsg.Append("Added Environment Variable: " + varName + ", Type = " + projVar.Type + ", Value = " + projVar.Value + ", Description = " + projVar.Description + ", Sensitive = " + (varSensitive == false ? "false" : "true") + "\r\n");
                        }
                        environment.Alter();
                        txtErrMsg.Text = sbMsg.ToString();
                        txtErrMsg.Refresh();

                        //Add environment reference to the SSIS project:
                        ProjectCollection SSISProjects = catalogFolder.Projects;
                        ProjectInfo SSISProject = SSISProjects[sSSISProjectName];
                        if (SSISProject.References.Contains(sEnvironmentName, sSSISFolderName) == true)
                            SSISProject.References.Remove(sEnvironmentName, sSSISFolderName);
                        SSISProject.References.Add(sEnvironmentName, sSSISFolderName);
                        SSISProject.Alter();

                        sbMsg.Append("Environment reference '" + sEnvironmentType + "' has been added to the SSIS Project " + sSSISFolderName + "\r\n");
                        txtErrMsg.Text = sbMsg.ToString();
                        txtErrMsg.Refresh();

                        //Create Credential and Proxy.
                        if ((deployXML.CreateCredential == true && deployXML.CredentialName != string.Empty) || (deployXML.CreateProxy == true && deployXML.SSISProxyName != string.Empty))
                        {
                            if ((deployXML.SameIdentitySecretForAllEnv == true && envCnt == 1) || deployXML.SameIdentitySecretForAllEnv == false)
                            {
                                using (var credProxy = new CredProxy(sEnvironmentType, deployXML.CredentialName, deployXML.SSISProxyName, deployXML.CreateCredential))
                                {
                                    credProxy.ShowDialog();
                                    sIdentity = credProxy.Identity;
                                    sSecret = credProxy.Secret;
                                }
                            }

                            // Run script to create the credential.
                            if (deployXML.CreateCredential == true)
                            {
                                sErrorMsg = string.Empty;

                                SqlParameter[] objParameter = new SqlParameter[4];
                                objParameter[0] = new SqlParameter("@sCredName", SqlDbType.NVarChar, 128)
                                {
                                    Value = deployXML.CredentialName
                                };
                                objParameter[1] = new SqlParameter("@sIdentity", SqlDbType.NVarChar, 128)
                                {
                                    Value = sIdentity
                                };
                                objParameter[2] = new SqlParameter("@sSecret", SqlDbType.NVarChar, 128)
                                {
                                    Value = sSecret
                                };
                                objParameter[3] = new SqlParameter("@sErrorMsg", SqlDbType.NVarChar, 1000)
                                {
                                    Value = sErrorMsg,
                                    Direction = ParameterDirection.Output
                                };

                                ExecuteNonQueryAsText(SQLScripts.ADHOCCreateCredential, objParameter, sqlConnPkgDeply); // Uses Initial Catalog of master.
                                sErrorMsg = objParameter[3].Value.ToString().Trim(); // This gets the value output for sErrorMsg.
                                sbMsg.Append("Script used to create the SQL job CREDENTIAL was run.\r\n");
                                if (sErrorMsg.Length > 0)
                                    sbMsg.Append("Error running script used to create the job run CREDENTIAL:\r\n" + sErrorMsg + "\r\n");
                                txtErrMsg.Text = sbMsg.ToString();
                                txtErrMsg.Refresh();
                            }

                            // Run script to create the proxy.
                            if (deployXML.CreateProxy == true)
                            {
                                sErrorMsg = string.Empty;

                                SqlParameter[] objParameter = new SqlParameter[4];
                                objParameter[0] = new SqlParameter("@sCredName", SqlDbType.NVarChar, 128)
                                {
                                    Value = deployXML.CredentialName
                                };
                                objParameter[1] = new SqlParameter("@sProxyName", SqlDbType.NVarChar, 128)
                                {
                                    Value = deployXML.SSISProxyName
                                };
                                objParameter[2] = new SqlParameter("@sIdentity", SqlDbType.NVarChar, 128)
                                {
                                    Value = sIdentity
                                };
                                objParameter[3] = new SqlParameter("@sErrorMsg", SqlDbType.NVarChar, 1000)
                                {
                                    Value = sErrorMsg,
                                    Direction = ParameterDirection.Output
                                };

                                ExecuteNonQueryAsText(SQLScripts.ADHOCCreateProxy, objParameter, sqlConnJobScript); // Uses Initial Catalog of msdb.
                                sErrorMsg = objParameter[3].Value.ToString().Trim(); // This gets the value output for sErrorMsg.
                                sbMsg.Append("Script used to create the SQL job PROXY was run.\r\n");
                                if (sErrorMsg.Length > 0)
                                    sbMsg.Append("Error running script used to create the job run PROXY:\r\n" + sErrorMsg + "\r\n");
                                txtErrMsg.Text = sbMsg.ToString();
                                txtErrMsg.Refresh();
                            }
                        }

                        // Run script to map project parameters to environment variables.
                        if (bMapProjParamsToEnvVar == true)
                        {

                            SqlParameter[] objParameter = new SqlParameter[3];
                            objParameter[0] = new SqlParameter("@sEnvironmentType", SqlDbType.NVarChar, 50)
                            {
                                Value = sEnvironmentName
                            };
                            objParameter[1] = new SqlParameter("@sSSISFolderName", SqlDbType.NVarChar, 128)
                            {
                                Value = sSSISFolderName
                            };
                            objParameter[2] = new SqlParameter("@sSSISProjectName", SqlDbType.NVarChar, 128)
                            {
                                Value = sSSISProjectName
                            };
                            ExecuteNonQueryAsText(SQLScripts.ADHOCMapProjParamsToEnvVar, objParameter, sqlConnPkgDeply);
                            sbMsg.Append("Script used to map Project Parameters to Environment Variables was run.\r\n");
                            txtErrMsg.Text = sbMsg.ToString();
                            txtErrMsg.Refresh();
                        }
                    } // if (htEnvironmentVariables.Count > 0)

                    foreach (string jobScriptFile in sSQLAgentJobScriptList)
                    {
                        sbSQLAgentJobScriptText = new StringBuilder();
                        sbSQLAgentJobScriptText.Append(System.IO.File.ReadAllText(sDeployFilePath + jobScriptFile));
                        SqlParameter[] objParameter = new SqlParameter[6];
                        objParameter[0] = new SqlParameter("@sEnvironmentType", SqlDbType.NVarChar, 50)
                        {
                            Value = sEnvironmentName
                        };
                        objParameter[1] = new SqlParameter("@bMultiEnvPerCatalog", SqlDbType.Bit)
                        {
                            Value = bMultiEnvPerCatalog
                        };
                        objParameter[2] = new SqlParameter("@sSSISFolderName", SqlDbType.NVarChar, 128)
                        {
                            Value = sSSISFolderName
                        };
                        objParameter[3] = new SqlParameter("@sSSISProjectName", SqlDbType.NVarChar, 128)
                        {
                            Value = sSSISProjectName
                        };
                        objParameter[4] = new SqlParameter("@sSSISProxyName", SqlDbType.NVarChar, 128)
                        {
                            Value = sSSISProxyName
                        };
                        objParameter[5] = new SqlParameter("@sSSISCatServerName", SqlDbType.NVarChar, 128)
                        {
                            Value = sEnvironmentServerName
                        };
                        ExecuteNonQueryAsText(sbSQLAgentJobScriptText.ToString(), objParameter, sqlConnJobScript);
                        sbMsg.Append("SQL Agent Job Script File " + sDeployFilePath + jobScriptFile + " was run.\r\n");
                        txtErrMsg.Text = sbMsg.ToString();
                        txtErrMsg.Refresh();
                    } // foreach (string jobScriptFile in sSQLAgentJobScriptList)

                    sbMsg.Append("\r\n");
                    txtErrMsg.Text = sbMsg.ToString();
                    txtErrMsg.Refresh();

                } // foreach (string env in sEnvironmentTypes)

                btnExit.BackColor = Color.Green;
                btnExit.Focus();

            }
            catch (Exception ex)
            {
                txtErrMsg.Text = sbMsg.ToString() + "\r\n" + ex.Message + (ex.InnerException != null ? "\r\n\r\nInner Exception: " + ex.InnerException.Message : "") + "\r\n";
                txtErrMsg.ForeColor = Color.Red;
                txtErrMsg.Refresh();
            }
            finally
            {
                // Create or append run results to the log file.
                CopyToLogFile(txtErrMsg.Text);
            }
        } // private void btnDeployPackage_Click(object sender, EventArgs e)

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private int ExecuteNonQueryAsText(string queryText, string sqlConnJobScript)
        {
            // Assuming this is an adhoc query with no TRY/CATCH.
            SqlConnection oConnection = new SqlConnection(sqlConnJobScript);
            SqlCommand oCommand = new SqlCommand(queryText, oConnection);

            oCommand.CommandType = CommandType.Text;
            int iReturnValue;
            oConnection.Open();
            using (SqlTransaction oTransaction = oConnection.BeginTransaction())
            {
                try
                {
                    oCommand.Transaction = oTransaction;
                    iReturnValue = oCommand.ExecuteNonQuery();
                    oTransaction.Commit();
                }
                catch
                {
                    oTransaction.Rollback();
                    throw;
                }
                finally
                {
                    if (oConnection.State == ConnectionState.Open)
                        oConnection.Close();
                    oConnection.Dispose();
                    oCommand.Dispose();
                }
            }
            return iReturnValue;
        } // private int ExecuteNonQueryAsText(string queryText, string sqlConnJobScript)

        private static int ExecuteNonQueryAsText(string queryText, SqlParameter[] parameters, string sqlConnJobScript)
        {
            // Assuming adhoc queries with parameters have TRY/CATCH blocks.
            SqlConnection oConnection = new SqlConnection(sqlConnJobScript);
            SqlCommand oCommand = new SqlCommand(queryText, oConnection);

            oCommand.CommandType = CommandType.Text;
            int iReturnValue;
            oConnection.Open();
            try
            {
                if (parameters != null)
                    oCommand.Parameters.AddRange(parameters);

                iReturnValue = oCommand.ExecuteNonQuery();
            }
            catch
            {
                throw;
            }
            finally
            {
                if (oConnection.State == ConnectionState.Open)
                    oConnection.Close();
                oConnection.Dispose();
                oCommand.Dispose();
            }
            return iReturnValue;
        } // public static int ExecuteNonQueryAsText(string queryText, SqlParameter[] parameters, string sqlConnJobScript)

        private void CopyToLogFile(string deployMsg)
        {
            try
            {
                DateTime dt = DateTime.Now;
                StringBuilder sbMsg = new StringBuilder();
                sbMsg.Append('=', 100);
                sbMsg.Append("\r\n");
                sbMsg.Append("Time Logged: " + String.Format("{0:F}", dt));
                sbMsg.Append("\r\n\r\n");
                sbMsg.Append(deployMsg);

                if (File.Exists(sDeployFilePath + sLogFileName) == false)
                {
                    // WriteAllText creates a file, writes the specified text to the file,
                    // and then closes the file. You do NOT need to call Flush() or Close().
                    System.IO.File.WriteAllText(sDeployFilePath + sLogFileName, sbMsg.ToString());
                }
                else
                {
                    // Append new text to an existing file.
                    // The using statement automatically flushes AND Closes the stream
                    // and calls IDisposable.Dispose on the stream object.
                    using (System.IO.StreamWriter file =
                        new System.IO.StreamWriter(sDeployFilePath + sLogFileName, true))
                    {
                        file.WriteLine(sbMsg.ToString());
                    }
                }
            }
            catch
            {
                // Do nothing, since this is called from the finally!
            }
        } // private void CopyToLogFile(string deployMsg)

        private static void ValidationEventHandler(object sender, ValidationEventArgs e)
        {
            XmlSeverityType type = XmlSeverityType.Warning;
            if (Enum.TryParse<XmlSeverityType>("Error", out type))
            {
                if (type == XmlSeverityType.Error) throw new Exception(e.Message);
            }
        } // static void ValidationEventHandler(object sender, ValidationEventArgs e)

    } // public partial class SSISPkgDeploy
} // namespace SSISPkgDeploy
