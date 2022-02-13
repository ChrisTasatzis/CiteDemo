using CiteDemoBL.Models;
using System.ComponentModel.DataAnnotations;

namespace CiteDemoApi.DTO.CAttributeDTOS
{
    public class AttributePostDTO
    {

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
                Name = Name,
                Value = Value
            };
        }

    }
}
