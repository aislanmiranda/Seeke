using Ambev.DeveloperEvaluation.Domain.Entities;
using Bogus;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities.TestData;

public static class SaleTestData
{
    private static readonly Faker<SaleItem> SaleItemFaker = new Faker<SaleItem>()
        .CustomInstantiator(f => new SaleItem(
            productId: Guid.NewGuid(),
            productName: f.Commerce.ProductName(),
            quantity: f.Random.Int(1, 20),
            unitPrice: f.Finance.Amount(10, 200)
        ));

    private static readonly Faker<Sale> SaleFaker = new Faker<Sale>()
        .CustomInstantiator(f =>
        {
            var customerId = Guid.NewGuid();
            var branchId = Guid.NewGuid();
            var items = SaleItemFaker.Generate(f.Random.Int(1, 5));

            var sale = new Sale(                
                customerId: customerId,
                customerName: f.Name.FullName(),
                branchId: branchId,
                branchName: f.Company.CompanyName()
            );

            sale.AddItems(items);

            return sale;
        });

    public static Sale GenerateValidSale()
    {
        return SaleFaker.Generate();
    }

    //public static List<SaleItem> GenerateSaleItems(int count)
    //{
    //    return SaleItemFaker.Generate(count);
    //}

    public static List<SaleItem> GenerateSaleItems(int quantity, decimal? unitPrice = null)
    {
        return Enumerable.Range(1, quantity).Select(_ =>
        {
            var item = SaleItemFaker.Generate();
            return unitPrice.HasValue
                ? new SaleItem(item.ProductId, item.ProductName, quantity, unitPrice.Value)
                : item;
        }).ToList();
    }
}
