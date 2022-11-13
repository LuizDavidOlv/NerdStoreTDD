using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Vendas.Application.Commands;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.OrderItems
{
    public class OrderItemCommandHandlerTest
    {
        private readonly AutoMocker mocker;
        private readonly OrderCommandHandler orderCommandHanlder;

        public OrderItemCommandHandlerTest()
        {
            mocker = new AutoMocker();
            orderCommandHanlder = mocker.CreateInstance<OrderCommandHandler>();
        }
        
        [Fact(DisplayName= "Successfully add new order item")]
        [Trait("Command Handler", "Vendas - OrderItem Commands")]
        public async Task AddOrderItem_NewOrder_SuccessfullyAddNewOrderItem()
        {
            // Arrange
            
            var orderCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);
            
            var mocker = new AutoMocker();
            var orderhandler = mocker.CreateInstance<OrderCommandHandler>();

            mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await orderhandler.Handle(orderCommand, CancellationToken.None);
            
            // Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            //mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(),CancellationToken.None), Times.Once);
        }
    }
}
