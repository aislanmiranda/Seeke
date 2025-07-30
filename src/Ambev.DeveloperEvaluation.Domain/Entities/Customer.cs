
namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Customer
{
    public Guid Id { get; private set; }
    public string? CustomerName { get; private set; }

    public Customer(Guid id, string? customerName)
    {
        Id = id;
        CustomerName = customerName;
    }
}