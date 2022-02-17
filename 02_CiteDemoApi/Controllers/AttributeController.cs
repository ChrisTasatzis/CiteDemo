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
        private readonly IAttributeService _AttributeService;

        public AttributeController(IAttributeService AttributeService)
        {
            _AttributeService = AttributeService;
        }

        [HttpGet, Route("{id}")]
        public async Task<ActionResult<AttributeGetDTO>> GetAttribute([FromRoute] Guid id)
        {
            Response<CAttribute> response = await _AttributeService.ReadAttribute(id);

            if (response.StatusCode != ErrorCodes.Success) return NotFound(response.Description);

            return Ok(new AttributeGetDTO(response.Data));
        }

        [HttpGet]
        public async Task<ActionResult<ICollection<AttributeGetDTO>>> GetAttribute()
        {
            Response<ICollection<CAttribute>> response = await _AttributeService.ReadAttribute();

            if (response.StatusCode != ErrorCodes.Success) return NotFound(response.Description);

            var result = new List<AttributeGetDTO>();

            foreach (var Attribute in response.Data)
            {
                result.Add(new AttributeGetDTO(Attribute));
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<AttributeGetDTO>> PostAttribute([FromBody] AttributePostDTO Attribute)
        {

            Response<CAttribute> response = await _AttributeService.CreateAttribute(Attribute.ToCAttribute());

            if (response.StatusCode != ErrorCodes.Success) return BadRequest(response.Description);

            return Ok(new AttributeGetDTO(response.Data));

        }

        [HttpPut]
        public async Task< ActionResult<AttributeGetDTO>> PutAttribute([FromBody] AttributePutDTO Attribute)
        {

            Response<CAttribute> response = await _AttributeService.UpdateAttribute(Attribute.ToCAttribute());

            if (response.StatusCode != ErrorCodes.Success) return BadRequest(response.Description);

            return Ok(new AttributeGetDTO(response.Data));

        }

        [HttpDelete, Route("{id}")]
        public async Task<ActionResult<bool>> DeleteAttribute([FromRoute] Guid id)
        {
            Response<bool> response = await _AttributeService.DeleteAttribute(id);

            if (response.StatusCode != ErrorCodes.Success) return NotFound(response.Description);

            return Ok(response.Description);
        }
    }
}
