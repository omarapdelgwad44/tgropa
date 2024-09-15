using System.Text;
using tgropa.Core;
using tgropa.Data;

namespace SQLite
{

    public class Program
    {
        static void Main(string[] args)
        {
            InvoiceEF Ie = new InvoiceEF();
            // Creating a new invoice
            ItemEF dataHelper = new ItemEF();
            // Adding a new item

            // dataHelper.Delete(55);
            // dataHelper.Delete(44);
            //  dataHelper.Delete(77);
            
            Console.WriteLine(dataHelper.Add(new Item { Id = 1, Name = "chips", InitialPrice = 10, RetailPrice = 17, WholesalePrice = 15, Quantity = 50 }));
            Console.WriteLine(dataHelper.Add(new Item { Id = 2, Name = "tiger", InitialPrice = 10, RetailPrice = 17, WholesalePrice = 15, Quantity = 50 }));
            Console.WriteLine(dataHelper.Add(new Item { Id = 3, Name = "row", InitialPrice = 50, RetailPrice = 60, WholesalePrice = 55, Quantity = 25 }));
            
          Console.WriteLine($"dfgfds { Ie.Delete(4)}");
          
            var newInvoice = new Invoice { Id=15, Description="D!" };

            // List of item IDs to add to the invoice
            List<int> itemIds = new List<int> { 1, 2};  // Example item IDs

            // Corresponding quantities of the items (ensure they don't exceed available stock)
            List<int> itemQuantities = new List<int> { 10, 10 };  // Quantities of items to reduce

            // Add invoice with items and reduce their stock
            try
            {
                // Add invoice with items and reduce their stock
                int result = Ie.AddInvoiceWithItemIds(newInvoice, itemIds, itemQuantities);

                if (result == 1)
                {
                    Console.WriteLine("Invoice added successfully and stock updated.");
                }
                else
                {
                    Console.WriteLine("Error adding invoice or insufficient stock.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding invoice: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
          
           Ie.show( Ie.GetAllData());
           dataHelper.show(dataHelper.GetAllData());

           /* dataHelper.GetAllData().ToArray();

            foreach (var item in dataHelper.GetAllData())
            {
                Console.WriteLine($"Item ID: {item.Id}, Name: {item.Name},intialPrice:  {item.InitialPrice}, picePrice: {item.RetailPrice},wholesalePrice: {item.WholesalePrice},Quantity: {item.Quantity},{item}");
            }
            
            */
                   }

    }
}