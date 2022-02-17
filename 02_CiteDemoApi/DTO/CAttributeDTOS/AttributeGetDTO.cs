using CiteDemoBL.Models;
using System.Text.Json.Serialization;

namespace CiteDemoApi.DTO.CAttributeDTOS
{
    public class AttributeGetDTO
    {
        public Guid? Id { get; set; }

        public string? Name { get; set; }

        public string? Value { get; set; }

        public AttributeGetDTO(CAttribute attribute)
        {
            Id = attribute.Id;

            Name = attribute.Name;

            Value = attribute.Value;
        }

    }
}
