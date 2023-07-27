using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class MiniOrder
    {
        [Key]
        public int OrderID { get; set; }
        public int FoodID { get; set; }
        public int quantity { get; set; }
        public int ReceiptID { get; set; }
        public virtual Food Food { get; set; }
        public virtual Receipt Receipt { get; set; }
        public MiniOrder()
        {

        }
    }
}
