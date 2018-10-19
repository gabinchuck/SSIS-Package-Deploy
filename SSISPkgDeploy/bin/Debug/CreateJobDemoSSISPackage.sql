--\ -------------------------------------------------------------------
---) Script to delete and create SQL Agent Jobs for the deployed SSIS packages.
--/ -------------------------------------------------------------------
/*
USE [msdb]; -- Comment out if run by the deployment tool.
GO -- Comment out if run by the deployment tool.
-- Parameters used when run by the deployment tool.
DECLARE @sEnvironmentType NVARCHAR(50) = N'Test'; -- Environment Name (Test, Dev, QA, UAT, PRD). Comment out if run by the deployment tool.
DECLARE @bMultiEnvPerCatalog BIT = 'False'; -- Comment out if run by the deployment tool.
DECLARE @sSSISFolderName NVARCHAR(128) = N'DemoSSISPackage'; -- Comment out if run by the deployment tool.
DECLARE @sSSISProjectName NVARCHAR(128) = N'DemoSSISPackage'; -- Comment out if run by the deployment tool.
DECLARE @sSSISProxyName NVARCHAR(128) = N''; -- Comment out if run by the deployment tool. (e.g. PayHub_Proxy, blank for no proxy)
DECLARE @sSSISCatServerName NVARCHAR(128) = N'BACKUPSERVER'; -- Comment out if run by the deployment tool.
*/
/*
SELECT * FROM msdb.dbo.sysjobs WHERE [name] LIKE 'SSIS.%' ORDER BY [name]
*/

SET NOCOUNT ON;
SET CONCAT_NULL_YIELDS_NULL OFF;

DECLARE @sName SYSNAME,
	@uID UNIQUEIDENTIFIER,
	@iID INT,
	@sErrorMsg NVARCHAR(1000),
	@ReturnCode INT,
	@bJobID BINARY(16),
	@sPkgName SYSNAME,
	@sDTSXName NVARCHAR(200),
	@sDescription NVARCHAR(512),
	@sCatName SYSNAME,
	@sSchName SYSNAME,
	@sCommand NVARCHAR(2000),
	@iStartDate INT,
	@iEndDate INT,
	@iEnvReference INT,
	@uSchID UNIQUEIDENTIFIER;
SET @ReturnCode = 0;
SET @sPkgName = N'SSIS.' + CASE WHEN @bMultiEnvPerCatalog = 'True' THEN @sEnvironmentType + '.' ELSE '' END + 'DemoSSISPackage';
SET @sDTSXName = N'DemoSSISPackage.dtsx';
SET @sSchName = N'Daily Mon - Fri at 09:00 am' + CASE WHEN @bMultiEnvPerCatalog = 'True' THEN ' (' + @sEnvironmentType + ')' ELSE '' END;
SET @sCatName = N'[Uncategorized (Local)]';
SET @iStartDate = YEAR(GETDATE()) * 10000 + MONTH(GETDATE()) * 100 + DAY(GETDATE());
--SET @iEndDate  = YEAR(DATEADD(d, 7, GETDATE())) * 10000 + MONTH(DATEADD(d, 7, GETDATE())) * 100 + DAY(DATEADD(d, 7, GETDATE())); -- Current date plus 7 days, use for testing.
SET @iEndDate = 99991231 -- December 31st 9999, No End Date.

