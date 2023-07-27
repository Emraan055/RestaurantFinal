using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class FoodCategory
    {
        public int FoodCategoryID { get; set; }
        public string Name { get; set; }
        public virtual List<Food> Foods { get; set; }
        public FoodCategory()
        {

        }
        
    }
}
