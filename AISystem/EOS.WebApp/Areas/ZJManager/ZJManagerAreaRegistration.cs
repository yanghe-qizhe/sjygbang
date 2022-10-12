﻿using System.Web.Mvc;

namespace EOS.WebApp.Areas.ZJManager
{
    public class ZJManagerAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "ZJManager";
            }
        }

      
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
