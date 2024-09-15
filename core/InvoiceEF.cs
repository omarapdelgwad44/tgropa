
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using tgropa.Data;

namespace tgropa.Core
{
    public class InvoiceEF : IDataHelper<Invoice>
    {
        private readonly DataBaseContext db;
        public InvoiceEF()
        {
            db = new DataBaseContext();
        }
        public int Add(Invoice invoice)
        {
            try
            {
                db.Invoices.Add(invoice);

                // Save the changes, including many-to-many relationships
                db.SaveChanges();

                return 1;  // Success
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding invoice: {ex.Message}");
                return 0;  // Failure
            }
        }
        public int AddInvoiceWithItemIds(Invoice invoice, List<int> itemIds, List<int> itemQuantities)
        {
            using var transaction = db.Database.BeginTransaction();
            try
            {
                for (int i = 0; i < itemIds.Count; i++)
                {
                    int itemId = itemIds[i];
                    int quantityToReduce = itemQuantities[i];

                    // Fetch the real item (inventory) from the database by its ID
                    var item = db.Items.FirstOrDefault(i => i.Id == itemId);

                    if (item != null)
                    {
                        // Check if there is enough stock to fulfill the invoice
                        if (item.Quantity >= quantityToReduce)
                        {
                            // Reduce the stock of the item
                            item.Quantity -= quantityToReduce;

                            // Check if the item is already in the invoice (to avoid duplicate entry)
                            var existingInvoiceItem = db.InvoiceItems
                                .FirstOrDefault(ii => ii.InvoiceId == invoice.Id && ii.ItemId == itemId);

                            if (existingInvoiceItem != null)
                            {
                                // If the item already exists in the invoice, just update the quantity
                                existingInvoiceItem.Quantity += quantityToReduce;
                                db.InvoiceItems.Update(existingInvoiceItem);
                            }
                            else
                            {
                                // If the item does not exist, add it to the invoice
                                var invoiceItem = new InvoiceItem
                                {
                                    ItemId = item.Id,
                                    InvoiceId = invoice.Id,
                                    Quantity = quantityToReduce
                                };
                                invoice.InvoiceItems.Add(invoiceItem);
                            }

                            // Update the item's quantity in the database
                            db.Items.Update(item);
                        }
                        else
                        {
                            Console.WriteLine($"Insufficient stock for {item.Name}. Available: {item.Quantity}, Requested: {quantityToReduce}");
                            return 0;  // Failure due to insufficient stock
                        }
                    }
                    else
                    {
                        throw new Exception($"Item with ID {itemId} not found.");
                    }
                }

                // Check if the invoice already exists, update it; otherwise, add a new one
                if (db.Invoices.Find(invoice.Id) != null)
                {
                    db.Invoices.Update(invoice);
                }
                else
                {
                    db.Invoices.Add(invoice);
                }

                db.SaveChanges(); // Save all changes
                transaction.Commit(); // Commit transaction
                return 1;  // Success
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Rollback the transaction if there is an error
                Console.WriteLine($"Error while adding invoice: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                return 0;  // Failure
            }
        }
        public int Delete(int Id)
        {
            try
            {
                var invoice = db.Invoices.Find(Id);  
                if (invoice != null)
                {
                    var res = db.InvoiceItems
      .Where(x => x.InvoiceId == invoice.Id)
      .Select(x => new { x.Item, x.Quantity });

                    foreach (var item in res)
                    {
                        item.Item.Quantity += item.Quantity;
                        db.Items.Update(item.Item);
                        db.InvoiceItems.Remove(db.InvoiceItems.FirstOrDefault(ii => ii.InvoiceId == invoice.Id && ii.ItemId == item.Item.Id));
                        db.SaveChanges();
                       // Console.WriteLine($"{ item.Item.Quantity }    {item.Quantity}");

                    
                   }
                    db.Invoices.Remove(invoice);
                    db.SaveChanges();
                    return 1;
                }
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }
        public Invoice Find(int Id)
        {
            try
            {
                return db.Invoices.Find(Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return null;
            }
        }
        public List<Invoice> GetAllData()
    {
        // Fetch all invoices, including related InvoiceItems and Items
        var invoices = db.Invoices
                         .Include(i => i.InvoiceItems)  // Eager load InvoiceItems
                         .ThenInclude(ii => ii.Item)    // Eager load related Items through InvoiceItems
                         .ToList();                    // Execute the query

            Console.WriteLine("Text written to file successfully!");
            // Return the list of invoices
            return invoices;
    }
        public List<Invoice> Search(string searchItem)
        {
            try
            {
                return db.Invoices.Where(x => x.Id.ToString() == searchItem).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Invoice>();
            }
        }
        public int Update(Invoice entity)
        {
            try
            {
                db.Invoices.Update(entity);
                db.SaveChanges();
                return 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0;
            }
        }
        public void show(List<Invoice> invoices)
        {
            String output = new String("الفواتير");
            try
            {
                // Iterate over the invoices and print details
                foreach (var invoice in invoices)
                {

                    output += ($"\n************** Invoice ID: {invoice.Id}, Description: {invoice.Description} **************\n");
                    output += "                                              العناصر";
                    // Iterate over each item in the invoice
                    foreach (var invoiceItem in invoice.InvoiceItems)
                    {
                        var item = invoiceItem.Item; // Get the related item through the InvoiceItem
                        output += ($"\n                        Item Name: {item.Name}, Quantity: {invoiceItem.Quantity}");
                    }

                }
                string filePath = "فواتير.txt";

                // Write the Arabic text to the file with UTF-8 encoding
                File.WriteAllText(filePath, output, Encoding.UTF8);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

    }
}
