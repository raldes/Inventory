using AutoMapper;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using System.Linq.Expressions;

namespace Inventory.App
{
    public class ItemsService : IItemsService
    {
        private readonly IEFRepository<Item> _repository;
        private readonly IMapper _mapper;

        public ItemsService(IEFRepository<Item> repository,
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync()
        {
            var items = await GetAllAsync(null, null, "ItemType");

            var dtos = _mapper.Map<IEnumerable<ItemDto>>(items);

            return dtos;
        }

        public async Task<IEnumerable<ItemDto>> GetAllAsync(Expression<Func<Item, bool>> filter,
            Func<IQueryable<Item>, IOrderedQueryable<Item>> orderBy = null,
            string includeProperties = "")
        {
            var entities = await _repository.GetAsync(filter, orderBy, includeProperties);

            var dtos = _mapper.Map<IEnumerable<ItemDto>>(entities);

            return dtos;
        }

        public async Task<ItemDto> GetAsync(int id)
        {
            var item = await _repository.FindAsync(id);

            var dto = _mapper.Map<ItemDto>(item);

            return dto;
        }

        public async Task<Item> InsertEntityAsync(Item entity)
        {
            _repository.Insert(entity);

            _repository.SaveChanges();

            return entity;
        }

        public async Task<Item> AddEntityAsync(Item entity)
        {
            _repository.Add(entity);



            _repository.SaveChanges();

            return entity;
        }

        public async Task<Item> GetByFilterAsync(Expression<Func<Item, bool>> filter)
        {
            var entityRead = await _repository.GetByAsync(filter);

            return entityRead;
        }

        public async Task<Item> UpdateAsync(Item entity)
        {
            await _repository.UpdateAsync(entity);

            return entity;
        }

        public async Task<int> RemoveAsync(int id)
        {

            var item = await _repository.FindAsync(id);
            if (item == null)
            {
                throw new Exception($"The entity with Id {id} do not exists");
            }

            return await _repository.RemoveAsync(item);

        }
    }
}
