using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Entity;
using EOS.Business;
using EOS.DataAccess;
using System.Data.Common;
using EOS.Repository;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class TrainsController : PublicController<BD_TRAINS>
    {
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        BD_TRAINSBll bd_trainsbll = new BD_TRAINSBll();
        [LoginAuthorize]
        public ActionResult TrainsForm()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult TrainsIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult TrainsDetail()
        {
            return View();
        }

        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>

        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetTrainsList(string keyword, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<BD_TRAINS> ListData = bd_trainsbll.GetOrderList(keyword, StartTime, EndTime, jqgridparam);
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
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public override ActionResult SubmitForm(BD_TRAINS entity, string KeyValue)
        {
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        public override ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 【车辆管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            BD_TRAINS entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
