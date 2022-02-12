using CiteDemoBL.Models;

namespace CiteDemoApi.DTO.CEmployeeDTOS
{
    public class EmployeeGetDTO
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public DateTime? DateOfHire { get; set; }

        public Guid? SupervisorId { get; set; }

        public EmployeeGetDTO(CEmployee employee)
        {
            Id = employee.Id;

            Name = employee.Name;

            DateOfHire = employee.DateOfHire;

            if(employee.Supervisor != null)
                SupervisorId = employee.Supervisor.Id;
            else 
                SupervisorId = null;    
        }

    }
}
