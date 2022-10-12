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
    public class WeightNoController : PublicController<WB_SYS_WEIGHTCONFIG>
    {
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();

        WB_SYS_WEIGHTCONFIGBll bd_bll = new WB_SYS_WEIGHTCONFIGBll();
        /// <summary>
        /// 获取自动单据内码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }


        /// <summary>
        /// 列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderList(string keyword, string isuse, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<WB_SYS_WEIGHTCONFIG> ListData = bd_bll.GetOrderList(keyword, isuse, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(WB_SYS_WEIGHTCONFIG entity, string ModuleId, string KeyValue)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    entity.CODE = entity.ID;
                    entity.SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
                  //  database.Update(entity, isOpenTrans);
                      System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Clear();
                    strSql.Append(string.Format("update WB_SYS_WEIGHTCONFIG set NAME='{0}',SHORTNAME='{1}',DEF1='{2}',ISUSE={3},MEMO='{4}' where ID='{5}'", entity.NAME,entity.SHORTNAME, entity.DEF1, entity.ISUSE, entity.MEMO, KeyValue));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    entity.CODE = entity.ID;
                    entity.SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
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

        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("WB_SYS_WEIGHTCONFIG", "ID", KeyValue);
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
        public ActionResult WeightNoForm()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.ID = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        [LoginAuthorize]
        public ActionResult WeightNoIndex()
        {
            return View();
        }
        /// <summary>
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            WB_SYS_WEIGHTCONFIG entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
