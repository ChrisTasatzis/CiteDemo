using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CiteDemoBL.Models
{
    [Table("Attribute")]
    public class CAttribute
    {
        [Key]
        [Column("ATTR_ID")]
        public Guid Id { get; set; }
        [Column("ATTR_Name")]
        public string? Name { get; set; }
        [Column("ATTR_Value")]
        public string? Value { get; set; }
        public virtual ICollection<CEmployee>? Employees { get; set; }

        public void Copy(CAttribute attribute)
        {
            this.Name = attribute.Name;

            this.Value = attribute.Value;
        }
    }
}
