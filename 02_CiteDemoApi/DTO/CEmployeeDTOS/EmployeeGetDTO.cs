using CiteDemoApi.DTO.CAttributeDTOS;
using CiteDemoBL.Models;
using System.Text.Json.Serialization;

namespace CiteDemoApi.DTO.CEmployeeDTOS
{
    public class EmployeeGetDTO
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public bool? HasCar { get; set; }

        public string? Address { get; set; }

        public decimal? AddressLatitude { get; set; }

        public decimal? AddressLongitude { get; set; }

        public ICollection<AttributeGetDTO> Attributes { get; set; } = new List<AttributeGetDTO>();

        public Guid? SupervisorId { get; set; }

        [JsonConstructor]

        public EmployeeGetDTO() { }

        public EmployeeGetDTO(CEmployee employee)
        {
            Id = employee.Id;

            Name = employee.Name;

            DateOfBirth = employee.DateOfBirth;

            HasCar = employee.HasCar;   

            Address = employee.Address; 

            AddressLatitude = employee.AddressLatitude;

            AddressLongitude = employee.AddressLongitude;

            if(employee.Supervisor != null)
                SupervisorId = employee.Supervisor.Id;
            else 
                SupervisorId = null;    

            if(employee.Attributes != null)
            {
                foreach (var attribute in employee.Attributes)
                {
                    Attributes.Add(new AttributeGetDTO(attribute));
                }
            }
          
        }

    }
}


