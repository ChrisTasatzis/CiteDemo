using CiteDemoBL.Models;
using CiteDemoBL.Static;

namespace CiteDemoBL.Services
{
    public interface IAttributeService
    {
        Task<Response<CAttribute>> CreateAttribute(CAttribute attribute);

        Task<Response<CAttribute>> ReadAttribute(Guid? id);

        Task<Response<ICollection<CAttribute>>> ReadAttribute();

        Task<Response<CAttribute>> UpdateAttribute(CAttribute attribute);

        Task<Response<bool>> DeleteAttribute(Guid? id);

    }
}
