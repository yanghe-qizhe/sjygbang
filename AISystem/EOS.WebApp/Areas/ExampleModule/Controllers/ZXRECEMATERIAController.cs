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
    public class ZXRECEMATERIAController : PublicController<APP_HANDZXORDER>
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
        /// 新增
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXBillRECEFForm()
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

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXBillRECEFEdit()
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
        public ActionResult ZXBillRECEFIndex()
        {
            return View();
        }

        //明细页面
        [LoginAuthorize]
        public ActionResult ZXBillRECEFDetail()
        {
            return View();
        }
        /// <summary>
        /// 查询页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXBillRECEFQuery()
        {
            return View();
        }
        public ActionResult ZXSELECTBillRECEF()
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
                List<VAPP_HANDZXORDER> ListData = vorderbll.GetOrderList(TaskNo, "1", "", PK_UserT, TYPE, StartTime, EndTime, jqgridparam);
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
        /// 收货查询
        /// </summary>
        /// <param name="TaskNo"></param>
        /// <param name="Material"></param>
        /// <param name="StoreSend"></param>
        /// <param name="StoreTake"></param>
        /// <param name="Cars"></param>
        /// <param name="Driver"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetSeOrderList(string TaskNo, string Material, string StoreSend, string StoreTake, string Cars, string Driver, string SOperuser, string ROperuser, string StartTime, string EndTime, JqGridParam jqgridparam)
        {

            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDZXORDERBll vorderbll = new VAPP_HANDZXORDERBll();
                List<VAPP_HANDZXORDER> ListData = vorderbll.GetSeOrderList(TaskNo, Material, StoreSend, StoreTake, Cars, SOperuser, ROperuser, StartTime, EndTime, "1", jqgridparam);
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
        /// 收货确认
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult QuerenOrderForm(APP_HANDZXORDER entity, string KeyValue, string ModuleId)
        {
            string Message = KeyValue == "" ? "收货成功。" : "编辑收货成功。";

            if (string.IsNullOrEmpty(entity.PK_CARS) || entity.PK_CARS == "&nbsp;")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "车号为空，不允许发货" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    ///处理空格
                    if (string.IsNullOrEmpty(entity.PDAKZ1) || entity.PDAKZ1 == "&nbsp;") { entity.PDAKZ1 = "0"; }
                    #region 验证
                    /*if (entity.STATUS == "1")
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "已收货不能编辑：" + entity.BILLNO }.ToString());
                    }*/
                    #endregion

                    entity.OPERTIMETAKE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.OPERUSERTAKE = ManageProvider.Provider.Current().UserName;
                    entity.BILLFROMT = "1";
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                    #region 更新过磅表
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(string.Format("update WB_WEIGHT_ZX set PK_MATERIAL='{1}',MATERIAL='{2}',MATERIALKIND='{3}'," +
                    "PK_CARSID='{4}',CARS='{5}',PK_RECEIVESTORE='{6}',RECEIVESTORE='{7}'," +
                    "DEF5='{8}',DEF6='{9}',DEF7='{10}',DEF8='{11}',PDAKZ1='{12}',DEF9='{13}',DEF10='{14}',DEF11='{15}' " +
                    "where PK_ORDER='{0}'",
                    KeyValue, entity.PK_MATERIAL, entity.MATERIAL, entity.MATERIALSPEC,
                    entity.PK_CARS, entity.CARS, entity.PK_STORETAKE, entity.STORETAKE,
                    entity.PK_LEADERDEPAT, entity.LEADERDEPAT, entity.PK_DEPARTMENTT, entity.DEPARTMENTT,
                    entity.PDAKZ1, entity.DEF3, entity.DEF4, entity.DEF2
                    ));
                    DataFactory.Database().ExecuteBySql(strSql, isOpenTrans);
                    #endregion
                }
                else
                {
                    ///处理空格
                    if (string.IsNullOrEmpty(entity.PDAKZ1) || entity.PDAKZ1 == "&nbsp;") { entity.PDAKZ1 = "0"; }
                    #region 验证
                    if (entity.FINISH == "0")
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "未过磅不能收货确认：" + entity.CARS }.ToString());
                    }
                    string sql = String.Format("select count(1) from APP_HANDZXORDER  where  ID='{0}' and STATUS={1}", entity.ID, "1");
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "请勿重复收货确认：" + entity.BILLNO }.ToString());
                    }
                    //string sql1 = String.Format("select count(1) from QC_ZJZXCYCJ  where  PK_ORDER='{0}' and ISCY={1}", entity.ID, "0");
                    //int res1 = DataFactory.Database().FindCountBySql(sql1);
                    //if (res1 > 0)
                    //{
                    //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "此单需质检未采样，不允许收货：" + entity.BILLNO }.ToString());
                    //}
                    #endregion
                    if (entity.MATERIALSPEC == null || entity.MATERIALSPEC == "null")
                    {
                        entity.MATERIALSPEC = "";
                    }
                    entity.PDANOTAKE = "/";
                    entity.BILLFROMT = "1";
                    //entity.DEF2 = "";
                    entity.STATUS = "1";
                    entity.OPERTIMETAKE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.OPERUSERTAKE = ManageProvider.Provider.Current().UserName;
                    entity.USERIDT = ManageProvider.Provider.Current().UserId;
                    entity.ACCOUNTT = ManageProvider.Provider.Current().Account;
                    entity.PK_LEADERDEPAT = ManageProvider.Provider.Current().DepartmentPId;
                    entity.LEADERDEPAT = ManageProvider.Provider.Current().DepartmentPName;
                    entity.PK_DEPARTMENTT = ManageProvider.Provider.Current().DepartmentId;
                    entity.DEPARTMENTT = ManageProvider.Provider.Current().DepartmentName;
                    database.Update(entity, isOpenTrans);
                    #region 更新过磅表
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append(string.Format("update WB_WEIGHT_ZX set PK_MATERIAL='{1}',MATERIAL='{2}',MATERIALKIND='{3}'," +
                        "PK_CARSID='{4}',CARS='{5}',PK_RECEIVESTORE='{6}',RECEIVESTORE='{7}'," +
                        "DEF5='{8}',DEF6='{9}',DEF7='{10}',DEF8='{11}',PDAKZ1='{12}', " +
                        "PK_COMPANY='{13}',COMPANYNAME='{14}',PK_DEPARTMENT='{15}',DEPARTMENTNAME='{16}'" +
                        ",DEF9='{17}',DEF10='{18}',DEF11='{19}' " +
                        "where PK_ORDER='{0}'",
                        entity.ID, entity.PK_MATERIAL, entity.MATERIAL, entity.MATERIALSPEC,
                        entity.PK_CARS, entity.CARS, entity.PK_STORETAKE, entity.STORETAKE,
                        entity.PK_LEADERDEPAT, entity.LEADERDEPAT, entity.PK_DEPARTMENTT, entity.DEPARTMENTT,
                        entity.PDAKZ1, entity.PK_LEADERDEPAS, entity.LEADERDEPAS, entity.PK_DEPARTMENTS, entity.DEPARTMENTS,
                        entity.DEF3, entity.DEF4, entity.DEF2
                        ));
                    DataFactory.Database().ExecuteBySql(strSql, isOpenTrans);
                    #endregion
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
        public ActionResult DeleteOrder1(string KeyValue)
        {
            #region 验证
            APP_HANDZXORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "已收货确认，不能删除操作！" }.ToString());
            }
            if (entity.FINISH == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "已过磅，不能删除操作，请先删除过磅！" }.ToString());
            }
            #endregion
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
        //取消收货
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            #region 验证
            APP_HANDZXORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "取消收货成功。";
                entity.STATUS = "0";
                entity.OPERTIMETAKE = "";
                entity.OPERUSERTAKE = "";
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
        /// 返回对象JSON 新增 收货
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetEntity(string KeyValue)
        {
            KeyValue = Server.UrlDecode(KeyValue);
            VAPP_HANDZXORDERBll voverbll = new VAPP_HANDZXORDERBll();
            VAPP_HANDZXORDER entity = voverbll.GetEntity(KeyValue, "0");
            if (entity.ID == null)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "未查到对应发货数据" }.ToString());
            }
            if (entity.FINISH == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "未过磅，不允许收货" }.ToString());
            }
            else
            {
                string strJson = entity.ToJson();
                return Content(strJson);
            }
        }
    }
}
