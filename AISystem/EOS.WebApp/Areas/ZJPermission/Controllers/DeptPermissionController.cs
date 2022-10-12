using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using System.Data;
using EOS.Utilities;
using EOS.Business;
using System.Text;
using System.Collections;

namespace EOS.WebApp.Areas.ZJPermission.Controllers
{
    public class DeptPermissionController : PublicController<QC_ZJDEPARTMENTMATERIAL>
    {
        QC_ZJDEPARTMENTMATERIALBLL bll = new QC_ZJDEPARTMENTMATERIALBLL();
        #region 页面返回
        public ActionResult DeptPermissionList()
        {
            return View();
        }
        public ActionResult DeptPermissionSet()
        {
            return View();
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// 物料列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult ModuleTree(string KeyValue)
        {
            DataTable dt = bll.GetList(KeyValue);
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (DataHelper.IsExistRows(dt))
            {
                foreach (DataRow item in dt.Rows)
                {
                    string ID = item["ID"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(dt, "PK_PARENT = '" + ID + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = item["ID"].ToString();
                    tree.text = item["NAME"].ToString();
                    tree.value = item["ID"].ToString();
                    tree.checkstate = item["DEPARTMENT"].ToString() != "" ? 1 : 0;
                    tree.showcheck = true;
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.parentId = item["PK_PARENT"].ToString();
                    //tree.img = item["icon"].ToString() != null ? "/Content/Images/Icon16/" + item["icon"].ToString() : item["icon"].ToString();
                    TreeList.Add(tree);
                }
            }
            return Content(TreeToJsonList(TreeList));
        }
         
        #endregion

        #region 表单操作
        public ActionResult AuthorizedSubmit(string MID, string KeyValue)
        {
            try
            {
                ResultMessage message = bll.AuthorizedSubmit(MID, KeyValue);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

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
