using MediatR;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Application.Commands
{
    public class OrderCommandHandler : IRequestHandler<AddOrderItemCommand, bool>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMediator mediator;

        public OrderCommandHandler(IOrderRepository orderRespository, IMediator mediator = null)
        {
            this.orderRepository = orderRespository;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(AddOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message))
            {
                return false;
            }

            var order = await orderRepository.GetOrderItemByClientId(message.ClientId);
            var orderItem = new OrderItem(message.ProductId, message.Name, message.Ammount, message.UnitValue);

            if (order == null)
            {
                order = Order.OrderFactory.NewScketchOrder(message.ClientId);

                order.AddItem(orderItem);
                this.orderRepository.Add(order);
            }
            else
            {
                var orderExists = order.VerifyOrderItemExists(orderItem);
                order.AddItem(orderItem);

                if (orderExists)
                {
                    this.orderRepository
                        .UpdateItem(order.OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId));
                }
                else
                {
                    this.orderRepository.AddItem(orderItem);
                }

                this.orderRepository.Update(order);
            }

            order.AddEvent(new AddedOrderItemEvent(order.Id, order.ClientId, message.ProductId,
                message.Name, message.UnitValue, message.Ammount));

            return await this.orderRepository.UnitOfWork.Commit();
        }

        private bool ValidateCommand(Command message)
        {
            if (!message.IsValid())
            {
                foreach (var error in message.ValidationResult.Errors)
                {
                    this.mediator
                       .Publish(new DomainNotification(message.MessageType, error.ErrorMessage));
                }

                return false;
            }

            return true;
        }
    }
}
