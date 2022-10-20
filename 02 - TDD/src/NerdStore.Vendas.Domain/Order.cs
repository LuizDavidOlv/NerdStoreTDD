﻿using NerdStore.Core.DomainObjects;
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

        private bool VerifyOrderItemExists(OrderItem orderItem)
        {
            return OrderItems.Any(p => p.ProductId == orderItem.ProductId);
        }

        private void VerifyOrderItemDoesNotExists(OrderItem orderItem)
        {
            if (!VerifyOrderItemExists(orderItem))
            {
                throw new DomainException("The item does not exists in the current order");
            }
        }

        private void VerifyMaxAmmount(OrderItem newOrderItem)
        {
            var orderAmmount = newOrderItem.Ammount;
            OrderItem existingItem = null;
            if (VerifyOrderItemExists(newOrderItem))
            {
                existingItem = this.OrderItems.FirstOrDefault(p => p.ProductId == newOrderItem.ProductId);
                orderAmmount += existingItem.Ammount;
            }

            if (orderAmmount > MaxItemsAllowed)
            {
                throw new DomainException($"The maximun ammount of an item is {MaxItemsAllowed}");
            }
        }

        public void AddItem(OrderItem newOrderItem)
        {
            VerifyMaxAmmount(newOrderItem);
            
            if (VerifyOrderItemExists(newOrderItem))
            {
                var existingItem = this.OrderItems.FirstOrDefault(p => p.ProductId == newOrderItem.ProductId);
                existingItem.AddUnit(newOrderItem.Ammount);
                newOrderItem = existingItem;
                this.orderItems.Remove(existingItem);
            }

            this.orderItems.Add(newOrderItem);
            this.UpdateOrderPriceValue();

        }

        public void UpdateItem(OrderItem orderItem)
        {
            VerifyOrderItemDoesNotExists(orderItem);
            VerifyMaxAmmount(orderItem);
            
            var existingItem = this.OrderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
            this.orderItems.Remove(existingItem);
            this.orderItems.Add(orderItem);
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
