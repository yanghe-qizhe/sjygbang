using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using EOS.Business;
using System.Diagnostics;
using EOS.Utilities;

namespace EOS.WebApp.Areas.ZJZXManager.Controllers
{
    public class ZJZXCYBatchFindController : PublicController<VQC_ZJZXCYFIND>
    {
        VQC_ZJZXCYFINDBll vpcbll = new VQC_ZJZXCYFINDBll();
        #region 页面返回
        public ActionResult ZJZXCYBatchFindList()
        {
            return View();
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// 获取采样组批列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJZXCYFINDList(string BILLNO, string PRINTCODE, string StartTime, string EndTime, string MATERIALNAME, string CARNAME, string PCRAWTYPE, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCYFIND> ListData = vpcbll.GetOrderList(BILLNO, PRINTCODE, StartTime, EndTime, MATERIALNAME, CARNAME, PCRAWTYPE, jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        #endregion

    }
}
