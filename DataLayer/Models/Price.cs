using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Price
    {
        public int PriceID { get; set; }
        public int FoodID { get; set; }
        public int PriceValue { get; set; }
        public DateTime SetPriceDate { get; set; }
        public virtual Food Food { get; set; }
        public Price()
        {

        }
    }
}
