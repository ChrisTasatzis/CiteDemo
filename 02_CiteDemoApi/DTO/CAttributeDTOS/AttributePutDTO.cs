using CiteDemoBL.Models;
using System.ComponentModel.DataAnnotations;

namespace CiteDemoApi.DTO.CAttributeDTOS
{
    public class AttributePutDTO
    {
        [Required(AllowEmptyStrings = false)]
        [RegularExpression("^[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}$", ErrorMessage = "Not a Valid Guid")]
        public string Id { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string? Value { get; set; }

        public CAttribute ToCAttribute()
        {
            return new CAttribute()
            {
                Id = new Guid(Id),
                Name = Name,
                Value = Value
            };
        }

    }
}
