using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using System.Linq.Expressions;

namespace Inventory.App
{
    public interface IItemTypesService
    {
        Task<IEnumerable<ItemTypeDto>> GetAllAsync();

        Task<ItemTypeDto> GetAsync(int id);

        Task<ItemType> AddEntityAsync(ItemType entity);

        Task<ItemType> GetByFilterAsync(Expression<Func<ItemType, bool>> filter);

        Task<ItemType> UpdateAsync(ItemType entity);

        Task<int> RemoveAsync(int id);
    }
}
