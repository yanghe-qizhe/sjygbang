using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using EOS.Repository;

namespace EOS.WebApp.Areas.ZJJHManager.Controllers
{
    public class ZJJHCYShortProcessController : PublicController<QC_ZJJHCY>
    {
        QC_ZJJHCYBLL pcbll = new QC_ZJJHCYBLL();
        VQC_ZJJHCYBLL vpcbll = new VQC_ZJJHCYBLL();
        #region 页面返回
        public ActionResult ZJJHCYShortProcessList()
        {
            return View();
        }
        public ActionResult ZJJHCYShortProcessAdd()
        {
            ViewBag.FormID = "ZJJHCYShortProcessAdd";
            return View();
        }
        /// <summary>
        /// 重写 采样组批 编辑页面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public override ActionResult SetForm(string KeyValue)
        {
            VQC_ZJJHCY entity = DataFactory.Database().FindEntity<VQC_ZJJHCY>(KeyValue);
            return Content(entity.ToJson());
        }
        #endregion

        #region 数据列表

        /// <summary>
        /// 获取采样短流程列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJJHCYShortProcessList(string BILLNO, string StartTime, string EndTime, string MATERIALNAME, string CHECKTYPE, string CHECKISSEND, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJJHCY> ListData = vpcbll.GetZJJHCYShortProcessList(BILLNO, StartTime, EndTime, MATERIALNAME, CHECKTYPE, CHECKISSEND, jqgridparam);
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
        /// 提交表单 页面的完成 
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJJHCYFinishShortProcess(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = vpcbll.ZJJHCYFinishShortProcess(KeyValue);
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
        /// 短流程 保存制样化验数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJJHShortProcessSave(QC_ZJJHCY entity, string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = pcbll.ZJJHShortProcessSave(entity, KeyValue);
                    return Content(new JsonMessage { Success = false, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    message = pcbll.ZJJHShortProcessSave(entity, KeyValue);
                    return Content(new JsonMessage { Success = false, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
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
