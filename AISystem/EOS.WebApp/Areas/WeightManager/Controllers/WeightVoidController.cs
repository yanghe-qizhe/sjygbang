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
using System.Data;

namespace EOS.WebApp.Areas.WeightManager.Controllers
{

    public class WeightVoidController : PublicController<WB_WEIGHT>
    {
        string ModuleId = "";

        VWB_WEIGHTBSBll orderbll = new VWB_WEIGHTBSBll();


        #region 提单作废审批
        [LoginAuthorize]
        public ActionResult WeightTaskIndex()
        {
            return View();
        }

        public ActionResult WeightHWIndex()
        {
            return View();
        }
        #endregion
        //
        // GET: /WeightManager/WeightVoid/
        [LoginAuthorize]
        public ActionResult WeightVoidIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult WeightVoidQuery()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult InvalidForm()
        {
            return View();
        }
        /// 磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string VBillCode, string ContractBillNo, string TZBillNo, string Customer, string Material, string Cars, string Status, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_WEIGHTBS> ListData = orderbll.GetVoidOrderList(BillNo, VBillCode, ContractBillNo, TZBillNo, Customer, Material, Cars, Status, "", StartTime, EndTime, jqgridparam);
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


        //作废一次计量
        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue, string PK_ORDER, string MEMO)
        {
            //验证
            WB_WEIGHT entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity == null)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该计量单已作废！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().Account;
            try
            {
                string Message = "作废成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                int res = 0;
                string sql = "";
                //派车表实例

                if (entity.TYPE == "3")
                {
                    DP_SOCARSORDER so_entity = null;
                    so_entity = database.FindEntityByWhere<DP_SOCARSORDER>(string.Format(" AND ID='{0}'", entity.PK_ORDER));
                    res = 0;
                    if (so_entity.ISMULTI == "1")
                    {

                    }
                    else
                    {
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_SOCARSORDER set ISUSE=1 where ID='{0}'", entity.PK_ORDER));
                        res += database.ExecuteBySql(strSql, isOpenTrans);
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_SOCARSORDER_DTL set ISUSE=1 where PK_DP_SOCARSORDER='{0}' and ID='{1}'", entity.PK_ORDER, entity.PK_ORDER_B));
                        res += database.ExecuteBySql(strSql, isOpenTrans);
                    }

                }
                else if (entity.TYPE == "0")
                {
                    sql = String.Format("SELECT count(1) NUMS FROM VZJQY WHERE  ISCY='1' and DETAILSID in(select ID from DP_POCARSORDER_DTL where pk_dp_pocarsorder='{0}' and isuse=1) ", entity.PK_ORDER);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}、派车单:{1}状态为未计量的派车明细已采样！", entity.CARS, entity.PK_ORDER) }.ToString());
                    }
                    //sql = String.Format("select count(1) from APP_HANDORDER  where  ID in(select ID from DP_POCARSORDER_DTL where pk_dp_pocarsorder='{0}' and isuse=1) and STATUS='{1}'", entity.PK_ORDER, "1");
                    sql = String.Format("select count(1) from APP_HANDORDER  where  BILLNO='{0}' and STATUS='{1}'", entity.BILLNO, "1");
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("车辆:{0}、过磅单：{1}已收货确认!", entity.CARS, entity.BILLNO) }.ToString());
                    }
                    DP_POCARSORDER po_entity = null;
                    po_entity = database.FindEntityByWhere<DP_POCARSORDER>(string.Format(" AND ID='{0}'", entity.PK_ORDER));
                    if (po_entity.ISMULTI == "1")
                    {

                    }
                    else
                    {
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_POCARSORDER set ISUSE=1 where ID='{0}'", entity.PK_ORDER));
                        res += database.ExecuteBySql(strSql, isOpenTrans);
                        strSql.Clear();
                        strSql.Append(string.Format("update DP_POCARSORDER_DTL set ISUSE=1 where PK_DP_POCARSORDER='{0}' and ID='{1}'", entity.PK_ORDER, entity.PK_ORDER_B));
                        res += database.ExecuteBySql(strSql, isOpenTrans);
                    }
                }

                sql = String.Format("SELECT count(1) NUMS FROM WB_WEIGHT_V WHERE BILLNO='{0}' ", entity.BILLNO);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res == 0)
                {
                    strSql.Clear();
                    strSql.Append(string.Format("INSERT INTO WB_WEIGHT_V(BILLNO,PK_CARSID,PK_RECEIVEORG,PK_SENDORG,PK_RECEIVESTORE,PK_SENDSTORE,PK_DRIVERS,PK_MATERIAL,PK_MATERIALKIND,PK_TRAFFICCOMPANY,PK_STORE,PK_ORG,CARS,RECEIVEORG,SENDORG,RECEIVESTORE,SENDSTORE,DRIVERS,MATERIAL,MATERIALKIND,TRAFFICCOMPANY,STORE,BATHNO,WEIGHTNO,LIGHTNO,WEIGHTDATE,LIGHTDATE,CREATEDATE,LASTDATE,PRINTDATE,PRINTCOUNT,YFTARE,YFSUTTLE_TMP,GROSS,TARE,SUTTLE,PRICE,CHARGE,TRAFFICPRICE,TRAFFICCHARGE,PDAUSER,PDANO,PDADATE,PDAYF,PDAKZ1,PDAKZ2,INGBUSER,INDBUSER,OUTGBUSER,OUTDBUSER,TYPE,COMPUTERTYPE,UPLOAD,ICCARD,IDCARD,RFIDCARD,TWOBANG,FINISH,STATUS,PK_ORDER,PK_ORDER_B,PK_CONTRACT,PK_CONTRACT_B,PK_TASK,DEF1,DEF2,DEF3,DEF4,DEF5,DEF6,DEF7,DEF8,DEF9,DEF10,DEF11,DEF12,DEF13,DEF14,DEF15,DEF16,DEF17,DEF18,DEF19,DEF20,DATATYPE,UPLOAD1,YFPIECE,YFPIWEIGHT,BALANCEMETHOD,SURPRINT,PK_BILL_B,PK_BILL,BALANCEMETHODNAME,COMPANYNAME,PK_COMPANY,PK_DEPARTMENT,DEPARTMENTNAME,YFGROSS,YFSUTTLE,PK_ARRIVO,PK_REPORT,ISJS,UPDATEDATE,UPDATEUSER,CHECKUSER,DELETEDATE,DELETEUSER,ID,MEMO) SELECT BILLNO,PK_CARSID,PK_RECEIVEORG,PK_SENDORG,PK_RECEIVESTORE,PK_SENDSTORE,PK_DRIVERS,PK_MATERIAL,PK_MATERIALKIND,PK_TRAFFICCOMPANY,PK_STORE,PK_ORG,CARS,RECEIVEORG,SENDORG,RECEIVESTORE,SENDSTORE,DRIVERS,MATERIAL,MATERIALKIND,TRAFFICCOMPANY,STORE,BATHNO,WEIGHTNO,LIGHTNO,WEIGHTDATE,LIGHTDATE,CREATEDATE,LASTDATE,PRINTDATE,PRINTCOUNT,YFTARE,YFSUTTLE_TMP,GROSS,TARE,SUTTLE,PRICE,CHARGE,TRAFFICPRICE,TRAFFICCHARGE,PDAUSER,PDANO,PDADATE,PDAYF,PDAKZ1,PDAKZ2,INGBUSER,INDBUSER,OUTGBUSER,OUTDBUSER,TYPE,COMPUTERTYPE,UPLOAD,ICCARD,IDCARD,RFIDCARD,TWOBANG,FINISH,STATUS,PK_ORDER,PK_ORDER_B,PK_CONTRACT,PK_CONTRACT_B,PK_TASK,DEF1,DEF2,DEF3,DEF4,DEF5,DEF6,DEF7,DEF8,DEF9,DEF10,DEF11,DEF12,DEF13,DEF14,DEF15,DEF16,DEF17,DEF18,DEF19,DEF20,DATATYPE,UPLOAD1,YFPIECE,YFPIWEIGHT,BALANCEMETHOD,SURPRINT,PK_BILL_B,PK_BILL,BALANCEMETHODNAME,COMPANYNAME,PK_COMPANY,PK_DEPARTMENT,DEPARTMENTNAME,YFGROSS,YFSUTTLE,PK_ARRIVO,PK_REPORT,ISJS,'' as UPDATEDATE,'' as UPDATEUSER,'' as CHECKUSER,'{0}' as DELETEDATE,'{1}' as DELETEUSER,'{2}' as ID,'{3}' as MEMO from WB_WEIGHT where BILLNO='{4}'", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), ManageProvider.Provider.Current().UserName, CommonHelper.GetGuid, MEMO, entity.BILLNO));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }
                database.Delete("WB_WEIGHT", "BILLNO", KeyValue, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", string.Format("作废车号{0}一次计量单{1}成功，毛:{2}、皮：{3}。", entity.CARS, entity.BILLNO, entity.GROSS, entity.TARE));
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "作废一次计量单失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 【过磅单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            WB_WEIGHT entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }


    }
}
