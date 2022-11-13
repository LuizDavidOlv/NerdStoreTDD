using MediatR;
using NerdStore.Core.Messages;

namespace NerdStore.Vendas.Application.Events
{
    public class AddedOrderItemEvent : Event
    {
        public Guid ClientId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; set; }
        public decimal UnitValue { get; private set; }
        public int Ammount { get; private set; }

        public AddedOrderItemEvent(Guid clientId, Guid orderId, Guid productId, string productName, decimal unitValue, int ammount)
        {
            ClientId = clientId;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            UnitValue = unitValue;
            Ammount = ammount;
        }
    }
}
