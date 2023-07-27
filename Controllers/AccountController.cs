using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DataLayer.Context;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Restaurant.Models;

namespace Restaurant.Controllers
{
    public class AccountController : Controller
    {
        RestaurantContext db = new RestaurantContext();
        //
        // GET: /Account/Login


        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
        public ActionResult Login(string returnUrl)
        {

            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("LogOff");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }




        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl = "/")
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            string hashpass = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Password, "MD5").ToString();
            var user = db.Users.SingleOrDefault(u => u.UserName == model.Name && u.Password == hashpass);
            if (user == null)
            {
                ModelState.AddModelError("Name", "user not found");
                return View(model);
            }
            FormsAuthentication.SetAuthCookie(user.UserName, model.RememberMe);


            if (returnUrl.Contains(user.Role.RoleName))
            {
                return Redirect(returnUrl);
            }
            return Redirect($"/{user.Role.RoleName}/");
        }

    }
}