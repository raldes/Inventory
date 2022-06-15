using Microsoft.EntityFrameworkCore;
using Inventory.Domain.Entities;
using MediatR;
using Inventory.Domain.Repositories;
using Inventory.Infra.Database.Extensions;

namespace Inventory.Infra.Database
{
    public class ItemsDbContext : DbContext, IUnitOfWork
    {
        private readonly IMediator _mediator;

        public ItemsDbContext(DbContextOptions<ItemsDbContext> options) : base(options)
        {
        }
       
        public ItemsDbContext(DbContextOptions<ItemsDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            System.Diagnostics.Debug.WriteLine("Items DbContext::ctor ->" + this.GetHashCode());
        }

        public DbSet<Item> Items { get; set; }
        public DbSet<ItemType> ItemTypes { get; set; }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // 
            // Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return true;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ItemType>(eb =>
            {
                eb.HasKey("ItemTypeId");

                eb.HasMany(it => it.Items)
                     .WithOne(i => i.ItemType)
                     .HasForeignKey(i => i.ItemTypeId);
            });

            modelBuilder.Entity<Item>(eb =>
            {
                eb.HasKey("ItemId");


            });

        }

        public override int SaveChanges()
        {
            try
            {
               foreach (var entry in this.ChangeTracker.Entries())
                {
                    if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Added)
                    {
                        entry.Property("created_datetime").CurrentValue = DateTime.UtcNow;
                        entry.Property("ruid").CurrentValue = Guid.NewGuid();
                    }

                    if (entry.State == Microsoft.EntityFrameworkCore.EntityState.Modified)
                    {
                        entry.Property("modified_datetime").CurrentValue = DateTime.UtcNow;
                    }
                }

                return base.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
 
        }

        public void AddTestData()
        {
            if(ItemTypes.Any())
            {
                return;
            }

            var itemType1 = new ItemType
            {
                Code = "type1",
                Description = "type 1",
            };
            var itemType2 = new ItemType
            {
                Code = "type2",
                Description = "type 2",
            };
            var itemType3 = new ItemType
            {
                Code = "type3",
                Description = "type 3",
            };

            ItemTypes.Add(itemType1);
            ItemTypes.Add(itemType2);
            ItemTypes.Add(itemType3);

            SaveChanges();

            var item1 = Item.NewItemDraft("item1", "item 1", DateTime.Now, itemType1.ItemTypeId);
            var item2 = Item.NewItemDraft("item2", "item 2", DateTime.Now, itemType1.ItemTypeId);
            var item3 = Item.NewItemDraft("item3", "item 3", DateTime.Now, itemType3.ItemTypeId);

            Items.Add(item1);
            Items.Add(item2);
            Items.Add(item3);

            SaveChanges();
        }

    }
}
