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
    /// 转序发货控制
    /// </summary>
    public class ZXSENDMATERIAController : PublicController<APP_HANDORDER>
    {

        string ModuleId = "";
        APP_HANDZXORDERBll orderbll = new APP_HANDZXORDERBll();
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
        /// 新增页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXBillSENDFForm()
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
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXBillSENDFEdit()
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
        public ActionResult ZXBillSENDFIndex()
        {
            return View();
        }

        //明细页面
        [LoginAuthorize]
        public ActionResult ZXBillSENDFDetail()
        {
            return View();
        }
        /// <summary>
        /// 查询页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXBillSENDFQuery()
        {
            return View();
        }

        /// <summary>
        /// 点击弹出页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXSELECTBillSENDF()
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
        public ActionResult GetOrderList(string TaskNo, string PK_UserS, string PK_UserT,string TYPE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDZXORDERBll vorderbll = new VAPP_HANDZXORDERBll();
                List<VAPP_HANDZXORDER> ListData = vorderbll.GetOrderList(TaskNo, "", PK_UserS, "", TYPE, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetSeOrderList(string TaskNo, string Material, string StoreSend, string StoreTake, string Cars, string Driver, string SOperuser, string ROperuser, string StartTime, string EndTime, string Status, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDZXORDERBll vorderbll = new VAPP_HANDZXORDERBll();
                List<VAPP_HANDZXORDER> ListData = vorderbll.GetSeOrderList(TaskNo, Material, StoreSend, StoreTake, Cars, SOperuser, ROperuser, StartTime, EndTime, Status, jqgridparam);
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
        public ActionResult GetWbOrderList(string BillNo, string SendStore, string ReceiveStore, string Material, string MaterialName, string Cars, string Drivers, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VWB_WEIGHTBSZXBll orderbll = new VWB_WEIGHTBSZXBll();
                List<VWB_WEIGHTBS_ZX> ListData = orderbll.GetWbOrderList(BillNo, SendStore, ReceiveStore, Material, MaterialName, Cars, Drivers, "", "", StartTime, EndTime, jqgridparam);
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
        /// 发货确认
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuerenOrderForm(APP_HANDZXORDER entity, string KeyValue, string ModuleId)
        {
            string Message = KeyValue == "" ? "发货成功。" : "编辑发货成功。";
            if (string.IsNullOrEmpty(entity.PK_CARS) || entity.PK_CARS == "&nbsp;")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "车号为空，不允许发货" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //编辑
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    if (entity.STATUS == "0")
                    {
                        #region 验证

                        //同一个车辆有未完成发货记录
                        string sql1 = String.Format("select count(1) from APP_HANDZXORDER  where  PK_CARS='{0}' AND STATUS='{1}' AND ID!='{2}'", entity.PK_CARS, "0", KeyValue);
                        int res1 = DataFactory.Database().FindCountBySql(sql1);
                        if (res1 > 0)
                        {
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = "此车有未完成记录：" + entity.CARS }.ToString());
                        }
                        #endregion
                    }
                    if (entity.MATERIALSPEC == null || entity.MATERIALSPEC == "null")
                    {
                        entity.MATERIALSPEC = "";
                    }

                    entity.Modify(KeyValue);
                    entity.OPERTIMETAKE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.OPERUSERTAKE = ManageProvider.Provider.Current().UserName;
                    database.Update(entity, isOpenTrans);

                }
                else//新增
                {
                    #region 验证
                    //同一个车辆有未完成发货记录
                    string sql1 = String.Format("select count(1) from APP_HANDZXORDER  where  PK_CARS='{0}' AND STATUS='{1}'", entity.PK_CARS, "0");
                    int res1 = DataFactory.Database().FindCountBySql(sql1);
                    if (res1 > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "此车有未完成记录：" + entity.CARS }.ToString());
                    }
                    #endregion
                    if (entity.MATERIALSPEC == null || entity.MATERIALSPEC == "null")
                    {
                        entity.MATERIALSPEC = "";
                    }
                    entity.STATUS = "0";
                    entity.PDANOSEND = "/";
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
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
            string sql = String.Format("select count(1) from APP_HANDZXORDER where ID='{0}' and STATUS=1", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "已收货确认，不能删除操作！" }.ToString());
            }
            //删除发货记录时过磅是否作废验证
            string sql1 = String.Format("select count(1) from Wb_Weight_Zx where  COMPUTERTYPE!='99' and PK_ORDER='{0}'", KeyValue);
            int res1 = DataFactory.Database().FindCountBySql(sql1);
            if (res1 > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "已过磅计量，请先作废过磅记录！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("APP_HANDZXORDER", "ID", KeyValue);
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
            VAPP_HANDZXORDERBll overbll = new VAPP_HANDZXORDERBll();
            VAPP_HANDZXORDER entity = overbll.GetOrderEntity(KeyValue);
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
            VAPP_HANDZXORDERBll voverbll = new VAPP_HANDZXORDERBll();
            VAPP_HANDZXORDER entity1 = voverbll.GetEntity(KeyValue, "");
            if (entity1.ID == null)
            {
                VWB_CARDBll overbll = new VWB_CARDBll();
                VWB_CARD entity = overbll.GetEntity(KeyValue, ModuleId, "");
                if (entity.DRIVER == null)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未查到相关数据" }.ToString());
                }
                else
                {
                    string strJson = entity.ToJson();
                    return Content(strJson);
                }
            }
            else
            {
                string strJson = entity1.ToJson();
                return Content(strJson);
            }
        }
    }
}
