namespace tgropa.Data;

public class Invoice
{
    public int Id { get; set; }
    public string Description { get; set; }

    // Navigation property for the many-to-many relationship
    public ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
}