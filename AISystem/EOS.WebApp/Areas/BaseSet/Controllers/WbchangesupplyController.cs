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
    public class WbchangesupplyController : PublicController<WB_CHANGESUPPLY>
    {
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        WB_CHANGESUPPLYBll bd_driverbll = new WB_CHANGESUPPLYBll();
        /// <summary>
        /// 获取自动单据内码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }

        //列表
        [LoginAuthorize]
        public ActionResult WbchangesupplyIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SELECTPOORDERS()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SELECTPOORDERSNew()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult WbchangesupplyForm()
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
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public override ActionResult Form()
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
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>

        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetDriversList(string VBILLCODEOLD, string VBILLCODENEW, string PK_SUPPLYOLD, string MATERIALNAMEOLD, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<WB_CHANGESUPPLY> ListData = bd_driverbll.GetOrderList(VBILLCODEOLD, VBILLCODENEW, PK_SUPPLYOLD, MATERIALNAMEOLD, jqgridparam);
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
        public ActionResult SubmitOrderForm(WB_CHANGESUPPLY entity, string ModuleId, string KeyValue)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证
                string sql = "";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from WB_CHANGESUPPLY where VBILLCODEOLD='{0}' and WATERNUM!='{1}'", entity.VBILLCODEOLD, KeyValue);
                }
                else
                {
                    sql = String.Format("select count(1) from WB_CHANGESUPPLY where VBILLCODEOLD='{0}'", entity.VBILLCODEOLD);
                }
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "原订单编号已存在" }.ToString());
                }

                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    entity.WATERNUM = entity.WATERNUM.ToUpper();
                    //entity.SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
                    database.Update(entity, isOpenTrans);
                }
                else
                {

                    //base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from WB_CHANGESUPPLY where  VBILLCODEOLD='{0}'", entity.VBILLCODEOLD);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
           
                    }
                    entity.Create();
                    entity.WATERNUM = entity.WATERNUM.ToUpper();
                    //entity.SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
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

        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证

            JsonMessage jsonmsg = CHECK_DRIVERS_EXISTS(KeyValue);
            if (jsonmsg != null)
            {
                return Content(jsonmsg.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("WB_CHANGESUPPLY", "WATERNUM", KeyValue);
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
        /// 【司机管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            WB_CHANGESUPPLY entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
