using System.Web.Mvc;

namespace EOS.WebApp.Areas.ZJEnergyManager
{
    public class ZJEnergyManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ZJEnergyManager";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ZJEnergyManager_default",
                "ZJEnergyManager/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
