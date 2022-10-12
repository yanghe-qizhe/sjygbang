using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    public class SaleSuttleErrorController : PublicController<VWB_SALESUTTLEERROR>
    {
        string ModuleId = "";

        VWB_SALESUTTLEERRORBll orderbll = new VWB_SALESUTTLEERRORBll();
        VWB_SALEPRINTAUDITBll wb_order = new VWB_SALEPRINTAUDITBll();

        //列表
        [LoginAuthorize]
        public ActionResult SaleSuttleErrorIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SalePrintAuditIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult SaleSuttleErrorForm()
        {
            return View();
        }
          [LoginAuthorize]
        public ActionResult SalePrintAuditForm()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult WeightQuery()
        {
            return View();
        }


        //销售容差审批列表
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string VBillCode, string ContractBillNo, string FHBillNo, string Customer, string Material, string Cars, string Status, string STATE,string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_SALESUTTLEERROR> ListData = orderbll.GetOrderList(BillNo, VBillCode, ContractBillNo, FHBillNo, Customer, Material, Cars, Status,STATE, "8", StartTime, EndTime, jqgridparam);
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

        //销售打印审批列表
        [LoginAuthorize]
        public ActionResult GetPAOrderList(string BillNo, string VBillCode, string ContractBillNo, string FHBillNo, string Customer, string Material, string Cars, string Status, string STATE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
              
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_SALEPRINTAUDIT> ListData = wb_order.GetOrderList(BillNo, VBillCode, ContractBillNo, FHBillNo, Customer, Material, Cars, Status, STATE, "8", StartTime, EndTime, jqgridparam);
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


        //销售容差审批
        public ActionResult CheckOrder(string KeyValue, string MEMO, string ModuleId)
        {
            //验证
            VWB_SALESUTTLEERROR entity = orderbll.GetEntity(KeyValue);
            if (entity.STATE == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已容差审批！" }.ToString());
            }
            WB_SALESUTTLEERROR entityaudit = new WB_SALESUTTLEERROR();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                entityaudit.Create();
                entityaudit.BILLNO = entity.BILLNO;
                entityaudit.STATE = "1";
                entityaudit.MEMO = MEMO;
                entityaudit.CHECKDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entityaudit.CHECKUSER = ManageProvider.Provider.Current().UserName;
                database.Update(entityaudit, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


         //销售容差审批
        public ActionResult PACheckOrder(string KeyValue, string MEMO, string ModuleId)
        {
            //验证
            VWB_SALEPRINTAUDIT entity = wb_order.GetEntity(KeyValue);
            if (entity.STATE == 1)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已审批！" }.ToString());
            }
            WB_SALEPRINTAUDIT entityaudit = new WB_SALEPRINTAUDIT();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                entityaudit.Create();
                entityaudit.BILLNO = entity.BILLNO;
                entityaudit.STATE = 1;
           
                entityaudit.TYPE = 1;
                entityaudit.MEMO = MEMO;
                entityaudit.CHECKDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entityaudit.CHECKUSER = ManageProvider.Provider.Current().UserName;
                database.Insert(entityaudit, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        
        /// <summary>
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            VWB_SALESUTTLEERROR entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        public ActionResult SetPAFormControl(string KeyValue)
        {
            VWB_SALEPRINTAUDIT entity = wb_order.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

    }
}
