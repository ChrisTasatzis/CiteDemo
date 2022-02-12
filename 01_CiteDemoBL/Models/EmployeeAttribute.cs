using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CiteDemoBL.Models
{
    public class EmployeeAttribute
    {
        [Column("EMPATTR_EmployeeID")]
        public Guid EMPATTR_EmployeeID { get; set; }

        [Column("EMPATTR_AttributeID")]
        public Guid EMPATTR_AttributeID { get; set; }

        public virtual CEmployee? Employee { get; set; }
        public virtual CAttribute? Attribute { get; set; }

    }
}
