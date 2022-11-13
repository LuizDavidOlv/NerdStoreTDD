using NerdStore.Vendas.Application.Commands;
using Xunit;

namespace NerdStore.Vendas.Application.Tests.OrderItems
{
    public class AddOrderItemCommandTest
    {
        [Fact(DisplayName = "Add valid item command")]
        [Trait("Add Command", "Vendas - OrderItem Commands")]
        public void AddOrderItemCommand_ValidCommand_ValidationShouldReturnTrue()
        {
            //Arrange
            var pedidoCommand = new AddOrderItemCommand(Guid.NewGuid(), Guid.NewGuid(), "Produto Teste", 2, 100);

            //Act
            var result = pedidoCommand.IsValid();
            
            //Assert
            Assert.True(result);
        }

        [Fact(DisplayName = "Add invalid item command")]
        [Trait("Add Command", "Vendas - OrderItem Commands")]
        public void AddOrderItemComman_InvalidCommand_ValidationShouldReturnFalse()
        {
            //Arrange
            var pedidoCommand = new AddOrderItemCommand(Guid.Empty, Guid.NewGuid(), "", 0, 0);

            //Act
            var result = pedidoCommand.IsValid();
            
            //Assert
            Assert.False(result);
            Assert.Contains(AddOrderItemValidation.NameErrorMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.MinAmmountErrorMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.ClientIdErrorMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddOrderItemValidation.ValueErrorMsg, pedidoCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }
}
