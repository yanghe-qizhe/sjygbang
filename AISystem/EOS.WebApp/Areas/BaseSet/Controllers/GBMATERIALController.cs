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
using System.Text;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class GBMATERIALController : PublicController<PD_GBMATERIAL>
    {
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        PD_GBMATERIALBll bd_bll = new PD_GBMATERIALBll();


        #region 仓库排序
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GBMATERIALIndex()
        {
            return View();
        }

        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GBMATERIALForm()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult GBMATERIALForm_New()
        {
            return View();
        }


        /// <summary>
        /// 列表（返回Json）
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPageListJson(string Material, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<PD_GBMATERIAL> ListData = bd_bll.GetOrderList(Material, StartTime, EndTime, jqgridparam);
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
        ///  明细列表
        /// </summary>
        /// <param name="POOrderId">订单主键</param>
        /// <returns></returns>


        [LoginAuthorize]
        public ActionResult GetOrderEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = bd_bll.GetOrderEntryList(KeyValue),
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
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            List<PD_GBMATERIAL> entity = repositoryfactory.Repository().FindList("MATERIAL", KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm_OLD(PD_GBMATERIAL entity, string ModuleId, string KeyValue)
        {
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
        /// 提交表单
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="ModuleId"></param>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm(PD_GBMATERIAL entity, string OrderEntryJson, string ModuleId, string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                //数据列表
                List<PD_GBMATERIAL> OrderEntryList = OrderEntryJson.JonsToList<PD_GBMATERIAL>();

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    //编辑
                    database.Delete<PD_GBMATERIAL>("MATERIAL", KeyValue, isOpenTrans);
                }

                //新增
                foreach (PD_GBMATERIAL p in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(p.GBTYPE))
                    {
                        p.Create();
                        p.MATERIAL = entity.MATERIAL;
                        p.MATERIALNAME = entity.MATERIALNAME;
                        database.Insert(p, isOpenTrans);
                    }
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

        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("PD_GBMATERIAL", "ID", KeyValue);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion



    }
}
