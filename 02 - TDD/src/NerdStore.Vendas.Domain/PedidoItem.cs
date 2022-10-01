using System;
using System.Collections.Generic;
using System.Text;

namespace NerdStore.Vendas.Domain
{
    public class PedidoItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Ammount { get; private set; }
        public decimal UnitValue { get; private set; }

        public PedidoItem(Guid productId, string productName, int ammount, decimal unitValue)
        {
            ProductId = productId;
            ProductName = productName;
            Ammount = ammount;
            UnitValue = unitValue;
        }

       
    }
}
