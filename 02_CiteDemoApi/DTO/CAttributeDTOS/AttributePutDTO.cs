using CiteDemoBL.Models;
using System.ComponentModel.DataAnnotations;

namespace CiteDemoApi.DTO.CAttributeDTOS
{
    public class AttributePutDTO
    {
        [Required]
        public Guid? Id { get; set; }

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
                Id = Id,
                Name = Name,
                Value = Value
            };
        }

    }
}
