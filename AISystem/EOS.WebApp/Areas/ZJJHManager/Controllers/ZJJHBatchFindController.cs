using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Business;
using EOS.Entity;
using System.Diagnostics;
using EOS.Utilities;

namespace EOS.WebApp.Areas.ZJJHManager.Controllers
{
    public class ZJJHBatchFindController : PublicController<VQC_ZJJHFIND>
    {
        VQC_ZJJHFINDBll vpcbll = new VQC_ZJJHFINDBll();
        #region 页面返回
        public ActionResult ZJJHBatchFindList()
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
        public ActionResult GetZJJHCYFINDList(string BILLNO, string PRINTCODE, string StartTime, string EndTime, string MATERIALNAME, string CARNAME, string PCRAWTYPE, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJJHFIND> ListData = vpcbll.GetOrderList(BILLNO, PRINTCODE, StartTime, EndTime, MATERIALNAME, CARNAME, PCRAWTYPE, jqgridparam);
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
