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
using EOS.WebApp.WRZS_QKLINVALID;

namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    public class PCBillSOFController : PublicController<DP_POCARSORDER>
    {
        string ModuleId = "";

        DP_POCARSORDERBll orderbll = new DP_POCARSORDERBll();
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
        public ActionResult PCBillSOFForm()
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
        public ActionResult PCBillSOF_FORM()
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
        public ActionResult PCBillSOF_FORM1()
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
        public ActionResult PCBillSOF_FORM_BZ()
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

        //列表
        [LoginAuthorize]
        public ActionResult PCBillSOFIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCBillSOFIndex1()
        {
            return View();
        }
        //明细
        [LoginAuthorize]
        public ActionResult PCBillSOFDetail()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCBillSOFQuery()
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


        //质检汽运
        [LoginAuthorize]
        public ActionResult GetZJOrderList(string BillNo, string PK_ORG, string Supply, string Material, string CarsName, string TRAFFICCOMPANY, string PK_ORDER, string WEIGHTNO, string matclass, string istype, string DATETYPE, string DEF1,string TDBILLNO, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VZJQYBll vorderbll = new VZJQYBll();
                List<VZJQY> ListData = vorderbll.GetOrderList(BillNo, PK_ORG, Supply, Material, CarsName, TRAFFICCOMPANY, PK_ORDER, WEIGHTNO, matclass, istype, DATETYPE, DEF1, TDBILLNO, StartTime, EndTime, jqgridparam);
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

        //质检汽运木浆
        public ActionResult GetZJOrderList1(string BillNo, string PK_ORG, string Supply, string Material, string CarsName, string TRAFFICCOMPANY, string PK_STORE, string PK_ORDER, string WEIGHTNO, string matclass, string istype, string DATETYPE, string DEF1, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VZJQYMJBll vorderbll = new VZJQYMJBll();
                List<VZJQYMJ> ListData = vorderbll.GetOrderList(BillNo, PK_ORG, Supply, Material, CarsName, TRAFFICCOMPANY, PK_STORE, PK_ORDER, WEIGHTNO, matclass, istype, DATETYPE, DEF1, StartTime, EndTime, jqgridparam);
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
        /// <param name="BillNo">制单编号</param>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetOrderList(string BillNo, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<DP_POCARSORDER> ListData = orderbll.GetOrderList(BillNo, StartTime, EndTime, jqgridparam);
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

        //汽运
        [LoginAuthorize]
        public ActionResult GetQYOrderList(string BillNo, string VBillCode, string PK_ORG, string PK_DEPT, string Supply, string Material, string matclass, string Cars, string PSDNO, string ISUSE, string istype, string CREATEUSER, string DEF1, string TRAFFICCOMPANY, string BILLFROM, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_POCARSORDERBll vorderbll = new VDP_POCARSORDERBll();
                List<VDP_POCARSORDER> ListData = vorderbll.GetOrderList(BillNo, VBillCode, PK_ORG, PK_DEPT, Supply, Material, matclass, Cars, PSDNO, "1", ISUSE, istype, CREATEUSER, DEF1, TRAFFICCOMPANY, BILLFROM, StartTime, EndTime, jqgridparam);
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


        //汽运
        [LoginAuthorize]
        public ActionResult GetQYOrderList1(string BILLNO, string VBILLCODE, string PK_ORDER, string MATBATCHNO, string ZJBILLNO, string PZBILLNO, string PK_ORG, string Supply, string Material, string MATERIALTYPE, string Cars, string Drivers, string PSDNO, string CQDEF1, string ISUSE, string UPLOAD, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VWB_WEIGHT_FINISHSHBll vorderbll = new VWB_WEIGHT_FINISHSHBll();
                List<VWB_WEIGHT_FINISHSH> ListData = vorderbll.GetOrderList(BILLNO, VBILLCODE, PK_ORDER, MATBATCHNO, ZJBILLNO, PZBILLNO, PK_ORG, Supply, Material, MATERIALTYPE, Cars, Drivers, PSDNO, CQDEF1, ISUSE, UPLOAD, StartTime, EndTime, jqgridparam);
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

        //汽运明细
        [LoginAuthorize]
        public ActionResult GetSQYOrderList(string BillNo, string VBillCode, string PK_ORG, string Supply, string Material, string Cars, string ISUSE, string ISTYPE, string BILLSOFTYPE, string DEF1, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_POCARSORDER_DTLBll vorderbll = new VDP_POCARSORDER_DTLBll();
                List<VDP_POCARSORDER_DTL> ListData = vorderbll.GetOrderList(BillNo, VBillCode, PK_ORG, Supply, Material, Cars, "", ISUSE, ISTYPE, BILLSOFTYPE, DEF1, StartTime, EndTime, jqgridparam);
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
        /// 订单明细列表（返回Json）
        /// </summary>
        /// <param name="POOrderId">订单主键</param>
        /// <returns></returns>
        public ActionResult GetOrderEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetOrderEntryList1(KeyValue),
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
        [LoginAuthorize]
        public ActionResult SubmitOrderForm(string KeyValue, DP_POCARSORDER entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证
            //车辆验证
            JsonMessage jsonmsg = CHECK_CARS_ORDER(entity.CARS, 1, entity.ID, entity.DEF1);
            if (jsonmsg != null)
            {
                return Content(jsonmsg.ToString());
            }
            //身份证验证
            jsonmsg = CHECK_CARDNUMBER_ORDER(entity.PSDNO, 1, entity.ID, entity.DEF1);
            if (jsonmsg != null)
            {
                return Content(jsonmsg.ToString());
            }
            #endregion

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<DP_POCARSORDER_DTL> OrderEntryList = OrderEntryJson.JonsToList<DP_POCARSORDER_DTL>();
                int num = OrderEntryList.Where(p => !p.VBILLCODE.IsNull()).ToList().Count;
                if (num > 1)
                {
                    entity.ISMULTI = "1";
                }
                else
                {
                    entity.ISMULTI = "0";
                }
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    database.Delete<DP_POCARSORDER_DTL>("PK_DP_POCARSORDER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);

                    database.Update(entity, isOpenTrans);
                    EDIT_CARSORDER(entity.CARS, entity.CARSNAME, entity.RFID, entity.DRIVERS, entity.DRIVERSNAME, entity.PSDNO, "", "", entity.ID, database, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from DP_POCARSORDER where  ID='{0}'", entity.ID);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.ID = this.BillCode();
                    }
                    entity.Create();

                    database.Insert(entity, isOpenTrans);
                    ADD_CARSORDER(entity.CARS, entity.CARSNAME, entity.CARDNO, entity.DRIVERS, entity.DRIVERSNAME, entity.PSDNO, "", "", entity.ID, 1, 0, database, isOpenTrans);
                }
                if (entity.DEF11 > 0)
                {
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Clear();
                    strSql.Append(string.Format("update BD_CARS set TGGWEIGHT='{0}'where ID='{1}'", entity.DEF11, entity.CARS));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }

                //处理明细
                int index = 1;
                foreach (DP_POCARSORDER_DTL orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.VBILLCODE))
                    {
                        orderentry.Create();
                        orderentry.ID = string.Format("{0}0{1}", entity.ID, index);
                        orderentry.PK_DP_POCARSORDER = entity.ID;
                        orderentry.BILLSOFTYPE = entity.BILLSOFTYPE;
                        orderentry.CHARGE = orderentry.AMOUNT * orderentry.PRICE;
                        database.Insert(orderentry, isOpenTrans);
                        index++;
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运到货单号:{0}，车辆:{1}，新增成功。", entity.ID, entity.CARSNAME);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
        public ActionResult SubmitOrderForm1(string KeyValue, DP_POCARSORDER entity, string OrderEntryJson, string ModuleId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<DP_POCARSORDER_DTL> OrderEntryList = OrderEntryJson.JonsToList<DP_POCARSORDER_DTL>();
                int num = OrderEntryList.Where(p => !p.VBILLCODE.IsNull()).ToList().Count;
                if (num > 1)
                {
                    entity.ISMULTI = "1";
                }
                else
                {
                    entity.ISMULTI = "0";
                }
                //处理主头
                string Message = "编辑成功。";

                entity.Modify(KeyValue);
                database.Update(entity, isOpenTrans);

                //处理明细
                int index = 1;
                DP_POCARSORDER_DTL orderentry1 = null;
                foreach (DP_POCARSORDER_DTL orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.VBILLCODE))
                    {
                        if (!string.IsNullOrEmpty(orderentry.ID))
                        {
                            orderentry1 = database.FindEntityByWhere<DP_POCARSORDER_DTL>(string.Format(" AND ID='{0}'", orderentry.ID));
                            orderentry1.PK_ORDER = orderentry.PK_ORDER;
                            orderentry1.PK_ORDER_B = orderentry.PK_ORDER_B;
                            orderentry1.VBILLCODE = orderentry.VBILLCODE;
                            orderentry1.PK_SUPPLIER = orderentry.PK_SUPPLIER;
                            orderentry1.SUPPLIERNAME = orderentry.SUPPLIERNAME;
                            orderentry1.PK_MATERIAL = orderentry.PK_MATERIAL;
                            orderentry1.MATERIALNAME = orderentry.MATERIALNAME;
                            orderentry1.MATERIALSPEC = orderentry.MATERIALSPEC;
                            orderentry1.MATERIALCODE = orderentry.MATERIALCODE;
                            orderentry1.PK_ORG = orderentry.PK_ORG;
                            orderentry1.ORGNAME = orderentry.ORGNAME;
                            orderentry1.PK_DEPT = orderentry.PK_DEPT;
                            orderentry1.DEPTNAME = orderentry.DEPTNAME;
                            orderentry1.DBILLDATE = orderentry.DBILLDATE;
                            orderentry1.AMOUNT = orderentry.AMOUNT;
                            orderentry1.MDSUTTLE = orderentry.MDSUTTLE;
                            orderentry1.MDAMOUNT = orderentry.MDAMOUNT;
                            orderentry1.MDMINAMOUNT = orderentry.MDMINAMOUNT;
                            orderentry1.MINAMOUNT = orderentry.MINAMOUNT;
                            orderentry1.MJAMOUNT = orderentry.MJAMOUNT;
                            orderentry1.MATERIALLEVEL = orderentry.MATERIALLEVEL;
                            orderentry1.PK_SENDADDRESS = entity.PK_SENDADDRESS;
                            orderentry1.SENDADDRESS = entity.SENDADDRESS;
                            orderentry1.DEF5 = orderentry.DEF5;
                            orderentry1.ZJBILLNO = orderentry.ZJBILLNO;
                            orderentry1.PZBILLNO = orderentry.PZBILLNO;
                            orderentry1.MEMO = orderentry.MEMO;
                            database.Update(orderentry1, isOpenTrans);
                        }
                        else
                        {
                            orderentry.Create();
                            orderentry.PK_DP_POCARSORDER = entity.ID;
                            orderentry.BILLSOFTYPE = entity.BILLSOFTYPE;
                            orderentry.CHARGE = orderentry.AMOUNT * orderentry.PRICE;
                            orderentry.ID = string.Format("{0}0{1}", entity.ID, index);
                            database.Insert(orderentry, isOpenTrans);
                        }
                        index++;
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运到货单号:{0}，车辆:{1}，编辑成功。", entity.ID, entity.CARSNAME);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
        /// 提交表单 补增类型  不操作中间表
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SubmitOrderForm_BZ(string KeyValue, DP_POCARSORDER entity, string OrderEntryJson, string ModuleId)
        {

            #region 验证

            #endregion

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<DP_POCARSORDER_DTL> OrderEntryList = OrderEntryJson.JonsToList<DP_POCARSORDER_DTL>();
                int num = OrderEntryList.Where(p => !p.VBILLCODE.IsNull()).ToList().Count;
                if (num > 1)
                {
                    entity.ISMULTI = "1";
                }
                else
                {
                    entity.ISMULTI = "0";
                }
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    database.Delete<DP_POCARSORDER_DTL>("PK_DP_POCARSORDER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);

                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from DP_POCARSORDER where  ID='{0}'", entity.ID);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.ID = this.BillCode();
                    }
                    entity.Create();
                    entity.BILLSOFTYPE = "BZ";

                    database.Insert(entity, isOpenTrans);
                }
                if (entity.DEF11 > 0)
                {
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Clear();
                    strSql.Append(string.Format("update BD_CARS set TGGWEIGHT='{0}'where ID='{1}'", entity.DEF11, entity.CARS));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }

                //处理明细
                int index = 1;
                foreach (DP_POCARSORDER_DTL orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.VBILLCODE))
                    {
                        orderentry.Create();
                        orderentry.ID = string.Format("{0}0{1}", entity.ID, index);
                        orderentry.PK_DP_POCARSORDER = entity.ID;
                        orderentry.BILLSOFTYPE = entity.BILLSOFTYPE;
                        orderentry.CHARGE = orderentry.AMOUNT * orderentry.PRICE;
                        database.Insert(orderentry, isOpenTrans);
                        index++;
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运到货单号:{0}，车辆:{1}，新增成功。", entity.ID, entity.CARSNAME);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
            string sql = String.Format("select count(1) from DP_POCARSORDER where ID='{0}' and isuse!=4", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为作废的到货单能删除操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("DP_POCARSORDER", "ID", KeyValue);
                database.Delete("DP_POCARSORDER_DTL", "PK_DP_POCARSORDER", KeyValue);
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
        //作废
        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue)
        {
            //验证
            DP_POCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "4")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单已作废！" }.ToString());
            }
            else if (entity.ISUSE != "1" && entity.ISUSE != "5")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为未入厂的到货单能作废操作！" }.ToString());
            }
            string sql = String.Format("SELECT count(1) NUMS FROM VZJQY WHERE  ISCY='1' and ID='{0}'", entity.ID);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}已采样不能作废操作！", entity.CARSNAME, entity.ID) }.ToString());
            }
            sql = String.Format("SELECT count(1) NUMS FROM WB_WEIGHT WHERE  finish=0 and PK_ORDER='{0}'", entity.ID);
            res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}已一次计量不能作废操作！", entity.CARSNAME, entity.ID) }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //区块链同步作废  fhh/myt 220915 新增区块链数据作废同步至区块链
                string msg = "";
                if (entity.DATATYPE == "3")
                {
                    WRZS_QKLINVALID.SAPWebServiceSoapClient QKLINVALID = new WRZS_QKLINVALID.SAPWebServiceSoapClient();
                    WRZS_QKLINVALID.RETURN_QKL ret = new RETURN_QKL();
                    ret = QKLINVALID.WRZS_QKLINVALIDZS(entity.ID);
                    msg = ret.MSG;
                }
                //fhh/myt 220915 新增区块链数据作废同步至区块链
                string Account = ManageProvider.Provider.Current().Account;
                string Message = "作废成功。";
                entity.ISUSE = "4";
                entity.MEMO = $"业务平台作废:作废人{Account}、时间{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                database.Update(entity, isOpenTrans);

                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //明细
                strSql.Append(string.Format("update DP_POCARSORDER_DTL set ISUSE='4',MEMO='业务平台作废' where PK_DP_POCARSORDER='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                //门禁出厂
                strSql.Clear();
                strSql.Append(string.Format("update wb_weight_task set finish='1',status='出厂' where PK_ORDER='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                DEL_CARSORDERS(KeyValue, database, isOpenTrans);
                database.Commit();

                string message = string.Format("汽运到货单:{0}，车辆{1}业务平台作废成功。{2}", entity.ID, entity.CARSNAME, msg);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
            DP_POCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "6")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单已出厂！" }.ToString());
            }

            string sql = String.Format("select count(1) from DP_POCARSORDER_DTL where isuse<>2 and PK_DP_POCARSORDER='{0}'", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("该到货单有未二次计量的订单明细") }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "强制出厂成功。";
                entity.ISUSE = "6";
                entity.ISQZ = 1;
                entity.QZDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entity.QZUSER = ManageProvider.Provider.Current().UserName;
                entity.MEMO = "强制出厂";
                database.Update(entity, isOpenTrans);
                DEL_CARSORDERS(KeyValue, database, isOpenTrans);

                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //门禁出厂
                strSql.Append(string.Format("update wb_weight_task set finish='1',status='出厂' where PK_ORDER='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                //过磅出厂
                strSql.Clear();
                strSql.Append(string.Format("update wb_weight set finish='1',status='完成' where PK_ORDER='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("update DP_POCARSORDER_DTL set isuse='2' where PK_DP_POCARSORDER='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运到货单:{0}，车辆{1}强制出厂成功。", entity.ID, entity.CARS);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        //完结
        [LoginAuthorize]
        public ActionResult OutOrderDetail(string PK_ORDER, string PK_ORDER_B)
        {
            //验证
            DP_POCARSORDER entity = repositoryfactory.Repository().FindEntity(PK_ORDER);
            if (entity.ISUSE == "6")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单已出厂！" }.ToString());
            }
            if (entity.ISUSE == "4")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单已作废！" }.ToString());
            }
            //派车明细
            DP_POCARSORDER_DTL entity_b = DataFactory.Database().FindEntityByWhere<DP_POCARSORDER_DTL>(string.Format(" AND ID='{0}'", PK_ORDER_B));
            if (entity_b.ISUSE == "4")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单明细已作废！" }.ToString());
            }
            //if (entity_b.ISUSE != "1")
            //{
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单明细已计量！" }.ToString());
            //}


            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "强制完结成功。";
                //派车主表
                string sql = String.Format("select count(1) from DP_POCARSORDER_DTL where isuse=1 and PK_DP_POCARSORDER='{0}' and id!='{1}' ", PK_ORDER, PK_ORDER_B);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                }
                else
                {
                    entity.ISUSE = "6";
                    entity.MEMO = "强制完结";
                    database.Update(entity, isOpenTrans);
                    //中间表
                    DEL_CARSORDERS(PK_ORDER, database, isOpenTrans);
                    //门禁出厂
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Append(string.Format("update wb_weight set finish='1',status='出厂' where PK_ORDER_B='{0}'", PK_ORDER_B));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    strSql.Clear();
                    strSql.Append(string.Format("update wb_weight_task set finish='1',status='出厂' where PK_ORDER='{0}'", PK_ORDER));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }
                //派车明细
                entity_b.ISCHECK = 2;
                entity_b.ISUSE = "2";
                entity_b.MEMO = "强制完结";
                database.Update(entity_b, isOpenTrans);
                database.Commit();

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运到货单:{0}，车辆{1}强制完结成功。", entity.ID, entity.CARS);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
            DP_POCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单是审批状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                entity.STATUS = "1";
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

        //反审
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            DP_POCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单是未审批状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "反审成功。";
                entity.STATUS = "0";
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
        /// 签到
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult InQDOrder(string KeyValue)
        {
            //验证
            DP_POCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.DEF12 == 1)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单是已签到状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "签到成功。";
                entity.DEF12 = 1;
                database.Update(entity, isOpenTrans);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //明细
                strSql.Append(string.Format("update DP_POCARSORDER_DTL set DEF12=1,DEF9='{0}' where PK_DP_POCARSORDER='{1}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                //string sql = "";
                //List<DP_POCARSORDER_DTL> list = orderbll.GetOrderEntryList(KeyValue);
                //sql = String.Format("select count(1)+1 CARSNUM from Sort_Order");
                //int VINDEX = DataFactory.Database().FindCountBySql(sql);

                //foreach (DP_POCARSORDER_DTL model in list)
                //{
                //    strSql.Clear();
                //    sql = string.Format("insert into Sort_Order(ID,BILLTYPE,ORDERSTATUS,ORDERTIME,CALLTIME,PK_ASSIGNPLAN,PK_CARS,CARSNAME,PK_STORE,STORENAME,PK_MATERIAL,MATERIALNAME,MATSPEC,PK_DRIVER,DRIVERNAME,VINDEX,SENDTIME,CALLCOUNT) values(");
                //    sql += string.Format("'{0}',{1},{2},'{3}','',", CommonHelper.GetGuid, 0, 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //    sql += string.Format("'{0}','{1}','{2}','{3}','{4}',", entity.ID, entity.CARS, entity.CARSNAME, "","");
                //    sql += string.Format("'{0}','{1}','{2}','{3}','{4}',", model.PK_MATERIAL, model.MATERIALNAME, model.MATERIALSPEC, entity.DRIVERS, entity.DRIVERSNAME);
                //    sql += string.Format("{0},'',0", VINDEX);
                //    sql += string.Format(")");
                //    strSql.Append(sql);
                //    database.ExecuteBySql(strSql, isOpenTrans);
                //}

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运到货单号:{0}，{1}。", KeyValue, Message);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
        public ActionResult HandOrder(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient("SAPWebServiceSoap");
                WEBSERVICES_SAP.RETURN_SAP result = new WEBSERVICES_SAP.RETURN_SAP();
                result = sap.WRZS_HGBZWFINISTWEIGHTZS("", KeyValue);//化工一次计量后上传过账调用 
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
                result = sap.WRZS_SECONDWEIGHTZS("", KeyValue);//化工二次计量后上传过账调用 
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
        /// 【到货单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            DP_POCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }



    }
}
