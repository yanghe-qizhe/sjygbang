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
    public class DBOrderIN
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

    public class DBOrderINEntity
    {
        public string ID { get; set; }
        public string CARS { get; set; }
        public string CARSNAME { get; set; }
        public string RFCARD { get; set; }
        public string MEMO { get; set; }
    }

    public class DBBillSOFINController : PublicController<DP_DBINCARSORDER>
    {
        string ModuleId = "";

        VDP_DBINCARSORDERBll orderbll = new VDP_DBINCARSORDERBll();
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
        public ActionResult DBBillSOFINForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                string billno = this.BillCode();
                ViewBag.BillNo = billno;
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        [LoginAuthorize]
        public ActionResult DBBillSOFINIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult DBBillSOFINDetail()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult DBBillSOFINQuery()
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
        public ActionResult GetOrderList(string BillNo, string PK_ORG, string Send, string Receive, string Material, string Cars, string ISUSE, string STATUS, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();

                List<VDP_DBINCARSORDER> ListData = orderbll.GetOrderList(BillNo, PK_ORG, Send, Receive, Material, Cars, ISUSE, STATUS, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(string KeyValue, DP_DBINCARSORDER entity, string OrderEntryJson, string ModuleId)
        {
            string sql = "";
            if (!string.IsNullOrEmpty(KeyValue))
            {
                sql = String.Format("SELECT count(1) NUMS FROM DP_DBINCARSORDER WHERE STATUS=1 and OUTSTACKING='{0}' and INSTACKING='{1}' and ID!='{2}'", entity.OUTSTACKING, entity.INSTACKING, entity.ID);
            }
            else
            {
                sql = String.Format("SELECT count(1) NUMS FROM DP_DBINCARSORDER WHERE STATUS=1 and OUTSTACKING='{0}' and INSTACKING='{1}'", entity.OUTSTACKING, entity.INSTACKING);
            }
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("厂内倒运计划单:{0}：发货仓库:{1}，收货仓库:{2}，存在已启用的计划！", entity.ID, entity.OUTSTACKINGNAME, entity.INSTACKINGNAME) }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<DP_DBINCARSORDER_DTL> OrderEntryList = OrderEntryJson.JonsToList<DP_DBINCARSORDER_DTL>();

                string[] arr = OrderEntryList.Where(t => string.IsNullOrEmpty(t.CARS) == false).Select(s => s.CARS).ToArray();

                if (arr.Length > 0)
                {
                    if (arr.Length == 1)
                    {
                        sql = String.Format("SELECT count(1) NUMS FROM DP_DBINCARSORDER_DTL WHERE ISUSE=1 and PK_DBINCARSORDER!='{0}' and CARS='{1}'", entity.ID, arr[0]);
                    }
                    else
                    {
                        string str = string.Join("','", arr);
                        sql = String.Format("SELECT count(1) NUMS FROM DP_DBINCARSORDER_DTL WHERE ISUSE=1 and PK_DBINCARSORDER!='{0}' and CARS in('{1}')", entity.ID, str);
                    }
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("厂内倒运计划单:{0}中明细车辆在其它计划中存在！", entity.ID) }.ToString());
                    }
                }

                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "变更成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    database.Delete<DP_DBINCARSORDER_DTL>("PK_DBINCARSORDER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from DP_DBINCARSORDER where  ID='{0}'", entity.ID);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.ID = this.BillCode();
                    }
                    entity.Create();
                    entity.STATUS = "0";
                    database.Insert(entity, isOpenTrans);
                }
                //处理明细
                int index = 1;

                foreach (DP_DBINCARSORDER_DTL orderentry in OrderEntryList)
                {

                    if (!string.IsNullOrEmpty(orderentry.CARS))
                    {
                        orderentry.Create();
                        orderentry.ID = string.Format("{0}0{1}", entity.ID, index);
                        orderentry.PK_DBINCARSORDER = entity.ID;
                        database.Insert(orderentry, isOpenTrans);
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
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from DP_DBINCARSORDER where ID='{0}' and status=1", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为停用的数据能删除操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("DP_DBINCARSORDER", "ID", KeyValue);
                database.Delete("DP_DBINCARSORDER_DTL", "PK_DBINCARSORDER", KeyValue);
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
            DP_DBINCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
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
            DP_DBINCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
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
            DP_DBINCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计划单是启用状态！" }.ToString());
            }

            string sql = String.Format("SELECT count(1) NUMS FROM DP_DBINCARSORDER WHERE STATUS=1 and OUTSTACKING='{0}' and INSTACKING='{1}' and ID!='{2}'", entity.OUTSTACKING, entity.INSTACKING, entity.ID);

            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("厂内倒运计划单:{0}：发货仓库:{1}，收货仓库:{2}，存在已启用的计划！", entity.ID, entity.OUTSTACKINGNAME, entity.INSTACKINGNAME) }.ToString());
            }
            sql = String.Format("select count(1) CARNUM from DP_DBINCARSORDER where STATUS=1 and ID!='{0}' and id  in(select PK_DBINCARSORDER from DP_DBINCARSORDER_DTL where CARS in(select CARS from DP_DBINCARSORDER_DTL where PK_DBINCARSORDER='{0}'))", entity.ID);
            res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("厂内倒运计划:{0}明细车辆有已启用的计划！", entity.ID) }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "启用成功。";
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

        //停用
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            DP_DBINCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计划单是停用状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "停用成功。";
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
            DP_DBINCARSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
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
    }
}
