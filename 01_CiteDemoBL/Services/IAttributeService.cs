using CiteDemoBL.Models;
using CiteDemoBL.Static;

namespace CiteDemoBL.Services
{
    public interface IAttributeService
    {
        Response<CAttribute> CreateAttribute(CAttribute attribute);

        Response<CAttribute> ReadAttribute(Guid id);

        Response<CAttribute> UpdateAttribute(CAttribute attribute);

        Response<bool> DeleteAttribute(Guid id);

    }
}
