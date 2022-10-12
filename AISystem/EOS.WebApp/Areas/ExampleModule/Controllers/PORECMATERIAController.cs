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
using System.Configuration;
using System.Data;

namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    /// <summary>
    /// 厂内工序转序
    /// </summary>
    public class PORECMATERIAController : PublicController<APP_HANDORDER>
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
        public ActionResult POBillRECFForm()//POBillCOFForm
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                //ViewBag.BillNo = this.BillCode();    
                ViewBag.CreateUserId = ManageProvider.Provider.Current().Account;
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }

        [LoginAuthorize]
        public ActionResult POBillRECFForm1()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            ViewBag.CreateUserId = ManageProvider.Provider.Current().Account;
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        [LoginAuthorize]
        public ActionResult POBillRECFFormMJ()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            ViewBag.CreateUserId = ManageProvider.Provider.Current().Account;
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        [LoginAuthorize]
        public ActionResult POBillRECFFormJK()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            ViewBag.CreateUserId = ManageProvider.Provider.Current().Account;
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        //列表页面
        [LoginAuthorize]
        public ActionResult POBillRECFIndex()
        {
            return View();
        }
        //列表页面
        [LoginAuthorize]
        public ActionResult POBillRECFIndex1()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult POBillRECFIndexMJ()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult POBillRECFIndexJK()
        {
            return View();
        }
        //明细页面
        [LoginAuthorize]
        public ActionResult POBillRECFDetail()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult POBillRECFDetail1()
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
        [LoginAuthorize]
        public ActionResult POSELECTBillRECF1()
        {
            return View();
        }
        /// <summary>
        /// 木浆选择收货
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult POSELECTBillRECF2()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SELECTPOORDERS()
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
        public ActionResult GetOrderList(string TaskNo, string BILLNO, string User, string CQDEF1, string VBILLCODE, string ISTYPE, string ISJL, string TYPE, string BATCHNO, string PZBILLNO, string DEPARTMENT, string PK_SENDORG, string PK_SENDADDRESS, string PK_STORE, string UPLOAD, string ISERROR, string ISZJD, string PK_MARBASCLASS,string StartTime, string EndTime, JqGridParam jqgridparam)
        {

            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDORDERBll vorderbll = new VAPP_HANDORDERBll();
                List<VAPP_HANDORDER> ListData = vorderbll.GetOrderList(TaskNo, BILLNO, User, CQDEF1, VBILLCODE, ISTYPE, ISJL, StartTime, EndTime, TYPE, "1", BATCHNO, PZBILLNO, DEPARTMENT, PK_SENDORG, PK_SENDADDRESS, PK_STORE, UPLOAD, ISERROR, ISZJD, PK_MARBASCLASS,jqgridparam);
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


        public ActionResult GetOrderList1(string TaskNo, string BILLNO, string User, string CQDEF1, string VBILLCODE, string ISTYPE, string ISJL, string TYPE, string BATCHNO, string PZBILLNO, string DEPARTMENT, string PK_SENDORG, string PK_SENDADDRESS, string PK_STORE, string UPLOAD, string ISERROR, string ISZJD,string TDBILLNO,string MATERIAL, string StartTime, string EndTime, JqGridParam jqgridparam)
        {

            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDORDER1Bll vorderbll = new VAPP_HANDORDER1Bll();
                List<VAPP_HANDORDER1> ListData = vorderbll.GetOrderList(TaskNo, BILLNO, User, CQDEF1, VBILLCODE, ISTYPE, ISJL, StartTime, EndTime, TYPE, "1", BATCHNO, PZBILLNO, DEPARTMENT, PK_SENDORG, PK_SENDADDRESS, PK_STORE, UPLOAD, ISERROR, ISZJD, TDBILLNO, MATERIAL, jqgridparam);
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


        public ActionResult GetOrderList2(string TaskNo, string BILLNO, string User, string CQDEF1, string VBILLCODE, string ISTYPE, string ISJL, string TYPE, string BATCHNO, string PZBILLNO, string DEPARTMENT, string PK_SENDORG, string PK_SENDADDRESS, string PK_STORE, string UPLOAD, string ISERROR, string ISZJD,string MATERIALBARCODE, string TDBILLNO, string MATERIAL, string StartTime, string EndTime, JqGridParam jqgridparam)
        {

            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDORDER2Bll vorderbll = new VAPP_HANDORDER2Bll();
                List<VAPP_HANDORDER2> ListData = vorderbll.GetOrderList(TaskNo, BILLNO, User, CQDEF1, VBILLCODE, ISTYPE, ISJL, StartTime, EndTime, TYPE, "1", BATCHNO, PZBILLNO, DEPARTMENT, PK_SENDORG, PK_SENDADDRESS, PK_STORE, UPLOAD, ISERROR, ISZJD, MATERIALBARCODE, TDBILLNO, MATERIAL, jqgridparam);
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
        /// 查询（返回Json）
        /// </summary>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetWbOrderList(string BillNo, string PK_ORG, string Supply, string Material, string Cars, string CarsName, string DTYPE, string DATETYPE, string ISTYPE, string ISJL,string MATERIALBARCODE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VWB_WEIGHT_SHBll vorderbll = new VWB_WEIGHT_SHBll();
                List<VWB_WEIGHT_SH> ListData = vorderbll.GetWbOrderList(BillNo, PK_ORG, Supply, Material, Cars, CarsName, DTYPE, DATETYPE, ISTYPE, ISJL, MATERIALBARCODE, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(APP_HANDORDER entity, string KeyValue, string ModuleId)
        {
            VDP_POCARSORDER_DTLBll pcbll = new VDP_POCARSORDER_DTLBll();
            VDP_POCARSORDER_DTL pcenity = pcbll.GetEntity(entity.ID);
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增登记成功。" : "编辑登记成功。";
                #region 验证
                string sql = String.Format("select count(1) from APP_HANDORDER  where  ID='{0}' and STATUS=2", entity.ID);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "已经走退货流程，不允许收货登记：" + entity.BILLNO }.ToString());
                }
                sql = String.Format("select count(1) from APP_HANDORDER  where  ID='{0}' and STATUS=1", entity.ID);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "此单已经收货确认：" + entity.BILLNO }.ToString());
                }

                sql = String.Format("select count(1) from APP_HANDORDER  where ID='{0}'  and STATUS=0", entity.ID);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "此单已收货登记，请勿重复收货登记：" + entity.BILLNO }.ToString());
                }


                #endregion
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    entity.STATUS = "0";
                    entity.TYPE = "0";
                    if (string.IsNullOrEmpty(entity.MEMO))
                    {
                        entity.MEMO = "";
                    }
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    entity.STATUS = "0";
                    entity.TYPE = "0";
                    if (string.IsNullOrEmpty(entity.BILLNO))
                    {
                        entity.BILLNO = entity.ID;
                    }

                    //System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    //strSql.Clear();
                    //strSql.Append(string.Format("UPDATE DP_POCARSORDER_DTL set ISSH=1,DEF10='{0}' WHERE ID='{1}'", entity.BILLNO, entity.ID));
                    //database.ExecuteBySql(strSql, isOpenTrans);
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


        [HttpPost]
        public ActionResult SubmitOrderForm1(APP_HANDORDER entity, string KeyValue, string ModuleId)
        {
            VDP_POCARSORDER_DTLBll pcbll = new VDP_POCARSORDER_DTLBll();
            VDP_POCARSORDER_DTL pcenity = pcbll.GetEntity(entity.ID);
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    #region 收货
                    APP_HANDORDER edit_entity = database.FindEntity<APP_HANDORDER>(KeyValue);
                    edit_entity.Modify(KeyValue);
                    edit_entity.PK_STORE = entity.PK_STORE;
                    edit_entity.STORE = entity.STORE;
                    edit_entity.DEPARTMENT = entity.DEPARTMENT;
                    edit_entity.DEF9 = entity.DEF9;
                    edit_entity.PK_LEADERDEPA = entity.PK_LEADERDEPA;
                    edit_entity.PDAKZ5 = entity.PDAKZ5;
                    edit_entity.PDAKZ4 = entity.PDAKZ4;
                    edit_entity.PDAKZ3 = entity.PDAKZ3;
                    edit_entity.GPMATERIAL = entity.GPMATERIAL;
                    edit_entity.GPMATERIALNAME = entity.GPMATERIALNAME;
                    edit_entity.GPORDERNO = entity.GPORDERNO;
                    edit_entity.JZXSUTTLE = entity.JZXSUTTLE;
                    edit_entity.JZXSUTTLE1 = entity.JZXSUTTLE1;
                    edit_entity.PDAKZ2 = entity.PDAKZ2;
                    edit_entity.JSUTTLE1 = entity.JSUTTLE1;
                    edit_entity.PDASSJS = entity.PDASSJS;
                    edit_entity.PDAYF = entity.PDAYF;
                    edit_entity.DEF7 = entity.DEF7;
                    edit_entity.ZJUSER = entity.ZJUSER;
                    edit_entity.ZJTIME = entity.ZJTIME;
                    #endregion

                    if (edit_entity.TYPE == "7")
                    {
                        edit_entity.JSAMOUNT1 = entity.PDAYF;
                        if(!string.IsNullOrEmpty(entity.PDAYF)&& !string.IsNullOrEmpty(entity.JSUTTLE1))
                        {
                            edit_entity.TOTALJSUTTLE = $"{ Convert.ToDecimal(entity.PDAYF) * Convert.ToDecimal(entity.JSUTTLE1)}";
                        }
                    }
                    edit_entity.PK_DEPARTMENT = entity.PK_DEPARTMENT;//验级员

                    edit_entity.LEADERDEPA = entity.LEADERDEPA;//货源地
                    database.Update(edit_entity, isOpenTrans);
                    if (!string.IsNullOrEmpty(edit_entity.BILLNO))
                    {
                        WB_WEIGHT edit_weight = database.FindEntity<WB_WEIGHT>(edit_entity.BILLNO);
                        edit_weight.PK_RECEIVESTORE = entity.PK_STORE;
                        edit_weight.RECEIVESTORE = entity.STORE;
                        edit_weight.PK_STORE = entity.PK_STORE;
                        edit_weight.STORE = entity.STORE;
                        edit_weight.PK_ORDER_B = edit_entity.ID;
                        if (!string.IsNullOrEmpty(entity.PDAKZ2))
                        {
                            edit_weight.PDAKZ2 = entity.PDAKZ2;
                        }
                        edit_weight.PRICE = entity.JZXSUTTLE1;
                        database.Update(edit_weight, isOpenTrans);
                    }

                }
                else
                {
                    entity.Create();
                    entity.BILLFROM = "1";
                    entity.STATUS = "1";
                    //entity.TYPE = "5";
                    database.Insert(entity, isOpenTrans);
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Append(string.Format("UPDATE DP_POCARSORDER_DTL set ISSH=1,DEF10='{0}' WHERE ID='{1}'", entity.BILLNO, entity.ID));
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
        /// 收货确认
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
                string Message = "收货确认成功。";
                #region  验证
                string sql = String.Format("select count(1) from APP_HANDORDER where ID='{0}' and STATUS=2", entity.ID);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "已经走退货流程，不允许收货登记：" + entity.ID }.ToString());
                }

                //sql = String.Format("select count(1) from APP_HANDORDER  where  ID='{0}' and STATUS=1", entity.ID);
                //res = DataFactory.Database().FindCountBySql(sql);
                //if (res > 0)
                //{
                //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "此单已经收货确认：" + entity.ID }.ToString());
                //}

                //sql = String.Format("select count(1) from APP_HANDORDER  where  ID='{0}' and STATUS=0", entity.ID);
                //res = DataFactory.Database().FindCountBySql(sql);
                //if (res == 0)
                //{
                //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "请先收货登记：" + entity.ID }.ToString());
                //}

                #endregion

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    entity.STATUS = "1";
                    entity.TYPE = "0";
                    if (string.IsNullOrEmpty(entity.MEMO))
                    {
                        entity.MEMO = "";
                    }
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    entity.STATUS = "1";
                    entity.TYPE = "0";
                    if (string.IsNullOrEmpty(entity.BILLNO))
                    {
                        entity.BILLNO = entity.ID;
                    }
                    database.Insert(entity, isOpenTrans);
                }
                VDP_POCARSORDER_DTLBll pcbll = new VDP_POCARSORDER_DTLBll();
                VDP_POCARSORDER_DTL pcenity = pcbll.GetEntity(entity.ID);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("UPDATE DP_POCARSORDER_DTL set ISSH=1,DEF10='{0}' WHERE ID='{1}'", entity.BILLNO, entity.ID));
                database.ExecuteBySql(strSql, isOpenTrans);
                strSql.Clear();
                strSql.Append($"UPDATE WB_WEIGHT set PK_STORE='{entity.PK_STORE}',STORE='{entity.STORE}',PK_RECEIVESTORE='{entity.PK_STORE}',RECEIVESTORE='{entity.STORE}',UPLOAD1={entity.ISJS} WHERE PK_ORDER_B='{entity.ID}'");
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
                entity.OPERUSER = ManageProvider.Provider.Current().UserName;
                entity.OPERTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                string sql = String.Format("select count(1) from APP_HANDORDER  where  BILLNO='{0}' and  STATUS!='2'", entity.BILLNO);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "已走收货流程：" + entity.BILLNO + "不允许退货" }.ToString());
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
        ///删除 
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from APP_HANDORDER where ID='{0}' and STATUS=2", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "已退货确认，不能取消收货操作!" }.ToString());
            }
            //
            //sql = String.Format("select count(1) from WB_WEIGHT_B  where FINISH=1 and PK_ORDER_B='{0}'", KeyValue);
            //res = DataFactory.Database().FindCountBySql(sql);
            //if (res > 0)
            //{
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("派车单：{0}已二次计量，不能取消收货操作!", KeyValue) }.ToString());
            //}

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "取消收货成功。";
                database.Delete("APP_HANDORDER", "ID", KeyValue);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update DP_POCARSORDER_DTL SET ISSH=0,DEF10='{0}' where ID='{1}'", string.Empty, KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("单号：{0}业务平台取消收货成功。", KeyValue);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        [LoginAuthorize]
        public ActionResult GetLogEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetLogEntryList(KeyValue),
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
        public ActionResult HandOrder(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient("SAPWebServiceSoap");
                WEBSERVICES_SAP.RETURN_SAP result = new WEBSERVICES_SAP.RETURN_SAP();
                result = sap.WRZS_SECONDWEIGHTZS("", KeyValue);//废纸二次计量后上传过账调用 
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
        public ActionResult HandOrder1(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient("SAPWebServiceSoap");
                WEBSERVICES_SAP.RETURN_SAP result = new WEBSERVICES_SAP.RETURN_SAP();
                result = sap.WRZS_SECONDCBWEIGHTZS("", KeyValue);//废纸二次计量后移库调用 
                JsonMessage jsonmeg = null;
                if (result.STATUS == "S")
                {
                    string Message = "移库成功。";
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
        public ActionResult HandOrder2(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient("SAPWebServiceSoap");
                WEBSERVICES_SAP.RETURN_SAP result = new WEBSERVICES_SAP.RETURN_SAP();
                result = sap.WRZS_QKLSECONDWEIGHTZS("", KeyValue);//废纸二次计量后移库调用 
                JsonMessage jsonmeg = null;
                if (result.STATUS == "S")
                {
                    string Message = "移库成功。";
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
                //APP_HANDORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
                //entity.UPLOAD = "0";
                //database.Update(entity, isOpenTrans);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //strSql.Append(string.Format("update APP_HANDORDER set UPLOAD=0,PZBILLNO='',PZDATE='',UPLOAD1=0,PZBILLNO1='',PZDATE1='' where ID='{0}'", KeyValue));
                strSql.Append(string.Format("update APP_HANDORDER set UPLOAD=0,PZBILLNO='',PZDATE='' where ID='{0}'", KeyValue));
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
                APP_HANDORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);

                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update APP_HANDORDER set UPLOAD1=0,PZBILLNO1='',PZDATE1='' where ID='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                strSql.Clear();
                strSql.Append(string.Format("update WB_WEIGHT_XB set UPLOAD1=0,PZBILLNO1='',PZDATE1='' where PK_ORDER_B='{0}'", entity.ID));
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
        public ActionResult GetOrderPicList(string KeyValue)
        {
            try
            {
                List<WB_CYCHRESULT_PIC> list_new = new List<WB_CYCHRESULT_PIC>();
                List<WB_CYCHRESULT_PIC> list = orderbll.GetOrderPicList(KeyValue);
                string httpurl = ConfigurationManager.AppSettings["httpUrl1"].Trim();
                foreach (WB_CYCHRESULT_PIC model in list)
                {
                    model.PICURL = model.PICURL.TrimStart('"');
                    model.PICURL = model.PICURL.TrimEnd('"');
                    model.PICURL = string.Format("{0}{1}", httpurl, model.PICURL.Replace(@"\", @"/").Replace("bmp", "jpg"));
                    list_new.Add(model);
                }
                var JsonData = new
                {
                    rows = list_new,
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


        [HttpPost]
        public ActionResult SetFormControl1(string KeyValue)
        {
            VAPP_HANDORDERBll overbll = new VAPP_HANDORDERBll();
            VAPP_HANDORDER1 entity = overbll.GetEntity1(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        [HttpPost]
        public ActionResult SetFormControl2(string KeyValue)
        {
            VAPP_HANDORDERBll overbll = new VAPP_HANDORDERBll();
            VAPP_HANDORDER2 entity = overbll.GetEntity2(KeyValue);
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


        public ActionResult BDPrintWeight(string KeyValue)
        {
            string Account = ManageProvider.Provider.Current().Account;
            DataSet dataSet = orderbll.BDPrintWeight(KeyValue, Account);
            return Content(dataSet.GetXml());
        }

        public ActionResult BDPrintWeight1(string KeyValue)
        {
            string Account = ManageProvider.Provider.Current().Account;
            DataSet dataSet = orderbll.BDPrintWeight1(KeyValue, Account);
            return Content(dataSet.GetXml());
        }
    }
}
