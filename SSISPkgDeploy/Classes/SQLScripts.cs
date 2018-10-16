namespace SSISPkgDeploy
{
    public static class SQLScripts
    {
        #region "AD HOC QUERY PROPERTIES"

        private static string adhocMapProjParamsToEnvVar =
@"
--\ -------------------------------------------------------------------
---) Script to connect environment variables to package parameters based on variable name.
--/ -------------------------------------------------------------------
/*
USE SSISDB -- Comment out if run by the deployment tool.
GO -- Comment out if run by the deployment tool.
-- Parameters used when run by the deployment tool.
DECLARE @sEnvironmentType NVARCHAR(128) = N'QA'; -- Environment Name (Dev, QA, UAT, Prod). Comment out if run by the deployment tool.
DECLARE @sSSISFolderName SYSNAME = N'PaymentHub'; -- Comment out if run by the deployment tool.
DECLARE @sSSISProjectName NVARCHAR(128) = N'PaymentHub'; -- Comment out if run by the deployment tool.
*/

SET NOCOUNT ON;

DECLARE @tObjParamValue TABLE
(
	[id] INT IDENTITY(1, 1),
	[object_type] SMALLINT,
	[object_name] NVARCHAR(260),
	[parameter_name] NVARCHAR(128),
	[project_name] NVARCHAR(128)
);

DECLARE @sMsg NVARCHAR(255);
DECLARE @iID INT;
DECLARE @iMaxID INT;
DECLARE @iObjType SMALLINT;
DECLARE @sObjName NVARCHAR(260);
DECLARE @sObjType CHAR(1);
DECLARE @sParamName NVARCHAR(128);
DECLARE @vParamValue SQL_VARIANT;

--\
---) Table variable to store selected project names.
--/
DECLARE @tProjectNames TABLE
(
	[id] INT IDENTITY(1, 1),
	[project_name] NVARCHAR(128)
);

INSERT @tProjectNames([project_name])
SELECT p.[name]
FROM [SSISDB].[internal].[folders] f
JOIN [SSISDB].[internal].[environments] e
ON e.folder_id = f.folder_id
JOIN [SSISDB].[internal].[projects] p
ON p.folder_id = f.folder_id
WHERE f.[name] = @sSSISFolderName
	AND e.environment_name = @sEnvironmentType;
-- Optional project_name filter here:
-- AND p.name IN ('Project_1', 'Project_2')

--SELECT * FROM @tProjectNames

--\
---) Connect environment variables to project parameters and
---) package parameters, based on the name.
--/
INSERT @tObjParamValue
(
	[object_type],
	[object_name],
	[parameter_name],
	[project_name]
)
SELECT DISTINCT
	prm.[object_type],
	prm.[object_name],
	prm.[parameter_name],
	prj.[name]
FROM [SSISDB].[internal].[folders] f
JOIN [SSISDB].[internal].[environments] e
ON e.folder_id = f.folder_id
JOIN [SSISDB].[internal].[environment_variables] ev
ON e.environment_id = ev.environment_id
JOIN [SSISDB].[internal].[projects] prj
ON prj.folder_id = f.folder_id
JOIN @tProjectNames prjsel
ON prjsel.project_name = prj.[name]
JOIN [SSISDB].[internal].[object_parameters] prm
ON prj.project_id = prm.project_id
	AND prm.parameter_name = ev.[name]
WHERE prm.[value_type] != 'R' -- R = referenced value.
	AND prm.value_set = 0
	AND prm.[parameter_name] NOT LIKE 'CM.%'
	AND LEFT(prm.[parameter_name], 1) != '_' -- Naming convention for internally used parameters: start with _
	AND NOT ( prm.[object_type] = 30 AND LEFT(prm.[object_name], 1) = '_') -- Naming convention for internally used SSIS Packages: start with _
	AND f.[name] = @sSSISFolderName
	AND e.environment_name = @sEnvironmentType
ORDER BY prm.object_name, prm.parameter_name;

--SELECT * FROM @tObjParamValue

SELECT @iID = 1, @iMaxID = MAX([id]) FROM @tObjParamValue
WHILE @iID <= @iMaxID
BEGIN
	SELECT @iObjType = v.[object_type],
		@sObjName = v.[object_name],
		@sParamName = v.[parameter_name],
		@sSSISProjectName = v.[project_name]
	FROM @tObjParamValue v
	WHERE [id] = @iID;

	SELECT @sObjType = 'R', @vParamValue = @sParamName;

	--SET @sMsg = 'Parameter [' + @sParamName + '] (of object [' + @sObjName + ']) is mapped to environment variable.';
	--PRINT @sMsg;

	EXEC [SSISDB].[catalog].[set_object_parameter_value]
		@iObjType,
		@sSSISFolderName,
		@sSSISProjectName,
		@sParamName,
		@vParamValue,
		@sObjName,
		@sObjType;

    SET @iID = @iID + 1;
END
";
        public static string ADHOCMapProjParamsToEnvVar
        {
            get { return adhocMapProjParamsToEnvVar; }
        }

        private static string adhocCreateCredential =
@"
--\ -------------------------------------------------------------------
---) Script to create a credential used by SQL Agent Jobs running SSIS packages.
--/ -------------------------------------------------------------------
/*
USE master;
GO
DECLARE @sCredName SYSNAME = N'Test_CredProxy';
DECLARE @sIdentity SYSNAME = N'AMERICAS\hardyc1';
DECLARE @sSecret SYSNAME = N'hoppyB3er'; -- Add the service account password here.
DECLARE @sErrorMsg NVARCHAR(1000);
*/

