using System.ComponentModel.DataAnnotations;

namespace _03_CiteDemoPresentationLayer.Models.Employee
{
    public class InsertViewModel
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
    }
}
