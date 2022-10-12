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
using Newtonsoft.Json.Linq;
using System.Threading;
using EOS.Business.ZJSettlement;
using EOS.Entity.ZJSettlement;
using Newtonsoft.Json;


namespace EOS.WebApp.Areas.ZJSettlement.Controllers
{
    public class ResultJson
    {
        public string Flag { get; set; }
        public string Message { get; set; }
    }

    public class SUMKJRESULTMEMO
    {
        public string ZJITEM { get; set; }
        public string ITEMNAME { get; set; }
        public decimal ZJSUTTLE { get; set; }
        public decimal BDSUTTLE { get; set; }
        public decimal KS { get; set; }
        public string ZJVALUE { get; set; }
    }
    public class PCRawYLJSController : PublicController<DP_PSORDER>
    {
        string ModuleId = "";
        VDP_PSORDERBll orderbll = new VDP_PSORDERBll();

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

        #region 页面返回

        public ActionResult JSReportIndex()
        {
            ViewBag.CreateUser = ManageProvider.Provider.Current().UserName;
            return View();
        }
        public ActionResult JSReportIndex1()
        {
            ViewBag.CreateUser = ManageProvider.Provider.Current().UserName;
            return View();
        }
        public ActionResult JSReportIndex3()
        {
            ViewBag.CreateUser = ManageProvider.Provider.Current().UserName;
            return View();
        }
        public ActionResult JSReportIndex4()
        {
            ViewBag.CreateUser = ManageProvider.Provider.Current().UserName;
            return View();
        }