BEGIN TRY
	BEGIN TRANSACTION;

		-- DELETE existing job(s):
		SET @sName = @sPkgName;
		SET @uID = (SELECT job_id FROM msdb.dbo.sysjobs_view WHERE [name] = @sName);
		--PRINT CASE WHEN @uID IS NULL THEN 'NULL' ELSE CONVERT(NVARCHAR(40), @uID) END;
		IF (@uID IS NOT NULL)
		BEGIN
			EXEC msdb.dbo.sp_delete_job @job_id=@uID, @delete_unused_schedule = 1; -- 1 = will delete unused schedules.
		END

		-- DELETE existing schedules:
		SET @sName = @sSchName;

		IF OBJECT_ID('TempDB..#tmpSch', 'U') IS NOT NULL
			DROP TABLE #tmpSch;
		SELECT schedule_id
		INTO #tmpSch
		FROM msdb.dbo.sysschedules WHERE [name] = @sName;

		SET @iID = (SELECT MIN(schedule_id) FROM #tmpSch);
		WHILE (@iID IS NOT NULL)
		BEGIN
			--PRINT 'schedule_id: ' + CONVERT(VARCHAR(10), @iID) + ', schedule_name: ' + @sName ;
			EXEC msdb.dbo.sp_delete_schedule @schedule_id = @iID;
			SET @iID = (SELECT MIN(schedule_id) FROM #tmpSch WHERE schedule_id > @iID);
		END

		-- Add schedule for the jobs.
		EXEC @ReturnCode = msdb.dbo.sp_add_schedule
			@schedule_name = @sSchName,
			@enabled = 1,
			@freq_type = 8,
			@freq_interval = 62,
			@freq_subday_type = 1,
			@freq_subday_interval = 0,
			@freq_relative_interval = 0,
			@freq_recurrence_factor = 1,
			@active_start_date = 20180101,
			@active_end_date = 99991231,
			@active_start_time = 090000,
			@active_end_time = 235959,
			@schedule_uid=@uSchID OUTPUT;
		--PRINT 'Schedule ID: ' + CASE WHEN @uSchID IS NULL THEN 'NULL' ELSE CONVERT(NVARCHAR(40), @uSchID) END;

		-- Get the Environment Reference ID from the SSISDB for the Project Deployment.
		-- Used in the job command line.
		SELECT @iEnvReference = ER.reference_id
		FROM SSISDB.catalog.folders F
		INNER JOIN SSISDB.catalog.environments E
		ON E.folder_id = F.folder_id
		INNER JOIN SSISDB.catalog.environment_references ER
		ON (ER.reference_type = 'A' AND ER.environment_folder_name = F.name AND ER.environment_name = E.name)
			OR (ER.reference_type = 'R' AND ER.environment_name = E.name)
		WHERE ER.environment_folder_name = @sSSISFolderName
			AND ER.environment_name = @sEnvironmentType
			AND F.name = @sSSISFolderName

		IF NOT EXISTS (SELECT [name] FROM msdb.dbo.syscategories WHERE [name] = @sCatName AND category_class = 1)
		BEGIN
			EXEC @ReturnCode = msdb.dbo.sp_add_category @class=N'JOB', @type=N'LOCAL', @name=@sCatName
		END

		-- Create New Job:
		SET @bJobID = NULL;
		SET @sDescription = N'SQL Job To Run The Demo SSIS Package ' + @sDTSXName;
		EXEC @ReturnCode = msdb.dbo.sp_add_job
			@job_name = @sPkgName,
			@enabled = 1, -- 1 = True, 0 = False.
			@notify_level_eventlog = 0,
			@notify_level_email = 0,
			@notify_level_netsend = 0,
			@notify_level_page = 0,
			@delete_level = 0,
			@description = @sDescription,
			@category_name = @sCatName,
			@job_id = @bJobID OUTPUT;

		SET @sName = N'Run Package ' + @sDTSXName;
		SET @sCommand = N'/ISSERVER "\"\SSISDB\' + @sSSISFolderName + '\' + @sSSISProjectName + '\' + @sDTSXName + '\"" /SERVER ' + @sSSISCatServerName + ' /ENVREFERENCE ' + CONVERT(VARCHAR(10), @iEnvReference) + ' /Par "\"$ServerOption::LOGGING_LEVEL(Int16)\"";1 /Par "\"$ServerOption::SYNCHRONIZED(Boolean)\"";True /CALLERINFO SQLAGENT /REPORTING E';
		EXEC @ReturnCode = msdb.dbo.sp_add_jobstep
			@job_id = @bJobID,
			@step_name = @sName,
			@step_id = 1,
			@cmdexec_success_code = 0,
			@on_success_action = 1,
			@on_success_step_id = 0,
			@on_fail_action = 2,
			@on_fail_step_id = 0,
			@retry_attempts = 0,
			@retry_interval = 0,
			@os_run_priority = 0,
			@subsystem = N'SSIS',
			@command = @sCommand,
			@database_name = N'master',
			@flags = 0,
			@proxy_name = @sSSISProxyName;

		EXEC msdb.dbo.sp_attach_schedule
			@job_id=@bJobID,
			@schedule_name=@sSchName;

		EXEC msdb.dbo.sp_add_jobserver @job_id = @bJobID;

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
