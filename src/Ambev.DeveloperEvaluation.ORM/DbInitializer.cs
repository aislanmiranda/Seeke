using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.ORM;

public static class DbInitializer
{
    public static async Task SeedAsync(DefaultContext context)
    {
        if (!await context.Customers.AnyAsync())
        {
            var customer = new Customer(Guid.NewGuid(), "Cliente1");
            context.Customers.Add(customer);
            await context.SaveChangesAsync();
        }

        if (!await context.Products.AnyAsync())
        {
            for (int i = 1; i <= 2; i++)
            {
                var product = new Product(Guid.NewGuid(), $"Product{i}");
                context.Products.Add(product);
                await context.SaveChangesAsync();
            }
        }
    }
}