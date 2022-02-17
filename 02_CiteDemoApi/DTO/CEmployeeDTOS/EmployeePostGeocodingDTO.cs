using CiteDemoBL.Models;
using System.ComponentModel.DataAnnotations;

namespace CiteDemoApi.DTO.CEmployeeDTOS
{
    public class EmployeePostGeocodingDTO
    {

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public bool? HasCar { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string? Address { get; set; }

        public Guid? SupervisorId { get; set; }

        public CEmployee ToCEmployee()
        {
            return new CEmployee()
            {
                Name = Name,
                DateOfBirth = (DateTime)DateOfBirth,
                HasCar = (bool)HasCar,
                Address = Address,
            };
        }

    }
}
