DECLARE @EmployeeId UNIQUEIDENTIFIER
DECLARE @EmployeeHasWeight int
DECLARE @WeightThinID UNIQUEIDENTIFIER

PRINT 'Getting the ID of the attribute with name Weight and value Thin'
SET @WeightThinID = (SELECT Attribute.ATTR_ID 
					FROM Attribute
					WHERE ATTR_Name = 'Weight'
					AND ATTR_Value = 'Thin')
PRINT 'ID = ' + CONVERT(VARCHAR(50), @WeightThinID)

PRINT 'Creating a cursor with all the employee''s ids'
DECLARE employee_cursor CURSOR FOR 
SELECT Employee.EMP_ID 
FROM Employee
OPEN employee_cursor  


FETCH NEXT FROM employee_cursor INTO @EmployeeId  
PRINT 'Loop through all employees'
WHILE @@FETCH_STATUS = 0  
BEGIN  
	-- Check if employee has attribute weight
	SET @EmployeeHasWeight = (SELECT COUNT(*)
			FROM EmployeeAttribute
			JOIN Attribute ON EmployeeAttribute.EMPATTR_AttributeID = Attribute.ATTR_ID
			WHERE ATTR_Name = 'Weight' AND EMPATTR_EmployeeID = @EmployeeId)

	-- Update or insert accordingly
	IF @EmployeeHasWeight > 0 
	BEGIN
		PRINT 'Employee with id ' + CONVERT(VARCHAR(50), @EmployeeId) + ' has attribute Weight'
		UPDATE EmployeeAttribute
		SET EMPATTR_AttributeID = @WeightThinID
		WHERE EMPATTR_EmployeeID = @EmployeeId AND EMPATTR_AttributeID IN (SELECT Attribute.ATTR_ID FROM Attribute WHERE ATTR_Name = 'Weight')
		PRINT 'Attribute Updated'
	END
	ELSE
	BEGIN
		PRINT 'Employee with id ' + CONVERT(VARCHAR(50), @EmployeeId) + ' doesn''t have attribute Weight'
		INSERT INTO EmployeeAttribute (EMPATTR_EmployeeID, EMPATTR_AttributeID)	
		VALUES (@EmployeeId, @WeightThinID)
		PRINT 'Attribute Added'
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
WHERE ATTR_Name = 'Weight'  
GO

