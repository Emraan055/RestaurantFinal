using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Receipt
    {
        public int ReceiptID { get; set; }
        public DateTime ReceiptDate { get; set; }
        public string Address { get; set; }
        public string CustomerNumber { get; set; }
        public bool BirunBar { get; set; }
        public bool Confirm { get; set; }
        public int TotalPrice { get; set; }
        public virtual List<Offer> Offer { get; set; }
        public virtual List<MiniOrder> MiniOrders { get; set; }
        public Receipt()
        {

        }
        
    }
}
