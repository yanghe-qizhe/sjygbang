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
    public class ZXBillSOFController : PublicController<Z_ITEM_DEMAND_SUM>
    {
        string ModuleId = "";
        VZ_ITEM_DEMAND_SUMBll vorderbll = new VZ_ITEM_DEMAND_SUMBll();
        Z_ITEM_DEMAND_SUMBll orderbll = new Z_ITEM_DEMAND_SUMBll();
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
        /// 上料计划表单
        /// </summary>
        /// <returns></returns>

        [LoginAuthorize]
        public ActionResult ZXBillSOF_FORM()
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

        /// <summary>
        /// 批量上料计划表单
        /// </summary>
        /// <returns></returns>

        [LoginAuthorize]
        public ActionResult ZXBillSOF_BATCHFORM()
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
        public ActionResult ZXBillSOF_FORMFJ()
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
        public ActionResult ZXBillSOFIndex()
        {
            return View();
        }
        //明细
        [LoginAuthorize]
        public ActionResult ZXBillSOFDetail()
        {
            return View();
        }



        /// 订单列表（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string SITENAME, string WORK_SHOP_BONAME, string PRODUCTION_LINE_BONAME, string OPERATION_BONAME, string SHIFT, string MATERIALNAME, string STATUS, string CREATED_USER, string ISFJ, string TYPE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZ_ITEM_DEMAND_SUM> ListData = vorderbll.GetOrderList(BillNo, SITENAME, WORK_SHOP_BONAME, PRODUCTION_LINE_BONAME, OPERATION_BONAME, SHIFT, MATERIALNAME, STATUS, CREATED_USER, ISFJ, TYPE, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(string KeyValue, Z_ITEM_DEMAND_SUM entity, string ModuleId)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;

                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    Z_ITEM_DEMAND_SUM Oldentity = database.FindEntity<Z_ITEM_DEMAND_SUM>(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    entity.DATATYPE = "0";
                    IsOk = database.Update(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    entity.Create();
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from Z_ITEM_DEMAND_SUM where  ID='{0}'", entity.ID);
                    entity.DATATYPE = "0";

                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.ID = this.BillCode();
                        entity.HANDLE = entity.ID;
                    }
                    if (entity.SHIFTNAME == "全班")
                    {
                        entity.SHIFT = "B";
                        entity.SHIFTNAME = "白班";
                        entity.ID = $"{entity.ID}01";
                        entity.HANDLE = $"{entity.ID}01";
                        IsOk = database.Insert(entity, isOpenTrans);
                        entity.SHIFT = "C";
                        entity.SHIFTNAME = "中班";
                        entity.ID = $"{entity.ID}02";
                        entity.HANDLE = $"{entity.ID}02";
                        IsOk = database.Insert(entity, isOpenTrans);
                        entity.SHIFT = "A";
                        entity.SHIFTNAME = "夜班";
                        entity.ID = $"{entity.ID}03";
                        entity.HANDLE = $"{entity.ID}03";
                        IsOk = database.Insert(entity, isOpenTrans);
                    }
                    else
                    {
                        IsOk = database.Insert(entity, isOpenTrans);
                    }

                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("上料计划单{0}。", Message);
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
        public ActionResult SubmitOrderBForm(Z_ITEM_DEMAND_SUM entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证
            //验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                //明细行数据
                List<Z_ITEM_DEMAND_SUM> OrderEntryList = OrderEntryJson.JonsToList<Z_ITEM_DEMAND_SUM>();
                //处理主头
                string Message = "批量新增成功。";
                int i = 1;
                foreach (Z_ITEM_DEMAND_SUM orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.WORK_SHOP_BO))
                    {
                        orderentry.Create();
                        orderentry.ID = string.Format("{0}{1:D2}", entity.ID, i);
                        orderentry.HANDLE = string.Format("{0}{1:D2}", entity.ID, i);
                        orderentry.CREATED_DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        orderentry.CREATED_USER = ManageProvider.Provider.Current().UserName;
                        orderentry.MODIFED_DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        orderentry.MODIFED_USER = ManageProvider.Provider.Current().UserName;
                        orderentry.DEMAND_CONFIRMED_DATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        orderentry.DEMAND_CONFIRMED_USER = ManageProvider.Provider.Current().UserName;
                        database.Insert(orderentry, isOpenTrans);
                        i++;
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


        [HttpPost]
        public ActionResult SubmitOrderFormFJ(string KeyValue, Z_ITEM_DEMAND_SUM entity, string OrderEntryJson, string ModuleId)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                //明细行数据
                List<Z_ITEM_DEMAND_SUM_DTL> OrderEntryList = OrderEntryJson.JonsToList<Z_ITEM_DEMAND_SUM_DTL>();

                database.Delete<Z_ITEM_DEMAND_SUM_DTL>("MAINID", KeyValue, isOpenTrans);
                string Message = "分解成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update Z_ITEM_DEMAND_SUM set ISFJ=1 where ID='{0}'", KeyValue));
                database.ExecuteBySql(strSql, isOpenTrans);
                //处理明细
                int index = 1;
                foreach (Z_ITEM_DEMAND_SUM_DTL orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.STORE))
                    {
                        orderentry.Create();
                        orderentry.ID = string.Format("{0}{1:00}", entity.ID, index);
                        orderentry.MAINID = entity.ID;
                        orderentry.HANDLE = entity.HANDLE;
                        orderentry.SITE = entity.SITE;
                        orderentry.SITENAME = entity.SITENAME;
                        orderentry.WORK_SHOP_BO = entity.WORK_SHOP_BO;
                        orderentry.WORK_SHOP_BONAME = entity.WORK_SHOP_BONAME;
                        orderentry.PRODUCTION_LINE_BO = entity.PRODUCTION_LINE_BO;
                        orderentry.PRODUCTION_LINE_BONAME = entity.PRODUCTION_LINE_BONAME;
                        orderentry.OPERATION_BO = entity.OPERATION_BO;
                        orderentry.OPERATION_BONAME = entity.OPERATION_BONAME;
                        orderentry.ITEM_BO = entity.ITEM_BO;
                        orderentry.ITEM_BONAME = entity.ITEM_BONAME;
                        orderentry.ITEM_BONAME = entity.ITEM_BONAME;
                        orderentry.DEMAND_ORIGINAL_QTY = entity.DEMAND_ORIGINAL_QTY;
                        orderentry.DEMAND_CONFIRMED_QTY = entity.DEMAND_CONFIRMED_QTY;
                        //orderentry.NET_DEMAND_CONFIRMED_QTY = entity.NET_DEMAND_CONFIRMED_QTY;

                        orderentry.SYNC_FLAG = orderentry.SYNC_FLAG;
                        orderentry.DEMAND_DATE = entity.DEMAND_DATE;
                        orderentry.SHIFT = entity.SHIFT;
                        orderentry.SHIFTNAME = entity.SHIFTNAME;
                        orderentry.WORK_BO = entity.WORK_BO;
                        orderentry.WORK_BONAME = entity.WORK_BONAME;
                        orderentry.DATATYPE = entity.DATATYPE;
                        orderentry.TYPE = entity.TYPE;
                        orderentry.DEMAND_CONFIRMED_DATE = entity.DEMAND_CONFIRMED_DATE;
                        orderentry.DEMAND_CONFIRMED_USER = entity.DEMAND_CONFIRMED_USER;
                        orderentry.CREATED_USER = ManageProvider.Provider.Current().UserName;
                        orderentry.CREATED_DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        orderentry.MODIFED_USER = ManageProvider.Provider.Current().UserName;
                        orderentry.MODIFED_DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        orderentry.DATATYPE = "0";
                        database.Insert(orderentry, isOpenTrans);
                        index++;
                    }
                }

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("上料计划单分解成功。");
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
            //string sql = String.Format("select count(1) from Z_ITEM_DEMAND_SUM where ID='{0}' and isuse!=4", KeyValue);
            //int res = DataFactory.Database().FindCountBySql(sql);
            //if (res > 0)
            //{
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为作废的到货单能删除操作！" }.ToString());
            //}

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                //database.Delete("Z_ITEM_DEMAND_SUM", "ID", KeyValue);
                database.Delete("Z_ITEM_DEMAND_SUM_DTL", "ID", KeyValue);
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

        //启用
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            Z_ITEM_DEMAND_SUM_DTL entity = DataFactory.Database().FindEntity<Z_ITEM_DEMAND_SUM_DTL>(KeyValue);
            if (entity.STATUS == "CONFIRMED")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该分解计划单是已确认状态！" }.ToString());
            }

            string sql = String.Format("SELECT count(1) NUMS FROM Z_ITEM_DEMAND_SUM_DTL WHERE STATUS=1 and STORE='{0}' and ID!='{1}'", entity.STORE, entity.ID);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("上料分解计划单:{0}：库存:{1}，存在已确认的计划！", entity.ID, entity.STORE) }.ToString());
            }


            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "确认成功。";
                entity.STATUS = "CONFIRMED";
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

        //停用
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            Z_ITEM_DEMAND_SUM_DTL entity = DataFactory.Database().FindEntity<Z_ITEM_DEMAND_SUM_DTL>(KeyValue);
            if (entity.STATUS == "CANCELED")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该分解计划单是已取消状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "取消确认成功。";
                entity.STATUS = "CANCELED";
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
        /// 【上料计划单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            VZ_ITEM_DEMAND_SUM entity = vorderbll.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
        public ActionResult GetBatch(string Store,string Material)
        {
            BD_MATSTORE_BATCHNOBll batchorderbll = new BD_MATSTORE_BATCHNOBll();
            BD_MATSTORE_BATCHNO entity = batchorderbll.GetCKWLPHEntity(Material, Store);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
        
    }
}
