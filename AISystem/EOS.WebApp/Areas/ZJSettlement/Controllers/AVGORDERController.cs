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
using System.Text;


namespace EOS.WebApp.Areas.ZJSettlement.Controllers
{
    public class AVGWB_WEIGHT_B
    {
        public string JSDATE { get; set; }
        public string BILLNO { get; set; }
        public string PK_ORDER { get; set; }
        public string PK_ORDER_B { get; set; }
        public decimal CARSNUM { get; set; }
        public decimal YFSUTTLE { get; set; }
        public decimal SUTTLE { get; set; }
        public decimal JSSUTTLE { get; set; }
        public string ITEM1 { get; set; }
        public string ITEMNAME1 { get; set; }
        public string ITEMBZ1 { get; set; }
        public decimal ITEMVALUE1 { get; set; }
        public string ITEM2 { get; set; }
        public string ITEMNAME2 { get; set; }
        public string ITEMBZ2 { get; set; }
        public decimal ITEMVALUE2 { get; set; }
        public string ITEM3 { get; set; }
        public string ITEMNAME3 { get; set; }
        public string ITEMBZ3 { get; set; }
        public decimal ITEMVALUE3 { get; set; }
        public string ITEM4 { get; set; }
        public string ITEMNAME4 { get; set; }
        public string ITEMBZ4 { get; set; }
        public decimal ITEMVALUE4 { get; set; }
        public string ITEM5 { get; set; }
        public string ITEMNAME5 { get; set; }
        public string ITEMBZ5 { get; set; }
        public decimal ITEMVALUE5 { get; set; }
    }
    public class AVGORDERController : PublicController<DP_AVGORDER>
    {
        string ModuleId = "";
        VDP_AVGORDERBll orderbll = new VDP_AVGORDERBll();
        VWB_WEIGHT_B_Bll weight_bbll = new VWB_WEIGHT_B_Bll();
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

        public ActionResult AVGReportIndex()
        {
            ViewBag.CreateUser = ManageProvider.Provider.Current().UserName;
            return View();
        }

