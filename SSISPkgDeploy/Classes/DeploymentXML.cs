using System;
using System.Xml;
using System.Collections.Generic;

namespace SSISPkgDeploy
{

    public class DeploymentXML
    {
        #region "PRIVATE VARIABLES"

        private XmlDocument deployDoc = null;
        private string environmentList = string.Empty;
        private string ssisCatalog = string.Empty;
        private bool multiEnvPerCatalog = false;
        private string ssisFolderName = string.Empty;
        private string ssisFolderDescription = string.Empty;
        private string ssisProjectName = string.Empty;
        private string ssisProjectFilename = string.Empty;
        private bool useSSISProjectFilename = false;
        private bool mapProjParamsToEnvVar = false;
        private string sqlAgentJobScript = string.Empty;
        private bool useSQLAgentJobScript = false;

        private string credentialName = string.Empty;
        private string ssisProxyName = string.Empty;
        private bool useSSISProxyName = false;
        private bool createCredential = false;
        private bool createProxy = false;
        private bool sameIdentitySecretForAllEnv = false;

        private List<EnvSrvNm> envSrvNm;
        private List<EnvVar> envVar;

        #endregion

        #region "PUBLIC PROPERTIES"

        public string EnvironmentList { get { return environmentList; } }
        public string SSISCatalog { get { return ssisCatalog; } }
        public bool MultiEnvPerCatalog { get { return multiEnvPerCatalog; } }
        public string SSISFolderName { get { return ssisFolderName; } }
        public string SSISFolderDescription { get { return ssisFolderDescription; } }
        public string SSISProjectName { get { return ssisProjectName; } }
        public string SSISProjectFilename { get { return ssisProjectFilename; } }
        public bool UseSSISProjectFilename { get { return useSSISProjectFilename; } }
        public bool MapProjParamsToEnvVar { get { return mapProjParamsToEnvVar; } }
        public string SQLAgentJobScript { get { return sqlAgentJobScript; } }
        public bool UseSQLAgentJobScript { get { return useSQLAgentJobScript; } }

        public string CredentialName { get { return credentialName; } }
        public string SSISProxyName { get { return ssisProxyName; } }
        public bool UseSSISProxyName { get { return useSSISProxyName; } }
        public bool CreateCredential { get { return createCredential; } }
        public bool CreateProxy { get { return createProxy; } }
        public bool SameIdentitySecretForAllEnv { get { return sameIdentitySecretForAllEnv; } }

        public List<EnvSrvNm> EnvironmentServerNames { get { return envSrvNm; } }
        public List<EnvVar> EnvironmentVariables { get { return envVar; } }

        #endregion

