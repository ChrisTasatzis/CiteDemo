using CiteDemoBL.Models;
using System.ComponentModel.DataAnnotations;

namespace CiteDemoApi.DTO.CEmployeeDTOS
{
    public class EmployeePostDTO
    {

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        public DateTime? DateOfHire { get; set; }

        [RegularExpression("^[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}$", ErrorMessage = "Not a Valid Guid")]
        public string? SupervisorId { get; set; }

        public CEmployee ToCEmployee()
        {
            return new CEmployee()
            {
                Name = Name,
                DateOfHire = (DateTime)DateOfHire
            };
        }

    }
}
