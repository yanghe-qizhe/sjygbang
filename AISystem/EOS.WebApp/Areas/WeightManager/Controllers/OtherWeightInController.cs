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

namespace EOS.WebApp.Areas.WeightManager.Controllers
{
    public class OtherWeightInController : PublicController<WB_WEIGHT>
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
        // GET: /WeightManager/WeightIn/
        [LoginAuthorize]
        public ActionResult OtherWeightInIndex()
        {
            return View();
        }

        public ActionResult OtherWeightInDetail()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult OtherWeightInForm()
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
        public ActionResult OtherWeightInQuery()
        {
            return View();
        }

        /// 采购磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string VBillCode, string ContractBillNo, string DHBillNo, string Supply, string Material, string Cars, string Status, string FINISH, string ZFFLAG, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHTBS> ListData = orderbll.GetQTRKOrderList(BillNo, VBillCode, ContractBillNo, DHBillNo, Supply, Material, Cars, Status, "4", FINISH, StartTime, EndTime, ZFFLAG, jqgridparam);
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
                    entity.TYPE = "4";
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
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该采购计量单已作废！" }.ToString());
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
            return Content(JsonData);
        }
    }
}
