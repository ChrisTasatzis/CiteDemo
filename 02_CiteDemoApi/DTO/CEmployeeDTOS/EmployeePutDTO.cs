﻿using CiteDemoBL.Models;
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
        public DateTime? DateOfBirth { get; set; }

        [Required]
        public bool? HasCar { get; set; }

        [Required(AllowEmptyStrings = false)]
        [StringLength(200)]
        public string? Address { get; set; }

        [Required]
        [Range(-90, 90)]
        public decimal? AddressLatitude { get; set; }

        [Required]
        [Range(-180, 180)]
        public decimal? AddressLongitude { get; set; }

        [RegularExpression("^[0-9a-fA-F]{8}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{4}\\-[0-9a-fA-F]{12}$", ErrorMessage = "Not a Valid Guid")]
        public string SupervisorId { get; set; }

        public CEmployee ToCEmployee()
        {
            return new CEmployee()
            {
                Id = new Guid(Id),
                Name = Name,
                DateOfBirth = (DateTime)DateOfBirth,
                HasCar = (bool)HasCar,
                Address = Address,
                AddressLatitude = (decimal)AddressLatitude,
                AddressLongitude = (decimal)AddressLongitude
            };
        }

    }
}
