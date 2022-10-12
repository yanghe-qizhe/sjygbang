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

    public class OtherWeightOutController : PublicController<WB_WEIGHT>
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

        //
        // GET: /WeightManager/WeightOut/

        public ActionResult OtherWeightOutIndex()
        {
            return View();
        }
        public ActionResult OtherWeightOutDetail()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult OtherWeightOutForm()
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
        public ActionResult OtherWeightOutQuery()
        {
            return View();
        }

        /// 销售磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string VBillCode, string ContractBillNo, string FHBillNo, string Customer, string Material, string Cars, string Status, string finish, string ZFFLAG, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHTBS> ListData = orderbll.GetQTCKOrderList(BillNo, VBillCode, ContractBillNo, FHBillNo, Customer, Material, Cars, Status, "5", finish, StartTime, EndTime, ZFFLAG, jqgridparam);
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

                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    entity.ISEDIT = 1;
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    string sql = String.Format("select count(1) from WB_WEIGHT where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    entity.TYPE = "5";
                    entity.FINISH = "1";
                    entity.INGBUSER = ManageProvider.Provider.Current().UserName;// "磅房管理员";
                    database.Insert(entity, isOpenTrans);
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
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
        public ActionResult InvalidOrder(string KeyValue)
        {
           
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
                entity.MEMO = "作废";
                database.Update(entity, isOpenTrans);
                if (!string.IsNullOrEmpty(entity.PK_ORDER))
                {
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_OTCARSORDER set IsUse='{0}' where ID='{1}'", 4, entity.PK_ORDER));
                    repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);

                    //门禁作废
                    strSql.Clear();
                    strSql.Append(string.Format("update wb_weight_task set finish='1',status='作废' where PK_ORDER='{0}'", entity.PK_ORDER));
                    repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
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
        /// 【到货单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            WB_WEIGHT entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();

            VNC_SALEORDER_B sorder = DataFactory.Database().FindEntityBySql<VNC_SALEORDER_B>(string.Format("select * from VNC_SALEORDER_BQY where CSALEORDERBID='{0}'", entity.PK_BILL_B));
            JsonData = JsonData.Insert(1, "\"NASTNUM\":\"" + sorder.NASTNUM + "\",");
            JsonData = JsonData.Insert(1, "\"YLNUM\":\"" + sorder.YLNUM + "\",");
            return Content(JsonData);
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


        #endregion
    }
}
