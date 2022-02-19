using CiteDemoApi.DTO.CAttributeDTOS;
using CiteDemoApi.DTO.CEmployeeDTOS;

namespace _03_CiteDemoPresentationLayer.Models.Employee
{
    public class AttributesViewModel
    {
        public EmployeeGetDTO Employee { get; set; }

        public List<AttributeGetDTO> Attributes { get; set; }
    }
}
