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
    public class SALEORDERSController : PublicController<NC_SALEORDER>
    {
        string ModuleId = "";

        NC_SALEORDERBll orderbll = new NC_SALEORDERBll();
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();

        [LoginAuthorize]
        public ActionResult SaleOrdersIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SALEORDERSDetail()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SALEORDERS_Form()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SALEORDERSQuery()
        {
            return View();
        }


        /// <summary>
        /// 获取自动单据编码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }
        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="BillNo">制单编号</param>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<NC_SALEORDER> ListData = orderbll.GetOrderList(BillNo, StartTime, EndTime, jqgridparam);
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

        [LoginAuthorize]
        public ActionResult GetSOOrderList(string BillNo, string PK_ORG, string Customer, string Material, string matclass, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VNC_SALEORDERBll vorderbll = new VNC_SALEORDERBll();
                List<VNC_SALEORDER> ListData = vorderbll.GetOrderList(BillNo, PK_ORG, Customer, "", Material, matclass, StartTime, EndTime, jqgridparam);
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

        [LoginAuthorize]
        public ActionResult GetQYSOOrderList(string BillNo, string PK_ORG, string Customer, string Material, string FSTATUSFLAG, string iszx, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VNC_SALEORDER_BBll vorderbll = new VNC_SALEORDER_BBll();
                List<VNC_SALEORDER_B> ListData = vorderbll.GetQYOrderList(BillNo, PK_ORG, Customer, "", Material, FSTATUSFLAG, iszx, StartTime, EndTime, jqgridparam);
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
        [LoginAuthorize]
        public ActionResult GetHYSOOrderList(string BillNo, string Customer, string Material, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VNC_SALEORDER_BBll vorderbll = new VNC_SALEORDER_BBll();
                List<VNC_SALEORDER_B> ListData = vorderbll.GetOrderList(BillNo, Customer, "0001E510000000000W3U", Material, StartTime, EndTime, jqgridparam);
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

        /// <summary>
        /// 订单明细列表（返回Json）
        /// </summary>
        /// <param name="POOrderId">订单主键</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetOrderEntryList(KeyValue),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        [LoginAuthorize]
        public ActionResult GetQYOrderEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetQYOrderEntryList(KeyValue),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// 【采购订单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            VNC_SALEORDERBll vorderbll = new VNC_SALEORDERBll();
            VNC_SALEORDER entity = vorderbll.SetFormControl(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }



        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SubmitOrderForm(string KeyValue, NC_SALEORDER entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<NC_SALEORDER_B> OrderEntryList = OrderEntryJson.JonsToList<NC_SALEORDER_B>();
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    strSql.Clear();
                    database.Delete<NC_SALEORDER_B>("CSALEORDERID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from DP_POCARSORDER where  ID='{0}'", entity.VBILLCODE);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.VBILLCODE = this.BillCode();
                    }
                    entity.Create();
                    entity.CSALEORDERID  = entity.VBILLCODE;
                    database.Insert(entity, isOpenTrans);
                }

                //处理明细
                int index = 1;
                foreach (NC_SALEORDER_B orderentry in OrderEntryList)
                {
                    //判断物料不为空
                    if (!string.IsNullOrEmpty(orderentry.CMATERIALID))
                    {
                        orderentry.Create();
                        orderentry.CSALEORDERBID = entity.CSALEORDERID + orderentry.CROWNO;//子表ID
                        orderentry.CSALEORDERID = entity.CSALEORDERID;//主表ID
                        database.Insert(orderentry, isOpenTrans);
                        index++;
                    }
                }
                decimal sumamount = OrderEntryList.Sum(c => c.NASTNUM);
                //更新主表总数量
                strSql.Clear();
                strSql.Append(string.Format("update NC_SALEORDER set NTOTALNUM=" + sumamount + " where CSALEORDERID='{0}'", entity.CSALEORDERID));
                database.ExecuteBySql(strSql, isOpenTrans);
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("销售订单号:{0}，新增成功。", entity.CSALEORDERID);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Add, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //审批 
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            NC_SALEORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.FSTATUSFLAG == 1)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该销售订单是启用状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "启用成功。";
                entity.FSTATUSFLAG = 1;
                database.Update(entity, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        //反审
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            NC_SALEORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.FSTATUSFLAG == 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该销售订单是停用状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "停用成功。";
                entity.FSTATUSFLAG = 0;
                database.Update(entity, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        //删除
        public ActionResult DeleteOrder(string keyvalue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string sql = String.Format("select count(1) from DP_SOCARSORDER_DTL where PK_SALEORDER in('{0}')", keyvalue);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "删除的订单中已有派车业务,请您确实后删除！" }.ToString());
                }
                database.Delete<NC_SALEORDER>(keyvalue, isOpenTrans);
                database.Delete<NC_SALEORDER_B>("CSALEORDERID", keyvalue, isOpenTrans);
                // database.ExecuteBySql(new StringBuilder("DELETE BD_ZJZY WHERE PCRAWID='" + keyvalue + "'"), isOpenTrans);

                database.Commit();
                string Message = "操作成功";
                string Account = ManageProvider.Provider.Current().Account;
                string msg = string.Format("销售订单删除成功。");
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", msg);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

    }
}