SET NOCOUNT ON;

DECLARE @sSQLCmd NVARCHAR(2000);
DECLARE @iRetCode INT;

BEGIN TRY
	BEGIN TRANSACTION;

		-- First, create a credential to be used for the Proxy,
		-- that will be assigned to the SSIS Package SQL Job Step.
		SET @sSQLCmd =
N'IF EXISTS (SELECT 1 FROM sys.credentials WHERE name = ''' + @sCredName + ''')
BEGIN 
	DROP CREDENTIAL [' + @sCredName + ']' + '
END
CREATE CREDENTIAL [' + @sCredName + ']' + '
	WITH IDENTITY = ''' + @sIdentity + ''', 
	SECRET = ''' + @sSecret + ''';';

		--PRINT @sSQLCmd
		EXECUTE @iRetCode = sp_sqlexec @sSQLCmd;

	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	IF XACT_STATE() <> 0
		ROLLBACK TRANSACTION;

	-- Print error information. 
	SET @sErrorMsg = 'Error: ' + CONVERT(varchar(50), ERROR_NUMBER()) + CHAR(13) + CHAR(10) +
		'Severity: ' + CONVERT(varchar(5), ERROR_SEVERITY()) + CHAR(13) + CHAR(10) +
		'State: ' + CONVERT(varchar(5), ERROR_STATE()) +  CHAR(13) + CHAR(10) +
		'Line: ' + CONVERT(varchar(5), ERROR_LINE()) + CHAR(13) + CHAR(10) +
		'Message: ' + ERROR_MESSAGE();
END CATCH
";

        public static string ADHOCCreateCredential
        {
            get { return adhocCreateCredential; }
        }

        private static string adhocCreateProxy =
@"
--\ -------------------------------------------------------------------
---) Script to create an SSIS Proxy to be used by SQL Agent Jobs running SSIS packages.
--/ -------------------------------------------------------------------
/*
USE msdb;
GO

DECLARE @sCredName SYSNAME = N'Test_CredProxy';
DECLARE @sProxyName SYSNAME = N'Report_CredProxy';
DECLARE @sIdentity SYSNAME = N'AMERICAS\hardyc1';
DECLARE @sErrorMsg NVARCHAR(1000);
*/

SET NOCOUNT ON;

BEGIN TRY
	BEGIN TRANSACTION;
		-- Drop the proxy if it already exists:
		IF EXISTS (SELECT 1 FROM msdb.dbo.sysproxies WHERE [name] = @sProxyName)
		BEGIN
			EXEC dbo.sp_delete_proxy @proxy_name = @sProxyName;
		END
 
		-- Create a proxy and use the same credential as created above:
		EXEC msdb.dbo.sp_add_proxy
			@proxy_name = @sProxyName,
			@credential_name = @sCredName,
			@enabled = 1;

		-- FYI: To enable or disable the proxy you can use this command:
		--EXEC msdb.dbo.sp_update_proxy
		--	@proxy_name = N'SSISProxyDemo',
		--	@enabled = 1 --@enabled = 0

		-- View all the subsystems of SQL Server Agent with this command:
		--EXEC sp_enum_sqlagent_subsystems;
		-- View all the proxies granted to all the subsystems;
		--EXEC dbo.sp_enum_proxy_for_subsystem

		-- Grant created proxy access to SQL Agent subsystem:
		-- You can grant created proxy to any number of available subsystems.
		EXEC msdb.dbo.sp_grant_proxy_to_subsystem
			@proxy_name = @sProxyName,
			@subsystem_id = 11; -- subsystem 11 is for SSIS.

		-- Grant proxy account access to security principals that could be
		-- either login name or fixed server role or msdb role.
		-- Please note, Members of sysadmin server role are allowed to use any proxy.
		EXEC msdb.dbo.sp_grant_login_to_proxy
			@proxy_name = @sProxyName,
			@login_name = @sIdentity;
			--,@fixed_server_role=N''
			--,@msdb_role=N''

		-- View logins provided access to proxies;
		--EXEC dbo.sp_enum_login_for_proxy 
	COMMIT TRANSACTION;
END TRY
BEGIN CATCH
	IF XACT_STATE() <> 0
		ROLLBACK TRANSACTION;

	-- Print error information. 
	SET @sErrorMsg = 'Error: ' + CONVERT(varchar(50), ERROR_NUMBER()) + CHAR(13) + CHAR(10) +
		'Severity: ' + CONVERT(varchar(5), ERROR_SEVERITY()) + CHAR(13) + CHAR(10) +
		'State: ' + CONVERT(varchar(5), ERROR_STATE()) +  CHAR(13) + CHAR(10) +
		'Line: ' + CONVERT(varchar(5), ERROR_LINE()) + CHAR(13) + CHAR(10) +
		'Message: ' + ERROR_MESSAGE();

	--THROW 50000, @sErrorMsg, 1; -- Only works with SQL 2012 and beyond!
	RAISERROR(@sErrorMsg, 16, 1); -- Works with SQL 2008!
END CATCH
";
        public static string ADHOCCreateProxy
        {
            get { return adhocCreateProxy; }
        }


        // Used as a template!
        private static string adhocBlank =
@"
";
        public static string ADHOCBlank
        {
            get { return adhocBlank; }
        }

        #endregion

    } // public static class SQLScripts
} // namespace SSISPkgDeploy
