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
    public class WeightZXController : PublicController<WB_WEIGHT_ZX>
    {
        string ModuleId = "";
        WB_WEIGHTZXBll wbbll = new WB_WEIGHTZXBll();
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        VWB_WEIGHTBSZXBll orderbll = new VWB_WEIGHTBSZXBll();
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
        public ActionResult WeightZXIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightZXOneIndex()
        {
            return View();
        }
        public ActionResult WeightZXDetail()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightZXForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
                ViewBag.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                ViewBag.DepartmentName = ManageProvider.Provider.Current().DepartmentName;
                ViewBag.DepartmentPId = ManageProvider.Provider.Current().DepartmentPId;
                ViewBag.DepartmentPName = ManageProvider.Provider.Current().DepartmentPName;
            }
            return View();
        }

        [LoginAuthorize]
        public ActionResult WeightZXWHForm()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightZXQuery()
        {
            return View();
        }

        public ActionResult GetBillNo(string ModuleId)
        {
            this.ModuleId = ModuleId;
            return Content(BillCode());
        }

        /// 采购磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string SendStore, string ReceiveStore, string Material, string MaterialName, string Cars, string Drivers, string Type, string Finish, string COMPUTERTYPE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHTBS_ZX> ListData = orderbll.GetZXOrderList(BillNo, SendStore, ReceiveStore, Material, MaterialName, Cars, Drivers, Type, Finish, COMPUTERTYPE, StartTime, EndTime, jqgridparam);
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

        public ActionResult GetOneOrderList(string BillNo, string SendStore, string ReceiveStore, string Material, string Cars, string Drivers, string COMPUTERTYPE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHTBS_ZX> ListData = orderbll.GetZXOrderList(BillNo, SendStore, ReceiveStore, Material, "", Cars, Drivers, "", "0", COMPUTERTYPE, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(string KeyValue, WB_WEIGHT_ZX entity, string ModuleId)
        {
            if (string.IsNullOrEmpty(entity.PDAKZ1) || entity.PDAKZ1 == "&nbsp;")
            {
                entity.PDAKZ1 = "0";
            }
            #region 验证

            #endregion

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {

                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    WB_WEIGHT_ZX edit_entity = repositoryfactory.Repository().FindEntity(KeyValue);
                    edit_entity.PK_SENDORG = entity.PK_SENDORG;
                    edit_entity.SENDORG = entity.SENDORG;
                    edit_entity.PK_RECEIVEORG = entity.PK_RECEIVEORG;
                    edit_entity.RECEIVEORG = entity.RECEIVEORG;
                    edit_entity.PK_CARSID = entity.PK_CARSID;
                    edit_entity.CARS = entity.CARS;
                    edit_entity.PK_SENDSTORE = entity.PK_SENDSTORE;
                    edit_entity.SENDSTORE = entity.SENDSTORE;
                    edit_entity.PK_RECEIVESTORE = entity.PK_RECEIVESTORE;
                    edit_entity.RECEIVESTORE = entity.RECEIVESTORE;
                    edit_entity.GROSS = entity.GROSS;
                    edit_entity.TARE = entity.TARE;
                    edit_entity.SUTTLE = entity.SUTTLE;
                    edit_entity.PK_MATERIAL = entity.PK_MATERIAL;
                    edit_entity.MATERIAL = entity.MATERIAL;
                    edit_entity.PK_MATERIALKIND = entity.PK_MATERIALKIND;
                    edit_entity.MATERIALKIND = entity.MATERIALKIND;
                    edit_entity.PDAKZ1 = entity.PDAKZ1;
                    edit_entity.CREATEDATE = entity.CREATEDATE;
                    edit_entity.LIGHTDATE = entity.LIGHTDATE;
                    edit_entity.WEIGHTDATE = entity.WEIGHTDATE;
                    edit_entity.OUTGBUSER = entity.OUTGBUSER;
                    edit_entity.INGBUSER = entity.INGBUSER;
                    edit_entity.ISEDIT = 1;
                    edit_entity.TYPE = entity.TYPE;
                    edit_entity.MEMO = entity.MEMO;
                    edit_entity.VMEMO = entity.VMEMO;
                    database.Update(edit_entity, isOpenTrans);

                    #region 倒运收发货
                    APP_HANDZXORDER app_entity = null;
                    app_entity = database.FindEntityByWhere<APP_HANDZXORDER>(string.Format(" and ID='{0}'", entity.PK_ORDER));
                    //发货单位
                    app_entity.DEF1 = entity.PK_SENDORG;
                    app_entity.DEF2 = entity.SENDORG;
                    //收货单位
                    app_entity.DEF3 = entity.PK_RECEIVEORG;
                    app_entity.DEF4 = entity.RECEIVEORG;


                    app_entity.PK_STORESEND = entity.PK_SENDSTORE;//发货仓库
                    app_entity.STORESEND = entity.SENDSTORE;
                    app_entity.PK_STORETAKE = entity.PK_RECEIVESTORE;//收货仓库
                    app_entity.STORETAKE = entity.RECEIVESTORE;
                    app_entity.PK_MATERIAL = entity.PK_MATERIAL;//物料
                    app_entity.MATERIAL = entity.MATERIAL;
                    app_entity.MATERIALSPEC = entity.MATERIALKIND;
                    app_entity.CARS = entity.CARS;
                    app_entity.PK_CARS = entity.PK_CARSID;
                    app_entity.PK_DRIVER = entity.PK_DRIVERS;
                    app_entity.DRIVER = entity.DRIVERS;
                    //获取ICCard
                    //List<WB_CARD> List = database.FindList<WB_CARD>(" AND DRIVERS='" + entity.PK_DRIVERS + "'");
                    //WB_CARD m = null;
                    //if (List.Count > 0)
                    //{
                    //    m = List[0];
                    //    app_entity.ICCARD = m != null ? m.ICCARD : "";
                    //}
                    database.Update(app_entity, isOpenTrans);
                    #endregion

                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from WB_WEIGHT_ZX where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    entity.LASTDATE = entity.WEIGHTDATE;
                    entity.PK_ORDER = Guid.NewGuid().ToString();
                    entity.INGBUSER = ManageProvider.Provider.Current().UserName;// "磅房管理员";
                    database.Insert(entity, isOpenTrans);

                    #region 倒运收发货
                    APP_HANDZXORDER app_entity = new APP_HANDZXORDER();
                    app_entity.Create();
                    //重写
                    //发货单位
                    app_entity.DEF1 = entity.PK_SENDORG;
                    app_entity.DEF2 = entity.SENDORG;
                    //收货单位
                    app_entity.DEF3 = entity.PK_RECEIVEORG;
                    app_entity.DEF4 = entity.RECEIVEORG;
                    app_entity.ID = entity.PK_ORDER;

                    app_entity.PDANOSEND = "/";
                    app_entity.CARS = entity.CARS;
                    app_entity.PK_CARS = entity.PK_CARSID;
                    app_entity.PK_DRIVER = entity.PK_DRIVERS;
                    app_entity.DRIVER = entity.DRIVERS;
                    //获取ICCard
                    List<WB_CARD> List = database.FindList<WB_CARD>(" AND DRIVERS='" + entity.PK_DRIVERS + "'");
                    WB_CARD m = new WB_CARD();
                    if (List.Count > 0)
                    {
                        m = List[0];
                        app_entity.ICCARD = m != null ? m.ICCARD : "";
                    }
                    app_entity.PK_STORESEND = entity.PK_SENDSTORE;
                    app_entity.STORESEND = entity.SENDSTORE;
                    app_entity.PK_STORETAKE = entity.PK_RECEIVESTORE;
                    app_entity.STORETAKE = entity.RECEIVESTORE;

                    app_entity.PK_MATERIAL = entity.PK_MATERIAL;
                    app_entity.MATERIAL = entity.MATERIAL;
                    app_entity.MATERIALSPEC = entity.MATERIALKIND;
                    app_entity.PDAKZ1 = entity.PDAKZ1;
                    app_entity.BILLFROM = "1";
                    app_entity.FINISH = "1";
                    app_entity.STATUS = "1";
                    database.Insert(app_entity, isOpenTrans);
                    #endregion

                }
                database.Commit();
                string msg = string.Format("{0}车号转序计量单{1}毛{2}、皮{3},调转成功", entity.CARS, entity.BILLNO, entity.GROSS, entity.TARE);
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



        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            WB_WEIGHT_ZX entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.FINISH == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计量单已二次计量！" }.ToString());
            }
            if (entity.COMPUTERTYPE == "99")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计量单已作废！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("WB_WEIGHT_ZX", "BILLNO", KeyValue);
                database.Commit();
                string msg = string.Format("{0}车号转序计量单{1}毛{2}、皮{3},删除成功", entity.CARS, entity.BILLNO, entity.GROSS, entity.TARE);
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
        //作废
        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue)
        {
            //验证
            WB_WEIGHT_ZX entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.COMPUTERTYPE == "99")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计量单已作废！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "作废成功。";
                entity.COMPUTERTYPE = "99";
                entity.MEMO = "作废";
                entity.FINISH = "1";
                entity.STATUS = "完成";
                database.Update(entity, isOpenTrans);
                database.Commit();

                string msg = string.Format("{0}车号转序计量单{1}毛{2}、皮{3},作废成功", entity.CARS, entity.BILLNO, entity.GROSS, entity.TARE);
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

        //作废
        [LoginAuthorize]
        public ActionResult OneInvalidOrder(string KeyValue)
        {
            //验证
            WB_WEIGHT_ZX entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.COMPUTERTYPE == "99")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计量单已作废！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "作废成功。";
                entity.COMPUTERTYPE = "99";
                entity.MEMO = "作废";
                entity.FINISH = "1";
                entity.STATUS = "完成";
                entity.PK_ORDER = "";
                entity.PK_ORDER_B = "";
                database.Update(entity, isOpenTrans);

                database.Commit();

                string msg = string.Format("{0}车号转序计量单{1}毛{2}、皮{3},作废成功", entity.CARS, entity.BILLNO, entity.GROSS, entity.TARE);
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



        //出厂
        [LoginAuthorize]
        public ActionResult OutOrder(string KeyValue)
        {
            //验证
            WB_WEIGHT_ZX entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.FINISH == "1" && entity.STATUS == "完成")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计量单已完成！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "强制完成成功。";
                entity.Modify(KeyValue);
                entity.FINISH = "1";
                entity.STATUS = "完成";
                database.Update(entity, isOpenTrans);

                //门禁出厂
                //System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //strSql.Append(string.Format("update wb_weight_task_zx set finish='1',status='出厂' where PK_TASK='{0}'", KeyValue));
                //repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);

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
            WB_WEIGHT_ZX entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        public ActionResult BDPrintWeight(string KeyValue)
        {
            DataSet dataSet = orderbll.BDPrintWeight(KeyValue);
            return Content(dataSet.GetXml());
        }
    }
}
