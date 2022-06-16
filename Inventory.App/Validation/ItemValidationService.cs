using FluentValidation;
using FluentValidation.Results;
using Inventory.App.Commands;
using Inventory.Domain.Entities;

namespace Inventory.App.Validation
{
    public class ItemValidationService : IItemValidationService
    {
        private readonly IItemValidationRules _itemValidationRules;

        public ItemValidationService(
            IItemValidationRules itemValidationRules)
        {
            _itemValidationRules = itemValidationRules;
        }

        /// <summary>
        /// Validates the specified device view model.
        /// </summary>
        /// <param name="model">The device view model.</param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public string Validate(CreateItemCommand model)
        {
            var validationResult = _itemValidationRules.Validate(model);

            if (validationResult == null)
            {
                throw new ValidationException("Validation errors", validationResult.Errors);
            }

            if(!validationResult.IsValid)
            {
                return validationResult.ToString();
            }

            return String.Empty;
        }
    }
}
