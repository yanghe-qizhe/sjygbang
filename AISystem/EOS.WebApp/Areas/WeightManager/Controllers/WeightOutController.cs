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

    public class WeightOutController : PublicController<WB_WEIGHT>
    {
        string ModuleId = "";
        WB_WEIGHTBll wbbll = new WB_WEIGHTBll();
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        VWB_WEIGHTBSBll orderbll = new VWB_WEIGHTBSBll();
        /// <summary>
        /// 获取自动单据编码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }
        public ActionResult WeightOutDetail()
        {
            return View();
        }
        //
        // GET: /WeightManager/WeightOut/

        public ActionResult WeightOutIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult WeightOutForm()
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
        public ActionResult WeightOutForm1()
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
        public ActionResult SELECTBillSOF()
        {
            return View();
        }
        /// <summary>
        /// 设计要求销售订单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SELECTSALEORDERS()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightOutQuery()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult InvalidForm()
        {
            return View();
        }
        /// 销售磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string VBillCode, string ContractBillNo, string FHBillNo, string Customer, string Material, string Cars, string Status, string UPLOAD, string Finish, string COMPUTERTYPE, string DEF11, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHTBS> ListData = orderbll.GetCKOrderList(BillNo, VBillCode, ContractBillNo, FHBillNo, Customer, Material, Cars, "", "", "", "", Status, "3,8", UPLOAD, Finish, "", COMPUTERTYPE, DEF11, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(string KeyValue, WB_WEIGHT entity, string ModuleId)
        {
            #region 验证

            #endregion

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //派车表实例
                DP_SOCARSORDER pc_entity = database.FindEntityByWhere<DP_SOCARSORDER>(string.Format(" AND ID='{0}'", entity.PK_ORDER));

                //派车明细表实例
                DP_SOCARSORDER_DTL pcdtl_entity = database.FindEntityByWhere<DP_SOCARSORDER_DTL>(string.Format(" AND ID='{0}'", entity.PK_ORDER_B));
                string sql = "";
                int res = 0;
                sql = String.Format("SELECT count(1) NUMS FROM APP_HANDORDER WHERE  BILLNO='{0}'", entity.BILLNO);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    pcdtl_entity.DEF10 = entity.BILLNO;
                }
                else
                {
                    pcdtl_entity.DEF10 = null;
                }

                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                entity.WEIGHTDATE = entity.WEIGHTDATE.Replace("&nbsp;", "");
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("销售磅单 :{0}，车辆:{1}，{2}。", entity.BILLNO, entity.CARS, Message);
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    WB_WEIGHT edit_entity = repositoryfactory.Repository().FindEntity(KeyValue);

                   
                    if (entity.FINISH == "0")
                    {
                        entity.LASTDATE = entity.LIGHTDATE;
                    }
                    else
                    {
                        entity.LASTDATE = entity.WEIGHTDATE;
                    }

                    //订单发生变化时
                    if (entity.PK_CONTRACT_B != edit_entity.PK_CONTRACT_B)
                    {
                        //更新派车明细表信息
                        pcdtl_entity.PK_SALEORDER = entity.PK_BILL;
                        pcdtl_entity.CSALEORDERBID = entity.PK_BILL_B;
                        pcdtl_entity.VBILLCODE = entity.PK_CONTRACT_B;
                        pcdtl_entity.CCUSTOMERID = entity.PK_RECEIVEORG;
                        pcdtl_entity.CUSTNAME = entity.RECEIVEORG;
                        pcdtl_entity.CMATERIALID = entity.PK_MATERIAL;
                        pcdtl_entity.MATERIALNAME = entity.MATERIAL;
                        pcdtl_entity.MATERIALSPEC = entity.MATERIALKIND;

                        //更新派车主表信息
                        pc_entity.CARS = entity.PK_CARSID;
                        pc_entity.CARSNAME = entity.CARS;
                        pc_entity.DRIVERS = entity.PK_DRIVERS;
                        pc_entity.DRIVERSNAME = entity.DRIVERS;
                        pc_entity.TRAFFICCOMPANY = entity.PK_TRAFFICCOMPANY;
                        pc_entity.TRAFFICCOMPANYNAME = entity.TRAFFICCOMPANY;
                        if (pcdtl_entity.ORDERINDEX == 1)
                        {
                            pc_entity.CCUSTOMERID = entity.PK_RECEIVEORG;
                            pc_entity.CUSTNAME = entity.RECEIVEORG;
                        }
                    }
                    edit_entity.PK_TASK = entity.PK_TASK;
                    edit_entity.PK_ORDER_B = entity.PK_ORDER_B;
                    edit_entity.PK_ORDER = entity.PK_ORDER;
                    edit_entity.PK_BILL = entity.PK_BILL;
                    edit_entity.PK_BILL_B = entity.PK_BILL_B;
                    edit_entity.PK_CONTRACT_B = entity.PK_CONTRACT_B;
                    edit_entity.PK_RECEIVEORG = entity.PK_RECEIVEORG;
                    edit_entity.RECEIVEORG = entity.RECEIVEORG;
                    edit_entity.PK_MATERIAL = entity.PK_MATERIAL;
                    edit_entity.MATERIAL = entity.MATERIAL;
                    edit_entity.PK_MATERIALKIND = entity.PK_MATERIALKIND;
                    edit_entity.MATERIALKIND = entity.MATERIALKIND;
                    edit_entity.YFSUTTLE = entity.YFSUTTLE;
                    edit_entity.PK_TRAFFICCOMPANY = entity.PK_TRAFFICCOMPANY;
                    edit_entity.TRAFFICCOMPANY = entity.TRAFFICCOMPANY;
                    edit_entity.PK_CARSID = entity.PK_CARSID;
                    edit_entity.CARS = entity.CARS;
                    edit_entity.PK_DRIVERS = entity.PK_DRIVERS;
                    edit_entity.DRIVERS = entity.DRIVERS;
                    edit_entity.GROSS = entity.GROSS;
                    edit_entity.TARE = entity.TARE;
                    edit_entity.SUTTLE = entity.SUTTLE;
                    edit_entity.PK_STORE = entity.PK_STORE;
                    edit_entity.STORE = entity.STORE;
                    edit_entity.LIGHTDATE = entity.LIGHTDATE;
                    edit_entity.WEIGHTDATE = entity.WEIGHTDATE;
                    edit_entity.CREATEDATE = entity.CREATEDATE;
                    edit_entity.FINISH = entity.FINISH;
                    edit_entity.DEF11 = entity.DEF11;
                    edit_entity.MEMO = entity.MEMO;
                    //edit_entity.VMEMO = entity.VMEMO;
                    edit_entity.ISEDIT = 1;
                    edit_entity.OUTGBUSER = ManageProvider.Provider.Current().UserName;
                    edit_entity.LASTDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    database.Update(edit_entity, isOpenTrans);
                    if (entity.FINISH == "1")
                    {
                        pcdtl_entity.ISUSE = "2";
                        pc_entity.ISUSE = "6";
                    }
                    pc_entity.CARS = entity.PK_CARSID;
                    pc_entity.CARSNAME = entity.CARS;
                    database.Update(pcdtl_entity, isOpenTrans);
                    database.Update(pc_entity, isOpenTrans);
                    Base_SysLogBll.Instance.WriteLog(Account, OperationType.Add, "1", message);
                }
                else
                {
                    sql = String.Format("select count(1) from WB_WEIGHT where  BILLNO='{0}'", entity.BILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    entity.COMPUTERTYPE = "17";
                    entity.TYPE = "3";
                    entity.INGBUSER = ManageProvider.Provider.Current().UserName;// "磅房管理员";
                    database.Insert(entity, isOpenTrans);
                    pc_entity.CARS = entity.PK_CARSID;
                    pc_entity.CARSNAME = entity.CARS;
                    if (entity.FINISH == "0")
                    {
                        pcdtl_entity.ISUSE = "3";
                        database.Update(pcdtl_entity, isOpenTrans);
                        pc_entity.ISUSE = "7";
                        database.Update(pc_entity, isOpenTrans);
                    }
                    else
                    {
                        //删除中间表
                        database.Delete("DP_CARSORDER", "ORDERID", entity.PK_ORDER);
                        pcdtl_entity.ISUSE = "2";
                        database.Update(pcdtl_entity, isOpenTrans);
                        pc_entity.ISUSE = "6";
                        database.Update(pc_entity, isOpenTrans);
                    }
                    Base_SysLogBll.Instance.WriteLog(Account, OperationType.Update, "1", message);
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
        public ActionResult InvalidOrder(string KeyValue, string MEMO)
        {
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            //验证
            WB_WEIGHT entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.COMPUTERTYPE == "99")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该销售计量单已作废！" }.ToString());
            }


            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
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
                    strSql.Append(string.Format("update DP_SOCARSORDER set IsUse='{0}' where ID='{1}'", 4, entity.PK_ORDER));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_SOCARSORDER_DTL set IsUse='{0}' where PK_DP_SOCARSORDER='{1}'", 4, entity.PK_ORDER));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    strSql.Clear();
                    strSql.Append(string.Format("DELETE DP_CARSORDER where ORDERID='{0}'", entity.PK_ORDER));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }

                database.Commit();
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运销售计量单:{0}，车辆:{1}毛{2}皮{3}作废成功。{4}", entity.BILLNO, entity.CARS, entity.GROSS, entity.TARE, MEMO);
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
        /// 【到货单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            WB_WEIGHT entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();

            VNC_SALEORDER_B sorder = DataFactory.Database().FindEntityBySql<VNC_SALEORDER_B>(string.Format("select * from VNC_SALEORDER_B where CSALEORDERBID='{0}'", entity.PK_BILL_B));
            JsonData = JsonData.Insert(1, "\"NASTNUM\":\"" + sorder.NASTNUM + "\",");
            JsonData = JsonData.Insert(1, "\"YLNUM\":\"" + sorder.YLNUM + "\",");
            return Content(JsonData);
        }

        public ActionResult BDPrintWeight(string KeyValue)
        {
            DataSet dataSet = orderbll.BDPrintWeight(KeyValue);
            return Content(dataSet.GetXml());
        }

        #region 首页数据
        /// <summary>
        /// 日统计
        /// </summary>
        /// <returns>在厂，出厂，日统计</returns>
        public ActionResult GetWeightDay()
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetWeightOut(),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        public ActionResult GetWeightZDay()
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetWeightZDay(),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        public ActionResult GetWeightJDay()
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetWeightJDay(),
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
        /// 采购日统计
        /// </summary>
        /// <returns>在厂，出厂，日销售统计</returns>
        public ActionResult GetWeightInDay()
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetWeightIn(),
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
        /// 车辆预警
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCarsWarning()
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetCarsWarning(),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }



        //销售走势
        public ActionResult GetMonthSales()
        {
            try
            {
                List<string> X = orderbll.GetChartX(0);
                DataTable dt = orderbll.GetMonthSales();
                var JsonData = orderbll.ChartJsonLine(dt, "Material", X);
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        //销售占比
        public ActionResult GetDaySales()
        {
            try
            {
                DataTable dt = orderbll.GetDaySales();
                var JsonData = orderbll.ChartJsonPie(dt);
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }


        public ActionResult GetInCars()
        {
            try
            {
                DataTable dt = orderbll.GetInCars();
                var JsonData = orderbll.ChartJsonPie(dt);
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        #endregion
    }
}
