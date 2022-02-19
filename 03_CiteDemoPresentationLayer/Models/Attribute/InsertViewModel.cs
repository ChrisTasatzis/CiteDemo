using CiteDemoApi.DTO.CAttributeDTOS;
using System.ComponentModel.DataAnnotations;

namespace _03_CiteDemoPresentationLayer.Models.Attribute
{
    public class InsertViewModel
    {
        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string? Name { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(100)]
        public string? Value { get; set; }

    }
}
