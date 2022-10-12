using EOS.Business;
using EOS.Entity;
using EOS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using EOS.DataAccess;
using EOS.Repository;

namespace EOS.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// 编码规则主表控制器
    /// </summary>
    public class CodeRuleController : PublicController<Base_CodeRule>
    {

        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        /// <summary>
        /// 获取自动单据内码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }
        //列表
        [LoginAuthorize]
        public ActionResult CodeRuleIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult CodeRuleForm()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.Code = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            else
            {
                Base_CodeRule entity = repositoryfactory.Repository().FindEntity(KeyValue);
                ViewBag.ModuleName = DataFactory.Database().FindEntity<Base_Module>(entity.ModuleId).FullName;
            }
            return View();
        }
        /// <summary>
        /// 表单页面
        /// </summary>
        /// <returns></returns>
        public override ActionResult Form()
        {
            string KeyValue = Request["KeyValue"];//主键
            if (!string.IsNullOrEmpty(KeyValue))
            {
                Base_CodeRule entity = repositoryfactory.Repository().FindEntity(KeyValue);
                ViewBag.ModuleName = DataFactory.Database().FindEntity<Base_Module>(entity.ModuleId).FullName;
            }
            return View();
        }
        #region 列表
        /// <summary>
        /// 【编码规则】返回列表JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult ListJson()
        {
            List<Base_CodeRule> list = base_coderulebll.GetList();
            return Content(list.ToJson());
        }

        /// <summary>
        /// 【编码规则】返回主表JSON
        /// </summary>
        /// <param name="ViewId">主表 主键值</param>
        /// <returns></returns>
        public JsonResult GetEntityJson(string CodeRuleId)
        {
            Base_CodeRule entity = repositoryfactory.Repository().FindEntity(CodeRuleId);
            return Json(entity, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 【编码规则】返回列表JSON
        /// </summary>
        /// <param name="ViewId">主表 主键值</param>
        /// <returns></returns>
        public JsonResult GetDetailsEntityJson(string CodeRuleId)
        {
            List<Base_CodeRuleDetail> list = DataFactory.Database().FindList<Base_CodeRuleDetail>("CodeRuleId", CodeRuleId).OrderBy(o => o.SortCode).ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region 表单

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="TableDescription">表说明</param>
        /// <param name="TableFieldJson">表字段</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SubmitForm_CodeRule(string KeyValue, Base_CodeRule base_coderule, string CodeRuleDetailJson)
        {
            try
            {
                int IsOk = 0;
                IsOk = base_coderulebll.SubmitForm(KeyValue, base_coderule, CodeRuleDetailJson);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 明细
        #endregion
    }
}