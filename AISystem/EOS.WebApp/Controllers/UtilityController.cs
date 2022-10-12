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

namespace EOS.WebApp.Controllers
{
    /// <summary>
    /// 公共控制器
    /// </summary>
    public class UtilityController : Controller
    {
        #region 工具栏/右击栏 按钮
        /// <summary>
        /// 加载工具栏/右击栏 按钮
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadButton(string ModuleId)
        {
            Base_ButtonPermissionBll base_buttonpermissionbll = new Base_ButtonPermissionBll();
            string ObjectId = ManageProvider.Provider.Current().ObjectId;
            if (string.IsNullOrEmpty(ModuleId))
            {
                ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            }
              
            List<Base_Button> list = base_buttonpermissionbll.GetButtonList(ObjectId, ModuleId).FindAll(t => t.Enabled == 1);
            return Content(list.ToJson().Replace("&nbsp;", ""));
        }
        #endregion

        #region 表格视图列名
        /// <summary>
        /// 表格视图列名
        /// </summary>
        /// <returns></returns>
        public ActionResult LoadViewColumn()
        {
            Base_ViewPermissionBll base_viewpermissionbll = new Base_ViewPermissionBll();
            string ObjectId = ManageProvider.Provider.Current().ObjectId;
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            List<Base_View> list = base_viewpermissionbll.GetViewList(ObjectId, ModuleId).FindAll(t => t.Enabled == 1);
            return Content(list.ToJson());
        }
        /// <summary>
        /// 自定义格式化
        /// </summary>
        /// <param name="CustomSwitch"></param>
        /// <returns></returns>
        public string ToFormatter(string CustomSwitch)
        {
            StringBuilder str = new StringBuilder();
            str.Append("function (cellvalue, options, rowObject) {");
            Hashtable ht = HashtableHelper.JsonToHashtable(CustomSwitch);
            if (ht != null && ht.Count > 0)
            {
                foreach (string key in ht.Keys)
                {
                    str.Append("if (cellvalue == '" + key + "')");
                    str.Append("    return \"" + ht[key] + "\";");
                }
            }
            else
            {
                str.Append("return \"<img src='../../Content/Images/\" + cellvalue + \"'/>\";");
            }
            str.Append("}");
            return str.ToString();
        }
        #endregion

