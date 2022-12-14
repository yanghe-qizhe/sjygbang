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
    public class JYPConfigController : PublicController<WB_MATERIAL_JYP>
    {
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        WB_MATERIAL_JYPBll bd_bll = new WB_MATERIAL_JYPBll();
        /// <summary>
        /// 获取自动单据内码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }
        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>

        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderList(string MATERIAL, string MATERIALNAME, string TYPE, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<WB_MATERIAL_JYP> ListData = bd_bll.GetOrderList(MATERIAL, MATERIALNAME, TYPE, jqgridparam);
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
        public ActionResult SubmitOrderForm(WB_MATERIAL_JYP entity, string ModuleId, string KeyValue)
        {
            return Content(bd_bll.SubmitOrder(entity, ModuleId, KeyValue).ToString());
        }
        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>

        [LoginAuthorize]
        public ActionResult JYPConfigForm()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.Code = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        //列表
        [LoginAuthorize]
        public ActionResult SGIndex()
        {
            return View();
        }
        //列表
        [LoginAuthorize]
        public ActionResult JYPConfigIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult SGJYPConfigForm()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.Code = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            return Content(bd_bll.DeleteOrder(KeyValue).ToString());
        }
        /// <summary>
        ///  返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            WB_MATERIAL_JYP entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        [LoginAuthorize]
        public ActionResult GetEntity(string KeyValue)
        {
            WB_MATERIAL_JYP entity = bd_bll.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
