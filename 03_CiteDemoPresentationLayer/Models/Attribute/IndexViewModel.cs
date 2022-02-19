using CiteDemoApi.DTO.CAttributeDTOS;

namespace _03_CiteDemoPresentationLayer.Models.Attribute
{
    public class IndexViewModel
    {
        public List<AttributeGetDTO> Attributes { get; set; }

        public AttributePostDTO AttributePost { get; set; } 

        public string AttributeDeleteId { get; set; }   
    }
}
