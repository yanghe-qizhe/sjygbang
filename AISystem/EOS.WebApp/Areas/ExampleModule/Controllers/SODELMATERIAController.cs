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
    /// <summary>
    /// 销售发货
    /// </summary>
    public class SODELMATERIAController : PublicController<APP_HANDORDER>
    {

        string ModuleId = "";
        APP_HANDORDERBll orderbll = new APP_HANDORDERBll();
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
        /// 新增、编辑页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SOBillDELFForm()//POBillCOFForm
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                //ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }

        //列表页面
        [LoginAuthorize]
        public ActionResult SOBillDELFIndex()
        {
            return View();
        }

        //明细页面
        [LoginAuthorize]
        public ActionResult SOBillDELFDetail()
        {
            return View();
        }
        /// <summary>
        /// 查询页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SOBillDELFQuery()
        {
            return View();
        }

        /// <summary>
        /// 点击弹出页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SOSELECTBillDELF()
        {
            return View();
        }

        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="TaskNo">单据编号</param>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetOrderList(string TaskNo, string User, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDORDER_OUTBll vorderbll = new VAPP_HANDORDER_OUTBll();
                List<VAPP_HANDORDER_OUT> ListData = vorderbll.GetOrderList(TaskNo, User, StartTime, EndTime, "3", "2", jqgridparam);
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
        /// 查询（返回Json）
        /// </summary>
        /// <param name="TaskNo">单据编号</param>
        /// <param name="Material">物料</param>
        /// <param name="Store">仓库</param>
        /// <param name="Cars">车辆</param>
        /// <param name="Supply">供应商</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetSeOrderList(string TaskNo, string Material, string Store, string Cars, string Customer, string User, string StartTime, string EndTime, JqGridParam jqgridparam)
        {

            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDORDER_OUTBll vorderbll = new VAPP_HANDORDER_OUTBll();
                List<VAPP_HANDORDER_OUT> ListData = vorderbll.GetSeOrderList(TaskNo, Material, Store, Cars, Customer, User, StartTime, EndTime, "3", "2", jqgridparam);
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
        /// 查询（返回Json）
        /// </summary>
        /// <param name="TaskNo">单据编号</param>
        /// <param name="Material">物料</param>
        /// <param name="Store">仓库</param>
        /// <param name="Cars">车辆</param>
        /// <param name="Supply">供应商</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetWbOrderList(string BillNo, string Customer, string Material, string Cars, string CarsName, string StartTime, string EndTime, JqGridParam jqgridparam)
        {

            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VWB_WEIGHT_FHBll vorderbll = new VWB_WEIGHT_FHBll();
                List<VWB_WEIGHT_FH> ListData = vorderbll.GetWbOrderList(BillNo, Customer, Material, Cars, CarsName, StartTime, EndTime, jqgridparam);
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
        /// 发货登记
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm(APP_HANDORDER entity, string KeyValue, string ModuleId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                entity.STATUS = "0";
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    string sql = String.Format("select count(1) from APP_HANDORDER  where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "请勿重复发货登记：" + entity.BILLNO }.ToString());
                    }
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                    //base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
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

        /// <summary>
        /// 发货确认
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuerenOrderForm(APP_HANDORDER entity, string KeyValue, string ModuleId)
        {
            #region 验证
            #endregion

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {

                string Message = KeyValue == "" ? "发货成功。" : "发货成功。";

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);

                    entity.OPERUSER = ManageProvider.Provider.Current().UserName;
                    entity.OPERTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.STATUS = "1";
                    entity.OPERTYPE = "2";
                    entity.TYPE = "3";
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    string sql = String.Format("select count(1) from APP_HANDORDER  where  BILLNO='{0}' AND STATUS='1'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "请勿重复发货确认：" + entity.BILLNO }.ToString());
                    }
                    string sql1 = String.Format("select count(1) from APP_HANDORDER  where  BILLNO='{0}'  AND STATUS='2'", entity.BILLNO);
                    int res1 = DataFactory.Database().FindCountBySql(sql1);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "已走退货流程：" + entity.BILLNO + "不允许发货" }.ToString());
                    }
                    entity.Create();
                    entity.STATUS = "1";
                    entity.OPERTYPE = "2";
                    entity.TYPE = "3";
                    database.Insert(entity, isOpenTrans);
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Append(string.Format("update DP_SOCARSORDER_DTL set def10='{0}' where ID='{1}'", entity.BILLNO, entity.ID));
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

        /// <summary>
        /// 整车退货
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult TuiHuoOrderForm(APP_HANDORDER entity, string KeyValue, string ModuleId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                entity.STATUS = "2";
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                string sql = String.Format("select count(1) from APP_HANDORDER  where  BILLNO='{0}' and  STATUS!='2'", entity.BILLNO);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "已走发货流程：" + entity.BILLNO + "不允许退货" }.ToString());
                }
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
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

        /// <summary>
        ///删除 
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("APP_HANDORDER", "ID", KeyValue);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update DP_SOCARSORDER_DTL set def10='{0}' where ID='{1}'", string.Empty, KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
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
        /// 返回对象JSON 编辑 详细
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            VAPP_HANDORDER_OUTBll overbll = new VAPP_HANDORDER_OUTBll();
            VAPP_HANDORDER_OUT entity = overbll.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        /// <summary>
        /// 返回对象JSON 新增 发货
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEntity(string KeyValue)
        {

            VWB_WEIGHT_FHBll vorderbll = new VWB_WEIGHT_FHBll();
            VWB_WEIGHT_FH entity = vorderbll.GetEntity(KeyValue);
            if (entity.BILLNO == null)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "未查到相关数据，或未过一次磅" }.ToString());
            }
            else
            {
                string strJson = entity.ToJson();
                return Content(strJson);
            }
        }
    }
}
