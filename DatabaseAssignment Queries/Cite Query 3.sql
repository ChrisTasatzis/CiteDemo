IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'AddTeamToSupervised') AND type in (N'P', N'PC'))
  DROP PROCEDURE [dbo].AddTeamToSupervised
GO
PRINT 'Creating Stored Procedure'
GO
CREATE PROCEDURE AddTeamToSupervised @EmployeeId UNIQUEIDENTIFIER
AS

	DECLARE @EmployeeName AS NVARCHAR(50)
	DECLARE @EmployeeHasTeam AS INT
	DECLARE @AttributeTeam AS INT
	DECLARE @AttributeTeamID AS UNIQUEIDENTIFIER
	DECLARE @UNIQUEX UNIQUEIDENTIFIER
	-- Create a new UNIQUEIDENTIFIER
	SET @UNIQUEX = NEWID(); 

	-- Check if an employee with the given id exists
	SET @EmployeeName = (SELECT EMP_Name
			FROM Employee
			WHERE EMP_ID = @EmployeeId)

	-- If not exists exit 
	IF @EmployeeName = NULL 
	BEGIN
		PRINT 'No employee with this ID exists'
		RETURN 
	END 

	PRINT 'The employee with this id is ' + @EmployeeName

	-- Check if attribute with name team and value the given id exists
	SET @AttributeTeam = (SELECT COUNT(*)
			FROM Attribute
			WHERE ATTR_Name = 'Team' AND ATTR_Value = @EmployeeName)

	-- Insert if necessary
	IF @AttributeTeam = 0 
	BEGIN
		PRINT 'Inserting an attribute with Name Team and Value ' + @EmployeeName + ' as it didn''t exists before'
		INSERT INTO Attribute (ATTR_ID, ATTR_Name, ATTR_Value)
		VALUES (@UNIQUEX, 'Team', @EmployeeName)
	END

	-- Get attribute ID
	SET @AttributeTeamID = (SELECT ATTR_ID
			FROM Attribute
			WHERE ATTR_Name = 'Team' AND ATTR_Value = @EmployeeName)


	-- Get all supervied employees (using recursion)
	PRINT 'Getting all the employees this employee supervises (on all levels)'
	DECLARE employee_cursor CURSOR FOR 
	WITH boss (ID,PARENTID) AS (
		SELECT  EMP_ID,
				EMP_Supervisor
		FROM    Employee
		WHERE   EMP_Supervisor = @EmployeeId
	),
	 bossChild (ID,PARENTID) AS (
		SELECT  ID,
				PARENTID
		FROM    boss
		UNION ALL
		SELECT  Employee.EMP_ID,
				Employee.EMP_Supervisor
		FROM    Employee  INNER JOIN
				bossChild  ON Employee.EMP_Supervisor = bossChild.ID
		WHERE   Employee.EMP_ID NOT IN (SELECT PARENTID FROM boss)
	)
	SELECT  ID
	FROM    bossChild
	OPTION (MAXRECURSION 0)

	OPEN employee_cursor  

	FETCH NEXT FROM employee_cursor INTO @EmployeeId  
	PRINT 'Loop through all employees this employee supervises'
	WHILE @@FETCH_STATUS = 0  
	BEGIN 
		-- Check if employee has attribute Team
		SET @EmployeeHasTeam = (SELECT COUNT(*)
				FROM EmployeeAttribute
				JOIN Attribute ON EmployeeAttribute.EMPATTR_AttributeID = Attribute.ATTR_ID
				WHERE ATTR_Name = 'Team' AND EMPATTR_EmployeeID = @EmployeeId)

		-- Update or insert accordingly
		IF @EmployeeHasTeam > 0 
		BEGIN
			PRINT 'Employee with id ' + CONVERT(VARCHAR(50), @EmployeeId) + ' has attribute Team'
			UPDATE EmployeeAttribute
			SET EMPATTR_AttributeID = @AttributeTeamID
			WHERE EMPATTR_EmployeeID = @EmployeeId AND EMPATTR_AttributeID IN (SELECT Attribute.ATTR_ID FROM Attribute WHERE ATTR_Name = 'Team')
			PRINT 'Attribute Updated'
		END
		ELSE
		BEGIN
			PRINT 'Employee with id ' + CONVERT(VARCHAR(50), @EmployeeId) + ' dosen''t have attribute Team'
			INSERT INTO EmployeeAttribute (EMPATTR_EmployeeID, EMPATTR_AttributeID)	
			VALUES (@EmployeeId, @AttributeTeamID)
			PRINT 'Attribute Added'
		END 

		FETCH NEXT FROM employee_cursor INTO @EmployeeId 
	END 

	CLOSE employee_cursor  
	DEALLOCATE employee_cursor
GO

PRINT 'Executing Stored Procedure'
EXEC AddTeamToSupervised @EmployeeId = '82D58D49-72A2-42B0-A250-471E5C10D7D9';
GO 

-- Show the desired output
SELECT Employee.EMP_Name, Attribute.ATTR_Name, Attribute.ATTR_Value
FROM EmployeeAttribute
JOIN Attribute ON EmployeeAttribute.EMPATTR_AttributeID = Attribute.ATTR_ID
JOIN Employee ON EmployeeAttribute.EMPATTR_EmployeeID = Employee.EMP_ID
WHERE ATTR_Name = 'Team' 
GO 