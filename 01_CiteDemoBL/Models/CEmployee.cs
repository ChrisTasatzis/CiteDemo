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

        [Required]
        [Column("EMP_Name")]
        public string Name { get; set; }

        [Required]
        [Column("EMP_DateOfHire")]
        public DateTime DateOfHire { get; set; }

        [ForeignKey("EMP_Supervisor")]
        public virtual CEmployee? Supervisor { get; set; }

        public virtual ICollection<CAttribute>? Attributes { get; set; }

        public void Copy(CEmployee employee)
        {
            this.Name = employee.Name;

            this.DateOfHire = employee.DateOfHire;

            this.Supervisor = employee.Supervisor;
        }
    }
}
