using FluentValidation;
using NerdStore.Core.Messages;
using NerdStore.Vendas.Domain;


namespace NerdStore.Vendas.Application.Commands
{
    public class AddOrderItemCommand : Command
    {
        public Guid ClientId { get; set; }
        public Guid ProductId { get; set; }
        public string Name { get; set; }
        public int Ammount { get; set; }
        public decimal UnitValue { get; set; }

        public AddOrderItemCommand(Guid clientId, Guid productId, string name, int ammount, decimal unitValue)
        {
            ClientId = clientId;
            ProductId = productId;
            Name = name;
            Ammount = ammount;
            UnitValue = unitValue;
        }

        public override bool IsValid()
        {
            ValidationResult = new AddOrderItemValidation().Validate(this);
            return ValidationResult.IsValid;
        }


    }

    public class AddOrderItemValidation : AbstractValidator<AddOrderItemCommand>
    {

        public static string ClientIdErrorMsg => "Invalid Clent Id";
        public static string ProductIdErrorMsg => "Invalid product Id";
        public static string NameErrorMsg => "Product name not informed";
        public static string MaxAmmountErrorMsg => $"item max ammount is {Order.MaxItemsAllowed}";
        public static string MinAmmountErrorMsg => "Minimal item ammount is 1.";
        public static string ValueErrorMsg => "Item value has to be greater than 0.";

        public AddOrderItemValidation()
        {
            RuleFor(c => c.ClientId)
                .NotEqual(Guid.Empty)
                .WithMessage(ClientIdErrorMsg);

            RuleFor(c => c.ProductId)
                .NotEqual(Guid.Empty)
                .WithMessage(ProductIdErrorMsg);

            RuleFor(c => c.Name)
                .NotEmpty()
                .WithMessage(NameErrorMsg);

            RuleFor(c => c.Ammount)
                .GreaterThan(0)
                .WithMessage(MinAmmountErrorMsg)
                .LessThanOrEqualTo(Order.MaxItemsAllowed)
                .WithMessage(MaxAmmountErrorMsg);

            RuleFor(c => c.UnitValue)
                .GreaterThan(0)
                .WithMessage(ValueErrorMsg);
        }
    }
}

