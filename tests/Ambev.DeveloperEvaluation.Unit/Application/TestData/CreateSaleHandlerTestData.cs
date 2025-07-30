using Bogus;
using Ambev.DeveloperEvaluation.Application.Sales.CreateSale;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class CreateSaleHandlerTestData
{
    private static readonly Faker<CreateSaleCommand.SaleItemCommand> SaleItemCommandFaker =
    new Faker<CreateSaleCommand.SaleItemCommand>()
        .RuleFor(x => x.ProductId, f => f.Random.Guid())
        .RuleFor(x => x.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(x => x.UnitPrice, f => f.Finance.Amount(10, 200));

    private static readonly Faker<CreateSaleCommand> createSaleHandlerFaker =
        new Faker<CreateSaleCommand>()
            .RuleFor(x => x.CustomerId, f => f.Random.Guid())
            .RuleFor(x => x.Items, f => SaleItemCommandFaker.Generate(f.Random.Int(1, 5)));

    public static CreateSaleCommand GenerateValidCommand()
    {
        return createSaleHandlerFaker.Generate();
    }

}
