using AutoMapper;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Exceptions;
using Inventory.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.App.Commands
{
    public class RemoveItemCommandHandler : IRequestHandler<RemoveItemCommand, ItemDto>
    {
        private readonly IEFRepository<Item> _repository;
        private readonly ILogger<CreateItemCommandHandler> _logger;
        private readonly IMapper _mapper;

        public RemoveItemCommandHandler(
            IEFRepository<Item> repository,
            ILogger<CreateItemCommandHandler> logger,
            IMapper mapper
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ItemDto> Handle(RemoveItemCommand command, CancellationToken cancellationToken)
        {
            bool result;

            ItemDto? itemDto = null;

             _logger.LogInformation($"----- Soft Removing Item {command.Item.ItemId}");

            try
            {
                command.Item.RemoveThisItem();

                var itemSoftRemoved = await _repository.UpdateAsync(command.Item, false);

                result = await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                if (result)
                {
                    itemDto = _mapper.Map<ItemDto>(itemSoftRemoved);
                }

                return itemDto;
            }
            catch (Exception)
            {
                throw new ItemDomainException($"Exception removing Item with name: {command.Item.ItemId}");
            }
        }
    }
}
