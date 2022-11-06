using FluentValidation.Results;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Vendas.Domain.Tests
{
    public class VoucherTests
    {
        [Fact(DisplayName = "Validade voucher")]
        [Trait("Voucher", "Sales - Voucher")]
        public void ValidateVoucher_ValidVoucher_ShouldReturnTrue()
        {
            //Arrange 
            var voucher = new Voucher("PROMO-15-REAIS", null, 15, VoucherDiscountType.Value, 1, DateTime.Now.AddDays(15), true, false);

            //Act
            var result = voucher.ValidateVoucher();

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Invalid Voucher")]
        [Trait("Voucher", "Sales - Voucher")]
        public void ValidateVoucher_InvalidVoucher_ShouldReturnErrorList()
        {
            //Arrange
            var voucher = new Voucher("", null, 0, VoucherDiscountType.Value, 0, DateTime.Now.AddDays(-1), false, true);

            //Act
            var result = voucher.ValidateVoucher();
            
            //Assert
            Assert.False(result.IsValid);
            Assert.Equal(6, result.Errors.Count);
            Assert.Contains(VoucherValidation.ActiveErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.InvalidVoucherCodeErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.ExpirationDateErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.AmmountErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.UsedErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(VoucherValidation.DiscountValueErrorMsg, result.Errors.Select(c => c.ErrorMessage));
            
        }

        
        [Fact(DisplayName = "Voucher percentual is valid")]
        [Trait("Voucher", "Sales - Voucher")]
        public void ValidateVoucher_ValidPercentual_ShouldReturnTrue()
        {
            //Arrange
            var voucher = new Voucher("PROMO-15-REAIS", 15, 0, VoucherDiscountType.Percentual, 1, DateTime.Now.AddDays(15), true, false);

            //Act
            var result = voucher.ValidateVoucher();

            //Assert
            Assert.True(result.IsValid);
        }


        [Fact(DisplayName = "Voucher percentual is invalid")]
        [Trait("Voucher", "Sales - Voucher")]
        public void ValidateVoucher_InvalidPercentual_ShouldReturnFalse()
        {
            //Arrange
            var voucher = new Voucher("", null, null, VoucherDiscountType.Percentual, 0,DateTime.Now.AddDays(-1), false, true);
            //Act
            var result = voucher.ValidateVoucher();

            //Assert
            Assert.False(result.IsValid);
        }


    }
}
