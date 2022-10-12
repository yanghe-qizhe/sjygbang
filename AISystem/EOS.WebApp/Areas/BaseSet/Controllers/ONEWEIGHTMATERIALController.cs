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
    public class ONEWEIGHTMATERIALController : PublicController<BD_ONEWEIGHTMATERIAL>
    {
        string ModuleId = "";

        BD_ONEWEIGHTMATERIALBll bd_carsbll = new BD_ONEWEIGHTMATERIALBll();

        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>

        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPageListJson(string Material, string Type, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<BD_ONEWEIGHTMATERIAL> ListData = bd_carsbll.GetOrderList(Material, Type, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(BD_ONEWEIGHTMATERIAL entity, string ModuleId, string KeyValue)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string sql = "";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from BD_ONEWEIGHTMATERIAL where MATERIAL='{0}' and ID!='{1}'", entity.MATERIAL, KeyValue);
                }
                else
                {
                    sql = String.Format("select count(1) from BD_ONEWEIGHTMATERIAL where MATERIAL='{0}'", entity.MATERIAL);
                }
                int res = DataFactory.Database().FindCountBySql(sql);

                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("物料：{0}已存在", entity.MATERIALNAME) }.ToString());
                }


                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    if (entity.MATERIALSPEC == "null" || entity.MATERIALSPEC == null)
                    {
                        entity.MATERIALSPEC = "";
                    }
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


        [HttpPost]
        public ActionResult SubmitOrderAllForm(string OrderEntryJson, string ModuleId)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                List<BD_ONEWEIGHTMATERIAL> OrderEntryList = OrderEntryJson.JonsToList<BD_ONEWEIGHTMATERIAL>();
                string sql = "";
                int res = 0;
                //处理明细
                int index = 1;
                foreach (BD_ONEWEIGHTMATERIAL orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.MATERIAL))
                    {
                        sql = String.Format("select count(1) from BD_ONEWEIGHTMATERIAL where MATERIAL='{0}' and Type={1}", orderentry.MATERIAL, orderentry.TYPE);
                        res = DataFactory.Database().FindCountBySql(sql);
                        if (res == 0)
                        {
                            orderentry.Create();
                            if (orderentry.MATERIALSPEC == "null" || orderentry.MATERIALSPEC == null)
                            {
                                orderentry.MATERIALSPEC = "";
                            }
                            database.Insert(orderentry, isOpenTrans);
                        }
                        index++;
                    }
                }

                string Message = "新增成功。";

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
        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALForm()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALBATCHForm()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALKZForm()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALBATCHKZForm()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALXCIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALNDIndex()
        {
            return View();
        }
        /// <summary>
        /// 物料扣重配置
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALKZIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALCYIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALPDIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult ONEWEIGHTMATERIALSPIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult SelectMaterial()
        {
            return View();
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
                database.Delete("BD_ONEWEIGHTMATERIAL", "ID", KeyValue);
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
        /// 【计量一次物料管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            BD_ONEWEIGHTMATERIAL entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
