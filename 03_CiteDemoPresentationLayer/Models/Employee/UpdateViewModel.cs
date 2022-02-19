using System.ComponentModel.DataAnnotations;

namespace _03_CiteDemoPresentationLayer.Models.Employee
{
    public class UpdateViewModel
    { 
        [Required]
        public Guid Id { get; set; }

        [Required]
        [Range(-90, 90)]
        public decimal? AddressLatitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public decimal? AddressLongitude { get; set; }

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