        #region 公共查询
        public Base_QueryRecordBll base_queryrecordbll = new Base_QueryRecordBll();
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        public ActionResult QueryPage()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】方案列表
        /// </summary>
        /// <param name="ModuleId">模块ID</param>
        /// <returns></returns>
        public ActionResult QueryListProject(string ModuleId)
        {
            string CreateUserId = ManageProvider.Provider.Current().UserId;
            List<Base_QueryRecord> list = base_queryrecordbll.GetList(ModuleId, CreateUserId);
            string JsonData = list.ToJson().Replace("ParamName", "FieldName").Replace("Operation", "CompareValue").Replace("ParamValue", "FilterValue");
            return Content(JsonData);
        }
        /// <summary>
        /// 【查询页面】删除方案
        /// </summary>
        /// <param name="QueryRecordId"></param>
        /// <returns></returns>
        public ActionResult QueryDeleteProject(string QueryRecordId)
        {
            try
            {
                var Message = "删除失败。";
                int IsOk = DataFactory.Database().Delete<Base_QueryRecord>(QueryRecordId);
                if (IsOk > 0)
                {
                    Message = "删除成功。";
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 【查询页面】保存方案
        /// </summary>
        /// <param name="QueryRecordId">主键</param>
        /// <param name="entity">对象实体</param>
        /// <returns></returns>
        public ActionResult QuerySaveProject(string QueryRecordId, Base_QueryRecord entity)
        {
            try
            {
                int IsOk = 0;
                string Message = QueryRecordId == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(QueryRecordId))
                {
                    entity.Modify(QueryRecordId);
                    IsOk = DataFactory.Database().Update(entity);
                }
                else
                {
                    entity.Create();
                    IsOk = DataFactory.Database().Insert(entity);
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 【查询页面】设置初始化默认方案
        /// </summary>
        /// <param name="ModuleId">模块ID</param>
        /// <param name="QueryRecordId">主键</param>
        /// <returns></returns>
        public ActionResult QueryDefaultProject(string ModuleId, string QueryRecordId)
        {
            try
            {
                int IsOk = base_queryrecordbll.DefaultProject(ModuleId, QueryRecordId);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 省、市、区 列表
        Base_ProvinceCityBll base_provincecitybll = new Base_ProvinceCityBll();
        /// <summary>
        /// 获取省、市、区 列表
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns></returns>
        public ActionResult GetProvinceCityListJson(string ParentId)
        {
            List<Base_ProvinceCity> ListData = base_provincecitybll.GetList(ParentId);
            return Content(ListData.ToJson());
        }
        #endregion

        #region 验证对象值不能重复
        /// <summary>
        /// 验证对象值不能重复
        /// </summary>
        /// <param name="tablename">实体类</param>
        /// <param name="fieldname">属性字段</param>
        /// <param name="fieldvalue">属性字段值</param>
        /// <param name="keyfield">主键字段</param>
        /// <param name="keyvalue">主键字段值</param>
        /// <returns></returns>
        public ActionResult FieldExist(string tablename, string fieldname, string fieldvalue, string keyfield, string keyvalue)
        {
            bool IsOk = BaseFactory.BaseHelper().FieldExist(tablename, fieldname, fieldvalue, keyfield, keyvalue);
            return Content(IsOk.ToString().ToLower());
        }
        #endregion

        #region JqGrid导出Excel
        /// <summary>
        /// 获取要导出表头字段
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDeriveExcelColumn()
        {
            string JsonColumn = GZipHelper.Uncompress(CookieHelper.GetCookie("JsonColumn_DeriveExcel"));
            return Content(JsonColumn);
        }
        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="ExportField">要导出字段</param>
        public void GetDeriveExcel(string ExportField)
        {
            string JsonColumn = GZipHelper.Uncompress(CookieHelper.GetCookie("JsonColumn_DeriveExcel"));
            string JsonData = GZipHelper.Uncompress(CookieHelper.GetCookie("JsonData_DeriveExcel"));
            string JsonFooter = GZipHelper.Uncompress(CookieHelper.GetCookie("JsonFooter_DeriveExcel"));
            string fileName = GZipHelper.Uncompress(CookieHelper.GetCookie("FileName_DeriveExcel"));
            DeriveExcel.JqGridToExcel(JsonColumn, JsonData, ExportField, fileName);


            //CookieHelper.DelCookie("JsonColumn_DeriveExcel");
            //CookieHelper.DelCookie("JsonData_DeriveExcel");
            //CookieHelper.DelCookie("JsonFooter_DeriveExcel");
            //CookieHelper.DelCookie("FileName_DeriveExcel");
        }
        /// <summary>
        /// 写入数据到Cookie
        /// </summary>
        /// <param name="JsonColumn">表头</param>
        /// <param name="JsonData">数据</param>
        /// <param name="JsonFooter">底部合计</param>
        [ValidateInput(false)]
        public void SetDeriveExcel(string JsonColumn, string JsonData, string JsonFooter, string FileName)
        {
            CookieHelper.WriteCookie("JsonColumn_DeriveExcel", GZipHelper.Compress(JsonColumn));
            CookieHelper.WriteCookie("JsonData_DeriveExcel", GZipHelper.Compress(JsonData));
            CookieHelper.WriteCookie("JsonFooter_DeriveExcel", GZipHelper.Compress(JsonFooter));
            CookieHelper.WriteCookie("FileName_DeriveExcel", GZipHelper.Compress(FileName));
        }
        #endregion

        #region 选择用户
        Base_UserBll base_userbll = new Base_UserBll();
        /// <summary>
        /// 选择用户
        /// </summary>
        /// <returns></returns>
        public ActionResult OptionUser()
        {
            return View();
        }
        /// <summary>
        ///用户列表JSON
        /// </summary>
        /// <param name="keyword">模糊查询</param>
        /// <returns></returns>
        public ActionResult OptionUserJson(string keyword)
        {
            DataTable ListData = base_userbll.OptionUserList(keyword);
            StringBuilder sb = new StringBuilder();
            sb.Append("[");
            if (DataHelper.IsExistRows(ListData))
            {
                foreach (DataRow item in ListData.Rows)
                {
                    sb.Append("{");
                    sb.Append("\"id\":\"" + item["userid"] + "\",");
                    sb.Append("\"text\":\"" + item["realname"] + "（" + item["account"] + "）\",");
                    sb.Append("\"account\":\"" + item["account"] + "\",");
                    sb.Append("\"code\":\"" + item["code"] + "\",");
                    sb.Append("\"realname\":\"" + item["realname"] + "\",");
                    string Genderimg = "user_female.png";
                    if (item["Gender"].ToString() == "男")
                    {
                        Genderimg = "user_green.png";
                    }
                    sb.Append("\"img\":\"/Content/Images/Icon16/" + Genderimg + "\",");
                    sb.Append("\"isexpand\":true,");
                    sb.Append("\"hasChildren\":false");
                    sb.Append("},");
                }
                sb = sb.Remove(sb.Length - 1, 1);
            }
            sb.Append("]");
            return Content(sb.ToString());
        }
        #endregion

        #region 生成打印
        /// <summary>
        /// 打印当前页
        /// </summary>
        /// <returns></returns>
        public ActionResult PrintPage()
        {
            return View();
        }
        #endregion

        #region 工作流处理
        //WF_ExcuteBll wf_excutebll = new WF_ExcuteBll();
        /// <summary>
        /// 审核处理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditIndex(string frmTitle)
        {
            return View();
        }
        /// <summary>
        /// 审核处理记录
        /// </summary>
        /// <param name="TaskId">任务Id</param>
        /// <returns></returns>
        //public ActionResult GetAuditRecordListJson(string TaskId)
        //{
        //    var JsonData = new
        //    {
        //       rows = wf_excutebll.GetAuditRecordList(TaskId),
        //    };
        //    return Content(JsonData.ToJson());
        //}
        /// <summary>
        /// 审核节点步骤
        /// </summary>
        /// <param name="TaskId">任务Id</param>
        /// <returns></returns>
        //public ActionResult GetAuditNodeJson(string TaskId)
        //{
        //    List<string[]> list = wf_excutebll.GetExcuteNode(TaskId);
        //    return Content(list.ToJson());
        //}
        /// 执行工作流
        /// </summary>
        /// <param name="taskId">任务ID</param>
        /// <param name="buttonId">按钮ID</param>
        /// <param name="executyType">是否通过（3-不通过，1,2,4-通过、-1结束流程）</param>
        /// <param name="HandSuggest">处理意见</param>
        /// <returns></returns>
        //        public ActionResult ExcuteFlow(string taskId, string buttonId, int executyType, string HandSuggest, string flowTitle)
        //        {
        //            WF_ExcuteBll wf_excutebll = new WF_ExcuteBll();
        //            IDatabase database = DataFactory.Database();
        //            //根据buttonId获得节点ID和操作类型(已作废的流程要筛选掉)
        //            WF_NodeButtonRelation wf_nodebuttonrelation = database.FindEntityBySql<WF_NodeButtonRelation>(string.Format(@"SELECT  b.*
        //                                                                                                                            FROM    WF_NodeButtonRelation b
        //                                                                                                                                    INNER JOIN WF_FlowNode n ON b.FlowNodeId = n.FlowNodeId
        //                                                                                                                                    INNER JOIN WF_FlowMain f ON n.FlowMainId = f.FlowMainId
        //                                                                                                                                    WHERE f.Enabled=1 AND f.DeleteMark=0 AND b.ButtonId='{0}'", buttonId));            //得到当前用户的ID
        //            string userId = ManageProvider.Provider.Current().UserId;
        //            string[] result = wf_excutebll.Excute(taskId, wf_nodebuttonrelation.FlowNodeId, executyType, HandSuggest, userId, flowTitle);
        //            return Content(result.ToJson());
        //        }
        #endregion

        /// <summary>
        /// 判断内容是否已存在
        /// </summary>
        /// <param name="table"></param>
        /// <param name="key"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public ActionResult IsExists(string table, string key, string field, string value, string id)
        {
            String strJson = "";
            bool bol = false;
            int i = 0;
            if (table != null)
            {
                try
                {
                    if (!String.IsNullOrEmpty(id) && id != "undefined")
                    {
                        i = DataFactory.Database().FindCountBySql(String.Format("select count(1) from {0} where {1}='{2}' and {3}!='{4}'", table, field, value, key, id));
                        bol = i > 0 ? true : false;
                    }
                    else
                    {
                        i = DataFactory.Database().FindCountBySql(String.Format("select count(1) from {0} where {1}='{2}' ", table, field, value));
                        bol = i > 0 ? true : false;
                    }
                    strJson = bol == true ? "false" : "true";
                }
                catch (Exception ex)
                {
                    strJson = ex.Message;
                }
            }
            return Content(strJson);
        }

        /// <summary>
        /// 返回拼音码
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public String GetPY(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                return PinyinHelper.PinyinString(str);
            }
            else
            {
                return "";
            }

        }



        #region 公共选择弹出页
        //物料大类
        [LoginAuthorize]
        public ActionResult SelectMaterialType()
        {
            return View();
        }
        //物料
        [LoginAuthorize]
        public ActionResult SelectMaterialMore()
        {
            return View();
        }
        //物料
        [LoginAuthorize]
        public ActionResult SelectMaterial()
        {
            return View();
        }
        //部门
        [LoginAuthorize]
        public ActionResult SelectDept()
        {
            return View();
        }
        #endregion
    }
}
