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
    /// 厂内工序转序
    /// </summary>
    public class GXCONVOPERController : PublicController<DP_GXCONVOPER>
    {

        string ModuleId = "";
        DP_GXCONVOPERBll orderbll = new DP_GXCONVOPERBll();
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
        [LoginAuthorize]
        public ActionResult GXBillCOFForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
                ViewBag.STARTTIME = EOS.Utilities.DateTimeHelper.GetToday();
                ViewBag.ENDTIME = EOS.Utilities.DateTimeHelper.Now.AddYears(5).ToString("yyyy-MM-dd");
            }
            return View();
        }

        [LoginAuthorize]
        public ActionResult GXBillCOFBATCHForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
                ViewBag.STARTTIME = EOS.Utilities.DateTimeHelper.GetToday();
                ViewBag.ENDTIME = EOS.Utilities.DateTimeHelper.Now.AddYears(5).ToString("yyyy-MM-dd");
            }
            return View();
        }

        //列表
        [LoginAuthorize]
        public ActionResult GXBillCOFIndex()
        {
            return View();
        }
        //明细
        [LoginAuthorize]
        public ActionResult GXBillCOFDetail()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GXBillCOFQuery()
        {
            return View();
        }
        /// <summary>
        /// 设计要求采购订单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SELECTPOORDERS()
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
        public ActionResult GetOrderList(string TaskNo, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_GXCONVOPERBll vorderbll = new VDP_GXCONVOPERBll();
                List<VDP_GXCONVOPER> ListData = vorderbll.GetOrderList(TaskNo, StartTime, EndTime, jqgridparam);
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
        /// 工序查询
        /// </summary>
        /// <param name="TaskNo"></param>
        /// <param name="StoveNum"></param>
        /// <param name="Acceptunit"></param>
        /// <param name="Material"></param>
        /// <param name="Scale"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <param name="DEF2">牌号</param>
        /// <param name="MATERIALSPEC">规格</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetGXOrderList(string TaskNo, string Material, string PK_SCALE, string MATERIALSPEC, string DEF2, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_GXCONVOPERBll vorderbll = new VDP_GXCONVOPERBll();
                List<VDP_GXCONVOPER> ListData = vorderbll.GetGXOrderList(TaskNo, PK_SCALE, MATERIALSPEC, DEF2, Material, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(DP_GXCONVOPER entity, string KeyValue, string ModuleId)
        {
            #region 验证
            ////车辆验证
            //JsonMessage jsonmsg = CHECK_CARS_ORDER(entity.CARS, 2, entity.ID);
            //if (jsonmsg != null)
            //{
            //    return Content(jsonmsg.ToString());
            //}
            ////身份证验证
            //jsonmsg = CHECK_CARS_ORDER(entity.PSDNO, 2, entity.ID);
            //if (jsonmsg != null)
            //{
            //    return Content(jsonmsg.ToString());
            //}
            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
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



        public ActionResult SubmitAllOrderForm(DP_GXCONVOPER entity, string OrderEntryJson, string ModuleId)
        {
            entity.Create();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<DP_GXCONVOPER> OrderEntryList = OrderEntryJson.JonsToList<DP_GXCONVOPER>();
                //处理主头
                string Message = "新增成功。";

                string UserId = ManageProvider.Provider.Current().UserId;
                foreach (DP_GXCONVOPER poentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(poentry.MATERIALNAME))
                    {
                        base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        poentry.Create();
                        poentry.PK_SENDUNIT = entity.PK_SENDUNIT;
                        poentry.SENDUNITNAME = entity.SENDUNITNAME;
                        poentry.PK_ACCEPTUNIT = entity.PK_ACCEPTUNIT;
                        poentry.ACCEPTUNITNAME = entity.ACCEPTUNITNAME;
                        poentry.PK_SCALE = entity.PK_SCALE;
                        poentry.SCALENAME = entity.SCALENAME;
                        poentry.STARTTIME = entity.STARTTIME;
                        poentry.ENDTIME = entity.ENDTIME;
                        poentry.MEMO = entity.MEMO;
                        poentry.TASKNO = base_coderulebll.GetBillCode(UserId, ModuleId);
                        database.Insert(poentry, isOpenTrans);
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

        /// <summary>
        ///删除 
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from DP_GXCONVOPER where ID='{0}' and isuse!=0", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "单据启用中，不能删除操作，请停用再删除！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("DP_GXCONVOPER", "ID", KeyValue);
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
        //作废
        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue)
        {
            //验证
            DP_GXCONVOPER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "4")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单已作废！" }.ToString());
            }
            else if (entity.ISUSE != "1" && entity.ISUSE != "5")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为未入厂的到货单能作废操作！" }.ToString());
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
        //出厂
        [LoginAuthorize]
        public ActionResult OutOrder(string KeyValue)
        {
            //验证
            DP_GXCONVOPER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "6")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单已出厂！" }.ToString());
            }
            else if (entity.ISUSE != "3")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为空车的到货单能出厂操作！" }.ToString());
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
        //启用审批 
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            DP_GXCONVOPER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据是已使用状态，无需重复启用！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "启用成功。";
                entity.ISUSE = "1";
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
        //停用反审
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            DP_GXCONVOPER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已用，不能停用！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "停用成功。";
                entity.ISUSE = "0";
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
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            DP_GXCONVOPER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
