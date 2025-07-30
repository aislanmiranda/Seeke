using Bogus;
using Ambev.DeveloperEvaluation.Application.Sales.UpdateSale;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class UpdateSaleHandlerTestData
{
    private static readonly Faker<UpdateSaleCommand.UpdateSaleItemCommand> SaleItemCommandFaker =
    new Faker<UpdateSaleCommand.UpdateSaleItemCommand>()
        .RuleFor(x => x.ProductId, f => f.Random.Guid())
        .RuleFor(x => x.Quantity, f => f.Random.Int(1, 20))
        .RuleFor(x => x.UnitPrice, f => f.Finance.Amount(10, 200));

    private static readonly Faker<UpdateSaleCommand> UpdateSaleHandlerFaker =
        new Faker<UpdateSaleCommand>()
            .RuleFor(x => x.SaleNumber, f => f.Random.Guid().ToString().Substring(0,8).ToUpper())
            .RuleFor(x => x.CustomerId, f => f.Random.Guid())
            .RuleFor(x => x.Items, f => SaleItemCommandFaker.Generate(f.Random.Int(1, 5)));

    public static UpdateSaleCommand GenerateValidCommand()
    {
        return UpdateSaleHandlerFaker.Generate();
    }

}
