using Inventory.Domain.Entities;
using MediatR;

namespace Inventory.Infra.Database.Extensions;

static class MediatorExtension
{
    public static async Task DispatchDomainEventsAsync(this IMediator mediator, ItemsDbContext ctx)
    {
        var domainEntities = ctx.ChangeTracker
            .Entries<EFEntity>()
            .Where(x => x.Entity.DomainEvents != null && x.Entity.DomainEvents.Any());

        var domainEvents = domainEntities
            .SelectMany(x => x.Entity.DomainEvents)
            .ToList();

        domainEntities.ToList()
            .ForEach(entity => entity.Entity.ClearDomainEvents());

        foreach (var domainEvent in domainEvents)
            await mediator.Publish(domainEvent);
    }
}
