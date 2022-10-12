using System.Web.Mvc;

namespace EOS.WebApp.Areas.ZJGCManager
{
    public class ZJGCManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ZJGCManager";
            }
        }

        //public override void RegisterArea(AreaRegistrationContext context)
        //{
        //    context.MapRoute(
        //        "ZJGCManager_default",
        //        "ZJGCManager/{controller}/{action}/{id}",
        //        new { action = "Index", id = UrlParameter.Optional }
        //    );
        //}
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                this.AreaName + "_Default",
                this.AreaName + "/{controller}/{action}/{id}",
                new { area = this.AreaName, controller = "Home", action = "Index", id = UrlParameter.Optional },
                new string[] { "EOS.WebApp.Areas." + this.AreaName + ".Controllers" }
            );
        }
    }
}
