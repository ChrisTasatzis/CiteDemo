using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiteDemoBL.Models
{
    [Table("Employee")]
    public class CEmployee
    {
        [Key]
        [Column("EMP_ID")]
        public Guid Id { get; set; }

        [Column("EMP_Name")]
        public string Name { get; set; }

        [Column("EMP_DateOfBirth")]
        public DateTime DateOfBirth { get; set; }

        [Column("EMP_HasCar")]
        public bool HasCar { get; set; }

        [Column("EMP_Address")]
        public string Address { get; set; }

        [Column("EMP_AddressLatitude")]
        public decimal AddressLatitude { get; set; }

        [Column("EMP_AddressLongitude")]
        public decimal AddressLongitude { get; set; }

        [ForeignKey("EMP_Supervisor")]
        public virtual CEmployee? Supervisor { get; set; }

        public virtual ICollection<CAttribute>? Attributes { get; set; }

        public void Copy(CEmployee employee)
        {
            this.Name = employee.Name;

            this.DateOfBirth = employee.DateOfBirth;

            this.HasCar = employee.HasCar;

            this.Address = employee.Address;

            this.AddressLatitude = employee.AddressLatitude;

            this.AddressLongitude = employee.AddressLongitude;

            this.Supervisor = employee.Supervisor;

            this.Attributes = employee.Attributes;
        }

        public bool hasAttribute(string attributeName)
        {
            return this.Attributes.Any(a => a.Name == attributeName);
        }

        public bool hasAttribute(Guid? attributeId)
        {
            return this.Attributes.Any(a => a.Id == attributeId);
        }
    }
}
