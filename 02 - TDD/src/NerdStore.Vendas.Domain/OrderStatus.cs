using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain
{
    public enum OrderStatus
    {
        Scketch =0,
        Initiated =1,
        Payed =4,
        Delivered =5,
        Canceled =6
    }
}
