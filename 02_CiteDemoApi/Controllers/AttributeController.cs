using CiteDemoApi.DTO.CAttributeDTOS;
using CiteDemoBL.Models;
using CiteDemoBL.Services;
using CiteDemoBL.Static;
using Microsoft.AspNetCore.Mvc;

namespace CiteDemoApi.Controllers
{
    [Route("Attribute")]
    [ApiController]
    public class AttributeController : Controller
    {
        private IAttributeService _AttributeService;

        public AttributeController(IAttributeService AttributeService)
        {
            _AttributeService = AttributeService;
        }

        [HttpGet, Route("{id}")]
        public ActionResult<AttributeGetDTO> GetAttribute([FromRoute] Guid id)
        {
            Response<CAttribute> response = _AttributeService.ReadAttribute(id);

            if (response.StatusCode != ErrorCodes.Success) return NotFound("No Attribute found with this id");

            return Ok(new AttributeGetDTO(response.Data));
        }

        [HttpGet]
        public ActionResult<ICollection<AttributeGetDTO>> GetAttribute()
        {
            Response<ICollection<CAttribute>> response = _AttributeService.ReadAttribute();

            if (response.StatusCode != ErrorCodes.Success) return NotFound("No Attribute found with this id");

            var result = new List<AttributeGetDTO>();

            foreach (var Attribute in response.Data)
            {
                result.Add(new AttributeGetDTO(Attribute));
            }

            return Ok(result);
        }

        [HttpPost]
        public ActionResult<AttributeGetDTO> PostAttribute([FromBody] AttributePostDTO Attribute)
        {

            Response<CAttribute> response = _AttributeService.CreateAttribute(Attribute.ToCAttribute());

            if (response.StatusCode != ErrorCodes.Success) return BadRequest("Could not add Attribute");

            return Ok(new AttributeGetDTO(response.Data));

        }

        [HttpPut]
        public ActionResult<AttributeGetDTO> PutAttribute([FromBody] AttributePutDTO Attribute)
        {

            Response<CAttribute> response = _AttributeService.UpdateAttribute(Attribute.ToCAttribute());

            if (response.StatusCode != ErrorCodes.Success) return BadRequest(response.Description);

            return Ok(new AttributeGetDTO(response.Data));

        }

        [HttpDelete, Route("{id}")]
        public ActionResult<bool> DeleteAttribute([FromRoute] Guid id)
        {
            Response<bool> response = _AttributeService.DeleteAttribute(id);

            if (response.StatusCode != ErrorCodes.Success) return NotFound("No Attribute found with this id");

            return Ok(response.Data);
        }
    }
}
