using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Inventory.App;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.App.Queries;

namespace Inventory.Web.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemQueries _itemsQueries;
        private readonly IItemsService _itemsService;
        private readonly IItemTypesService _itemTypesService;
        private readonly IMapper _mapper;

        public ItemsController(
            IItemsService itemsService,
            IItemQueries itemsQueries,
            IItemTypesService itemTypesService,
            IMapper mapper)
        {
            _itemsQueries = itemsQueries ?? throw new ArgumentNullException(nameof(itemsQueries));
            _itemsService = itemsService ?? throw new ArgumentNullException(nameof(itemsService));
            _itemTypesService = itemTypesService ?? throw new ArgumentNullException(nameof(itemTypesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<List<ItemDtoWeb>> GetAllItems()
        {
            var entities = (await _itemsQueries.GetAllDtoAsync()).ToList();

            var dtoW = _mapper.Map<List<ItemDtoWeb>>(entities);

            return dtoW;
        }

        [HttpGet]
        public async Task<List<ItemTypeDto>> GetAllItemTypes()
        {
            var entities = (await _itemTypesService.GetAllAsync()).ToList();

            return entities;
        }

        [HttpGet]
        public async Task<ItemDto> GetById(int itemId)
        {
            var entity = await _itemsQueries.GetAsync(itemId);

            if (entity == null)
            {
                throw new Exception($"Entity with Id = {itemId} do not exists");
            }

            return entity;
        }

        [HttpPost]
        public async Task<ItemDto> AddUpdateItem([FromForm] ItemUpdateRequest value)
        {
            ItemDto itemDtoResult = new ItemDto();

            var entityType = await _itemTypesService.GetAsync(value.ItemTypeId);

            if (entityType == null)
            {
                var error = $"ItemType with Id = {value.ItemTypeId} do not exists";

                return itemDtoResult;
            }

            if (value.ItemId > 0)
            {
                itemDtoResult = await UpdateItem(value);

                return itemDtoResult;
            }

            itemDtoResult = await InsertItem(value);

            return itemDtoResult;
        }

        [HttpDelete]
        public async Task<int> Delete(int itemId)
        {
            var result = 0;
            string message = "Deleted";

            var entityExists = await _itemsService.GetAsync(itemId);

            if (entityExists == null)
            {
                result = -1;
                message = $"Entity with id {itemId} not found.";
            }

            await _itemsService.RemoveAsync(itemId);

            return result;
        }

        private async Task<ItemDto> InsertItem(ItemUpdateRequest value)
        {
            var entity = _mapper.Map<Item>(value);

            var entityCreated = await _itemsService.InsertEntityAsync(entity);

            var dto = _mapper.Map<ItemDto>(entityCreated);

            return dto;
        }
        
        private async Task<ItemDto> UpdateItem(ItemUpdateRequest value)
        {
            var entity = _mapper.Map<Item>(value);

            var entityUpdated = await _itemsService.UpdateAsync(entity);

            var itemDtoResult = _mapper.Map<ItemDto>(entityUpdated);

            return itemDtoResult;
        }
    }
}
