using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class OrderItemTests 
    {
        [Fact(DisplayName = "Add orderItem minimun ammount")]
        [Trait("Category", "OrderItems Test")]
        public void AddOrderItem_ItemUnderMinimunRequired_ShouldReturnAnException()
        {
            //Arrange, Act and Assert 
            Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Test Item", Order.MinItemsAllowed - 1, 100));
        }
    }
}
