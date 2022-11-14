using NerdStore.Core.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain
{
    public interface IOrderRepository : IRepository<Order>
    {
        void Add(Order order);
        void Update(Order order);
        Task<Order> GetOrderItemByClientId(Guid clientId);
        void AddItem(OrderItem orderItem);
        void UpdateItem(OrderItem orderItem);
    }
}
