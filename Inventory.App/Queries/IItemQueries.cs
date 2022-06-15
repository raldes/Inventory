using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using System.Linq.Expressions;

namespace Inventory.App.Queries;

public interface IItemQueries
{
    Task<IEnumerable<ItemDto>> GetAllDtoAsync();

    Task<IEnumerable<Item>> GetAllAsync(
        Expression<Func<Item, bool>> filter,
        Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = null,
        string includeProperties = "");

    Task<IEnumerable<ItemDto>> GetAllDtoAsync(
        Expression<Func<Item, bool>> filter,
        Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = null,
        string includeProperties = "");

    Task<ItemDto> GetAsync(int id);
 
}