        public DeploymentXML(XmlDocument doc)
        {
            deployDoc = doc;
            try
            {
                XmlNodeList projectSettings = deployDoc.SelectNodes(@"/deployment/projectSettings/setting");
                foreach (XmlNode node in projectSettings)
                {
                    if (node.Attributes != null)
                    {
                        var nameAttr = node.Attributes["name"];
                        if (nameAttr != null)
                        {
                            if (node.Attributes["name"].Value == "EnvironmentType")
                                environmentList = node.Attributes["value"].Value;
                            else if (node.Attributes["name"].Value == "SSISCatalog")
                                ssisCatalog = node.Attributes["value"].Value;
                            else if (node.Attributes["name"].Value == "MultiEnvPerCatalog")
                                multiEnvPerCatalog = node.Attributes["value"].Value.ToLower() == "true" || node.Attributes["value"].Value.ToLower() == "yes" ? true : false;
                            else if (node.Attributes["name"].Value == "SSISFolderName")
                                ssisFolderName = node.Attributes["value"].Value;
                            else if (node.Attributes["name"].Value == "SSISFolderDescription")
                                ssisFolderDescription = node.Attributes["value"].Value;
                            else if (node.Attributes["name"].Value == "SSISProjectName")
                                ssisProjectName = node.Attributes["value"].Value;
                            else if (node.Attributes["name"].Value == "SSISProxyName")
                            {
                                ssisProxyName = node.Attributes["value"].Value;
                                if (node.Attributes["use"] == null)
                                    useSSISProxyName = ssisProxyName.Length > 0 ? true : false;
                                else
                                    useSSISProxyName = node.Attributes["use"].Value.ToLower() == "true" || node.Attributes["use"].Value.ToLower() == "yes" ? true : false;
                            }
                            else if (node.Attributes["name"].Value == "SSISProjectFilename")
                            {
                                ssisProjectFilename = node.Attributes["value"].Value;
                                if (node.Attributes["use"] == null)
                                    useSSISProjectFilename = ssisProjectFilename.Length > 0 ? true : false;
                                else
                                    useSSISProjectFilename = node.Attributes["use"].Value.ToLower() == "true" || node.Attributes["use"].Value.ToLower() == "yes" ? true : false;
                            }
                            else if (node.Attributes["name"].Value == "MapProjParamsToEnvVar")
                                mapProjParamsToEnvVar = node.Attributes["value"].Value.ToLower() == "true" || node.Attributes["value"].Value.ToLower() == "yes" ? true : false;
                            else if (node.Attributes["name"].Value == "SQLAgentJobScript")
                            {
                                sqlAgentJobScript = node.Attributes["value"].Value;
                                if (node.Attributes["use"] == null)
                                    useSQLAgentJobScript = sqlAgentJobScript.Length > 0 ? true : false;
                                else
                                    useSQLAgentJobScript = node.Attributes["use"].Value.ToLower() == "true" || node.Attributes["use"].Value.ToLower() == "yes" ? true : false;
                            }
                        }
                    }
                } // foreach (XmlNode node in projectSettings)

                XmlNodeList proxyCredentialSetup = deployDoc.SelectNodes(@"/deployment/proxyCredentialSetup/proxy");
                foreach (XmlNode node in proxyCredentialSetup)
                {
                    if (node.Attributes != null)
                    {
                        var nameAttr = node.Attributes["name"];
                        if (nameAttr != null)
                        {
                            if (node.Attributes["name"].Value == "CredentialName")
                                credentialName = node.Attributes["value"].Value;
                            else if (node.Attributes["name"].Value == "SSISProxyName")
                            {
                                ssisProxyName = node.Attributes["value"].Value;
                                if (node.Attributes["use"] == null)
                                    useSSISProxyName = ssisProxyName.Length > 0 ? true : false;
                                else
                                    useSSISProxyName = node.Attributes["use"].Value.ToLower() == "true" || node.Attributes["use"].Value.ToLower() == "yes" ? true : false;
                            }
                            else if (node.Attributes["name"].Value == "CreateCredential")
                                createCredential = node.Attributes["value"].Value.ToLower() == "true" || node.Attributes["value"].Value.ToLower() == "yes" ? true : false;
                            else if (node.Attributes["name"].Value == "CreateProxy")
                                createProxy = node.Attributes["value"].Value.ToLower() == "true" || node.Attributes["value"].Value.ToLower() == "yes" ? true : false;
                            else if (node.Attributes["name"].Value == "SameIdentitySecretForAllEnv")
                                sameIdentitySecretForAllEnv = node.Attributes["value"].Value.ToLower() == "true" || node.Attributes["value"].Value.ToLower() == "yes" ? true : false;
                        }
                    }
                } // foreach (XmlNode node in proxyCredentialSetup)

                var envSrv = deployDoc.SelectNodes(@"/deployment/environmentServerNames/environment");
                envSrvNm = new List<EnvSrvNm>();
                foreach (XmlNode node in envSrv)
                {
                    envSrvNm.Add(new EnvSrvNm(node.Attributes["name"].Value, node.Attributes["server"].Value));
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Constructor for class DeploymentXML: " + ex.Message + (ex.InnerException != null ? "\r\n\r\nInner Exception: " + ex.InnerException.Message : ""));
            }

        } // public DeploymentXML(XmlDocument doc)


        public void SetEnvironmentVariablesList(string envName)
        {
            envVar = new List<EnvVar>();
            var projVar = deployDoc.SelectNodes(@"/deployment/environmentVariables" + envName + @"/variable");
            envVar = new List<EnvVar>();
            foreach (XmlNode node in projVar)
            {
                envVar.Add(new EnvVar(node.Attributes["name"].Value, node.Attributes["type"].Value, node.Attributes["value"].Value, node.Attributes["sensitive"].Value, node.Attributes["description"].Value));
            }

        }

        #region "PUBLIC CLASSES"

        public class EnvSrvNm
        {
            private string name;
            private string server;

            internal EnvSrvNm(string name, string server)
            {
                this.name = name;
                this.server = server;
            }
            public string Name { get { return name; } set { name = value; } }
            public string Server { get { return server; } set { server = value; } }
        }

        public class EnvVar
        {
            private string name;
            private string type;
            private string _value;
            private string sensitive;
            private string description;

            internal EnvVar(string name, string type, string value, string sensitive, string description)
            {
                this.name = name;
                this.type = type;
                this._value = value;
                this.sensitive = sensitive;
                this.description = description;
            }
            public string Name { get { return name; } set { name = value; } }
            public string Type { get { return type; } set { type = value; } }
            public string Value { get { return _value; } set { _value = value; } }
            public string Sensitive { get { return sensitive; } set { sensitive = value; } }
            public string Description { get { return description; } set { description = value; } }
        }

        #endregion

    } // public class DeploymentXML
} // namespace SSISPkgDeploy
