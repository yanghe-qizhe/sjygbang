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
    public class CARRIERController : PublicController<BD_CARRIER>
    {
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        BD_CARRIERBll bd_bll = new BD_CARRIERBll();
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
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CARRIERForm()
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
        public ActionResult CARRIERIndex()
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
        public ActionResult GETCARRIERList(string keyword, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<BD_CARRIER> ListData = bd_bll.GetOrderList(keyword, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(BD_CARRIER entity, string ModuleId, string KeyValue)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string sql = "";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from BD_CARRIER where NAME='{0}' and ID!='{1}'", entity.NAME, KeyValue);
                }
                else
                {
                    sql = String.Format("select count(1) from BD_CARRIER where NAME='{0}'", entity.NAME);
                }
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "承运商已存在" }.ToString());
                }
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    entity.SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    sql = String.Format("select count(1) from BD_CARRIER where  CODE='{0}'", entity.CODE);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.CODE = this.BillCode();
                    }
                    entity.Create();
                    entity.SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
                    database.Insert(entity, isOpenTrans);
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
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
            //验证
            string sql = String.Format("select count(1) from DP_SOCARSORDER where TRAFFICCOMPANY='{0}'", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该数据已使用！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("BD_CARRIER", "ID", KeyValue);
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
        /// 【车辆管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            BD_CARRIER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
