using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EOS.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// 接口日志处理
    /// </summary>
    public class IFLogsController : PublicController<IF_LOG>
    {
        string ModuleId = "";
        IF_LOGBll orderbll = new IF_LOGBll();
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        /// <summary>
        /// 获取自动单据编码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }
        /// <summary>
        /// 接口日志表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult IFBillCOFForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        //列表
        [LoginAuthorize]
        public ActionResult IFLogIndex()
        {
            return View();
        }
        //明细
        [LoginAuthorize]
        public ActionResult IFLogDetail()
        {
            return View();
        }
        //删除
        [LoginAuthorize]
        public ActionResult IFLogDelete()
        {
            return View();
        }
        //查询
        [LoginAuthorize]
        public ActionResult IFLogQuery()
        {
            return View();
        }
        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="BillNo">制单编号</param>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetOrderList(string TaskNo, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<IF_LOG> ListData = orderbll.GetOrderList(TaskNo, StartTime, EndTime, jqgridparam);
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
        /// 接口查询
        /// </summary>
        /// <param name="TaskNo"></param>
        /// <param name="IFCode"></param>
        /// <param name="IFType"></param>
        /// <param name="IFInsatu"></param>
        /// <param name="IFRequest"></param>
        /// <param name="IFRespone"></param>
        /// <param name="IFMsg"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetSeOrderList(string TaskNo, string IFCode, string IFType, string IFInsatu, string IFRequest, string IFRespone, string IFMsg, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<IF_LOG> ListData = orderbll.GetSeOrderList(TaskNo, IFCode, IFType, IFInsatu, IFRequest, IFRespone, IFMsg, StartTime, EndTime, jqgridparam);
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
        /// 接口删除
        /// </summary>
        /// <param name="IF_CODE"></param>
        /// <param name="IF_INSATU"></param>
        /// <param name="IF_TYPE"></param>
        /// <param name="EndTime"></param>
        /// <param name="StartTime"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteIFLog(string IF_CODE, string IF_INSATU, string IF_TYPE, string EndTime, string StartTime)
        {
            string Message ="接口日志删除失败";
            if (string.IsNullOrEmpty(IF_CODE) || IF_CODE == "&nbsp;") { IF_CODE = ""; }
            if (string.IsNullOrEmpty(IF_INSATU) || IF_INSATU == "&nbsp;") { IF_INSATU = ""; }
            if (string.IsNullOrEmpty(IF_TYPE) || IF_TYPE == "&nbsp;") { IF_TYPE = ""; }
            try
            {
                int isdelok = orderbll.DeleteLog(StartTime, EndTime, IF_CODE, IF_INSATU, IF_TYPE);
                if (isdelok>=0)
                {
                    Message = "接口日志删除成功";
                }
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
        //停止接口传输
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            IF_LOG entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.DY_OS != "MQIS")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "外部触发无法停止！" }.ToString());
            }
            if (entity.IF_IN_SATU == "成功")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已传输成功，无需停用！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = entity.BILL_NO + entity.IF_NAME + "接口停止传输成功。";
                string MemoLog =DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")+ManageProvider.Provider.Current().UserName+"人工停止传输";
                StringBuilder strSql = new StringBuilder();
                switch (entity.IF_NAME)
                {
                    case "汽运采购到货":
                        strSql.Append(string.Format("update WB_WEIGHT set UPLOAD='{1}',MEMO='{2}'" +
                    " where BILLNO='{0}'", entity.BILL_NO, "1", MemoLog));
                        break;
                    case "汽运采购质检":
                        strSql.Append(string.Format("update WB_WEIGHT set UPLOAD='{1}',MEMO='{2}'" +
                    " where BILLNO='{0}'", entity.BILL_NO, "1", MemoLog));
                        break;
                    case "入厂信息":
                        strSql.Append(string.Format("update WB_WEIGHT_Task set IS_UPINDATE='{1}',MEMO='{2}' where PK_TASK='{0}'" +
                    " where BILLNO='{0}'", entity.BILL_NO, "1", MemoLog));
                        break;
                    default:
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未找到对应单据接口停用方法！" }.ToString());
                }
                DataFactory.Database().ExecuteBySql(strSql, isOpenTrans);
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
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            IF_LOG entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
