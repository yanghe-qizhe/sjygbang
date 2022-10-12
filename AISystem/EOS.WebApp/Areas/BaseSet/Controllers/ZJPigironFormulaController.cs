﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using EOS.DataAccess;
using EOS.Repository;
using EOS.Business;
using EOS.Utilities;
using System.Diagnostics;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class ZJPigironFormulaController : PublicController<VBASE_ZJPIGIRONMODEL>
    {
        VBASE_ZJPIGIRONMODELBLL bll = new VBASE_ZJPIGIRONMODELBLL();
        IDatabase database = DataFactory.Database();
        #region 页面返回
        public ActionResult ZJPigironFormulaList()
        {
            return View();
        }
        public ActionResult ZJPigironFormulaForm()
        {
            ViewBag.FormID = "ZJPigironFormulaForm";
            return View();
        }
        /// <summary>
        /// 重写 质检方案 编辑页面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public override ActionResult SetForm(string KeyValue)
        {
            VBASE_ZJPIGIRONMODEL entity = DataFactory.Database().FindEntity<VBASE_ZJPIGIRONMODEL>(KeyValue);
            return Content(entity.ToJson());
        }
        #endregion


        #region 数据列表
        /// <summary>
        /// 数据列表
        /// </summary>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPIGIronList(JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBASE_ZJPIGIRONMODEL> ListData = bll.GetPIGIronList(jqgridparam);
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
        /// 生铁判定公式保存
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="LSKZData"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PIGIRONSave(string KeyValue, string LSKZData)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(LSKZData))
                {
                    message = bll.PIGIRONSave(KeyValue, LSKZData);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：流水扣重公式不能为空,请稍后重试" }.ToString());
                }

            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 删除规则
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeletePIGIRON(string KeyValue)
        {
            try
            {
                int IsOk = 0;
                var Message = "删除失败。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IsOk = new RepositoryFactory<BASE_ZJPIGIRONMODEL>().Repository().Delete(KeyValue);
                    database.Commit();
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
                    }
                }
                else
                {
                    Message = "公式编号不能为空";
                }

                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion
    }
}
