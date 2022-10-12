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
    public class HYBillSOFController : PublicController<DP_HYSOCARSORDER>
    {
        string ModuleId = "";

        DP_HYSOCARSORDERBll orderbll = new DP_HYSOCARSORDERBll();
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
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
        /// 发货通知单表单
        /// </summary>
        /// <returns></returns>
        public ActionResult HYBillSOFForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                string billno = this.BillCode();
                ViewBag.BillNo = billno;
                ViewBag.CARSNO = billno.Replace("HF01", "");
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        [LoginAuthorize]
        public ActionResult HYBillSOFIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult HYBillSOFDetail()
        {
            return View();
        }
        /// <summary>
        /// 设计要求销售订单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SELECTSALEORDERS()
        {
            return View();
        }
        /// <summary>
        /// 设计要求车皮号
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SELECTTRAINS()
        {
            return View();
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
                List<DP_HYSOCARSORDER> ListData = orderbll.GetOrderList(BillNo, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetHYOrderList(string BillNo, string VBillCode, string Customer, string Material, string Cars, string ISUSE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_HYSOCARSORDERBll vorderbll = new VDP_HYSOCARSORDERBll();
                List<VDP_HYSOCARSORDER> ListData = vorderbll.GetOrderList(BillNo, VBillCode, Customer, Material, Cars, ISUSE, StartTime, EndTime, jqgridparam);
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
        /// 提交订单表单（新增、编辑）
        /// </summary>
        /// <param name="KeyValue">订单主键</param>
        /// <param name="entity">订单实体</param>
        /// <param name="POOrderEntryJson">订单明细</param>
        /// <returns></returns>
        public ActionResult SubmitOrderForm(string KeyValue, string ModuleId, DP_HYSOCARSORDER entity, string OrderEntryJson)
        {
            entity.Create();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<DP_HYSOCARSORDER> OrderEntryList = OrderEntryJson.JonsToList<DP_HYSOCARSORDER>();

                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "变更成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    foreach (DP_HYSOCARSORDER poorderentry in OrderEntryList)
                    {
                        if (!string.IsNullOrEmpty(poorderentry.VBILLCODE))
                        {
                            poorderentry.CARSNO = entity.CARSNO;
                            poorderentry.CREATEDATE = entity.CREATEDATE;
                            poorderentry.MEMO = entity.MEMO;
                            database.Update(poorderentry, isOpenTrans);
                        }
                    }
                }
                else
                {
                    string UserId = ManageProvider.Provider.Current().UserId;
                    foreach (DP_HYSOCARSORDER poorderentry in OrderEntryList)
                    {
                        if (!string.IsNullOrEmpty(poorderentry.VBILLCODE))
                        {
                            poorderentry.Create();
                            poorderentry.TASKNO = entity.TASKNO;
                            poorderentry.CARSNO = entity.CARSNO;
                            poorderentry.CREATEDATE = entity.CREATEDATE;
                            poorderentry.MEMO = entity.MEMO;
                            poorderentry.ID = base_coderulebll.GetBillCode(UserId, ModuleId);
                            database.Insert(poorderentry, isOpenTrans);
                            base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        }
                    }
                }

                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from DP_HYSOCARSORDER where ID='{0}' and isuse!=4", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为作废的发货单能删除操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("DP_HYSOCARSORDER", "ID", KeyValue);
                //DEL_CARSORDERS(KeyValue, database, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue)
        {
            //验证
            DP_HYSOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "4")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该发货单已作废！" }.ToString());
            }
            else if (entity.ISUSE != "1" && entity.ISUSE != "5")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为未入厂的发货单能作废操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "作废成功。";
                entity.ISUSE = "4";
                entity.MEMO = "作废";
                database.Update(entity, isOpenTrans);
                DEL_CARSORDERS(KeyValue, database, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        [LoginAuthorize]
        public ActionResult OutOrder(string KeyValue)
        {
            //验证
            DP_HYSOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "6")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该发货单已出厂！" }.ToString());
            }
            else if (entity.ISUSE != "3")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为空车的发货单能出厂操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "强制出厂成功。";
                entity.ISUSE = "6";
                entity.MEMO = "强制出厂";
                database.Update(entity, isOpenTrans);
                DEL_CARSORDERS(KeyValue, database, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            DP_HYSOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到发单是审批状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                entity.STATUS = "1";
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
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            DP_HYSOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该发货单是未审批状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "反审成功。";
                entity.STATUS = "0";
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

        /// <summary>
        /// 【发货单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            DP_HYSOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        /// <summary>
        /// 订单明细列表（返回Json）
        /// </summary>
        /// <param name="POOrderId">订单主键</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderEntryList(string TASKNO)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetOrderEntryList(TASKNO),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
    }
}
