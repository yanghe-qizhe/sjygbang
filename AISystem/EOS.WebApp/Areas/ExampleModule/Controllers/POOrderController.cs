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


namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    public class POOrderController : PublicController<NC_PO_ORDER>
    {
        string ModuleId = "";
        NC_PO_ORDERBll orderbll = new NC_PO_ORDERBll();
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

        [LoginAuthorize]
        public ActionResult POOrderIndex()
        {
            return View();
        }

        //分配承运商
        [LoginAuthorize]
        public ActionResult POOrderIndex1()
        {
            return View();
        }
        //散户订单分配
        [LoginAuthorize]
        public ActionResult POOrderIndex2()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult POOrderZJFA()
        {
            return View();
        }


        [LoginAuthorize]
        public ActionResult POORDER_Form()
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
        public ActionResult POORDER_Form1()
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
        public ActionResult POOrderForm()
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
        public ActionResult POOrderForm1()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult POOrderForm2()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult POOrderForm3()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult POORDERDetail()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult POORDERQuery()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult POORDERQuery1()
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
        public ActionResult GetOrderList(string BillNo, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<NC_PO_ORDER> ListData = orderbll.GetOrderList(BillNo, StartTime, EndTime, jqgridparam);
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

        //全部采购订单
        [LoginAuthorize]
        public ActionResult GetPOOrderList(string BillNo, string SOURCEBILLNO, string VCONTRACTCODE, string Supply, string SupplyName, string Material, string PK_DEPT, string DATATYPE, string BILLTYPE, string TRAFFICCOMPANY, string ISFP, string ISFZ, string GROUPUSER, string matclass, string RECCOMPANYID, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VNC_PO_ORDERBll vorderbll = new VNC_PO_ORDERBll();
                List<VNC_PO_ORDER> ListData = vorderbll.GetOrderList(BillNo, SOURCEBILLNO, VCONTRACTCODE, Supply, SupplyName, "", Material, PK_DEPT, DATATYPE, BILLTYPE, TRAFFICCOMPANY, ISFP, ISFZ, GROUPUSER, matclass, RECCOMPANYID, StartTime, EndTime, jqgridparam);
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


        //公路运输订单
        [LoginAuthorize]
        public ActionResult GetQYPOOrderList(string BillNo, string RECCOMPANYID, string Supply, string Material, string matclass, string istype, string iszx, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VNC_PO_ORDER_BBll vorderbll = new VNC_PO_ORDER_BBll();
                List<VNC_PO_ORDER_B> ListData = vorderbll.GetQYOrderList(BillNo, RECCOMPANYID, Supply, "", Material, matclass, istype, iszx, StartTime, EndTime, jqgridparam);
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

        //物料类型订单查询
        [LoginAuthorize]
        public ActionResult GetQYPOOrderList1(string BillNo, string RECCOMPANYID, string Supply, string Material, string matclass, string istype, string isjl, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VNC_PO_ORDER_BBll vorderbll = new VNC_PO_ORDER_BBll();
                List<VNC_PO_ORDER_B> ListData = vorderbll.GetQYOrderList1(BillNo, RECCOMPANYID, Supply, "", Material, matclass, istype, isjl, StartTime, EndTime, jqgridparam);
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


        public ActionResult GetHBZPOOrderList(string BillNo, string RECCOMPANYID, string Supply, string Material, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VNC_PO_ORDER_BBll vorderbll = new VNC_PO_ORDER_BBll();
                List<VNC_PO_ORDER_B> ListData = vorderbll.GetHBZOrderList(BillNo, RECCOMPANYID, Supply, "", Material, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetQYPOOrderList2(string BillNo, string RECCOMPANYID, string Supply, string Material, string matclass, string istype, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VNC_PO_ORDER_BBll vorderbll = new VNC_PO_ORDER_BBll();
                List<VNC_PO_ORDER_B> ListData = vorderbll.GetOrderList(BillNo, RECCOMPANYID, Supply, "", Material, matclass, istype, StartTime, EndTime, jqgridparam);
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
        [LoginAuthorize]
        public ActionResult GetOrderEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetOrderEntryList(KeyValue),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        //承运商分配信息
        [LoginAuthorize]
        public ActionResult GetOrderEntryListC(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetOrderEntryListC(KeyValue),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        //散户用户分配信息
        [LoginAuthorize]
        public ActionResult GetOrderEntryListD(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetOrderEntryListD(KeyValue),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        public ActionResult GetQYOrderEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetQYOrderEntryList(KeyValue),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }


        public ActionResult CHECK_QYPOORDER_NUM(string pk_orders)
        {
            Decimal TotalNum = 0;

            string sql = string.Format("select nvl(sum(YLNUM),0) YLNUM from VNC_PO_ORDER_BQY where PK_ORDER_B in('{0}')", pk_orders);
            DataTable dt = dt = DataFactory.Database().FindTableBySql(sql);
            if (dt.Rows.Count > 0)
            {
                TotalNum = Convert.ToDecimal(dt.Rows[0]["YLNUM"].ToString());
            }
            var JsonData = new
            {
                ylnum = TotalNum
            };
            return Content(JsonData.ToJson());
        }

        public ActionResult CHECK_HYPOORDER_NUM(string pk_orders)
        {
            Decimal TotalNum = 0;

            string sql = string.Format("select nvl(sum(YLNUM),0) YLNUM from VNC_PO_ORDER_BHY where PK_ORDER_B in('{0}')", pk_orders);
            DataTable dt = dt = DataFactory.Database().FindTableBySql(sql);
            if (dt.Rows.Count > 0)
            {
                TotalNum = Convert.ToDecimal(dt.Rows[0]["YLNUM"].ToString());
            }
            var JsonData = new
            {
                ylnum = TotalNum
            };
            return Content(JsonData.ToJson());
        }

        /// <summary>
        /// 【采购订单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            VNC_PO_ORDERBll vorderbll = new VNC_PO_ORDERBll();
            VNC_PO_ORDER entity = vorderbll.SetFormControl(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }



        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm(string KeyValue, NC_PO_ORDER entity, string ModuleId)
        {
            #region 验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    NC_PO_ORDER editentity = repositoryfactory.Repository().FindEntity(KeyValue);
                    editentity.ACTUALVALIDATE = entity.ACTUALVALIDATE;
                    editentity.ACTUALINVALIDATE = entity.ACTUALINVALIDATE;
                    editentity.VCONTRACTCODE = entity.VCONTRACTCODE;
                    editentity.HKCGBILLNO = entity.HKCGBILLNO;
                    editentity.DEF3 = entity.DEF3;
                    editentity.TRADEMODE = entity.TRADEMODE;
                    editentity.PORTARRIVAL = entity.PORTARRIVAL;
                    editentity.REGISTERACCOUNT = entity.REGISTERACCOUNT;
                    editentity.PK_RECEIVESTORE = entity.PK_RECEIVESTORE;
                    editentity.RECEIVESTORE = entity.RECEIVESTORE;
                    editentity.CARSNUM = entity.CARSNUM;
                    editentity.Modify(KeyValue);
                    database.Update(editentity, isOpenTrans);
                }
                else
                {
                    string sql = String.Format("select count(1) from NC_PO_ORDER where  VBILLCODE='{0}'", entity.VBILLCODE);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        this.ModuleId = ModuleId;
                        entity.VBILLCODE = this.BillCode();
                    }
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采购单号:{0}，{1}。", entity.VBILLCODE, Message);
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
        public ActionResult SubmitOrderForm1(string TRAFFICCOMPANY, string TRAFFICCOMPANYNAME, string OrderEntryJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<NC_PO_ORDER> OrderEntryList = OrderEntryJson.JonsToList<NC_PO_ORDER>();
                string Message = "订单分配承运商成功。";
                NC_PO_ORDER entity = new NC_PO_ORDER();
                foreach (NC_PO_ORDER obj in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(obj.VBILLCODE))
                    {
                        //  entity = repositoryfactory.Repository().FindEntity(obj.PK_ORDER);
                        NC_PO_ORDER_C add_entity = new NC_PO_ORDER_C();
                        add_entity.Create();
                        add_entity.PK_ORDER = obj.PK_ORDER;
                        add_entity.VBILLCODE = obj.VBILLCODE;
                        add_entity.TRAFFICCOMPANY = TRAFFICCOMPANY;
                        add_entity.TRAFFICCOMPANYNAME = TRAFFICCOMPANYNAME;
                        database.Insert(add_entity, isOpenTrans);
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采购订单分配承运商{0}成功。", TRAFFICCOMPANYNAME);
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

        //承运商分配
        [LoginAuthorize]
        public ActionResult SubmitOrderForm2(string PK_ORDER, string VBILLCODE, string OrderEntryJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string sql = "";
                DataTable dt = new DataTable();
                database.Delete<NC_PO_ORDER_C>("PK_ORDER", PK_ORDER, isOpenTrans);
                //明细行数据
                List<NC_PO_ORDER_C> OrderEntryList = OrderEntryJson.JonsToList<NC_PO_ORDER_C>();
                string Message = "订单分配承运商成功。";
                NC_PO_ORDER entity = new NC_PO_ORDER();
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //是否分配
                strSql.Append(string.Format("update NC_PO_ORDER set PK_TRANSPORTTYPE='1' where PK_ORDER='{0}'", PK_ORDER));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                foreach (NC_PO_ORDER_C obj in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(obj.TRAFFICCOMPANY))
                    {
                        //sql = String.Format("select * from NC_PO_ORDER_C where TRAFFICCOMPANY='{0}' and PK_ORDER!='{1}'", obj.TRAFFICCOMPANY, PK_ORDER);
                        //dt = DataFactory.Database().FindTableBySql(sql);
                        //if (dt.Rows.Count > 0)
                        //{
                        //}
                        //else
                        //{
                        NC_PO_ORDER_C add_entity = new NC_PO_ORDER_C();
                        add_entity.Create();
                        add_entity.PK_ORDER = PK_ORDER;
                        add_entity.VBILLCODE = VBILLCODE;
                        add_entity.TRAFFICCOMPANY = obj.TRAFFICCOMPANY;
                        add_entity.TRAFFICCOMPANYNAME = obj.TRAFFICCOMPANYNAME;
                        add_entity.MEMO = obj.MEMO;
                        database.Insert(add_entity, isOpenTrans);
                        //}
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采购订单{0}分配承运商成功。", VBILLCODE);
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
        //散户分配
        [LoginAuthorize]
        public ActionResult SubmitOrderForm3(string PK_ORDER, string VBILLCODE, string OrderEntryJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                DataTable dt = new DataTable();
                database.Delete<NC_PO_ORDER_D>("PK_ORDER", PK_ORDER, isOpenTrans);
                //明细行数据
                List<NC_PO_ORDER_D> OrderEntryList = OrderEntryJson.JonsToList<NC_PO_ORDER_D>();
                string Message = "订单分配散户用户成功。";

                foreach (NC_PO_ORDER_D obj in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(obj.USERNAME))
                    {
                        NC_PO_ORDER_D add_entity = new NC_PO_ORDER_D();
                        add_entity.Create();
                        add_entity.PK_ORDER = PK_ORDER;
                        add_entity.VBILLCODE = VBILLCODE;
                        add_entity.USERID = obj.USERID;
                        add_entity.USERNAME = obj.USERNAME;
                        add_entity.MEMO = obj.MEMO;
                        database.Insert(add_entity, isOpenTrans);
                    }
                }
                database.Commit();
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采购订单{0}分配散户用户成功。", VBILLCODE);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderFormAdd(string KeyValue, NC_PO_ORDER entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                //明细行数据
                List<NC_PO_ORDER_B> OrderEntryList = OrderEntryJson.JonsToList<NC_PO_ORDER_B>();

                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    database.Delete<NC_PO_ORDER_B>("PK_ORDER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    IsOk = database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    entity.PK_ORDER = string.Format("{0}", entity.VBILLCODE);
                    string sql = String.Format("select count(1) from NC_PO_ORDER where  VBILLCODE='{0}'", entity.VBILLCODE);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.DATATYPE = "0";
                        entity.VBILLCODE = this.BillCode();
                        entity.PK_ORDER = string.Format("{0}", entity.VBILLCODE);
                    }
                    entity.Create();

                    IsOk = database.Insert(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }

                //处理明细
                int index = 1;
                foreach (NC_PO_ORDER_B orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.PK_MATERIAL))
                    {
                        orderentry.Create();
                        orderentry.DR = 1;
                        orderentry.PK_ORDER = entity.PK_ORDER;
                        orderentry.PK_ORDER_B = string.Format("{0}{1}", entity.PK_ORDER, orderentry.CROWNO);
                        orderentry.BARRIVECLOSE = "0";
                        orderentry.PURUNITNAME = orderentry.SECPURUNITNAME;
                        orderentry.NASTNUM = orderentry.SECQUANTITY;
                        database.Insert(orderentry, isOpenTrans);
                        index++;
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采购单号:{0}，新增成功。", entity.VBILLCODE);
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
        /// 生成到货单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderFormCopy(string KeyValue, NC_PO_ORDER entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                //明细行数据
                List<NC_PO_ORDER_B> OrderEntryList = OrderEntryJson.JonsToList<NC_PO_ORDER_B>();

                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    database.Delete<NC_PO_ORDER_B>("PK_ORDER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    IsOk = database.Update(entity, isOpenTrans);
                }
                else
                {
                    string code = DateTime.Now.ToString("ffff");
                    entity.VBILLCODE = entity.VBILLCODE;
                    entity.PK_ORDER = string.Format("{0}{1}", entity.PK_ORDER, code);
                    string sql = String.Format("select count(1) from NC_PO_ORDER where  VBILLCODE='{0}'", entity.VBILLCODE);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "到货单号已存在，请重新录入到货单号！" }.ToString());

                    }
                    entity.Create();
                    entity.BILLTYPE = "入库通知标准单据";
                    IsOk = database.Insert(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }

                //处理明细
                int index = 1;
                foreach (NC_PO_ORDER_B orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.PK_ORDER))
                    {
                        orderentry.Create();
                        orderentry.DR = 1;
                        orderentry.PK_ORDER = entity.PK_ORDER;
                        orderentry.PK_ORDER_B = string.Format("{0}0{1}", entity.PK_ORDER, index);
                        database.Insert(orderentry, isOpenTrans);
                        index++;
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("生成到货单号:{0}，新增成功。", entity.VBILLCODE);
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
        //启用
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            //验证
            string sql = String.Format("select count(1) from NC_PO_ORDER where FORDERSTATUS=0 and PK_ORDER in('{0}')", bills);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "选中数据有启用订单,无法继续启用！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "启用成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //明细
                strSql.Append(string.Format("update NC_PO_ORDER set FORDERSTATUS=0,DEF6='{0}',DEF7='{1}' where PK_ORDER in('{2}')", ManageProvider.Provider.Current().UserName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), bills));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //停用 
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            //验证
            string sql = String.Format("select count(1) from NC_PO_ORDER where FORDERSTATUS=1 and PK_ORDER in('{0}')", bills);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "选中数据有停用订单,无法继续停用！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "停用成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();

                strSql.Append(string.Format("update NC_PO_ORDER set FORDERSTATUS=1,DEF6='{0}',DEF7='{1}' where PK_ORDER in('{2}')", ManageProvider.Provider.Current().UserName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), bills));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
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
        public ActionResult HandOrder(string vbillcode,string begindate, string strwerks,string supply,string type)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient("SAPWebServiceSoap");
                
                WEBSERVICES_SAP.RETURN_SAP result = new WEBSERVICES_SAP.RETURN_SAP();
                if (string.IsNullOrEmpty(begindate))
                {
                    begindate = DateTime.Now.ToString("yyyy-MM-dd");
                }
                result = sap.SAP_116ZS(begindate, "", vbillcode, strwerks, supply, type, "");
                JsonMessage jsonmeg = null;
                if (result.STATUS == "S")
                {
                    string Message = "同步成功。";
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


        //关闭 
        [LoginAuthorize]
        public ActionResult OrderClose(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from NC_PO_ORDER_B where BARRIVECLOSE=1 and PK_ORDER_B='{0}'", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "选中订单明细已关闭,无法继续关闭！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "订单明细行关闭成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update NC_PO_ORDER_B set BARRIVECLOSE=1 where PK_ORDER_B='{0}'", KeyValue));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //关闭 
        [LoginAuthorize]
        public ActionResult OrderAllClose(string KeyValue)
        {
            //验证
            string[] arr = KeyValue.Split(',');
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "订单明细行关闭成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                if (arr.Length == 1)
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update NC_PO_ORDER_B set BARRIVECLOSE=1 where PK_ORDER='{0}'", KeyValue));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    strSql.Clear();
                    strSql.Append(string.Format("update NC_PO_ORDER set FORDERSTATUS=1 where PK_ORDER='{0}'", KeyValue));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }
                else
                {
                    foreach (string key in arr)
                    {
                        strSql.Clear();
                        strSql.Append(string.Format("update NC_PO_ORDER_B set BARRIVECLOSE=1 where PK_ORDER='{0}'", key));
                        database.ExecuteBySql(strSql, isOpenTrans);
                        strSql.Clear();
                        strSql.Append(string.Format("update NC_PO_ORDER set FORDERSTATUS=1 where PK_ORDER='{0}'", key));
                        database.ExecuteBySql(strSql, isOpenTrans);
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

        //开启 
        [LoginAuthorize]
        public ActionResult OrderOpen(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from NC_PO_ORDER_B where BARRIVECLOSE=0 and PK_ORDER_B='{0}'", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "选中订单明细已开启,无法继续开启！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "订单明细行开启成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update NC_PO_ORDER_B set BARRIVECLOSE=0 where PK_ORDER_B='{0}'", KeyValue));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
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
            string bills = KeyValue.Replace(",", "','");
            //验证
            string sql = String.Format("select count(1) from NC_PO_ORDER where PK_ORDER in('{0}') and  DATATYPE>0", bills);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只能删除数据来源自建的单据！" }.ToString());
            }
            sql = String.Format("select count(1) from DP_POCARSORDER_DTL where PK_ORDER in('{0}')", bills);
            res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "删除的订单中已有派车业务,请您确认后删除！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("delete NC_PO_ORDER where PK_ORDER in('{0}')", bills));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                strSql.Clear();
                strSql.Append(string.Format("delete NC_PO_ORDER_B where PK_ORDER in('{0}')", bills));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}