        [LoginAuthorize]
        public ActionResult AVGORDERIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult AVGORDERIndex1()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult AVGORDERForm()
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
        public ActionResult SELECTAVGEXPRESSIONS()
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
        public ActionResult GetOrderList(string BILLNO, string VBILLCODE, string SUPPLY, string MATERIAL, string CREATEUSER, string STATUS, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VDP_AVGORDER> ListData = orderbll.GetOrderList(BILLNO, VBILLCODE, SUPPLY, MATERIAL, CREATEUSER, STATUS, StartTime, EndTime, jqgridparam);
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
        public ActionResult SetFormControl(string KeyValue)
        {
            DP_AVGORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        [HttpPost]
        public ActionResult GetOrderEntity(string KeyValue)
        {
            VDP_AVGORDER entity = orderbll.GetOrderEntity(KeyValue);
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
                List<DP_AVGORDER_B> ListData = orderbll.GetOrderListItem(MAINID, jqgridparam);
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
        public ActionResult GetOrderWeightList(string BillNo, string VBILLCODE, string DHBillNo, string Supply, string matclass, string Material, string Cars, string WEIGHTNO, string BRANDMODEL, string PK_RECEIVEORG, string PK_RECEIVESTORE, string TRAFFICCOMPANY, string HZUSERNAME, string Status, string UPLOAD, string ISJS, string ZJSTATUS, string CQDEF11, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VWB_WEIGHT_BIN_BSBll vorderbll = new VWB_WEIGHT_BIN_BSBll();
                List<VWB_WEIGHT_BIN_BS> ListData = vorderbll.GetRKOrderListNew(BillNo, VBILLCODE, DHBillNo, Supply, matclass, Material, Cars, Status, UPLOAD, WEIGHTNO, BRANDMODEL, PK_RECEIVEORG, PK_RECEIVESTORE, TRAFFICCOMPANY, HZUSERNAME, ISJS, ZJSTATUS, "", CQDEF11, StartTime, EndTime, jqgridparam);
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
            DP_AVGORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == 1)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该结算单是审批状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                entity.STATUS = 1;
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
            DP_AVGORDER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该结算单是未审批状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "反审成功。";
                entity.STATUS = 0;
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
                strSql.Append(string.Format("delete DP_AVGORDER where BILLNO in('{0}')", bills));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("delete DP_AVGORDER_B where PK_AVGORDER in('{0}')", bills));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);

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
        public ActionResult SubmitOrderForm(string KeyValue, DP_AVGORDER entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<DP_AVGORDER_B> OrderEntryList = OrderEntryJson.JonsToList<DP_AVGORDER_B>();
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    database.Delete<DP_AVGORDER_B>("PK_AVGORDER", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from DP_AVGORDER where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                }

                //处理明细
                int index = 1;
                foreach (DP_AVGORDER_B orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.JSDATE))
                    {
                        //结算明细
                        orderentry.Create();
                        orderentry.PK_AVGORDER = entity.BILLNO;
                        orderentry.MEMO = orderentry.MEMO.Replace("null", "");
                        database.Insert(orderentry, isOpenTrans);

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


        public ActionResult CheckData(string MATERIAL, string SUPPLY, string BEGINDATE, string ENDDATE, string VBILLCODE, string PK_ORDER_B, string PK_AVGEXPRESSIONS, string AVGTYPE)
        {

            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {

                List<VWB_WEIGHT_B> list = new List<VWB_WEIGHT_B>();
                VWB_WEIGHT_B model = null;
                //加权平均公式
                PD_AVGEXPRESSIONSBll avg_bll = new PD_AVGEXPRESSIONSBll();
                PD_AVGEXPRESSIONS agvexp = avg_bll.GetEntity(PK_AVGEXPRESSIONS);

                DataTable dt = GetWegithList(VBILLCODE, PK_ORDER_B, SUPPLY, MATERIAL, BEGINDATE, ENDDATE, AVGTYPE);

                //获取结算磅单化验项化验值
                VBD_ZJRESULTBll vorderbll = new VBD_ZJRESULTBll();
                List<VBD_ZJRESULTDETAILS> ZJListData = null;
                VBD_ZJRESULTDETAILS zjentity = null;

                AVGWB_WEIGHT_B avgmodel = null;
                List<AVGWB_WEIGHT_B> avglist = new List<AVGWB_WEIGHT_B>();
                //明细行数据
                foreach (DataRow row in dt.Rows)
                {
                    avgmodel = new AVGWB_WEIGHT_B();
                    #region 平均模型赋值
                    avgmodel.JSDATE = row["JSDATE"].ToString();
                    avgmodel.BILLNO = row["BILLNO"].ToString();
                    avgmodel.PK_ORDER = row["PK_ORDER"].ToString();
                    avgmodel.PK_ORDER_B = row["PK_ORDER_B"].ToString();
                    if (!string.IsNullOrEmpty(row["YFSUTTLE"].ToString()))
                    {
                        avgmodel.YFSUTTLE = Convert.ToDecimal(row["YFSUTTLE"].ToString());
                    }
                    if (!string.IsNullOrEmpty(row["SUTTLE"].ToString()))
                    {
                        avgmodel.SUTTLE = Convert.ToDecimal(row["SUTTLE"].ToString());
                    }
                    if (!string.IsNullOrEmpty(row["JSSUTTLE"].ToString()))
                    {
                        avgmodel.JSSUTTLE = Convert.ToDecimal(row["JSSUTTLE"].ToString());
                    }
                    avgmodel.ITEM1 = "";
                    avgmodel.ITEMNAME1 = "";
                    avgmodel.ITEMBZ1 = "";
                    avgmodel.ITEMVALUE1 = 0;
                    avgmodel.ITEM2 = "";
                    avgmodel.ITEMNAME2 = "";
                    avgmodel.ITEMBZ2 = "";
                    avgmodel.ITEMVALUE2 = 0;
                    avgmodel.ITEM3 = "";
                    avgmodel.ITEMNAME3 = "";
                    avgmodel.ITEMBZ3 = "";
                    avgmodel.ITEMVALUE3 = 0;
                    avgmodel.ITEM4 = "";
                    avgmodel.ITEMNAME4 = "";
                    avgmodel.ITEMBZ4 = "";
                    avgmodel.ITEMVALUE4 = 0;
                    avgmodel.ITEM5 = "";
                    avgmodel.ITEMNAME5 = "";
                    avgmodel.ITEMBZ5 = "";
                    avgmodel.ITEMVALUE5 = 0;
                    //质检结果
                    ZJListData = vorderbll.GetOrderEntryList1(avgmodel.PK_ORDER_B);
                    if (ZJListData.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(agvexp.ITEM))
                        {
                            zjentity = ZJListData.Where(p => p.ZJITEM == agvexp.ITEM).FirstOrDefault();
                            if (zjentity != null)
                            {
                                avgmodel.ITEM1 = zjentity.ZJITEM;
                                avgmodel.ITEMNAME1 = zjentity.ZJITEM;
                                avgmodel.ITEMBZ1 = agvexp.ITEMBZ;
                                if (!string.IsNullOrEmpty(zjentity.ZJVALUE))
                                {
                                    avgmodel.ITEMVALUE1 = Convert.ToDecimal(zjentity.ZJVALUE);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(agvexp.ITEM2))
                        {
                            zjentity = ZJListData.Where(p => p.ZJITEM == agvexp.ITEM2).FirstOrDefault();
                            if (zjentity != null)
                            {
                                avgmodel.ITEM2 = zjentity.ZJITEM;
                                avgmodel.ITEMNAME2 = zjentity.ZJITEM;
                                avgmodel.ITEMBZ2 = agvexp.ITEMBZ2;
                                if (!string.IsNullOrEmpty(zjentity.ZJVALUE))
                                {
                                    avgmodel.ITEMVALUE2 = Convert.ToDecimal(zjentity.ZJVALUE);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(agvexp.ITEM3))
                        {
                            zjentity = ZJListData.Where(p => p.ZJITEM == agvexp.ITEM3).FirstOrDefault();
                            if (zjentity != null)
                            {
                                avgmodel.ITEM3 = zjentity.ZJITEM;
                                avgmodel.ITEMNAME3 = zjentity.ZJITEM;
                                avgmodel.ITEMBZ3 = agvexp.ITEMBZ3;
                                if (!string.IsNullOrEmpty(zjentity.ZJVALUE))
                                {
                                    avgmodel.ITEMVALUE3 = Convert.ToDecimal(zjentity.ZJVALUE);
                                }
                            }
                        }
                        if (!string.IsNullOrEmpty(agvexp.ITEM4))
                        {
                            zjentity = ZJListData.Where(p => p.ZJITEM == agvexp.ITEM4).FirstOrDefault();
                            if (zjentity != null)
                            {
                                avgmodel.ITEM4 = zjentity.ZJITEM;
                                avgmodel.ITEMNAME4 = zjentity.ZJITEM;
                                avgmodel.ITEMBZ4 = agvexp.ITEMBZ4;
                                if (!string.IsNullOrEmpty(zjentity.ZJVALUE))
                                {
                                    avgmodel.ITEMVALUE4 = Convert.ToDecimal(zjentity.ZJVALUE);
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(agvexp.ITEM5))
                        {
                            zjentity = ZJListData.Where(p => p.ZJITEM == agvexp.ITEM5).FirstOrDefault();
                            if (zjentity != null)
                            {
                                avgmodel.ITEM5 = zjentity.ZJITEM;
                                avgmodel.ITEMNAME5 = zjentity.ZJITEM;
                                avgmodel.ITEMBZ5 = agvexp.ITEMBZ5;
                                if (!string.IsNullOrEmpty(zjentity.ZJVALUE))
                                {
                                    avgmodel.ITEMVALUE5 = Convert.ToDecimal(zjentity.ZJVALUE);
                                }
                            }
                        }
                    }

                    #endregion
                    avglist.Add(avgmodel);
                }

                if (avglist.Count > 0)
                {
                    var AvgData = avglist.GroupBy(x => new { x.JSDATE, x.ITEM1, x.ITEMNAME1, x.ITEM2, x.ITEMNAME2, x.ITEM3, x.ITEMNAME3, x.ITEM4, x.ITEMNAME4, x.ITEM5, x.ITEMNAME5 }).Select(y => new
                     {
                         JSDATE = y.Key.JSDATE,
                         ITEM1 = y.Key.ITEM1,
                         ITEMNAME1 = y.Key.ITEMNAME1,
                         ITEM2 = y.Key.ITEM2,
                         ITEMNAME2 = y.Key.ITEMNAME2,
                         ITEM3 = y.Key.ITEM3,
                         ITEMNAME3 = y.Key.ITEMNAME3,
                         ITEM4 = y.Key.ITEM4,
                         ITEMNAME4 = y.Key.ITEMNAME4,
                         ITEM5 = y.Key.ITEM5,
                         ITEMNAME5 = y.Key.ITEMNAME5,
                         CARSNUM = y.Count(),
                         YFSUTTLE = y.Sum(x => x.YFSUTTLE),
                         SUTTLE = y.Sum(x => x.SUTTLE),
                         JSSUTTLE = y.Sum(x => x.JSSUTTLE),
                         ITEMBZ1 = y.Max(x => x.ITEMBZ1),
                         ITEMVALUE1 = y.Average(x => x.ITEMVALUE1),
                         ITEMBZ2 = y.Max(x => x.ITEMBZ2),
                         ITEMVALUE2 = y.Average(x => x.ITEMVALUE2),
                         ITEMBZ3 = y.Max(x => x.ITEMBZ3),
                         ITEMVALUE3 = y.Average(x => x.ITEMVALUE3),
                         ITEMBZ4 = y.Max(x => x.ITEMBZ4),
                         ITEMVALUE4 = y.Average(x => x.ITEMVALUE4),
                         ITEMBZ5 = y.Max(x => x.ITEMBZ5),
                         ITEMVALUE5 = y.Average(x => x.ITEMVALUE5)
                     });
                    var JsonData = new
                    {
                        rows = AvgData
                    };
                    return Content(JsonData.ToJson());
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //查询加权平均计量单
        public DataTable GetWegithList(string vbillcode, string pk_order_b, string supply, string material, string StartTime, string EndTime, string avgtype)
        {
            string JSSUTTLE = "SUTTLE";
            if (avgtype == "0")
            {
                JSSUTTLE = "YFSUTTLE";
            }
            string strSql = "";
            strSql = string.Format(@"SELECT substr(LASTDATE,0,10) as JSDATE,BILLNO,PK_ORDER,PK_ORDER_B,1 CARSNUM,YFSUTTLE,SUTTLE,{0} as JSSUTTLE,0 as ITEMVALUE1,0 as ITEMVALUE2,0 as ITEMVALUE3,0 as ITEMVALUE4,0 as ITEMVALUE5
FROM VWB_WEIGHT_B where type=0 and finish=1 ", JSSUTTLE);
            //订单号
            if (!string.IsNullOrEmpty(vbillcode))
            {
                strSql += string.Format(" AND PK_CONTRACT_B='{0}'", vbillcode);
            }
            //订单子表Pk
            if (!string.IsNullOrEmpty(pk_order_b))
            {
                strSql += string.Format(" AND PK_BILL_B='{0}'", pk_order_b);
            }
            //供应商
            if (!string.IsNullOrEmpty(supply))
            {
                strSql += string.Format(" AND PK_SENDORG='{0}'", supply);
            }

            //物料
            if (!string.IsNullOrEmpty(material))
            {
                strSql += string.Format(" AND PK_MATERIAL='{0}'", material);
            }

            //轻车时间
            if (!string.IsNullOrEmpty(StartTime) && !string.IsNullOrEmpty(EndTime))
            {
                strSql += string.Format(" AND LASTDATE Between '{0}' and '{1}'", StartTime, EndTime);
            }
            return DataFactory.Database().FindTableBySql(strSql);
        }

        #endregion


    }
}
