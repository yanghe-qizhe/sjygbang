using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    public class LicenseRecognitionController : PublicController<WB_CARSEXAUDIT>
    {

        WB_CARSEXAUDITBll orderbll = new WB_CARSEXAUDITBll();
        #region 页面返回
        public ActionResult LicenseRecognitionList()
        {
            return View();
        }
        #endregion


        #region 数据查询列表
        /// <summary>
        /// 查询车牌识别审批列表
        /// </summary>
        /// <param name="CarsName"></param>
        /// <param name="OrderBillNo"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetList(string CarsName, string OrderBillNo, string STATE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<WB_CARSEXAUDIT> ListData = orderbll.GetList(CarsName, OrderBillNo, STATE, StartTime, EndTime, jqgridparam);
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


        #region 数据操作
        /// <summary>
        /// 车牌识别审批
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="MEMO"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            WB_CARSEXAUDIT entity = orderbll.GetEntity(KeyValue);
            if (entity.STATE == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已审批！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                database.ExecuteBySql(new StringBuilder("UPDATE WB_CARSEXAUDIT SET STATE=1,AUDITUSER='" + ManageProvider.Provider.Current().UserName + "',AUDITDATE='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "' WHERE ID='" + KeyValue + "' "), isOpenTrans);
                database.Commit();
                string message = string.Format("车牌识别审批：车号{0}审批成功。", entity.CARSNAME);
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Other, "1", message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion


    }
}
