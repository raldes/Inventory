using AutoMapper;
using Inventory.App.Commands;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;

namespace Inventory.App.Mapper
{
    public class ItemProfile : Profile
    {
        public ItemProfile()
        {
            CreateMap<Item, ItemDto>()
                     .ForMember(m => m.ItemStatus, opt => opt.MapFrom(s => ItemStatus.From(s.ItemStatusId)));

            CreateMap<ItemDto, Item>();

            CreateMap<ItemDto, ItemDtoWeb>()
                .ForMember(m => m.ItemTypeId, opt => opt.MapFrom(s => s.ItemType.ItemTypeId))
                .ForMember(m => m.Code, opt => opt.MapFrom(s => s.ItemType.Code));

            CreateMap<ItemCreateRequest, Item>();

            CreateMap<CreateItemCommand, Item>();

            CreateMap<ItemUpdateRequest, Item>();
        }
    }
}
