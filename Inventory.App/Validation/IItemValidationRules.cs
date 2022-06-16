using FluentValidation.Results;
using Inventory.App.Commands;
using Inventory.Domain.Entities;

namespace Inventory.App.Validation
{
    public interface IItemValidationRules
    {
        ValidationResult Validate(CreateItemCommand model);
        /// <summary>
        /// Validates the asynchronous.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="cancellation">The cancellation.</param>
        /// <returns></returns>
        Task<ValidationResult> ValidateAsync(CreateItemCommand instance, CancellationToken cancellation);
    }
}
