using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Models
{
    public class Offer
    {
        public int OfferID { get; set; }
        public string OfferTitle { get; set; }
        public string OfferCode { get; set; }
        public int OfferPercent { get; set; }
        public int OfferDiscount { get; set; }
        public bool IsOptional { get; set; }
        public virtual List<Receipt> Receipts { get; set; }

    }
}
