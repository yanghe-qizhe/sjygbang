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
using System.Text;
using System.Data;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class MaterialController : PublicController<BD_CMATERIAL>
    {
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        BD_MaterialBll bd_bll = new BD_MaterialBll();
        VBD_MATERIALForGBMATERIALBll vbd_bll = new VBD_MATERIALForGBMATERIALBll();

        /// <summary>
        /// 获取自动单据内码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }
        #region 分类管理
        /// <summary>
        /// 分类管理视图
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Ignore)]
        public ActionResult SortManage()
        {
            ViewBag.SortCode = BaseFactory.BaseHelper().GetSortCode<Base_DataDictionary>("SortCode").ToString();
            return View();
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSortManage(string KeyValue)
        {
            try
            {
                var Message = "删除失败。";
                int IsOk = 0;
                if (DataFactory.Database().FindCount<BD_MARBASCLASS>("PK_PARENT", KeyValue) == 0)
                {
                    IsOk = DataFactory.Database().Delete<BD_MARBASCLASS>(KeyValue);
                    if (IsOk > 0)
                    {
                        Message = string.Format("成功删除 {0} 条。", 1);
                    }
                }
                else
                {
                    throw new Exception("当前所选有子节点数据，不能删除。");
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "错误：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 表单赋值
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetFormSortManage(string KeyValue)
        {
            BD_MARBASCLASS entity = DataFactory.Database().FindEntity<BD_MARBASCLASS>("INNERCODE", KeyValue);
            string JsonData = entity.ToJson();

            string vname = entity.PK_PARENT;
            string vnewname = "";
            if (vname == "~")
            {
                vnewname = "物料基本分类";
            }

            JsonData = JsonData.Insert(1, "\"ParentName\":\"" + DataFactory.Database().FindEntity<BD_MARBASCLASS>("ID", vname).NAME + vnewname + "\",");
            return Content(JsonData);
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体类</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitFormSortManage(BD_MARBASCLASS entity, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    IsOk = DataFactory.Database().Update(entity);
                }
                else
                {
                    entity.Create();
                    entity.ID = "111" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    entity.DEF1 = "PT";
                    //entity.ISUSE = "1";
                    entity.INNERCODE = entity.ID;
                    entity.PK_MARBASCLASS = entity.ID;
                    string SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
                    entity.SPELLCODE = SPELLCODE;
                    if (string.IsNullOrEmpty(entity.ISUSE))
                    {
                        entity.ISUSE = "0";
                    }
                    //if (entity.PK_PARENT == "~")
                    //{
                    //    entity.PK_PARENT = "222" + DateTime.Now.ToString("yyyyMMddHHmmss");
                    //}

                    IsOk = DataFactory.Database().Insert(entity);
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = KeyValue == "" ? "新增成功。" : "编辑成功。" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }
        #endregion
        public ActionResult TreeJson(string type)
        {
            DataTable dt = bd_bll.TreeList();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            TreeJsonEntity root = new TreeJsonEntity();
            root.id = "~";
            root.text = "物料基本分类";
            root.value = "01";
            root.parentId = "0";
            root.isexpand = true;
            root.complete = true;
            root.hasChildren = true;
            root.img = "/Content/Images/Icon16/note.png";
            TreeList.Add(root);
            if (DataHelper.IsExistRows(dt))
            {
                foreach (DataRow row in dt.Rows)
                {
                    string id = row["INNERCODE"].ToString();
                    string level = row["DATAORIGINFLAG"].ToString();
                    bool hasChildren = false;

                    DataTable childnode = DataHelper.GetNewDataTable(dt, "pk_parent='" + id + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }

                    TreeJsonEntity tree = new TreeJsonEntity();
                    tree.id = id;
                    tree.text = row["name"].ToString();
                    tree.value = row["id"].ToString();
                    tree.parentId = row["pk_parent"].ToString();
                    tree.isexpand = (level == "1" ? true : false);
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    tree.img = "/Content/Images/Icon16/document_yellow.png";
                    TreeList.Add(tree);
                }
            }
            return Content(TreeList.TreeToJson("0"));
        }


        ///// <summary>
        ///// 【部门管理】返回公司列表JONS
        ///// </summary>
        ///// <returns></returns>
        public ActionResult TreeGridListJson(string keyword)
        {
            List<MaterialType> ListData = bd_bll.GetTreeList(keyword);
            ListData = ListData.OrderBy(c => c.Code).ToList();

            StringBuilder sb = new StringBuilder();
            if (string.IsNullOrEmpty(keyword))
            {
                sb.Append("{ \"rows\": [");
                sb.Append(TreeGridJson(ListData, -1));
                sb.Append("]}");
            }
            else
            {
                sb.Append("{ \"rows\": [");
                sb.Append(TreeGridJson(ListData, -1, ""));
                sb.Append("]}");
            }
            return Content(sb.ToString());
        }

        int lft = 1, rgt = 1000000;
        public string TreeGridJson(List<MaterialType> ListData, int index, string ParentId = "~")
        {
            StringBuilder sb = new StringBuilder();
            List<MaterialType> ChildNodeList = null;
            if (!string.IsNullOrEmpty(ParentId))
            {
                ChildNodeList = ListData.FindAll(t => t.PK_PARENT == ParentId);
            }
            else
            {
                ChildNodeList = ListData;
            }

            if (ChildNodeList.Count > 0) { index++; }
            if (index > 2)
            {
            }
            else
            {
                foreach (MaterialType entity in ChildNodeList)
                {
                    string strJson = entity.ToJson();
                    strJson = strJson.Insert(1, "\"level\":" + index + ",");
                    strJson = strJson.Insert(1, "\"isLeaf\":" + (ListData.Count<MaterialType>(t => t.PK_PARENT == entity.Id) == 0 && index <= 2 ? true : false).ToString().ToLower() + ",");
                    strJson = strJson.Insert(1, "\"expanded\":true,");
                    strJson = strJson.Insert(1, "\"lft\":" + lft++ + ",");
                    strJson = strJson.Insert(1, "\"rgt\":" + rgt-- + ",");
                    sb.Append(strJson);
                    sb.Append(TreeGridJson(ListData, index, entity.Id));
                }
            }
            return sb.ToString().Replace("}{", "},{");
        }


        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MaterialForm()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.Code = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        [LoginAuthorize]
        public ActionResult MaterialIndex()
        {
            return View();
        }
        //产品标准管理
        [LoginAuthorize]
        public ActionResult BZMANAGERIndex()
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
                database.Delete("BD_CMATERIAL", "ID", KeyValue);
                StringBuilder strSql = new StringBuilder();
                strSql.Append(string.Format("delete BD_MATERIAL where id='{0}'", KeyValue));
                DataFactory.Database().ExecuteBySql(strSql, isOpenTrans);
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
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public override ActionResult SubmitForm(BD_CMATERIAL entity, string KeyValue)
        {
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string sql = String.Format("select count(1) from BD_CMATERIAL where ID='{0}'", KeyValue);
                int res = DataFactory.Database().FindCountBySql(sql);
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (res > 0)
                {
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                }
                StringBuilder strSql = new StringBuilder();
                strSql.Append(string.Format("update BD_MATERIAL set spellcode='{0}',isjl='{1}' where id='{2}'", entity.SPELLCODE, entity.ISJL, KeyValue));
                DataFactory.Database().ExecuteBySql(strSql, isOpenTrans);
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
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm(BD_CMATERIAL entity, string KeyValue, string ModuleId, string YJDD, string SourceareaType)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                string sql = "";
                int res = 0;
                StringBuilder strSql = new StringBuilder();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from BD_CMATERIAL where ID='{0}'", KeyValue);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        entity.Modify(KeyValue);
                        database.Update(entity, isOpenTrans);
                    }
                    else
                    {
                        entity.Create();
                        database.Insert(entity, isOpenTrans);
                    }
                    //新增验级地点与物料挂钩逻辑 将验级地点更新至BD_MATERIAL表DEF3字段 220831 sxy/myt   新增货源地类型与物料挂钩逻辑 将货源地类型更新至BD_MATERIAL表DEF4字段 220908
                    strSql.Append(string.Format("update BD_MATERIAL set spellcode='{0}',isjl='{1}',isuse='{2}',MATERIALSPEC='{3}',NAME='{4}',SHORTNAME='{4}',PK_MARBASCLASS='{5}',PK_GROUP='{6}',MATERIALBARCODE='{7}',ISDEL={8},DEF3='{9}',DEF4='{10}' where id='{11}'", entity.SPELLCODE, entity.ISJL, entity.ISUSE, entity.MATERIALSPEC, entity.NAME, entity.PK_MARBASCLASS, entity.PK_GROUP, entity.MATERIALBARCODE, entity.ISDEL,YJDD, SourceareaType, KeyValue));
                    DataFactory.Database().ExecuteBySql(strSql, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from BD_MATERIAL where  CODE='{0}'", entity.CODE);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.CODE = this.BillCode();
                    }
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                    string SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
                    //新增验级地点与物料挂钩逻辑 将验级地点更新至BD_MATERIAL表DEF3字段 220831 sxy/myt  新增货源地类型与物料挂钩逻辑 将货源地类型更新至BD_MATERIAL表DEF4字段 220908
                    strSql.Append(string.Format("insert into BD_MATERIAL(ID,CODE,NAME,SHORTNAME,SPELLCODE,PK_MARBASCLASS,ISJL,ISUSE,MATERIALSPEC,PK_GROUP,MATERIALBARCODE,ISDEL,DEF3,DEF4) values('{0}','{0}','{1}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}',{9},'{10}','{11}')", entity.CODE, entity.NAME, SPELLCODE, entity.PK_MARBASCLASS, entity.ISJL, entity.ISUSE, entity.MATERIALSPEC, entity.PK_GROUP, entity.MATERIALBARCODE, entity.ISDEL, YJDD, SourceareaType));
                    DataFactory.Database().ExecuteBySql(strSql, isOpenTrans);
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
        /// 列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetMaterialList(string keyword, string matclass, string istype, string datatype, string isuse, string MATERIALBARCODE, string PK_GROUP, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_MATERIAL> ListData = bd_bll.GetOrderList(keyword, istype, datatype, isuse, MATERIALBARCODE, PK_GROUP, "", "", jqgridparam, matclass);
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
        /// 列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetMaterialListForGBMATERIAL(string keyword, JqGridParam jqgridparam, string matclass)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_MATERIALForGBMATERIAL> ListData = vbd_bll.GetOrderListForGBMATERIAL(keyword, "", "", jqgridparam, matclass);
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


        [LoginAuthorize]
        public ActionResult GetJLMaterialList(string keyword, JqGridParam jqgridparam, string matclass)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_MATERIAL> ListData = bd_bll.GetJLOrderList(keyword, "", "", jqgridparam, matclass);
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
        /// 列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetMaterialListForZJ(string keyword, JqGridParam jqgridparam, string matclass)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_MATERIAL> ListData = bd_bll.GetOrderListForZJ(keyword, "", "", jqgridparam, matclass);
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

        [LoginAuthorize]
        public ActionResult HandOrder(string id, string MATERIALBARCODE, string PK_GROUP)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient("SAPWebServiceSoap");
                WEBSERVICES_SAP.RETURN_SAP result = new WEBSERVICES_SAP.RETURN_SAP();
                result = sap.SAP_117ZS1(id, "", PK_GROUP, "", MATERIALBARCODE, "", "", "");
                JsonMessage jsonmeg = null;
                if (result.STATUS == "S")
                {
                    string Message = "同步成功。";
                    jsonmeg = new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}:{1}", Message, result.REMSG) };
                }
                else
                {
                    jsonmeg = new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", result.REMSG) };
                }

                return Content(jsonmeg.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }
        /// <summary>
        /// 【车辆管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            VBD_MATERIAL entity = bd_bll.GetEntity(KeyValue);// DataFactory.Database().FindEntity<VBD_MATERIAL>(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
