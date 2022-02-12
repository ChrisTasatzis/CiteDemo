DECLARE @EmployeeId UNIQUEIDENTIFIER
DECLARE @EmployeeIsSupervisor INT
DECLARE @EmployeeHasHeight INT
DECLARE @HeightShortID UNIQUEIDENTIFIER

PRINT 'Getting the ID of the attribute with name Height and value Short'
SET @HeightShortID = (SELECT TOP(1) Attribute.ATTR_ID 
					FROM Attribute
					WHERE ATTR_Name = 'Height'
					AND ATTR_Value = 'Short');
PRINT 'ID = ' + CONVERT(VARCHAR(50), @HeightShortID)


PRINT 'Creating a cursor with all the employee''s ids'
DECLARE employee_cursor CURSOR FOR 
SELECT Employee.EMP_ID 
FROM Employee
OPEN employee_cursor  

FETCH NEXT FROM employee_cursor INTO @EmployeeId  
PRINT 'Loop through all employees'
WHILE @@FETCH_STATUS = 0  
BEGIN  

	-- Check if employee is supervisor to others 
	SET @EmployeeIsSupervisor = (SELECT COUNT(*)
			FROM Employee
			WHERE EMP_Supervisor = @EmployeeId)

	-- Check if employee has attribute height
	SET @EmployeeHasHeight = (SELECT COUNT(*)
			FROM EmployeeAttribute
			JOIN Attribute ON EmployeeAttribute.EMPATTR_AttributeID = Attribute.ATTR_ID
			WHERE ATTR_Name = 'Height' AND EMPATTR_EmployeeID = @EmployeeId)

	IF @EmployeeIsSupervisor > 0 
	BEGIN
		-- Update or insert accordingly
		IF @EmployeeHasHeight > 0
		BEGIN
			PRINT 'Employee with id ' + CONVERT(VARCHAR(50), @EmployeeId) + ' is supervisor and has attribute Height'
			UPDATE EmployeeAttribute
			SET EMPATTR_AttributeID = @HeightShortID
			WHERE EMPATTR_EmployeeID = @EmployeeId AND EMPATTR_AttributeID IN (SELECT Attribute.ATTR_ID FROM Attribute WHERE ATTR_Name = 'Height')
			PRINT 'Attribute Updated'
		END
		ELSE
		BEGIN
			PRINT 'Employee with id ' + CONVERT(VARCHAR(50), @EmployeeId) + ' is supervisor and dosen''t have attribute Height'
			INSERT INTO EmployeeAttribute (EMPATTR_EmployeeID, EMPATTR_AttributeID)	
			VALUES (@EmployeeId, @HeightShortID)
			PRINT 'Attribute Added'
		END 
	END 

    FETCH NEXT FROM employee_cursor INTO @EmployeeId 
END 

CLOSE employee_cursor  
DEALLOCATE employee_cursor  
GO 

-- Show the desired output
SELECT Employee.EMP_Name, Attribute.ATTR_Name, Attribute.ATTR_Value
FROM EmployeeAttribute
JOIN Attribute ON EmployeeAttribute.EMPATTR_AttributeID = Attribute.ATTR_ID
JOIN Employee ON EmployeeAttribute.EMPATTR_EmployeeID = Employee.EMP_ID
WHERE ATTR_Name = 'Height'  
GO
