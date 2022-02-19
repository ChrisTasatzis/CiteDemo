using _03_CiteDemoPresentationLayer.Models;
using _03_CiteDemoPresentationLayer.Models.Employee;
using CiteDemoApi.DTO.CAttributeDTOS;
using CiteDemoApi.DTO.CEmployeeDTOS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web;

namespace _03_CiteDemoPresentationLayer.Controllers
{
    public class EmployeeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<EmployeeGetDTO> Employees = new List<EmployeeGetDTO>();

            using (var client = new HttpClient())
            {
                // Define the base client
                client.BaseAddress = new Uri("https://localhost:7252/");
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP GET
                HttpResponseMessage result = await client.GetAsync("api/Employee");

                if (!result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                else
                {
                    var EmployeesJSON = await result.Content.ReadAsStringAsync();

                    Employees = JsonConvert.DeserializeObject<List<EmployeeGetDTO>>(EmployeesJSON);
                }

            }

            return View(new IndexViewModel()
            {
                Employees = Employees
            });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {

            using (var client = new HttpClient())
            {
                // Define the base client
                client.BaseAddress = new Uri("https://localhost:7252");
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP Delete
                HttpResponseMessage result = await client.DeleteAsync("api/Employee/" + id);

                // Show error message when not possible 
                if (!result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    ModelState.AddModelError(string.Empty, response);

                    return View("Error", new ErrorViewModel()
                    {
                        RequestId = response
                    });
                }

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Insert(InsertViewModel model)
        {

            using (var client = new HttpClient())
            {
                // Define the base client
                client.BaseAddress = new Uri("https://localhost:7252");
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Create Post body 
                var body = new EmployeePostDTO()
                {
                    Name = model.Name,
                    DateOfBirth = model.DateOfBirth,
                    HasCar = model.HasCar,
                    Address = model.Address
                };

                //HTTP Post
                HttpResponseMessage result = await client.PostAsJsonAsync("api/Employee/Geocoding", body);

                if (!result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    ModelState.AddModelError(string.Empty, response);
                    return View(model);
                }

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public IActionResult Update(UpdateViewModel model)
        {
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePost(UpdateViewModel model)
        {

            using (var client = new HttpClient())
            {
                // Define the base client
                client.BaseAddress = new Uri("https://localhost:7252");
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Create Post body 
                var body = new EmployeePutDTO()
                {
                    Id = model.Id,
                    Name = model.Name,
                    DateOfBirth = model.DateOfBirth,
                    HasCar = model.HasCar,
                    Address = model.Address,
                    AddressLatitude = model.AddressLatitude,
                    AddressLongitude = model.AddressLongitude,
                };

                //HTTP Put
                HttpResponseMessage result = await client.PutAsJsonAsync("api/Employee", body);

                if (!result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    ModelState.AddModelError(string.Empty, response);
                    return View("Update", model);
                }

                return RedirectToAction("Index");
            }
        }

        [HttpGet]
        public async Task<IActionResult> Attributes(string id)
        {
            EmployeeGetDTO Employee = new EmployeeGetDTO();

            List<AttributeGetDTO> Attributes = new List<AttributeGetDTO>();

            using (var client = new HttpClient())
            {
                // Define the base client
                client.BaseAddress = new Uri("https://localhost:7252/");
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP GET
                HttpResponseMessage result = await client.GetAsync("api/Employee/" + id);

                if (!result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                else
                {
                    var EmployeesJSON = await result.Content.ReadAsStringAsync();

                    Employee = JsonConvert.DeserializeObject<EmployeeGetDTO>(EmployeesJSON);
                }

            }

            // Should be replaced with a method that returns all available attributes for this employee
            using (var client = new HttpClient())
            {
                // Define the base client
                client.BaseAddress = new Uri("https://localhost:7252/");
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP GET
                HttpResponseMessage result = await client.GetAsync("api/Attribute/");

                if (!result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                else
                {
                    var AttributesJSON = await result.Content.ReadAsStringAsync();

                    Attributes = JsonConvert.DeserializeObject<List<AttributeGetDTO>>(AttributesJSON);
                }

            }

            return View(new AttributesViewModel()
            {
                Employee = Employee,
                Attributes = Attributes
            });
        }

        [HttpPost]
        public async Task<IActionResult> RemoveAttribute(EmployeeAttributeDTO dto)
        {
            using (var client = new HttpClient())
            {
                // Define the base client
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var uriBuilder = new UriBuilder("https://localhost:7252/api/Employee/Attribute");
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                query["employeeId"] = dto.EmployeeId.ToString();
                query["attributeId"] = dto.AttributeId.ToString();
                uriBuilder.Query = query.ToString();

                //HTTP Delete
                HttpResponseMessage result = await client.DeleteAsync(uriBuilder.ToString());

                // Show error message when not possible 
                if (!result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    ModelState.AddModelError(string.Empty, response);

                    return View("Error", new ErrorViewModel()
                    {
                        RequestId = response
                    });
                }

                return RedirectToAction("Attributes", new { id = dto.EmployeeId });
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAttribute(EmployeeAttributeDTO dto)
        {
            using (var client = new HttpClient())
            {
                // Define the base client
                client.BaseAddress = new Uri("https://localhost:7252");
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP Post
                HttpResponseMessage result = await client.PostAsJsonAsync("api/Employee/Attribute", dto);

                // Show error message when not possible 
                if (!result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    ModelState.AddModelError(string.Empty, response);

                    return View("Error", new ErrorViewModel()
                    {
                        RequestId = response
                    });
                }

                return RedirectToAction("Attributes", new { id = dto.EmployeeId });
            }
        }
    }
}

