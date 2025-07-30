
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class SaleItem : BaseEntity
{
    /// <summary>
    /// Gets the unique identifier of the sale associated with this item.
    /// Used to link the item to a specific sale record.
    /// </summary>
    public Guid SaleId { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the product.
    /// Used to reference the item in the product catalog.
    /// </summary>
    public Guid ProductId { get; private set; }

    /// <summary>
    /// Gets the name of the product.
    /// Should be a descriptive name to identify the product.
    /// </summary>
    public string ProductName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the quantity of the product being sold.
    /// Must be greater than zero.
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// Gets the unit price of the product.
    /// Must be a positive monetary value.
    /// </summary>
    public decimal UnitPrice { get; private set; }

    /// <summary>
    /// Gets the subtotal amount before discounts.
    /// Calculated as Quantity multiplied by UnitPrice.
    /// </summary>
    public decimal SubTotal { get; private set; }

    /// <summary>
    /// Gets the discount applied to the item.
    /// Must be zero or a positive value less than or equal to SubTotal.
    /// </summary>
    public decimal Discount { get; private set; }

    /// <summary>
    /// Gets the total amount after applying the discount.
    /// Calculated as SubTotal minus Discount.
    /// </summary>
    public decimal Total { get; private set; }

    protected SaleItem() { }

    public SaleItem(Guid productId, string productName, int quantity, decimal unitPrice)
    {
        if (quantity < 1)
            throw new ArgumentException("A quantidade deve ser no mínimo 1.");

        if (quantity > 20)
            throw new ArgumentException("Não é possível vender mais de 20 itens idênticos.");

        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        SubTotal = (Quantity * UnitPrice);
        Discount = CalculateDiscount(quantity, unitPrice);
        Total = (Quantity * UnitPrice) - Discount;
    }

    private decimal CalculateDiscount(int quantity, decimal unitPrice)
    {
        if (quantity >= 10 && quantity <= 20)
            return quantity * unitPrice * 0.20m;
        if (quantity >= 4)
            return quantity * unitPrice * 0.10m;
        return 0;
    }
}

