using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Utilities;
using System.Diagnostics;
using EOS.Business;
using EOS.Entity;
using EOS.DataAccess;
using EOS.Repository;
using System.Data.Common;
using System.Data;
using System.Collections;
using System.Text;

namespace EOS.WebApp.Areas.ZJGCManager.Controllers
{
    public class ZJGCCYController : PublicController<QC_ZJGCCY>
    {
        string ModuleId = "";

        QC_ZJGCCYBll orderbll = new QC_ZJGCCYBll();
        VQC_ZJGCCYBll vorderbll = new VQC_ZJGCCYBll();
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
        ///  
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJGCCYForm()
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
        [LoginAuthorize]
        public ActionResult ZJGCCYForm1()
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
        //采样交样报表
        [LoginAuthorize]
        public ActionResult CYJYReportIndex()
        {
            return View();
        }
        //采样明细报表
        [LoginAuthorize]
        public ActionResult CYReportIndex()
        {
            return View();
        }


        [LoginAuthorize]
        public ActionResult ZJGCCYIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult ZJGCCYIndex1()
        {
            return View();
        }
        //明细
        [LoginAuthorize]
        public ActionResult ZJGCCYDetails()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJGCCYQuery()
        {
            return View();
        }


        /// 订单列表（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BILLNO, string MATERIALNAME, string ICCARD, string TYPE, string ISSEND, string ISZK, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJGCCY> ListData = vorderbll.GetOrderList(BILLNO, MATERIALNAME, ICCARD, TYPE, ISSEND, ISZK, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(string KeyValue, QC_ZJGCCY entity, string ModuleId)
        {
            #region 验证

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    QC_ZJGCCY Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from QC_ZJGCCY where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    entity.PRINTCODE = entity.HANDBILLNO;
                    IsOk = database.Insert(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("过程样单号:{0}，新增成功。", entity.BILLNO);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = entity.BILLNO }.ToString());
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
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm1(string KeyValue, QC_ZJGCCY entity, string ModuleId)
        {
            #region 验证

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int IsOk = 0;

                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    QC_ZJGCCY Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    IsOk = database.Update(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from QC_ZJGCCY where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    entity.PRINTCODE = entity.HANDBILLNO;
                    IsOk = database.Insert(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("过程样单号:{0}，新增成功。", entity.BILLNO);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("QC_ZJGCCY", "BILLNO", KeyValue);

                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        //启用
        [LoginAuthorize]
        public ActionResult StartOrder(string KeyValue)
        {
            //验证
            QC_ZJGCCY entity = repositoryfactory.Repository().FindEntity(KeyValue);


            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "启用成功。";

                entity.MEMO = "启用";
                database.Update(entity, isOpenTrans);

                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                string Account = ManageProvider.Provider.Current().Account;
                string message = "";
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
        /// 制卡
        /// </summary>
        /// <param name="ICNO"></param>
        /// <param name="KeyValue"></param>
        /// <param name="TYPE"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJGCCYMakeCardSave(string ICNO, string KeyID, string TYPE)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(ICNO))
                {
                    if (!string.IsNullOrEmpty(KeyID))
                    {
                        message = orderbll.ZJGCCYMakeCardSave(ICNO, KeyID, TYPE);
                        return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = (message.Code == 1 ? message.Content : message.Message).ToString() }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：异形卡号不能为空" }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：异形卡号不能为空" }.ToString());
                }
            }
            catch (Exception ex)
            {
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
            VQC_ZJGCCY entity = vorderbll.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }




        //采样交接记录
        public ActionResult CYJYDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = vorderbll.CYJYDataSetReport(Parm_Key_Value);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, true);
            }
            return View();
        }

        //采样明细
        public ActionResult CYDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = vorderbll.CYDataSetReport(Parm_Key_Value);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, true);
            }
            return View();
        }
    }
}
