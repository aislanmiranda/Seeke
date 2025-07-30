
namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string? ProductName { get; private set; }

    public Product(Guid id, string? productName)
    {
        Id = id;
        ProductName = productName;
    }
}