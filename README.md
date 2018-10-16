# SSIS-Package-Deploy
Project Used To Deploy SSIS Packages

Facilitates SSIS Package installation, configuring Package settings, and SQL Agent Job deployment
using a C# Windows Form utility program SSISPkgDeploy.exe. Most of the settings for the deployment
are located in an XM file.

Prerequisites for running this program are having the .Net Framework 4.5.2 installed, and the
utility uses integrated security to connect to the target deployment server(s). So, the user
running the program needs to have access to the Integration Services Catalogs (SSISDB), master
database to configure the SSIS packages, and msdb database to install the SQL Agent Jobs. Also,
the SSISDB must exist on the target server, the utility will not create the SSISDB.
