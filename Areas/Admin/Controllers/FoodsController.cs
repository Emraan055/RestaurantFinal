using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Context;
using DataLayer.Models;

namespace Restaurant.Areas.Admin.Controllers
{
    [Authorize(Roles ="Admin")]
    public class FoodsController : Controller
    {
        private RestaurantContext db = new RestaurantContext();

        // GET: Admin/Foods
        public ActionResult Index()
        {
            return Redirect("/Admin/FoodCategories");
        }

        public ActionResult FoodsListByCategory(int id)
        {
            var foods = db.FoodCategories.Find(id).Foods;
            return View("index", foods.ToList());
        }

        // GET: Admin/Foods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // GET: Admin/Foods/Create
        public ActionResult Create()
        {
            ViewBag.FoodCategoryID = new SelectList(db.FoodCategories, "FoodCategoryID", "Name");
            return View();
        }

        // POST: Admin/Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "FoodID,FoodCategoryID,Name,Image,IsActive,Amount,Price")] Food food, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {

                if (imgUp != null)
                {
                    food.Image = Guid.NewGuid() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/FoodImages/" + food.Image));
                }

                db.Foods.Add(food);
                db.SaveChanges();
                return RedirectToAction("Index", "FoodCategories");
            }

            ViewBag.FoodCategoryID = new SelectList(db.FoodCategories, "FoodCategoryID", "Name", food.FoodCategoryID);
            return View(food);
        }

        // GET: Admin/Foods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            ViewBag.FoodCategoryID = new SelectList(db.FoodCategories, "FoodCategoryID", "Name", food.FoodCategoryID);
            return View(food);
        }

        // POST: Admin/Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FoodID,FoodCategoryID,Name,Image,IsActive,Amount,Price")] Food food, HttpPostedFileBase imgUp)
        {
            if (ModelState.IsValid)
            {
                if (imgUp != null)
                {
                    if (food.Image != null)
                    {
                        System.IO.File.Delete(Server.MapPath("/FoodImages/" + food.Image));
                    }

                    food.Image = Guid.NewGuid() + Path.GetExtension(imgUp.FileName);
                    imgUp.SaveAs(Server.MapPath("/FoodImages/" + food.Image));
                }
                var prevFood = db.Foods.AsNoTracking().FirstOrDefault(f => f.FoodID == food.FoodID);
                if (prevFood.Price != food.Price)
                {
                    var price = new Price();
                    price.SetPriceDate = DateTime.Now;
                    price.PriceValue = prevFood.Price;
                    price.FoodID = food.FoodID;
                    db.Prices.Add(price);
                }
                
                db.Entry(food).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "FoodCategories");
            }
            ViewBag.FoodCategoryID = new SelectList(db.FoodCategories, "FoodCategoryID", "Name", food.FoodCategoryID);
            return View(food);
        }

        // GET: Admin/Foods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Food food = db.Foods.Find(id);
            if (food == null)
            {
                return HttpNotFound();
            }
            return View(food);
        }

        // POST: Admin/Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Food food = db.Foods.Find(id);
            System.IO.File.Delete(Server.MapPath("/FoodImages/" + food.Image));
            db.Foods.Remove(food);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
