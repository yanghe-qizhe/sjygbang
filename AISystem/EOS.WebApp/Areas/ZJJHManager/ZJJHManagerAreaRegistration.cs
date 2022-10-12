using System.Web.Mvc;

namespace EOS.WebApp.Areas.ZJJHManager
{
    public class ZJJHManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ZJJHManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ZJJHManager_default",
                "ZJJHManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
