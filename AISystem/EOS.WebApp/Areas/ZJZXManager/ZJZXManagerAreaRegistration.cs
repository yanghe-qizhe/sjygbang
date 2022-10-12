using System.Web.Mvc;

namespace EOS.WebApp.Areas.ZJZXManager
{
    public class ZJZXManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ZJZXManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ZJZXManager_default",
                "ZJZXManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
