using CiteDemoBL.Models;
using CiteDemoBL.Static;

namespace CiteDemoBL.Services
{
    public interface IEmployeeService
    {
        Response<CEmployee> CreateEmployee(CEmployee employee, Guid? supervisorId);

        Response<CEmployee> CreateEmployee(CEmployee employee);

        Response<CEmployee> ReadEmployee(Guid id);

        Response<CEmployee> UpdateEmployee(CEmployee employee);

        Response<CEmployee> UpdateEmployee(CEmployee employee, Guid? supervisorId);

        Response<bool> DeleteEmployee(Guid id);

    }
}
