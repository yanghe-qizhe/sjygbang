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
    public class CHECKOUTController : PublicController<APP_HANDCHECKOUT>
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
        public ActionResult CheckOutFForm()//POBillCOFForm
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
        public ActionResult CheckOutFIndex()
        {
            return View();
        }

        //明细页面
        [LoginAuthorize]
        public ActionResult CheckOutDetail()
        {
            return View();
        }
        /// <summary>
        /// 查询页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult POBillRECFQuery()
        {
            return View();
        }

        /// <summary>
        /// 点击弹出页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult POSELECTBillRECF()
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
            User = ManageProvider.Provider.Current().UserId;
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDCHECKOUTORDERBll vorderbll = new VAPP_HANDCHECKOUTORDERBll();
                List<VAPP_HANDCHECKOUTORDER> ListData = vorderbll.GetOrderList(TaskNo, StartTime, EndTime, jqgridparam);
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
            User = ManageProvider.Provider.Current().UserId;
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDORDERBll vorderbll = new VAPP_HANDORDERBll();
                List<VAPP_HANDORDER> ListData = vorderbll.GetSeOrderList(TaskNo, VBILLCODE, matclass, Material, Store, Cars, Supply, User, CQDEF1, PK_ORG, ISTYPE, ISJL, StartTime, EndTime, "0", "1", jqgridparam);
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
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="TaskNo">单据编号</param>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetChekOutOrderList(string Pk_Task, JqGridParam jqgridparam)
        {
            string User = ManageProvider.Provider.Current().UserId;
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                WB_WEIGHTBll vorderbll = new WB_WEIGHTBll();

                List<WB_WEIGHT> ListData = vorderbll.GetChekOutOrderList(Pk_Task, jqgridparam);

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
        /// 收货登记
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm(APP_HANDCHECKOUT entity, string KeyValue, string ModuleId, string PK_TASK)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                entity.BILLNO = PK_TASK;
                entity.BILLFROM = "1";
                entity.PDANO = "/";
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                #region 验证
                //无过磅记录不让核验出厂
                string sql = String.Format("select count(1) from WB_WEIGHT where PK_TASK='{0}'", entity.BILLNO);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res < 1)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未找到对应的过磅记录" }.ToString());
                }
                //有未完成的过磅记录不让核验出厂
                string sql0 = String.Format("select count(1) from WB_WEIGHT where PK_TASK='{0}' and FINISH='{1}'", entity.BILLNO, '0');
                int res0 = DataFactory.Database().FindCountBySql(sql0);
                if (res0 > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "有未完成的过磅记录" }.ToString());
                }
                //判断是否重复核验
                string sql1 = String.Format("select count(1) from APP_HANDCHECKOUT where  BILLNO='{0}'", entity.BILLNO);
                int res1 = DataFactory.Database().FindCountBySql(sql1);
                if (res1 > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请勿重复门禁核验：" + entity.BILLNO }.ToString());
                }
                #endregion

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
        ///删除 
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            #region 验证
            string sql = String.Format("select * from WB_WEIGHT_TASK where PK_TASK='{0}' AND FINISH='{1}'", KeyValue, "1");
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "已出厂状态，不能删除操作！" }.ToString());
            }
            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("APP_HANDCHECKOUT", "ID", KeyValue);
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
            VAPP_HANDCHECKOUTORDERBll overbll = new VAPP_HANDCHECKOUTORDERBll();
            VAPP_HANDCHECKOUTORDER entity = overbll.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        /// <summary>
        /// 返回对象JSON 新增 收货
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEntity(string IcCnumber)
        {
            WB_WEIGHT_TASKBll overbll = new WB_WEIGHT_TASKBll();
            WB_WEIGHT_TASK entity = overbll.GetEntityCheckOut(IcCnumber);
            //if (entity.PK_TASK== null)
            //{
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未查到入厂数据" }.ToString());
            //}
            //else
            //{
            string strJson = entity.ToJson();
            return Content(strJson);
            //}
        }
    }
}
