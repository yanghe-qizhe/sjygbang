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
    public class BillSOFController : PublicController<DP_SOCARSORDER>
    {
        string ModuleId = "";

        DP_SOCARSORDERBll orderbll = new DP_SOCARSORDERBll();
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
        public ActionResult BillSOFForm()
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
        public ActionResult BillSOF_FORM()
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
        public ActionResult BillSOF_FORM_BZ()
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
        public ActionResult BillSOFIndex()
        {
            return View();
        }
        //明细
        [LoginAuthorize]
        public ActionResult BillSOFDetail()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult BillSOFQuery()
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
                List<DP_SOCARSORDER> ListData = orderbll.GetOrderList(BillNo, StartTime, EndTime, jqgridparam);
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

        /// 订单列表（返回Json）
        [LoginAuthorize]
        public ActionResult GetQYOrderList(string BillNo, string VBillCode, string PK_ORG, string Customer, string Material, string Cars, string ISUSE, string matclass, string DEF1, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_SOCARSORDERBll vorderbll = new VDP_SOCARSORDERBll();
                List<VDP_SOCARSORDER> ListData = vorderbll.GetOrderList(BillNo, VBillCode, PK_ORG, Customer, Material, Cars, ISUSE, "1", matclass, DEF1, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetSQYOrderList(string BillNo, string VBillCode, string Customer, string Material, string Cars, string ISUSE, string matclass, string BILLSOFTYPE, string DEF1, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();


                VDP_SOCARSORDER_DTLBll vorderbll = new VDP_SOCARSORDER_DTLBll();
                List<VDP_SOCARSORDER_DTL> ListData = vorderbll.GetOrderList(BillNo, VBillCode, Customer, Material, Cars, ISUSE, matclass, "1", BILLSOFTYPE, DEF1, StartTime, EndTime, jqgridparam);
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
        /// <param name="ID">订单主键</param>
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


        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm(string KeyValue, DP_SOCARSORDER entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证
            //车辆验证
            JsonMessage jsonmsg = CHECK_CARS_ORDER(entity.CARS, 2, entity.ID,entity.DEF1);
            if (jsonmsg != null)
            {
                return Content(jsonmsg.ToString());
            }
            //身份证验证
            jsonmsg = CHECK_CARDNUMBER_ORDER(entity.PSDNO, 2, entity.ID, entity.DEF1);
            if (jsonmsg != null)
            {
                return Content(jsonmsg.ToString());
            }
            #endregion

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                //明细行数据
                List<DP_SOCARSORDER_DTL> OrderEntryList = OrderEntryJson.JonsToList<DP_SOCARSORDER_DTL>();

                int num = OrderEntryList.Where(p => !p.VBILLCODE.IsNull()).ToList().Count;
                if (num > 1)
                {
                    entity.ISMULTI = "1";
                }
                else
                {
                    entity.ISMULTI = "0";
                }
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    DP_SOCARSORDER Oldentity = database.FindEntity<DP_SOCARSORDER>(KeyValue);//获取没更新之前实体对象
                    database.Delete<DP_SOCARSORDER_DTL>("PK_DP_SOCARSORDER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    IsOk = database.Update(entity, isOpenTrans);
                    EDIT_CARSORDER(entity.CARS, entity.CARSNAME, entity.CARDNO, entity.DRIVERS, entity.DRIVERSNAME, entity.PSDNO, "", "", entity.ID, database, isOpenTrans);
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from DP_SOCARSORDER where  ID='{0}'", entity.ID);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.ID = this.BillCode();
                    }
                    entity.Create();
                    IsOk = database.Insert(entity, isOpenTrans);

                    ADD_CARSORDER(entity.CARS, entity.CARSNAME, entity.CARDNO, entity.DRIVERS, entity.DRIVERSNAME, entity.PSDNO, "", "", entity.ID, 2, 0, database, isOpenTrans);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
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
                foreach (DP_SOCARSORDER_DTL orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.VBILLCODE))
                    {
                        orderentry.Create();
                        orderentry.ID = string.Format("{0}0{1}", entity.ID, index);
                        orderentry.PK_DP_SOCARSORDER = entity.ID;
                        orderentry.CCUSTOMERID = entity.CCUSTOMERID;
                        orderentry.CUSTNAME = entity.CUSTNAME;
                        database.Insert(orderentry, isOpenTrans);
                        index++;
                    }
                }

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运发货单号:{0}，车辆:{1}，{2}。", entity.ID, entity.CARSNAME, Message);
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


        [HttpPost]
        public ActionResult SubmitOrderForm_BZ(string KeyValue, DP_SOCARSORDER entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证
            //车辆验证
            JsonMessage jsonmsg = CHECK_CARS_ORDER(entity.CARS, 2, entity.ID, entity.DEF1);
            if (jsonmsg != null)
            {
                return Content(jsonmsg.ToString());
            }
            //身份证验证
            jsonmsg = CHECK_CARDNUMBER_ORDER(entity.PSDNO, 2, entity.ID, entity.DEF1);
            if (jsonmsg != null)
            {
                return Content(jsonmsg.ToString());
            }
            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                //明细行数据
                List<DP_SOCARSORDER_DTL> OrderEntryList = OrderEntryJson.JonsToList<DP_SOCARSORDER_DTL>();

                int num = OrderEntryList.Where(p => !p.VBILLCODE.IsNull()).ToList().Count;
                if (num > 1)
                {
                    entity.ISMULTI = "1";
                }
                else
                {
                    entity.ISMULTI = "0";
                }
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    DP_SOCARSORDER Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    database.Delete<DP_SOCARSORDER_DTL>("PK_DP_SOCARSORDER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    IsOk = database.Update(entity, isOpenTrans);
                    //EDIT_CARSORDER(entity.CARS, entity.CARSNAME, entity.CARDNO, entity.DRIVERS, entity.DRIVERSNAME, entity.PSDNO, "", "", entity.ID, database, isOpenTrans);
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from DP_SOCARSORDER where  ID='{0}'", entity.ID);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.ID = this.BillCode();
                    }
                    entity.Create();
                    entity.BILLSOFTYPE = "BZ";
                    IsOk = database.Insert(entity, isOpenTrans);

                    //ADD_CARSORDER(entity.CARS, entity.CARSNAME, entity.CARDNO, entity.DRIVERS, entity.DRIVERSNAME, entity.PSDNO, "", "", entity.ID, 2, 0, database, isOpenTrans);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
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
                foreach (DP_SOCARSORDER_DTL orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.VBILLCODE))
                    {
                        orderentry.Create();
                        orderentry.ID = string.Format("{0}0{1}", entity.ID, index);
                        orderentry.PK_DP_SOCARSORDER = entity.ID;
                        orderentry.CCUSTOMERID = entity.CCUSTOMERID;
                        orderentry.CUSTNAME = entity.CUSTNAME;
                        database.Insert(orderentry, isOpenTrans);
                        index++;
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运发货单号:{0}，车辆:{1}，新增成功。", entity.ID, entity.CARSNAME);
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
            string sql = String.Format("select count(1) from DP_SOCARSORDER where ID='{0}' and isuse!=4", KeyValue);
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
                database.Delete("DP_SOCARSORDER", "ID", KeyValue);
                database.Delete("DP_SOCARSORDER_DTL", "PK_DP_SOCARSORDER", KeyValue);
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
            DP_SOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "4")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该发货单已作废！" }.ToString());
            }
            //else if (entity.ISUSE != "1" && entity.ISUSE != "5")
            //{
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为未入厂的发货单能作废操作！" }.ToString());
            //}
            //string sql = String.Format("SELECT count(1) NUMS FROM WB_WEIGHT WHERE  finish=0 and PK_ORDER='{0}'", entity.ID);
            //int res = DataFactory.Database().FindCountBySql(sql);
            //if (res > 0)
            //{
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}的发货单:{1}已一次计量不能作废操作！", entity.CARSNAME, entity.ID) }.ToString());
            //}

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Account = ManageProvider.Provider.Current().Account;
                string Message = "作废成功。";
                entity.ISUSE = "4";
                entity.MEMO = $"业务平台作废:作废人{Account}、时间{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}";
                database.Update(entity, isOpenTrans);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //明细
                strSql.Append(string.Format("update DP_SOCARSORDER_DTL set ISUSE='4',MEMO='业务平台作废' where PK_DP_SOCARSORDER='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                //门禁出厂
                strSql.Clear();
                strSql.Append(string.Format("update wb_weight_task set finish='1',status='出厂' where PK_ORDER='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                DEL_CARSORDERS(KeyValue, database, isOpenTrans);


                database.Commit();
              
                string message = string.Format("汽运发货单:{0}，车辆{1}业务平台作废成功。", entity.ID, entity.CARSNAME);
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
            DP_SOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "6")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该发货单已出厂！" }.ToString());
            }
            string sql = String.Format("select count(1) from DP_SOCARSORDER_DTL where isuse<>2 and PK_DP_SOCARSORDER='{0}'", KeyValue);

            int res = DataFactory.Database().FindCountBySql(sql);

            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("该发货单有未二次计量的订单明细") }.ToString());
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
                database.ExecuteBySql(strSql, isOpenTrans);
                //过磅出厂
                strSql.Clear();
                strSql.Append(string.Format("update wb_weight set finish='1',status='完成' where PK_ORDER='{0}'", KeyValue));
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
        //审批
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            DP_SOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该到发单是审批状态！" }.ToString());
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
            DP_SOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该发货单是未审批状态！" }.ToString());
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
            DP_SOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.DEF12 == 1)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该发货单是已签到状态！" }.ToString());
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
                strSql.Append(string.Format("update DP_SOCARSORDER_DTL set DEF12=1,DEF9='{0}' where PK_DP_SOCARSORDER='{1}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                //string sql = "";
                //List<DP_SOCARSORDER_DTL> list = orderbll.GetOrderEntryList(KeyValue);
                //sql = String.Format("select count(1)+1 CARSNUM from Sort_Order");
                //int VINDEX = DataFactory.Database().FindCountBySql(sql);

                //foreach (DP_SOCARSORDER_DTL model in list)
                //{
                //    strSql.Clear();
                //    sql = string.Format("insert into Sort_Order(ID,BILLTYPE,ORDERSTATUS,ORDERTIME,CALLTIME,PK_ASSIGNPLAN,PK_CARS,CARSNAME,PK_STORE,STORENAME,PK_MATERIAL,MATERIALNAME,MATSPEC,PK_DRIVER,DRIVERNAME,VINDEX,SENDTIME,CALLCOUNT) values(");
                //    sql += string.Format("'{0}',{1},{2},'{3}','',", CommonHelper.GetGuid, 0, 0, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //    sql += string.Format("'{0}','{1}','{2}','{3}','{4}',", entity.ID, entity.CARS, entity.CARSNAME,"", "");
                //    sql += string.Format("'{0}','{1}','{2}','{3}','{4}',", model.CMATERIALID, model.MATERIALNAME, model.MATERIALSPEC, entity.DRIVERS, entity.DRIVERSNAME);
                //    sql += string.Format("{0},'',0", VINDEX);
                //    sql += string.Format(")");
                //    strSql.Append(sql);
                //    database.ExecuteBySql(strSql, isOpenTrans);
                //}

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("汽运发货单号:{0}，{1}。", KeyValue, Message);
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
        /// 【发货单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            DP_SOCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
