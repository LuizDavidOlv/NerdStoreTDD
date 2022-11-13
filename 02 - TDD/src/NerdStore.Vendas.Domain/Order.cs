using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using NerdStore.Vendas.Domain;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NerdStore.Vendas.Domain
{
    public class Order : Entity
    {
        public static int MaxItemsAllowed => 15;
        public static int MinItemsAllowed = 1;
        
        protected Order()
        {
            this.orderItems = new List<OrderItem>();
        }

        public Guid ClientId { get; private set; }
        public decimal Discount { get; set; }
        public decimal TotalValue { get; private set; }
        public bool UsedVoucher { get; set; }
        public Voucher Voucher { get; private set; }
        private readonly List<OrderItem> orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => this.orderItems;
        public OrderStatus OrderStatus { get; private set; }
       


        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            ValidationResult result = voucher.ValidateVoucher();
            if (!result.IsValid) return result;

            Voucher = voucher;
            UsedVoucher = true;

            UpdateTotalValueDiscount();

            return result;
        }

        private void UpdateTotalValueDiscount()
        {
            if (!UsedVoucher) return;

            decimal discount = 0;
            var value = TotalValue;

            if (Voucher.VoucherDiscountType == VoucherDiscountType.Value)
            {
                if (Voucher.DiscountValue.HasValue)
                {
                    discount = Voucher.DiscountValue.Value;
                    value -= discount;
                }
            }
            else
            {
                if (Voucher.DiscountPercentual.HasValue)
                {
                    discount = (TotalValue * Voucher.DiscountPercentual.Value) / 100;
                    value -= discount;
                }
            }

            TotalValue = value < 0 ? 0 : value;
            Discount = discount;
        }
        private void UpdateOrderPriceValue()
        {
            TotalValue = OrderItems.Sum(p => p.UpdatePriceValue());
            UpdateTotalValueDiscount();
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

        public void RemoveItem(OrderItem orderItem)
        {
            VerifyOrderItemDoesNotExists(orderItem);
            this.orderItems.Remove(orderItem);
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
