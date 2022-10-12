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
    public class SupplyController : PublicController<BD_SUPPLY>
    {
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        BD_SupplyBll bd_bll = new BD_SupplyBll();
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
        /// 列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetSupplyList(string keyword, string datatype, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<BD_SUPPLY> ListData = bd_bll.GetOrderList(keyword, datatype, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(BD_SUPPLY entity, string ModuleId, string KeyValue)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证
                string sql = "";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from BD_SUPPLY where NAME='{0}' and ID!='{1}'", entity.NAME, KeyValue);
                }
                else
                {
                    sql = String.Format("select count(1) from BD_SUPPLY where NAME='{0}'", entity.NAME);
                }
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "名称已存在" }.ToString());
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
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from BD_SUPPLY where  CODE='{0}'", entity.CODE);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.CODE = this.BillCode();
                    }
                    entity.Create();
                    if (entity.ID.Trim() != entity.CODE.Trim())
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "编号和内码不相同!" }.ToString());
                    }
                    //entity.ID = entity.CODE;
                    entity.ISUSE = "1";
                    entity.SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
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
        [LoginAuthorize]
        public ActionResult SupForm()
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
        [LoginAuthorize]
        public ActionResult SupplyIndex()
        {
            return View();
        }
        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            JsonMessage jsonmsg = CHECK_CUSTOMER_EXISTS(KeyValue);
            if (jsonmsg != null)
            {
                return Content(jsonmsg.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("BD_SUPPLY", "ID", KeyValue);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        [LoginAuthorize]
        public ActionResult HandOrder(string id)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient("SAPWebServiceSoap");
                WEBSERVICES_SAP.RETURN_SAP result = new WEBSERVICES_SAP.RETURN_SAP();
                result = sap.SAP_95ZS(id, "");
                JsonMessage jsonmeg = null;
                if (result.STATUS == "S")
                {
                    string Message = "同步成功。";
                    jsonmeg = new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}:{1}", Message, result.REMSG) };
                }
                else
                {
                    jsonmeg = new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", result.REMSG) };
                }

                return Content(jsonmeg.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }

        /// <summary>
        /// 【供应商管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            BD_SUPPLY entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
