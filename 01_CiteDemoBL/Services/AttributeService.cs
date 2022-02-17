using CiteDemoBL.Models;
using CiteDemoBL.Static;
using Microsoft.EntityFrameworkCore;

namespace CiteDemoBL.Services
{
    public class AttributeService : IAttributeService
    {
        private readonly CiteDemoDbContext _dbContext;

        public AttributeService(CiteDemoDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Response<CAttribute>> CreateAttribute(CAttribute attribute)
        {
            var attributeDB = await _dbContext.CAttributes
               .FirstOrDefaultAsync(u => u.Name == attribute.Name && u.Value == attribute.Value);

            if (attributeDB != null)
                return new Response<CAttribute>
                {
                    Data = null,
                    StatusCode = ErrorCodes.AttributeAlreadyExists,
                    Description = "Attribute already exists."
                };

            _dbContext.CAttributes.Add(attribute);

            if (await _dbContext.SaveChangesAsync() != 1)
                return new Response<CAttribute>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "No changes saved."
                };

            return new Response<CAttribute>
            {
                Data = attribute,
                StatusCode = ErrorCodes.Success,
                Description = "CAttribute added successfully."
            };
        }

        public async Task<Response<CAttribute>> ReadAttribute(Guid? id)
        {
            var attribute = await _dbContext.CAttributes
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == id);


            if (attribute == null)
                return new Response<CAttribute>
                {
                    Data = null,
                    StatusCode = ErrorCodes.AttributeNotFound,
                    Description = "No attribute with this id exists."
                };

            return new Response<CAttribute>
            {
                Data = attribute,
                StatusCode = ErrorCodes.Success,
                Description = "CAttribute Found."
            };
        }

        public async Task<Response<ICollection<CAttribute>>> ReadAttribute()
        {
            var attributes = await _dbContext.CAttributes
                .AsNoTracking()
                .ToListAsync();

            return new Response<ICollection<CAttribute>>
            {
                Data = attributes,
                StatusCode = ErrorCodes.Success,
                Description = "CAttributes Found."
            };
        }

        public async Task<Response<CAttribute>> UpdateAttribute(CAttribute attribute)
        {
            var attributeDB = await _dbContext.CAttributes
                .FirstOrDefaultAsync(u => u.Id == attribute.Id);

            if(attributeDB == null)
                return new Response<CAttribute>
                {
                    Data = null,
                    StatusCode = ErrorCodes.AttributeNotFound,
                    Description = "No attribute with this id exists."
                };

            attributeDB.Copy(attribute);

            if (await _dbContext.SaveChangesAsync() != 1)
                return new Response<CAttribute>
                {
                    Data = null,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "No changes saved."
                };

            return new Response<CAttribute>
            {
                Data = attribute,
                StatusCode = ErrorCodes.Success,
                Description = "CAttribute updated successfully."
            };
        }

        public async  Task<Response<bool>> DeleteAttribute(Guid? id)
        {
            var attribute = await _dbContext.CAttributes
                .FirstOrDefaultAsync(u => u.Id == id);

            if (attribute == null)
                return new Response<bool>
                {
                    Data = false,
                    StatusCode = ErrorCodes.AttributeNotFound,
                    Description = "No attribute with this id exists."
                };

            _dbContext.CAttributes.Remove(attribute);

            if (await _dbContext.SaveChangesAsync() != 1)
                return new Response<bool>
                {
                    Data = false,
                    StatusCode = ErrorCodes.InternalError,
                    Description = "No changes saved."
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
