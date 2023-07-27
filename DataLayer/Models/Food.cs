using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Food
    {
        public int FoodID { get; set; }
        public int? FoodCategoryID { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public int Amount { get; set; }
        public int Price { get; set; }
        public virtual List<Price> Prices { get; set; }
        public virtual List<MiniOrder> MiniOrders { get; set; }
        public virtual FoodCategory Category { get; set; }
        public Food()
        {

        }
    }
}
