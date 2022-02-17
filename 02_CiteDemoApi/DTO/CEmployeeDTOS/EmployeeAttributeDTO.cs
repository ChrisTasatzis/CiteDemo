using System.ComponentModel.DataAnnotations;

namespace CiteDemoApi.DTO.CEmployeeDTOS
{
    public class EmployeeAttributeDTO
    {

        [Required]
        public Guid? EmployeeId { get; set; }

        [Required]
        public Guid? AttributeId { get; set; }
    }
}
