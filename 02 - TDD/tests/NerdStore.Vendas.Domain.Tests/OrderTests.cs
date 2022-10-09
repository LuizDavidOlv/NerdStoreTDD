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
        [Trait("Category", "Order Test")]
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
        [Trait("Category", "Order Test")]
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

        [Fact(DisplayName = "Add orderItem over max allowed")]
        [Trait("Category","Order Test")]
        public void AddOrderItem_ItemOverMaxAllowed_ShouldReturnAnException()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Test Item",Order.MaxItemsAllowed +1, 100);

            //Act and Assert 
            Assert.Throws<DomainException>(() => order.AddItem(orderItem));
        }

       
    }
}
