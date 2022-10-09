using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Vendas.Domain
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Ammount { get; private set; }
        public decimal UnitValue { get; private set; }

        public OrderItem(Guid productId, string productName, int ammount, decimal unitValue)
        {
            if (ammount < Order.MinItemsAllowed)
            {
                throw new DomainException("The minimun ammount of an item is 1");
            }
            
            ProductId = productId;
            ProductName = productName;
            Ammount = ammount;
            UnitValue = unitValue;
        }

        internal void AddUnit(int unit)
        {
            Ammount += unit;
        }

        internal decimal UpdatePriceValue()
        {
            return Ammount * UnitValue;
        }

    }
}
