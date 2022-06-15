using AutoMapper;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using System.Linq.Expressions;

namespace Inventory.App
{
    public class ItemTypesService : IItemTypesService
    {
        private readonly IEFRepository<ItemType> _repository;
        private readonly IMapper _mapper;

        public ItemTypesService(IEFRepository<ItemType> repository,
            IMapper mapper)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<IEnumerable<ItemTypeDto>> GetAllAsync()
        {
            var itemTypes = await _repository.GetAllAsync();

            var dtos = _mapper.Map<IEnumerable<ItemTypeDto>>(itemTypes);

            return dtos;
        }

        public async Task<ItemTypeDto> GetAsync(int id)
        {
            var item = await _repository.FindAsync(id);

            var dto = _mapper.Map<ItemTypeDto>(item);

            return dto;
        }

        public async Task<ItemType> AddEntityAsync(ItemType entity)
        {
            await _repository.InsertAsync(entity);

            _repository.SaveChanges();

            return entity;
        }

        public async Task<ItemType> GetByFilterAsync(Expression<Func<ItemType, bool>> filter)
        {
            var entityRead = await _repository.GetByAsync(filter);

            return entityRead;
        }

        public async Task<ItemType> UpdateAsync(ItemType entity)
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
