using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Vendas.Domain
{
    public class Voucher
    {
        public string VoucherCode { get; private set; }
        public decimal? DiscountPercentual { get; private set; }
        public decimal? DiscountValue { get; private set; }
        public VoucherDiscountType VoucherDiscountType { get; private set; }
        public int Ammount { get; private set; }
        public DateTime ExpirationDate { get; private set; }
        public bool Active { get; private set; }
        public bool Used { get; private set; }

        public Voucher(string voucherCode, decimal? discountPercentual, decimal? discountValue, VoucherDiscountType voucherDiscountType, int ammount, DateTime expirationDate, bool active, bool used)
        {
            VoucherCode = voucherCode;
            DiscountPercentual = discountPercentual;
            DiscountValue = discountValue;
            VoucherDiscountType = voucherDiscountType;
            Ammount = ammount;
            ExpirationDate = expirationDate;
            Active = active;
            Used = used;
        }
        
        public ValidationResult ValidateVoucher()
        {
            return new VoucherValidation().Validate(this);
        }
    }
    
    public class VoucherValidation : AbstractValidator<Voucher>
    {
        public static string VoucherNotFoundCodeErrorMsg => "Voucher not found.";
        public static string ExpirationDateErrorMsg => "Expired voucher.";
        public static string ActiveErrorMsg => "This voucher is no longer valid.";
        public static string UsedErrorMsg => "This voucher has already been used.";
        public static string AmmountErrorMsg => "This voucher is not available anymore";
        public static string DiscountValueErrorMsg => "The discount value has to be greaater than 0.";
        public static string PercentualDiscountErrorMsg => "The discount percetange has to be greater than 0.";
        public static string DiscountTypeErrorMsg => "The discount type is invalid";
        
        public VoucherValidation()
        {
            RuleFor(c => c.VoucherCode)
                .NotEmpty()
                .WithMessage(VoucherNotFoundCodeErrorMsg);
            
            RuleFor(c => c.ExpirationDate)
                .Must(DateIsValid)
                .WithMessage(ExpirationDateErrorMsg);

            RuleFor(c => c.Active)
                .Equal(true)
                .WithMessage(ActiveErrorMsg);

            RuleFor(c => c.Used)
                .Equal(false)
                .WithMessage(UsedErrorMsg);

            RuleFor(c => c.Ammount)
                .GreaterThan(0)
                .WithMessage(AmmountErrorMsg);

            When(f => f.VoucherDiscountType == VoucherDiscountType.Value, () =>
            {
                RuleFor(f => f.DiscountValue)
                    .NotNull()
                    .WithMessage(DiscountValueErrorMsg)
                    .GreaterThan(0)
                    .WithMessage(DiscountValueErrorMsg);
            });

            When(f => f.VoucherDiscountType == VoucherDiscountType.Percentual, () =>
            {
                RuleFor(f => f.DiscountPercentual)
                    .NotNull()
                    .WithMessage(PercentualDiscountErrorMsg)
                    .GreaterThan(0)
                    .WithMessage(PercentualDiscountErrorMsg);

            });




        }
        protected static bool DateIsValid(DateTime date)
        {
            return date > DateTime.Now;
        }
    }
}
