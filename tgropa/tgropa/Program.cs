
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
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
            IDataHelper<Item> dataHelper = new ItemEF();
            // Adding a new item

            // dataHelper.Delete(55);
            // dataHelper.Delete(44);
            //  dataHelper.Delete(77);
            Console.WriteLine(dataHelper.Add(new Item { Id = 1, Name = "chips", InitialPrice = 10, RetailPrice = 17, WholesalePrice = 15, Quantity = 50 }));
            Console.WriteLine(dataHelper.Add(new Item { Id = 2, Name = "tiger", InitialPrice = 10, RetailPrice = 17, WholesalePrice = 15, Quantity = 50 }));
            Console.WriteLine(dataHelper.Add(new Item { Id = 3, Name = "row", InitialPrice = 50, RetailPrice = 60, WholesalePrice = 55, Quantity = 25 }));
            var items = dataHelper.GetAllData();

            foreach (var item in items)
            {
                Console.WriteLine($"Item ID: {item.Id}, Name: {item.Name},intialPrice:  {item.InitialPrice}, picePrice: {item.RetailPrice},wholesalePrice: {item.WholesalePrice},Quantity: {item.Quantity},{item}");
            }

            var newInvoice = new Invoice { Description = "Customer Purchase" };

            // List of item IDs to add to the invoice
            List<int> itemIds = new List<int> { 1, 2, 3 };  // Example item IDs

            // Corresponding quantities of the items (ensure they don't exceed available stock)
            List<int> itemQuantities = new List<int> { 1, 2, 1 };  // Quantities of items to reduce

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

            dataHelper.GetAllData().ToArray();

            foreach (var item in dataHelper.GetAllData())
            {
                Console.WriteLine($"Item ID: {item.Id}, Name: {item.Name},intialPrice:  {item.InitialPrice}, picePrice: {item.RetailPrice},wholesalePrice: {item.WholesalePrice},Quantity: {item.Quantity},{item}");
            }
        }

    }
}