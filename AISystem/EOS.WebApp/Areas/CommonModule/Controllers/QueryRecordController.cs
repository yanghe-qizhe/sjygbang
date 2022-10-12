using EOS.Business;
using EOS.Entity;
using EOS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EOS.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// 查询条件记录控制器
    /// </summary>
    public class QueryRecordController : PublicController<Base_QueryRecord>
    {

        public ActionResult Form()
        {
            return View();
        }
    }
}