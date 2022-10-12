using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using System.Data;

namespace EOS.WebApp.Areas.ZJPermission.Controllers
{
    public class UserSetController : PublicController<QC_ZJUSER>
    {
        QC_ZJUSERBLL bll = new QC_ZJUSERBLL();
        #region 页面返回
        public ActionResult UserSetList()
        {
            return View();
        }
        public ActionResult UserSetAdd()
        {
            return View();
        }
        public ActionResult UserSetGroup()
        {
            return View();
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// 查询质检用户列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <param name="DEF2">用户权限类型（管理员、职员）</param>
        /// <param name="DEF3">用户分组类型</param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GridZJUserList(string keywords, string DEF2, string DEF3, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QC_ZJUSER> ListData = bll.GetZJUserList(keywords, DEF2, DEF3, jqgridparam);
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
        /// 【用户管理】返回用户列表JSON
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <param name="CompanyId">公司ID</param>
        /// <param name="DepartmentId">部门ID</param>
        /// <param name="ParameterJson">高级查询条件JSON</param>
        /// <param name="jqgridparam">表格参数</param>
        /// <returns></returns>
        public ActionResult GetAllUserList(string keywords, string Enabled, string CompanyId, string DepartmentId, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = bll.GetAllUserList(keywords, Enabled, CompanyId, DepartmentId, ParameterJson, ref jqgridparam);
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
        #endregion

        #region 表单操作
        /// <summary>
        /// 用户保存
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJUserSave(string DATA)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = bll.ZJUserSave(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中用户,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 设置为管理员(1)  或 职员(2)
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJUserSetPermission(string DATA, string TYPE)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = bll.ZJUserSetPermission(DATA, TYPE);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中用户,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJUserSetDelPermission(string DATA)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = bll.ZJUserSetDelPermission(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中用户,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 质检用户分组设置
        /// </summary>
        /// <param name="DEF3">分组ID</param>
        /// <param name="DEF4">分组名称</param>
        /// <param name="DATA">用户ID</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJUserSetGroup(string DEF3, string DEF4, string DATA)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(DATA))
                {
                    if (!string.IsNullOrEmpty(DEF3))
                    {
                        message = bll.ZJUserSetGroup(DEF3, DEF4, DATA);
                        return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中分组,请重新选择" }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中用户,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

    }
}
