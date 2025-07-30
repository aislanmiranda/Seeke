
using Ambev.DeveloperEvaluation.Domain.Common;

namespace Ambev.DeveloperEvaluation.Domain.Entities;

public class Sale : BaseEntity
{
    /// <summary>
    /// Gets the unique identifier of the sale in human-readable format.
    /// Typically used for display or reference purposes (e.g., invoice number).
    /// </summary>
    public string SaleNumber { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the date and time when the sale was created.
    /// Automatically set at the time of sale creation.
    /// </summary>
    public DateTime SaleDate { get; private set; }

    /// <summary>
    /// Gets the unique identifier of the customer associated with the sale.
    /// Used to link the sale to a specific customer.
    /// </summary>
    public Guid CustomerId { get; private set; }

    /// <summary>
    /// Gets the name of the customer associated with the sale.
    /// Should reflect the full name of the customer at the time of sale.
    /// </summary>
    public string CustomerName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the unique identifier of the branch where the sale was made.
    /// Used for associating the sale with a physical or virtual store location.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    /// Gets the name of the branch where the sale was made.
    /// Should be descriptive enough to identify the branch location.
    /// </summary>
    public string BranchName { get; private set; } = string.Empty;

    /// <summary>
    /// Gets the list of items included in the sale.
    /// Each item represents a product, its quantity, pricing, and totals.
    /// </summary>
    public List<SaleItem>? Items { get; private set; }

    /// <summary>
    /// Gets the total monetary amount of the sale.
    /// Calculated as the sum of all item totals (after discounts).
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    /// Gets a value indicating whether the sale was canceled.
    /// True if the sale has been voided or reversed.
    /// </summary>
    public bool IsCanceled { get; private set; }

    // Construtor vazio para o EF Core
    protected Sale() {}

    public Sale(Guid customerId, string customerName, Guid branchId, string branchName)
    {
        SaleNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
        SaleDate = DateTime.UtcNow;
        CustomerId = customerId;
        CustomerName = customerName;
        BranchId = branchId;
        BranchName = branchName;
        TotalAmount = 0;
        IsCanceled = false;
    }

    public void UpdateCustomer(Guid customerId, string customerName)
    {
        CustomerId = customerId;
        CustomerName = customerName;
    }

    public void AddItems(List<SaleItem> items)
    {
        Items = items;
        TotalAmount = Items.Sum(i => i.Total);
    }

    public void UpdateItems(List<SaleItem> updatedItems)
    {
        Items = updatedItems;
        TotalAmount = Items.Sum(item => item.Total);
    }

    public void CancelSale()
    {
        IsCanceled = true;
    }
}