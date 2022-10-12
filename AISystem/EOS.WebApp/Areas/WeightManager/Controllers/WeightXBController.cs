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
    public class WeightXBController : PublicController<WB_WEIGHT_XB>
    {
        string ModuleId = "";
        WB_WEIGHT_XBBll orderbll = new WB_WEIGHT_XBBll();
        VWB_WEIGHT_XBBll vorderbll = new VWB_WEIGHT_XBBll();
        VWB_WEIGHT_XBCBBll vorderbll1 = new VWB_WEIGHT_XBCBBll();
        VWB_WEIGHT_XBYKBll vorderbll2 = new VWB_WEIGHT_XBYKBll();
        VWB_WEIGHT_XB1Bll vorderbll3 = new VWB_WEIGHT_XB1Bll();
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
        public ActionResult WeightXBIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightXBIndex1()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightXBIndex2()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult WeightXBIndex3()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightXBIndexMES()
        {
            return View();
        }
        public ActionResult WeightXBDetail()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightXBForm()
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
        public ActionResult WeightXBForm1()
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
        [LoginAuthorize]
        public ActionResult WeightXBForm2()
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
        [LoginAuthorize]
        public ActionResult WeightXBForm3()
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
        [LoginAuthorize]
        public ActionResult WeightXBWHForm()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightXBQuery()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SELECTPCBillSOF()
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
        public ActionResult GetOrderList(string BillNo, string PK_ORDER_B, string SendStore, string ReceiveStore, string Material, string MaterialName, string Cars, string Drivers, string Type, string Finish, string COMPUTERTYPE, string BATHNO, string UPLOAD1, string DEF4, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHT_XB> ListData = vorderbll.GetOrderList(BillNo, PK_ORDER_B, SendStore, ReceiveStore, Material, MaterialName, Cars, Drivers, Type, Finish, COMPUTERTYPE, BATHNO, UPLOAD1, DEF4, StartTime, EndTime, jqgridparam);
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
  
        /// 采购磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList1(string BillNo, string PK_ORDER_B, string SendStore, string ReceiveStore, string Material, string MaterialName, string Cars, string Drivers, string Type, string Finish, string COMPUTERTYPE, string BATHNO,string UPLOAD1, string PCCARSNAME, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHT_XBCB> ListData = vorderbll1.GetOrderList(BillNo, PK_ORDER_B, SendStore, ReceiveStore, Material, MaterialName, Cars, Drivers, Type, Finish, COMPUTERTYPE, BATHNO, UPLOAD1, PCCARSNAME, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetOrderList2(string BillNo, string PK_ORDER_B, string SendStore, string ReceiveStore, string Material, string MaterialName, string Cars, string Drivers, string Type, string Finish, string COMPUTERTYPE, string BATHNO, string UPLOAD1, string DEF4, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHT_XBYK> ListData = vorderbll2.GetOrderList(BillNo, PK_ORDER_B, SendStore, ReceiveStore, Material, MaterialName, Cars, Drivers, Type, Finish, COMPUTERTYPE, BATHNO, UPLOAD1, DEF4, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetOrderList3(string BillNo, string PK_ORDER_B, string SendStore, string ReceiveStore, string Material, string MaterialName, string Cars, string Drivers, string Type, string Finish, string COMPUTERTYPE, string BATHNO, string UPLOAD1, string DEF4, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHT_XB1> ListData = vorderbll3.GetOrderList(BillNo, PK_ORDER_B, SendStore, ReceiveStore, Material, MaterialName, Cars, Drivers, Type, Finish, COMPUTERTYPE, BATHNO, UPLOAD1, DEF4, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetOrderListMES(string SITE, string ITEM, string SHIFT, string STORAGELOCATION, string BATCH, string ITEM_MARK,string UPLOAD,string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                VWB_WEIGHT_XB_MESBll vorderbll = new VWB_WEIGHT_XB_MESBll();
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHT_XB_MES> ListData = vorderbll.GetOrderList(SITE, ITEM, SHIFT, STORAGELOCATION, BATCH, ITEM_MARK , UPLOAD, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(string KeyValue, WB_WEIGHT_XB entity, string ModuleId)
        {

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
                    WB_WEIGHT_XB edit_entity = database.FindEntity<WB_WEIGHT_XB>(KeyValue);
                    edit_entity.PK_SENDORG = entity.PK_SENDORG;
                    //edit_entity.SENDORG = entity.SENDORG;
                    edit_entity.PK_RECEIVEORG = entity.PK_RECEIVEORG;
                    edit_entity.RECEIVEORG = entity.RECEIVEORG;
                    edit_entity.PK_CARSID = entity.PK_CARSID;
                    edit_entity.CARS = entity.CARS;
                    edit_entity.PK_SENDSTORE = entity.PK_SENDSTORE;
                    edit_entity.SENDSTORE = entity.SENDSTORE;
                    edit_entity.DEF1 = entity.DEF1;
                    edit_entity.PK_RECEIVESTORE = entity.PK_RECEIVESTORE;
                    edit_entity.RECEIVESTORE = entity.RECEIVESTORE;
                    edit_entity.GROSS = entity.GROSS;
                    edit_entity.TARE = entity.TARE;
                    edit_entity.SUTTLE = entity.SUTTLE;
                    edit_entity.PK_MATERIAL = entity.PK_MATERIAL;
                    edit_entity.MATERIAL = entity.MATERIAL;
                    edit_entity.PK_MATERIALKIND = entity.PK_MATERIALKIND;
                    edit_entity.MATERIALKIND = entity.MATERIALKIND;
                    edit_entity.BATHNO = entity.BATHNO;
                    edit_entity.PDAKZ1 = entity.PDAKZ1;
                    edit_entity.DEF4 = entity.DEF4;
                    edit_entity.DEF3 = entity.DEF3;
                    edit_entity.STORE = entity.STORE;
                    edit_entity.DEF15 = entity.DEF15;
                    edit_entity.DEF16 = entity.DEF16;
                    edit_entity.CREATEDATE = entity.CREATEDATE;
                    //edit_entity.LIGHTDATE = entity.LIGHTDATE;
                    edit_entity.WEIGHTDATE = entity.WEIGHTDATE;
                    edit_entity.OUTGBUSER = entity.OUTGBUSER;
                    edit_entity.INGBUSER = entity.INGBUSER;
                    edit_entity.ISEDIT = 1;
                    edit_entity.TYPE = entity.TYPE;
                    edit_entity.MEMO = entity.MEMO;
                    edit_entity.ISJS =entity.ISJS;
                    edit_entity.PDAKZ1 = entity.PDAKZ1;
                    edit_entity.PDAKZ2 = entity.PDAKZ2;
                    edit_entity.YFPIECE = entity.YFPIECE;
                    database.Update(edit_entity, isOpenTrans);

                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from WB_WEIGHT_XB where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    entity.LASTDATE = entity.WEIGHTDATE;
                    entity.INGBUSER = ManageProvider.Provider.Current().UserName;// "磅房管理员";
                    database.Insert(entity, isOpenTrans);
                }
                if (entity.TYPE == "3")
                {
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Append(string.Format("update APP_HANDORDER set PK_LEADERDEPA='{0}' where ID='{1}'", entity.SUTTLE, entity.PK_ORDER_B));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }
                database.Commit();
                string msg = string.Format("{0}车号小磅计量单{1}毛{2}、皮{3},上料成功", entity.CARS, entity.BILLNO, entity.GROSS, entity.TARE);
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
            WB_WEIGHT_XB entity = repositoryfactory.Repository().FindEntity(KeyValue);
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
                database.Delete("WB_WEIGHT_XB", "BILLNO", KeyValue);
                database.Commit();
                string msg = string.Format("{0}车号工序计量单物料{1}毛{2}、皮{3},删除成功", entity.CARS, entity.MATERIAL, entity.GROSS, entity.TARE);
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
            WB_WEIGHT_XB entity = repositoryfactory.Repository().FindEntity(KeyValue);
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

                string msg = string.Format("{0}车号上料计量单{1}毛{2}、皮{3},作废成功", entity.CARS, entity.BILLNO, entity.GROSS, entity.TARE);
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
            WB_WEIGHT_XB entity = repositoryfactory.Repository().FindEntity(KeyValue);
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

                string msg = string.Format("{0}车号上料计量单{1}毛{2}、皮{3},作废成功", entity.CARS, entity.BILLNO, entity.GROSS, entity.TARE);
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
            WB_WEIGHT_XB entity = repositoryfactory.Repository().FindEntity(KeyValue);
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
        /// MES上传
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult HandOrder(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证
                WB_WEIGHT_XB entity = database.FindEntity<WB_WEIGHT_XB>(KeyValue);
                if (entity.UPLOAD == "1")
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计单据已上传！" }.ToString());
                }
                WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient("SAPWebServiceSoap");
                WEBSERVICES_SAP.RETURN_SAP result = new WEBSERVICES_SAP.RETURN_SAP();
                result = sap.WRZS_MESSLWEIGHTZS(KeyValue);//上料上传MES 
                JsonMessage jsonmeg = null;
                if (result.STATUS == "S")
                {
                    string Message = "上传成功。";
                    jsonmeg = new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}:{1}", Message, result.REMSG) };
                }
                else
                {
                    jsonmeg = new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", result.REMSG) };
                }
                return Content(jsonmeg.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }

        /// <summary>
        /// MES上传
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult HandOrderMES(string SITE, string ITEM, string SHIFT, string STORAGELOCATION, string BATCH, string ITEM_MARK, string StartTime, string EndTime)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证
                WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient("SAPWebServiceSoap");
                WEBSERVICES_SAP.RETURN_SAP result = new WEBSERVICES_SAP.RETURN_SAP();
                result = sap.WRZS_MESDJWEIGHTZS("", SITE, StartTime, EndTime, STORAGELOCATION, ITEM, SHIFT, ITEM_MARK);

                JsonMessage jsonmeg = null;
                if (result.STATUS == "S")
                {
                    string Message = "上传成功。";
                    jsonmeg = new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}:{1}", Message, result.REMSG) };
                }
                else
                {
                    jsonmeg = new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", result.REMSG) };
                }
                return Content(jsonmeg.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }

        

        /// <summary>
        /// 小磅移库
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult HandOrder1(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证
                WB_WEIGHT_XB entity = database.FindEntity< WB_WEIGHT_XB>(KeyValue);
                if (entity.UPLOAD1=="1")
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计单据已移库！" }.ToString());
                }

                WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient("SAPWebServiceSoap");
                WEBSERVICES_SAP.RETURN_SAP result = new WEBSERVICES_SAP.RETURN_SAP();
                result = sap.WRZS_SECONDXBWEIGHTZS("", KeyValue);//小磅移库 
                JsonMessage jsonmeg = null;
                if (result.STATUS == "S")
                {
                    string Message = "上传成功。";
                    jsonmeg = new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}:{1}", Message, result.REMSG) };
                }
                else
                {
                    jsonmeg = new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", result.REMSG) };
                }
                return Content(jsonmeg.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }


        [LoginAuthorize]
        public ActionResult UNHandOrder(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "取消上传成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update WB_WEIGHT_XB set UPLOAD=0,PZBILLNO='',PZDATE='' where BILLNO='{0}'", KeyValue));
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
        [LoginAuthorize]
        public ActionResult UNHandOrder1(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "取消移库成功。";
                WB_WEIGHT_XB entity = database.FindEntity<WB_WEIGHT_XB>(KeyValue);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update WB_WEIGHT_XB set UPLOAD1=0,PZBILLNO1='',PZDATE1='' where BILLNO='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                if (entity.TYPE == "3")
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update APP_HANDORDER set UPLOAD1=0,PZBILLNO1='',PZDATE1='' where ID='{0}'", entity.PK_ORDER_B));
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
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            VWB_WEIGHT_XB entity = database.FindEntity<VWB_WEIGHT_XB>(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        [HttpPost]
        public ActionResult SetFormControl1(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            VWB_WEIGHT_XBCB entity = database.FindEntity<VWB_WEIGHT_XBCB>(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
        [HttpPost]
        public ActionResult SetFormControl2(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            VWB_WEIGHT_XBYK entity = database.FindEntity<VWB_WEIGHT_XBYK>(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
        [HttpPost]
        public ActionResult SetFormControl3(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            VWB_WEIGHT_XB1 entity = database.FindEntity<VWB_WEIGHT_XB1>(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
