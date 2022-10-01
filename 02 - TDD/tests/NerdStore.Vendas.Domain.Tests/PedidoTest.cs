using NerdStore.Vendas.Domain;
using System;
using Xunit;
namespace NerStore.Vendas.Domain.Tests
{
    public class PedidoTest
    {
        [Fact(DisplayName = "Adicionar Item Novo Pedido")]
        [Trait("Categoria", "Vendas - Pedido")]
        public void AdicionarItemPedido_ItemNaoAdicionado_DeveRetornarException()
        {
            // Arrange
            var pedido = new Pedido();
            var pedidoItem = new PedidoItem(Guid.NewGuid(), "Produto Teste", 2, 100);

            // Act 
            pedido.AddItem(pedidoItem);
            
            //Assert
            Assert.Equal(200,pedido.TotalValue);
        }
    }
}
