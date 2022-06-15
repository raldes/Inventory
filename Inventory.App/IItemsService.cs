using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using System.Linq.Expressions;

namespace Inventory.App
{
    public interface IItemsService
    {
        Task<IEnumerable<ItemDto>> GetAllAsync();

        Task<IEnumerable<ItemDto>> GetAllAsync(Expression<Func<Item, bool>> filter,
            Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = null,
            string includeProperties = "");

        Task<ItemDto> GetAsync(int id);

        Task<Item> InsertEntityAsync(Item entity);

        Task<Item> GetByFilterAsync(Expression<Func<Item, bool>> filter);

        Task<Item> UpdateAsync(Item entity);

        Task<int> RemoveAsync(int id);
    }
}
