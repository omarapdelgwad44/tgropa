namespace tgropa.Data;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double InitialPrice { get; set; }
    public double RetailPrice { get; set; } // I assume you meant 'pricePrice' as retail price
    public double WholesalePrice { get; set; }
    public int Quantity { get; set; }

    // Navigation property for the many-to-many relationship
    public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}