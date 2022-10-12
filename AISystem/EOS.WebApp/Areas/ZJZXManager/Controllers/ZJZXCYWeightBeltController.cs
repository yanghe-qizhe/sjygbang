using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using EOS.Business;
using EOS.Utilities;
using System.Diagnostics;
using EOS.Repository;
using EOS.DataAccess;

namespace EOS.WebApp.Areas.ZJZXManager.Controllers
{
    public class ZJZXCYWeightBeltController : PublicController<QC_ZJZXCY>
    {
        QC_ZJZXCYBll bll = new QC_ZJZXCYBll();
        VQC_ZJZXCYBll vbll = new VQC_ZJZXCYBll();
        VQC_ZJZXCYDETAILSBll vbllitem = new VQC_ZJZXCYDETAILSBll();
        VZJZXQYBLL vzjzxbll = new VZJZXQYBLL();
        #region 页面返回
        public ActionResult ZJZXCYWeightBeltList()
        {
            return View();
        }
        public ActionResult ZJZXCYWeightBeltAdd()
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
            return Content(entity.ToJson());
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// 获取采样组批列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetBELTList(string BILLNO, string StartTime, string EndTime, string MATERIALNAME, string CHECKTYPE, string CHECKISSEND, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCY> ListData = vbll.GetBELTList(BILLNO, StartTime, EndTime, MATERIALNAME, CHECKTYPE, CHECKISSEND, jqgridparam);
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
        /// 根据选择的物料获取唯一的质检方案
        /// </summary>
        /// <param name="MaterialID"></param>
        /// <param name="TYPE">空 按照物料查询    不为空 表示 烧结矿 全分析 或 化学 </param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetMaterialZJFA(string MaterialID, string TYPE)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                IDatabase database = DataFactory.Database();

                List<VBase_ZJFA> mlist;

                if (string.IsNullOrEmpty(TYPE))
                {
                    mlist = database.FindList<VBase_ZJFA>(" AND MATERIALID='" + MaterialID + "' AND ENABLED=1 AND TYPE='78341346-7108-443d-a20c-dd3a061ec999' ");
                }
                else
                {
                    mlist = database.FindList<VBase_ZJFA>(" AND MATERIALID='" + MaterialID + "' AND ENABLED=1 AND TYPE='78341346-7108-443d-a20c-dd3a061ec999' AND FANAME LIKE '%" + TYPE + "%'");
                }


                if (mlist.Count > 0)
                {
                    message.Code = 1;
                    message.Message = mlist.First<VBase_ZJFA>().ToJson();
                }
                else
                {
                    message.Code = -1;
                    message.Message = "未查询到当前所选物料的质检方案,请先维护质检方案";
                }

                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 皮带秤完成操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJZXCYWeightBeltFinish(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.ZJZXCYWeightBeltFinish(KeyValue);
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

        /// <summary>
        /// 皮带秤取消完成
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJZXCYWeightBeltUNFinish(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.ZJZXCYWeightBeltUNFinish(KeyValue);
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
        /// 皮带秤 保存制样化验数据
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZYSAVE(QC_ZJZXCY entity, string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.ZXBELTCYSave(entity, KeyValue);
                    return Content(new JsonMessage { Success = false, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    message = bll.ZXBELTCYSave(entity, KeyValue);
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
