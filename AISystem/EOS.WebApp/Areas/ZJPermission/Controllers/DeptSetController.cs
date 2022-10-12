using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using EOS.Entity;
using EOS.Business;
using EOS.Utilities;
using EOS.DataAccess;
using EOS.Repository;
using System.Data.Common;
using System.Data;


namespace EOS.WebApp.Areas.ZJPermission.Controllers
{
    public class DeptSetController : PublicController<QC_ZJDEPARTMENT>
    {
        QC_ZJDEPARTMENTBLL bll = new QC_ZJDEPARTMENTBLL();
        #region 页面返回
        public ActionResult DeptSetAdd()
        {
            return View();
        }
        public ActionResult DeptSetList()
        {
            return View();
        }
        public ActionResult DeptSetZJ()
        {
            return View();
        }
        /// <summary>
        /// 重写  编辑页面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public override ActionResult SetForm(string KeyValue)
        {
            QC_ZJDEPARTMENT entity = DataFactory.Database().FindEntity<QC_ZJDEPARTMENT>(KeyValue);
            return Content(entity.ToJson());
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// 加载部门数据
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <param name="TYPE">0 全部部门   1质检部门</param>
        /// <returns></returns>
        public ActionResult LoadDeptData(string CompanyId, string TYPE)
        {
            List<Department> ListData = bll.GetTreeList(CompanyId, TYPE);
            StringBuilder sb = new StringBuilder();
            sb.Append("{ \"rows\": [");
            sb.Append(TreeGridJson(ListData, -1));
            sb.Append("]}");
            return Content(sb.ToString());
        }

        /// <summary>
        /// 加载部门树形结构
        /// </summary>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public ActionResult LoadDeptTreeList(string CompanyId)
        {
            DataTable dt = bll.GetTreeListDept(CompanyId);
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (DataHelper.IsExistRows(dt))
            {
                foreach (DataRow item in dt.Rows)
                {
                    string ID = item["DEPARTMENTID"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "PARENTID = '" + ID + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = item["DEPARTMENTID"].ToString();
                    tree.text = item["FULLNAME"].ToString();
                    tree.value = item["DEPARTMENTID"].ToString();
                    tree.checkstate = item["DEF1"].ToString() == "1" ? 1 : 0;
                    tree.showcheck = true;
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.parentId = item["PARENTID"].ToString();
                    //tree.img = item["icon"].ToString() != null ? "/Content/Images/Icon16/" + item["icon"].ToString() : item["icon"].ToString();
                    TreeList.Add(tree);
                }
            }
            return Content(TreeToJsonList(TreeList));
        }

        #endregion

        #region 数据操作
        /// <summary>
        /// 保存部门
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public override ActionResult SubmitForm(QC_ZJDEPARTMENT entity, string KeyValue)
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

        public ActionResult GetParent(string CompanyId, string ParentId)
        {
            QC_ZJDEPARTMENT model = DataFactory.Database().FindEntityByWhere<QC_ZJDEPARTMENT>(" AND DEPARTMENTID='" + ParentId + "' AND COMPANYID='" + CompanyId + "'");
            return Content(model.ToJson());
        }

        public ActionResult DeleteItem(string KeyValue)
        {
            try
            {
                ResultMessage message = bll.DeleteItem(KeyValue);

                WriteLog(message.Code, KeyValue, message.Message);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 保存质检部门
        /// </summary>
        /// <param name="MID"></param>
        /// <param name="CompanyId"></param>
        /// <returns></returns>
        public ActionResult DeptZJSubmit(string MID, string CompanyId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                ResultMessage message = new ResultMessage();
                if (string.IsNullOrEmpty(CompanyId))
                {
                    message.Code = -1;
                    message.Content = "请选择公司";
                }

                string strwhere = MID.Trim(',').Replace(",", "','");
                StringBuilder strsql = new StringBuilder();
                strsql.AppendFormat("UPDATE QC_ZJDEPARTMENT SET DEF1=0 WHERE COMPANYID='{0}'", CompanyId);
                database.ExecuteBySql(strsql, isOpenTrans);

                strsql.Clear();
                strsql.AppendFormat("UPDATE QC_ZJDEPARTMENT SET DEF1=1 WHERE DEPARTMENTID IN ('{0}') AND COMPANYID='{1}'", strwhere, CompanyId);
                database.ExecuteBySql(strsql, isOpenTrans);

                strsql.Clear();
                strsql.AppendFormat("UPDATE QC_ZJDEPARTMENTMATERIAL SET DEF1=1 WHERE DEPARTMENT IN ('{0}') AND DEPARTMENT IN (SELECT DEPARTMENTID FROM QC_ZJDEPARTMENT WHERE COMPANYID='{1}')", strwhere, CompanyId);
                database.ExecuteBySql(strsql, isOpenTrans);

                strsql.Clear();
                strsql.AppendFormat("UPDATE QC_ZJDEPARTMENTMATERIAL SET DEF1=0 WHERE DEPARTMENT NOT IN ('{0}') AND DEPARTMENT IN (SELECT DEPARTMENTID FROM QC_ZJDEPARTMENT WHERE COMPANYID='{1}')", strwhere, CompanyId);
                database.ExecuteBySql(strsql, isOpenTrans);

                message.Code = 1;
                message.Content = "操作成功";

                database.Commit();
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

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
        /// 转换树形Json
        /// </summary>
        /// <returns></returns>
        public string TreeToJsonList(List<TreeJsonEntity> list, string ParentId = "~")
        {
            StringBuilder strJson = new StringBuilder();
            List<TreeJsonEntity> item = DataHelper.IListToList<TreeJsonEntity>(list).FindAll(t => t.parentId == ParentId);
            strJson.Append("[");
            if (item.Count > 0)
            {
                foreach (TreeJsonEntity entity in item)
                {
                    strJson.Append("{");
                    strJson.Append("\"id\":\"" + entity.id + "\",");
                    strJson.Append("\"text\":\"" + entity.text.Replace("&nbsp;", "") + "\",");
                    strJson.Append("\"value\":\"" + entity.value + "\",");
                    if (entity.Attribute != null && !string.IsNullOrEmpty(entity.Attribute))
                    {
                        strJson.Append("\"" + entity.Attribute + "\":\"" + entity.AttributeValue + "\",");
                    }
                    if (entity.AttributeA != null && !string.IsNullOrEmpty(entity.AttributeA))
                    {
                        strJson.Append("\"" + entity.AttributeA + "\":\"" + entity.AttributeValueA + "\",");
                    }
                    if (entity.title != null && !string.IsNullOrEmpty(entity.title.Replace("&nbsp;", "")))
                    {
                        strJson.Append("\"title\":\"" + entity.title.Replace("&nbsp;", "") + "\",");
                    }
                    if (entity.img != null && !string.IsNullOrEmpty(entity.img.Replace("&nbsp;", "")))
                    {
                        strJson.Append("\"img\":\"" + entity.img.Replace("&nbsp;", "") + "\",");
                    }
                    if (entity.checkstate != null)
                    {
                        strJson.Append("\"checkstate\":" + entity.checkstate + ",");
                    }
                    if (entity.parentId != null)
                    {
                        strJson.Append("\"parentnodes\":\"" + entity.parentId + "\",");
                    }
                    strJson.Append("\"showcheck\":" + entity.showcheck.ToString().ToLower() + ",");
                    strJson.Append("\"isexpand\":" + entity.isexpand.ToString().ToLower() + ",");
                    if (entity.complete == true)
                    {
                        strJson.Append("\"complete\":" + entity.complete.ToString().ToLower() + ",");
                    }
                    strJson.Append("\"hasChildren\":" + entity.hasChildren.ToString().ToLower() + ",");
                    strJson.Append("\"ChildNodes\":" + TreeToJsonList(list, entity.id) + "");
                    strJson.Append("},");
                }
                strJson = strJson.Remove(strJson.Length - 1, 1);
            }
            strJson.Append("]");
            return strJson.ToString();
        }

    }
}
