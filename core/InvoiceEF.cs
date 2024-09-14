
using Microsoft.EntityFrameworkCore;
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

                            // Add the item to the invoice (via the junction table InvoiceItem)
                            var invoiceItem = new InvoiceItem
                            {
                                ItemId = item.Id,
                                InvoiceId = invoice.Id,
                                Quantity = quantityToReduce
                            };

                            // Add the new InvoiceItem to the invoice
                            invoice.InvoiceItems.Add(invoiceItem);

                            // Save changes after updating item quantity
                            db.Items.Update(item);

                        }
                        else
                        {
                            // Notify the user about the stock limitation
                            Console.WriteLine($"Insufficient stock for {item.Name}. Available: {item.Quantity}, Requested: {quantityToReduce}");
                            return 0;  // Failure due to insufficient stock
                        }
                    }
                    else
                    {
                        throw new Exception($"Item with ID {itemId} not found.");
                    }
                }

                // Add the invoice with its items to the database
                db.Invoices.Add(invoice);
                db.SaveChanges();

                // Commit the transaction
                transaction.Commit();

                return 1;  // Success
            }
            catch (Exception ex)
            {
                transaction.Rollback(); // Rollback the transaction if there is an error
                Console.WriteLine($"Error while adding invoice: {ex.Message}");
                return 0;  // Failure
            }
        }






        public int Delete(int Id)
        {
            try
            {
                var invoice = Find(Id);
                if (invoice != null)
                {
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
                return db.Invoices.FirstOrDefault(x => x.Id == Id) ?? new Invoice();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new Invoice();
            }
        }

        public List<Invoice> GetAllData()
        {
            throw new NotImplementedException();
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
    }
}
