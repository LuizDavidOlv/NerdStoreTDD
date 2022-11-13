using MediatR;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Application.Events;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var orderItem = new OrderItem(message.ProductId, message.Name, message.Ammount, message.UnitValue);
            var order = Order.OrderFactory.NewScketchOrder(message.ClientId);
            order.AddItem(orderItem);
            
            this.orderRepository.Add(order);
            
            order.AddEvent(new AddedOrderItemEvent(order.Id,order.ClientId,message.ProductId,
                message.Name,message.UnitValue,message.Ammount));
            
            return await this.orderRepository.UnitOfWork.Commit();
        }
    }
}
