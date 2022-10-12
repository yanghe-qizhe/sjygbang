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
    public class POTUIMATERIAController : PublicController<APP_HANDORDER>
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
        public ActionResult POBillTUIFForm()
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
        public ActionResult POBillTUIFIndex()
        {
            return View();
        }

        //明细页面
        [LoginAuthorize]
        public ActionResult POBillTUIFDetail()
        {
            return View();
        }
        /// <summary>
        /// 查询页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult POBillTUIFQuery()
        {
            return View();
        }

        /// <summary>
        /// 点击弹出页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult POSELECTBillTUIF()
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
        public ActionResult GetOrderList(string TaskNo, string BILLNO, string User, string CQDEF1, string VBILLCODE, string ISTYPE, string ISJL,string BATCHNO,string PZBILLNO, string DEPARTMENT, string PK_SENDORG, string PK_SENDADDRESS, string PK_STORE,string UPLOAD,string ISERROR, string ISZJD,string PK_MARBASCLASS, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDORDERBll vorderbll = new VAPP_HANDORDERBll();
                List<VAPP_HANDORDER> ListData = vorderbll.GetOrderList(TaskNo, BILLNO, User, CQDEF1, VBILLCODE, ISTYPE, ISJL, StartTime, EndTime, "0", "0", BATCHNO, PZBILLNO, DEPARTMENT, PK_SENDORG, PK_SENDADDRESS, PK_STORE, UPLOAD, ISERROR,ISZJD, PK_MARBASCLASS, jqgridparam);
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
        public ActionResult GetSeOrderList(string TaskNo, string VBILLCODE, string matclass, string Material, string Store, string Cars, string Supply, string User, string CQDEF1, string PK_ORG, string ISTYPE, string ISJL, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDORDERBll vorderbll = new VAPP_HANDORDERBll();
                List<VAPP_HANDORDER> ListData = vorderbll.GetSeOrderList(TaskNo, VBILLCODE, matclass, Material, Store, Cars, Supply, User, CQDEF1, PK_ORG, ISTYPE, ISJL, StartTime, EndTime, "0", "0", jqgridparam);
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
        public ActionResult GetWbOrderList(string BillNo, string PK_ORG, string Supply, string Material, string Cars, string CarsName, string DATETYPE, string ISTYPE, string ISJL, string MATERIALBARCODE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VWB_WEIGHT_SHBll vorderbll = new VWB_WEIGHT_SHBll();
                List<VWB_WEIGHT_SH> ListData = vorderbll.GetWbOrderList(BillNo, PK_ORG, Supply, Material, Cars, CarsName, "2", DATETYPE, ISTYPE, ISJL, MATERIALBARCODE, StartTime, EndTime, jqgridparam);
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
        /// 退货确认
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
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                string sql = String.Format("select count(1) from APP_HANDORDER  where  ID='{0}' and  STATUS!='2'", entity.ID);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "已走收货流程：" + entity.BILLNO + "不允许退货" }.ToString());
                }

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    entity.STATUS = "2";//退货状态
                    entity.OPERTYPE = "0";//退货操作
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    string sql1 = String.Format("select count(1) from APP_HANDORDER  where  ID='{0}' and STATUS='{1}'", entity.ID, "2");
                    int res1 = DataFactory.Database().FindCountBySql(sql1);
                    if (res1 > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "此单已经退货确认：" + entity.BILLNO }.ToString());
                    }

                    entity.Create();
                    entity.BILLNO = entity.BILLNO.Replace("&nbsp;", "");
                    if (string.IsNullOrEmpty(entity.BILLNO))
                    {
                        entity.BILLNO = entity.ID;
                    }
                    entity.STATUS = "2";//退货状态
                    entity.OPERTYPE = "0";//退货操作
                    database.Insert(entity, isOpenTrans);
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Append(string.Format("update DP_POCARSORDER_DTL set def10='{0}' where ID='{1}'", entity.BILLNO, entity.ID));
                    database.ExecuteBySql(strSql, isOpenTrans);
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
        /// 退货确认
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuerenOrderForm(APP_HANDORDER entity, string KeyValue, string ModuleId)
        {


            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                entity.STATUS = "2";//退货状态
                entity.OPERTYPE = "0";//退货操作
                string sql = String.Format("select count(1) from APP_HANDORDER  where  BILLNO='{0}'", entity.BILLNO);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res < 1)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请先退货登记：" + entity.BILLNO }.ToString());
                }
                string Message = KeyValue == "" ? "退货成功。" : "退货成功。";
                entity.Modify(KeyValue);
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
                entity.STATUS = "2";//退货状态
                entity.OPERTYPE = "0";//退货操作
                entity.OPERUSER = ManageProvider.Provider.Current().UserName;
                entity.OPERTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                string sql = String.Format("select count(1) from APP_HANDORDER  where  BILLNO='{0}' and  STATUS!='2'", entity.BILLNO);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "已走收货流程：" + entity.BILLNO + "不允许退货" }.ToString());
                }
                string sql1 = String.Format("select count(1) from APP_HANDORDER  where  BILLNO='{0}'", entity.BILLNO);
                int res1 = DataFactory.Database().FindCountBySql(sql1);
                if (res1 > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请勿重复退货：" + entity.BILLNO }.ToString());
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
        /// 质检确认操作
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZjsubOrder(string KeyValue)
        {
            //验证
            APP_HANDORDER entity = orderbll.GetEntity(KeyValue, "APP_HANDORDER"); //repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS != "2")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据还未退货确认，不能质检确认操作！" }.ToString());
            }
            if (entity.ZJSTATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "已经质检确认，请勿重复质检确认！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "质检确认成功。";
                entity.ZJSTATUS = "1";
                entity.ZJTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entity.ZJUSRE = ManageProvider.Provider.Current().UserName;
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
        ///删除 
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from APP_HANDORDER where ID='{0}' and STATUS=1", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "已收货确认，不能删除操作！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("APP_HANDORDER", "ID", KeyValue);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update DP_POCARSORDER_DTL SET ISSH=0,DEF10='{0}' where ID='{1}'", string.Empty, KeyValue));
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
            VAPP_HANDORDERBll overbll = new VAPP_HANDORDERBll();
            VAPP_HANDORDER entity = overbll.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        /// <summary>
        /// 返回对象JSON 新增 退货
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEntity(string KeyValue)
        {

            VWB_WEIGHT_SHBll vorderbll = new VWB_WEIGHT_SHBll();
            VWB_WEIGHT_SH entity = vorderbll.GetEntity(KeyValue);
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
