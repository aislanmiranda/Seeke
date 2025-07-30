using Bogus;
using Ambev.DeveloperEvaluation.Application.Sales.CancelSale;

namespace Ambev.DeveloperEvaluation.Unit.Domain;

public static class CancelSaleHandlerTestData
{
    
    private static readonly Faker<CancelSaleCommand> CancelSaleHandlerFaker =
        new Faker<CancelSaleCommand>()
            .RuleFor(x => x.SaleNumber, f => f.Random.Guid().ToString().Substring(0,8).ToUpper())
            .RuleFor(x => x.CustomerId, f => f.Random.Guid());

    public static CancelSaleCommand GenerateValidCommand()
    {
        return CancelSaleHandlerFaker.Generate();
    }

}
