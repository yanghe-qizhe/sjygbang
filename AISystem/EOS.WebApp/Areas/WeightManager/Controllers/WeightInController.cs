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
using System.IO;
using System.Threading;
using System.Data;


namespace EOS.WebApp.Areas.WeightManager.Controllers
{
    public class WB_WEIGHT_IMPORT
    {
        public string PK_CONTRACT_B { get; set; }
        public string CARS { get; set; }
        public string DRIVERS { get; set; }
        public string RECEIVESTORE { get; set; }
        public string TRAFFICCOMPANY { get; set; }
        public string YFSUTTLE { get; set; }
        public string GROSS { get; set; }
        public string TARE { get; set; }
        public string SUTTLE { get; set; }
        public string DEF1 { get; set; }
    }

    public class WeightInController : PublicController<WB_WEIGHT>
    {
        string ModuleId = "";
        WB_WEIGHTBll wbbll = new WB_WEIGHTBll();
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        VWB_WEIGHTBSBll orderbll = new VWB_WEIGHTBSBll();
        VWB_WEIGHT_B_Bll order_b_bll = new VWB_WEIGHT_B_Bll();
        VWB_WEIGHT_BCZ_Bll order_bcz_bll = new VWB_WEIGHT_BCZ_Bll();
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
        public ActionResult WeightInIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightInForm()
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
        public ActionResult WeightInForm1()
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
        public ActionResult WeightInForm2()
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
        public ActionResult WeightInForm3()
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

        //二次未结算磅单参照
        [LoginAuthorize]
        public ActionResult WeightInForm4()
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

        //导入
        public ActionResult WeightInImport()
        {
            return View();
        }
        public ActionResult WeightInDetail()
        {
            return View();
        }
        public ActionResult WeightInQuery()
        {
            return View();
        }
        //废纸批量打印
        public ActionResult FZWeightInReportIndex()
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

