using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Utilities;
using System.Diagnostics;
using EOS.Business;
using EOS.Entity;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class ZJBZController : PublicController<Base_ZJBZModelItem>
    {
        Base_ZJBZModelItemBll zjbzbitemll = new Base_ZJBZModelItemBll();
        VBase_ZJBZModelBll vzjbzbll = new VBase_ZJBZModelBll();
        #region 页面返回
        public ActionResult ZLBZList()
        {
            return View();
        }
        public ActionResult ZLBZForm()
        {
            return View();
        }

        /// <summary>
        /// 返回质检标准明细信息
        /// </summary>
        /// <param name="MAINID"></param>
        /// <returns></returns>
        public ActionResult GetZJBZITEM(string MAINID)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(MAINID))
                {
                    message = vzjbzbll.GetZJBZITEM(MAINID);
                    return Content(new JsonMessage { Success = false, Code = message.Code.ToString(), Message = (message.Content as List<Base_ZJBZModelItem>).ToJson() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = message.Content.ToString() }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 数据列表

        /// <summary>
        /// 质检方案 质检项 的 质量标准主表 数据列表
        /// </summary>
        /// <param name="FANO">方案编号</param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJBZList(string FANO, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBase_ZJBZModel> ListData = vzjbzbll.GetZJBZList(FANO, jqgridparam);
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
        /// 质检方案 质检项 的 质量标准子表 数据列表
        /// </summary>
        /// <param name="MAINID">质量标准主表ID</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJBZItemList(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<Base_ZJBZModelItem> ListData = zjbzbitemll.GetZJBZItemList(MAINID, jqgridparam);
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

        #region 表达提交
        /// <summary>
        /// 质检标准提交
        /// </summary>
        /// <param name="MAINID"></param>
        /// <param name="ENABLED"></param>
        /// <param name="ZJBZITEM"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJBZITEMSave(string MAINID, string ENABLED, string ZJBZITEM)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(MAINID))
                {
                    if (!string.IsNullOrEmpty(ZJBZITEM))
                    {
                        message = vzjbzbll.ZJBZITEMSave(MAINID, ENABLED, ZJBZITEM);
                        return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：质检标准明细不能为空,请稍后重试" }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：质检标准ID不能为空,请稍后重试" }.ToString());
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
