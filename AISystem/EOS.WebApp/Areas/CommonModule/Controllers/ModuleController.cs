using EOS.Business;
using EOS.Entity;
using EOS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EOS.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// 模块管理控制器
    /// </summary>
    public class ModuleController : PublicController<Base_Module>
    {
        public Base_ModuleBll base_modulebll = new Base_ModuleBll();
        public Base_ModuleScjBll base_modulescjbll = new Base_ModuleScjBll();
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ModuleIndex()
        {
            return View();
        }
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ModuleForm()
        {
            return View();
        }
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ModuleIndex1()
        {
            return View();
        }
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult ModuleForm1()
        {
            return View();
        }
        /// <summary>
        /// 【模块管理】返回树JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson()
        {
            List<Base_Module> list = base_modulebll.GetList();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            foreach (Base_Module item in list)
            {
                TreeJsonEntity tree = new TreeJsonEntity();
                bool hasChildren = false;
                List<Base_Module> childnode = list.FindAll(t => t.ParentId == item.ModuleId);
                if (childnode.Count > 0)
                {
                    hasChildren = true;
                }
                tree.id = item.ModuleId;
                tree.text = item.FullName;
                tree.value = item.ModuleId;
                tree.isexpand = item.Isexpand == 1 ? true : false;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.img = item.Icon != null ? "/Content/Images/Icon16/" + item.Icon : item.Icon;
                TreeList.Add(tree);
            }
            return Content(TreeList.TreeToJson());
        }


        /// <summary>
        /// 【模块管理】返回树JONS
        /// </summary>
        /// <returns></returns>
        public ActionResult TreeJson1()
        {
            List<Base_ModuleScj> list = base_modulescjbll.GetList();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            foreach (Base_ModuleScj item in list)
            {
                TreeJsonEntity tree = new TreeJsonEntity();
                bool hasChildren = false;
                List<Base_ModuleScj> childnode = list.FindAll(t => t.ParentId == item.ModuleId);
                if (childnode.Count > 0)
                {
                    hasChildren = true;
                }
                tree.id = item.ModuleId;
                tree.text = item.FullName;
                tree.value = item.ModuleId;
                tree.isexpand = item.Isexpand == 1 ? true : false;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                tree.parentId = item.ParentId;
                tree.img = item.Icon != null ? "/Content/Images/Icon16/" + item.Icon : item.Icon;
                TreeList.Add(tree);
            }
            return Content(TreeList.TreeToJson());
        }
        /// <summary>
        /// 【模块管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            Base_Module entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            JsonData = JsonData.Insert(1, "\"ParentName\":\"" + base_modulebll.Repository().FindEntity(entity.ParentId).FullName + "\",");
            return Content(JsonData);
        }

        /// <summary>
        /// 【模块管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl1(string KeyValue)
        {
            Base_ModuleScj entity = base_modulescjbll.SetFormControl(KeyValue);
            string JsonData = entity.ToJson();
            JsonData = JsonData.Insert(1, "\"ParentName\":\"" + base_modulebll.Repository().FindEntity(entity.ParentId).FullName + "\",");
            return Content(JsonData);
        }
        
    }
}
