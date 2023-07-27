using DataLayer.Context;
using System.IO;

using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;


using DataLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Data;
using System.Drawing;
using Syncfusion.Pdf.Grid;
using static iTextSharp.text.pdf.AcroFields;

namespace Restaurant.Areas.Cashier.Controllers
{
    [Authorize(Roles = "Cashier")]
    public class ReportsController : Controller
    {
        RestaurantContext db = new RestaurantContext();

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult HtmlToPDF(string htmlString)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                iText.Kernel.Pdf.PdfWriter writer = new iText.Kernel.Pdf.PdfWriter(memoryStream);
                iText.Kernel.Pdf.PdfDocument pdfDoc = new iText.Kernel.Pdf.PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdfDoc, iText.Kernel.Geom.PageSize.A4, true);

                iText.Html2pdf.ConverterProperties converterProperties = new iText.Html2pdf.ConverterProperties();

                iText.Html2pdf.HtmlConverter.ConvertToPdf(htmlString, pdfDoc, converterProperties);

                document.Close();

                byte[] bytes = memoryStream.ToArray();
                memoryStream.Close();

                return File(bytes, "application/pdf", "converted.pdf");
            }

        }
        [HttpGet]
        [ValidateInput(false)]
        public ActionResult GetPdfReceipts()
        {
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Receipt Date");
            dataTable.Columns.Add("Address");
            dataTable.Columns.Add("Customer Number");
            dataTable.Columns.Add("Birun Bar");
            dataTable.Columns.Add("Confirm");
            dataTable.Columns.Add("Total Price");
            dataTable.Columns.Add("Updated Price");
            var receipts = db.Receipts.ToList();
            int sum1 = 0;
            int sum2 = 0;
            foreach (var item in receipts)
            {
                int sum = 0;
                foreach (var sub in item.MiniOrders.ToList())
                {
                    sum += sub.quantity * sub.Food.Price;
                }
                sum1 += item.TotalPrice;
                sum2 += sum;
                dataTable.Rows.Add(new object[] { item.ReceiptDate,
                    item.Address,
                    item.CustomerNumber,
                    item.BirunBar,
                    item.Confirm,
                    item.TotalPrice,
                    sum
                });
            }
            dataTable.Rows.Add(new object[] { 
                    "Total",
                    "",
                    "",
                    "",
                    "",
                    sum1,
                    sum2
                });

            pdfGrid.DataSource = dataTable;
            PdfGridBuiltinStyleSettings settings = new PdfGridBuiltinStyleSettings();
            settings.ApplyStyleForHeaderRow = true;
            settings.ApplyStyleForBandedRows = true;
            settings.ApplyStyleForLastRow = true;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent1,settings);
            pdfGrid.Draw(page, new PointF(10, 10));
            doc.Save("Output.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
            doc.Close(true);
            return View();
        }

        public ActionResult GetPdfFoods()
        {

            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Food Name");
            dataTable.Columns.Add("Total Orders");
            dataTable.Columns.Add("total amount");
            dataTable.Columns.Add("total cash");
            var foods = db.Foods.ToList();
            int sum1 = 0;
            int sum2 = 0;
            int sum3 = 0;
            foreach (var item in foods)
            {
                sum1 += item.MiniOrders.Count;
                sum2 += item.MiniOrders.Sum(mo => mo.quantity);
                sum3 += item.MiniOrders.Sum(mo => mo.quantity * mo.Food.Price);
                dataTable.Rows.Add(new object[] { 
                    item.Name,
                    item.MiniOrders.Count,
                    item.MiniOrders.Sum(mo => mo.quantity),
                    item.MiniOrders.Sum(mo => mo.quantity * mo.Food.Price)
                });
            }
            dataTable.Rows.Add(new object[] {
                    "Total",
                    sum1,
                    sum2,
                    sum3
                });

            pdfGrid.DataSource = dataTable;
            PdfGridBuiltinStyleSettings settings = new PdfGridBuiltinStyleSettings();
            settings.ApplyStyleForHeaderRow = true;
            settings.ApplyStyleForBandedRows = true;
            settings.ApplyStyleForLastRow = true;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent2, settings);
            pdfGrid.Draw(page, new PointF(10, 10));
            doc.Save("Output.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
            doc.Close(true);
            return View();

        }

        public ActionResult GetPdfCategories()
        {
            PdfDocument doc = new PdfDocument();
            PdfPage page = doc.Pages.Add();
            PdfGrid pdfGrid = new PdfGrid();
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Category Name");
            dataTable.Columns.Add("Total Orders");
            dataTable.Columns.Add("total amount");
            dataTable.Columns.Add("total cash");
            var categories = db.FoodCategories.ToList();
            int sum1 = 0;
            int sum2 = 0;
            int sum3 = 0;
            foreach (var item in categories)
            {
                sum1 += item.Foods.SelectMany(f => f.MiniOrders).Distinct().Count();
                sum2 += item.Foods.SelectMany(f => f.MiniOrders).Distinct().Sum(mo => mo.quantity);
                sum3 += item.Foods.SelectMany(f => f.MiniOrders).Distinct().Sum(mo => mo.quantity * mo.Food.Price);
                dataTable.Rows.Add(new object[] {
                    item.Name,
                    item.Foods.SelectMany(f => f.MiniOrders).Distinct().Count(),
                    item.Foods.SelectMany(f => f.MiniOrders).Distinct().Sum(mo => mo.quantity),
                    item.Foods.SelectMany(f => f.MiniOrders).Distinct().Sum(mo => mo.quantity * mo.Food.Price)
                });
            }
            dataTable.Rows.Add(new object[] {
                    "Total",
                    sum1,
                    sum2,
                    sum3
                });

            pdfGrid.DataSource = dataTable;
            PdfGridBuiltinStyleSettings settings = new PdfGridBuiltinStyleSettings();
            settings.ApplyStyleForHeaderRow = true;
            settings.ApplyStyleForBandedRows = true;
            settings.ApplyStyleForLastRow = true;
            pdfGrid.ApplyBuiltinStyle(PdfGridBuiltinStyle.GridTable4Accent3, settings);
            pdfGrid.Draw(page, new PointF(10, 10));
            doc.Save("Output.pdf", HttpContext.ApplicationInstance.Response, HttpReadType.Save);
            doc.Close(true);
            return View();
        }




        public ActionResult Foods()
        {
            var foods = db.Foods.ToList();
            return View(foods);
        }

        public ActionResult Categories()
        {
            var Categories = db.FoodCategories.ToList();
            return View(Categories);
        }

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
        public ActionResult All()
        {


            var receipts = db.Receipts.ToList();
            return View("ShowReceipts", receipts);
        }

        // GET: Cashier/Reports
        public ActionResult Index()
        {
            return View();
        }
    }
}