        [LoginAuthorize]
        public ActionResult SELECTPOORDERS1()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SELECTPOORDERS2()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SELECTPCBillSOF()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult InvalidForm()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult InvalidForm1()
        {
            return View();
        }
        //管理员验证
        public ActionResult SELECTCHECKUSER()
        {
            return View();
        }
        //管理员验证
        public ActionResult SELECTCHECKUSER1()
        {
            return View();
        }
        /// 采购磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string VBillCode, string ContractBillNo, string DHBillNo, string PK_RECEIVEORG, string Supply, string Material, string matclass, string Cars, string Status, string UPLOAD, string Finish, string COMPUTERTYPE, string ISJS, string TRAFFICCOMPANY, string DEF11, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHTBS> ListData = orderbll.GetRKOrderList(BillNo, VBillCode, ContractBillNo, DHBillNo, PK_RECEIVEORG, Supply, Material, matclass, Cars, Status, "0", UPLOAD, Finish, COMPUTERTYPE, ISJS, TRAFFICCOMPANY, DEF11, StartTime, EndTime, jqgridparam);
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
        //图片
        [LoginAuthorize]
        public ActionResult GetOrderEntryList(string KeyValue)
        {
            try
            {
                List<WB_WEIGHTPICDETAIL> list_new = new List<WB_WEIGHTPICDETAIL>();
                List<WB_WEIGHTPICDETAIL> list = orderbll.GetOrderEntryList(KeyValue);
                string httpurl = ConfigurationManager.AppSettings["httpUrl"].Trim();
                foreach (WB_WEIGHTPICDETAIL model in list)
                {
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
        /// 汽运采购磅单明细列表（返回Json）
        /// </summary>
        /// <param name="KeyValue">磅单主键</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderEntryList1(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = order_b_bll.GetOrderEntryList(KeyValue),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        public ActionResult GetOrderEntryListCZ(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = order_bcz_bll.GetOrderEntryList(KeyValue),
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
        public ActionResult SubmitOrderForm(string KeyValue, WB_WEIGHT entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<WB_WEIGHT_B> OrderEntryList = OrderEntryJson.JonsToList<WB_WEIGHT_B>();
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    #region 验证
                    string sql = String.Format("SELECT count(1) NUMS FROM VZJQY WHERE  ISCY='0' and ID='{0}'", entity.PK_ORDER);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}未取样！", entity.CARS, entity.PK_ORDER) }.ToString());
                    }
                    sql = String.Format("SELECT count(1) NUMS FROM APP_HANDORDER WHERE  BILLNO='{0}'", entity.BILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}已收货确认！", entity.CARS, entity.PK_ORDER) }.ToString());
                    }
                    #endregion
                    database.Delete<WB_WEIGHT_B>("PK_TASK", KeyValue, isOpenTrans);
                    WB_WEIGHT obj = repositoryfactory.Repository().FindEntity(KeyValue);
                    //通知单变更
                    if (obj.PK_ORDER != entity.PK_ORDER)
                    {
                        strSql.Clear();
                        strSql.Append(string.Format("delete DP_CARSORDER where orderid='{0}'", obj.PK_ORDER));
                        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_POCARSORDER set IsUse='{0}' where ID='{1}'", 1, obj.PK_ORDER));
                        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_POCARSORDER_DTL set IsUse='{0}' where ID='{1}'", 1, obj.PK_ORDER_B));
                        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                    }
                    else
                    {
                        //订单变更
                        if (obj.PK_CONTRACT_B != entity.PK_CONTRACT_B)
                        {
                            strSql.Append(string.Format("update DP_POCARSORDER set PK_SUPPLIER='{0}',SUPPLIERNAME='{1}' where ID='{2}'", entity.PK_SENDORG, entity.SENDORG, entity.PK_ORDER));
                            repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                            strSql.Clear();
                        }
                    }
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    string sql = String.Format("select count(1) from WB_WEIGHT where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    if (!string.IsNullOrEmpty(entity.DEF3))
                    {
                        entity.YFPIECE = Convert.ToDecimal(entity.DEF3) > 0 ? Convert.ToDecimal(entity.DEF3) : entity.SUTTLE;
                    }
                    entity.TYPE = "0";
                    entity.LASTDATE = entity.LIGHTDATE;
                    database.Insert(entity, isOpenTrans);
                }

                if (!string.IsNullOrEmpty(entity.PK_ORDER))
                {
                    if (entity.FINISH == "1")
                    {
                        strSql.Clear();
                        strSql.Append(string.Format("delete DP_CARSORDER where orderid='{0}'", entity.PK_ORDER));
                        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_POCARSORDER set IsUse='{0}' where ID='{1}'", 6, entity.PK_ORDER));
                        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                        strSql.Clear();
                    }
                    else
                    {
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_POCARSORDER set IsUse='{0}' where ID='{1}'", 7, entity.PK_ORDER));
                        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                    }
                }
                //处理明细
                int index = 1;
                DP_POCARSORDER_DTL pcdel_entity = null;
                foreach (WB_WEIGHT_B orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.PK_ORDER_B))
                    {

                        //结算明细
                        orderentry.Create();
                        orderentry.PK_TASK = entity.BILLNO;
                        orderentry.BILLNO = string.Format("{0}{1}", entity.BILLNO, index);
                        orderentry.PK_TRAFFICCOMPANY = entity.PK_TRAFFICCOMPANY;
                        orderentry.TRAFFICCOMPANY = entity.TRAFFICCOMPANY;
                        orderentry.WEIGHTDATE = entity.WEIGHTDATE;
                        orderentry.LIGHTDATE = entity.LIGHTDATE;
                        orderentry.LASTDATE = entity.LIGHTDATE;
                        if (!string.IsNullOrEmpty(orderentry.DEF3))
                        {
                            orderentry.YFPIECE = Convert.ToDecimal(orderentry.DEF3) > 0 ? Convert.ToDecimal(orderentry.DEF3) : orderentry.SUTTLE;
                        }
                        orderentry.MEMO = orderentry.MEMO.Replace("null", "");
                        database.Insert(orderentry, isOpenTrans);
                        pcdel_entity = database.FindEntityByWhere<DP_POCARSORDER_DTL>(string.Format(" AND ID='{0}'", orderentry.PK_ORDER_B));
                        if (orderentry.PK_CONTRACT_B != pcdel_entity.VBILLCODE)
                        {
                            //更新派车明细表
                            pcdel_entity.PK_ORDER = orderentry.PK_BILL;
                            pcdel_entity.PK_ORDER_B = orderentry.PK_BILL_B;
                            pcdel_entity.VBILLCODE = orderentry.PK_CONTRACT_B;
                            pcdel_entity.PK_SUPPLIER = orderentry.PK_SENDORG;
                            pcdel_entity.SUPPLIERNAME = orderentry.SENDORG;
                            pcdel_entity.PK_MATERIAL = orderentry.PK_MATERIAL;
                            pcdel_entity.MATERIALNAME = orderentry.MATERIAL;
                            pcdel_entity.MATERIALSPEC = orderentry.MATERIALKIND;
                            pcdel_entity.PK_ORG = orderentry.PK_RECEIVEORG;
                            pcdel_entity.ORGNAME = orderentry.RECEIVEORG;
                            pcdel_entity.DEF1 = orderentry.DEF1;
                            pcdel_entity.DEF2 = orderentry.DEF2;
                            pcdel_entity.ISUSE = "3";
                            database.Update(pcdel_entity, isOpenTrans);
                        }
                        index++;
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


        //补增
        [HttpPost]
        public ActionResult SubmitOrderFormAdd(WB_WEIGHT entity, string ModuleId)
        {
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //string sql = String.Format("SELECT count(1) NUMS FROM APP_HANDORDER WHERE  ID='{0}'", entity.PK_ORDER_B);
                //int res = DataFactory.Database().FindCountBySql(sql);
                //if (res == 0)
                //{
                //    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}未收货确认！", entity.CARS, entity.PK_ORDER_B) }.ToString());
                //}
                //处理主头
                string Message = "新增成功。";
                string sql = String.Format("select count(1) from WB_WEIGHT where  BILLNO='{0}'", entity.BILLNO);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    this.ModuleId = ModuleId;
                    entity.BILLNO = this.BillCode();
                }
                entity.Create();
                entity.FINISH = "0";
                entity.TYPE = "0";
                entity.LASTDATE = entity.LIGHTDATE;
                database.Insert(entity, isOpenTrans);

                if (!string.IsNullOrEmpty(entity.PK_ORDER))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("delete DP_CARSORDER where orderid='{0}'", entity.PK_ORDER));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_POCARSORDER set IsUse='{0}' where ID='{1}'", 6, entity.PK_ORDER));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_POCARSORDER_DTL set IsUse='{0}' where ID='{1}'", 2, entity.PK_ORDER_B));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采购磅单 :{0}，车辆:{1}，补增成功。", entity.BILLNO, entity.CARS);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Add, "1", message);
                database.Commit();

                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        //参照
        [HttpPost]
        public ActionResult SubmitOrderFormEdit(string KeyValue, WB_WEIGHT entity, string ModuleId)
        {
            BD_MaterialBll matbll = new BD_MaterialBll();
            VBD_MATERIAL mat = matbll.GetEntity(entity.MATERIAL);
            bool ischeck = false;
            if (mat != null)
            {
                if (mat.ISBATCH == 1)
                {
                    ischeck = true;
                }
            }
            #region 验证
            string sql = "";
            int res;
            if (entity.FINISH == "1")
            {
                //if (ischeck)
                //{
                //    sql = String.Format("SELECT count(1) NUMS FROM VZJQY WHERE  ISCY='0' and ID='{0}'", entity.PK_ORDER);
                //    res = DataFactory.Database().FindCountBySql(sql);
                //    if (res > 0)
                //    {
                //        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}未采样完成！", entity.CARS, entity.PK_ORDER) }.ToString());
                //    }
                //}
                //sql = String.Format("SELECT count(1) NUMS FROM APP_HANDORDER WHERE  BILLNO='{0}'", entity.BILLNO);
                //res = DataFactory.Database().FindCountBySql(sql);
                //if (res == 0)
                //{
                //    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}未收货确认！", entity.CARS, entity.PK_ORDER) }.ToString());
                //}
            }
            else
            {
                if (ischeck)
                {
                    sql = String.Format("SELECT count(1) NUMS FROM VZJQY WHERE  ISCY='1' and ID='{0}'", entity.PK_ORDER);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}已采样完成！", entity.CARS, entity.PK_ORDER) }.ToString());
                    }
                }
                sql = String.Format("SELECT count(1) NUMS FROM APP_HANDORDER WHERE  BILLNO='{0}'", entity.BILLNO);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}已收货确认！", entity.CARS, entity.PK_ORDER) }.ToString());
                }
            }

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //处理主头
                string Message = "编辑成功。";
                NC_PO_ORDER po_order = database.FindEntityByWhere<NC_PO_ORDER>(string.Format(" AND PK_ORDER='{0}'", entity.PK_BILL));
                DP_POCARSORDER_DTL pc_entity = database.FindEntityByWhere<DP_POCARSORDER_DTL>(string.Format(" AND ID='{0}'", entity.PK_ORDER_B));
                pc_entity.VBILLCODE = entity.PK_CONTRACT;
                pc_entity.PK_ORDER = entity.PK_BILL;
                pc_entity.PK_ORDER_B = entity.PK_BILL_B;
                if (po_order.BILLTYPE == "YG15")
                {

                }
                else
                {
                    pc_entity.PK_SUPPLIER = entity.PK_SENDORG;
                    pc_entity.SUPPLIERNAME = entity.SENDORG;
                }

                pc_entity.PK_MATERIAL = entity.PK_MATERIAL;
                pc_entity.MATERIALNAME = entity.MATERIAL;
                pc_entity.MATERIALSPEC = entity.MATERIALKIND;
                database.Update(pc_entity, isOpenTrans);

                WB_WEIGHT obj = database.FindEntity<WB_WEIGHT>(KeyValue);
                //更新磅单 
                if (!string.IsNullOrEmpty(entity.LIGHTDATE))
                {
                    obj.LIGHTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    obj.LIGHTDATE = entity.LIGHTDATE;
                }
                obj.PK_STORE = entity.PK_RECEIVESTORE;
                obj.STORE = entity.RECEIVESTORE;
                obj.OUTGBUSER = ManageProvider.Provider.Current().UserName;
                obj.LASTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                obj.PK_ORDER = entity.PK_ORDER;
                obj.PK_ORDER_B = entity.PK_ORDER_B;
                if (po_order.BILLTYPE == "YG15")
                {
                    //obj.PK_SENDORG = pc_entity.PK_SENDADDRESS;
                    //obj.SENDORG = pc_entity.SENDADDRESS;
                }
                else
                {
                    obj.PK_SENDORG = entity.PK_SENDORG;
                    obj.SENDORG = entity.SENDORG;
                }
                obj.PK_BILL = entity.PK_BILL;
                obj.PK_BILL_B = entity.PK_BILL_B;
                obj.PK_CONTRACT = entity.PK_CONTRACT;
                obj.PK_CONTRACT_B = entity.PK_CONTRACT_B;
                obj.PK_MATERIAL = entity.PK_MATERIAL;
                obj.MATERIAL = entity.MATERIAL;
                obj.PK_TRAFFICCOMPANY = entity.PK_TRAFFICCOMPANY;
                obj.TRAFFICCOMPANY = entity.TRAFFICCOMPANY;
                obj.PK_RECEIVEORG = entity.PK_RECEIVEORG;
                obj.RECEIVEORG = entity.RECEIVEORG;
                obj.PK_CARSID = entity.PK_CARSID;
                obj.CARS = entity.CARS;
                obj.PK_DRIVERS = entity.PK_DRIVERS;
                obj.DRIVERS = entity.DRIVERS;
                obj.YFSUTTLE = entity.YFSUTTLE;
                obj.GROSS = entity.GROSS;
                obj.TARE = entity.TARE;
                obj.SUTTLE = entity.SUTTLE;
                obj.WEIGHTDATE = entity.WEIGHTDATE;
                obj.LIGHTDATE = entity.LIGHTDATE;
                obj.PRICE = entity.PRICE;
                obj.TRAFFICPRICE = entity.TRAFFICPRICE;
                obj.FINISH = entity.FINISH;
                obj.UPLOAD1 = entity.UPLOAD1;
                obj.MEMO = entity.MEMO;
                //更新计量单
                database.Update(obj, isOpenTrans);
                if (!string.IsNullOrEmpty(entity.PK_ORDER_B))
                {
                    APP_HANDORDER app_entity = database.FindEntityByWhere<APP_HANDORDER>(string.Format(" AND ID='{0}'", entity.PK_ORDER_B));
                    if (app_entity != null)
                    {
                        if (!string.IsNullOrEmpty(app_entity.ID))
                        {
                            app_entity.JZXSUTTLE1 = entity.PRICE;
                            app_entity.PK_STORE = entity.PK_RECEIVESTORE;
                            app_entity.STORE = entity.RECEIVESTORE;               //更新收货单
                            if (obj.PK_BILL_B != entity.PK_BILL_B)
                            {
                                app_entity.GPMATERIAL = "";
                                app_entity.GPMATERIALNAME = "";
                                app_entity.GPORDERNO = "";
                            }
                            app_entity.GROSS = entity.GROSS;
                            app_entity.TARE = entity.TARE;
                            app_entity.SUTTLE = entity.SUTTLE;
                            decimal bl = 0;
                            decimal sumzjvalue = 0;
                            if (!string.IsNullOrEmpty(app_entity.PDAKZ3))
                            {
                                sumzjvalue += Convert.ToDecimal(app_entity.PDAKZ3);
                            }
                            if (!string.IsNullOrEmpty(app_entity.PDAKZ4))
                            {
                                sumzjvalue += Convert.ToDecimal(app_entity.PDAKZ4);
                            }
                            if (!string.IsNullOrEmpty(app_entity.PDAKZ5))
                            {
                                sumzjvalue += Convert.ToDecimal(app_entity.PDAKZ5);
                            }
                            if (!string.IsNullOrEmpty(app_entity.PDAKZ6))
                            {
                                sumzjvalue += Convert.ToDecimal(app_entity.PDAKZ6);
                            }
                            if (sumzjvalue > 0)
                            {
                                bl = sumzjvalue / 100;
                            }
                            int KZ = Convert.ToInt32(entity.SUTTLE * bl);
                            app_entity.JZXSUTTLE = entity.SUTTLE - KZ;
                            app_entity.PDAKZ2 = $"{ KZ}";
                            database.Update(app_entity, isOpenTrans);
                        }
                    }
                }
                database.Commit();
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采购磅单 :{0}，车辆:{1}，参照成功。", entity.BILLNO, entity.CARS);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Update, "1", message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //正常参照
        [HttpPost]
        public ActionResult SubmitOrderFormEdit1(string KeyValue, WB_WEIGHT entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证
            string sql = "";
            int res;
            if (entity.FINISH == "1")
            {
                //二次计量验证
                sql = String.Format("SELECT count(1) NUMS FROM VZJQY WHERE  ISCY='0' and ID='{0}'", entity.PK_ORDER);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}未采样完成！", entity.CARS, entity.PK_ORDER) }.ToString());
                }
                sql = String.Format("SELECT count(1) NUMS FROM APP_HANDORDER WHERE  BILLNO='{0}'", entity.BILLNO);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res == 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}未收货确认！", entity.CARS, entity.PK_ORDER) }.ToString());
                }
            }
            else
            {
                //一次计量验证
                sql = String.Format("SELECT count(1) NUMS FROM VZJQY WHERE  ISCY='1' and ID='{0}'", entity.PK_ORDER);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}已采样！", entity.CARS, entity.PK_ORDER) }.ToString());
                }
                sql = String.Format("SELECT count(1) NUMS FROM APP_HANDORDER WHERE  BILLNO='{0}'", entity.BILLNO);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}已收货确认！", entity.CARS, entity.PK_ORDER) }.ToString());
                }
            }


            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //处理主头
                string Message = "编辑成功。";
                //明细行数据
                List<DP_POCARSORDER_DTL> OrderEntryList = OrderEntryJson.JonsToList<DP_POCARSORDER_DTL>();
                #region 司机信息
                BD_Driver driver = null;
                DP_REGIDCARD iccard = null;
                if (!string.IsNullOrEmpty(entity.PK_DRIVERS))
                {
                    driver = DataFactory.Database().FindEntityBySql<BD_Driver>(string.Format("select * from BD_Driver where id='{0}'", entity.PK_DRIVERS));
                    if (driver != null)
                    {
                        iccard = DataFactory.Database().FindEntityBySql<DP_REGIDCARD>(string.Format("select * from DP_REGIDCARD where CARDNO='{0}'", driver.IDNUM));
                    }
                }
                #endregion

                //更新磅单主表
                entity.Modify(KeyValue);


                bool bol = false;

                if (iccard != null)
                {
                    if (!string.IsNullOrEmpty(iccard.CARDSN))
                    {
                        entity.ICCARD = iccard.CARDSN;//身份证卡序号
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("司机:{0}未注册！", entity.DRIVERS) }.ToString());
                }
                entity.OUTGBUSER = ManageProvider.Provider.Current().UserName;
                entity.INGBUSER = ManageProvider.Provider.Current().UserName;
                database.Update(entity, isOpenTrans);

                //派车表实例
                DP_POCARSORDER pc_entity = null;
                pc_entity = database.FindEntityByWhere<DP_POCARSORDER>(string.Format(" AND ID='{0}'", entity.PK_ORDER));

                //派车明细 
                int index = 1;
                foreach (DP_POCARSORDER_DTL orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.ID))
                    {
                        orderentry.Modify(orderentry.ID);
                        orderentry.PK_DP_POCARSORDER = pc_entity.ID;
                        orderentry.BILLSOFTYPE = pc_entity.BILLSOFTYPE;
                        orderentry.CHARGE = orderentry.AMOUNT * orderentry.PRICE;
                        database.Update(orderentry, isOpenTrans);
                        //更新派车主表信息
                        if (index == 1)
                        {
                            pc_entity.PK_SUPPLIER = orderentry.PK_SUPPLIER;
                            pc_entity.SUPPLIERNAME = orderentry.SUPPLIERNAME;
                            pc_entity.CARS = entity.PK_CARSID;
                            pc_entity.CARSNAME = entity.CARS;
                            pc_entity.DRIVERS = entity.PK_DRIVERS;
                            pc_entity.DRIVERSNAME = entity.DRIVERS;
                            pc_entity.TRAFFICCOMPANY = entity.PK_TRAFFICCOMPANY;
                            pc_entity.TRAFFICCOMPANYNAME = entity.TRAFFICCOMPANY;
                            database.Update(pc_entity, isOpenTrans);
                        }
                        index++;
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采购磅单 :{0}，车辆:{1}，参照成功。", entity.BILLNO, entity.CARS);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Update, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //正常参照
        [HttpPost]
        public ActionResult SubmitOrderFormEdit2(string KeyValue, WB_WEIGHT entity, string OrderEntryJson, string ModuleId)
        {
            BD_MaterialBll matbll = new BD_MaterialBll();
            VBD_MATERIAL mat = matbll.GetEntity(entity.MATERIAL);
            bool ischeck = false;
            if (mat != null)
            {
                if (mat.ISBATCH == 1)
                {
                    ischeck = true;
                }
            }
            #region 验证
            string sql = "";
            int res;
            if (entity.FINISH == "1")
            {
                if (ischeck)
                {
                    //二次计量验证
                    sql = String.Format("SELECT count(1) NUMS FROM VZJQY WHERE  ISCY='0' and ID='{0}'", entity.PK_ORDER);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}未采样完成！", entity.CARS, entity.PK_ORDER) }.ToString());
                    }
                }
                sql = String.Format("SELECT count(1) NUMS FROM APP_HANDORDER WHERE  BILLNO='{0}'", entity.BILLNO);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res == 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}未收货确认！", entity.CARS, entity.PK_ORDER) }.ToString());
                }
            }
            else
            {
                //if (ischeck)
                //{
                //一次计量验证
                sql = String.Format("SELECT count(1) NUMS FROM VZJQY WHERE  ISCY='1' and ID='{0}'", entity.PK_ORDER);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}已采样！", entity.CARS, entity.PK_ORDER) }.ToString());
                }
                //}
                sql = String.Format("SELECT count(1) NUMS FROM APP_HANDORDER WHERE  BILLNO='{0}'", entity.BILLNO);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的到货单:{1}已收货确认！", entity.CARS, entity.PK_ORDER) }.ToString());
                }
            }


            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //处理主头
                string Message = "编辑成功。";
                string msg = "";
                //明细行数据
                List<WB_WEIGHT_B> OrderEntryList = OrderEntryJson.JonsToList<WB_WEIGHT_B>();

                #region 获取司机相关信息
                BD_Driver driver = null;
                DP_REGIDCARD iccard = null;
                if (!string.IsNullOrEmpty(entity.PK_DRIVERS))
                {
                    driver = DataFactory.Database().FindEntityBySql<BD_Driver>(string.Format("select * from BD_Driver where id='{0}'", entity.PK_DRIVERS));
                    if (driver != null)
                    {
                        iccard = DataFactory.Database().FindEntityBySql<DP_REGIDCARD>(string.Format("select * from DP_REGIDCARD where CARDNO='{0}'", driver.IDNUM));
                    }
                }
                #endregion

                //更新磅单主表
                entity.Modify(KeyValue);
                #region 司机更改
                bool bol = false;
                if (iccard != null)
                {
                    if (!string.IsNullOrEmpty(iccard.CARDSN))
                    {
                        entity.ICCARD = iccard.CARDSN;//身份证卡序号
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("司机:{0}未注册！", entity.DRIVERS) }.ToString());
                }
                #endregion
                if (!string.IsNullOrEmpty(entity.LIGHTDATE))
                {
                    entity.LIGHTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    entity.LIGHTDATE = entity.LIGHTDATE;
                }
                entity.ISEDIT = 1;
                entity.OUTGBUSER = ManageProvider.Provider.Current().UserName;
                //entity.INGBUSER = ManageProvider.Provider.Current().UserName;
                entity.LASTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                database.Update(entity, isOpenTrans);

                int index = 1;
                //派车表实例
                DP_POCARSORDER pc_entity = null;
                pc_entity = database.FindEntityByWhere<DP_POCARSORDER>(string.Format(" AND ID='{0}'", entity.PK_ORDER));

                //派车明细表实例
                DP_POCARSORDER_DTL pcdtl_entity = null;

                //收货确认实例
                VAPP_HANDORDERBll overbll = new VAPP_HANDORDERBll();
                VAPP_HANDORDER handentity = null;
                //处理明细
                foreach (WB_WEIGHT_B orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.PK_ORDER_B))
                    {
                        //获取原派车明细
                        pcdtl_entity = database.FindEntityByWhere<DP_POCARSORDER_DTL>(string.Format(" AND ID='{0}'", orderentry.PK_ORDER_B));
                        #region 二次计量
                        if (entity.FINISH == "1")
                        {
                            //结算明细
                            orderentry.Create();
                            #region 明细赋值
                            orderentry.ISEDIT = 1;
                            orderentry.PK_TASK = entity.BILLNO;
                            #region 采购收料
                            handentity = overbll.GetEntity(orderentry.PK_ORDER_B);
                            if (handentity != null)
                            {
                                orderentry.DEF3 = handentity.PDASSJS;
                                orderentry.DEF4 = handentity.TOTALJSUTTLE;
                                orderentry.DEF5 = handentity.TOTALJTARE;
                                orderentry.PDAUSER = handentity.OPERUSER;
                                orderentry.PDADATE = handentity.OPERTIME;
                                orderentry.PDANO = handentity.PDANO;
                                orderentry.PDAKZ1 = (handentity.PDAKZ1.ToDecimal() * 0.001M).ToString("f3"); ;
                                orderentry.PDAKZ2 = handentity.PDAKZ2;
                                if (!string.IsNullOrEmpty(handentity.PDAYF))
                                {
                                    orderentry.YFSUTTLE = Convert.ToDecimal(handentity.PDAYF);
                                }

                                if (!string.IsNullOrEmpty(handentity.TOTALJSUTTLE))
                                {
                                    orderentry.YFPIWEIGHT = handentity.TOTALJSUTTLE.ToDecimal() * 0.001M;
                                    var jb1 = handentity.JIABAN1.ToDecimal() * handentity.JBAMOUNT1.ToDecimal() * 0.001M;
                                    var jb2 = handentity.JIABAN2.ToDecimal() * handentity.JBAMOUNT2.ToDecimal() * 0.001M;
                                    var jb3 = handentity.JIABAN3.ToDecimal() * handentity.JBAMOUNT3.ToDecimal() * 0.001M;
                                    orderentry.DEF6 = ((jb1 + jb2 + jb3) * 1000).ToString();
                                    var js1 = handentity.JTARE1.ToDecimal() * handentity.JSAMOUNT1.ToDecimal() * 0.001M;
                                    var js2 = handentity.JTARE2.ToDecimal() * handentity.JSAMOUNT2.ToDecimal() * 0.001M;
                                    var js3 = handentity.JTARE3.ToDecimal() * handentity.JSAMOUNT3.ToDecimal() * 0.001M;
                                    orderentry.DEF7 = ((js1 + js2 + js3) * 1000).ToString();
                                    orderentry.DEF15 = jb1 + jb2 + jb3 + js1 + js2 + js3;
                                }
                                orderentry.PK_STORE = handentity.PK_STORE;
                                orderentry.STORE = handentity.STORE;
                                orderentry.PK_RECEIVESTORE = handentity.PK_STORE;
                                orderentry.RECEIVESTORE = handentity.STORE;
                                orderentry.DEF3 = handentity.PDASSJS;
                                orderentry.DEF4 = handentity.TOTALJSUTTLE;
                                orderentry.DEF5 = handentity.TOTALJTARE;
                                orderentry.DEF9 = handentity.MEMO;

                            }
                            #endregion
                            bol = CheckContract_CG(orderentry, ref msg);
                            if (bol == false)
                            {
                                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + msg }.ToString());
                            }
                            orderentry.PK_TRAFFICCOMPANY = entity.PK_TRAFFICCOMPANY;
                            orderentry.TRAFFICCOMPANY = entity.TRAFFICCOMPANY;
                            orderentry.WEIGHTDATE = entity.WEIGHTDATE;

                            if (!string.IsNullOrEmpty(entity.LIGHTDATE))
                            {
                                orderentry.LIGHTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            }
                            else
                            {
                                orderentry.LIGHTDATE = entity.LIGHTDATE;
                            }
                            orderentry.LASTDATE = entity.LIGHTDATE;


                            if (!string.IsNullOrEmpty(orderentry.MEMO))
                            {
                                orderentry.MEMO = orderentry.MEMO.Replace("null", "").Replace(@"&nbsp;", "");
                            }
                            if (!string.IsNullOrEmpty(entity.VMEMO))
                            {
                                orderentry.VMEMO = entity.VMEMO.Replace("null", "").Replace(@"&nbsp;", "");
                            }

                            #endregion
                            if (string.IsNullOrEmpty(orderentry.BILLNO))
                            {
                                //插入明细磅单
                                orderentry.BILLNO = string.Format("{0}0{1}", entity.BILLNO, index);
                                orderentry.OUTGBUSER = ManageProvider.Provider.Current().UserName;
                                orderentry.INGBUSER = ManageProvider.Provider.Current().UserName;
                                database.Insert(orderentry, isOpenTrans);
                            }
                            else
                            {
                                //更新明细磅单
                                // database.Update(orderentry, isOpenTrans);
                            }
                            //更新派车明细二次计量状态
                            pcdtl_entity.ISUSE = "2";
                        }
                        orderentry.FINISH = entity.FINISH;
                        #endregion

                        #region 派车单明细更新
                        if (orderentry.PK_ORDER_B == pcdtl_entity.ID && orderentry.PK_BILL_B != pcdtl_entity.PK_ORDER_B)
                        {
                            //更新派车明细表信息
                            pcdtl_entity.PK_ORDER = orderentry.PK_BILL;
                            pcdtl_entity.PK_ORDER_B = orderentry.PK_BILL_B;
                            pcdtl_entity.VBILLCODE = orderentry.PK_CONTRACT_B;
                            pcdtl_entity.PK_SUPPLIER = orderentry.PK_SENDORG;
                            pcdtl_entity.SUPPLIERNAME = orderentry.SENDORG;
                            pcdtl_entity.PK_MATERIAL = orderentry.PK_MATERIAL;
                            pcdtl_entity.MATERIALNAME = orderentry.MATERIAL;
                            pcdtl_entity.MATERIALSPEC = orderentry.MATERIALKIND;
                            pcdtl_entity.PK_ORG = orderentry.PK_RECEIVEORG;
                            pcdtl_entity.ORGNAME = orderentry.RECEIVEORG;
                            if (!string.IsNullOrEmpty(orderentry.PK_DEPARTMENT))
                            {
                                pcdtl_entity.PK_DEPT = orderentry.PK_DEPARTMENT;
                                pcdtl_entity.DEPTNAME = orderentry.DEPARTMENTNAME;
                            }
                            pcdtl_entity.DEF1 = orderentry.DEF1;
                            pcdtl_entity.DEF2 = orderentry.DEF2;
                            if (index == 1)
                            {
                                //更新派车主表信息
                                pc_entity.PK_SUPPLIER = orderentry.PK_SENDORG;
                                pc_entity.SUPPLIERNAME = orderentry.SENDORG;

                            }
                        }
                        //更新派车明细
                        database.Update(pcdtl_entity, isOpenTrans);
                        #endregion
                        index++;
                    }
                }

                if (entity.FINISH == "1")
                {
                    //删除中间表
                    database.Delete("DP_CARSORDER", "ORDERID", entity.PK_ORDER);
                    //主表出厂状态
                    pc_entity.ISUSE = "6";
                    pc_entity.CARS = entity.PK_CARSID;
                    pc_entity.CARSNAME = entity.CARS;
                    pc_entity.DRIVERS = entity.PK_DRIVERS;
                    pc_entity.DRIVERSNAME = entity.DRIVERS;
                    pc_entity.TRAFFICCOMPANY = entity.PK_TRAFFICCOMPANY;
                    pc_entity.TRAFFICCOMPANYNAME = entity.TRAFFICCOMPANY;
                }
                //更新派车主表
                database.Update(pc_entity, isOpenTrans);
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采购磅单 :{0}，车辆:{1}，参照成功。", entity.BILLNO, entity.CARS);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Update, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //二次参照
        [HttpPost]
        public ActionResult SubmitOrderFormEdit4(string KeyValue, string account, WB_WEIGHT_B entity, string OrderEntryJson, string ModuleId, string type)
        {
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<APP_HANDORDER> OrderEntryList = OrderEntryJson.JonsToList<APP_HANDORDER>();
                //处理主头
                string Message = "编辑成功。";
                string msg = "";
                bool bol = false;

                #region 计量单明细
                WB_WEIGHT_B edit_entity_dtl = null;
                edit_entity_dtl = database.FindEntity<WB_WEIGHT_B>(KeyValue);

                #region 验证
                if (type == "0")
                {
                    if (edit_entity_dtl != null)
                    {
                        if (edit_entity_dtl.GROSS != entity.GROSS || edit_entity_dtl.TARE != entity.TARE)
                        {
                            return Content(new JsonMessage { Success = false, Code = "-2", Message = string.Format("该计量单的毛重({0})、皮重({1})、净重({2})与修改前毛重({3})、皮重({4})、净重({5})不一致！", edit_entity_dtl.GROSS, edit_entity_dtl.TARE, edit_entity_dtl.SUTTLE, entity.GROSS, entity.TARE, entity.SUTTLE) }.ToString());
                        }
                    }
                }
                //if (edit_entity_dtl.ISJS > 0)
                //{
                //    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}过磅单:{1}已结算！", entity.CARS, entity.BILLNO) }.ToString());
                //}
                #endregion

                #region 磅单明细日志记录
                strSql.Clear();
                string memo = "";
                if (!string.IsNullOrEmpty(entity.VMEMO))
                {
                    memo = entity.VMEMO.Replace(@"&nbsp;", "");
                }

                strSql.Append(string.Format("INSERT INTO WB_WEIGHT_B_V(BILLNO,PK_CARSID,PK_RECEIVEORG,PK_SENDORG,PK_RECEIVESTORE,PK_SENDSTORE,PK_DRIVERS,PK_MATERIAL,PK_MATERIALKIND,PK_TRAFFICCOMPANY,PK_STORE,PK_ORG,CARS,RECEIVEORG,SENDORG,RECEIVESTORE,SENDSTORE,DRIVERS,MATERIAL,MATERIALKIND,TRAFFICCOMPANY,STORE,BATHNO,WEIGHTNO,LIGHTNO,WEIGHTDATE,LIGHTDATE,CREATEDATE,LASTDATE,PRINTDATE,PRINTCOUNT,YFTARE,YFSUTTLE_TMP,GROSS,TARE,SUTTLE,PRICE,CHARGE,TRAFFICPRICE,TRAFFICCHARGE,PDAUSER,PDANO,PDADATE,PDAYF,PDAKZ1,PDAKZ2,INGBUSER,INDBUSER,OUTGBUSER,OUTDBUSER,TYPE,COMPUTERTYPE,UPLOAD,ICCARD,IDCARD,RFIDCARD,TWOBANG,FINISH,STATUS,PK_ORDER,PK_ORDER_B,PK_CONTRACT,PK_CONTRACT_B,PK_TASK,DEF1,DEF2,DEF3,DEF4,DEF5,DEF6,DEF7,DEF8,DEF9,DEF10,DEF11,DEF12,DEF13,DEF14,DEF15,DEF16,DEF17,DEF18,DEF19,DEF20,DATATYPE,UPLOAD1,YFPIECE,YFPIWEIGHT,BALANCEMETHOD,SURPRINT,PK_BILL_B,PK_BILL,BALANCEMETHODNAME,COMPANYNAME,PK_COMPANY,PK_DEPARTMENT,DEPARTMENTNAME,YFGROSS,YFSUTTLE,PK_ARRIVO,PK_REPORT,ISJS,UPDATEDATE,UPDATEUSER,CHECKUSER,DELETEDATE,DELETEUSER,ID,MEMO)"));
                strSql.Append(string.Format(" VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity_dtl.BILLNO, edit_entity_dtl.PK_CARSID, edit_entity_dtl.PK_RECEIVEORG, edit_entity_dtl.PK_SENDORG, edit_entity_dtl.PK_RECEIVESTORE, edit_entity_dtl.PK_SENDSTORE, edit_entity_dtl.PK_DRIVERS, edit_entity_dtl.PK_MATERIAL, edit_entity_dtl.PK_MATERIALKIND, edit_entity_dtl.PK_TRAFFICCOMPANY));
                strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity_dtl.PK_STORE, edit_entity_dtl.PK_ORG, edit_entity_dtl.CARS, edit_entity_dtl.RECEIVEORG, edit_entity_dtl.SENDORG, edit_entity_dtl.RECEIVESTORE, edit_entity_dtl.SENDSTORE, edit_entity_dtl.DRIVERS, edit_entity_dtl.MATERIAL, edit_entity_dtl.MATERIALKIND));
                strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity_dtl.TRAFFICCOMPANY, edit_entity_dtl.STORE, edit_entity_dtl.BATHNO, edit_entity_dtl.WEIGHTNO, edit_entity_dtl.LIGHTNO, edit_entity_dtl.WEIGHTDATE, edit_entity_dtl.LIGHTDATE, edit_entity_dtl.CREATEDATE, edit_entity_dtl.LASTDATE, edit_entity_dtl.PRINTDATE));
                strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity_dtl.PRINTCOUNT, edit_entity_dtl.YFTARE, edit_entity_dtl.YFSUTTLE_TMP, edit_entity_dtl.GROSS, edit_entity_dtl.TARE, edit_entity_dtl.SUTTLE, edit_entity_dtl.PRICE, edit_entity_dtl.CHARGE, edit_entity_dtl.TRAFFICPRICE, edit_entity_dtl.TRAFFICCHARGE));
                strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity_dtl.PDAUSER, edit_entity_dtl.PDANO, edit_entity_dtl.PDADATE, edit_entity_dtl.PDAYF, edit_entity_dtl.PDAKZ1, edit_entity_dtl.PDAKZ2, edit_entity_dtl.INGBUSER, edit_entity_dtl.INDBUSER, edit_entity_dtl.OUTGBUSER, edit_entity_dtl.OUTDBUSER));
                strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity_dtl.TYPE, edit_entity_dtl.COMPUTERTYPE, edit_entity_dtl.UPLOAD, edit_entity_dtl.ICCARD, edit_entity_dtl.IDCARD, edit_entity_dtl.RFIDCARD, edit_entity_dtl.TWOBANG, edit_entity_dtl.FINISH, edit_entity_dtl.STATUS, edit_entity_dtl.PK_ORDER));
                strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity_dtl.PK_ORDER_B, edit_entity_dtl.PK_CONTRACT, edit_entity_dtl.PK_CONTRACT_B, edit_entity_dtl.PK_TASK, edit_entity_dtl.DEF1, edit_entity_dtl.DEF2, edit_entity_dtl.DEF3, edit_entity_dtl.DEF4, edit_entity_dtl.DEF5, edit_entity_dtl.DEF6));
                strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity_dtl.DEF7, edit_entity_dtl.DEF8, edit_entity_dtl.DEF9, edit_entity_dtl.DEF10, edit_entity_dtl.DEF11, edit_entity_dtl.DEF12, edit_entity_dtl.DEF13, edit_entity_dtl.DEF14, edit_entity_dtl.DEF15, edit_entity_dtl.DEF16));
                strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity_dtl.DEF17, edit_entity_dtl.DEF18, edit_entity_dtl.DEF19, edit_entity_dtl.DEF20, edit_entity_dtl.DATATYPE, edit_entity_dtl.UPLOAD1, edit_entity_dtl.YFPIECE, edit_entity_dtl.YFPIWEIGHT, edit_entity_dtl.BALANCEMETHOD, edit_entity_dtl.SURPRINT));
                strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity_dtl.PK_BILL_B, edit_entity_dtl.PK_BILL, edit_entity_dtl.BALANCEMETHODNAME, edit_entity_dtl.COMPANYNAME, edit_entity_dtl.PK_COMPANY, edit_entity_dtl.PK_DEPARTMENT, edit_entity_dtl.DEPARTMENTNAME, edit_entity_dtl.YFGROSS, edit_entity_dtl.YFSUTTLE, edit_entity_dtl.PK_ARRIVO));
                strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", edit_entity_dtl.PK_REPORT, edit_entity_dtl.ISJS, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ManageProvider.Provider.Current().UserName, account, "", "", CommonHelper.GetGuid, memo));
                database.ExecuteBySql(strSql, isOpenTrans);
                #endregion

                edit_entity_dtl.PK_BILL = entity.PK_BILL;
                edit_entity_dtl.PK_BILL_B = entity.PK_BILL_B;
                edit_entity_dtl.PK_CONTRACT = entity.PK_CONTRACT;
                edit_entity_dtl.PK_CONTRACT_B = entity.PK_CONTRACT_B;
                edit_entity_dtl.PK_CARSID = entity.PK_CARSID;
                edit_entity_dtl.CARS = entity.CARS;
                edit_entity_dtl.PK_DRIVERS = entity.PK_DRIVERS;
                edit_entity_dtl.DRIVERS = entity.DRIVERS;
                edit_entity_dtl.PK_RECEIVEORG = entity.PK_RECEIVEORG;
                edit_entity_dtl.RECEIVEORG = entity.RECEIVEORG;
                edit_entity_dtl.PK_STORE = entity.PK_STORE;
                edit_entity_dtl.STORE = entity.STORE;
                edit_entity_dtl.PK_RECEIVESTORE = entity.PK_RECEIVESTORE;
                edit_entity_dtl.RECEIVESTORE = entity.RECEIVESTORE;
                edit_entity_dtl.PK_TRAFFICCOMPANY = entity.PK_TRAFFICCOMPANY;
                edit_entity_dtl.TRAFFICCOMPANY = entity.TRAFFICCOMPANY;
                edit_entity_dtl.YFSUTTLE = entity.YFSUTTLE;
                edit_entity_dtl.GROSS = entity.GROSS;
                edit_entity_dtl.TARE = entity.TARE;
                edit_entity_dtl.SUTTLE = entity.SUTTLE;
                edit_entity_dtl.DEF3 = entity.DEF3;
                edit_entity_dtl.DEF11 = entity.DEF11;
                edit_entity_dtl.VMEMO = entity.VMEMO;
                edit_entity_dtl.ISEDIT = 1;
                edit_entity_dtl.TRAFFICCHARGE = entity.TRAFFICCHARGE;
                edit_entity_dtl.DEF17 = entity.DEF17;
                edit_entity_dtl.OUTGBUSER = ManageProvider.Provider.Current().UserName;
                //edit_entity_dtl.INGBUSER = ManageProvider.Provider.Current().UserName;
                edit_entity_dtl.LASTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                #endregion

                #region 计量主表
                WB_WEIGHT edit_entity = null;
                edit_entity = database.FindEntity<WB_WEIGHT>(edit_entity_dtl.PK_TASK);
                #endregion

                #region 收料表
                APP_HANDORDER edit_handentity = null;
                edit_handentity = database.FindEntityByWhere<APP_HANDORDER>(string.Format(" and ID='{0}'", edit_entity_dtl.PK_ORDER_B));
                #endregion


                int index = 1;
                #region 收货确认实例
                string JZXCODE = "";
                foreach (APP_HANDORDER handentity in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(handentity.ID))
                    {
                        if (!string.IsNullOrEmpty(handentity.JZXCODE))
                        {
                            JZXCODE = handentity.JZXCODE;
                        }

                        #region  更新收货
                        if (edit_entity_dtl.PK_ORDER_B == handentity.ID)
                        {
                            edit_entity.DEF1 = handentity.MEMO;
                            edit_entity_dtl.DEF9 = handentity.MEMO;
                            edit_entity_dtl.BALANCEMETHOD = handentity.JZXCODE;
                        }
                        edit_handentity.PK_STORE = handentity.PK_STORE;
                        edit_handentity.STORE = handentity.STORE;
                        edit_handentity.PDAYF = handentity.PDAYF;
                        edit_handentity.PDAKZ1 = handentity.PDAKZ1;
                        edit_handentity.JZXCODE = handentity.JZXCODE;
                        edit_handentity.JTARE1 = handentity.JTARE1;
                        edit_handentity.JTARE2 = handentity.JTARE2;
                        edit_handentity.JTARE3 = handentity.JTARE3;
                        edit_handentity.JSUTTLE1 = handentity.JSUTTLE1;
                        edit_handentity.JSUTTLE2 = handentity.JSUTTLE2;
                        edit_handentity.JSUTTLE3 = handentity.JSUTTLE3;
                        edit_handentity.JSAMOUNT1 = handentity.JSAMOUNT1;
                        edit_handentity.JSAMOUNT2 = handentity.JSAMOUNT2;
                        edit_handentity.JSAMOUNT3 = handentity.JSAMOUNT3;
                        edit_handentity.TOTALJTARE = handentity.TOTALJTARE;
                        edit_handentity.TOTALJSUTTLE = handentity.TOTALJSUTTLE;
                        edit_handentity.PDASSJS = handentity.PDASSJS;
                        edit_handentity.JBAMOUNT1 = handentity.JBAMOUNT1;
                        edit_handentity.JBAMOUNT2 = handentity.JBAMOUNT2;
                        edit_handentity.JBAMOUNT3 = handentity.JBAMOUNT3;
                        edit_handentity.JIABAN1 = handentity.JIABAN1;
                        edit_handentity.JIABAN2 = handentity.JIABAN2;
                        edit_handentity.JIABAN3 = handentity.JIABAN3;
                        edit_handentity.MEMO = handentity.MEMO;
                        database.Update(edit_handentity, isOpenTrans);
                        #endregion

                        #region 计量明细
                        edit_entity_dtl.PDAKZ1 = (handentity.PDAKZ1.ToDecimal() * 0.001M).ToString("f3"); ;
                        if (!string.IsNullOrEmpty(handentity.PDAYF))
                        {
                            edit_entity_dtl.YFSUTTLE = Convert.ToDecimal(handentity.PDAYF);
                        }

                        if (!string.IsNullOrEmpty(handentity.TOTALJSUTTLE))
                        {
                            edit_entity_dtl.YFPIWEIGHT = handentity.TOTALJSUTTLE.ToDecimal() * 0.001M;
                            var jb1 = handentity.JIABAN1.ToDecimal() * handentity.JBAMOUNT1.ToDecimal() * 0.001M;
                            var jb2 = handentity.JIABAN2.ToDecimal() * handentity.JBAMOUNT2.ToDecimal() * 0.001M;
                            var jb3 = handentity.JIABAN3.ToDecimal() * handentity.JBAMOUNT3.ToDecimal() * 0.001M;
                            edit_entity_dtl.DEF6 = ((jb1 + jb2 + jb3) * 1000).ToString();
                            var js1 = handentity.JTARE1.ToDecimal() * handentity.JSAMOUNT1.ToDecimal() * 0.001M;
                            var js2 = handentity.JTARE2.ToDecimal() * handentity.JSAMOUNT2.ToDecimal() * 0.001M;
                            var js3 = handentity.JTARE3.ToDecimal() * handentity.JSAMOUNT3.ToDecimal() * 0.001M;
                            edit_entity_dtl.DEF7 = ((js1 + js2 + js3) * 1000).ToString();
                            edit_entity_dtl.DEF15 = jb1 + jb2 + jb3 + js1 + js2 + js3;
                        }

                        edit_entity_dtl.DEF3 = handentity.PDASSJS;
                        edit_entity_dtl.DEF4 = handentity.TOTALJSUTTLE;
                        edit_entity_dtl.DEF5 = handentity.TOTALJTARE;
                        edit_entity_dtl.DEF9 = handentity.MEMO;

                        //确认保存
                        if (type == "1")
                        {
                            //计算
                            bol = CheckContract_CG2(edit_entity_dtl, ref msg);
                        }
                        else
                        {
                            //不计算
                            bol = CheckContract_CG0(edit_entity_dtl, ref msg);
                        }
                        #endregion
                        index++;
                    }
                }
                #endregion
                //更新磅单明细表
                database.Update(edit_entity_dtl, isOpenTrans);

                if (edit_entity != null)
                {
                    #region 磅单主表日志记录
                    strSql.Clear();
                    strSql.Append(string.Format("INSERT INTO WB_WEIGHT_V(BILLNO,PK_CARSID,PK_RECEIVEORG,PK_SENDORG,PK_RECEIVESTORE,PK_SENDSTORE,PK_DRIVERS,PK_MATERIAL,PK_MATERIALKIND,PK_TRAFFICCOMPANY,PK_STORE,PK_ORG,CARS,RECEIVEORG,SENDORG,RECEIVESTORE,SENDSTORE,DRIVERS,MATERIAL,MATERIALKIND,TRAFFICCOMPANY,STORE,BATHNO,WEIGHTNO,LIGHTNO,WEIGHTDATE,LIGHTDATE,CREATEDATE,LASTDATE,PRINTDATE,PRINTCOUNT,YFTARE,YFSUTTLE_TMP,GROSS,TARE,SUTTLE,PRICE,CHARGE,TRAFFICPRICE,TRAFFICCHARGE,PDAUSER,PDANO,PDADATE,PDAYF,PDAKZ1,PDAKZ2,INGBUSER,INDBUSER,OUTGBUSER,OUTDBUSER,TYPE,COMPUTERTYPE,UPLOAD,ICCARD,IDCARD,RFIDCARD,TWOBANG,FINISH,STATUS,PK_ORDER,PK_ORDER_B,PK_CONTRACT,PK_CONTRACT_B,PK_TASK,DEF1,DEF2,DEF3,DEF4,DEF5,DEF6,DEF7,DEF8,DEF9,DEF10,DEF11,DEF12,DEF13,DEF14,DEF15,DEF16,DEF17,DEF18,DEF19,DEF20,DATATYPE,UPLOAD1,YFPIECE,YFPIWEIGHT,BALANCEMETHOD,SURPRINT,PK_BILL_B,PK_BILL,BALANCEMETHODNAME,COMPANYNAME,PK_COMPANY,PK_DEPARTMENT,DEPARTMENTNAME,YFGROSS,YFSUTTLE,PK_ARRIVO,PK_REPORT,ISJS,UPDATEDATE,UPDATEUSER,CHECKUSER,DELETEDATE,DELETEUSER,ID,MEMO) "));
                    //SELECT BILLNO,PK_CARSID,PK_RECEIVEORG,PK_SENDORG,PK_RECEIVESTORE,PK_SENDSTORE,PK_DRIVERS,PK_MATERIAL,PK_MATERIALKIND,PK_TRAFFICCOMPANY,PK_STORE,PK_ORG,CARS,RECEIVEORG,SENDORG,RECEIVESTORE,SENDSTORE,DRIVERS,MATERIAL,MATERIALKIND,TRAFFICCOMPANY,STORE,BATHNO,WEIGHTNO,LIGHTNO,WEIGHTDATE,LIGHTDATE,CREATEDATE,LASTDATE,PRINTDATE,PRINTCOUNT,YFTARE,YFSUTTLE_TMP,GROSS,TARE,SUTTLE,PRICE,CHARGE,TRAFFICPRICE,TRAFFICCHARGE,PDAUSER,PDANO,PDADATE,PDAYF,PDAKZ1,PDAKZ2,INGBUSER,INDBUSER,OUTGBUSER,OUTDBUSER,TYPE,COMPUTERTYPE,UPLOAD,ICCARD,IDCARD,RFIDCARD,TWOBANG,FINISH,STATUS,PK_ORDER,PK_ORDER_B,PK_CONTRACT,PK_CONTRACT_B,PK_TASK,DEF1,DEF2,DEF3,DEF4,DEF5,DEF6,DEF7,DEF8,DEF9,DEF10,DEF11,DEF12,DEF13,DEF14,DEF15,DEF16,DEF17,DEF18,DEF19,DEF20,DATATYPE,UPLOAD1,YFPIECE,YFPIWEIGHT,BALANCEMETHOD,SURPRINT,PK_BILL_B,PK_BILL,BALANCEMETHODNAME,COMPANYNAME,PK_COMPANY,PK_DEPARTMENT,DEPARTMENTNAME,YFGROSS,YFSUTTLE,PK_ARRIVO,PK_REPORT,ISJS,'{0}' as UPDATEDATE,'{1}' as UPDATEUSER,'{2}' as CHECKUSER,'' as DELETEDATE,'' as DELETEUSER,'{3}' as ID,'{4}' as MEMO from WB_WEIGHT where BILLNO='{5}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ManageProvider.Provider.Current().UserName, account, CommonHelper.GetGuid, entity.MEMO, edit_entity.BILLNO));
                    //PK_REPORT,ISJS,'{0}' as UPDATEDATE,'{1}' as UPDATEUSER,'{2}' as CHECKUSER,'' as DELETEDATE,'' as DELETEUSER,'{3}' as ID,'{4}' as MEMO
                    strSql.Append(string.Format(" VALUES('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity.BILLNO, edit_entity.PK_CARSID, edit_entity.PK_RECEIVEORG, edit_entity.PK_SENDORG, edit_entity.PK_RECEIVESTORE, edit_entity.PK_SENDSTORE, edit_entity.PK_DRIVERS, edit_entity.PK_MATERIAL, edit_entity.PK_MATERIALKIND, edit_entity.PK_TRAFFICCOMPANY));
                    strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity.PK_STORE, edit_entity.PK_ORG, edit_entity.CARS, edit_entity.RECEIVEORG, edit_entity.SENDORG, edit_entity.RECEIVESTORE, edit_entity.SENDSTORE, edit_entity.DRIVERS, edit_entity.MATERIAL, edit_entity.MATERIALKIND));
                    strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity.TRAFFICCOMPANY, edit_entity.STORE, edit_entity.BATHNO, edit_entity.WEIGHTNO, edit_entity.LIGHTNO, edit_entity.WEIGHTDATE, edit_entity.LIGHTDATE, edit_entity.CREATEDATE, edit_entity.LASTDATE, edit_entity.PRINTDATE));
                    //PRINTCOUNT,YFTARE,YFSUTTLE_TMP,GROSS,TARE,SUTTLE,PRICE,CHARGE,TRAFFICPRICE,TRAFFICCHARGE
                    strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity.PRINTCOUNT, edit_entity.YFTARE, edit_entity.YFSUTTLE_TMP, edit_entity.GROSS, edit_entity.TARE, edit_entity.SUTTLE, edit_entity.PRICE, edit_entity.CHARGE, edit_entity.TRAFFICPRICE, edit_entity.TRAFFICCHARGE));
                    //PDAUSER,PDANO,PDADATE,PDAYF,PDAKZ1,PDAKZ2,INGBUSER,INDBUSER,OUTGBUSER,OUTDBUSER
                    strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity.PDAUSER, edit_entity.PDANO, edit_entity.PDADATE, edit_entity.PDAYF, edit_entity.PDAKZ1, edit_entity.PDAKZ2, edit_entity.INGBUSER, edit_entity.INDBUSER, edit_entity.OUTGBUSER, edit_entity.OUTDBUSER));
                    //TYPE,COMPUTERTYPE,UPLOAD,ICCARD,IDCARD,RFIDCARD,TWOBANG,FINISH,STATUS,PK_ORDER
                    strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity.TYPE, edit_entity.COMPUTERTYPE, edit_entity.UPLOAD, edit_entity.ICCARD, edit_entity.IDCARD, edit_entity.RFIDCARD, edit_entity.TWOBANG, edit_entity.FINISH, edit_entity.STATUS, edit_entity.PK_ORDER));
                    //PK_ORDER_B,PK_CONTRACT,PK_CONTRACT_B,PK_TASK,DEF1,DEF2,DEF3,DEF4,DEF5,DEF6
                    strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity.PK_ORDER_B, edit_entity.PK_CONTRACT, edit_entity.PK_CONTRACT_B, edit_entity.PK_TASK, edit_entity.DEF1, edit_entity.DEF2, edit_entity.DEF3, edit_entity.DEF4, edit_entity.DEF5, edit_entity.DEF6));
                    //DEF7,DEF8,DEF9,DEF10,DEF11,DEF12,DEF13,DEF14,DEF15,DEF16
                    strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity.DEF7, edit_entity.DEF8, edit_entity.DEF9, edit_entity.DEF10, edit_entity.DEF11, edit_entity.DEF12, edit_entity.DEF13, edit_entity.DEF14, edit_entity.DEF15, edit_entity.DEF16));
                    //DEF17,DEF18,DEF19,DEF20,DATATYPE,UPLOAD1,YFPIECE,YFPIWEIGHT,BALANCEMETHOD,SURPRINT
                    strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity.DEF17, edit_entity.DEF18, edit_entity.DEF19, edit_entity.DEF20, edit_entity.DATATYPE, edit_entity.UPLOAD1, edit_entity.YFPIECE, edit_entity.YFPIWEIGHT, edit_entity.BALANCEMETHOD, edit_entity.SURPRINT));
                    //PK_BILL_B,PK_BILL,BALANCEMETHODNAME,COMPANYNAME,PK_COMPANY,PK_DEPARTMENT,DEPARTMENTNAME,YFGROSS,YFSUTTLE,PK_ARRIVO
                    strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}',", edit_entity.PK_BILL_B, edit_entity.PK_BILL, edit_entity.BALANCEMETHODNAME, edit_entity.COMPANYNAME, edit_entity.PK_COMPANY, edit_entity.PK_DEPARTMENT, edit_entity.DEPARTMENTNAME, edit_entity.YFGROSS, edit_entity.YFSUTTLE, edit_entity.PK_ARRIVO));
                    strSql.Append(string.Format(" '{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}')", edit_entity.PK_REPORT, edit_entity.ISJS, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ManageProvider.Provider.Current().UserName, account, "", "", CommonHelper.GetGuid, memo));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    #endregion

                    List<WB_WEIGHT_B> list_entity_dtl = database.FindListBySql<WB_WEIGHT_B>(string.Format("select * from  WB_WEIGHT_B where PK_TASK='{0}' and PK_ORDER_B !='{1}'", entity.PK_TASK, entity.PK_ORDER_B));
                    list_entity_dtl.Add(edit_entity_dtl);
                    //更新过磅主表
                    edit_entity.PK_STORE = entity.PK_STORE;
                    edit_entity.STORE = entity.STORE;
                    edit_entity.PK_RECEIVESTORE = entity.PK_RECEIVESTORE;
                    edit_entity.RECEIVESTORE = entity.RECEIVESTORE;
                    edit_entity.SUTTLE = list_entity_dtl.Sum(p => p.SUTTLE);

                    if (list_entity_dtl.Count == 1)
                    {
                        edit_entity.GROSS = entity.GROSS;
                        edit_entity.TARE = entity.TARE;
                    }
                    else
                    {
                        edit_entity.GROSS = list_entity_dtl.Sum(p => p.GROSS);
                        edit_entity.TARE = list_entity_dtl.Sum(p => p.TARE);
                    }
                    //接收公司
                    edit_entity.PK_RECEIVEORG = entity.PK_RECEIVEORG;
                    edit_entity.RECEIVEORG = entity.RECEIVEORG;
                    edit_entity.DEF11 = entity.DEF11;
                    edit_entity.ISEDIT = 1;
                    edit_entity.PK_CARSID = entity.PK_CARSID;
                    edit_entity.CARS = entity.CARS;
                    edit_entity.PK_DRIVERS = entity.PK_DRIVERS;
                    edit_entity.DRIVERS = entity.DRIVERS;
                    edit_entity.VMEMO = edit_entity.VMEMO + entity.VMEMO;
                    edit_entity.BALANCEMETHOD = JZXCODE;
                    edit_entity.OUTGBUSER = ManageProvider.Provider.Current().UserName;
                    //edit_entity.INGBUSER = ManageProvider.Provider.Current().UserName;
                    edit_entity.LASTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    database.Update(edit_entity, isOpenTrans);
                    if (!string.IsNullOrEmpty(JZXCODE))
                    {
                        if (!string.IsNullOrEmpty(edit_entity.PK_ORDER))
                        {
                            strSql.Clear();
                            strSql.Append(string.Format("update DP_POCARSORDER set DEF4='{0}' where ID='{0}'", JZXCODE, edit_entity.PK_ORDER));
                            database.ExecuteBySql(strSql, isOpenTrans);
                        }
                    }
                }

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采购磅单 :{0}，车辆:{1}，参照成功。", entity.BILLNO, entity.CARS);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Update, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        internal bool CheckContract_CG2(WB_WEIGHT_B weightfinish, ref string msg)
        {
            //实际净重数据大于0时计算按实际重量进行计算
            if (weightfinish.YFGROSS > 0 && weightfinish.YFTARE > 0)
            {
                weightfinish.GROSS = weightfinish.YFGROSS;
                weightfinish.TARE = weightfinish.YFTARE;
                weightfinish.SUTTLE = weightfinish.YFGROSS - weightfinish.YFTARE;
            }


            BD_MaterialBll bd_bll = new BD_MaterialBll();
            VBD_MATERIAL mat_entity = bd_bll.GetEntity(weightfinish.PK_MATERIAL);

            if (string.IsNullOrEmpty(weightfinish.DEF3))
                weightfinish.DEF3 = "0";

            string SQL = string.Format("SELECT NASTNUM,SECQUANTITY,YLNUM,NPRICE,secpurunitname,purunitname,vbdef2,materialcode,def6 FROM VNC_PO_ORDER_YL_CS WHERE PK_ORDER_B='{0}' AND rownum < 2", weightfinish.PK_BILL_B);
            System.Data.DataTable dt = DataFactory.Database().FindTableBySql(SQL); //cardtools.QueryTable(SQL);
            if (dt != null && dt.Rows.Count > 0)
            {
                #region 判断换算率 按数量 按重量  千克 件 吨
                weightfinish.PRICE = Convert.ToDecimal(dt.Rows[0]["NPRICE"].ToString());
                string strunid = dt.Rows[0]["secpurunitname"].ToString();
                decimal ylnum = Convert.ToDecimal(dt.Rows[0]["YLNUM"].ToString());
                string strkd = dt.Rows[0]["vbdef2"].ToString().Trim();
                string strmaterial = dt.Rows[0]["materialcode"].ToString();
                weightfinish.DEF1 = dt.Rows[0]["PURUNITNAME"].ToString();
                weightfinish.DEF2 = dt.Rows[0]["SECPURUNITNAME"].ToString();
                if (string.IsNullOrEmpty(strkd))
                {
                    strkd = "0";
                }
                string strrc = dt.Rows[0]["def6"].ToString().Trim();
                if (string.IsNullOrEmpty(strrc))
                {
                    strrc = "0";
                }
                decimal ylrc = strrc.ToDecimal();
                bool ischa = true;
                string unittype = "重量";
                decimal rate = 1;
                decimal fwmin = -0.5M;
                decimal fwmax = 0.5M;
                if (weightfinish.DEF2 == "千克")
                {
                    rate = 1000;
                }

                #endregion

                decimal cha = 0M;
                if (weightfinish.DEF2 == "件")
                {
                    if (weightfinish.YFSUTTLE != weightfinish.DEF3.ToDecimal())
                    {
                        //msg = "原发和实收件数不一致不能过磅";
                        //return false;
                    }
                    else
                    {
                        weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                        weightfinish.YFPIECE = weightfinish.DEF3.ToDecimal();
                        //应收=主数量/辅数量
                        decimal numjz = dt.Rows[0]["NASTNUM"].ToString().ToDecimal() / dt.Rows[0]["SECQUANTITY"].ToString().ToDecimal();
                        //木浆件净 
                        decimal numsszl = numjz.ToString("f8").ToDecimal() * weightfinish.YFSUTTLE;//木浆件净*件数=应收
                        weightfinish.DEF16 = numjz.ToString("f8").ToDecimal();//木浆件净
                        //应收保留3位
                        weightfinish.YFPIWEIGHT = numsszl.ToString("f3").ToDecimal();
                        cha = weightfinish.SUTTLE - weightfinish.YFPIWEIGHT;

                        if (cha < 0)
                        {
                            //是否应收对比计算
                            bool isnolimite = mat_entity.ISWRITE == 1 ? true : false;
                            if (isnolimite)
                            {

                            }
                            else
                            {
                                weightfinish.DEF17 = cha;
                                weightfinish.SUTTLE = weightfinish.YFPIWEIGHT;
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                            }
                        }
                    }
                }
                else
                {
                    if (strmaterial.StartsWith("103") && weightfinish.DEF2 == "吨")//能源类 扣重
                    {
                        weightfinish.TRAFFICCHARGE = 0;
                        #region 能源类 扣重
                        //允许路损率
                        weightfinish.CHARGE = Convert.ToDecimal(strkd);//亏吨率
                        fwmin = Convert.ToDecimal(strkd) * 0.001M * weightfinish.YFSUTTLE;
                        if (fwmin > 0)
                        {
                            ///亏吨数量=应收数×（1-允许路损率）-实收数
                            cha = weightfinish.SUTTLE - weightfinish.YFSUTTLE;
                            if (cha > 0)
                            {
                                weightfinish.SUTTLE = weightfinish.YFSUTTLE;
                                weightfinish.YFPIECE = weightfinish.YFSUTTLE;
                            }
                            else if (cha < 0)
                            {
                                var numChaAbs = Math.Abs(cha);
                                if (numChaAbs > fwmin)
                                {
                                    var kd = numChaAbs - fwmin;
                                    weightfinish.TRAFFICCHARGE = kd;//亏吨
                                }
                                weightfinish.YFPIECE = weightfinish.SUTTLE;
                            }
                            else
                            {
                                weightfinish.YFPIECE = weightfinish.SUTTLE;
                            }
                        }
                        else
                        {
                            if (weightfinish.YFSUTTLE > 0)
                            {
                                cha = weightfinish.SUTTLE - weightfinish.YFSUTTLE;
                                if (cha > 0)
                                {
                                    weightfinish.SUTTLE = weightfinish.YFSUTTLE;
                                    weightfinish.YFPIECE = weightfinish.YFSUTTLE;

                                }
                            }
                        }
                        weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                        #endregion
                    }
                    else
                    {
                        if (weightfinish.YFPIWEIGHT > 0)
                        {
                            #region 控制量按 输入件数件皮件净夹板扣重
                            //实际净重-总件皮-总夹板 和 手持机件净比较
                            cha = (weightfinish.SUTTLE - weightfinish.DEF15) - weightfinish.YFPIWEIGHT;
                            if (cha > 0)
                            {
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE + cha;
                                weightfinish.DEF17 = cha;  //赋值DEF17保存涨吨（推到皮上）
                                //净重=应收-手持机扣重
                                weightfinish.SUTTLE = weightfinish.YFPIWEIGHT - weightfinish.PDAKZ1.ToDecimal();
                                //订单计算余量统一使用字段 YFPIECE（如果是按件这个字段存放件数，按重量这个字段存放重量
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }
                            else
                            {
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                                weightfinish.SUTTLE = weightfinish.SUTTLE - weightfinish.DEF15 - weightfinish.PDAKZ1.ToDecimal();
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 其他按重量计量扣重计算
                            weightfinish.YFPIWEIGHT = weightfinish.YFSUTTLE;
                            cha = (weightfinish.SUTTLE - weightfinish.DEF15) * rate - weightfinish.YFSUTTLE;
                            if (cha > 0 && weightfinish.YFSUTTLE > 0)
                            {
                                weightfinish.DEF17 = cha;
                                weightfinish.SUTTLE = weightfinish.YFSUTTLE / rate;
                                weightfinish.YFPIECE = weightfinish.YFSUTTLE;
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                            }
                            else
                            {
                                if (weightfinish.YFSUTTLE > 0)
                                    weightfinish.DEF17 = cha;

                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                                weightfinish.SUTTLE = weightfinish.SUTTLE - weightfinish.DEF15 - weightfinish.PDAKZ1.ToDecimal();
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }

                            #endregion
                        }

                    }
                }

            }
            else
            {
                msg = "订单余量查询异常";
                return false;
            }

            return true;
        }

        internal bool CheckContract_CG1(WB_WEIGHT_B weightfinish, ref string msg)
        {
            //实际净重数据大于0时计算按实际重量进行计算
            if (weightfinish.YFGROSS > 0 && weightfinish.YFTARE > 0)
            {
                weightfinish.GROSS = weightfinish.YFGROSS;
                weightfinish.TARE = weightfinish.YFTARE;
                weightfinish.SUTTLE = weightfinish.YFGROSS - weightfinish.YFTARE;
            }
            BD_MaterialBll bd_bll = new BD_MaterialBll();
            VBD_MATERIAL mat_entity = bd_bll.GetEntity(weightfinish.PK_MATERIAL);

            if (string.IsNullOrEmpty(weightfinish.DEF3))
                weightfinish.DEF3 = "0";

            string SQL = string.Format("SELECT NASTNUM,SECQUANTITY,YLNUM,NPRICE,secpurunitname,purunitname,vbdef2,materialcode,def6 FROM VNC_PO_ORDER_YL_CS WHERE PK_ORDER_B='{0}' AND rownum < 2", weightfinish.PK_BILL_B);
            System.Data.DataTable dt = DataFactory.Database().FindTableBySql(SQL); //cardtools.QueryTable(SQL);
            if (dt != null && dt.Rows.Count > 0)
            {
                #region 判断换算率 按数量 按重量  千克 件 吨
                weightfinish.PRICE = Convert.ToDecimal(dt.Rows[0]["NPRICE"].ToString());
                string strunid = dt.Rows[0]["secpurunitname"].ToString();
                decimal ylnum = Convert.ToDecimal(dt.Rows[0]["YLNUM"].ToString());
                string strkd = dt.Rows[0]["vbdef2"].ToString().Trim();
                string strmaterial = dt.Rows[0]["materialcode"].ToString();
                weightfinish.DEF1 = dt.Rows[0]["PURUNITNAME"].ToString();
                weightfinish.DEF2 = dt.Rows[0]["SECPURUNITNAME"].ToString();
                if (string.IsNullOrEmpty(strkd))
                {
                    strkd = "0";
                }
                string strrc = dt.Rows[0]["def6"].ToString().Trim();
                if (string.IsNullOrEmpty(strrc))
                {
                    strrc = "0";
                }
                decimal ylrc = strrc.ToDecimal();
                bool ischa = true;
                string unittype = "重量";
                decimal rate = 1;
                decimal fwmin = -0.5M;
                decimal fwmax = 0.5M;
                if (weightfinish.DEF2 == "千克")
                {
                    rate = 1000;
                }

                #endregion

                decimal cha = 0M;
                if (weightfinish.DEF2 == "件")
                {
                    if (weightfinish.YFSUTTLE != weightfinish.DEF3.ToDecimal())
                    {
                        //msg = "原发和实收件数不一致不能过磅";
                        //return false;
                    }
                    else
                    {
                        weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                        weightfinish.YFPIECE = weightfinish.DEF3.ToDecimal();
                        decimal numjz = dt.Rows[0]["NASTNUM"].ToString().ToDecimal() / dt.Rows[0]["SECQUANTITY"].ToString().ToDecimal();
                        decimal numsszl = numjz.ToString("f8").ToDecimal() * weightfinish.YFSUTTLE;//木浆件净*件数=应收
                        weightfinish.DEF16 = numjz.ToString("f8").ToDecimal();//木浆件净
                        weightfinish.YFPIWEIGHT = numsszl.ToString("f3").ToDecimal();//应收保留3位
                        cha = weightfinish.SUTTLE - weightfinish.YFPIWEIGHT;
                        if (cha > 0)
                        {
                            bool isnolimite = mat_entity.ISWRITE == 1 ? true : false;
                            if (isnolimite)
                            {
                            }
                            else
                            {
                                weightfinish.DEF17 = cha;
                                weightfinish.SUTTLE = weightfinish.YFPIWEIGHT;
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                            }
                        }
                    }
                }
                else
                {
                    if (strmaterial.StartsWith("103") && weightfinish.DEF2 == "吨")//能源类 扣重
                    {
                        weightfinish.TRAFFICCHARGE = 0;
                        #region 能源类 扣重
                        //允许路损率
                        weightfinish.CHARGE = Convert.ToDecimal(strkd);//亏吨率
                        fwmin = Convert.ToDecimal(strkd) * 0.001M * weightfinish.YFSUTTLE;
                        if (fwmin > 0)
                        {
                            ///亏吨数量=应收数×（1-允许路损率）-实收数
                            cha = weightfinish.SUTTLE - weightfinish.YFSUTTLE;
                            if (cha > 0)
                            {
                                weightfinish.SUTTLE = weightfinish.YFSUTTLE;
                                weightfinish.YFPIECE = weightfinish.YFSUTTLE;
                            }
                            else if (cha < 0)
                            {
                                var numChaAbs = Math.Abs(cha);
                                if (numChaAbs > fwmin)
                                {
                                    var kd = numChaAbs - fwmin;
                                    weightfinish.TRAFFICCHARGE = kd;//亏吨
                                }
                                weightfinish.YFPIECE = weightfinish.SUTTLE;
                            }
                            else
                            {
                                weightfinish.YFPIECE = weightfinish.SUTTLE;
                            }
                        }
                        else
                        {
                            if (weightfinish.YFSUTTLE > 0)
                            {
                                cha = weightfinish.SUTTLE - weightfinish.YFSUTTLE;
                                if (cha > 0)
                                {
                                    weightfinish.SUTTLE = weightfinish.YFSUTTLE;
                                    weightfinish.YFPIECE = weightfinish.YFSUTTLE;

                                }
                            }
                        }
                        weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                        #endregion
                    }
                    else
                    {
                        if (weightfinish.YFPIWEIGHT > 0)
                        {
                            #region 控制量按 输入件数件皮件净夹板扣重
                            //实际净重-总件皮-总夹板 和 手持机件净比较
                            cha = (weightfinish.SUTTLE - weightfinish.DEF15) - weightfinish.YFPIWEIGHT;
                            if (cha > 0)
                            {
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE + cha;
                                weightfinish.DEF17 = cha;  //赋值DEF17保存涨吨（推到皮上）
                                //净重=应收-手持机扣重
                                weightfinish.SUTTLE = weightfinish.YFPIWEIGHT - weightfinish.PDAKZ1.ToDecimal();
                                //订单计算余量统一使用字段 YFPIECE（如果是按件这个字段存放件数，按重量这个字段存放重量
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }
                            else
                            {
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                                weightfinish.SUTTLE = weightfinish.SUTTLE - weightfinish.DEF15 - weightfinish.PDAKZ1.ToDecimal();
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 其他按重量计量扣重计算
                            weightfinish.YFPIWEIGHT = weightfinish.YFSUTTLE;
                            cha = (weightfinish.SUTTLE - weightfinish.DEF15) * rate - weightfinish.YFSUTTLE;
                            if (cha > 0 && weightfinish.YFSUTTLE > 0)
                            {
                                weightfinish.DEF17 = cha;
                                weightfinish.SUTTLE = weightfinish.YFSUTTLE / rate;
                                weightfinish.YFPIECE = weightfinish.YFSUTTLE;
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                            }
                            else
                            {
                                if (weightfinish.YFSUTTLE > 0)
                                    weightfinish.DEF17 = cha;

                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                                weightfinish.SUTTLE = weightfinish.SUTTLE - weightfinish.DEF15 - weightfinish.PDAKZ1.ToDecimal();
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }

                            #endregion
                        }

                    }
                }

            }
            else
            {
                msg = "订单余量查询异常";
                return false;
            }

            return true;
        }



        internal bool CheckContract_CG0(WB_WEIGHT_B weightfinish, ref string msg)
        {
            BD_MaterialBll bd_bll = new BD_MaterialBll();
            VBD_MATERIAL mat_entity = bd_bll.GetEntity(weightfinish.PK_MATERIAL);

            if (string.IsNullOrEmpty(weightfinish.DEF3))
                weightfinish.DEF3 = "0";

            string SQL = string.Format("SELECT NASTNUM,SECQUANTITY,YLNUM,NPRICE,secpurunitname,purunitname,vbdef2,materialcode,def6 FROM VNC_PO_ORDER_YL_CS WHERE PK_ORDER_B='{0}' AND rownum < 2", weightfinish.PK_BILL_B);
            System.Data.DataTable dt = DataFactory.Database().FindTableBySql(SQL); //cardtools.QueryTable(SQL);
            if (dt != null && dt.Rows.Count > 0)
            {
                #region 判断换算率 按数量 按重量  千克 件 吨
                weightfinish.PRICE = Convert.ToDecimal(dt.Rows[0]["NPRICE"].ToString());
                string strunid = dt.Rows[0]["secpurunitname"].ToString();
                decimal ylnum = Convert.ToDecimal(dt.Rows[0]["YLNUM"].ToString());
                string strkd = dt.Rows[0]["vbdef2"].ToString().Trim();
                string strmaterial = dt.Rows[0]["materialcode"].ToString();
                weightfinish.DEF1 = dt.Rows[0]["PURUNITNAME"].ToString();
                weightfinish.DEF2 = dt.Rows[0]["SECPURUNITNAME"].ToString();
                if (string.IsNullOrEmpty(strkd))
                {
                    strkd = "0";
                }
                string strrc = dt.Rows[0]["def6"].ToString().Trim();
                if (string.IsNullOrEmpty(strrc))
                {
                    strrc = "0";
                }
                decimal ylrc = strrc.ToDecimal();
                bool ischa = true;
                string unittype = "重量";
                decimal rate = 1;
                decimal fwmin = -0.5M;
                decimal fwmax = 0.5M;
                if (weightfinish.DEF2 == "千克")
                {
                    rate = 1000;
                }

                #endregion

                decimal cha = 0M;
                if (weightfinish.DEF2 == "件")
                {
                    if (weightfinish.YFSUTTLE != weightfinish.DEF3.ToDecimal())
                    {
                        //msg = "原发和实收件数不一致不能过磅";
                        //return false;
                    }
                    else
                    {
                        weightfinish.YFPIECE = weightfinish.DEF3.ToDecimal();
                        decimal numjz = dt.Rows[0]["NASTNUM"].ToString().ToDecimal() / dt.Rows[0]["SECQUANTITY"].ToString().ToDecimal();
                        decimal numsszl = numjz.ToString("f8").ToDecimal() * weightfinish.YFSUTTLE;
                        weightfinish.DEF16 = numjz.ToString("f8").ToDecimal();//木浆件净
                        weightfinish.YFPIWEIGHT = numsszl.ToString("f3").ToDecimal();
                        cha = weightfinish.SUTTLE - weightfinish.YFPIWEIGHT;
                        if (cha > 0)
                        {
                            bool isnolimite = mat_entity.ISWRITE == 1 ? true : false;
                            if (isnolimite)
                            {
                            }
                            else
                            {
                                weightfinish.DEF17 = cha;
                                //weightfinish.SUTTLE = weightfinish.YFPIWEIGHT;
                                //weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                            }
                        }
                    }
                }
                else
                {
                    if (strmaterial.StartsWith("103") && weightfinish.DEF2 == "吨")//能源类 扣重
                    {
                        weightfinish.TRAFFICCHARGE = 0;
                        #region 能源类 扣重
                        //允许路损率
                        weightfinish.CHARGE = Convert.ToDecimal(strkd);//亏吨率
                        fwmin = Convert.ToDecimal(strkd) * 0.001M * weightfinish.YFSUTTLE;
                        if (fwmin > 0)
                        {
                            ///亏吨数量=应收数×（1-允许路损率）-实收数
                            cha = weightfinish.SUTTLE - weightfinish.YFSUTTLE;
                            if (cha > 0)
                            {
                                //weightfinish.SUTTLE = weightfinish.YFSUTTLE;
                                weightfinish.YFPIECE = weightfinish.YFSUTTLE;
                            }
                            else if (cha < 0)
                            {
                                var numChaAbs = Math.Abs(cha);
                                if (numChaAbs > fwmin)
                                {
                                    var kd = numChaAbs - fwmin;
                                    weightfinish.TRAFFICCHARGE = kd;//亏吨
                                }
                                weightfinish.YFPIECE = weightfinish.SUTTLE;
                            }
                            else
                            {
                                weightfinish.YFPIECE = weightfinish.SUTTLE;
                            }
                        }
                        else
                        {
                            if (weightfinish.YFSUTTLE > 0)
                            {
                                cha = weightfinish.SUTTLE - weightfinish.YFSUTTLE;
                                if (cha > 0)
                                {
                                    //weightfinish.SUTTLE = weightfinish.YFSUTTLE;
                                    weightfinish.YFPIECE = weightfinish.YFSUTTLE;
                                }
                            }
                        }
                        //weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                        #endregion
                    }
                    else
                    {
                        if (weightfinish.YFPIWEIGHT > 0)
                        {
                            #region 控制量按 输入件数件皮件净夹板扣重
                            //实际净重-总件皮-总夹板 和 手持机件净比较
                            cha = (weightfinish.SUTTLE - weightfinish.DEF15) - weightfinish.YFPIWEIGHT;
                            if (cha > 0)
                            {
                                //weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE + cha;
                                weightfinish.DEF17 = cha;  //赋值DEF17保存涨吨（推到皮上）
                                //净重=应收-手持机扣重
                                //weightfinish.SUTTLE = weightfinish.YFPIWEIGHT - weightfinish.PDAKZ1.ToDecimal();
                                //订单计算余量统一使用字段 YFPIECE（如果是按件这个字段存放件数，按重量这个字段存放重量
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }
                            else
                            {
                                //weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                                // weightfinish.SUTTLE = weightfinish.SUTTLE - weightfinish.DEF15 - weightfinish.PDAKZ1.ToDecimal();
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 其他按重量计量扣重计算
                            weightfinish.YFPIWEIGHT = weightfinish.YFSUTTLE;
                            cha = (weightfinish.SUTTLE - weightfinish.DEF15) * rate - weightfinish.YFSUTTLE;
                            if (cha > 0 && weightfinish.YFSUTTLE > 0)
                            {
                                weightfinish.DEF17 = cha;
                                // weightfinish.SUTTLE = weightfinish.YFSUTTLE / rate;
                                weightfinish.YFPIECE = weightfinish.YFSUTTLE;
                                //weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                            }
                            else
                            {
                                if (weightfinish.YFSUTTLE > 0)
                                    weightfinish.DEF17 = cha;
                                // weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                                // weightfinish.SUTTLE = weightfinish.SUTTLE - weightfinish.DEF15 - weightfinish.PDAKZ1.ToDecimal();
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }
                            #endregion
                        }

                    }
                }

            }
            else
            {
                msg = "订单余量查询异常";
                return false;
            }

            return true;
        }

        public int EditWeightJoinData(WB_WEIGHT entity, DbTransaction isOpenTrans)
        {
            int res = 0;
            //更新门禁
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            strSql.Clear();
            strSql.Append(string.Format("update WB_WEIGHT_TASK set where ID='{0}'", entity.PK_TASK));
            res = repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);

            return res;
        }

        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from WB_WEIGHT where BILLNO='{0}' and COMPUTERTYPE<99", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为作废的计量单能删除操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("WB_WEIGHT", "BILLNO", KeyValue);
                database.Delete("WB_WEIGHT_B", "BILLNO", KeyValue);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        //一次作废
        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue, string MEMO)
        {
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            //验证
            WB_WEIGHT entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.COMPUTERTYPE == "99")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该采购计量单已作废！" }.ToString());
            }
            string sql = "";
            int res = 0;


            sql = String.Format("SELECT count(1) NUMS FROM VZJQY WHERE  ISCY='1' and DETAILSID='{0}'", entity.PK_ORDER_B);
            res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的派车单:{1}已采样不能作废操作！", entity.CARS, entity.PK_ORDER_B) }.ToString());
            }

            sql = String.Format("select count(1) from APP_HANDORDER  where  BILLNO='{0}'", entity.BILLNO);
            res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("过磅单{0}已经登记或收货确认!", entity.BILLNO) }.ToString());
            }
            if (entity.FINISH == "1")
            {
                sql = String.Format("select count(1) from APP_HANDORDER  where UPLOAD=1 and ID='{0}'", entity.PK_ORDER_B);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("过磅计量单{0}已推单!", entity.BILLNO) }.ToString());
                }
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "作废成功。";
                if (!string.IsNullOrEmpty(entity.PK_ORDER_B))
                {
                    if (entity.FINISH == "0")
                    {
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_POCARSORDER_DTL set IsUse='{0}',MEMO='{1}' where ID='{2}'", 2, "一次计量作废!", entity.PK_ORDER_B));
                        database.ExecuteBySql(strSql, isOpenTrans);
                    }
                }
                entity.COMPUTERTYPE = "99";
                entity.FINISH = "1";
                entity.MEMO = string.Format("作废:{0};", MEMO);
                database.Update(entity, isOpenTrans);
                database.Commit();
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运采购计量单:{0}，车辆:{1}毛{2}皮{3}作废成功。{4}", entity.BILLNO, entity.CARS, entity.GROSS, entity.TARE, MEMO);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);

                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        //二次作废
        [LoginAuthorize]
        public ActionResult InvalidOrder1(string KeyValue, string PK_TASK, string PK_ORDER, string PK_ORDER_B, string MEMO)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            //验证

            WB_WEIGHT_B entity = database.FindEntity<WB_WEIGHT_B>(KeyValue);

            if (entity.COMPUTERTYPE == "99")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("计量单:{0}已作废！", entity.BILLNO) }.ToString());
            }
            string sql = "";
            int res = 0;

            if (!string.IsNullOrEmpty(PK_ORDER_B))
            {
                // select * from BD_PCRAWCYDETAILS where PCITEMID='QD01200410002801'
                sql = String.Format("SELECT count(1) NUMS FROM BD_PCRAWCYDETAILS WHERE  PCITEMID='{0}'", PK_ORDER_B);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的派车单:{1}已采样不能作废操作！", entity.CARS, PK_ORDER) }.ToString());
                }
            }
            sql = String.Format("select count(1) from APP_HANDORDER  where  ID='{0}' and STATUS='{1}'", PK_ORDER_B, "1");
            res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("过磅单：{0}已经收货确认!", PK_TASK) }.ToString());
            }
            if (entity.FINISH == "1")
            {
                sql = String.Format("select count(1) from APP_HANDORDER  where UPLOAD=1 and ID='{0}'", PK_ORDER_B);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("过磅计量单{0}已推单!", entity.BILLNO) }.ToString());
                }
            }
            try
            {
                string Message = "作废成功。";
                entity.COMPUTERTYPE = "99";
                entity.FINISH = "1";
                entity.MEMO = string.Format("作废:{0};", MEMO);
                database.Update(entity, isOpenTrans);

                if (!string.IsNullOrEmpty(entity.PK_ORDER))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_POCARSORDER_DTL set IsUse='{0}',MEMO='{1}' where ID='{2}'", 4, "二次计量作废!", entity.PK_ORDER_B));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    //计量明细为单条时，作废派车主表数据
                    sql = String.Format("select count(1) from WB_WEIGHT_B  where  COMPUTERTYPE!='99' and PK_ORDER='{0}' ", entity.PK_ORDER);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res == 1)
                    {
                        //作废磅单主表
                        strSql.Clear();
                        strSql.Append(string.Format("update WB_WEIGHT set COMPUTERTYPE='{0}',MEMO='作废：{1}',FINISH=1 where BILLNO='{2}'", 99, MEMO, entity.PK_TASK));
                        database.ExecuteBySql(strSql, isOpenTrans);
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_POCARSORDER set IsUse='{0}',MEMO='{1}' where ID='{2}'", 4, "二次计量作废!", entity.PK_ORDER));
                        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);

                    }
                    strSql.Clear();
                    strSql.Append(string.Format("DELETE DP_CARSORDER where ORDERID='{0}'", entity.PK_ORDER));
                    database.ExecuteBySql(strSql, isOpenTrans);

                    //门禁作废
                    //strSql.Clear();
                    //strSql.Append(string.Format("update wb_weight_task set finish='1',status='作废' where PK_ORDER='{0}'", entity.PK_ORDER));
                    //repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);

                    #region  保留删除收货采样
                    //if (entity.FINISH == "1")
                    //{
                    //    //删除收货
                    //    strSql.Clear();
                    //    strSql.Append(string.Format("delete APP_HANDORDER s where BILLNO='{0}'", entity.BILLNO));
                    //    database.ExecuteBySql(strSql, isOpenTrans);
                    //    //采样主表
                    //    sql = String.Format("select count(1) Num from BD_PCRAWCY where ID in(select MAINID from BD_PCRAWCYDETAILS where PCID='{0}')", entity.PK_ORDER);
                    //    res = DataFactory.Database().FindCountBySql(sql);
                    //    if (res > 1)
                    //    {
                    //        strSql.Clear();
                    //        strSql.Append(string.Format("update BD_PCRAWCY set CARSNUM=CARSNUM-1 where ID in(select MAINID from BD_PCRAWCYDETAILS where PCID='{0}')", entity.PK_ORDER));
                    //        database.ExecuteBySql(strSql, isOpenTrans);
                    //        //删除车数为0的
                    //        strSql.Clear();
                    //        strSql.Append(string.Format("delete BD_PCRAWCY where CARSNUM=0 and ID in(select MAINID from BD_PCRAWCYDETAILS where PCID='{0}')", entity.PK_ORDER));
                    //        database.ExecuteBySql(strSql, isOpenTrans);
                    //    }
                    //    else
                    //    {
                    //        strSql.Clear();
                    //        strSql.Append(string.Format("delete BD_PCRAWCY where ID in(select MAINID from BD_PCRAWCYDETAILS where PCID='{0}')", entity.PK_ORDER));
                    //        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                    //    }
                    //    //删除采样明细
                    //    strSql.Clear();
                    //    strSql.Append(string.Format("delete BD_PCRAWCYDETAILS s where PCID='{0}'", entity.PK_ORDER));
                    //    res = database.ExecuteBySql(strSql, isOpenTrans);
                    //}
                    #endregion
                }

                database.Commit();
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运采购计量单:{0}，车辆:{1}毛{2}皮{3}作废成功。{4}", entity.BILLNO, entity.CARS, entity.GROSS, entity.TARE, MEMO);
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
        /// 【计量单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            WB_WEIGHT entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            //VNC_PO_ORDER_B porder = DataFactory.Database().FindEntityBySql<VNC_PO_ORDER_B>(string.Format("select * from vnc_po_order_bqy where PK_ORDER_B='{0}'", entity.PK_BILL_B));
            //JsonData = JsonData.Insert(1, "\"NASTNUM\":\"" + porder.NASTNUM + "\",");
            //JsonData = JsonData.Insert(1, "\"YLNUM\":\"" + porder.YLNUM + "\",");
            return Content(JsonData);
        }
        /// <summary>
        /// 获取物料价格
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetMaterialPrice(string KeyValue)
        {
            WB_WEIGHT entity = repositoryfactory.Repository().FindEntity(KeyValue);
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
             DP_POCARSORDER_DTL pc_entity = database.FindEntityByWhere<DP_POCARSORDER_DTL>(string.Format(" AND ID='{0}'", entity.PK_ORDER_B));
            DP_POCARSORDER pc_entity1 = database.FindEntityByWhere<DP_POCARSORDER>(string.Format(" AND ID='{0}'", entity.PK_ORDER));

            APP_HANDORDER app_entity = database.FindEntityByWhere<APP_HANDORDER>(string.Format(" AND ID='{0}'", entity.PK_ORDER_B));
            string HZUSER = "";
            if (string.IsNullOrEmpty(app_entity.LEADERDEPA))
            {
                HZUSER = pc_entity1.HZUSER;
            }
            else
            {
                HZUSER = app_entity.LEADERDEPA;
            }
            string PK_MATERIAL = "";
            if (string.IsNullOrEmpty(app_entity.GPMATERIAL))
            {
                PK_MATERIAL = entity.PK_MATERIAL;
            }
            else
            {
                PK_MATERIAL = app_entity.GPMATERIAL;
            }
        
            BD_MATERIAL_PRICE_DTL mat_entity = database.FindEntityBySql<BD_MATERIAL_PRICE_DTL>(string.Format("  select * from (select * from BD_MATERIAL_PRICE_DTL where PK_SUPPLIER='{0}' and PK_MATERIAL='{1}'  and IMPLEMENTDATE<'{2}' order by IMPLEMENTDATE desc,CREATIONTIME desc) ww  where rownum<2", entity.PK_RECEIVEORG, entity.PK_MATERIAL, entity.WEIGHTDATE));
            BD_SOURCEAREA_PRICE_DTL area_entity = database.FindEntityBySql<BD_SOURCEAREA_PRICE_DTL>(string.Format(" select * from ( select * from BD_SOURCEAREA_PRICE_DTL where PK_CUSTOMER='{0}' and PK_MATERIAL='{1}' and PK_SUPPLIER='{2}' and IMPLEMENTDATE<'{3}' order by IMPLEMENTDATE desc,CREATIONTIME desc) ww  where rownum<2", entity.PK_RECEIVEORG,  PK_MATERIAL, HZUSER, entity.WEIGHTDATE));
            BD_SUPPLY_PRICE_DTL sup_entity = database.FindEntityBySql<BD_SUPPLY_PRICE_DTL>(string.Format(" select * from ( select * from BD_SUPPLY_PRICE_DTL where PK_SUPPLIER='{0}' and IMPLEMENTDATE<'{1}' order by IMPLEMENTDATE desc,CREATIONTIME desc) ww  where rownum<2", entity.PK_SENDORG, entity.WEIGHTDATE));

            BD_SUPPLY_PRICE_DTL sup_entity1 = database.FindEntityBySql<BD_SUPPLY_PRICE_DTL>(string.Format(" select * from ( select * from BD_SUPPLY_PRICE_DTL where PK_SUPPLIER='{0}' and IMPLEMENTDATE<'{1}' order by IMPLEMENTDATE desc,CREATIONTIME desc) ww  where rownum<2", pc_entity1.PK_SENDADDRESS, entity.WEIGHTDATE));
            decimal PRICE = 0,PRICE1=0;
            if (sup_entity.PRICE > 0)
            {
                PRICE = (mat_entity.PRICE + area_entity.PRICE) * sup_entity.PRICE;
            }
            else
            {
                PRICE = (mat_entity.PRICE + area_entity.PRICE);
            }

            if (sup_entity1.PRICE > 0)
            {
                PRICE1 = (mat_entity.PRICE + area_entity.PRICE) * sup_entity1.PRICE;
            }
            else
            {
                PRICE1 = (mat_entity.PRICE + area_entity.PRICE);
            }
            
            MaterialPrice matprice = new MaterialPrice();
            matprice.PRICE = PRICE;
            matprice.PRICE1 = PRICE1;
            string JsonData = matprice.ToJson();
            return Content(JsonData);
        }
        [HttpPost]
        public ActionResult SetFormControl1(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            WB_WEIGHT_B entity = database.FindEntity<WB_WEIGHT_B>(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        internal bool CheckContract_CG(WB_WEIGHT_B weightfinish, ref string msg)
        {
            BD_MaterialBll bd_bll = new BD_MaterialBll();
            VBD_MATERIAL mat_entity = bd_bll.GetEntity(weightfinish.PK_MATERIAL);
            if (weightfinish.COMPUTERTYPE == "112")
            {
                msg = ("退货不判断余量");
                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                return true;
            }
            if (string.IsNullOrEmpty(weightfinish.DEF3))
                weightfinish.DEF3 = "0";

            string SQL = string.Format("SELECT NASTNUM,SECQUANTITY,YLNUM,NPRICE,secpurunitname,purunitname,vbdef2,materialcode,def6 FROM VNC_PO_ORDER_YL_CS WHERE PK_ORDER_B='{0}' AND rownum < 2", weightfinish.PK_BILL_B);
            System.Data.DataTable dt = DataFactory.Database().FindTableBySql(SQL); //cardtools.QueryTable(SQL);
            if (dt != null && dt.Rows.Count > 0)
            {
                #region 判断换算率 按数量 按重量  千克 件 吨
                weightfinish.PRICE = Convert.ToDecimal(dt.Rows[0]["NPRICE"].ToString());
                string strunid = dt.Rows[0]["secpurunitname"].ToString();
                decimal ylnum = Convert.ToDecimal(dt.Rows[0]["YLNUM"].ToString());
                string strkd = dt.Rows[0]["vbdef2"].ToString().Trim();
                string strmaterial = dt.Rows[0]["materialcode"].ToString();
                weightfinish.DEF1 = dt.Rows[0]["PURUNITNAME"].ToString();
                weightfinish.DEF2 = dt.Rows[0]["SECPURUNITNAME"].ToString();
                if (string.IsNullOrEmpty(strkd))
                {
                    strkd = "0";
                }
                string strrc = dt.Rows[0]["def6"].ToString().Trim();
                if (string.IsNullOrEmpty(strrc))
                {
                    strrc = "0";
                }
                decimal ylrc = strrc.ToDecimal();
                bool ischa = true;
                string unittype = "重量";
                decimal rate = 1;
                decimal fwmin = -0.5M;
                decimal fwmax = 0.5M;
                if (weightfinish.DEF2 == "千克")
                {
                    rate = 1000;
                }

                #endregion

                decimal cha = 0M;
                if (weightfinish.DEF2 == "件")
                {
                    if (weightfinish.YFSUTTLE != weightfinish.DEF3.ToDecimal())
                    {
                        msg = "原发和实收件数不一致不能过磅";
                        return false;
                    }
                    weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                    weightfinish.YFPIECE = weightfinish.DEF3.ToDecimal();
                    decimal numjz = dt.Rows[0]["NASTNUM"].ToString().ToDecimal() / dt.Rows[0]["SECQUANTITY"].ToString().ToDecimal();
                    decimal numsszl = numjz.ToString("f8").ToDecimal() * weightfinish.YFSUTTLE;
                    weightfinish.DEF16 = numjz.ToString("f8").ToDecimal();//木浆件净
                    weightfinish.YFPIWEIGHT = numsszl.ToString("f3").ToDecimal();
                    cha = weightfinish.SUTTLE - weightfinish.YFPIWEIGHT;
                    if (cha > 0)
                    {
                        bool isnolimite = mat_entity.ISWRITE == 1 ? true : false;
                        if (isnolimite)
                        {
                        }
                        else
                        {
                            weightfinish.DEF17 = cha;
                            weightfinish.SUTTLE = weightfinish.YFPIWEIGHT;
                            weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                        }
                    }

                }
                else
                {
                    if (strmaterial.StartsWith("103") && weightfinish.DEF2 == "吨")//能源类 扣重
                    {
                        #region 能源类 扣重
                        weightfinish.CHARGE = Convert.ToDecimal(strkd);//亏吨率
                        fwmin = Convert.ToDecimal(strkd) * 0.001M * weightfinish.YFSUTTLE;
                        if (fwmin > 0)
                        {
                            ///亏吨数量=应收数×（1-允许路损率）-实收数
                            cha = weightfinish.SUTTLE - weightfinish.YFSUTTLE;
                            if (cha > 0)
                            {
                                weightfinish.SUTTLE = weightfinish.YFSUTTLE;
                                weightfinish.YFPIECE = weightfinish.YFSUTTLE;
                            }
                            else if (cha < 0)
                            {
                                var numChaAbs = Math.Abs(cha);
                                if (numChaAbs > fwmin)
                                {
                                    var kd = numChaAbs - fwmin;
                                    weightfinish.TRAFFICCHARGE = kd;//亏吨
                                }
                                weightfinish.YFPIECE = weightfinish.SUTTLE;
                            }
                            else
                                weightfinish.YFPIECE = weightfinish.SUTTLE;
                        }
                        else
                        {
                            if (weightfinish.YFSUTTLE > 0)
                            {
                                cha = weightfinish.SUTTLE - weightfinish.YFSUTTLE;
                                if (cha > 0)
                                {
                                    weightfinish.SUTTLE = weightfinish.YFSUTTLE;
                                    weightfinish.YFPIECE = weightfinish.YFSUTTLE;

                                }
                            }
                        }
                        weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;

                        #endregion
                    }
                    else
                    {
                        if (weightfinish.YFPIWEIGHT > 0)
                        {
                            #region 控制量按 输入件数件皮件净夹板扣重
                            //实际净重-总件皮-总夹板 和 手持机件净比较
                            cha = (weightfinish.SUTTLE - weightfinish.DEF15) - weightfinish.YFPIWEIGHT;
                            if (cha > 0)
                            {
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE + cha;
                                weightfinish.DEF17 = cha;  //赋值DEF17保存涨吨（推到皮上）
                                //净重=应收-手持机扣重
                                weightfinish.SUTTLE = weightfinish.YFPIWEIGHT - weightfinish.PDAKZ1.ToDecimal();
                                //订单计算余量统一使用字段 YFPIECE（如果是按件这个字段存放件数，按重量这个字段存放重量
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }
                            else
                            {
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                                weightfinish.SUTTLE = weightfinish.SUTTLE - weightfinish.DEF15 - weightfinish.PDAKZ1.ToDecimal();
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }
                            #endregion
                        }
                        else
                        {
                            #region 其他按重量计量扣重计算
                            weightfinish.YFPIWEIGHT = weightfinish.YFSUTTLE;
                            cha = (weightfinish.SUTTLE - weightfinish.DEF15) * rate - weightfinish.YFSUTTLE;
                            if (cha > 0 && weightfinish.YFSUTTLE > 0)
                            {
                                weightfinish.DEF17 = cha;
                                //cardtools.SetTip(weightfinish.PK_BILL_B + "原发控制" + weightfinish.SUTTLE + "=>" + weightfinish.YFSUTTLE, weightfinish.TWOBANG);
                                weightfinish.SUTTLE = weightfinish.YFSUTTLE / rate;
                                weightfinish.YFPIECE = weightfinish.YFSUTTLE;
                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                            }
                            else
                            {
                                if (weightfinish.YFSUTTLE > 0)
                                    weightfinish.DEF17 = cha;

                                weightfinish.TARE = weightfinish.GROSS - weightfinish.SUTTLE;
                                weightfinish.SUTTLE = weightfinish.SUTTLE - weightfinish.DEF15 - weightfinish.PDAKZ1.ToDecimal();
                                weightfinish.YFPIECE = weightfinish.SUTTLE * rate;
                            }

                            #endregion
                        }

                    }
                }

            }
            else
            {
                msg = "订单余量查询异常";
                return false;
            }

            return true;
        }

        private Dictionary<int, string> GetMapping()
        {
            Dictionary<int, string> importBOPMapping = new Dictionary<int, string>();
            importBOPMapping.Add(0, "PK_CONTRACT_B");
            importBOPMapping.Add(1, "CARS");
            importBOPMapping.Add(2, "DRIVERS");
            importBOPMapping.Add(3, "RECEIVESTORE");
            importBOPMapping.Add(4, "TRAFFICCOMPANY");
            importBOPMapping.Add(5, "YFSUTTLE");
            importBOPMapping.Add(6, "GROSS");
            importBOPMapping.Add(7, "TARE");
            importBOPMapping.Add(8, "SUTTLE");
            importBOPMapping.Add(9, "DEF1");
            return importBOPMapping;
        }

        public ActionResult SubmitUploadify(HttpPostedFileBase Filedata, string ModuleId)
        {
            try
            {
                Thread.Sleep(1000);////延迟500毫秒
                string IsOk = "";
                //没有文件上传，直接返回
                if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength == 0)
                {
                    return HttpNotFound();
                }
                //获取文件完整文件名(包含绝对路径)

                string fileGuid = CommonHelper.GetGuid;
                fileGuid = "LLJ" + DateTime.Now.ToString("yyMMddHHmmssffff");
                long filesize = Filedata.ContentLength;
                string FileEextension = Path.GetExtension(Filedata.FileName);
                string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                string virtualPath = string.Format("~/Resource/Document/LLJFile/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                string fullFileName = this.Server.MapPath(virtualPath);
                //创建文件夹，保存文件
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                if (!System.IO.File.Exists(fullFileName))
                {
                    Filedata.SaveAs(fullFileName);
                    try
                    {
                        Dictionary<int, string> mapping = GetMapping();
                        List<WB_WEIGHT_IMPORT> list = EOS.Utilities.NPOIClass.ExcelToList<WB_WEIGHT_IMPORT>(fullFileName, mapping);
                        if (list.Count > 0)
                        {
                            decimal SumSettle = list.Where(w => !string.IsNullOrEmpty(w.SUTTLE)).Sum(o => Convert.ToDecimal(o.SUTTLE));
                            int res = Import_WeightIn(list, ModuleId, SumSettle);
                            System.IO.File.Delete(fullFileName);
                            if (res == -2)
                            {
                                IsOk = "订单余量不足！";
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        IsOk = ex.Message;
                        System.IO.File.Delete(fullFileName);
                    }
                }
                return Content(IsOk);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }


        public int Import_WeightIn(List<WB_WEIGHT_IMPORT> list, string ModuleId, decimal sumsuttle)
        {
            int res = 0;
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                #region 实例对象
                //保存对象
                WB_WEIGHT entity = null;
                WB_WEIGHT_B orderentry = null;
                DP_POCARSORDER entity_pc = null;
                DP_POCARSORDER_DTL entity_pc_dtl = null;
                //基础对象
                VNC_PO_ORDER_B poorder_b = null;
                BD_CARS car = null;
                BD_Driver drivers = null;
                BD_CARRIER traffic = null;
                BD_STORDOC store = null;
                #endregion
                if (list.Count > 0)
                {
                    #region 获取信息

                    //车号 
                    if (!string.IsNullOrEmpty(list[0].CARS))
                    {
                        car = database.FindEntityByWhere<BD_CARS>(string.Format(" AND NAME='{0}'", list[0].CARS));
                    }
                    //司机 
                    if (!string.IsNullOrEmpty(list[0].DRIVERS))
                    {
                        drivers = database.FindEntityByWhere<BD_Driver>(string.Format(" AND NAME='{0}'", list[0].DRIVERS));
                    }
                    //承运商
                    if (!string.IsNullOrEmpty(list[0].TRAFFICCOMPANY))
                    {
                        traffic = database.FindEntityByWhere<BD_CARRIER>(string.Format(" AND NAME='{0}'", list[0].TRAFFICCOMPANY));
                    }

                    #endregion
                    string PK_ORDER = "";
                    string PK_ORDER_B = "";
                    decimal hzsuttle = 0;
                    foreach (WB_WEIGHT_IMPORT weight_imp in list)
                    {
                        //订单号 
                        if (!string.IsNullOrEmpty(weight_imp.PK_CONTRACT_B))
                        {
                            if (poorder_b != null)
                            {
                                if (weight_imp.PK_CONTRACT_B != poorder_b.VBILLCODE)
                                {
                                    poorder_b = database.FindEntityByWhere<VNC_PO_ORDER_B>(string.Format(" AND VBILLCODE='{0}'", weight_imp.PK_CONTRACT_B));
                                }
                            }
                            else
                            {
                                poorder_b = database.FindEntityByWhere<VNC_PO_ORDER_B>(string.Format(" AND VBILLCODE='{0}'", weight_imp.PK_CONTRACT_B));
                            }
                            hzsuttle = list.Where(w => w.PK_CONTRACT_B == weight_imp.PK_CONTRACT_B).Sum(o => Convert.ToDecimal(o.SUTTLE));
                            if (poorder_b.YLNUM < hzsuttle)
                            {
                                res = -2;
                                break;
                            }
                            //仓库
                            if (!string.IsNullOrEmpty(weight_imp.RECEIVESTORE))
                            {
                                store = database.FindEntityByWhere<BD_STORDOC>(string.Format(" AND NAME='{0}'", weight_imp.RECEIVESTORE));
                            }
                            #region 计量
                            #region 过磅主表
                            entity = new WB_WEIGHT();
                            base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                            this.ModuleId = ModuleId;
                            entity.BILLNO = this.BillCode();
                            entity.Create();
                            PK_ORDER = entity.BILLNO.Replace("B", "QD");
                            PK_ORDER_B = string.Format("{0}00", PK_ORDER);
                            //派车单号
                            entity.PK_ORDER = PK_ORDER;
                            entity.PK_ORDER_B = PK_ORDER_B;
                            //订单、供应商、物料
                            if (poorder_b != null)
                            {
                                entity.PK_BILL = poorder_b.PK_ORDER;
                                entity.PK_BILL_B = poorder_b.PK_ORDER_B;
                                entity.PK_CONTRACT = poorder_b.VBILLCODE;
                                entity.PK_CONTRACT_B = poorder_b.VBILLCODE;
                                entity.PK_SENDORG = poorder_b.PK_SUPPLIER;
                                entity.SENDORG = poorder_b.PK_SUPPLIERNAME;
                                //接收公司
                                entity.PK_RECEIVEORG = poorder_b.RECCOMPANYID;
                                entity.RECEIVEORG = poorder_b.RECCOMPANYNAME;
                                entity.PK_ORG = poorder_b.RECCOMPANYID;

                                entity.MATERIALKIND = poorder_b.MATERIALSPEC;
                                entity.PK_MATERIAL = poorder_b.PK_MATERIAL;
                                entity.MATERIAL = poorder_b.PK_MATERIALNAME;
                                entity.DATATYPE = poorder_b.DATATYPE;
                                entity.DEF1 = poorder_b.PURUNITNAME;
                                entity.DEF2 = poorder_b.SECPURUNITNAME;
                                if (weight_imp.DEF1 == "兖州厂区")
                                {
                                    entity.DEF11 = "10001";
                                }
                                else if (weight_imp.DEF1 == "邹城厂区")
                                {
                                    entity.DEF11 = "10002";
                                }
                                else if (weight_imp.DEF1 == "兴隆厂区")
                                {
                                    entity.DEF11 = "10003";
                                }
                                else if (weight_imp.DEF1 == "颜店厂区")
                                {
                                    entity.DEF11 = "10004";
                                }
                            }
                            //车号
                            if (car != null)
                            {
                                entity.PK_CARSID = car.ID;
                                entity.CARS = car.NAME;
                            }
                            //司机
                            if (drivers != null)
                            {
                                entity.PK_DRIVERS = drivers.ID;
                                entity.DRIVERS = drivers.NAME;
                            }
                            //承运商
                            if (traffic != null)
                            {
                                entity.PK_TRAFFICCOMPANY = traffic.ID;
                                entity.TRAFFICCOMPANY = traffic.NAME;
                            }
                            //仓库
                            if (store != null)
                            {
                                entity.PK_STORE = store.ID;
                                entity.STORE = store.NAME;
                                entity.PK_RECEIVESTORE = store.ID;
                                entity.RECEIVESTORE = store.NAME;
                            }
                            //毛皮净
                            entity.GROSS = Convert.ToDecimal(weight_imp.GROSS);
                            entity.TARE = Convert.ToDecimal(weight_imp.TARE);
                            entity.SUTTLE = Convert.ToDecimal(weight_imp.SUTTLE);
                            entity.YFSUTTLE = Convert.ToDecimal(weight_imp.YFSUTTLE);
                            entity.DEF3 = "0";
                            entity.YFPIECE = 0;
                            entity.YFPIWEIGHT = entity.YFSUTTLE;

                            entity.FINISH = "1";
                            entity.TYPE = "0";
                            entity.COMPUTERTYPE = "12";
                            entity.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            entity.LIGHTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            entity.WEIGHTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            entity.LASTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            entity.OUTGBUSER = ManageProvider.Provider.Current().UserName;
                            entity.INGBUSER = ManageProvider.Provider.Current().UserName;
                            res += database.Insert(entity, isOpenTrans);
                            #endregion

                            #region 过磅明细表
                            orderentry = new WB_WEIGHT_B();
                            orderentry.Create();
                            orderentry.PK_ORDER = PK_ORDER;
                            orderentry.PK_ORDER_B = PK_ORDER_B;
                            //订单、供应商、物料
                            if (poorder_b != null)
                            {
                                orderentry.PK_BILL = poorder_b.PK_ORDER;
                                orderentry.PK_BILL_B = poorder_b.PK_ORDER_B;
                                orderentry.PK_CONTRACT = poorder_b.VBILLCODE;
                                orderentry.PK_CONTRACT_B = poorder_b.VBILLCODE;
                                orderentry.PK_SENDORG = poorder_b.PK_SUPPLIER;
                                orderentry.SENDORG = poorder_b.PK_SUPPLIERNAME;
                                //接收公司
                                orderentry.PK_RECEIVEORG = poorder_b.RECCOMPANYID;
                                orderentry.RECEIVEORG = poorder_b.RECCOMPANYNAME;
                                orderentry.PK_ORG = poorder_b.RECCOMPANYID;

                                orderentry.MATERIALKIND = poorder_b.MATERIALSPEC;
                                orderentry.PK_MATERIAL = poorder_b.PK_MATERIAL;
                                orderentry.MATERIAL = poorder_b.PK_MATERIALNAME;
                                orderentry.DATATYPE = poorder_b.DATATYPE;
                                orderentry.DEF1 = poorder_b.PURUNITNAME;
                                orderentry.DEF2 = poorder_b.SECPURUNITNAME;
                            }
                            //车号
                            if (car != null)
                            {
                                orderentry.PK_CARSID = car.ID;
                                orderentry.CARS = car.NAME;
                            }
                            //司机
                            if (drivers != null)
                            {
                                orderentry.PK_DRIVERS = drivers.ID;
                                orderentry.DRIVERS = drivers.NAME;
                            }
                            //承运商
                            if (traffic != null)
                            {
                                orderentry.PK_TRAFFICCOMPANY = traffic.ID;
                                orderentry.TRAFFICCOMPANY = traffic.NAME;
                            }
                            //仓库
                            if (store != null)
                            {
                                orderentry.PK_STORE = store.ID;
                                orderentry.STORE = store.NAME;
                                orderentry.PK_RECEIVESTORE = store.ID;
                                orderentry.RECEIVESTORE = store.NAME;
                            }
                            //毛皮净
                            orderentry.GROSS = Convert.ToDecimal(weight_imp.GROSS);
                            orderentry.TARE = Convert.ToDecimal(weight_imp.TARE);
                            orderentry.SUTTLE = Convert.ToDecimal(weight_imp.SUTTLE);
                            orderentry.YFSUTTLE = Convert.ToDecimal(weight_imp.YFSUTTLE);
                            orderentry.DEF3 = "0";
                            orderentry.YFPIECE = 0;
                            orderentry.YFPIWEIGHT = orderentry.YFSUTTLE;
                            orderentry.FINISH = "1";
                            orderentry.TYPE = "0";
                            orderentry.COMPUTERTYPE = "12";
                            orderentry.PK_TASK = entity.BILLNO;
                            orderentry.BILLNO = string.Format("{0}_00", entity.BILLNO);
                            orderentry.PK_TRAFFICCOMPANY = entity.PK_TRAFFICCOMPANY;
                            orderentry.TRAFFICCOMPANY = entity.TRAFFICCOMPANY;
                            orderentry.CREATEDATE = entity.CREATEDATE;
                            orderentry.WEIGHTDATE = entity.WEIGHTDATE;
                            orderentry.LIGHTDATE = entity.LIGHTDATE;
                            orderentry.LASTDATE = entity.LIGHTDATE;
                            orderentry.OUTGBUSER = ManageProvider.Provider.Current().UserName;
                            orderentry.INGBUSER = ManageProvider.Provider.Current().UserName;
                            orderentry.PK_ORG = "";
                            if (!string.IsNullOrEmpty(orderentry.DEF3))
                            {
                                orderentry.YFPIECE = Convert.ToDecimal(orderentry.DEF3) > 0 ? Convert.ToDecimal(orderentry.DEF3) : orderentry.SUTTLE;
                            }
                            if (weight_imp.DEF1 == "兖州厂区")
                            {
                                orderentry.DEF11 = "10001";
                            }
                            else if (weight_imp.DEF1 == "邹城厂区")
                            {
                                orderentry.DEF11 = "10002";
                            }
                            else if (weight_imp.DEF1 == "兴隆厂区")
                            {
                                orderentry.DEF11 = "10003";
                            }
                            else if (weight_imp.DEF1 == "颜店厂区")
                            {
                                orderentry.DEF11 = "10004";
                            }

                            res += database.Insert(orderentry, isOpenTrans);
                            #endregion
                            #endregion


                            #region 派车单
                            #region 派车主表
                            entity_pc = new DP_POCARSORDER();
                            entity_pc.Create();
                            entity_pc.ID = PK_ORDER;
                            entity_pc.ISMULTI = "0";
                            // 供应商 
                            if (poorder_b != null)
                            {
                                entity_pc.PK_SUPPLIER = poorder_b.PK_SUPPLIER;
                                entity_pc.SUPPLIERNAME = poorder_b.PK_SUPPLIERNAME;
                                entity_pc.DATATYPE = poorder_b.DATATYPE;
                            }
                            //车号
                            if (car != null)
                            {
                                entity_pc.CARS = car.ID;
                                entity_pc.CARSNAME = car.NAME;
                            }
                            //司机
                            if (drivers != null)
                            {
                                entity_pc.DRIVERS = drivers.ID;
                                entity_pc.DRIVERSNAME = drivers.NAME;
                                entity_pc.PSDNO = drivers.IDNUM;
                            }
                            //承运商
                            if (traffic != null)
                            {
                                entity_pc.TRAFFICCOMPANY = traffic.ID;
                                entity_pc.TRAFFICCOMPANYNAME = traffic.NAME;
                            }

                            if (weight_imp.DEF1 == "兖州厂区")
                            {
                                entity_pc.DEF1 = "10001";
                            }
                            else if (weight_imp.DEF1 == "邹城厂区")
                            {
                                entity_pc.DEF1 = "10002";
                            }
                            else if (weight_imp.DEF1 == "兴隆厂区")
                            {
                                entity_pc.DEF1 = "10003";
                            }
                            else if (weight_imp.DEF1 == "颜店厂区")
                            {
                                entity_pc.DEF1 = "10004";
                            }
                            entity_pc.NASTNUM = Convert.ToDecimal(weight_imp.SUTTLE);
                            entity_pc.ISUSE = "6";
                            entity_pc.PDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            entity_pc.BILLSOFTYPE = "NORMAL";
                            res += database.Insert(entity_pc, isOpenTrans);
                            #endregion

                            #region 派车明细表
                            entity_pc_dtl = new DP_POCARSORDER_DTL();
                            entity_pc_dtl.Create();
                            entity_pc_dtl.ID = PK_ORDER_B;
                            entity_pc_dtl.PK_DP_POCARSORDER = PK_ORDER;
                            //订单、供应商、物料
                            if (poorder_b != null)
                            {
                                entity_pc_dtl.PK_ORDER = poorder_b.PK_ORDER;
                                entity_pc_dtl.PK_ORDER_B = poorder_b.PK_ORDER_B;
                                entity_pc_dtl.VBILLCODE = poorder_b.VBILLCODE;
                                entity_pc_dtl.PK_SUPPLIER = poorder_b.PK_SUPPLIER;
                                entity_pc_dtl.SUPPLIERNAME = poorder_b.PK_SUPPLIERNAME;
                                entity_pc_dtl.PK_MATERIAL = poorder_b.PK_MATERIAL;
                                entity_pc_dtl.MATERIALNAME = poorder_b.PK_MATERIALNAME;
                                entity_pc_dtl.MATERIALSPEC = poorder_b.MATERIALSPEC;
                                entity_pc_dtl.MATERIALCODE = poorder_b.MATERIALCODE;
                                entity_pc_dtl.NASTNUM = Convert.ToDecimal(poorder_b.SECQUANTITY);
                                entity_pc_dtl.YLNUM = poorder_b.YLNUM;
                                entity_pc_dtl.PRICE = poorder_b.NPRICE;
                                entity_pc_dtl.DEF1 = poorder_b.PURUNITNAME;
                                entity_pc_dtl.DEF2 = poorder_b.SECPURUNITNAME;
                                //接收公司
                                entity_pc_dtl.PK_ORG = poorder_b.RECCOMPANYID;
                                entity_pc_dtl.PK_DEPT = poorder_b.RECORGID;
                                entity_pc_dtl.DBILLDATE = poorder_b.DBILLDATE;
                            }

                            entity_pc_dtl.AMOUNT = Convert.ToDecimal(weight_imp.SUTTLE);
                            entity_pc_dtl.ISUSE = "2";
                            entity_pc_dtl.BILLSOFTYPE = "NORMAL";
                            entity_pc_dtl.ORDERINDEX = 1;
                            res += database.Insert(entity_pc_dtl, isOpenTrans);
                            #endregion
                            #endregion
                        }
                    }
                }
                database.Commit();
                return res;
            }
            catch (Exception ex)
            {
                database.Rollback();
                return -1;
            }
        }


        //废纸榜单批量打印
        public ActionResult FZWeightInDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = wbbll.FZWeightInDataSetReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        public ActionResult BDPrintWeight(string KeyValue)
        {
            DataSet dataSet = order_b_bll.BDPrintWeight(KeyValue);
            return Content(dataSet.GetXml());
        }
    }
    public class MaterialPrice
    {
        public decimal PRICE { get; set; }
        public decimal PRICE1 { get; set; }
    }
}
