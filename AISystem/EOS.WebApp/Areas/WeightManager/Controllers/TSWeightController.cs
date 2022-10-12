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

namespace EOS.WebApp.Areas.WeightManager.Controllers
{
    public class TSWeightController : PublicController<WB_RAILWAY_DTL>
    {
        string ModuleId = "";
        WB_RAILWAY_DTLBll orderbll = new WB_RAILWAY_DTLBll();
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

        //
        // GET: /WeightManager/WeightIn/
        [LoginAuthorize]
        public ActionResult TSWeightIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult TSWeightForm()
        {
            string op = Request["op"];
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue) || op == "add")
            {
                ViewBag.BillNo = this.BillCode();

                string sql = String.Format("select count(1) from WB_RAILWAY_DTL where  BILLNO='{0}'", ViewBag.BillNo);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId);
                    ViewBag.BillNo = this.BillCode();
                }

                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
                ViewBag.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                ViewBag.DepartmentName = ManageProvider.Provider.Current().DepartmentName;
                ViewBag.DepartmentPId = ManageProvider.Provider.Current().DepartmentPId;
                ViewBag.DepartmentPName = ManageProvider.Provider.Current().DepartmentPName;
            }
            return View();
        }
        [LoginAuthorize]
        public ActionResult TSWeight_Form()
        {

            ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            ViewBag.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
            ViewBag.DepartmentName = ManageProvider.Provider.Current().DepartmentName;
            ViewBag.DepartmentPId = ManageProvider.Provider.Current().DepartmentPId;
            ViewBag.DepartmentPName = ManageProvider.Provider.Current().DepartmentPName;

            return View();
        }

        public ActionResult TSWeightQuery()
        {
            return View();
        }
        /// <summary>
        /// 设计要求采购订单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SELECTHYCONVOPER()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SELECTHYCONVOPERS()
        {
            return View();
        }
        /// 采购磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string Material, string STOVENUM, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_RAILWAY_DTL> ListData = orderbll.GetTSOrderList(BillNo, Material, STOVENUM, "4", StartTime, EndTime, jqgridparam);
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
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm(string KeyValue, WB_RAILWAY_DTL entity, string ModuleId)
        {
            #region 验证
            //验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {

                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    WB_RAILWAY_DTL obj = repositoryfactory.Repository().FindEntity(KeyValue);
                    entity.Modify(KeyValue);

                    if (!string.IsNullOrEmpty(obj.PK_EID) && !string.IsNullOrEmpty(entity.PK_EID))
                    {
                        if (obj.PK_EID != entity.PK_EID)
                        {
                            strSql.Clear();
                            strSql.Append(string.Format("update DP_HYCONVOPER set STATUS='{0}' where TASKNO='{1}'", 0, obj.PK_EID));
                            repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                        }
                    }
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from WB_RAILWAY_DTL where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    //铁水
                    entity.TYPE = "4";
                    entity.WEIGHTER = ManageProvider.Provider.Current().UserName;// "磅房管理员";
                    database.Insert(entity, isOpenTrans);
            
                }
                if (!string.IsNullOrEmpty(entity.PK_EID))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_HYCONVOPER set STATUS='{0}' where TASKNO='{1}'", 1, entity.PK_EID));
                    repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
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


        [HttpPost]
        public ActionResult SubmitOrderBForm(string KeyValue, string OrderEntryJson, string ModuleId)
        {
            #region 验证
            //验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<WB_RAILWAY_DTL> OrderEntryList = OrderEntryJson.JonsToList<WB_RAILWAY_DTL>();

                //处理主头
                string Message = "新增成功。";

                foreach (WB_RAILWAY_DTL orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.PK_EID))
                    {
                        base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        this.ModuleId = ModuleId;
                        orderentry.BILLNO = this.BillCode();
                        orderentry.Create();
                        //铁水
                        orderentry.TYPE = "4";
                        orderentry.WEIGHTER = ManageProvider.Provider.Current().UserName;// "磅房管理员";
                        database.Insert(orderentry, isOpenTrans);
                        if (!string.IsNullOrEmpty(orderentry.PK_EID))
                        {
                            strSql.Clear();
                            strSql.Append(string.Format("update DP_HYCONVOPER set STATUS='{0}' where TASKNOID='{1}'", 1, orderentry.PK_EID));
                            repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
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

        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from WB_RAILWAY_DTL where BILLNO='{0}' and ISVALID=1", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为作废的计量单能删除操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("WB_RAILWAY_DTL", "BILLNO", KeyValue);
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
            WB_RAILWAY_DTL entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ADUIT == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "此数据已审批！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                entity.ADUIT = "1";
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

        //作废
        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue)
        {
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            //验证
            WB_RAILWAY_DTL entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISVALID == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计量单已作废！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "作废成功。";
                entity.ISVALID = "0";
                entity.DEF9 = "作废";
                database.Update(entity, isOpenTrans);
                if (!string.IsNullOrEmpty(entity.PK_EID))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_HYCONVOPER set STATUS='{0}' where TASKNOID='{1}'", 0, entity.PK_EID));
                    repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                }
                database.Commit();
                string msg = string.Format("铁水转序计量单{0}车号{1}毛{2}皮{3}作废成功", entity.BILLNO, entity.CARNUM, entity.MAOAMOUNT, entity.PIAMOUNT);
                string Account = ManageProvider.Provider.Current().Account;
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", msg);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 【到货单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            WB_RAILWAY_DTL entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
