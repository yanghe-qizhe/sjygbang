using System.Web.Mvc;

namespace EOS.WebApp.Areas.SelectValue
{
    public class SelectValueAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SelectValue";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SelectValue_default",
                "SelectValue/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
