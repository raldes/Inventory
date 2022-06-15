using Autofac;
using Inventory.App.Commands;
using Inventory.App.DomainEventHandlers;
using MediatR;
using System.Reflection;

namespace Inventory.Api.AutofacModules;

public class MediatorModule : Autofac.Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
            .AsImplementedInterfaces();

        // Register all the Command classes (they implement IRequestHandler) in assembly holding the Commands
        builder.RegisterAssemblyTypes(typeof(CreateItemCommand).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));

        builder.RegisterAssemblyTypes(typeof(UpdateItemCommand).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));

        builder.RegisterAssemblyTypes(typeof(RemoveItemCommand).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(IRequestHandler<,>));

        // Register the DomainEventHandler classes (they implement INotificationHandler<>) in assembly holding the Domain Events
        builder.RegisterAssemblyTypes(typeof(ItemCreatedDomainEventHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(INotificationHandler<>));

        builder.RegisterAssemblyTypes(typeof(ItemUpdatedDomainEventHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(INotificationHandler<>));

        builder.RegisterAssemblyTypes(typeof(ItemRemovedDomainEventHandler).GetTypeInfo().Assembly)
            .AsClosedTypesOf(typeof(INotificationHandler<>));


        builder.Register<ServiceFactory>(context =>
        {
            var componentContext = context.Resolve<IComponentContext>();
            return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
        });

        //builder.RegisterGeneric(typeof(LoggingBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(ValidatorBehavior<,>)).As(typeof(IPipelineBehavior<,>));
        //builder.RegisterGeneric(typeof(TransactionBehaviour<,>)).As(typeof(IPipelineBehavior<,>));

    }
}
