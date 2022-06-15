using AutoMapper;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.Exceptions;
using Inventory.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Inventory.App.Commands
{
    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand, ItemDto>
    {
        private readonly IEFRepository<Item> _repository;
        private readonly ILogger<UpdateItemCommandHandler> _logger;
        private readonly IMapper _mapper;

        public UpdateItemCommandHandler(
            IEFRepository<Item> repository,
            ILogger<UpdateItemCommandHandler> logger,
            IMapper mapper
            )
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<ItemDto> Handle(UpdateItemCommand command, CancellationToken cancellationToken)
        {
            bool result;

            ItemDto? itemDto = null;

            _logger.LogInformation($"----- Updating Item {command.ItemId}");

            try
            {
                var item = await _repository.FindAsync(command.ItemId);

                if(item == null)
                {
                    return itemDto;
                }

                item.UpdateThisItem(command.Name, command.Description, command.ExpirationDate, command.ItemTypeId);

                var itemUpdated = await _repository.UpdateAsync(item, false);

                result = await _repository.UnitOfWork.SaveEntitiesAsync(cancellationToken);

                if (result)
                {
                    itemDto = _mapper.Map<ItemDto>(itemUpdated);
                }

                return itemDto;
            }
            catch (Exception ex)
            {
                throw new ItemDomainException($"Exception updating Item with name: {command.ItemId}");
            }
        }
    }
}
