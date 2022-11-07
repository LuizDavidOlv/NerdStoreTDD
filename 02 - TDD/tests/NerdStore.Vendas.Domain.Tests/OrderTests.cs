using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using NerdStore.Vendas.Domain;
using System;
using System.Linq;
using Xunit;
namespace NerdStore.Vendas.Domain.Tests
{
    public class OrderTest
    {
        [Fact(DisplayName = "Add new orderItem")]
        [Trait("Add", "Sales - Order")]
        public void AddOrderItem_NewOrder_ShouldUpdateValue()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);

            var orderItem = new OrderItem(orderId, "Test Item", 2, 100);

            // Act 
            order.AddItem(orderItem);

            //Assert
            Assert.Equal(200, order.TotalValue);
        }

        [Fact(DisplayName = "Add existing orderItem")]
        [Trait("Add", "Sales - Order")]
        public void AddOrderItem_ExistingOrder_ShouldIncrementItensAndUpdateValue()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Test Item", 2, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(orderId, "Test Item", 1, 100);

            //Act
            order.AddItem(orderItem2);

            //Assert
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(300, order.TotalValue);
            Assert.Equal(3, order.OrderItems.FirstOrDefault(p => p.ProductId == orderId).Ammount);

        }

        [Fact(DisplayName = "Add existing orderItem over max allowed")]
        [Trait("Add", "Sales - Order")]
        public void AddOrderItem_ItemOverMaxAllowed_ShouldReturnAnException()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Test Item", 13, 100);
            order.AddItem(orderItem);
            var orderItem2 = new OrderItem(orderId, "Teste Item", 6, 100);

            //Act and Assert 
            Assert.Throws<DomainException>(() => order.AddItem(orderItem2));
        }

        [Fact(DisplayName = "Update non existing item")]
        [Trait("Update", "Sales - Order")]
        public void UpdateOrderItem_ItemDoesNotExist_ShouldReturnAnException()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var newOrderItem = new OrderItem(orderId, "Teste Item", 4, 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(newOrderItem));
        }

        [Fact(DisplayName = "Update existing item ammount")]
        [Trait("Update", "Sales - Order")]
        public void UpdateOrderItem_ValidItem_ShouldUpdateOrderItemAmmount()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Teste item", 3, 100);
            order.AddItem(orderItem);

            var updatedOrderitem = new OrderItem(orderId, "Teste item", 5, 100);
            var newAmmount = updatedOrderitem.Ammount;
            //Act
            order.UpdateItem(updatedOrderitem);

            //Assert    
            Assert.Equal(newAmmount, order.OrderItems.FirstOrDefault(p => p.ProductId == orderId).Ammount);
        }

        [Fact(DisplayName = "Update existing order total ammount")]
        [Trait("Update", "Sales - Order")]
        public void UpdateOrderItem_Ammount_ShouldReturnUpdatedAmmount()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Teste Item", 6, 100);
            order.AddItem(orderItem);
            var updatedOrderItem = new OrderItem(orderId, "Teste Item", 1, 100);
            var updatedTotalValue = updatedOrderItem.Ammount * updatedOrderItem.UnitValue;
            //Act
            order.UpdateItem(updatedOrderItem);

            //Assert
            Assert.Equal(updatedTotalValue, order.TotalValue);
        }

        [Fact(DisplayName = "Update existing item over Max Allowed")]
        [Trait("Update", "Sales - Order")]
        public void UpdateOrderItem_ItemOverMaxAllowed_ShouldReturnAnException()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Teste Item", 5, 100);
            var updatedOrderItem = new OrderItem(orderId, "Teste Item", 156, 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(updatedOrderItem));
        }

        [Fact(DisplayName = "Remove non existing orderItem")]
        [Trait("Remove", "Sales - Order")]
        public void RemoveOrderItem_ItemDoesNotExist_ShouldReturnAnException()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Teste item", 3, 100);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.RemoveItem(orderItem));
        }

        [Fact(DisplayName = "Remove existing orderItem")]
        [Trait("Remove", "Sales - Order")]
        public void RemoveOrderItem_ItemExists_ShouldUpdateTotalValue()
        {
            //Arrange
            var orderId = Guid.NewGuid();
            var order = Order.OrderFactory.NewScketchOrder(orderId);
            var orderItem = new OrderItem(orderId, "Shazan", 3, 100);
            order.AddItem(orderItem);
            var orderItem2 = new OrderItem(Guid.NewGuid(), "Opa", 4, 180);
            order.AddItem(orderItem2);
            var totalOrderValue = orderItem2.Ammount * orderItem2.UnitValue;

            //Act
            order.RemoveItem(orderItem);

            //Assert
            Assert.Equal(totalOrderValue, order.TotalValue);

        }

        [Fact(DisplayName = "Apply valid voucher")]
        [Trait("Apply", "Sales - Voucher")]
        public void ApplyVoucher_ValidVoucher_ShouldReturnValidationResultTrue()
        {
            //Arrange 
            Guid orderId = Guid.NewGuid();
            Order order = Order.OrderFactory.NewScketchOrder(orderId);
            OrderItem firstOrderItem = new OrderItem(orderId, "Teste Item", 3, 100);
            order.AddItem(firstOrderItem);
            OrderItem secondOrderItem = new OrderItem(Guid.NewGuid(), "Teste Item 2", 4, 150);
            order.AddItem(secondOrderItem);
            Voucher newVoucher = new Voucher("PROMO-15-REAIS", null, 15, VoucherDiscountType.Value, 1, DateTime.Now.AddDays(15), true, false);

            //Act
            ValidationResult result = order.ApplyVoucher(newVoucher);

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Apply invalid voucher")]
        [Trait("Apply", "Sales - Voucher")]
        public void ApplyVoucher_InvalidVoucher_ShouldReturnValidationResultFalse()
        {
            //Arrange 
            Guid orderId = Guid.NewGuid();
            Order order = Order.OrderFactory.NewScketchOrder(orderId);
            OrderItem firstOrderItem = new OrderItem(orderId, "Teste Item", 3, 100);
            order.AddItem(firstOrderItem);
            OrderItem secondOrderItem = new OrderItem(Guid.NewGuid(), "Teste Item 2", 4, 150);
            order.AddItem(secondOrderItem);
            Voucher newVoucher = new Voucher("", null, 15, VoucherDiscountType.Value, 1, DateTime.Now.AddDays(15), true, false);

            //Act
            ValidationResult result = order.ApplyVoucher(newVoucher);

            //Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName ="Apply voucher value discount")]
        [Trait("Discount", "Sales - Voucher")]
        public void ApplyVoucher_VoucherValueTypeDiscount_ShouldUpdateTotalValue()
        {
            //Arrange 
            Guid orderId = Guid.NewGuid();
            Order order = Order.OrderFactory.NewScketchOrder(orderId);
            OrderItem firstOrderItem = new OrderItem(orderId, "Teste Item", 3, 100);
            order.AddItem(firstOrderItem);
            OrderItem secondOrderItem = new OrderItem(Guid.NewGuid(), "Teste Item 2", 4, 150);
            order.AddItem(secondOrderItem);
            Voucher newVoucher = new Voucher("PROMO-15-REAIS", null, 15, VoucherDiscountType.Value, 1, DateTime.Now.AddDays(15), true, false);
            var totalValue = order.TotalValue;
            var voucherValue = newVoucher.DiscountValue;

            //Act
            order.ApplyVoucher(newVoucher);

            //Assert
            Assert.Equal(totalValue - voucherValue, order.TotalValue);
        }

        [Fact(DisplayName = "Apply voucher percentual discount")]
        [Trait("Discount", "Sales - Voucher")]
        public void ApplyVoucher_VoucherPercentualTypeDiscount_ShouldUpdateTotalValue()
        {
            //Arrange 
            Guid orderId = Guid.NewGuid();
            Order order = Order.OrderFactory.NewScketchOrder(orderId);
            OrderItem firstOrderItem = new OrderItem(orderId, "Teste Item", 3, 100);
            order.AddItem(firstOrderItem);
            OrderItem secondOrderItem = new OrderItem(Guid.NewGuid(), "Teste Item 2", 4, 150);
            order.AddItem(secondOrderItem);
            
            Voucher newVoucher = new Voucher("PROMO-15OFF", 15, null, VoucherDiscountType.Percentual, 1, DateTime.Now.AddDays(15), true, false);

            var discount = (order.TotalValue * newVoucher.DiscountPercentual) / 100;
            var totalValueWithDiscount = order.TotalValue - discount;

            //Act
            order.ApplyVoucher(newVoucher);
            
            //Assert
            Assert.Equal(totalValueWithDiscount, order.TotalValue);
        }
    }
}

