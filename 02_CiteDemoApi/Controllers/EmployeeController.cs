using CiteDemoApi.DTO.CEmployeeDTOS;
using CiteDemoBL.Models;
using CiteDemoBL.Services;
using CiteDemoBL.Static;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CiteDemoApi.Controllers
{
    [Route("Employee")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private IEmployeeService _employeeService;

        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet, Route("{id}")]
        public ActionResult<EmployeeGetDTO> GetEmployee([FromRoute] Guid id)
        {
            Response<CEmployee> response =  _employeeService.ReadEmployee(id);

            if (response.StatusCode != ErrorCodes.Success) return NotFound("No employee found with this id");

            return Ok(new EmployeeGetDTO(response.Data));
        }

        [HttpPost]
        public ActionResult<EmployeeGetDTO> PostEmployee([FromBody] EmployeePostDTO employee)
        {

            Response<CEmployee> response;

            if (employee.SupervisorId != null)
                response = _employeeService.CreateEmployee(employee.ToCEmployee(), new Guid(employee.SupervisorId));
            else
                response = _employeeService.CreateEmployee(employee.ToCEmployee());
            

            if (response.StatusCode != ErrorCodes.Success) return BadRequest("Could not add employee");

            return Ok(new EmployeeGetDTO(response.Data));

        }

        [HttpPut]
        public ActionResult<EmployeeGetDTO> PutEmployee([FromBody] EmployeePutDTO employee)
        {

            Response<CEmployee> response;

            if (employee.SupervisorId != null)
                response = _employeeService.UpdateEmployee(employee.ToCEmployee(), new Guid(employee.SupervisorId));
            else
                response = _employeeService.UpdateEmployee(employee.ToCEmployee());


            if (response.StatusCode != ErrorCodes.Success) return BadRequest(response.Description);

            return Ok(new EmployeeGetDTO(response.Data));

        }

        [HttpDelete, Route("{id}")]
        public ActionResult<bool> DeleteEmployee([FromRoute] Guid id)
        {
            Response<bool> response = _employeeService.DeleteEmployee(id);

            if (response.StatusCode != ErrorCodes.Success) return NotFound("No employee found with this id");

            return Ok(response.Data);
        }
    }
}
