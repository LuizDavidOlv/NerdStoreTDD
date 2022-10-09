using NerdStore.Core.DomainObjects;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NerdStore.Vendas.Domain
{
    public class Order
    {
        protected Order()
        {
            this.orderItems = new List<OrderItem>();
        }
        
        public static int MaxItemsAllowed => 15;
        public static int MinItemsAllowed = 1;
        public decimal TotalValue { get; private set; }
        
        private readonly List<OrderItem> orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => this.orderItems;
        public OrderStatus OrderStatus { get; private set; }
        public Guid ClientId { get; private set; }


        public void UpdateOrderPriceValue()
        {
            TotalValue = OrderItems.Sum(p => p.UpdatePriceValue());
        }
        
        public void AddItem(OrderItem newOrderItem)
        {
            if(this.orderItems.Any(p => p.ProductId == newOrderItem.ProductId))
            {
                var oldOrderItem = this.orderItems.FirstOrDefault(p => p.ProductId == newOrderItem.ProductId);
                oldOrderItem.AddUnit(newOrderItem.Ammount);
                newOrderItem = oldOrderItem;
                this.orderItems.Remove(oldOrderItem);
            }

            this.orderItems.Add(newOrderItem);

            if (this.orderItems[0].Ammount > MaxItemsAllowed)
            {
                throw new DomainException("Order can't have more than 15 items");
            }

            this.UpdateOrderPriceValue();
            
        }   

        public void ChangeToScketch()
        {
            OrderStatus = OrderStatus.Scketch;
        }

        //The OrderFactory class is a nested class. Due to this, it is possible to create an instante of the protected Order Class
        public static class OrderFactory
        {
            public static Order NewScketchOrder(Guid clientId)
            {
                var order = new Order
                {
                    ClientId = clientId,
                };
                order.ChangeToScketch();
                return order;
            }
        }
    }
}
