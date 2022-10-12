using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using EOS.Business;
using EOS.Repository;
using EOS.Utilities;
using System.Diagnostics;

namespace EOS.WebApp.Areas.ZJZXManager.Controllers
{
    public class ZJZXCYAllBatchController : PublicController<QC_ZJZXCY>
    {
        QC_ZJZXCYBll pcbll = new QC_ZJZXCYBll();
        VQC_ZJZXCYBll vpcbll = new VQC_ZJZXCYBll();
        #region 页面返回
        public ActionResult ZJZXCYAllBatchList()
        {
            return View();
        }
        public ActionResult ZJZXCYAllBatchAdd()
        {
            return View();
        }
        public ActionResult ZJZXCYAllBatchSelect()
        {
            return View();
        }
        /// <summary>
        /// 重写 采样组批 编辑页面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public override ActionResult SetForm(string KeyValue)
        {
            VQC_ZJZXCY entity = DataFactory.Database().FindEntity<VQC_ZJZXCY>(KeyValue);
            List<VQC_ZJZXCYDETAILSBATCH> entitydetails = DataFactory.Database().FindList<VQC_ZJZXCYDETAILSBATCH>("MAINID", KeyValue, "CRETIME", "ASC");
            return Content("{\"M\":[" + entity.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// List页面获取组大批列表
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetZJZXCYBatchList(string BillNo, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCY> ListData = vpcbll.GetZJZXCYBatchList(BillNo, StartTime, EndTime, jqgridparam);
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
        /// List页面获取组大批明细批次列表
        /// </summary>
        /// <param name="MAINID"></param>
        /// <returns></returns>
        public ActionResult GetZJZXCYBatchListItem(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCYDETAILS> ListData = vpcbll.GetOrderListItem(MAINID, jqgridparam);
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
        /// 多选组大批列表
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="MATERIAL"></param>
        /// <param name="SUPPLY"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetZJZXCYList(string StartTime, string EndTime, string MATERIAL, string MATERIALNAME, string BILLNO, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCY> ListData = vpcbll.GetZJZXCYBatchListSelect(StartTime, EndTime, MATERIAL, MATERIALNAME, BILLNO, jqgridparam);
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
        /// 获取选中的批次信息，通过批次主键ID
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult ZJZXCYGetBatchData(string KeyValue)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    return Content(pcbll.ZJZXCYGetBatchData(KeyValue));
                }
                else
                {
                    return Content("");
                }
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
        /// 删除
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual ActionResult DeleteItem(string KeyValue)
        {
            try
            {
                ResultMessage message = pcbll.PCZXCYDelete(KeyValue);

                WriteLog(message.Code, KeyValue, message.Message);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 短流程完成操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJZXCYAllBatchFinish(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = pcbll.ZJZXCYAllBatchFinish(KeyValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中批号,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        #endregion

        #region 表单提交
        /// <summary>
        /// 组大批表单提交
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <param name="PCRAWCYDetailJson"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJZXCYAllBatchSave(QC_ZJZXCY entity, string KeyValue, string ZJZXCYDetailJson)
        {
            int IsOk = 0;
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IsOk = vpcbll.ZJZXBATCHCYEDITSave(entity, KeyValue, ZJZXCYDetailJson);
                    if (IsOk == 2)
                    {
                        return Content(new JsonMessage { Success = false, Code = "0", Message = "当前批号已存在,请重新输入批号" }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
                    }
                }
                else
                {

                    IsOk = vpcbll.ZJZXBATCHCYSave(entity, ZJZXCYDetailJson);
                    if (IsOk == 2)
                    {
                        return Content(new JsonMessage { Success = false, Code = "0", Message = "当前批号已存在,请重新输入批号" }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
                    }
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
