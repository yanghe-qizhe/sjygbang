using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using EOS.Entity;
using EOS.DataAccess;
using EOS.Repository;
using System.Data.Common;
using System.Data;
using EOS.WebApp.Areas.BaseSet.Models;
using EOS.WebApp.SxtjModels;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class QualityPersonController : Controller
    {
        //
        // GET: /BaseSet/QUALITYPERSON/
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        BD_QUALITYPERSONBLL bd_qualitypersonbll = new BD_QUALITYPERSONBLL();
        BD_PCRAWCYBll pcbll = new BD_PCRAWCYBll();
        VBD_PCRAWCYBll vpcbll = new VBD_PCRAWCYBll();
        VBD_ZJRESULTBll vorderbll = new VBD_ZJRESULTBll();

        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }

        public ActionResult QualityPersonIndex()
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
        public ActionResult GetQualityPersonList(JqGridParam jqgridparam)
        {
            try
            {
                string KeyValue = "";
                Stopwatch watch = CommonHelper.TimerStart();
                List<BD_QUALITYPERSON> ListData = bd_qualitypersonbll.GetOrderList(KeyValue);
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

        public ActionResult QualityPersonForm()
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

        /// <summary>
        /// 【验级人员管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            BD_QUALITYPERSON entity = bd_qualitypersonbll.GetOrder(KeyValue);
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
        public ActionResult SubmitOrderForm(BD_QUALITYPERSON entity, string ModuleId, string KeyValue)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update BD_QUALITYPERSON set WORKER_ID='{0}',WORKER_NAME='{1}',VALID='{2}' where ID='{3}'",
                        entity.WORKER_ID, entity.WORKER_NAME, entity.VALID, int.Parse(KeyValue)));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }
                else
                {
                    //验证
                    string sql = String.Format("select count(1) from BD_QUALITYPERSON where WORKER_ID='{0}'", entity.WORKER_ID);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "验级员工号已存在" }.ToString());
                    }

                    strSql.Clear();
                    strSql.Append(string.Format("insert into BD_QUALITYPERSON (WORKER_ID,WORKER_NAME,VALID) VALUES ('{0}','{1}','{2}')",
                        entity.WORKER_ID, entity.WORKER_NAME, entity.VALID));
                    database.ExecuteBySql(strSql, isOpenTrans);

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
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            try
            {
                string Message = "删除成功。";
                strSql.Clear();
                strSql.Append(string.Format("delete from BD_QUALITYPERSON where ID='{0}'", int.Parse(KeyValue)));
                database.ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

    }
}
