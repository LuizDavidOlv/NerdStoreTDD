using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace NerdStore.Vendas.Domain
{
    public class Pedido
    {
        public Pedido()
        {
            this.pedidoItens = new List<PedidoItem>();
        }
        
        public decimal TotalValue { get; private set; }
        
        private readonly List<PedidoItem> pedidoItens;
        public IReadOnlyCollection<PedidoItem> PedidoItens => this.pedidoItens;

        public void AddItem(PedidoItem pedidoItems)
        {
            this.pedidoItens.Add(pedidoItems);
            TotalValue = PedidoItens.Sum(p => p.UnitValue * p.Ammount);
        }   
    }
}
