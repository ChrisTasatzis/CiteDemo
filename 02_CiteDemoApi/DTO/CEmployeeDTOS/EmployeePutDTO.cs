using CiteDemoBL.Models;
using System.ComponentModel.DataAnnotations;

namespace CiteDemoApi.DTO.CEmployeeDTOS
{
    public class EmployeePutDTO
    {
        [Required(AllowEmptyStrings = false)]
        [RegularExpression("^[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}$", ErrorMessage = "Not a Valid Guid")]
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required]
        public DateTime? DateOfHire { get; set; }

        [RegularExpression("^[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}$", ErrorMessage = "Not a Valid Guid")]
        public string SupervisorId { get; set; }

        public CEmployee ToCEmployee()
        {
            return new CEmployee()
            {
                Id = new Guid(Id),
                Name = Name,
                DateOfHire = (DateTime)DateOfHire
            };
        }

    }
}
