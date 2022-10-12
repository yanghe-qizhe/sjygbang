using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;

namespace EOS.WebApp.Areas.ZJPermission.Controllers
{
    public class UserDeptController : PublicController<QC_ZJUSERDEPARTMENT>
    {
        QC_ZJUSERDEPARTMENTBLL bll = new QC_ZJUSERDEPARTMENTBLL();
        #region 页面返回
        public ActionResult UserDeptList()
        {
            return View();
        }
        public ActionResult UserDeptAdd()
        {
            return View();
        }
        public ActionResult UserDeptSetTime()
        {
            return View();
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// 获取指定部门下的用户
        /// </summary>
        /// <param name="DEPTID"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult LoadDeptUser(string DEPTID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QC_ZJUSERDEPARTMENT> ListData = bll.GetZJUserListForDept(DEPTID, jqgridparam);
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
        /// 获取质检用户，未被分配到指定部门的
        /// </summary>
        /// <param name="keywords"></param>
        ///  <param name="DEF2">质检用户权限类型</param>
        ///  <param name="DEF3">质检用户分组</param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GridZJUserListNoDept(string keywords, string DEF2, string DEF3, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QC_ZJUSER> ListData = bll.GridZJUserListNoDept(keywords, DEF2, DEF3, jqgridparam);
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
        /// 保存用户部门绑定关系
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJUserDeptSave(string DATA, string REALNAMES, string Dept)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = bll.ZJUserDeptSave(DATA, REALNAMES, Dept);
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
        /// 将用户移出当前部门
        /// </summary>
        /// <param name="DATA"></param>
        /// <param name="Dept"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJUserDeptDel(string DATA, string Dept)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = bll.ZJUserDeptDel(DATA, Dept);
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
        /// 设置用户截止时间点
        /// </summary>
        /// <param name="ETIME"></param>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJUserDeptSetTime(string DATA, string ETIME, string DEPT)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(DATA))
                {
                    DateTime etime = new DateTime();
                    if (DateTime.TryParse(ETIME + ":00:00", out etime))
                    {
                        if (!string.IsNullOrEmpty(DEPT))
                        {
                            message = bll.ZJUserDeptSetTime(DATA, etime.ToString("yyyy-MM-dd HH:mm:ss"), DEPT);
                        }
                        else
                        {
                            message.Code = -1;
                            message.Content = "部门不能为空";
                        }
                    }
                    else
                    {
                        message.Code = -1;
                        message.Content = "时间格式异常,请重新输入";
                    }

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
        /// 清理指定部门下的过期用户
        /// </summary>
        /// <param name="DEPT"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJUserDeptClearUser(string DEPT)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(DEPT))
                {
                    message = bll.ZJUserDeptClearUser(DEPT);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未查询到部门,请重新选择" }.ToString());
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
