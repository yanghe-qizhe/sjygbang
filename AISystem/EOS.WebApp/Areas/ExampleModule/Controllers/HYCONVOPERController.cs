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

namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    /// <summary>
    /// 厂内铁水衡转序
    /// </summary>
    public class HYCONVOPERController : PublicController<DP_HYCONVOPER>
    {
        string ModuleId = "";

        QC_ZJZXCYBll ZJZXCYBLL = new QC_ZJZXCYBll();

        DP_HYCONVOPERBll orderbll = new DP_HYCONVOPERBll();
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



        [LoginAuthorize]
        public string GETGLNO(string glno)
        {
            string strJson = "";

            if (!string.IsNullOrEmpty(glno))
            {
                //铁水初始炉号
                BASE_SYSTEMSET entity = new BASE_SYSTEMSET();

                if (glno == "8")
                {
                    entity = DataFactory.Database().FindEntityBySql<BASE_SYSTEMSET>("SELECT * FROM BASE_SYSTEMSET WHERE fieldname='8TSLHConfig'");
                }
                else if (glno == "9")
                {
                    entity = DataFactory.Database().FindEntityBySql<BASE_SYSTEMSET>("SELECT * FROM BASE_SYSTEMSET WHERE fieldname='9TSLHConfig'");
                }
                else if (glno == "10")
                {
                    entity = DataFactory.Database().FindEntityBySql<BASE_SYSTEMSET>("SELECT * FROM BASE_SYSTEMSET WHERE fieldname='10TSLHConfig'");
                }
                string sql = String.Format("select max(STOVENUM) as STOVENUM  from DP_HYCONVOPER where  substr(stovenum,0,1)={0}", glno);
                object obj = DataFactory.Database().FindMaxBySql(sql);
                string docno = "";
                string DocHead = "00001";
                double num = 1.0;
                double n = 0;
                string format = "";
                for (int j = 0; j < DocHead.Length; j++)
                {
                    format = format + "0";
                }
                if (!string.IsNullOrEmpty(obj.ToString()))
                {
                    num = Convert.ToDouble(obj) + 1;
                    docno = string.Format("{0}", num);
                }
                else
                {
                    if (!string.IsNullOrEmpty(entity.FVALUE))
                    {
                        num = Convert.ToDouble(entity.FVALUE) + 1;
                    }
                    DocHead = num.ToString(format);
                    docno = string.Format("{0}{1}", glno, DocHead);
                }
                strJson = docno;// strJson = "{\"billno\":\"" + BillNo + "\"}";
            }
            return strJson;
        }

        /// <summary>
        /// 火运任务单
        /// </summary>
        /// <returns></returns>
        public ActionResult HYBillCOFForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                string billno = this.BillCode();
                ViewBag.BillNo = billno;
                ViewBag.CARSNO = billno.Replace("HF01", "");
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        [LoginAuthorize]
        public ActionResult HYBillCOFIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult HYBillCOFDetail()
        {
            return View();
        }
        /// <summary>
        /// 设计要求销售订单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult HYBillCOFQuery()
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
        [LoginAuthorize]
        public ActionResult GetOrderList(string TaskNo, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_HYCONVOPERBll vorderbll = new VDP_HYCONVOPERBll();
                List<VDP_HYCONVOPER> ListData = vorderbll.GetOrderList(TaskNo, StartTime, EndTime, "", jqgridparam);
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

        [LoginAuthorize]
        public ActionResult GetHYOrderList(string TaskNo, string StartTime, string EndTime, string STOVENUM, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_HYCONVOPERBll vorderbll = new VDP_HYCONVOPERBll();
                List<VDP_HYCONVOPER> ListData = vorderbll.GetOrderList(TaskNo, StartTime, EndTime, STOVENUM, jqgridparam);
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
        [LoginAuthorize]
        public ActionResult GetBHYOrderList(string TaskNo, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VDP_HYCONVOPERBll vorderbll = new VDP_HYCONVOPERBll();
                List<VDP_HYCONVOPER> ListData = vorderbll.GetOrderList(TaskNo, "0", "1", StartTime, EndTime, jqgridparam);
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
        /// 提交订单表单（新增、编辑）
        /// </summary>
        /// <param name="KeyValue">订单主键</param>
        /// <param name="entity">订单实体</param>
        /// <param name="POOrderEntryJson">订单明细</param>
        /// <returns></returns>
        public ActionResult SubmitOrderForm(string KeyValue, string ModuleId, DP_HYCONVOPER entity, string OrderEntryJson)
        {
            entity.Create();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                int i = 1;
                //明细行数据
                List<DP_HYCONVOPER> OrderEntryList = OrderEntryJson.JonsToList<DP_HYCONVOPER>();

                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "变更成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    foreach (DP_HYCONVOPER poorderentry in OrderEntryList)
                    {
                        if (!string.IsNullOrEmpty(poorderentry.STOVENUM))
                        {
                            poorderentry.TASKNO = entity.TASKNO;
                            poorderentry.TASKNOID = string.Format("{0}{1}", entity.TASKNO, i.ToString("00"));
                            //开始时间
                            poorderentry.STARTTIME = entity.STARTTIME;
                            //结束时间
                            poorderentry.ENDTIME = entity.ENDTIME;
                            //发货单位
                            poorderentry.PK_SENDUNIT = entity.PK_SENDUNIT;
                            poorderentry.SENDUNITNAME = entity.SENDUNITNAME;
                            //收货单位
                            poorderentry.PK_ACCEPTUNIT = entity.PK_ACCEPTUNIT;
                            poorderentry.ACCEPTUNITNAME = entity.ACCEPTUNITNAME;
                            //物料
                            poorderentry.PK_MATERIAL = entity.PK_MATERIAL;
                            poorderentry.MATERIALNAME = entity.MATERIALNAME;
                            poorderentry.MATERIALSPEC = entity.MATERIALSPEC;
                            //物料辅助属性
                            poorderentry.PK_MATERIALSPEC = entity.PK_MATERIALSPEC;
                            poorderentry.MATERIALSPECNAME = entity.MATERIALSPECNAME;
                            //秤点
                            poorderentry.PK_SCALE = entity.PK_SCALE;
                            poorderentry.SCALENAME = entity.SCALENAME;
                            //班组
                            poorderentry.PK_WTEAM = entity.PK_WTEAM;
                            poorderentry.WTEAMNAME = entity.WTEAMNAME;
                            poorderentry.MEMO = entity.MEMO;
                            poorderentry.DEF1 = entity.DEF1;
                            poorderentry.DEF2 = entity.DEF2;
                            poorderentry.DEF3 = entity.DEF3;
                            entity.Modify(KeyValue);
                            database.Update(poorderentry, isOpenTrans);
                        }
                    }
                }
                else
                {
                    string UserId = ManageProvider.Provider.Current().UserId;

                    foreach (DP_HYCONVOPER poorderentry in OrderEntryList)
                    {
                        if (!string.IsNullOrEmpty(poorderentry.STOVENUM))
                        {
                            base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                            poorderentry.Create();
                            poorderentry.ISUSE = "0";

                            //任务单
                            poorderentry.TASKNO = entity.TASKNO;
                            poorderentry.TASKNOID = string.Format("{0}{1}", entity.TASKNO, i.ToString("00"));
                            //开始时间
                            poorderentry.STARTTIME = entity.STARTTIME;
                            //结束时间
                            poorderentry.ENDTIME = entity.ENDTIME;
                            //发货单位
                            poorderentry.PK_SENDUNIT = entity.PK_SENDUNIT;
                            poorderentry.SENDUNITNAME = entity.SENDUNITNAME;
                            //收货单位
                            poorderentry.PK_ACCEPTUNIT = entity.PK_ACCEPTUNIT;
                            poorderentry.ACCEPTUNITNAME = entity.ACCEPTUNITNAME;
                            //物料
                            poorderentry.PK_MATERIAL = entity.PK_MATERIAL;
                            poorderentry.MATERIALNAME = entity.MATERIALNAME;
                            poorderentry.MATERIALSPEC = entity.MATERIALSPEC;
                            //物料辅助属性
                            poorderentry.PK_MATERIALSPEC = entity.PK_MATERIALSPEC;
                            poorderentry.MATERIALSPECNAME = entity.MATERIALSPECNAME;
                            //秤点
                            poorderentry.PK_SCALE = entity.PK_SCALE;
                            poorderentry.SCALENAME = entity.SCALENAME;
                            //班组
                            poorderentry.PK_WTEAM = entity.PK_WTEAM;
                            poorderentry.WTEAMNAME = entity.WTEAMNAME;
                            poorderentry.MEMO = entity.MEMO;
                            poorderentry.DEF1 = entity.DEF1;
                            poorderentry.DEF2 = entity.DEF2;
                            poorderentry.DEF3 = entity.DEF3;
                            database.Insert(poorderentry, isOpenTrans);
                            entity.Create();

                            i++;
                        }
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

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {

            //验证
            DP_HYCONVOPER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为未使用的发货单能删除操作！" }.ToString());
            }
            if (entity.ISUSE == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为未发布的发货单能删除操作！" }.ToString());
            }


            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                StringBuilder strsql = new StringBuilder();
                strsql.AppendFormat("DELETE DP_HYCONVOPER WHERE TASKNO IN (SELECT TASKNO FROM DP_HYCONVOPER WHERE ID='{0}' GROUP BY TASKNO) ", KeyValue);
                database.ExecuteBySql(strsql, isOpenTrans);

                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = "删除成功" }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            DP_HYCONVOPER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该数据是发布状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "发布成功。";

                StringBuilder strsql = new StringBuilder();
                strsql.AppendFormat("UPDATE DP_HYCONVOPER SET ISUSE='1' WHERE TASKNO = '{0}' ", entity.TASKNO);
                database.ExecuteBySql(strsql, isOpenTrans);

                //生成铁水转序采样数据
                ZJZXCYBLL.ZJZXCYMOLTENCYSAVEForHYCONVOPER(KeyValue, ref database, ref isOpenTrans);

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
        /// 取消发布
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            DP_HYCONVOPER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该数据是未发布状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证铁水采样数据状态
                ResultMessage message = ZJZXCYBLL.ZJZXCYMOLTENSTATUS_GET(KeyValue, ref database, ref isOpenTrans);
                if (message.Code == 1)
                {
                    StringBuilder strsql = new StringBuilder();
                    strsql.AppendFormat("UPDATE DP_HYCONVOPER SET ISUSE='0' WHERE TASKNO = '{0}'", entity.TASKNO);
                    database.ExecuteBySql(strsql, isOpenTrans);

                    //删除铁水采样数据
                    ZJZXCYBLL.ZJZXCYMOLTEN_DELETE(KeyValue, ref database, ref isOpenTrans);

                    database.Commit();
                    return Content(new JsonMessage { Success = true, Code = "1", Message = "操作成功" }.ToString());

                }
                else
                {
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 【发货单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            DP_HYCONVOPER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        /// <summary>
        /// 订单明细列表（返回Json）
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
                    rows = orderbll.GetOrderEntryList(KeyValue),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }
    }
}
