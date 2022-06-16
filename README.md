# Inventory

## Solution

This solution is DDD, CQRS and EventDriven designed.

- For queries the controller call ItemQueries service to get data from the repository (the queries could do direct calls to sql commands, views, etc)

- For commands the controller send Commands to CommandHandler methods (in the App layer), using MediatR.

- The command handler call de entities methods (in the domain layer) to create, update or remove the entity.

- Aggregate root: only the entity (in the domain layer) contains the logic to create and modify itself, modify its status and generate DomainEvents related to these modifications.

- The Command handler also call repositories to persist the changes. The repository uses UnitOfWorks pattern to persist the changes only if all the db operations where succesfully (transaction mode).

- The DomainEventHandler could send IntegrationEvent to comunicate other services about that event (EventDriven)

- Any other service could to susbcribe IntegrationEvents to know about these events.

For integration events we use RabbitMQ service (deployed with docker compose)


### Projects: 

The solution has 5 projects. All projects are .Net 6 :

- Inventory.Api: Api REST with Swagger, IoD and Configuration.

- Inventory.App: query services, validation services, events and integration event handlers, Automapper profiles (class lib)

- Inventory.Domain: Domain entities, domain events, Dtos, exceptions, and generic repository contracts (class lib)

- Inventory.Domain.UnitTest: Domain entities unit testing.

- Inventory.Infra: Infrastructure, Db Context and Generic EF Repository (class lib)


#### Entities

We have two entities: Item and ItemType. 

- Item: is the main entity. It has a foreign key to ItemTypes table.

- ItemType contains the types of items.

(See ER Diagram.png)

#### Database

- The Infrastructure uses EF Core InMemory provider. 

- When the application starts, some ItemTypes and Items are generated for testing purpose.

#### Nuget Packages:

- MediatR to connect Events with EventHandlers.

- Autofac to register (IoD) EventHandlers.

- Fluent Validation for entity validation.

- Entity Framework Core In Memmory.

- Oths.EventBus.RabbitMQ (myself). Publish and subscribe integration events.

- xUnit for unit testing.

- Serilog for logging.

- Automapper to map entities with dtos.


#### Deploy

- To deploy we use docker for each project, and docker compose to orchestrate the services. To run application use docker compose to start RabbitMQ infraestructure.
