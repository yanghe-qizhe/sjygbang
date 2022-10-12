using System.Web.Mvc;

namespace EOS.WebApp.Areas.ZJPermission
{
    public class ZJPermissionAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ZJPermission";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ZJPermission_default",
                "ZJPermission/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
