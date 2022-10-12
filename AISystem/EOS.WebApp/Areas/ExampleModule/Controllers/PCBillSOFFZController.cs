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
    public class PCBillSOFFZController : PublicController<DP_POCARSORDER>
    {
        string ModuleId = "";
        VDP_POCARSORDER_DTLBll bill_dtl = new VDP_POCARSORDER_DTLBll();
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


        [LoginAuthorize]
        public ActionResult PCBillSOFFZ_FORM()
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
        public ActionResult PCBillSOFFZ_FORMPD()
        {
            ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            return View();
        }

        [LoginAuthorize]
        public ActionResult PCBillSOFFZ_FORM_BZ()
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
        public ActionResult PCBillSOFFZIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCBillSOFFZPDIndex()
        {
            return View();
        }
        //明细
        [LoginAuthorize]
        public ActionResult PCBillSOFFZDetail()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCBillSOFFZQuery()
        {
            return View();
        }


        /// <summary>
        /// 设计要求采购订单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SELECTPOORDERSFZ()
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


        //包材汽运按物料类型查询
        [LoginAuthorize]
        public ActionResult GetQYOrderList(string BillNo, string VBillCode, string PK_ORG, string PK_DEPT, string Supply, string Material, string matclass, string Cars, string PSDNO, string ISUSE, string istype, string CREATEUSER, string DEF1, string TRAFFICCOMPANY,string BILLFROM, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_POCARSORDERBll vorderbll = new VDP_POCARSORDERBll();
                List<VDP_POCARSORDER> ListData = vorderbll.GetOrderList(BillNo, VBillCode, PK_ORG, PK_DEPT, Supply, Material, matclass, Cars, PSDNO, "", ISUSE, istype, CREATEUSER, DEF1, TRAFFICCOMPANY, BILLFROM, StartTime, EndTime, jqgridparam);
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
        [LoginAuthorize]
        public ActionResult GetSQYOrderList1(string BillNo, string VBillCode, string PK_ORG, string Supply, string Material, string Cars, string ISUSE, string MISUSE, string ISTYPE, string DEF1, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_POCARSORDER_DTLBll vorderbll = new VDP_POCARSORDER_DTLBll();
                List<VDP_POCARSORDER_DTL> ListData = vorderbll.GetOrderList1(BillNo, VBillCode, PK_ORG, Supply, Material, Cars, "", MISUSE, ISTYPE, DEF1, StartTime, EndTime, jqgridparam);
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

        //判定
        [LoginAuthorize]
        public ActionResult GetSQYOrderList2(string BillNo, string VBillCode, string PK_ORG, string Supply, string Material, string Cars, string ISUSE, string MISUSE, string ISTYPE, string DEF1, string DEF5, string DATETYPE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_POCARSORDER_DTLPDBll vorderbll = new VDP_POCARSORDER_DTLPDBll();
                List<VDP_POCARSORDER_DTLPD> ListData = vorderbll.GetOrderList1(BillNo, VBillCode, PK_ORG, Supply, Material, Cars, "", MISUSE, ISTYPE, DEF1, DEF5, DATETYPE, StartTime, EndTime, jqgridparam);
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
                    entity.DTYPE = "3";
                    database.Insert(entity, isOpenTrans);
                    ADD_CARSORDER(entity.CARS, entity.CARSNAME, entity.CARDNO, entity.DRIVERS, entity.DRIVERSNAME, entity.PSDNO, "", "", entity.ID, 1, 0, database, isOpenTrans);
                }
                if (entity.DEF11 > 0)
                {
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Clear();
                    strSql.Append(string.Format("update BD_CARS set TGGWEIGHT='{0}'where ID='{1}'", entity.DEF11, entity.CARS));
                    repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
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
                        orderentry.DTYPE = "3";
                        database.Insert(orderentry, isOpenTrans);
                        index++;
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运包材到货单号:{0}，车辆:{1}，新增成功。", entity.ID, entity.CARSNAME);
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
                    entity.DTYPE = "3";
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
                string message = string.Format("汽运包材到货单号:{0}，车辆:{1}，新增成功。", entity.ID, entity.CARSNAME);
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
                string Message = "作废成功。";
                entity.ISUSE = "4";
                entity.MEMO = "业务平台作废";
                database.Update(entity, isOpenTrans);

                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //明细
                strSql.Append(string.Format("update DP_POCARSORDER_DTL set ISUSE='4',MEMO='业务平台作废' where PK_DP_POCARSORDER='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);

                DEL_CARSORDERS(KeyValue, database, isOpenTrans);
                database.Commit();

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运到货单:{0}，车辆{1}业务平台作废成功。", entity.ID, entity.CARSNAME);
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
                entity.MEMO = "强制出厂";

                database.Update(entity, isOpenTrans);
                DEL_CARSORDERS(KeyValue, database, isOpenTrans);

                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //门禁出厂
                strSql.Append(string.Format("update wb_weight_task set finish='1',status='出厂' where PK_ORDER='{0}'", KeyValue));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                //过磅出厂
                strSql.Clear();
                strSql.Append(string.Format("update wb_weight set finish='1',status='完成' where PK_ORDER='{0}'", KeyValue));
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
            if (entity_b.ISUSE != "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到货单明细已计量！" }.ToString());
            }


            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "强制完结成功。";
                //派车主表
                //string sql = String.Format("select count(1) from DP_POCARSORDER_DTL where isuse=1 and PK_DP_POCARSORDER='{0}' and id!='{1}' ", PK_ORDER, PK_ORDER_B);
                //int res = DataFactory.Database().FindCountBySql(sql);
                //if (res > 0)
                //{
                //}
                //else
                //{
                //    entity.ISUSE = "6";
                //    entity.MEMO = "强制完结";
                //    database.Update(entity, isOpenTrans);
                //    //中间表
                //    DEL_CARSORDERS(PK_ORDER, database, isOpenTrans);
                //}
                //派车明细
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

        [LoginAuthorize]
        public ActionResult PDCheckOrder(string KeyValue, string WDDEF5, String MEMO)
        {
            //验证
            VDP_POCARSORDER_DTL entity = bill_dtl.GetEntity(KeyValue);

            if (entity.ISUSE == "4")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "汽运废纸到货单已作废！" }.ToString());
            }
            if (entity.MISUSE == "6")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "汽运废纸到货单已出厂！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().UserName;
            try
            {
                int res = 0;
                string Message = "判定成功。";
                //派车单出厂
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update DP_POCARSORDER set DEF5='{0}',DEF6='{1}',DEF7='{2}',DEF8='{3}' where ID='{4}'", WDDEF5, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), Account, MEMO, entity.PK_DP_POCARSORDER));
                res = database.ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                string msg = string.Format("{0}车号到货单号{1},内外地判定成功!", entity.PK_DP_POCARSORDER, entity.CARSNAME);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", msg);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "内外地判定失败：" + ex.Message);
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
            DP_POCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
        //派车明细模型
        [HttpPost]
        public ActionResult SetFormControlDtl(string KeyValue)
        {

            VDP_POCARSORDER_DTL entity = bill_dtl.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

    }
}
