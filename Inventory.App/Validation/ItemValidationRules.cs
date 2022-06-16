

using FluentValidation;
using Inventory.App.Commands;
using Inventory.App.Queries;
using Inventory.Domain.Entities;
using System.Linq.Expressions;

namespace Inventory.App.Validation
{
    public class ItemValidationRules : AbstractValidator<CreateItemCommand>, IItemValidationRules
    {

        public static string EmptyNameValidationError = "Item name must be not empty";
        public static string UniqueNameValidationError = "Item name must be unique";
        public static string ItemTypeValidationError = "Item type must exists";

        private readonly IItemQueries _itemQueries;
        private readonly IItemTypesService _itemTypesService;

        public ItemValidationRules(
            IItemQueries itemQueries,
            IItemTypesService itemTypesService)
        {
            _itemQueries = itemQueries;
            _itemTypesService = itemTypesService;

            RuleFor(Item => Item.Name).NotEmpty().WithMessage(EmptyNameValidationError);

            RuleFor(Item => Item.Name).Must(ItemNameDoNotExists).WithMessage(UniqueNameValidationError);

            RuleFor(Item => Item.ItemTypeId).Must(ItemTypeMustExist).WithMessage(ItemTypeValidationError);
        }
        
        private bool ItemNameDoNotExists(string name)
        {
            Expression<Func<Item, bool>> filter = i => i.Name == name;

            var existsItem = _itemQueries.GetAllAsync(filter).Result;
            
            return existsItem == null || !existsItem.Any();
        }

        private bool ItemTypeMustExist(int itemTypeId)
        {
            var entityType = _itemTypesService.GetAsync(itemTypeId).Result;

            return entityType != null;
        }
    }
}
