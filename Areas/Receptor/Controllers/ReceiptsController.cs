using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Context;
using DataLayer.Models;

namespace Restaurant.Areas.Receptor.Controllers
{
    [Authorize(Roles ="receptor")]
    public class ReceiptsController : Controller
    {
        private RestaurantContext db = new RestaurantContext();

        public ActionResult ConfirmReceipt(int id)
        {
            var receipt = db.Receipts.Find(id);
            receipt.Confirm = true;
            db.Entry(receipt).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("index");
        }
        // GET: Cashier/Receipts
        public ActionResult Index()
        {
            return View(db.Receipts.Where(r=>!r.Confirm).ToList());
        }

        // GET: Cashier/Receipts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Receipt receipt = db.Receipts.Find(id);
            if (receipt == null)
            {
                return HttpNotFound();
            }
            return View(receipt);
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
