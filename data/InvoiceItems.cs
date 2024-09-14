using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tgropa.Data
{
    public class InvoiceItem
    {
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }

        public int ItemId { get; set; }
        public Item Item { get; set; }

        public int Quantity { get; set; }  // Quantity of this item in this invoice
    }
}
