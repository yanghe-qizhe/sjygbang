using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EOS.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// 角色管理控制器
    /// </summary>
    public class RolesController : PublicController<Base_Roles>
    {
        Base_RolesBll base_rolesbll = new Base_RolesBll();
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult RolesIndex()
        {
            return View();
        }
      
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult RolesIndexWG()
        {
            return View();
        }
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult RolesForm()
        {
            return View();
        }
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult RolesDetail()
        {
            return View();
        }
        /// <summary>
        /// 【角色管理】返回列表JONS
        /// </summary>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="jqgridparam">JqGrid表格参数</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(string CompanyId, string keyword, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = base_rolesbll.GetPageList(CompanyId, keyword, ref jqgridparam);
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
        public override ActionResult SubmitForm(Base_Roles entity, string KeyValue)
        {
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
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
                    Base_DataScopePermissionBll.Instance.AddScopeDefault(ModuleId, ManageProvider.Provider.Current().UserId, entity.RoleId, "2", isOpenTrans);
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
    }
}