using CiteDemoApi.DTO.CEmployeeDTOS;
using CiteDemoBL.Models;
using CiteDemoBL.Services;
using CiteDemoBL.Static;
using Geocoding;
using Geocoding.Google;
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

        [HttpGet]
        public ActionResult<ICollection<EmployeeGetDTO>> GetEmployee()
        {
            Response<ICollection<CEmployee>> response = _employeeService.ReadEmployee();

            if (response.StatusCode != ErrorCodes.Success) return NotFound("No employee found with this id");

            var result = new List<EmployeeGetDTO>();

            foreach (var employee in response.Data)
            {
                result.Add(new EmployeeGetDTO(employee));
            }

            return Ok(result);
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

        [HttpPost, Route("Geocoding")]
        public async Task<ActionResult<EmployeeGetDTO>> PostEmployeeGeocoding([FromBody] EmployeePostGeocodingDTO employee)
        {
            CEmployee employeeObj = employee.ToCEmployee();
            Response<CEmployee> response;

            try
            {
                GoogleGeocoder geocoder = new GoogleGeocoder() { ApiKey = "api key here " };
                IEnumerable<Address> addresses = await geocoder.GeocodeAsync(employee.Address);

                employeeObj.AddressLatitude = (decimal)addresses.First().Coordinates.Latitude;
                employeeObj.AddressLongitude = (decimal)addresses.First().Coordinates.Longitude;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error within the google geocoding api occured");
            }

            if (employee.SupervisorId != null)
                response = _employeeService.CreateEmployee(employeeObj, new Guid(employee.SupervisorId));
            else
                response = _employeeService.CreateEmployee(employeeObj);


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
