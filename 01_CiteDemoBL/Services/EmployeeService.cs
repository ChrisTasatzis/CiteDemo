using CiteDemoBL.Models;
using CiteDemoBL.Static;
using Microsoft.EntityFrameworkCore;

namespace CiteDemoBL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly CiteDemoDbContext _dbContext;

        public EmployeeService(CiteDemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<CEmployee>> CreateEmployee(CEmployee employee, Guid? supervisorId)
        {
            CEmployee? supervisor = null;

            if(supervisorId != null)
            {
                supervisor = await _dbContext.CEmployees.FirstOrDefaultAsync(u => u.Id == supervisorId);

                if (supervisor == null)
                {
                    return new Response<CEmployee>
                    {
                        Data = null,
                        StatusCode = ErrorCodes.SupervisorNotFound,
                        Description = "No employee with this supervisor id exists."
                    };
                }
            }

            employee.Supervisor = supervisor;

            _dbContext.CEmployees.Add(employee);

            if (await _dbContext.SaveChangesAsync() != 1)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "No changes saved."
                };

            return new Response<CEmployee>
            {
                Data = employee,
                StatusCode = ErrorCodes.Success,
                Description = "Employee added successfully."
            };
        }

        public async Task<Response<CEmployee>> ReadEmployee(Guid? id)
        {
            var employee = await  _dbContext.CEmployees
                .Include(e => e.Supervisor)
                .Include(e => e.Attributes)
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);


            if (employee == null)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.EmployeeNotFound,
                    Description = "No employee with this id exists."
                };

            return new Response<CEmployee>
            {
                Data = employee,
                StatusCode = ErrorCodes.Success,
                Description = "Employee Found."
            };
        }

        public async Task<Response<ICollection<CEmployee>>> ReadEmployee()
        {
            var employees = await _dbContext.CEmployees
                .Include(e => e.Supervisor)
                .Include(e => e.Attributes)
                .AsNoTracking()
                .ToListAsync();

            return new Response<ICollection<CEmployee>>
            {
                Data = employees,
                StatusCode = ErrorCodes.Success,
                Description = "Employees Found."
            };
        }

        public async Task<Response<CEmployee>> UpdateEmployee(CEmployee employee)
        {
            var employeeDB = await _dbContext.CEmployees
                .Include(u => u.Supervisor)
                .Include(u => u.Attributes)
                .FirstOrDefaultAsync(u => u.Id == employee.Id);

            if (employeeDB == null)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.EmployeeNotFound,
                    Description = "No employee with this id exists."
                };

            employeeDB.Copy(employee);

            if (await _dbContext.SaveChangesAsync() != 1)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "No changes saved."
                };

            return new Response<CEmployee>
            {
                Data = employee,
                StatusCode = ErrorCodes.Success,
                Description = "Employee updated successfully."
            };
    }

        public async Task<Response<CEmployee>> UpdateEmployee(CEmployee employee, Guid? supervisorId)
        {
            CEmployee? supervisor = null;

            if (supervisorId != null)
            {
                supervisor = await _dbContext.CEmployees.FirstOrDefaultAsync(u => u.Id == supervisorId);

                if (supervisor == null)
                {
                    return new Response<CEmployee>
                    {
                        Data = null,
                        StatusCode = ErrorCodes.SupervisorNotFound,
                        Description = "No employee with this supervisor id exists."
                    };
                }
            }

            employee.Supervisor = supervisor;

            return await UpdateEmployee(employee);
        }

        public async Task<Response<bool>> DeleteEmployee(Guid? id)
        {
            var employeeDB = await _dbContext.CEmployees
                .FirstOrDefaultAsync(u => u.Id == id);

            if (employeeDB == null)
                return new Response<bool>
                {
                    Data = false,
                    StatusCode = ErrorCodes.EmployeeNotFound,
                    Description = "No employee with this id exists."
                };

            _dbContext.CEmployees.Remove(employeeDB);

            if (await _dbContext.SaveChangesAsync() != 1)
                return new Response<bool>
                {
                    Data = false,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "No changes saved."
                };

            return new Response<bool>
            {
                Data = true,
                StatusCode = ErrorCodes.Success,
                Description = "Employee deleted successfully."
            };

        }

        public async Task<Response<CEmployee>> AddAttribute(Guid? employeeId, CAttribute attribute)
        {
            var employee = await _dbContext.CEmployees
                .Include(e => e.Attributes)
                .FirstOrDefaultAsync(e => e.Id == employeeId);    

            if (employee == null)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.EmployeeNotFound,
                    Description = "No employee with this id exists."
                };


            if (employee.hasAttribute(attribute.Name))
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.EmployeeAlreadyHasThisAttribute,
                    Description = "This employee already has this attribute."
                };


            employee.Attributes.Add(attribute);

            if (await _dbContext.SaveChangesAsync() != 2)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "No changes saved."
                };

            return new Response<CEmployee>
            {
                Data = employee,
                StatusCode = ErrorCodes.Success,
                Description = "Attribute added successfully."
            };

        }

        public async Task<Response<CEmployee>> RemoveAttribute(Guid? employeeId, Guid? attributeId)
        {
            var employee = await _dbContext.CEmployees
                .Include(e => e.Attributes)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.EmployeeNotFound,
                    Description = "No employee with this id exists."
                };


            if (!employee.hasAttribute(attributeId))
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.EmployeeNoAttributeId,
                    Description = "This employee does not have an attribute with this id."
                };

            var attribute = await _dbContext.CAttributes.FirstOrDefaultAsync(a => a.Id == attributeId);

            employee.Attributes.Remove(attribute);

            if (await _dbContext.SaveChangesAsync() != 1)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "No changes saved."
                };

            return new Response<CEmployee>
            {
                Data = employee,
                StatusCode = ErrorCodes.Success,
                Description = "Attribute added successfully."
            };

        }
    }
}
