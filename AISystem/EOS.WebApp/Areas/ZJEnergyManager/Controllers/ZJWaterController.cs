using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using EOS.Entity;
using EOS.Repository;

namespace EOS.WebApp.Areas.ZJEnergyManager.Controllers
{
    public class ZJWaterController : PublicController<QC_ZJGAS>
    {
        QC_ZJGASBll gasbll = new QC_ZJGASBll();
        #region 页面返回
        public ActionResult ZJWaterAddItem()
        {
            return View();
        }
        public ActionResult ZJWaterFANOForm()
        {
            return View();
        }
        public ActionResult ZJWaterFormSave()
        {
            return View();
        }
        public ActionResult ZJWaterList()
        {
            return View();
        }
        public ActionResult ZJWaterAddTime()
        {
            return View();
        }
        public override ActionResult SetForm(string ID)
        {
            QC_ZJGAS m = DataFactory.Database().FindEntity<QC_ZJGAS>(ID);
            List<QC_ZJGAS> model = DataFactory.Database().FindList<QC_ZJGAS>(" AND BILLNO='" + m.BILLNO + "' AND DEF2='" + m.DEF2 + "' AND TYPE=2 ");
            List<VQC_ZJGASDETAILS> entitydetails = DataFactory.Database().FindList<VQC_ZJGASDETAILS>(" AND MAINBILLNO='" + m.BILLNO + "' AND DEF2='" + m.DEF2 + "' AND TYPE=2 ");
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
        /// 水分析任务列表
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="BILLNO"></param>
        /// <param name="MATERIALNAME"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetListWater(string StartTime, string EndTime, string BILLNO, string MATERIALNAME, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QC_ZJGAS> ListData = gasbll.GetList("2", StartTime, EndTime, BILLNO, MATERIALNAME, jqgridparam);
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
        /// 水煤气分析质检方案
        /// </summary>
        /// <param name="CYDATE"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetWaterZJFAList(string CYDATE, string DEF2, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBase_ZJFA> ListData = gasbll.GetWaterZJFAList(CYDATE, DEF2, jqgridparam);
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
        /// 新建水任务
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CREATEWater(string DATA, string CYDATE, string DEF2)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = gasbll.CREATEGAS("2", DATA, CYDATE, DEF2);
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
        /// 保存 水分析数据
        /// </summary>
        /// <param name="PCRAWZJFA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJWaterDataSave(string ID, string postDataNew)
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
        public ActionResult ZJWaterItemSave(string GASID, string ITEMNO)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(GASID))
                {
                    if (!string.IsNullOrEmpty(ITEMNO))
                    {
                        message = gasbll.ZJGASItemSave("2", GASID, ITEMNO);
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
        public ActionResult DeleteWaterDetailsID(string KeyValue)
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
