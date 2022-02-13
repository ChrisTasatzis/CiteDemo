using CiteDemoBL.Models;
using CiteDemoBL.Static;
using Microsoft.EntityFrameworkCore;

namespace CiteDemoBL.Services
{
    public class AttributeService : IAttributeService
    {
        private CiteDemoDbContext _dbContext;

        public AttributeService(CiteDemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Response<CAttribute> CreateAttribute(CAttribute attribute)
        {
            _dbContext.CAttributes.Add(attribute);

            if (_dbContext.SaveChanges() != 1)
                return new Response<CAttribute>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "Could not save changes."
                };

            return new Response<CAttribute>
            {
                Data = attribute,
                StatusCode = ErrorCodes.Success,
                Description = "CAttribute added successfully."
            };
        }

        public Response<CAttribute> ReadAttribute(Guid id)
        {
            var attribute = _dbContext.CAttributes
                .AsNoTracking()
                .FirstOrDefault(u => u.Id == id);


            if (attribute == null)
                return new Response<CAttribute>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "No attribute with this id exists."
                };

            return new Response<CAttribute>
            {
                Data = attribute,
                StatusCode = ErrorCodes.Success,
                Description = "CAttribute Found."
            };
        }

        public Response<ICollection<CAttribute>> ReadAttribute()
        {
            var attributes = _dbContext.CAttributes
                .AsNoTracking()
                .ToList();

            return new Response<ICollection<CAttribute>>
            {
                Data = attributes,
                StatusCode = ErrorCodes.Success,
                Description = "CAttribute Found."
            };
        }

        public Response<CAttribute> UpdateAttribute(CAttribute attribute)
        {
            var attributeDB = _dbContext.CAttributes
                .FirstOrDefault(u => u.Id == attribute.Id);

            attributeDB.Copy(attribute);

            if (_dbContext.SaveChanges() != 1)
                return new Response<CAttribute>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "Could not save changes."
                };

            return new Response<CAttribute>
            {
                Data = attribute,
                StatusCode = ErrorCodes.Success,
                Description = "CAttribute updated successfully."
            };
        }

        public Response<bool> DeleteAttribute(Guid id)
        {
            var attribute = _dbContext.CAttributes
                .FirstOrDefault(u => u.Id == id);

            _dbContext.CAttributes.Remove(attribute);

            if (_dbContext.SaveChanges() != 1)
                return new Response<bool>
                {
                    Data = false,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "Could not save changes."
                };

            return new Response<bool>
            {
                Data = true,
                StatusCode = ErrorCodes.Success,
                Description = "CAttribute deleted successfully."
            };
        }
    }
}
