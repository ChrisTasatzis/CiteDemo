using CiteDemoApi.DTO.CEmployeeDTOS;
using CiteDemoBL.Models;
using CiteDemoBL.Services;
using CiteDemoBL.Static;
using Geocoding.Google;
using Microsoft.AspNetCore.Mvc;
using GoogleApi.Entities.Maps.Directions.Response;
using System.Net.Http.Headers;
using System.Globalization;

namespace CiteDemoApi.Controllers
{
    [Route("Api/Employee")]
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IAttributeService _attributeService;
        private readonly string APIKEY = "API_KEY_HERE";

        public EmployeeController(IEmployeeService employeeService, IAttributeService attributeService)
        {
            _employeeService = employeeService;
            _attributeService = attributeService;
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult<EmployeeGetDTO>> GetEmployee([FromRoute] Guid id)
        {
            Response<CEmployee> response =  await _employeeService.ReadEmployee(id);

            if (response.StatusCode != ErrorCodes.Success) return NotFound(response.Description);

            return Ok(new EmployeeGetDTO(response.Data));
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<EmployeeGetDTO>>> GetEmployee()
        {
            Response<ICollection<CEmployee>> response = await _employeeService.ReadEmployee();

            if (response.StatusCode != ErrorCodes.Success) return NotFound(response.Description);

            var result = new List<EmployeeGetDTO>();

            foreach (var employee in response.Data)
            {
                result.Add(new EmployeeGetDTO(employee));
            }

            return Ok(result);
        }

        [HttpGet, Route("Attribute/{id}")]
        public async Task<ActionResult<ICollection<EmployeeGetDTO>>> GetEmployeeByAttributeId(Guid id)
        {
            Response<ICollection<CEmployee>> response = await _employeeService.ReadEmployeesByAttribute(id);

            if (response.StatusCode != ErrorCodes.Success) return NotFound(response.Description);

            var result = new List<EmployeeGetDTO>();

            foreach (var employee in response.Data)
            {
                result.Add(new EmployeeGetDTO(employee));
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<EmployeeGetDTO>> PostEmployee([FromBody] EmployeePostDTO employee)
        {
            var response = await _employeeService.CreateEmployee(employee.ToCEmployee(), employee.SupervisorId);

            if (response.StatusCode != ErrorCodes.Success) return BadRequest(response.Description);

            return Ok(new EmployeeGetDTO(response.Data));

        }

        [HttpPost, Route("Geocoding")]
        public async Task<ActionResult<EmployeeGetDTO>> PostEmployeeGeocoding([FromBody] EmployeePostGeocodingDTO employee)
        {
            CEmployee employeeObj = employee.ToCEmployee();

            try
            {
                GoogleGeocoder geocoder = new GoogleGeocoder() { ApiKey = this.APIKEY };
                IEnumerable<Geocoding.Address> addresses = await geocoder.GeocodeAsync(employee.Address);

                if(!addresses.Any()) return BadRequest("Invalid address.");

                employeeObj.AddressLatitude = (decimal)addresses.First().Coordinates.Latitude;
                employeeObj.AddressLongitude = (decimal)addresses.First().Coordinates.Longitude;
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error within the google geocoding api occured");
            }

            var response = await _employeeService.CreateEmployee(employeeObj, employee.SupervisorId);

            if (response.StatusCode != ErrorCodes.Success) return BadRequest(response.Description);

            return Ok(new EmployeeGetDTO(response.Data));
        }

        [HttpPost, Route("Attribute")]
        public async Task<ActionResult<EmployeeGetDTO>> PostEmployeeAddAttribute([FromBody] EmployeeAttributeDTO dto)
        {

            var attributeResponse = await _attributeService.ReadAttribute(dto.AttributeId);

            if (attributeResponse.StatusCode != ErrorCodes.Success) return BadRequest(attributeResponse.Description);

            var attribute = attributeResponse.Data;

            var response = await _employeeService.AddAttribute(dto.EmployeeId, attribute);

            if (response.StatusCode != ErrorCodes.Success) return BadRequest(response.Description);

            return Ok(new EmployeeGetDTO(response.Data));
        }

        [HttpPut]
        public async Task<ActionResult<EmployeeGetDTO>> PutEmployee([FromBody] EmployeePutDTO employee)
        {

            var response = await _employeeService.UpdateEmployee(employee.ToCEmployee(), employee.SupervisorId);

            if (response.StatusCode != ErrorCodes.Success) return BadRequest(response.Description);

            return Ok(new EmployeeGetDTO(response.Data));

        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult<bool>> DeleteEmployee([FromRoute] Guid id)
        {
            Response<bool> response = await _employeeService.DeleteEmployee(id);

            if (response.StatusCode != ErrorCodes.Success) return NotFound(response.Description);

            return Ok(response.Description);
        }

        [HttpDelete, Route("Attribute")]
        public async Task<ActionResult<EmployeeGetDTO>> DeleteEmployeeRemoveAttribute([FromQuery] Guid employeeId, [FromQuery] Guid attributeId)
        {

            var response = await _employeeService.RemoveAttribute(employeeId, attributeId);

            if (response.StatusCode != ErrorCodes.Success) return BadRequest(response.Description);

            return Ok(new EmployeeGetDTO(response.Data));
        }

        [HttpGet, Route("Directions")]
        public async Task<ActionResult<DirectionsResponse>> GetDirections([FromQuery] Guid employeeFromId, [FromQuery] Guid employeeToId)
        {
            DirectionsResponse directionsResponse;

            Response<CEmployee> responseFrom = await _employeeService.ReadEmployee(employeeFromId);

            if (responseFrom.StatusCode != ErrorCodes.Success) return NotFound($"No employee found with this id {employeeFromId}.");

            Response<CEmployee> responseTo = await _employeeService.ReadEmployee(employeeToId);

            if (responseTo.StatusCode != ErrorCodes.Success) return NotFound($"No employee found with this id {employeeToId}.");

            var employeeFrom = responseFrom.Data;
            var employeeTo = responseTo.Data;

            using (var client = new HttpClient())
            {
                // Define the base client
                client.BaseAddress = new Uri("https://maps.googleapis.com/maps/api/directions/json");
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Format strings with .
                var fromLat = employeeFrom.AddressLatitude.ToString(CultureInfo.InvariantCulture);
                var fromLong = employeeFrom.AddressLongitude.ToString(CultureInfo.InvariantCulture);
                var toLat = employeeTo.AddressLatitude.ToString(CultureInfo.InvariantCulture);
                var toLong = employeeTo.AddressLongitude.ToString(CultureInfo.InvariantCulture);

                // Add Query Parameters
                var query = $"?key={APIKEY}";
                query += $"&origin= {{ lat: {fromLat}, lng: {fromLong} }}";
                query += $"&destination= {{ lat: {toLat}, lng: {toLong} }}";

                if(employeeFrom.HasCar)
                    query += $"&mode= DRIVING";
                else
                    query += $"&mode= WALKING";

                //HTTP Get
                HttpResponseMessage result = await client.GetAsync(query);

                // Show error message when not possible 
                if (!result.IsSuccessStatusCode)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "An error within the google directions api occured.");
                }

                var directionsJSON = await result.Content.ReadAsStringAsync();

                return Ok(directionsJSON);
            }

        }
    }
}
