using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using EOS.Utilities;
using EOS.Business;
using System.Text;
using System.Data.Common;
using EOS.Repository;
using System.Diagnostics;
using EOS.Entity;

namespace EOS.WebApp.Areas.ZJEnergyManager.Controllers
{
    public class ZJGASController : PublicController<QC_ZJGAS>
    {
        QC_ZJGASBll gasbll = new QC_ZJGASBll();
        #region 页面返回
        public ActionResult ZJGASList()
        {
            return View();
        }
        public ActionResult ZJGASForm()
        {
            return View();
        }
        public ActionResult ZJGASFANOSelect()
        {
            return View();
        }
        public ActionResult ZJGASFormSave()
        {
            return View();
        }
        public ActionResult ZJGASAddItem()
        {
            return View();
        }
        public ActionResult ZJGASFANOForm()
        {
            return View();
        }
        public ActionResult ZJGASAddTime()
        {
            return View();
        }
        public override ActionResult SetForm(string BILLNO)
        {
            List<QC_ZJGAS> model = DataFactory.Database().FindList<QC_ZJGAS>(" AND BILLNO='" + BILLNO + "' AND TYPE=1 ");
            List<VQC_ZJGASDETAILS> entitydetails = DataFactory.Database().FindList<VQC_ZJGASDETAILS>(" AND MAINBILLNO='" + BILLNO + "' AND TYPE=1 ");
            return Content("{\"M\":[" + model.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
        }
        public ActionResult SetFormForAddItem(string GASID)
        {
            QC_ZJGAS model = DataFactory.Database().FindEntityByWhere<QC_ZJGAS>(" AND ID='" + GASID + "'");
            return Content(model.ToJson());
        }

        #endregion


        #region 数据列表
        /// <summary>
        /// 煤气分析任务列表
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="BILLNO"></param>
        /// <param name="MATERIALNAME"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetList(string StartTime, string EndTime, string BILLNO, string MATERIALNAME, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QC_ZJGAS> ListData = gasbll.GetList("1", StartTime, EndTime, BILLNO, MATERIALNAME, jqgridparam);
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
        /// 获取煤气分析质检方案
        /// </summary>
        /// <param name="CYDATE"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetGASZJFAList(string CYDATE, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBase_ZJFA> ListData = gasbll.GetGASZJFAList(CYDATE, jqgridparam);
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

        #region 表单提交

        /// <summary>
        /// 新建煤气任务
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CREATEGAS(string DATA, string CYDATE)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = gasbll.CREATEGAS("1", DATA, CYDATE, "");
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中物料" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 保存 煤气分析数据
        /// </summary>
        /// <param name="PCRAWZJFA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJGASDataSave(string ID, string postDataNew)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(ID))
                {
                    if (!string.IsNullOrEmpty(postDataNew))
                    {
                        message = gasbll.ZJGASDataSave(ID, postDataNew);
                        return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中任务" }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中任务" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 插入质检项
        /// </summary>
        /// <param name="GASID"></param>
        /// <param name="ITEMNO"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJGASItemSave(string GASID, string ITEMNO)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(GASID))
                {
                    if (!string.IsNullOrEmpty(ITEMNO))
                    {
                        message = gasbll.ZJGASItemSave("1", GASID, ITEMNO);
                        return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中质检项" }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "任务ID为空" }.ToString());
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
        /// 删除
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteItem(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = gasbll.DeleteItem(KeyValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中物料" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 删除质检项目
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteGASDetailsID(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = gasbll.DeleteDetailsID(KeyValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中,请刷新后重试" }.ToString());
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
