using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Business;
using EOS.Entity;
using EOS.Repository;
using System.Diagnostics;
using EOS.Utilities;
using EOS.DataAccess;
using System.Data.Common;

namespace EOS.WebApp.Areas.ZJZXManager.Controllers
{
    public class ZJZXCYShortProcessController : PublicController<VQC_ZJZXCY>
    {
        QC_ZJZXCYBll pcbll = new QC_ZJZXCYBll();
        VQC_ZJZXCYBll vpcbll = new VQC_ZJZXCYBll();
        #region 页面返回
        public ActionResult ZJZXCYShortProcessList()
        {
            return View();
        }
        public ActionResult ZJZXCYShortProcessAdd()
        {
            ViewBag.FormID = "ZJZXCYShortProcessAdd";
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
        /// 获取采样短流程列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJZXCYShortProcessList(string BILLNO, string StartTime, string EndTime, string MATERIALNAME, string CHECKTYPE, string CHECKISSEND, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCY> ListData = vpcbll.GetZJZXCYShortProcessList(BILLNO, StartTime, EndTime, MATERIALNAME, CHECKTYPE, CHECKISSEND, jqgridparam);
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
        /// 短流程完成操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJZXCYFinishShortProcess(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = pcbll.ZJZXCYFinishShortProcess(KeyValue);
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
        /// 获取质检方案 方案物料
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetFANOMaterial(string id, string name, string type)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            ResultMessage message = new ResultMessage();
            try
            {
                //type 类型说明
                //type=1  单种煤 混合煤  方案名称为单种煤、混合煤  物料随机  按照方案名称、方案类型去查询方案、物料
                //type=2  按照物料ID获取方案
                //type=3  特殊煤粉  方案名称为煤粉，物料为喷吹烟煤  按照物料、方案类型去查询方案、物料

                string FAType = "78341346-7108-443d-a20c-dd3a061ec999";//过程产品
                string DEF1 = "4";//短流程
                VBase_ZJFA MReturn = new VBase_ZJFA();
                List<VBase_ZJFA> MReturnList = new List<VBase_ZJFA>();
                VBase_ZJFA m = new VBase_ZJFA();
                switch (type)
                {
                    case "1":
                        m.FANAME = name;
                        MReturnList = database.FindList<VBase_ZJFA>(" AND TYPE='" + FAType + "' AND DEF1='" + DEF1 + "' AND FANAME='" + m.FANAME + "' ");
                        break;
                    case "2":
                        m.MATERIALID = id;
                        MReturnList = database.FindList<VBase_ZJFA>(" AND TYPE='" + FAType + "' AND DEF1='" + DEF1 + "' AND MATERIALID='" + m.MATERIALID + "' ");
                        break;
                    case "3":
                        m.MATERIALID = id;
                        m.FANAME = name;
                        MReturnList = database.FindList<VBase_ZJFA>(" AND TYPE='" + FAType + "' AND DEF1='" + DEF1 + "' AND MATERIALID='" + m.MATERIALID + "' AND FANAME='" + m.FANAME + "' ");
                        break;
                }

                if (MReturnList.Count > 0)
                {
                    MReturn = MReturnList.First();
                    if (string.IsNullOrEmpty(MReturn.FANAME))
                    {
                        message.Code = -1;
                        message.Content = "未查询到质检方案";
                    }
                    else if (string.IsNullOrEmpty(MReturn.FANO))
                    {
                        message.Code = -1;
                        message.Content = "未查询到质检方案";
                    }
                    else if (string.IsNullOrEmpty(MReturn.MATERIALID))
                    {
                        message.Code = -1;
                        message.Content = "质检方案【" + MReturn.FANAME + "】物料为空";
                    }
                    else if (string.IsNullOrEmpty(MReturn.MATERIALNAME))
                    {
                        message.Code = -1;
                        message.Content = "质检方案【" + MReturn.FANAME + "】物料为空";
                    }
                    else
                    {
                        message.Code = 1;
                        message.Content = MReturn.ToJson();
                    }
                }
                else
                {
                    message.Code = -1;
                    message.Content = "未查询到质检方案";
                }

                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());

            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 数据提交
        /// <summary>
        /// 短流程 保存制样化验数据
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
                    message = pcbll.ZJZXZYCYSave(entity, KeyValue);
                    return Content(new JsonMessage { Success = false, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    message = pcbll.ZJZXZYCYSave(entity, KeyValue);
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
