using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Inventory.Api.Controllers.ViewModel;
using Inventory.App;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using System.Net;
using Inventory.App.Commands;
using MediatR;
using Inventory.App.Queries;
using System.Linq.Expressions;
using Inventory.App.Validation;

namespace Inventory.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemsController : ControllerBase
    {
        private readonly IItemQueries _itemsQueries;
        private readonly IItemTypesService _itemTypesService;
        private readonly IMapper _mapper;
        private readonly ILogger<ItemsController> _logger;
        private readonly IMediator _mediator;
        private readonly IItemValidationService _itemValidationService;

        public ItemsController(
            IMediator mediator,
            IItemQueries itemsQueries,
            IItemTypesService itemTypesService,
            IMapper mapper,
            ILogger<ItemsController> logger,
            IItemValidationService itemValidationService
            )
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _itemsQueries = itemsQueries ?? throw new ArgumentNullException(nameof(itemsQueries));
            _itemTypesService = itemTypesService ?? throw new ArgumentNullException(nameof(itemTypesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _itemValidationService = itemValidationService ?? throw new ArgumentNullException(nameof(itemValidationService));
        }


        #region Queries (using IItemQueries service)
        [HttpGet]
        [ProducesResponseType(typeof(PaginatedItemsViewModel<ItemDto>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginatedItemsViewModel<ItemDto>>> Get([FromQuery] int pageSize = 10, [FromQuery] int pageIndex = 0)
        {
            var entities = await _itemsQueries.GetAllDtoAsync();

            var totalItems = entities.Count();

            var itemsOnPage = entities
                .Skip(pageSize * pageIndex)
                .Take(pageSize)
                .ToList();

            return new PaginatedItemsViewModel<ItemDto>(pageIndex, pageSize, totalItems, itemsOnPage);
        }

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<ItemDto>> GetById(int id)
        {
            var entity = await _itemsQueries.GetAsync(id);

            if (entity == null)
            {
                var error = $"Entity with Id = {id} do not exists";

                return NotFound(error);
            }

            return entity;
        }

        #endregion Queries

        #region Commands (using Command - Command handler)

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult> CreateAsync([FromBody] CreateItemCommand createItemCommand)
        {
            var validationResult = _itemValidationService.Validate(createItemCommand);

            if(!string.IsNullOrEmpty(validationResult))
            {
                return BadRequest(validationResult);
            }
 
             _logger.LogInformation($"Creating item. Name: {createItemCommand.Name}, TypeId: {createItemCommand.ItemTypeId}");

            var dto =  await _mediator.Send(createItemCommand);

            return CreatedAtAction(nameof(GetById), new { id = dto.ItemId }, dto);
        }

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateItemCommand value)
        {
            if (id != value.ItemId)
            {
                var error = $"Update query id = {id} is different to requested object Id = {value.ItemId}";

                return BadRequest(error);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var entityType = await _itemTypesService.GetAsync(value.ItemTypeId);

            if (entityType == null)
            {
                var error = $"ItemType with Id = {value.ItemTypeId} do not exists";

                return BadRequest(error);
            }

            _logger.LogInformation($"Updating item. Id: {id}");

            var entityUpdated = await _mediator.Send(value);

            return CreatedAtAction(nameof(GetById), new { id = entityUpdated.ItemId }, entityUpdated);
        }


        [HttpDelete("{name}")]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> SoftRemove(string name)
        {
            Expression<Func<Item, bool>> filter = i => i.Name == name;

            var items = await _itemsQueries.GetAllAsync(filter);

            if (items == null || !items.Any())
            {
                var message = $"Item with Name {name} do not exists";

                _logger.LogInformation(message);

                return NotFound(new { Message = message });
            }

            var item = items.First();

            _logger.LogInformation($"Removing item. Name: {name}, Id: {item.ItemId}");

            var dto = await _mediator.Send(new RemoveItemCommand(item));

            return NoContent();
        }

        #endregion Commands (using Command - Command handler)

    }
}
