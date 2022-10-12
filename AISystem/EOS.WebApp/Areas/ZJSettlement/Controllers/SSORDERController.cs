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

    public class SSORDERController : PublicController<DP_SSORDER>
    {
        string ModuleId = "";
        VDP_SSORDERBll orderbll = new VDP_SSORDERBll();

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
        [LoginAuthorize]
        public ActionResult SSORDERIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SSORDERIndex1()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SSORDERForm()
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
        public ActionResult SSORDERPrint()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SELECTWEIGHTS()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult SELECTSALEORDERS()
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
        public ActionResult GetOrderList(string BILLNO, string VBILLCODE, string CUSTOMER, string MATERIAL, string WEIGHTNO, string CREATEUSER, string HZUSERNAME, string STATUS, string ISUSE, string CQDEF1, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VDP_SSORDER> ListData = orderbll.GetOrderList(BILLNO, VBILLCODE, CUSTOMER, MATERIAL, WEIGHTNO, CREATEUSER, HZUSERNAME, STATUS, ISUSE, CQDEF1, StartTime, EndTime, jqgridparam);
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
            DP_SSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        [HttpPost]
        public ActionResult GetOrderEntity(string KeyValue)
        {
            VDP_SSORDER entity = orderbll.GetOrderEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        //记忆
        [HttpPost]
        public ActionResult GetJYOrderEntity()
        {
            string createuser = ManageProvider.Provider.Current().UserName;
            VDP_SSORDER entity = orderbll.GetJYOrderEntity(createuser);
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
                List<DP_SSORDER_B> ListData = orderbll.GetOrderListItem(MAINID, jqgridparam);
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
        public ActionResult GetOrderWeightList(string BillNo, string VBILLCODE, string FHBillNo, string Customer, string Material, string Cars, string WEIGHTNO, string BRANDMODEL, string PK_RECEIVEORG, string STORE, string TRAFFICCOMPANY, string Status, string UPLOAD, string ISJS, string DEF11, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VWB_WEIGHTBSBll vorderbll = new VWB_WEIGHTBSBll();
                List<VWB_WEIGHTBS> ListData = vorderbll.GetCKOrderList(BillNo, VBILLCODE, "", FHBillNo, Customer, Material, Cars, STORE, WEIGHTNO, BRANDMODEL, TRAFFICCOMPANY, Status, "3", UPLOAD, "1", ISJS, "", DEF11, StartTime, EndTime, jqgridparam);
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
            DP_SSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
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
            DP_SSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
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
            //验证
            DP_SSORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == 4)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该结算单已作废！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "作废成功。";
                entity.ISUSE = 4;
                entity.MEMO = "作废";
                database.Update(entity, isOpenTrans);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //明细
                strSql.Append(string.Format("update DP_SSORDER_B set ISUSE='4',MEMO='作废',DELETEDATE='" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "',DELETEUSER='" + ManageProvider.Provider.Current().UserName + "' where PK_SSORDER='{0}'", KeyValue));
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

        public ActionResult DeleteItem(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update wb_weight set ISJS='0' where BILLNO in(select billno from DP_SSORDER_B where PK_SSORDER in('{0}'))", bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("delete DP_SSORDER where BILLNO in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("delete DP_SSORDER_B where PK_SSORDER in('{0}')", bills));
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
        public ActionResult SubmitOrderForm(string KeyValue, DP_SSORDER entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<DP_SSORDER_B> OrderEntryList = OrderEntryJson.JonsToList<DP_SSORDER_B>();
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update wb_weight set ISJS='0' where BILLNO in(select billno from DP_SSORDER_B where PK_SSORDER='{0}')", KeyValue));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    database.Delete<DP_SSORDER_B>("PK_SSORDER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from DP_SSORDER where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = orderbll.GetBillNo(entity.WEIGHTNO, entity.JSDATE);
                    }
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                }

                //处理明细
                int index = 1;
                foreach (DP_SSORDER_B orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.BILLNO))
                    {
                        //结算明细
                        orderentry.Create();
                        orderentry.PK_SSORDER = entity.BILLNO;
                        orderentry.MEMO = orderentry.MEMO.Replace("null", "");
                        orderentry.JSDATE = entity.JSDATE;
                        database.Insert(orderentry, isOpenTrans);
                        //更新是否过磅表中结算状态
                        strSql.Clear();
                        strSql.Append(string.Format("update WB_WEIGHT set ISJS=1 where BILLNO='{0}'", orderentry.BILLNO));
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
                        model = GetWeightEneity(orderentry, ref kjlist);
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
                        ZJSUTTLE = y.Sum(x => x.ZJSUTTLE)
                    });

                    foreach (var item in SumKJData)
                    {
                        SUMKJRESULTMEMO += string.Format("项目：{0},扣量：{1};", item.ITEMNAME, item.ZJSUTTLE);
                    }
                }
                var JsonData = new
                {
                    KJRESULTMEMO = SUMKJRESULTMEMO.TrimEnd(';'),
                    rows = list
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public VWB_WEIGHT_BIN_BS GetWeightEneity(VWB_WEIGHT_BIN_BS entity, ref List<SUMKJRESULTMEMO> kjlist)
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
                string EXPRESSIONS = "", EXPRESSIONS_JS = "0", EXPRESSIONS_JS1 = "0", JS_SQL = "";
                DataTable dt = null;
                DataRow row = null;
                foreach (VBD_ZJRESULTDETAILS zjentity in ListData)
                {
                    //扣重公式
                    //gsentity = GetGBMANAGER_DTL(entity.PK_SENDORG, entity.PK_MATERIAL, zjentity.ZJITEM, zjentity.ZJVALUE, "0");
                    list = GetGBMANAGER_DTL(entity.PK_SENDORG, entity.PK_MATERIAL, zjentity.ZJITEM, zjentity.ZJVALUE);
                    if (list.Count > 0)
                    {
                        gsentity = list.Where(p => p.ISBD == "0").FirstOrDefault();
                        if (gsentity != null)
                        {
                            EXPRESSIONS = gsentity.EXPRESSIONS;//不合格公式
                            if (!string.IsNullOrEmpty(EXPRESSIONS))
                            {
                                //不合格
                                EXPRESSIONS_JS = EXPRESSIONS.Replace("@净重@", entity.SUTTLE.ToString()).Replace("@净重", entity.SUTTLE.ToString()).Replace("@检验项目@", zjentity.ZJVALUE).Replace("检验项目", zjentity.ZJVALUE).Replace("（", "(").Replace("）", ")");
                            }
                        }
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
                                    EXPRESSIONS_JS1 = EXPRESSIONS.Replace("@净重@", entity.SUTTLE.ToString()).Replace("@净重", entity.SUTTLE.ToString()).Replace("@检验项目@", zjentity.ZJVALUE).Replace("检验项目", zjentity.ZJVALUE);
                                }
                            }
                            #endregion
                        }
                        JS_SQL = string.Format("select round({0},4) as ZJSUTTLE,round({1},4) as BDSUTTLE from VWB_WEIGHT_BIN_BS where BILLNO='{2}'", EXPRESSIONS_JS, EXPRESSIONS_JS1, entity.BILLNO);
                        dt = DataFactory.Database().FindTableBySql(JS_SQL);

                        if (dt != null)
                        {
                            kjresult = new SUMKJRESULTMEMO();
                            row = dt.Rows[0];
                            kjresult.ZJSUTTLE = 0;
                            if (Convert.ToDecimal(row["ZJSUTTLE"]) > 0)
                            {
                                kjresult.ZJITEM = zjentity.ZJITEM;
                                kjresult.ITEMNAME = zjentity.ITEMNAME;
                                kjresult.ZJSUTTLE = Convert.ToDecimal(row["ZJSUTTLE"]);
                            }
                            kjresult.BDSUTTLE = 0;
                            if (Convert.ToDecimal(row["BDSUTTLE"]) > 0)
                            {
                                kjresult.BDSUTTLE = Convert.ToDecimal(row["BDSUTTLE"]);
                            }
                            kjlist.Add(kjresult);
                            kjlist1.Add(kjresult);
                        }
                        EXPRESSIONS_JS = "0";
                        EXPRESSIONS_JS1 = "0";
                    }
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
                    entity.YFSUTTLE = entity.YFSUTTLE * 1000;
                    entity.GROSS = entity.GROSS * 1000;
                    entity.TARE = entity.TARE * 1000;
                    entity.SUTTLE = entity.SUTTLE * 1000;
                    entity.JJSUTTLE = entity.JJSUTTLE * 1000;
                    entity.ZJSUTTLE = entity.JJSUTTLE * 1000;
                    entity.TZSUTTLE = entity.JJSUTTLE * 1000;
                    entity.BDSUTTLE = entity.JJSUTTLE * 1000;
                }
            }

            //净重-采购部检斤重量-质检扣重-调整重量+补点重量
            decimal JSSUTTLE = (entity.SUTTLE - entity.JJSUTTLE - entity.ZJSUTTLE - entity.TZSUTTLE) + entity.BDSUTTLE;
            entity.JSSUTTLE = Math.Round(JSSUTTLE, 3);
            entity.PRICE = Math.Round(entity.PRICE, 3);
            entity.CHARGE = Math.Round(entity.JSSUTTLE * entity.PRICE, 2);

            foreach (SUMKJRESULTMEMO item in kjlist1)
            {
                KJRESULTMEMO += string.Format("项目：{0},扣量：{1};", item.ITEMNAME, item.ZJSUTTLE);
            }
            entity.KJRESULTMEMO = KJRESULTMEMO.TrimEnd(';');
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



        #endregion



        [LoginAuthorize]
        public ActionResult HandOrder(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证
                VDP_SSORDER entity = orderbll.GetOrderEntity(KeyValue);


                JsonMessage jsonmeg = null;
                //全推
                
                string str = "";
                ResultJson result = JsonConvert.DeserializeObject<ResultJson>(str);
                if (result.Flag == "1")
                {
                    //更新UPLOA=1为上传标识
                    DP_SSORDER up_entity = repositoryfactory.Repository().FindEntity(KeyValue);
                    up_entity.UPLOAD = 1;
                    database.Update(up_entity, isOpenTrans);
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_SSORDER_B set UPLOAD=1 where PK_PSORDRR='{0}'", up_entity.BILLNO));
                    int res = database.ExecuteBySql(strSql, isOpenTrans);
                    database.Commit();
                    string Message = "上传成功。";
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
        public ActionResult UNHandOrder(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证
                VDP_SSORDER entity = orderbll.GetOrderEntity(KeyValue);
                JsonMessage jsonmeg = null;
                //全推
                string str = "";
                ResultJson result = JsonConvert.DeserializeObject<ResultJson>(str);
                if (result.Flag == "1")
                {
                    //更新UPLOA=0为取消上传标识
                    DP_SSORDER up_entity = repositoryfactory.Repository().FindEntity(KeyValue);
                    up_entity.UPLOAD = 0;
                    database.Update(up_entity, isOpenTrans);
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_SSORDER_B set UPLOAD=0 where PK_PSORDRR='{0}'", up_entity.BILLNO));
                    int res = database.ExecuteBySql(strSql, isOpenTrans);
                    database.Commit();
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

        public ActionResult XSJSReport(string Parm_Key_Value, string Type)
        {
            DataSet dataSet = orderbll.XSJSReport(Parm_Key_Value, Type);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, false);
            }
            return null;
        }
        public ActionResult SGDReport(string KeyValue)
        {
            DataSet dataSet = orderbll.SGDReport(KeyValue);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, false);
            }
            return View();
        }
    }
}
