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
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EOS.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// ���Ź��������
    /// </summary>
    public class DepartmentController : PublicController<Base_Department>
    {
        Base_DepartmentBll base_departmentbll = new Base_DepartmentBll();

        [LoginAuthorize]
        public ActionResult DepartmentIndex()
        {
            return View();
        }
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DepartmentForm()
        {
            return View();
        }

        /// <summary>
        /// �����Ź������� ��˾������ ��JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            DataTable dt = base_departmentbll.GetTree();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (DataHelper.IsExistRows(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string DepartmentId = row["departmentid"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "parentid='" + DepartmentId + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = DepartmentId;
                    tree.text = row["fullname"].ToString();
                    tree.value = row["code"].ToString();
                    tree.parentId = row["parentid"].ToString();
                    tree.Attribute = "Type";
                    tree.AttributeValue = row["sort"].ToString();
                    tree.AttributeA = "CompanyId";
                    tree.AttributeValueA = row["companyid"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    if (tree.parentId == "0")
                    {
                        tree.img = "/Content/Images/Icon16/note.png";
                    }
                    else
                    {
                        tree.img = "/Content/Images/Icon16/document_yellow.png";
                    }
                    TreeList.Add(tree);
                }
            }
            return Content(TreeList.TreeToJson());
        }

        ///// <summary>
        ///// �����Ź������ع�˾�б�JONS
        ///// </summary>
        ///// <returns></returns>
        public ActionResult TreeGridListJson(string CompanyId, string FullName)
        {
            List<Department> ListData = base_departmentbll.GetTreeList(CompanyId, FullName);
            StringBuilder sb = new StringBuilder();
            sb.Append("{ \"rows\": [");
            sb.Append(TreeGridJson(ListData, -1));
            sb.Append("]}");
            return Content(sb.ToString());
        }

        int lft = 1, rgt = 1000000;
        public string TreeGridJson(List<Department> ListData, int index, string ParentId = "~")
        {
            StringBuilder sb = new StringBuilder();
            List<Department> ChildNodeList = ListData.FindAll(t => t.ParentId == ParentId);
            if (ChildNodeList.Count > 0) { index++; }
            foreach (Department entity in ChildNodeList)
            {
                string strJson = entity.ToJson();
                strJson = strJson.Insert(1, "\"level\":" + index + ",");
                strJson = strJson.Insert(1, "\"isLeaf\":" + (ListData.Count<Department>(t => t.ParentId == entity.DepartmentId) == 0 ? true : false).ToString().ToLower() + ",");
                strJson = strJson.Insert(1, "\"expanded\":true,");
                strJson = strJson.Insert(1, "\"lft\":" + lft++ + ",");
                strJson = strJson.Insert(1, "\"rgt\":" + rgt-- + ",");
                sb.Append(strJson);
                sb.Append(TreeGridJson(ListData, index, entity.DepartmentId));
            }
            return sb.ToString().Replace("}{", "},{");
        }

        /// <summary>
        /// �����Ź������ر��JONS
        /// </summary>
        /// <param name="CompanyId">��˾ID</param>
        /// <returns></returns>
        public ActionResult GridListJson(string CompanyId)
        {
            DataTable ListData = base_departmentbll.GetList(CompanyId);
            var JsonData = new
            {
                rows = ListData,
            };
            return Content(JsonData.ToJson());
        }
        /// <summary>
        /// �����Ź������ݹ�˾id��ȡ�����б�����JONS
        /// </summary>
        /// <param name="CompanyId">��˾Id</param>
        /// <returns></returns>
        public ActionResult DepartmentTreeJson(string CompanyId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable ListData = base_departmentbll.GetList(CompanyId);
            if (ListData.Rows.Count > 0)
            {
                sb.Append("[");
                foreach (DataRow item in ListData.Rows)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + item["departmentid"] + "\",");
                    sb.Append("\"text\":\"" + item["fullname"] + "\",");
                    sb.Append("\"value\":\"" + item["departmentid"] + "\",");
                    sb.Append("\"img\":\"/Content/Images/Icon16/chart_organisation.png\",");
                    sb.Append("\"isexpand\":true,");
                    sb.Append("\"hasChildren\":false");
                    sb.Append("},");
                }
                sb = sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
            }
            return Content(sb.ToString());
        }
        /// <summary>
        /// �����Ź��������б�JONS
        /// </summary>
        /// <param name="CompanyId">��˾ID</param>
        /// <returns></returns>
        public ActionResult ListJson(string CompanyId)
        {
            DataTable ListData = base_departmentbll.GetList(CompanyId);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// �����Ź���ɾ������
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult DeleteDepartment(string KeyValue)
        {
            try
            {
                var Message = "ɾ��ʧ�ܡ�";
                int IsOk = 0;
                int UserCount = DataFactory.Database().FindCount<Base_User>("DepartmentId", KeyValue);
                if (UserCount == 0)
                {
                    IsOk = repositoryfactory.Repository().Delete(KeyValue);
                    if (IsOk > 0)
                    {
                        Message = "ɾ���ɹ���";
                    }
                }
                else
                {
                    Message = "���������û�������ɾ����";
                }
                WriteLog(IsOk, KeyValue, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "����ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// �ύ��
        /// </summary>
        /// <param name="entity">ʵ����Ϣ</param>
        /// <param name="KeyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        public override ActionResult SubmitForm(Base_Department entity, string KeyValue)
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
                    // Base_DataScopePermissionBll.Instance.AddScopeDefault(ModuleId, ManageProvider.Provider.Current().UserId, entity.DepartmentId, "1", isOpenTrans);
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