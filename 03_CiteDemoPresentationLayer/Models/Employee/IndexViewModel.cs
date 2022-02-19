using CiteDemoApi.DTO.CEmployeeDTOS;

namespace _03_CiteDemoPresentationLayer.Models.Employee
{
    public class IndexViewModel
    {
        public List<EmployeeGetDTO> Employees { get; set; }

        public EmployeePostDTO EmployeePost { get; set; } 

        public string EmployeeDeleteId { get; set; }   
    }
}
