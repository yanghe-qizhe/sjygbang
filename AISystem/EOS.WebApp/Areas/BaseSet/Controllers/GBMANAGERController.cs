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
    public class GBMANAGERController : PublicController<PD_GBMANAGER>
    {
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        PD_GBMANAGERBll bd_bll = new PD_GBMANAGERBll();
        VPD_GBMANAGERBll vbd_bll = new VPD_GBMANAGERBll();
        /// <summary>
        /// 获取自动单据内码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }
        #region 仓库排序
        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GBMANAGERIndex()
        {
            return View();
        }

        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GBMANAGERForm()
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
        /// 列表（返回Json）
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPageListJson(string keyword, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VPD_GBMANAGER> ListData = vbd_bll.GetOrderList(keyword, StartTime, EndTime, jqgridparam);
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
        /// 订单明细列表
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
            PD_GBMANAGER entity = repositoryfactory.Repository().FindEntity(KeyValue);
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
        public ActionResult SubmitOrderForm(PD_GBMANAGER entity, string OrderEntryJson, string ModuleId, string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                //明细行数据
                List<PD_GBMANAGER_DTL> OrderEntryList = OrderEntryJson.JonsToList<PD_GBMANAGER_DTL>();

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    database.Delete<PD_GBMANAGER_DTL>("PK_GBMANAGER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    entity.SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from PD_GBMANAGER where  ID='{0}'", entity.ID);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.ID = this.BillCode();
                    }
                    entity.Create();
                    entity.SPELLCODE = Chinese2PinYin.GetFirstPinYin(entity.NAME);
                    database.Insert(entity, isOpenTrans);
                }

                int index = 1;
                foreach (PD_GBMANAGER_DTL orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.CHECKITEM))
                    {
                        orderentry.Create();
                        orderentry.ID = string.Format("{0}0{1}", entity.ID, index);
                        orderentry.PK_GBMANAGER = entity.ID;
                        database.Insert(orderentry, isOpenTrans);
                        index++;
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
                database.Delete("PD_GBMANAGER", "ID", KeyValue);
                database.Delete("PD_GBMANAGER_DTL", "PK_GBMANAGER", KeyValue);
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
