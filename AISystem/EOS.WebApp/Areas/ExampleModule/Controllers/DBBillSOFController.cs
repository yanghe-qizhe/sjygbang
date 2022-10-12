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
    /// <summary>
    /// 调拨主实体
    /// </summary>
    public class DBOrder
    {
        public string INSTACKING { get; set; }
        public string INSTACKINGNAME { get; set; }
        public string OUTSTACKING { get; set; }
        public string OUTSTACKINGNAME { get; set; }
        public string MATERIAL { get; set; }
        public string MATERIALNAME { get; set; }
        public string BEGINDATE { get; set; }
        public string ENDDATE { get; set; }
        public string CREATEDATE { get; set; }
    }

    public class DBOrderEntity
    {
        public string ID { get; set; }
        public string CARS { get; set; }
        public string CARSNAME { get; set; }
        public string RFCARD { get; set; }
        public string MEMO { get; set; }
    }

    public class DBBillSOFController : PublicController<DP_DBCARSORDER>
    {
        string ModuleId = "";

        VDP_DBCARSORDERBll orderbll = new VDP_DBCARSORDERBll();
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
        public ActionResult DBBillSOFForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                string billno = this.BillCode();
                ViewBag.BillNo = billno;
                ViewBag.CARSNO = billno.Replace("HF01", "");
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        [LoginAuthorize]
        public ActionResult DBBillSOFIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult DBBillSOFDetail()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult DBBillSOFQuery()
        {
            return View();
        }

        
        /// <summary>
        /// 车号
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SELECTCARS()
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

        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string Send, string Receive, string Material, string Cars, string ISUSE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();

                List<VDP_DBCARSORDER> ListData = orderbll.GetOrderList(BillNo, Send, Receive, Material, Cars, ISUSE, StartTime, EndTime, jqgridparam);
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
        /// 提交订单表单（新增、编辑）
        /// </summary>
        /// <param name="KeyValue">订单主键</param>
        /// <param name="entity">订单实体</param>
        /// <param name="POOrderEntryJson">订单明细</param>
        /// <returns></returns>
        public ActionResult SubmitOrderForm(string KeyValue, string ModuleId, DP_DBCARSORDER entity, string OrderEntryJson)
        {
            entity.Create();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                DP_DBCARSORDER dborderentry = new DP_DBCARSORDER();
                //明细行数据
                List<DBOrderEntity> OrderEntryList = OrderEntryJson.JonsToList<DBOrderEntity>();
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "变更成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    foreach (DBOrderEntity orderentry in OrderEntryList)
                    {
                        if (!string.IsNullOrEmpty(orderentry.CARS))
                        {
                            dborderentry.SEND = entity.SEND;
                            dborderentry.SENDNAME = entity.SENDNAME;
                            dborderentry.RECEIVE = entity.RECEIVE;
                            dborderentry.RECEIVENAME = entity.RECEIVENAME;
        
                            dborderentry.OUTSTACKING = entity.OUTSTACKING;
                            dborderentry.OUTSTACKINGNAME = entity.OUTSTACKINGNAME;
                            dborderentry.INSTACKING = entity.INSTACKING;
                            dborderentry.INSTACKINGNAME = entity.INSTACKINGNAME;

                            dborderentry.MATERIAL = entity.MATERIAL;
                            dborderentry.MATERIALNAME = entity.MATERIALNAME;
                            dborderentry.BEGINDATE = entity.BEGINDATE;
                            dborderentry.ENDDATE = entity.ENDDATE;

                            dborderentry.CARS = orderentry.CARS;
                            dborderentry.CARSNAME = orderentry.CARSNAME;
                            dborderentry.CARSNAME = orderentry.CARSNAME;
                            dborderentry.RFCARD = orderentry.RFCARD;
                            dborderentry.MEMO = orderentry.MEMO;
                            dborderentry.ID = orderentry.ID;

                            database.Update(dborderentry, isOpenTrans);
                        }
                    }
                }
                else
                {
                  
                    string UserId = ManageProvider.Provider.Current().UserId;
                    foreach (DBOrderEntity orderentry in OrderEntryList)
                    {
                        if (!string.IsNullOrEmpty(orderentry.CARS))
                        {
                            dborderentry.YBILLNO = entity.YBILLNO;
                            dborderentry.ISUSE = "1";
                            dborderentry.STATUS = "1";
                            dborderentry.CREATEDATE = entity.CREATEDATE;
                            dborderentry.CREATEUSER = entity.CREATEUSER;

                            dborderentry.SEND = entity.SEND;
                            dborderentry.SENDNAME = entity.SENDNAME;
                            dborderentry.RECEIVE = entity.RECEIVE;
                            dborderentry.RECEIVENAME = entity.RECEIVENAME;
                            dborderentry.INSTACKING = entity.INSTACKING;
                            dborderentry.INSTACKINGNAME = entity.INSTACKINGNAME;
                            dborderentry.OUTSTACKING = entity.OUTSTACKING;
                            dborderentry.OUTSTACKINGNAME = entity.OUTSTACKINGNAME;
                            dborderentry.MATERIAL = entity.MATERIAL;
                            dborderentry.MATERIALNAME = entity.MATERIALNAME;
                            dborderentry.BEGINDATE = entity.BEGINDATE;
                            dborderentry.ENDDATE = entity.ENDDATE;

                            dborderentry.CARS = orderentry.CARS;
                            dborderentry.CARSNAME = orderentry.CARSNAME;
                            dborderentry.CARSNAME = orderentry.CARSNAME;
                            dborderentry.RFCARD = orderentry.RFCARD;
                            dborderentry.MEMO = orderentry.MEMO;
                            dborderentry.ID = base_coderulebll.GetBillCode(UserId, ModuleId);

                            database.Insert(dborderentry, isOpenTrans);
                            base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        }
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

        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from DP_DBCARSORDER where ID='{0}' and isuse!=4", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为作废的数据能删除操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("DP_DBCARSORDER", "ID", KeyValue);
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


        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue)
        {
            //验证
            DP_DBCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "4")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该数据已作废！" }.ToString());
            }
            else if (entity.ISUSE != "1" && entity.ISUSE != "5")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为未入厂的发货单能作废操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "作废成功。";
                entity.ISUSE = "4";
                entity.MEMO = "作废";
                database.Update(entity, isOpenTrans);
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
        [LoginAuthorize]
        public ActionResult OutOrder(string KeyValue)
        {
            //验证
            DP_DBCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "6")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该发货单已出厂！" }.ToString());
            }
            else if (entity.ISUSE != "3")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为空车的发货单能出厂操作！" }.ToString());
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
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            DP_DBCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该数据是审批状态！" }.ToString());
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
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            DP_DBCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
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
        /// 【发货单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            DP_DBCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        /// <summary>
        /// 订单明细列表（返回Json）
        /// </summary>
        /// <param name="POOrderId">订单主键</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderEntryList(string TASKNO)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetOrderEntryList(TASKNO),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
    }
}
