using AutoMapper;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using System.Linq.Expressions;

namespace Inventory.App.Queries;
    
public class ItemQueries : IItemQueries
{
    private readonly IEFRepository<Item> _repository;
    private readonly IMapper _mapper;

    public ItemQueries(
        IEFRepository<Item> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));

        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<IEnumerable<ItemDto>> GetAllDtoAsync()
    {
        var items = await GetAllDtoAsync(null, null, "ItemType");

        var dtos = _mapper.Map<IEnumerable<ItemDto>>(items);

        return dtos;
    }

    public async Task<IEnumerable<ItemDto>> GetAllDtoAsync(Expression<Func<Item, bool>> filter,
        Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = null,
        string includeProperties = "")
    {
        var entities = await GetAllAsync(filter, orderBy, includeProperties);

        var dtos = _mapper.Map<IEnumerable<ItemDto>>(entities);

        return dtos;
    }
    
    public async Task<IEnumerable<Item>> GetAllAsync(Expression<Func<Item, bool>> filter,
        Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = null,
        string includeProperties = "")
    {
        var entities = await _repository.GetAsync(filter, orderBy, includeProperties);

        return entities;
    }

    public async Task<ItemDto> GetAsync(int id)
    {
        var item = await _repository.FindAsync(id);

        var dto = _mapper.Map<ItemDto>(item);

        return dto;
    }

    //This query (as example) uses SQL connection and query to dB: 

    //public async Task<Item> GetAsyncSQL(int id)
    //{
    //    using (var connection = new SqlConnection(_connectionString))
    //    {
    //        connection.Open();

    //        var result = await connection.QueryAsync<dynamic>(
    //            @"select .... from... where ..."
    //                , new { id }
    //            );

    //        if (result.AsList().Count == 0)
    //            throw new KeyNotFoundException();

    //        return result;
    //    }
    //}

 
}
