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
using System.Data;

namespace EOS.WebApp.Areas.WeightManager.Controllers
{

    public class WeightAuditController : PublicController<WB_WEIGHTAUDIT>
    {
        string ModuleId = "";

        VWB_WEIGHTBSBll orderbll = new VWB_WEIGHTBSBll();





        [LoginAuthorize]
        public ActionResult WeightAuditQuery()
        {
            return View();
        }

        /// 磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string VBillCode, string ContractBillNo, string TZBillNo, string Customer, string Material, string Cars, string Status, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHTBS> ListData = orderbll.GetVoidOrderList(BillNo, VBillCode, ContractBillNo, TZBillNo, Customer, Material, Cars, Status, "", StartTime, EndTime, jqgridparam);
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
        #region 红外
        [LoginAuthorize]
        public ActionResult HWWeightIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult GetHWOrderList(string BillNo, string VBillCode, string ContractBillNo, string TZBillNo, string Customer, string Material, string Cars, string Status, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHTBS> ListData = orderbll.GetHWOrderList(BillNo, VBillCode, ContractBillNo, TZBillNo, Customer, Material, Cars, Status, "8", StartTime, EndTime, jqgridparam);
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

        //红外一审批
        [LoginAuthorize]
        public ActionResult HWCheckOrder(string KeyValue)
        {
            //验证
            string strWhere = string.Format(" where billno='{0}'", KeyValue);
            VWB_WEIGHTBS entity = orderbll.GetEntity(strWhere, "VWB_WEIGHT_HW");
            if (entity.CHECKSTATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已红外一次审批！" }.ToString());
            }
            WB_WEIGHTAUDIT entityaudit = new WB_WEIGHTAUDIT();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                entityaudit.Create();
                entityaudit.BILLNO = entity.BILLNO;
                entityaudit.TYPE = "33";
                entityaudit.STATUS = "1";
                entityaudit.STATUS1 = "0";
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

        //红外二审批
        [LoginAuthorize]
        public ActionResult HWCheckOrder1(string KeyValue)
        {
            //验证
            string strWhere = string.Format(" where billno='{0}'", KeyValue);
            VWB_WEIGHTBS entity = orderbll.GetEntity(strWhere, "VWB_WEIGHT_HW");
            if (entity.CHECKSTATUS1 == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已红外二次审批！" }.ToString());
            }
            WB_WEIGHTAUDIT entityaudit = new WB_WEIGHTAUDIT();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                entityaudit.Create();
                entityaudit.BILLNO = entity.BILLNO;
                entityaudit.TYPE = "34";
                entityaudit.STATUS = "1";
                entityaudit.STATUS1 = "0";
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

        #endregion


        #region 空车
        [LoginAuthorize]
        public ActionResult KCWeightIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult GetKCOrderList(string BillNo, string VBillCode, string ContractBillNo, string TZBillNo, string Customer, string Material, string Cars, string Status, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHTBS> ListData = orderbll.GetKCOrderList(BillNo, VBillCode, ContractBillNo, TZBillNo, Customer, Material, Cars, Status, "8", StartTime, EndTime, jqgridparam);
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


        //空车审批
        [LoginAuthorize]
        public ActionResult KCCheckOrder(string KeyValue)
        {
            //验证
            string strWhere = string.Format(" where billno='{0}'", KeyValue);
            VWB_WEIGHTBS entity = orderbll.GetEntity(strWhere, "VWB_WEIGHT_KC");
            if (entity.CHECKSTATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已空车出厂审批！" }.ToString());
            }
            string sql = String.Format("select count(1) from lg_qy_task where BL_STATUS<>4 and BL_NO='{0}'", entity.PK_ORDER);

            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("请您先在电商平台作废提货单！") }.ToString());
            }

            WB_WEIGHTAUDIT entityaudit = new WB_WEIGHTAUDIT();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            string Account = ManageProvider.Provider.Current().Account;
            try
            {
                string Message = "审批成功。";
                entityaudit.Create();
                entityaudit.BILLNO = entity.BILLNO;
                entityaudit.TYPE = "3";
                entityaudit.STATUS = "1";
                entityaudit.STATUS1 = "0";
                database.Insert(entityaudit, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "提单作废审批成功");
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Login, "-1", "提单作废审批失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion


        /// <summary>
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            VWB_WEIGHTBS entity = orderbll.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

    }
}
