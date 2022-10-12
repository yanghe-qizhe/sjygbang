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
    /// �û�����������
    /// </summary>
    public class GroupUserController : PublicController<Base_GroupUser>
    {
        Base_GroupUserBll base_groupuserbll = new Base_GroupUserBll();


        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult GroupUserIndex()
        {
            return View();
        }
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult GroupUserForm()
        {
            return View();
        }
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult GroupUserDetail()
        {
            return View();
        }
        /// <summary>
        /// ���û�����������б�JONS
        /// </summary>
        /// <param name="CompanyId">��˾ID</param>
        /// <param name="DepartmentId">����ID</param>
        /// <param name="jqgridparam">��ҳ����</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(string CompanyId, string DepartmentId, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = base_groupuserbll.GetPageList(CompanyId, DepartmentId, ref jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// �ύ��
        /// </summary>
        /// <param name="entity">ʵ����Ϣ</param>
        /// <param name="KeyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        public override ActionResult SubmitForm(Base_GroupUser entity, string KeyValue)
        {
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                    Base_DataScopePermissionBll.Instance.AddScopeDefault(ModuleId, ManageProvider.Provider.Current().UserId, entity.GroupUserId, "4", isOpenTrans);
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
    }
}