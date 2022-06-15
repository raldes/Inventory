using Inventory.Domain.Events;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Inventory.Domain.Entities
{
    public class Item : EFEntity
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int ItemTypeId { get; set; }

        public DateTime ExpirationDate { get; set; }

        public int ItemStatusId { get; set; }

        [JsonIgnore]
        public virtual ItemType ItemType { get; set; }

        public Item()
        {
        }

        public static Item NewItemDraft(string name, string description, DateTime expirationDate, int itemTypeId)
        {
            var item = new Item(name, description, expirationDate, itemTypeId);
            item.SetStatusDraft();
            return item;
        }

        public static Item NewItemCreated(string name, string description, DateTime expirationDate, int itemTypeId)
        {
            var item = new Item(name, description, expirationDate, itemTypeId);
            item.SetStatusCreated();
            return item;
        }

        public Item(string name, string description, DateTime expirationDate, int itemTypeId)
        {
            Name = name;
            Description = description;
            ExpirationDate = expirationDate;
            ItemTypeId = itemTypeId;
        }
        
 
        //rich entity: only these methods could to change the entity properties and estatus and generate domain events:

        public void RemoveThisItem()
        {
            SetStatusRemoved();
        }
        
        public void UpdateThisItem(string name, string description, DateTime expirationDate, int itemTypeId)
        {
            Name = name;
            Description = description;
            ExpirationDate = expirationDate;
            ItemTypeId = itemTypeId;

            SetStatusUpdated();
        }

        public void CheckTodayExpirationDate()
        {
            CheckExpirationDate(DateTime.Now);
        }

        public void CheckExpirationDate(DateTime compareDateTime)
        {
            if (ExpirationDate <= compareDateTime)
            {
                SetStatusExpired();
            }
        }

        public void SetStatusCreated()
        {
            ItemStatusId = ItemStatus.FromName("Created").Id;

            AddItemCreatedDomainEvent(this.Name, this.Description, this.ExpirationDate, this.ItemTypeId);
        }
       
        public void SetStatusExpired()
        {
            ItemStatusId = ItemStatus.FromName("Expired").Id;

            AddItemExpiredDomainEvent(this);
        }
       
        private void SetStatusDraft()
        {
            ItemStatusId = ItemStatus.FromName("Draft").Id;
        }

        private void AddItemCreatedDomainEvent(string name, string description, DateTime expirationDate, int itemTypeId)
        {
            var itemCreatedDomainEvent = new ItemCreatedDomainEvent(name, description, expirationDate, itemTypeId);

            this.AddDomainEvent(itemCreatedDomainEvent);
        }

        private void SetStatusRemoved()
        {
            ItemStatusId = ItemStatus.FromName("Removed").Id;

            AddItemRemovedDomainEvent(this);
        }
       
        private void AddItemRemovedDomainEvent(Item item)
        {
            var itemRemovedDomainEvent = new ItemRemovedDomainEvent(item);

            this.AddDomainEvent(itemRemovedDomainEvent);
        }
        
        private void SetStatusUpdated()
        {
            ItemStatusId = ItemStatus.FromName("Updated").Id;

            AddItemUpdatedDomainEvent(this);
        }
       
        private void AddItemUpdatedDomainEvent(Item item)
        {
            var itemUpdatedDomainEvent = new ItemUpdatedDomainEvent(item);

            this.AddDomainEvent(itemUpdatedDomainEvent);
        }
        
        private void AddItemExpiredDomainEvent(Item item)
        {
            var itemExpiredDomainEvent = new ItemExpiredDomainEvent(item);

            this.AddDomainEvent(itemExpiredDomainEvent);
        }    
    }
}
