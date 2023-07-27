using System.Web.Mvc;

namespace Restaurant.Areas.Cashier
{
    public class CashierAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Cashier";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Cashier_default",
                "Cashier/{controller}/{action}/{id}",
                new {controller="Reports", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}