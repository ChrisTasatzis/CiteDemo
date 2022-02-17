using CiteDemoBL.Models;
using CiteDemoBL.Static;

namespace CiteDemoBL.Services
{
    public interface IEmployeeService
    {
        Task<Response<CEmployee>> CreateEmployee(CEmployee employee, Guid? supervisorId);

        Task<Response<ICollection<CEmployee>>> ReadEmployee();

        Task<Response<CEmployee>> ReadEmployee(Guid? id);

        Task<Response<CEmployee>> UpdateEmployee(CEmployee employee, Guid? supervisorId);

        Task<Response<bool>> DeleteEmployee(Guid? id);

        Task<Response<CEmployee>> AddAttribute(Guid? employeeId, CAttribute attribute);

        Task<Response<CEmployee>> RemoveAttribute(Guid? employeeId, Guid? attributeId);
    }
}
