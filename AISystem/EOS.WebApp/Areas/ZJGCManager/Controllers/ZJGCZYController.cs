using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using System.Data;
using Newtonsoft.Json.Linq;
using EOS.DataAccess;
using System.Data.Common;
using EOS.Repository;

namespace EOS.WebApp.Areas.ZJGCManager.Controllers
{


    public class ZJGCZYController : PublicController<QC_ZJGCCY>
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

        private string BillCode(string ModuleId)
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }

        #region 页面
        /// <summary>
        /// 发货通知单表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJGCZYForm()
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
        public ActionResult ZJGCZYIndex()
        {
            return View();
        }
        //明细
        [LoginAuthorize]
        public ActionResult ZJGCZYDetail()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJGCZYQuery()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult SelectJYCYData()
        {
            ViewBag.SYUSER = ManageProvider.Provider.Current().UserName;
            return View();
        }
        [LoginAuthorize]
        public ActionResult SelectJYCYSGData()
        {
            ViewBag.SYUSER = ManageProvider.Provider.Current().UserName;
            return View();
        }


        //制样明细报表
        [LoginAuthorize]
        public ActionResult ZYReportIndex()
        {
            return View();
        }
        #endregion

        /// 订单列表（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BILLNO, string PRINTCODE, string MATERIALNAME, string ICCARD, string TYPE, string ISSEND, string ISZK, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJGCCY> ListData = vorderbll.GetOrderListZY(BILLNO, PRINTCODE, MATERIALNAME, ICCARD, TYPE, ISSEND, ISZK, StartTime, EndTime, jqgridparam);
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
                    QC_ZJGCCY editentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    editentity.Modify(KeyValue);
                    editentity.MEMO = entity.MEMO;
                    IsOk = database.Update(editentity, isOpenTrans);
                    this.WriteLog(IsOk, entity, editentity, KeyValue, Message);
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
                    IsOk = database.Insert(entity, isOpenTrans);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("过程样单号:{0}，{1}。", entity.BILLNO, Message);
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
        /// 鑫跃
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="JYUSER"></param>
        /// <param name="JYUSER1"></param>
        /// <param name="SYUSER"></param>
        /// <param name="SYUSER1"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJGCZYSaveFinish(string KeyValue, string JYUSER, string JYUSER1, string SYUSER, string SYUSER1, string ModuleId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "操作成功。";
                this.ModuleId = ModuleId;
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    #region 验证
                    QC_ZJGCCY editentity = repositoryfactory.Repository().FindEntity(KeyValue);
                    if (editentity.ISZY == "1")
                    {
                        string msg = string.Format("采样单号:{0}已制样!", editentity.BILLNO);
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = msg }.ToString());
                    }
                    #endregion

                    #region 生成单号
                    WB_MATERIAL_JYPBll jyp_bll = new WB_MATERIAL_JYPBll();
                    WB_MATERIAL_JYP jyp_entity = jyp_bll.GetEntity(editentity.MATERIAL);
                    string BILLNO = "", SBILLNO = "", MYBILLNO = "", YBILLNO = "", GBILLNO = "";

                    //base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    //BILLNO = this.BillCode();

                    #endregion

                    #region 更新采样单
                    editentity.Modify(KeyValue);
                    editentity.ISZY = "1";
                    editentity.TYPE = "4";
                    //editentity.PRINTCODE = BILLNO;

                    editentity.JYUSER = JYUSER;
                    editentity.JYUSER1 = JYUSER1;
                    editentity.SYUSER = SYUSER;
                    editentity.SYUSER1 = SYUSER1;
                    string JYDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    editentity.JYDATE = JYDATE;
                    editentity.JYDATE = JYDATE;
                    editentity.ZYUSER = ManageProvider.Provider.Current().UserName;
                    editentity.ZYDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    database.Update(editentity, isOpenTrans);
                    #endregion
                    database.Commit();
                    return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中采样单号！" }.ToString());
                }
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 总厂
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="JYUSER"></param>
        /// <param name="JYUSER1"></param>
        /// <param name="SYUSER"></param>
        /// <param name="SYUSER1"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJGCZYSaveFinish1(string KeyValue, string JYUSER, string JYUSER1, string SYUSER, string SYUSER1, string ModuleId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "操作成功。";
                this.ModuleId = ModuleId;
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    QC_ZJGCCY entity = repositoryfactory.Repository().FindEntity(KeyValue);
                    if (entity.ISZY == "1")
                    {
                        string msg = string.Format("采样单号:{0}已制样!", entity.BILLNO);
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = msg }.ToString());
                    }
                    WB_MATERIAL_JYPBll jyp_bll = new WB_MATERIAL_JYPBll();
                    WB_MATERIAL_JYP jyp_entity = jyp_bll.GetEntity(entity.MATERIAL);
                    string BILLNO = "", SBILLNO = "", MYBILLNO = "", YBILLNO = "", GBILLNO = "";
                    //是否明码
                    if (jyp_entity.ISMM == "1")
                    {
                        BILLNO = entity.BILLNO;
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(jyp_entity.BIRTHCODE))
                        {
                            BILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, entity.BILLNO.Substring(2));
                        }
                        else
                        {
                            base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                            BILLNO = this.BillCode();
                        }
                    }
                    #region 更新
                    entity.Modify(KeyValue);
                    entity.ISZY = "1";
                    entity.TYPE = "4";
                    entity.PRINTCODE = BILLNO;
                    entity.JYUSER = JYUSER;
                    entity.JYUSER1 = JYUSER1;
                    entity.SYUSER = SYUSER;
                    entity.SYUSER1 = SYUSER1;
                    string JYDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    entity.JYDATE = JYDATE;
                    entity.JYDATE = JYDATE;
                    entity.ZYUSER = ManageProvider.Provider.Current().UserName;
                    entity.ZYDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    database.Update(entity, isOpenTrans);
                    database.Commit();
                    #endregion

                    return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());

                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中采样单号！" }.ToString());
                }
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }




        /// <summary>
        /// 取消送检
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult UNSendCheck(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = orderbll.UNSendCheck(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 送检
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CheckCYData(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = orderbll.SendCYData(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());

                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 送检
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CheckCYData1(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = orderbll.SendCYData(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());

                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 取消制样
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult UNCheck(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = orderbll.UNCheck(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }


        #region 鑫跃打印制样码

        public ActionResult ZYPrintCode(string KeyValue)
        {

            DataSet dataSet = vorderbll.ZYPrintCode(KeyValue, "1");
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, false);
            }
            return View();
        }

        public ActionResult BDZYPrintCode(string Parm_Key_Value)
        {

            DataSet dataSet = vorderbll.BDZYPrintCode(Parm_Key_Value, "1");
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, false);
            }
            return View();
        }


        #endregion

        #region 总厂打印制样码



        public ActionResult BDZYPrintCode1(string Parm_Key_Value)
        {

            DataSet dataSet = vorderbll.BDZYPrintCode(Parm_Key_Value, "2");
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, true);
            }
            return View();
        }

        public ActionResult ZYPrintCode1(string KeyValue)
        {
            DataSet dataSet = vorderbll.ZYPrintCode(KeyValue, "2");
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, true);
            }
            return View();
        }
        #endregion


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


        public ActionResult ZYDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = vorderbll.ZYDataSetReport(Parm_Key_Value);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, true);
            }
            return View();
        }
    }
}
