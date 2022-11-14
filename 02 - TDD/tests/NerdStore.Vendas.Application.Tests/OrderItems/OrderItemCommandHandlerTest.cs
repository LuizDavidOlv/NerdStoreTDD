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
        private readonly Guid clientId;
        private readonly Guid productId;
        private readonly Order order;
        private readonly AutoMocker mocker;
        private readonly OrderCommandHandler orderCommandHanlder;

        public OrderItemCommandHandlerTest()
        {
            mocker = new AutoMocker();
            orderCommandHanlder = mocker.CreateInstance<OrderCommandHandler>();
            clientId = Guid.NewGuid();
            productId = Guid.NewGuid();
            order = Order.OrderFactory.NewScketchOrder(clientId);
        }

        [Fact(DisplayName = "Add new order item successfully ")]
        [Trait("Command Handler", "Vendas - Order Commands Handler")]
        public async Task AddOrderItem_NewOrder_ShoudldAddNewOrderItemSuccessfully()
        {
            // Arrange
            var orderCommand = new AddOrderItemCommand(clientId, productId, "Produto Teste", 2, 100);
            this.mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await this.orderCommandHanlder.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }


        [Fact(DisplayName = "Add new order item successfully to existing scketch order")]
        [Trait("Command Handler", "Vendas - Order Command Handler")]
        public async Task AddOrderItem_NewOrderItemToScketchOrder_ShoudldAddNewOrderItemSuccessfully()
        {
            //Arrange
            var existingOrderItem = new OrderItem(Guid.NewGuid(), "Produto Teste", 2, 100);
            order.AddItem(existingOrderItem);
            
            var orderCommand = new AddOrderItemCommand(clientId, Guid.NewGuid(), "Produto Teste", 3, 160);

            
            this.mocker.GetMock<IOrderRepository>()
                .Setup(r => r.GetOrderItemByClientId(clientId)).Returns(Task.FromResult(order));
            
            this.mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            //Act
            var result = await this.orderCommandHanlder.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(r => r.AddItem(It.IsAny<OrderItem>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Update existing order item successfully to existing scketch order")]
        [Trait("Command Handler", "Vendas - Order Command Handler")]
        public async Task AddOrderItem_ExistingOrderItemToScketchOrder_ShoudldAddNewOrderItemSuccessfully()
        {
            //Arrange
            var order = Order.OrderFactory.NewScketchOrder(this.clientId);
            var existingOrderItem = new OrderItem(this.productId, "Produto Teste", 2, 100);
            order.AddItem(existingOrderItem);

            var orderCommand = new AddOrderItemCommand(this.clientId, this.productId, "Produto Teste", 2, 160);

            mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));
            mocker.GetMock<IOrderRepository>()
                .Setup(r => r.GetOrderItemByClientId(clientId)).Returns(Task.FromResult(order));

            //Act
            var result = await this.orderCommandHanlder.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UpdateItem(It.IsAny<OrderItem>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.Update(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
        }
        
        [Fact(DisplayName = "Add Invalid Command")]
        [Trait("Command Handler", "Vendas - Order Command Handler")]
        public async Task AddOrderItem_InvalidCommand_ShoudlReturnFalsseAndPublishNotificationEvents()
        {
            //Arrange
            var orderCommand = new AddOrderItemCommand(Guid.Empty, Guid.Empty, "", 0, 0);
            //Act
            var result = await this.orderCommandHanlder.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.False(result);
            mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}