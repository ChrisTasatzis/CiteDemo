using CiteDemoBL.Models;
using CiteDemoBL.Static;
using Microsoft.EntityFrameworkCore;

namespace CiteDemoBL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private CiteDemoDbContext _dbContext;

        public EmployeeService(CiteDemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Response<CEmployee> CreateEmployee(CEmployee employee, Guid? supervisorId)
        {

            var supervisor = _dbContext.CEmployees.FirstOrDefault(u => u.Id == supervisorId);

            if (supervisor == null)
            {
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.SupervisorNotFound,
                    Description = "No employee with this supervisor id exists."
                };
            }

            employee.Supervisor = supervisor;

            return CreateEmployee(employee);
        }

        public Response<CEmployee> CreateEmployee(CEmployee employee)
        {
            _dbContext.CEmployees.Add(employee);

            if (_dbContext.SaveChanges() != 1)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "Could not save changes."
                };

            return new Response<CEmployee>
            {
                Data = employee,
                StatusCode = ErrorCodes.Success,
                Description = "Employee added successfully."
            };
        }

        public Response<CEmployee> ReadEmployee(Guid id)
        {
            var employee = _dbContext.CEmployees
                .Include(e => e.Supervisor)
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);


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

        public Response<CEmployee> UpdateEmployee(CEmployee employee)
        {
            var employeeDB = _dbContext.CEmployees
                .Include(u => u.Supervisor)
                .FirstOrDefault(u => u.Id == employee.Id);

            if (employeeDB == null)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.EmployeeNotFound,
                    Description = "No employee with this id exists."
                };

            employeeDB.Copy(employee);

            if (_dbContext.SaveChanges() != 1)
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "Could not save changes."
                };

            return new Response<CEmployee>
            {
                Data = employee,
                StatusCode = ErrorCodes.Success,
                Description = "Employee updated successfully."
            };
    }

        public Response<CEmployee> UpdateEmployee(CEmployee employee, Guid? supervisorId)
        {
            var supervisor = _dbContext.CEmployees.FirstOrDefault(u => u.Id == supervisorId);

            if (supervisor == null)
            {
                return new Response<CEmployee>
                {
                    Data = null,
                    StatusCode = ErrorCodes.SupervisorNotFound,
                    Description = "No employee with this supervisor id exists."
                };
            }

            employee.Supervisor = supervisor;

            return UpdateEmployee(employee);
        }

        public Response<bool> DeleteEmployee(Guid id)
        {
            var employeeDB = _dbContext.CEmployees
                .FirstOrDefault(u => u.Id == id);

            if (employeeDB == null)
                return new Response<bool>
                {
                    Data = false,
                    StatusCode = ErrorCodes.EmployeeNotFound,
                    Description = "No employee with this id exists."
                };

            _dbContext.CEmployees.Remove(employeeDB);

            if (_dbContext.SaveChanges() != 1)
                return new Response<bool>
                {
                    Data = false,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "Could not save changes."
                };

            return new Response<bool>
            {
                Data = true,
                StatusCode = ErrorCodes.Success,
                Description = "Employee deleted successfully."
            };

        }

    
    }
}
