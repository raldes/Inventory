using AutoMapper;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Exceptions;
using Inventory.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.App.Commands
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemDto>
    {

        private readonly IEFRepository<Item> _repository;
        private readonly ILogger<CreateItemCommandHandler> _logger;
        private readonly IMapper _mapper;

        public CreateItemCommandHandler(
            IEFRepository<Item> repository,
            ILogger<CreateItemCommandHandler> logger,
            IMapper mapper
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ItemDto> Handle(CreateItemCommand command, CancellationToken cancellationToken)
        {
            ItemDto? itemDto = null;

            _logger.LogInformation($"... Creating item with name: { command.Name}");

            try
            {
                var item =  Item.NewItemCreated(command.Name, command.Description, command.ExpirationDate, command.ItemTypeId);

                //item domain event for this case:
                var itemAdded = _repository.Add(item);

                var result = await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                if (result)
                {
                    itemDto = _mapper.Map<ItemDto>(itemAdded);
                }

                return itemDto;
            }
            catch (Exception)
            {
                throw new ItemDomainException($"Exception Creating Item with name: {command.Name}");
            }
        }
    }
}
