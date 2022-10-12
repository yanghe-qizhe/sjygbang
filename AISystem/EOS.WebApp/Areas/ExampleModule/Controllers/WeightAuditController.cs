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
    public class WeightAuditController : PublicController<WB_WEIGHTAUDIT>
    {
        string ModuleId = "";
        VZC_WEIGHTBll orderbll = new VZC_WEIGHTBll();
        WB_WEIGHTAUDITBll orderbll1 = new WB_WEIGHTAUDITBll();
        #region 公共方法
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult WeightAuditQuery()
        {
            return View();
        }
        //门禁记录
        [LoginAuthorize]
        public ActionResult ZCWeightIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult CALLCARSLOGSIndex()
        {
            return View();
        }
        
        /// 门禁订单列表（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string Material, string Cars, string Drivers, string ICNUMBER, string Status, string Type, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();

                List<VZC_WEIGHT> ListData = orderbll.GetOrderList(BillNo, Material, Cars, Drivers, ICNUMBER, Status, Type, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetOrderListJH(string cars, string store, string material,  string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DP_CALLCARSLOGSBll jhorderbll = new DP_CALLCARSLOGSBll();
                List<DP_CALLCARSLOGS> ListData = jhorderbll.GetOrderList(cars, store, material,StartTime, EndTime, jqgridparam);
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
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            WB_WEIGHTAUDIT entity = orderbll1.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        /// <summary>
        /// 超重评价如下返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl2(string KeyValue)
        {
            WB_GROSSEXCEPTION entity = orderbll2.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        //作废页面
        [LoginAuthorize]
        public ActionResult InvalidOrderForm()
        {
            return View();
        }

        //门禁记录作废
        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue, string BillNo, string TYPE, String MEMO)
        {
            //验证
            VZC_WEIGHT entity = orderbll.GetEntity(BillNo, "VZC_WEIGHT");
            //if (entity.STATUS != "入厂")
            //{
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有入厂数据能作废操作！" }.ToString());
            //}
            if (entity.DATATYPE == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有业务平台数据能作废操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().Account;
            try
            {
                int res = 0;
                string Message = "作废成功。";
                //派车单出厂
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                if (entity.TYPE == "销售")
                {
                    strSql.Append(string.Format("update DP_SOCARSORDER set ISUSE=4,MEMO='作废：{1}' where ID='{0}'", KeyValue, MEMO));
                    res = repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                }
                else if (entity.TYPE == "采购")
                {
                    strSql.Append(string.Format("update DP_POCARSORDER set ISUSE=4,MEMO='作废：{1}' where ID='{0}'", KeyValue, MEMO));
                    res = repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                }

                //删除中间表
                DEL_CARSORDERS(KeyValue, database, isOpenTrans);
                //门禁作废
                strSql.Clear();
                strSql.Append(string.Format("update wb_weight_task set finish='1',status='作废',MEMO='作废：{1}' where PK_TASK='{0}'", BillNo, MEMO));
                res += repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("update wb_weight set finish='1',status='完成',COMPUTERTYPE='99',MEMO='业务平台作废' where PK_ORDER='{0}'", KeyValue));
                res += repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                string msg = string.Format("{0}车号任务单号{1},车辆状态记录作废成功", entity.PK_TASK, entity.CARSNAME);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", msg);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "车辆状态记录作废失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region  空车出厂审批

        //审批页面
        [LoginAuthorize]
        public ActionResult KCAuditForm()
        {
            return View();
        }
        //空车出厂
        [LoginAuthorize]
        public ActionResult KCZCWeightIndex()
        {
            return View();
        }
        /// 空车订单列表（返回Json）
        [LoginAuthorize]
        public ActionResult GetKCOrderList(string BillNo, string CarsName, string Status, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<WB_WEIGHTAUDIT> ListData = orderbll1.GetOrderList(BillNo, CarsName, Status, "3", StartTime, EndTime, jqgridparam);
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


        //空车审批
        [LoginAuthorize]
        public ActionResult KCCheckOrder(string KeyValue, string MEMO)
        {
            //验证
            WB_WEIGHTAUDIT entity = orderbll1.GetEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已空车出厂审批！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().Account;
            try
            {
                string Message = "审批成功。";
                entity.STATUS = "1";
                entity.STATUS1 = "0";
                entity.MEMO = MEMO;
                entity.AUDITDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entity.AUDITDATE = Account;
                database.Update(entity, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", string.Format("单据:{0}、车号:{1}空车出厂审批成功", entity.BILLNO, entity.CARSNAME));
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "单据空车出厂审批失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 原货出厂审批
        //审批页面
        [LoginAuthorize]
        public ActionResult YHAuditForm()
        {
            return View();
        }
        //原货出厂
        [LoginAuthorize]
        public ActionResult YHZCWeightIndex()
        {
            return View();
        }

        /// 原货出厂订单列表（返回Json）
        [LoginAuthorize]
        public ActionResult GetYHOrderList(string BillNo, string CarsName, string Status, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<WB_WEIGHTAUDIT> ListData = orderbll1.GetOrderList(BillNo, CarsName, Status, "13", StartTime, EndTime, jqgridparam);
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


        //原货出厂审批
        [LoginAuthorize]
        public ActionResult YHCheckOrder(string KeyValue, string MEMO)
        {
            //验证
            WB_WEIGHTAUDIT entity = orderbll1.GetEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已原货出厂一次审批！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().Account;
            try
            {
                string Message = "审批成功。";
                entity.STATUS = "1";
                entity.STATUS1 = "0";
                entity.AUDITDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entity.AUDITUSER = ManageProvider.Provider.Current().UserName;
                entity.MEMO = MEMO;
                database.Update(entity, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "单据原货出厂一次审批成功");
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "单据原货出厂一次审批失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        #endregion

        #region  皮重异常审批
        //皮重异常
        [LoginAuthorize]
        public ActionResult PZZCWeightIndex()
        {
            return View();
        }

        WB_TAREEXCEPTIONBll qssorderbll = new WB_TAREEXCEPTIONBll();
        public ActionResult GetPZOrderList(string BillNo, string Cars, string Status, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();

                List<WB_TAREEXCEPTION> ListData = qssorderbll.GetOrderList(BillNo, Cars, Status, StartTime, EndTime, jqgridparam);
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

        //皮重异常审批

        public ActionResult PZCheckOrder(string KeyValue)
        {
            //验证
            WB_TAREEXCEPTION entityaudit = qssorderbll.GetEntity(KeyValue);
            if (entityaudit.STATE == 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已皮重异常审批！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().Account;
            try
            {
                string Message = "审批成功。";
                entityaudit.STATE = 0;
                entityaudit.TE3 = ManageProvider.Provider.Current().UserName;
                entityaudit.TE4 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                database.Update(entityaudit, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", string.Format("单据:{0}，车号:{1}已皮重异常审批成功", entityaudit.BILLNO, entityaudit.CARSNAME));
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "单据已皮重异常审批失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion



        #region  超重异常审批

        //审批页面
        [LoginAuthorize]
        public ActionResult MZAuditForm()
        {
            return View();
        }
        //皮重异常
        [LoginAuthorize]
        public ActionResult MZZCWeightIndex()
        {
            return View();
        }

        WB_GROSSEXCEPTIONBll orderbll2 = new WB_GROSSEXCEPTIONBll();
        ///毛重异常订单列表（返回Json）
        public ActionResult GetMZOrderList(string BillNo, string Cars, string Status, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();

                List<WB_GROSSEXCEPTION> ListData = orderbll2.GetOrderList(BillNo, Cars, Status, StartTime, EndTime, jqgridparam);
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

        //超重异常审批

        public ActionResult MZCheckOrder(string KeyValue, string MEMO)
        {
            //验证
            WB_GROSSEXCEPTION entityaudit = orderbll2.GetEntity(KeyValue);
            if (entityaudit.STATE == 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已超重异常审批！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().Account;
            try
            {
                string Message = "审批成功。";
                entityaudit.STATE = 0;
                entityaudit.MEMO = MEMO;
                database.Update(entityaudit, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "单据已超重异常审批成功");
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "单据已超重异常审批失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        #endregion

        #region 红外被档审批

        //红个被挡
        [LoginAuthorize]
        public ActionResult HWZCWeightIndex()
        {
            return View();
        }

        /// 红外订单列表（返回Json）
        public ActionResult GetHWOrderList(string BillNo, string CarsName, string Status, string Type, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<WB_WEIGHTAUDIT> ListData = orderbll1.GetOrderListHW(BillNo, CarsName, Status, Type, StartTime, EndTime, jqgridparam);
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


        //红外一审批
        [LoginAuthorize]
        public ActionResult HWCheckOrder(string KeyValue)
        {
            //验证
            WB_WEIGHTAUDIT entity = orderbll1.GetEntity(KeyValue, "33");
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已红外一次审批！" }.ToString());
            }
            WB_WEIGHTAUDIT entityaudit = new WB_WEIGHTAUDIT();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().UserName;
            try
            {
                string Message = "审批成功。";
                //entity.STATUS = "1";
                //entity.STATUS1 = "0";
                //entity.AUDITDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //entity.AUDITUSER = ManageProvider.Provider.Current().UserName;
                //database.Update(entity, isOpenTrans);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update WB_WEIGHTAUDIT set STATUS=1,STATUS1=0,AUDITUSER='{0}',AUDITDATE='{1}' where BILLNO='{2}' and type=33", ManageProvider.Provider.Current().UserName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), KeyValue));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "单据红外一次审批成功");
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Login, "-1", "单据红外一次审批失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        //红外二审批
        [LoginAuthorize]
        public ActionResult HWCheckOrder1(string KeyValue)
        {
            //验证
            WB_WEIGHTAUDIT entity = orderbll1.GetEntity(KeyValue, "34");
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已红外二次审批！" }.ToString());
            }
            WB_WEIGHTAUDIT entityaudit = new WB_WEIGHTAUDIT();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().UserName;
            try
            {
                string Message = "审批成功。";
                //entity.STATUS = "1";
                //entity.STATUS1 = "0";
                //entity.AUDITDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                //entity.AUDITUSER = ManageProvider.Provider.Current().UserName;
                //database.Update(entity, isOpenTrans);

                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update WB_WEIGHTAUDIT set STATUS=1,STATUS1=0,AUDITUSER='{0}',AUDITDATE='{1}' where BILLNO='{2}' and type=34", ManageProvider.Provider.Current().UserName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), KeyValue));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "单据红外二次审批成功");
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Login, "-1", "单据红外二次审批失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

    }
}
