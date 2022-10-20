using NerdStore.Core.DomainObjects;
using NerdStore.Vendas.Domain;
using System;
using System.Linq;
using Xunit;
namespace NerStore.Vendas.Domain.Tests
{
    public class OrderTest
    {
        [Fact(DisplayName = "Add new orderItem")]
        [Trait("Add", "Sales - Order")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            
            var orderItem = new OrderItem(orderId, "Test Item", 2, 100);

            // Act 
            order.AddItem(orderItem);
            
            //Assert
            Assert.Equal(200,order.TotalValue);
        }

        [Fact(DisplayName = "Add existing orderItem")]
        [Trait("Add", "Sales - Order")]
        public void AdicionarItemPedido_ItemJaExistente_DeveIncrementarUnidadesSomarValores()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Test Item", 2, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(orderId, "Test Item", 1, 100);

            //Act
            order.AddItem(orderItem2);

            //Assert
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(300, order.TotalValue);
            Assert.Equal(3, order.OrderItems.FirstOrDefault(p => p.ProductId == orderId).Ammount);
            
        }

        [Fact(DisplayName = "Add existing orderItem over max allowed")]
        [Trait("Add", "Sales - Order")]
        public void AddOrderItem_ItemOverMaxAllowed_ShouldReturnAnException()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Test Item",13, 100);
            order.AddItem(orderItem);
            var orderItem2 = new OrderItem(orderId, "Teste Item", 6, 100);
            
            //Act and Assert 
            Assert.Throws<DomainException>(() => order.AddItem(orderItem2));
        }

        [Fact(DisplayName = "Update non existing item")]
        [Trait("Update", "Sales - Order")]
        public void UpdateOrderItem_ItemDoesNotExist_ShouldReturnAnException()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var newOrderItem = new OrderItem(orderId, "Teste Item", 4, 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(newOrderItem));
        }

        [Fact(DisplayName = "Update existing item ammount")]
        [Trait("Update", "Sales - Order")]
        public void UpdateOrderItem_ValidItem_ShouldUpdateOrderItemAmmount()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Teste item", 3, 100);
            order.AddItem(orderItem);

            var updatedOrderitem = new OrderItem(orderId, "Teste item", 5, 100);
            var newAmmount = updatedOrderitem.Ammount;
            //Act
            order.UpdateItem(updatedOrderitem);

            //Assert    
            Assert.Equal(newAmmount, order.OrderItems.FirstOrDefault(p => p.ProductId == orderId).Ammount);
        }

        [Fact(DisplayName = "Update existing order total ammount")]
        [Trait("Update", "Sales - Order")]
        public void UpdateOrderItem_Ammount_ShouldReturnUpdatedAmmount()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Teste Item", 6, 100);
            order.AddItem(orderItem);
            var updatedOrderItem = new OrderItem(orderId, "Teste Item", 1, 100);
            var updatedTotalValue = updatedOrderItem.Ammount * updatedOrderItem.UnitValue;
            //Act
            order.UpdateItem(updatedOrderItem);

            //Assert
            Assert.Equal(updatedTotalValue, order.TotalValue);
        }

        [Fact(DisplayName = "Update existing item over Max Allowed")]
        [Trait("Update", "Sales - Order")]
        public void UpdateOrderItem_ItemOverMaxAllowed_ShouldReturnAnException()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Teste Item", 5, 100);
            var updatedOrderItem = new OrderItem(orderId, "Teste Item", 156, 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(updatedOrderItem));
        }
    }
}
