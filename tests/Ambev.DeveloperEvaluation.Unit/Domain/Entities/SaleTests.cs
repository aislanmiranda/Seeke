using Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities
{
    public class SaleTests
	{
        [Fact(DisplayName = "Should cancel the sale and set IsCanceled to true")]
        public void Given_CancellationOfTheSale_When_StatusShouldReturnTrue()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();

            // Act
            sale.CancelSale();

            // Assert
            Assert.Equal(sale?.IsCanceled, true);
        }

        [Fact(DisplayName = "Should update customer info correctly")]
        public void Given_ValidCustomerData_When_UpdateCustomer_Then_PropertiesAreUpdated()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var newCustomerId = Guid.NewGuid();
            var newCustomerName = "New Customer";

            // Act
            sale.UpdateCustomer(newCustomerId, newCustomerName);

            // Assert
            Assert.Equal(newCustomerId, sale.CustomerId);
            Assert.Equal(newCustomerName, sale.CustomerName);
        }

        [Fact(DisplayName = "Should add items and calculate total amount correctly")]
        public void Given_ValidItems_When_AddItems_Then_ItemsAreAddedAndTotalCalculated()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var items = SaleTestData.GenerateSaleItems(2); // Ex: 2 itens de 10 cada

            // Act
            sale.AddItems(items);

            // Assert
            Assert.Equal(2, sale.Items.Count);
            Assert.Equal(items.Sum(i => i.Total), sale.TotalAmount);
        }

        [Fact(DisplayName = "Should replace items and recalculate total amount")]
        public void Given_UpdatedItems_When_UpdateItems_Then_PreviousItemsAreReplaced()
        {
            // Arrange
            var sale = SaleTestData.GenerateValidSale();
            var initialItems = SaleTestData.GenerateSaleItems(2);
            sale.AddItems(initialItems);

            var updatedItems = SaleTestData.GenerateSaleItems(1, unitPrice: 50);

            // Act
            sale.UpdateItems(updatedItems);

            // Assert
            Assert.Single(sale.Items);
            Assert.Equal(50, sale.TotalAmount);
        }
    }
}

