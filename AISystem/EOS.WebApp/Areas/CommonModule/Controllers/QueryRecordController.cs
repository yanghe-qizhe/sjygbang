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
    /// ��ѯ������¼������
    /// </summary>
    public class QueryRecordController : PublicController<Base_QueryRecord>
    {

        public ActionResult Form()
        {
            return View();
        }
    }
}