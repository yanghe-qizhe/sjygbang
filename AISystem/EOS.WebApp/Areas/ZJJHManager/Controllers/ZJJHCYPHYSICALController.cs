using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using EOS.Entity;
using EOS.DataAccess;
using EOS.Repository;

namespace EOS.WebApp.Areas.ZJJHManager.Controllers
{
    public class ZJJHCYPHYSICALController : PublicController<VQC_ZJJHRESULT>
    {
        VQC_ZJJHRESULTBLL VBLL = new VQC_ZJJHRESULTBLL();
        VQC_ZJJHRESULTDETAILSBLL VBLLITEM = new VQC_ZJJHRESULTDETAILSBLL();
        #region 页面返回
        public ActionResult ZJJHCYPHYForm()
        {
            return View();
        }
        public ActionResult ZJJHCYPHYSICALList()
        {
            return View();
        }

        /// <summary>
        /// 获取物理质检数据、采样组批明细数据 
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult SetFormMain(string KeyValue, string CYID)
        {
            IDatabase database = DataFactory.Database();
            VQC_ZJJHRESULT model = database.FindEntityByWhere<VQC_ZJJHRESULT>(" AND ID='" + KeyValue + "'");

            List<VQC_ZJJHCYDETAILS> entitydetails = DataFactory.Database().FindList<VQC_ZJJHCYDETAILS>("MAINID", CYID);
            return Content("{\"M\":[" + model.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
        }


        #endregion

        #region 数据列表
        /// <summary>
        /// 获取物理质检结果主表信息
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetJHPHYList(string BILLNO, string StartTime, string EndTime, string PHYSTATUS, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJJHRESULT> ListData = VBLL.GetOrderListForPHY(BILLNO, StartTime, EndTime, PHYSTATUS, jqgridparam);
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
        /// 获取物理质检结果明细表信息
        /// </summary>
        /// <param name="MAINID"></param> 
        /// <returns></returns>
        public ActionResult GetJHPHYZJItemList(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJJHRESULTDETAILS> ListData = VBLLITEM.GetJHPHYZJItemList(MAINID, jqgridparam);
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
        /// 获取当前物理检测状态
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="CYID"></param>
        /// <returns></returns>
        public ActionResult GetZJStatus(string KeyValue, string CYID)
        {
            try
            {
                if (!string.IsNullOrEmpty(KeyValue) && !string.IsNullOrEmpty(CYID))
                {
                    VQC_ZJJHRESULT model = VBLL.GetZJStatus(KeyValue, CYID);
                    if (!string.IsNullOrEmpty(model.ID))
                    {
                        return Content(new JsonMessage { Success = false, Code = "1", Message = (model as VQC_ZJJHRESULT).ToJson().ToString() }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "获取状态失败" }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "获取状态失败,主键编号不能为空" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }


        #endregion

        #region 表单操作

        /// <summary>
        /// 判断当前质检值是否合格
        /// </summary>
        /// <param name="ZJITEMNO"></param>
        /// <param name="KeyValue">ZJRESULTID</param>
        /// <param name="ZJValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult JHPHYCheckZJBZ(string ZJITEMNO, string ZJFA, string ZJValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(ZJITEMNO))
                {
                    message = VBLL.CheckZJBZForPHY("PHY", ZJITEMNO, ZJFA, ZJValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "判断失败,质检项目编号为空" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "判断失败," + ex.Message.ToString() }.ToString());
            }
        }

        /// <summary>
        /// 加载物理质检项目
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult LoadPHYZJItem(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.GetLoadPHYZJItem(KeyValue);
                    if (message.Code == 1)
                    {
                        return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = (message.Content as List<VQC_ZJJHRESULTDETAILS>).ToJson() }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中质检数据" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 物理检测结果提交
        /// </summary>
        /// <param name="KeyValue">质检结果主表ID</param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult JHPHYSave(string KeyValue, string JHPHYData, string PHYMemo)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(JHPHYData) && !string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.JHPHYSave(KeyValue, JHPHYData, PHYMemo);
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
        /// 物理检测结果 完成操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult JHPHYFinish(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.JHPHYFinish(KeyValue);
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
        /// 取消 物理检测结果 完成操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult JHPHYUNFinish(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.JHPHYUNFinish(KeyValue);
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
