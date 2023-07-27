using DataLayer.Context;
using DataLayer.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Restaurant.Controllers
{
    public class HomeController : Controller
    {


        private RestaurantContext db = new RestaurantContext();


        [HttpPost]
        public ActionResult finalPrice(string offerCode,string currentPrice)
        {
            int price;
            int.TryParse(currentPrice, out price);

            foreach(var of in db.Offers.Where(o => !o.IsOptional).ToList())
            {
                price = (price / 100) * of.OfferPercent + of.OfferDiscount;
            }
            var offer = db.Offers.SingleOrDefault(o => o.OfferCode == offerCode);
            if(offer == null) return Json(new {finalPrice = price});
            int final = (price / 100) * offer.OfferPercent + offer.OfferDiscount;
            return Json(new { finalPrice = final });
            
        }
        public ActionResult ShowFood(int id)
        {
            var food = db.Foods.Find(id);
            return PartialView(food);
        }
        public ActionResult MiniOrder()
        {
            var miniOrder = new MiniOrder();
            return View(miniOrder);
        }
        public ActionResult Menu()
        {
            var foodCategories = db.FoodCategories.Where(fc => fc.Foods.Any(f => f.Amount != 0 && f.IsActive));
            return View(foodCategories);
        }
        [HttpPost]
        public ActionResult Menu(string PhoneNumber, string Address, List<int> FoodIDs, List<int> Amounts,string offerCode)
        {
            int totalPrice = 0;
            for (int i = 0; i < FoodIDs.Count; i++)
            {
                var food = db.Foods.Find(FoodIDs[i]);
                var amount = Amounts[i];
                totalPrice += amount * food.Price;
                if (food.Amount < amount)
                {
                    ModelState.AddModelError(food.FoodID.ToString(), "not enough food");
                    var foodCategories = db.FoodCategories.Where(fc => fc.Foods.Any(f => f.Amount != 0 && f.IsActive));
                    return View(foodCategories);
                }
                food.Amount-=amount;
                db.Entry(food).State=System.Data.Entity.EntityState.Modified;
            }
            bool birunbar = true;
            if (Address.IsNullOrWhiteSpace())
            {
                birunbar = false;
            }

            foreach (var of in db.Offers.Where(o => !o.IsOptional).ToList())
            {
                totalPrice = (totalPrice / 100) * of.OfferPercent + of.OfferDiscount;
            }
            var offer = db.Offers.SingleOrDefault(o => o.OfferCode == offerCode);
            if (offer != null)
                totalPrice = (totalPrice / 100) * offer.OfferPercent + offer.OfferDiscount;


            var receipt = new Receipt()
            {
                Address = Address,
                CustomerNumber = PhoneNumber,
                ReceiptDate = DateTime.Now,
                TotalPrice = totalPrice,
                BirunBar = birunbar,
                Confirm = false 
            };

            db.Receipts.Add(receipt);

            for (int i = 0; i < FoodIDs.Count; i++)
            {
                var food = db.Foods.Find(FoodIDs[i]);
                var amount = Amounts[i];
                var miniOrder = new MiniOrder()
                {
                    Food = food,
                    quantity = amount,
                    Receipt = receipt,
                };
                db.MiniOrders.Add(miniOrder);
            }

            db.SaveChanges();


            return Redirect("/");
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}