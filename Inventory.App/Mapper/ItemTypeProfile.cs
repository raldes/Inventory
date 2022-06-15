using AutoMapper;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;

namespace Inventory.App.Mapper
{
    public class ItemTypeProfile : Profile
    {
        public ItemTypeProfile()
        {
            CreateMap<ItemType, ItemTypeDto>();

            CreateMap<ItemTypeCreateRequest, ItemType>();

            CreateMap<ItemTypeUpdateRequest, ItemType>();
        }
    }
}
