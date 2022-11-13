﻿using NerdStore.Core.Data;
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
    }
}