        public ActionResult SELECTCHECKUSER()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawYLJSList()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawYLJSList1()
        {
            ViewBag.CreateUser = ManageProvider.Provider.Current().UserName;
            return View();
        }
        //万国列表显示隐藏单价金额 
        [LoginAuthorize]
        public ActionResult PCRawYLJSList2()
        {
            ViewBag.CreateUser = ManageProvider.Provider.Current().UserName;
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawYLJSForm()
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
        public ActionResult PCRawYLJSForm2()
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
        //打印
        [LoginAuthorize]
        public ActionResult PCRawYLJSPrint()
        {
            return View();
        }
        //打印
        [LoginAuthorize]
        public ActionResult PCRawYLJSPrint2()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SELECTWEIGHTS()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult SELECTPOORDERS()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult PCRawYLJSAddCar()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawSELECTWEIGHTSAddCar()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult PCRawYLJSDeleteCar()
        {
            return View();
        }

        #endregion

        #region 数据列表
        /// <summary>
        /// 获取原料结算列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderList(string BILLNO, string VBILLCODE, string PK_RECEIVEORG, string SUPPLY, string MATERIAL, string WEIGHTNO, string CREATEUSER, string HZUSERNAME, string ISTYPE, string CARS, string PK_RECEIVESTORE, string SOURCEBILLNO, string STATUS, string ISUSE, string UPLOAD, string CQDEF1, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VDP_PSORDER> ListData = orderbll.GetOrderList(BILLNO, VBILLCODE, PK_RECEIVEORG, SUPPLY, MATERIAL, WEIGHTNO, CREATEUSER, HZUSERNAME, ISTYPE, CARS, PK_RECEIVESTORE, SOURCEBILLNO, STATUS, ISUSE, UPLOAD, CQDEF1, StartTime, EndTime, jqgridparam);
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

        [HttpPost]
        public string GetBillCode(string strCode, string strDate, string ModuleId, string type)
        {
            //  string UserId = ManageProvider.Provider.Current().UserId;
            // string docno =  base_coderulebll.GetBillCode(UserId, ModuleId, string.Format("J{0}", strCode), strDate);
            string docno = orderbll.GetBillNo(strCode, strDate);
            string strJson = type == "0" ? "{\"billno\":\"" + docno + "\"}" : docno;
            return strJson;
        }


        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            DP_PSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        [HttpPost]
        public ActionResult GetOrderEntity(string KeyValue)
        {
            VDP_PSORDER entity = orderbll.GetOrderEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        //记忆
        [HttpPost]
        public ActionResult GetJYOrderEntity()
        {
            string createuser = ManageProvider.Provider.Current().UserName;
            VDP_PSORDER entity = orderbll.GetJYOrderEntity(createuser);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        /// <summary>
        /// 主表PK获取明细信息
        /// </summary>
        /// <param name="KeyValue"></param>
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

        /// <summary>
        /// 主表PK获取分页明细信息
        /// </summary>
        /// <param name="MAINID"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderListItems(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<DP_PSORDER_B> ListData = orderbll.GetOrderListItem(MAINID, jqgridparam);
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

        /// 采购结算磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderWeightList(string BillNo, string VBILLCODE, string DHBillNo, string Supply, string matclass, string Material, string Cars, string WEIGHTNO, string BRANDMODEL, string PK_RECEIVEORG, string PK_RECEIVESTORE, string TRAFFICCOMPANY, string HZUSERNAME, string Status, string UPLOAD, string ISJS, string ZJSTATUS, string DATETYPE, string CQDEF11, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VWB_WEIGHT_BIN_BSBll vorderbll = new VWB_WEIGHT_BIN_BSBll();
                List<VWB_WEIGHT_BIN_BS> ListData = vorderbll.GetRKOrderListNew(BillNo, VBILLCODE, DHBillNo, Supply, matclass, Material, Cars, Status, UPLOAD, WEIGHTNO, BRANDMODEL, PK_RECEIVEORG, PK_RECEIVESTORE, TRAFFICCOMPANY, HZUSERNAME, "0", ZJSTATUS, DATETYPE, CQDEF11, StartTime, EndTime, jqgridparam);
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

        //审批 
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            DP_PSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.AUDITSTATUS == 1)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该结算单是审批状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                entity.AUDITSTATUS = 1;
                database.Update(entity, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        //反审
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            DP_PSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.AUDITSTATUS == 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该结算单是未审批状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "反审成功。";
                entity.AUDITSTATUS = 0;
                database.Update(entity, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //作废
        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            #region  //验证

            string sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and ISUSE=4", bills);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有作废数据,无法继续作废的操作！" }.ToString());
            }

            sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and UPLOAD=1", bills);
            res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有已上传数据,无法继续作废的操作！" }.ToString());
            }
            int ZCUser = ManageProvider.Provider.Current().ZCUser;
            if (ZCUser == 0)
            {
                sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and CREATEUSER!='{1}'", bills, ManageProvider.Provider.Current().UserName);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单创建人与当前用户不符,无法继续作废的操作！" }.ToString());
                }
            }
            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "作废成功。";
                //主表 
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update DP_PSORDER set ISUSE=4,MEMO='{0}' where BILLNO in('{1}')", "作废", bills));
                database.ExecuteBySql(strSql, isOpenTrans);
                //明细
                strSql.Clear();
                strSql.Append(string.Format("update DP_PSORDER_B set ISUSE='4',MEMO='作废',DELETEDATE='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',DELETEUSER='" + ManageProvider.Provider.Current().UserName + "' where PK_PSORDER in('{0}')", bills));
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

        //删除
        [LoginAuthorize]
        public ActionResult DeleteItem(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            #region  //验证

            string sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and ISUSE=0", bills);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有未作废数据,无法继续删除的操作！" }.ToString());
            }

            sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and UPLOAD=1", bills);
            res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有已上传数据,无法继续删除的操作！" }.ToString());
            }
            int ZCUser = ManageProvider.Provider.Current().ZCUser;
            if (ZCUser == 0)
            {
                sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and CREATEUSER!='{1}'", bills, ManageProvider.Provider.Current().UserName);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单创建人与当前用户不符,无法继续删除的操作！" }.ToString());
                }
            }
            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update wb_weight_b set ISJS='0' where BILLNO in(select billno from DP_PSORDER_B where PK_PSORDER in('{0}'))", bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("delete DP_PSORDER where BILLNO in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("delete DP_PSORDER_B where PK_PSORDER in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("结算单删除成功。");
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

        #endregion

        #region 表单提交
        /// 随机号提交表单 
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="PCRAWCYDetailJson">子表数据</param>
        /// <returns></returns>
        public ActionResult SubmitOrderForm(string KeyValue, DP_PSORDER entity, string OrderEntryJson, string ModuleId)
        {
            string sql = "";
            int res = 0;
            #region 验证
            decimal CHECKSUTTLE = entity.JSSUTTLE + entity.JJSUTTLE + entity.ZJSUTTLE + entity.TZSUTTLE - entity.BDSUTTLE;
            if (!string.IsNullOrEmpty(entity.PDAKZ2))
            {
                CHECKSUTTLE = CHECKSUTTLE + (Convert.ToDecimal(entity.PDAKZ2));
            }
            if (entity.SUTTLE > CHECKSUTTLE)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("结算单:{0}计算不符，请验证后保存！", entity.BILLNO) }.ToString());
            }
            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<DP_PSORDER_B> OrderEntryList = OrderEntryJson.JonsToList<DP_PSORDER_B>();
                string[] strArr = OrderEntryList.Where(x => string.IsNullOrEmpty(x.BILLNO) == false).Select(y => y.BILLNO).ToArray();
                string bills = String.Join("','", strArr);
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from DP_PSORDER_B where  BILLNO in('{0}') and PK_PSORDER!='{1}'", bills, KeyValue);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "结算明细中存在已结算的计量单,请您重新选择结算！" }.ToString());
                    }
                    if (!ManageProvider.Provider.Current().IsSystem)
                    {
                        int ZCUser = ManageProvider.Provider.Current().ZCUser;
                        if (ZCUser == 0 && entity.CREATEUSER != ManageProvider.Provider.Current().UserName)
                        {
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("结算单号:{0}，创建人为:{1}，您没有权限修改！", entity.BILLNO, entity.CREATEUSER) }.ToString());
                        }
                    }
                    strSql.Clear();
                    strSql.Append(string.Format("update wb_weight_b set ISJS='0' where BILLNO in(select billno from DP_PSORDER_B where PK_PSORDER='{0}')", KeyValue));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    database.Delete<DP_PSORDER_B>("PK_PSORDER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    sql = String.Format("select count(1) from DP_PSORDER_B where  BILLNO in('{0}')", bills);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "结算明细中存在已结算的计量单,请您重新选择结算！" }.ToString());
                    }
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from DP_PSORDER where  BILLNO='{0}'", entity.BILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = orderbll.GetBillNo(entity.WEIGHTNO, entity.JSDATE);
                    }
                    entity.Create();
                    sql = String.Format("select count(1) from DP_PSORDER where  BILLNO='{0}'", entity.BILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "结算单号已存在,请您重新保存操作！" }.ToString());
                    }
                    database.Insert(entity, isOpenTrans);
                }

                //处理明细
                int index = 1;
                foreach (DP_PSORDER_B orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.BILLNO))
                    {

                        //结算明细
                        orderentry.Create();
                        orderentry.PK_PSORDER = entity.BILLNO;
                        orderentry.MEMO = orderentry.MEMO.Replace("null", "");
                        orderentry.JSDATE = entity.JSDATE;
                        database.Insert(orderentry, isOpenTrans);
                        //更新是否过磅表中结算状态
                        strSql.Clear();
                        strSql.Append(string.Format("update WB_WEIGHT_B set ISJS=1 where BILLNO='{0}'", orderentry.BILLNO));
                        database.ExecuteBySql(strSql, isOpenTrans);

                        index++;
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("结算单号:{0}，新增成功。", entity.BILLNO);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message, BillNo = entity.BILLNO }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public ActionResult CheckData(string OrderEntryJson, string ModuleId)
        {
            string KSMEMO = "";
            List<SUMKJRESULTMEMO> kjlist = new List<SUMKJRESULTMEMO>();
            List<VWB_WEIGHT_BIN_BS> list = new List<VWB_WEIGHT_BIN_BS>();
            VWB_WEIGHT_BIN_BS model = null;
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<VWB_WEIGHT_BIN_BS> OrderEntryList = OrderEntryJson.JonsToList<VWB_WEIGHT_BIN_BS>();
                //处理明细
                foreach (VWB_WEIGHT_BIN_BS orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.BILLNO))
                    {
                        model = GetWeightEneity(orderentry, ref kjlist, ref KSMEMO);
                        list.Add(model);
                    }
                }
                string SUMKJRESULTMEMO = "";
                if (kjlist.Count > 0)
                {
                    var SumKJData = kjlist.GroupBy(x => new { x.ZJITEM, x.ITEMNAME }).Select(y => new
                    {
                        ZJITEM = y.Key.ZJITEM,
                        ITEMNAME = y.Key.ITEMNAME,
                        ZJSUTTLE = y.Sum(x => x.ZJSUTTLE),
                        KS = y.Sum(x => x.KS)
                    });

                    foreach (var item in SumKJData)
                    {
                        if (item.KS > 0)
                        {
                            //if (item.ZJSUTTLE != 0)
                            // {
                            SUMKJRESULTMEMO += string.Format("项目：{0},扣量：{1};加扣：{2};", item.ITEMNAME, item.ZJSUTTLE, item.KS);
                            // }
                        }
                        else
                        {
                            //  if (item.ZJSUTTLE != 0)
                            //{
                            SUMKJRESULTMEMO += string.Format("项目：{0},扣量：{1};", item.ITEMNAME, item.ZJSUTTLE);
                            //}
                        }
                    }
                }
                var JsonData = new
                {
                    KJRESULTMEMO = string.Format("{0}", SUMKJRESULTMEMO.TrimEnd(';')),
                    rows = list
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public VWB_WEIGHT_BIN_BS GetWeightEneity(VWB_WEIGHT_BIN_BS entity, ref List<SUMKJRESULTMEMO> kjlist, ref string KSMEMO)
        {
            
            List<SUMKJRESULTMEMO> kjlist1 = new List<SUMKJRESULTMEMO>();
            string KJRESULTMEMO = "";
            if (entity != null)
            {
                #region 物料-是否补点
                bool isbd = false;
                BD_MaterialBll matbll = new BD_MaterialBll();
                VBD_MATERIAL mat = matbll.GetEntity(entity.PK_MATERIAL);
                if (mat != null)
                {
                    if (mat.ISBD == 1)
                    {
                        isbd = true;
                    }
                }
                #endregion
                //获取结算磅单化验项化验值
                VBD_ZJRESULTBll vorderbll = new VBD_ZJRESULTBll();
                List<VBD_ZJRESULTDETAILS> ListData = vorderbll.GetOrderEntryList1(entity.PK_ORDER_B);

                //ZJBZRESULT:0合格，1 不合格
                //后重公式 
                PD_GBMANAGER_DTL gsentity = null;
                PD_GBMANAGER_DTL gsentity1 = null;//补点公式
                List<PD_GBMANAGER_DTL> list = null;
                SUMKJRESULTMEMO kjresult = null;
                string EXPRESSIONS = "0", EXPRESSIONS_JS = "0", EXPRESSIONS_JS1 = "0", JS_SQL = "";
                DataTable dt = null;
                DataRow row = null;
                string ZJVAL = "0";
                foreach (VBD_ZJRESULTDETAILS zjentity in ListData)
                {
                    if (string.IsNullOrEmpty(zjentity.ZJVALUE))
                    {
                        zjentity.ZJVALUE = "0";
                    }
                    kjresult = new SUMKJRESULTMEMO();
                    //扣重公式
                    //gsentity = GetGBMANAGER_DTL(entity.PK_SENDORG, entity.PK_MATERIAL, zjentity.ZJITEM, zjentity.ZJVALUE, "0");
                    if (zjentity.ITEMNAME == "杂质%")
                    {
                        ZJVAL = string.Format("{0}", (zjentity.KS + Convert.ToDecimal(zjentity.ZJVALUE)));
                        list = GetGBMANAGER_DTL1(entity.PK_SENDORG, entity.PK_MATERIAL, zjentity.ZJITEM, ZJVAL);
                    }
                    else
                    {
                        ZJVAL = zjentity.ZJVALUE;
                        list = GetGBMANAGER_DTL1(entity.PK_SENDORG, entity.PK_MATERIAL, zjentity.ZJITEM, ZJVAL);
                    }

                    if (list.Count > 0)
                    {
                        decimal SUTTLE = entity.SUTTLE;
                        if (entity.PDAKZ2 > 0)
                        {
                            SUTTLE = Math.Round((entity.SUTTLE - entity.PDAKZ2), 3, MidpointRounding.AwayFromZero);
                        }
                        if (entity.COMPUTERTYPE != "112" && entity.SUTTLE < 0)
                        {
                            SUTTLE = 0;
                        }
                        #region 公式计算
                        gsentity = list.Where(p => p.ISBD == "0").FirstOrDefault();
                        if (gsentity != null)
                        {
                            //扣减公式
                            EXPRESSIONS = gsentity.EXPRESSIONS;
                            if (!string.IsNullOrEmpty(EXPRESSIONS))
                            {
                                EXPRESSIONS_JS = EXPRESSIONS.Replace("@净重@", SUTTLE.ToString()).Replace("@净重", SUTTLE.ToString()).Replace("@检验项目@", ZJVAL).Replace("检验项目", ZJVAL).Replace("（", "(").Replace("）", ")");

                                //if (zjentity.ITEMNAME == "杂质%")
                                //{
                                //    EXPRESSIONS_JS = EXPRESSIONS.Replace("@净重@",  SUTTLE.ToString()).Replace("@净重", SUTTLE.ToString()).Replace("@检验项目@", ZJVAL).Replace("检验项目", ZJVAL).Replace("（", "(").Replace("）", ")");
                                //}
                                //else
                                //{
                                //    //entity.PDAKZ2
                                //    EXPRESSIONS_JS = EXPRESSIONS.Replace("@净重@", SUTTLE.ToString()).Replace("@净重", SUTTLE.ToString()).Replace("@检验项目@", zjentity.ZJVALUE).Replace("检验项目", zjentity.ZJVALUE).Replace("（", "(").Replace("）", ")");
                                //}
                            }
                        }
                        #endregion

                        #region 补点公式计算
                        if (isbd == true)
                        {
                            #region  补点
                            //gsentity1 = GetGBMANAGER_DTL(entity.PK_SENDORG, entity.PK_MATERIAL, zjentity.ZJITEM, zjentity.ZJVALUE, "1");
                            gsentity1 = list.Where(p => p.ISBD == "1").FirstOrDefault();
                            if (gsentity1 != null)
                            {
                                EXPRESSIONS = gsentity1.EXPRESSIONS;//补点公式
                                if (!string.IsNullOrEmpty(EXPRESSIONS))
                                {
                                    //补点
                                    EXPRESSIONS_JS1 = EXPRESSIONS.Replace("@净重@", SUTTLE.ToString()).Replace("@净重", SUTTLE.ToString()).Replace("@检验项目@", ZJVAL).Replace("检验项目", ZJVAL);
                                }
                            }
                            #endregion
                        }
                        #endregion

                        #region 质检扣重 补点重量
                        JS_SQL = string.Format("select round({0},4) as ZJSUTTLE,round({1},4) as BDSUTTLE from VWB_WEIGHT_BIN_BS where BILLNO='{2}'", EXPRESSIONS_JS, EXPRESSIONS_JS1, entity.BILLNO);
                        dt = DataFactory.Database().FindTableBySql(JS_SQL);

                        if (dt != null)
                        {
                            row = dt.Rows[0];
                            kjresult.ZJSUTTLE = 0;
                            if (entity.SUTTLE > 0 && Convert.ToDecimal(row["ZJSUTTLE"]) > 0)
                            {
                                kjresult.ZJSUTTLE = Math.Round(Convert.ToDecimal(row["ZJSUTTLE"]), 3, MidpointRounding.AwayFromZero);
                            }
                            else if (entity.SUTTLE < 0 && Convert.ToDecimal(row["ZJSUTTLE"]) < 0)
                            {
                                kjresult.ZJSUTTLE = Math.Round(Convert.ToDecimal(row["ZJSUTTLE"]), 3, MidpointRounding.AwayFromZero);
                            }
                            kjresult.BDSUTTLE = 0;
                            if (Convert.ToDecimal(row["BDSUTTLE"]) > 0)
                            {
                                kjresult.BDSUTTLE = Math.Round(Convert.ToDecimal(row["BDSUTTLE"]), 3, MidpointRounding.AwayFromZero);
                            }
                            kjresult.KS = 0;
                            if (zjentity.ITEMNAME == "杂质%")
                            {
                                kjresult.KS = zjentity.KS;
                            }
                            kjresult.ZJVALUE = zjentity.ZJVALUE;
                        }
                        EXPRESSIONS_JS = "0";
                        EXPRESSIONS_JS1 = "0";
                        #endregion
                    }
                    else
                    {
                        kjresult.KS = 0;
                        kjresult.BDSUTTLE = 0;
                        kjresult.ZJSUTTLE = 0;
                        kjresult.ZJVALUE = "0";
                    }
                    kjresult.ZJITEM = zjentity.ZJITEM;
                    kjresult.ITEMNAME = zjentity.ITEMNAME;
                    kjlist.Add(kjresult);
                    kjlist1.Add(kjresult);
                }
            }
            if (kjlist1.Count > 0)
            {
                entity.ZJSUTTLE = kjlist1.Sum(p => p.ZJSUTTLE);
                entity.BDSUTTLE = kjlist1.Sum(p => p.BDSUTTLE);
            }
            else
            {
                entity.ZJSUTTLE = 0;
                entity.BDSUTTLE = 0;
            }

            if (!string.IsNullOrEmpty(entity.DEF1))
            {
                if (entity.DEF1.Contains("千克"))
                {
                    if (entity.YFSUTTLE > 100)
                    {
                        entity.YFSUTTLE = (entity.YFSUTTLE / 1000) * 1000;
                    }
                    else
                    {
                        entity.YFSUTTLE = entity.YFSUTTLE * 1000;
                    }

                    if (entity.GROSS > 100)
                    {
                        entity.GROSS = (entity.GROSS / 1000) * 1000;
                    }
                    else
                    {
                        entity.GROSS = entity.GROSS * 1000;
                    }
                    if (entity.TARE > 100)
                    {
                        entity.TARE = (entity.TARE / 1000) * 1000;
                    }
                    else
                    {
                        entity.TARE = entity.TARE * 1000;
                    }

                    if (entity.SUTTLE > 100)
                    {
                        entity.SUTTLE = (entity.SUTTLE / 1000) * 1000;
                    }
                    else
                    {
                        entity.SUTTLE = entity.SUTTLE * 1000;
                    }

                    if (entity.JJSUTTLE > 100)
                    {
                        entity.JJSUTTLE = (entity.JJSUTTLE / 1000) * 1000;
                    }
                    else
                    {
                        entity.JJSUTTLE = entity.JJSUTTLE * 1000;
                    }
                    if (entity.ZJSUTTLE > 100)
                    {
                        entity.ZJSUTTLE = (entity.ZJSUTTLE / 1000) * 1000;
                    }
                    else
                    {
                        entity.ZJSUTTLE = entity.ZJSUTTLE * 1000;
                    }
                    if (entity.TZSUTTLE > 100)
                    {
                        entity.TZSUTTLE = (entity.TZSUTTLE / 1000) * 1000;
                    }
                    else
                    {
                        entity.TZSUTTLE = entity.TZSUTTLE * 1000;
                    }
                    if (entity.BDSUTTLE > 100)
                    {
                        entity.BDSUTTLE = (entity.BDSUTTLE / 1000) * 1000;
                    }
                    else
                    {
                        entity.BDSUTTLE = entity.BDSUTTLE * 1000;
                    }

                }
            }

            //净重-采购部检斤重量-质检扣重-调整重量+补点重量
            decimal JSSUTTLE = (entity.SUTTLE - entity.JJSUTTLE - entity.ZJSUTTLE - entity.TZSUTTLE) + entity.BDSUTTLE - entity.PDAKZ2;

            if (entity.COMPUTERTYPE != "112" && entity.SUTTLE < 0)
            {
                JSSUTTLE = 0;
            }
            entity.JSSUTTLE = Math.Round(JSSUTTLE, 3, MidpointRounding.AwayFromZero);
            entity.PRICE = Math.Round(entity.PRICE, 3, MidpointRounding.AwayFromZero);
            entity.CHARGE = Math.Round(Math.Round(JSSUTTLE, 3, MidpointRounding.AwayFromZero) * Math.Round(entity.PRICE, 3, MidpointRounding.AwayFromZero), 3, MidpointRounding.AwayFromZero);
            foreach (SUMKJRESULTMEMO item in kjlist1)
            {
                if (item.KS > 0)
                {
                    //if (item.ZJSUTTLE != 0)
                    //{
                    KJRESULTMEMO += string.Format("项目：{0},扣量：{1};扣杂与加扣：{2}与{3};", item.ITEMNAME, item.ZJSUTTLE, item.ZJVALUE, item.KS);
                    //}
                }
                else
                {
                    //if (item.ZJSUTTLE != 0)
                    // {
                    KJRESULTMEMO += string.Format("项目：{0},扣量：{1};", item.ITEMNAME, item.ZJSUTTLE);
                    //}
                }
            }
            decimal KS = kjlist1.Sum(p => p.KS);
            entity.KJRESULTMEMO = string.Format("{0}", KJRESULTMEMO.TrimEnd(';'));
            if (KS > 0)
            {
                string oldstr = string.Format("加扣杂质:{0}", KS);
                string RESULTMEMO = entity.ZJRESULTMEMO.Replace(oldstr, "");
                entity.ZJRESULTMEMO = string.Format("{0};加扣杂质:{1}", RESULTMEMO, KS);
            }
            return entity;
        }

        public PD_GBMANAGER_DTL GetGBMANAGER_DTL(string PK_SENDORG, string PK_MATERIAL, string ZJITEM, string ZJVALUE, string ISBD)
        {
            PD_GBMANAGER_DTL gsentity = null;
            PD_GBMANAGERBll gsorderblll = new PD_GBMANAGERBll();
            List<PD_GBMANAGER_DTL> GSListData = gsorderblll.GetOrderEntryList1(PK_SENDORG, PK_MATERIAL, ZJITEM, ZJVALUE, ISBD);
            if (GSListData.Count == 0)
            {
                GSListData = gsorderblll.GetOrderEntryList1("", PK_MATERIAL, ZJITEM, ZJVALUE, ISBD);
            }
            if (GSListData.Count > 0)
            {
                gsentity = GSListData[0];
            }
            return gsentity;
        }

        public List<PD_GBMANAGER_DTL> GetGBMANAGER_DTL(string PK_SENDORG, string PK_MATERIAL, string ZJITEM, string ZJVALUE)
        {
            PD_GBMANAGERBll gsorderblll = new PD_GBMANAGERBll();
            List<PD_GBMANAGER_DTL> GSListData = gsorderblll.GetOrderEntryList1(PK_SENDORG, PK_MATERIAL, ZJITEM, ZJVALUE);
            if (GSListData.Count == 0)
            {
                GSListData = gsorderblll.GetOrderEntryList1("", PK_MATERIAL, ZJITEM, ZJVALUE);
            }
            return GSListData;
        }

        public List<PD_GBMANAGER_DTL> GetGBMANAGER_DTL1(string PK_SENDORG, string PK_MATERIAL, string ZJITEM, string ZJVALUE)
        {
            List<PD_GBMANAGER_DTL> entity_list = new List<PD_GBMANAGER_DTL>();
            PD_GBMANAGERBll gsorderblll = new PD_GBMANAGERBll();
            // List<PD_GBMANAGER_DTL> GSListData = gsorderblll.GetOrderEntryList1(PK_SENDORG, PK_MATERIAL, ZJITEM, ZJVALUE);
            List<PD_GBMANAGER_DTL> GSListData = gsorderblll.GetOrderEntryList1(PK_SENDORG, PK_MATERIAL, ZJITEM);
            if (GSListData.Count == 0)
            {
                GSListData = gsorderblll.GetOrderEntryList1("", PK_MATERIAL, ZJITEM, ZJVALUE);
            }

            foreach (PD_GBMANAGER_DTL entity in GSListData)
            {
                if (entity.SIGN1 == ">" || string.IsNullOrEmpty(entity.SIGN1))
                {
                    if (Convert.ToDecimal(ZJVALUE) > Convert.ToDecimal(entity.MAXJS))
                    {
                        if (entity.SIGN2 == "<" || string.IsNullOrEmpty(entity.SIGN2))
                        {
                            if (Convert.ToDecimal(ZJVALUE) < Convert.ToDecimal(entity.MINJS))
                            {
                                entity_list.Add(entity);
                            }
                        }
                        else
                        {
                            if (Convert.ToDecimal(ZJVALUE) <= Convert.ToDecimal(entity.MINJS))
                            {
                                entity_list.Add(entity);
                            }
                        }
                    }
                }
                else if (entity.SIGN1 == ">=")
                {
                    if (Convert.ToDecimal(ZJVALUE) >= Convert.ToDecimal(entity.MAXJS))
                    {
                        if (entity.SIGN2 == "<=" || string.IsNullOrEmpty(entity.SIGN2))
                        {
                            if (Convert.ToDecimal(ZJVALUE) <= Convert.ToDecimal(entity.MINJS))
                            {
                                entity_list.Add(entity);
                            }
                        }
                        else
                        {
                            if (Convert.ToDecimal(ZJVALUE) < Convert.ToDecimal(entity.MINJS))
                            {
                                entity_list.Add(entity);
                            }
                        }
                    }
                }
            }

            return entity_list;
        }

        /// <summary>
        /// 移入 过磅数据移入到指定单号
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="TASKLIST"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawYLJSAddCarItem(string KeyValue, string BILLNO)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = orderbll.PCRawYLJSAddCarItem(KeyValue, BILLNO);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中结算单号,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 表单操作


        /// <summary>
        /// 删除组批明细车号
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="KeyList"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawYLJSDeleteItem(string KeyValue, string KeyList, string BillNoList)
        {
            try
            {
                ResultMessage message = orderbll.PCRawYLJSDeleteItem(KeyValue, KeyList, BillNoList);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        [LoginAuthorize]
        public ActionResult HandOrder(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                #region  //验证
                string sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and ISUSE=4", bills);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有作废数据,无法继续上传的操作！" }.ToString());
                }

                sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and UPLOAD=1", bills);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有上传数据,无法继续上传的操作！" }.ToString());
                }
                #endregion
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                JsonMessage jsonmeg = null;
                //全推
                
                string[] arr = KeyValue.Split(',');
                foreach (string billno in arr)
                {
                    if (!string.IsNullOrEmpty(billno))
                    {
                        string str = "";
                        ResultJson result = JsonConvert.DeserializeObject<ResultJson>(str);
                        if (result.Flag == "1")
                        {
                            strSql.Clear();
                            strSql.Append(string.Format("update DP_PSORDER set UPLOAD=1 where BILLNO='{0}'", billno));
                            res += database.ExecuteBySql(strSql, isOpenTrans);
                            strSql.Clear();
                            strSql.Append(string.Format("update DP_PSORDER_B set UPLOAD=1 where PK_PSORDER='{0}'", billno));
                            res = database.ExecuteBySql(strSql, isOpenTrans);
                        }
                        else
                        {
                            //if (result.Message.Contains("重复"))
                            //{
                            //    strSql.Clear();
                            //    strSql.Append(string.Format("update DP_PSORDER set UPLOAD=1 where BILLNO='{0}'", billno));
                            //    res += database.ExecuteBySql(strSql, isOpenTrans);
                            //    strSql.Clear();
                            //    strSql.Append(string.Format("update DP_PSORDER_B set UPLOAD=1 where PK_PSORDER='{0}'", billno));
                            //    res = database.ExecuteBySql(strSql, isOpenTrans);
                            //}
                        }
                    }
                }

                if (res > 0)
                {
                    database.Commit();
                    string Message = "上传成功。";
                    jsonmeg = new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}", Message) };
                }
                else
                {
                    jsonmeg = new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}", "上传失败：") };
                }

                return Content(jsonmeg.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }


        [LoginAuthorize]
        public ActionResult UNHandOrder(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证
                #region  //验证
                string sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and ISUSE=4", bills);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有作废数据,无法继续取消上传的操作！" }.ToString());
                }

                //sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and UPLOAD=0", bills);
                //res = DataFactory.Database().FindCountBySql(sql);
                //if (res > 0)
                //{
                //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有未上传数据,无法继续取消上传的操作！" }.ToString());
                //}
                #endregion
                //全推
                
                ResultJson result = new ResultJson();
                JsonMessage jsonmeg = null;
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                res = 0;
                string[] arr = KeyValue.Split(',');
                foreach (string billno in arr)
                {
                    if (!string.IsNullOrEmpty(billno))
                    {
                        string str = "";
                        result = JsonConvert.DeserializeObject<ResultJson>(str);
                        if (result.Flag == "1")
                        {
                            strSql.Clear();
                            strSql.Append(string.Format("update DP_PSORDER set UPLOAD=0 where BILLNO='{0}'", billno));
                            res += database.ExecuteBySql(strSql, isOpenTrans);
                            strSql.Clear();
                            strSql.Append(string.Format("update DP_PSORDER_B set UPLOAD=0 where PK_PSORDER='{0}'", billno));
                            res += database.ExecuteBySql(strSql, isOpenTrans);
                        }
                    }
                }
                if (res > 0)
                {
                    database.Commit();
                    string Message = "取消上传成功。";
                    jsonmeg = new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}:{1}", Message, result.Message) };
                }
                else
                {
                    jsonmeg = new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", result.Message) };
                }

                return Content(jsonmeg.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }

        [LoginAuthorize]
        public ActionResult UNHandOrder2(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证
                #region  //验证
                string sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and ISUSE=4", bills);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有作废数据,无法继续取消上传的操作！" }.ToString());
                }


                #endregion

                JsonMessage jsonmeg = null;
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                res = 0;
                string[] arr = KeyValue.Split(',');
                foreach (string billno in arr)
                {
                    if (!string.IsNullOrEmpty(billno))
                    {
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_PSORDER set UPLOAD=0 where BILLNO='{0}'", billno));
                        res += database.ExecuteBySql(strSql, isOpenTrans);
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_PSORDER_B set UPLOAD=0 where PK_PSORDER='{0}'", billno));
                        res += database.ExecuteBySql(strSql, isOpenTrans);
                    }
                }
                if (res > 0)
                {
                    database.Commit();
                    string Message = "取消上传成功。";
                    jsonmeg = new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}", Message) };
                }
                else
                {
                    jsonmeg = new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}", "操作失败：") };
                }

                return Content(jsonmeg.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }


        //万国取消上传
        [LoginAuthorize]
        public ActionResult UNHandOrder1(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                #region  //验证
                string sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and ISUSE=4", bills);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有作废数据,无法继续取消上传的操作！" }.ToString());
                }

                sql = String.Format("select count(1) from DP_PSORDER where BILLNO in('{0}') and UPLOAD=0", bills);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前结算单有未上传数据,无法继续取消上传的操作！" }.ToString());
                }
                #endregion

                JsonMessage jsonmeg = null;
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update DP_PSORDER set UPLOAD=0 where BILLNO in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);
                strSql.Clear();
                strSql.Append(string.Format("update DP_PSORDER_B set UPLOAD=0 where PK_PSORDER in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                string Message = "取消上传成功。";
                jsonmeg = new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}", Message) };
                return Content(jsonmeg.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }

        public ActionResult CGJSReport(string Parm_Key_Value, string Type)
        {
            DataSet dataSet = orderbll.CGJSReport(Parm_Key_Value, Type);
            return Content(dataSet.GetXml());
        }

        public ActionResult CGJSReport1(string Parm_Key_Value, string Type)
        {
            DataSet dataSet = orderbll.CGJSReport1(Parm_Key_Value, Type);
            return Content(dataSet.GetXml());
        }

        public ActionResult SGDReport(string KeyValue)
        {
            DataSet dataSet = orderbll.SGDReport(KeyValue);
            return Content(dataSet.GetXml());
        }
    }
}
