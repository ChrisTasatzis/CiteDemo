using _03_CiteDemoPresentationLayer.Models;
using _03_CiteDemoPresentationLayer.Models.Attribute;
using CiteDemoApi.DTO.CAttributeDTOS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Web.Http;
using HttpGetAttribute = Microsoft.AspNetCore.Mvc.HttpGetAttribute;
using HttpPostAttribute = Microsoft.AspNetCore.Mvc.HttpPostAttribute;

namespace _03_CiteDemoPresentationLayer.Controllers
{
    public class AttributeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<AttributeGetDTO> attributes = new List<AttributeGetDTO>();

            using (var client = new HttpClient())
            {
                // Define the base client
                client.BaseAddress = new Uri("https://localhost:7252/");
                client.DefaultRequestHeaders.Clear();

                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                //HTTP GET
                HttpResponseMessage result = await client.GetAsync("api/attribute");

                if (!result.IsSuccessStatusCode)
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
                else
                {
                    var attributesJSON = await result.Content.ReadAsStringAsync();

                    attributes = JsonConvert.DeserializeObject<List<AttributeGetDTO>>(attributesJSON);
                }

            }

            return View(new IndexViewModel()
            {
                Attributes = attributes
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
                HttpResponseMessage result = await client.DeleteAsync("api/attribute/" + id);

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
                var body = new AttributePostDTO()
                {
                    Name = model.Name,
                    Value = model.Value
                };

                //HTTP Post
                HttpResponseMessage result = await client.PostAsJsonAsync("api/attribute", body);

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
                var body = new AttributePutDTO()
                {
                    Id = model.Id,
                    Name = model.Name,
                    Value = model.Value
                };

                //HTTP Put
                HttpResponseMessage result = await client.PutAsJsonAsync("api/attribute", body);

                if (!result.IsSuccessStatusCode)
                {
                    var response = result.Content.ReadAsStringAsync().Result;
                    ModelState.AddModelError(string.Empty, response);
                    return View("Update", model);
                }

                return RedirectToAction("Index");
            }
        }

    }
}
