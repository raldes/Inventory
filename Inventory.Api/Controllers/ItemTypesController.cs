using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Inventory.Api.Controllers.ViewModel;
using Inventory.App;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using System.Linq.Expressions;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemTypesController : ControllerBase
    {
        private readonly IItemTypesService _itemTypesService; 
        public readonly IMapper _mapper;


        public ItemTypesController(
            IItemTypesService itemTypesService,
            IMapper mapper)
        {
            _itemTypesService = itemTypesService ?? throw new ArgumentNullException(nameof(itemTypesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ItemTypeDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<ItemTypeDto>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var entities = await _itemTypesService.GetAllAsync();

            var totalItems = entities.Count();

            var itemsOnPage = entities
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToList();

            return new PaginatedItemsViewModel<ItemTypeDto>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ItemTypeDto>> GetById(int id)
        {
            var entity = await _itemTypesService.GetAsync(id);

            if (entity == null)
            {
                var error = $"Entity with Id = {id} do not exists";

                return NotFound(error);
            }

            return entity;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateAsync([FromBody] ItemTypeCreateRequest value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<ItemType>(value);

            var entityCreated = await _itemTypesService.AddEntityAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id = entityCreated.ItemTypeId }, entityCreated);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> Put(int id, [FromBody] ItemTypeUpdateRequest value)
        {
            if (id != value.ItemTypeId)
            {
                var error = $"Update query id = {id} is different to requested object Id = {value.ItemTypeId}";

                return BadRequest(error);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entity = _mapper.Map<ItemType>(value);

            var entityUpdated = await _itemTypesService.UpdateAsync(entity);

            return CreatedAtAction(nameof(GetById), new { id = entityUpdated.ItemTypeId }, entityUpdated);

        }

        [HttpDelete("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> Delete(int id)
        {
            var entityExists = await _itemTypesService.GetAsync(id);

            if (entityExists == null)
            {
                return NotFound(new { Message = $"Entity with id {id} not found." });
            }

            await _itemTypesService.RemoveAsync(id);

            return NoContent();
        }
    }
}
