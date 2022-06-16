using FluentValidation.Results;
using Inventory.App.Commands;

namespace Inventory.App.Validation
{
    public interface IItemValidationService
    {
        string Validate(CreateItemCommand model);
    }
}
