using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;

namespace EOS.WebApp.Areas.ZJZXManager.Controllers
{
    public class ZJZXZYController : PublicController<QC_ZJZXSEND>
    {
        QC_ZJZXCYBll bll = new QC_ZJZXCYBll();
        VQC_ZJZXCYBll vbll = new VQC_ZJZXCYBll();
        #region 页面返回
        public ActionResult ZJZXZYList()
        {
            return View();
        }
        public ActionResult ZJZXZYAddCY()
        {
            return View();
        }
        public ActionResult ZJZXZYSelect()
        {
            return View();
        }
        public ActionResult SelectCYData()
        {
            return View();
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// 获取采样组批制样列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZXZYList(string BILLNO, string StartTime, string EndTime, string ISSEND, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCY> ListData = vbll.GetOrderListForZY(BILLNO, StartTime, EndTime, ISSEND, jqgridparam);
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
        /// 制样新增页面 选择采样批次（已完成、未送检、未制样）
        /// </summary>
        /// <param name="BILLNO"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZXZYSelectList(string BILLNO, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCY> ListData = vbll.GetOrderListForZYSelect(BILLNO, StartTime, EndTime, jqgridparam);
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
        /// 送检
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CheckCYData(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = bll.SendCYData(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());

                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 取消送检
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult UNSendCheck(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = bll.UNSendCheck(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 送检匹配质检方案 多选方案 查询匹配列表 返回
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CheckCYDataSelect(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = bll.SendCYDataSelect(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }


        #endregion

        #region 表单提交
        /// <summary>
        /// 新增多选采样批次，制样操作
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MutilZYSave(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = bll.MutilZYSave(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 送检自动匹配质检方案 保存
        /// </summary>
        /// <param name="PCRAWZJFA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXZJFASendSave(string PCRAWZJFA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(PCRAWZJFA))
                {
                    message = bll.SendCYDataSave(PCRAWZJFA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }


        #endregion
    }
}
