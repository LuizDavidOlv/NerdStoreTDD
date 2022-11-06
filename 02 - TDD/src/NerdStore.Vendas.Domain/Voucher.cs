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
        public static string InvalidVoucherCodeErrorMsg => "Voucher sem código válido.";
        public static string ExpirationDateErrorMsg => "Este voucher está expirado.";
        public static string ActiveErrorMsg => "Este voucher não é mais válido.";
        public static string UsedErrorMsg => "Este voucher já foi utilizado.";
        public static string AmmountErrorMsg => "Este voucher não está mais disponível";
        public static string DiscountValueErrorMsg => "O valor do desconto precisa ser superior a 0";
        public static string PercentualDescontoErrorMsg => "O valor da porcentagem de desconto precisa ser superior a 0";
        public static string DiscountTypeErrorMsg => "O tipo de desconto não é válido";
        
        public VoucherValidation()
        {
            RuleFor(c => c.VoucherCode)
                .NotEmpty()
                .WithMessage(InvalidVoucherCodeErrorMsg);
            
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
                    .WithMessage(PercentualDescontoErrorMsg)
                    .GreaterThan(0)
                    .WithMessage(PercentualDescontoErrorMsg);

            });




        }
        protected static bool DateIsValid(DateTime date)
        {
            return date > DateTime.Now;
        }
    }
}
