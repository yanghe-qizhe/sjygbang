using System.Web.Mvc;

namespace EOS.WebApp.Areas.ZJSettlement
{
    public class ZJSettlementAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ZJSettlement";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "ZJSettlement_default",
                "ZJSettlement/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
