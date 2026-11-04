using System.Web.Mvc;

namespace _25dh110017_Mystore.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "_25dh110017_Mystore.Areas.Admin.Controllers" } // Định hướng chính xác Controller của Admin
            );
        }
    }
}