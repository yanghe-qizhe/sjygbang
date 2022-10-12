using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Entity;
using EOS.Business;
using EOS.DataAccess;
using System.Data.Common;
using EOS.Repository;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class ZJFAConfigController : PublicController<Base_ZJFAConfig>
    {
        string ModuleId = "";

        Base_ZJFAConfigBll bd_carsbll = new Base_ZJFAConfigBll();
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
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPageListJson(String SupplyName, string MaterialName, string FANAME, string ISUSE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<Base_ZJFAConfig> ListData = bd_carsbll.GetOrderList(SupplyName, MaterialName, FANAME, ISUSE, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(Base_ZJFAConfig entity, string ModuleId, string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string sql = "";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from Base_ZJFAConfig where MATERIAL='{0}' and SUPPLY='{1}' and FANO='{2}' and ID!='{3}'", entity.SUPPLY, entity.MATERIAL, entity.FANO, KeyValue);
                }
                else
                {
                    sql = String.Format("select count(1) from Base_ZJFAConfig where MATERIAL='{0}' and SUPPLY='{1}' and FANO='{2}' ", entity.SUPPLY, entity.MATERIAL, entity.FANO);
                }
                int res = DataFactory.Database().FindCountBySql(sql);

                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("供应商:{0}、物料:{1}、方案：{2}已存在", entity.SUPPLYNAME, entity.MATERIALNAME, entity.FANAME) }.ToString());
                }


                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
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
        public ActionResult SubmitOrderAllForm(Base_ZJFAConfig entity, string OrderEntryJson, string ModuleId)
        {
            this.ModuleId = ModuleId;
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                List<ZJFAConfigDetails> OrderEntryList = OrderEntryJson.JonsToList<ZJFAConfigDetails>();
                string sql = "";
                int res = 0;
                //处理明细
                int index = 1;
                string BillNo = entity.ID;
                foreach (ZJFAConfigDetails orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.SUPPLY))
                    {
                        base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        entity.Create();
                        entity.ID = this.BillCode();
                        entity.SUPPLY = orderentry.SUPPLY;
                        entity.SUPPLYNAME = orderentry.SUPPLYNAME;
                        entity.MEMO = orderentry.MEMO;
                        database.Insert(entity, isOpenTrans);
                        index++;
                    }
                }
                string Message = "新增成功。";
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
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJFAConfigBatchForm()
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
        public ActionResult ZJFAConfigForm()
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
        public ActionResult ZJFAConfigIndex()
        {
            return View();
        }



        [LoginAuthorize]
        public ActionResult SelectSupply()
        {
            return View();
        }

        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("Base_ZJFAConfig", "ID", KeyValue);
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
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            Base_ZJFAConfig entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
