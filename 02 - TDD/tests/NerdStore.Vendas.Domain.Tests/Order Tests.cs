using NerdStore.Vendas.Domain;
using System;
using System.Linq;
using Xunit;
namespace NerStore.Vendas.Domain.Tests
{
    public class PedidoTest
    {
        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Pedidos Test")]
        public void AdicionarItemPedido_NovoPedido_DeveAtualizarValor()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var pedido = Order.OrderFactory.NewScketchOrder(orderId);
            
            var pedidoItem = new OrderItem(orderId, "Produto Teste", 2, 100);

            // Act 
            pedido.AddItem(pedidoItem);
            
            //Assert
            Assert.Equal(200,pedido.TotalValue);
        }

        [Fact(DisplayName = "Adicionar Item Pedido Existente")]
        [Trait("Categoria", "Pedidos Teste")]
        public void AdicionarItemPedido_ItemJaExistente_DeveIncrementarUnidadesSomarValores()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var pedido = Order.OrderFactory.NewScketchOrder(orderId);
            var pedidoItem = new OrderItem(orderId, "Produto Teste", 2, 100);
            pedido.AddItem(pedidoItem);

            var pedidoItem2 = new OrderItem(orderId, "Produto Teste", 1, 100);

            //Act
            pedido.AddItem(pedidoItem2);

            //Assert
            Assert.Equal(1, pedido.OrderItems.Count);
            Assert.Equal(300, pedido.TotalValue);
            Assert.Equal(3, pedido.OrderItems.FirstOrDefault(p => p.ProductId == orderId).Ammount);
            
        }
    }
}
