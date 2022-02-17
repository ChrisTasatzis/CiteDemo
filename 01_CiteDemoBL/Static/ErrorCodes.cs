namespace CiteDemoBL.Static
{
    public enum ErrorCodes {

        Success = 200,
        InternalError = 500,

        // Employee Related
        EmployeeNotFound = 50,
        SupervisorNotFound = 51,
        EmployeeIdAlreadyExists = 52,
        EmployeeAlreadyHasThisAttribute = 53,
        EmployeeNoAttributeId = 54,

        // Attribute Related
        AttributeNotFound = 80,
        AttributeIdAlreadyExists = 81,
        AttributeAlreadyExists = 82

    }
}
