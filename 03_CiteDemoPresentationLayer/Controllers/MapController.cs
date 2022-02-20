using _03_CiteDemoPresentationLayer.Models.Map;
using CiteDemoApi.DTO.CAttributeDTOS;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace _03_CiteDemoPresentationLayer.Controllers
{
    public class MapController : Controller
    {
        public async Task<IActionResult> IndexAsync()
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
    }
}
