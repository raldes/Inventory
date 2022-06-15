using Inventory.Domain.Entities;
using System;
using Xunit;

namespace Inventory.Domain.UnitTests
{
    /// <summary>
    /// Unit test: testing domain methods
    /// </summary>
    public class ItemUnitTest
    {
        [Fact]
        public void new_item_status_is_draft()
        {
            //Arrange    
            var name = "name1";
            var description = "description1";
            var expirationDate = DateTime.Now.AddDays(2);
            var itemTypeId = 1;

            //Act 
            var item = Item.NewItemDraft(name, description, expirationDate, itemTypeId );

            //Assert
            Assert.Equal(item.ItemStatusId, ItemStatus.Draft.Id);
            Assert.NotNull(item);
        }

        [Fact]
        public void new_item_created_status_is_creted()
        {
            //Arrange    
            var name = "name1";
            var description = "description1";
            var expirationDate = DateTime.Now.AddDays(2);
            var itemTypeId = 1;

            //Act 
            var item = Item.NewItemCreated(name, description, expirationDate, itemTypeId );

            //Assert
            Assert.Equal(item.ItemStatusId, ItemStatus.Created.Id);
            Assert.NotNull(item);
        }
        
        [Fact]
        public void update_item_status_is_updated()
        {
            //Arrange    
            var name = "name1";
            var description = "description1";
            var expirationDate = DateTime.Now.AddDays(2);
            var itemTypeId = 1;

            //Act 
            var item = Item.NewItemCreated(name, description, expirationDate, itemTypeId );
            item.UpdateThisItem("otro", description, expirationDate, itemTypeId);

            //Assert
            Assert.Equal(item.ItemStatusId, ItemStatus.Updated.Id);
            Assert.NotNull(item);
        }
        
        [Fact]
        public void remove_item_status_is_removed()
        {
            //Arrange    
            var name = "name1";
            var description = "description1";
            var expirationDate = DateTime.Now.AddDays(2);
            var itemTypeId = 1;

            //Act 
            var item = Item.NewItemDraft(name, description, expirationDate, itemTypeId );
            item.RemoveThisItem();

            //Assert
            Assert.Equal(item.ItemStatusId, ItemStatus.Removed.Id);
            Assert.NotNull(item);
        }
        
        [Fact]
        public void expirationdate_yesterday_is_expired_item()
        {
            //Arrange    
            var name = "name1";
            var description = "description1";
            var expirationDate = DateTime.Now.AddDays(-1);
            var itemTypeId = 1;

            //Act 
            var item = Item.NewItemDraft(name, description, expirationDate, itemTypeId );
            item.CheckTodayExpirationDate();

            //Assert
            Assert.Equal(item.ItemStatusId, ItemStatus.Expired.Id);
            Assert.NotNull(item);
        }
        
        [Fact]
        public void expirationdate_tomorrow_is_not_expired_item()
        {
            //Arrange    
            var name = "name1";
            var description = "description1";
            var expirationDate = DateTime.Now.AddDays(1);
            var itemTypeId = 1;

            //Act 
            var item = Item.NewItemDraft(name, description, expirationDate, itemTypeId );
            item.CheckTodayExpirationDate();

            //Assert
            Assert.NotEqual(item.ItemStatusId, ItemStatus.Expired.Id);
            Assert.NotNull(item);
        }
    }
}
