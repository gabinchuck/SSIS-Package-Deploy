# SSIS-Package-Deploy
Project Used To Deploy SSIS Packages

Deploys SSIS Packages using the Project Deployment Model supported by SQL Server 2012 to 2017. Also allows, configuring Project Parameters for multiple Environments, and creating SQL Agent Jobs. The applicaton is a C# Windows Form utility program (SSISPkgDeploy.exe). Most of the settings for the deployment are specified in an XML file.

The main configureable package deployment features are SSIS packages (ISPAC files), Project Parameter settings, and SQL Agent Jobs can be deployed to a single environment, or multiple environments. Can setup Project Environments for Test, Dev, QA, UAT and Production. This allows consistent deployments to multiple environments. Also, the application can be used to create Credentials and Proxy Accounts that can be used to run SQL Agent Jobs.

Prerequisites for running this program are having the .Net Framework 4.5.2 installed, and the utility uses integrated security to connect to the target deployment server(s). So, the user running the program needs to have access to the Integration Services Catalogs (SSISDB),  master database to configure the SSIS packages, and msdb database to install the SQL Agent Jobs. Also, the SSISDB must exist on the target server, the utility will not create the SSISDB.

More detailed documentation can be found in the Project here: $\SSISPkgDeploy\DemoSSISPackage\Scripts\SSISPackageDeploy.docx

You can experiment with the project by running the deployment tool (SSISPkgDeploy.exe), located in folder $\SSISPkgDeploy\SSISPkgDeploy\bin\Debug, and modifying the deployment XML file (DemoSSISPackage.xml) as needed.
