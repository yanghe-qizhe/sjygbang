using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    /// <summary>
    /// 倒运
    /// </summary>
    public class FixedCardController : PublicController<WB_FIXEDCARD>
    {
        string ModuleId = "";
        WB_FIXEDCARDBll bill = new WB_FIXEDCARDBll();
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

        public ActionResult AuditForm()
        {
            return View();
        }
        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        public override ActionResult Form()
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

        public ActionResult QueryIndex()
        {
            return View();
        }
        /// <summary>
        /// 列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetList(string Drivers, string TrafficCompany, string ICCard, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<WB_FIXEDCARD> ListData = bill.GetList(Drivers, TrafficCompany, ICCard, jqgridparam);
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
        /// 审批明细列表（返回Json）
        /// </summary>
        /// <param name="ID">主键</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = bill.GetOrderEntryList(KeyValue),
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
        public ActionResult SubmitOrderForm(WB_FIXEDCARD entity, string KeyValue, string ModuleId, string op)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            WB_FIXEDCARD old_entity = null;
            try
            {

                if (op == "hedit")
                {
                    DP_REGICCARDBll ic_bill = new DP_REGICCARDBll();

                    DP_REGICCARD ic_entity = ic_bill.GetEntity(entity.ICCARD);

                    old_entity = repositoryfactory.Repository().FindEntity(KeyValue);

                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Append(string.Format("update wb_weight_task set ICCARD='{0}',ICNUMBER='{1}' where ICCARD='{2}' and finish=0", entity.ICCARD, ic_entity.ICNUMBER, old_entity.ICCARD));

                    int res = repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                    strSql.Clear();
                    strSql.Append(string.Format("update wb_weight set ICCARD='{0}' where ICCARD='{1}' and finish=0", entity.ICCARD, old_entity.ICCARD));
                    res += repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                    //更新收发货表
                    strSql.Clear();
                    strSql.Append(string.Format("update App_Handorder set ICCARD='{0}' where ICCARD='{1}' and STATUS='0'", entity.ICCARD, old_entity.ICCARD));
                    res += repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                }
                else
                {
                    //验证
                    string sql = string.Format("select count(1) from DP_REGICCARD where ICNO='{0}' and isuse=1 and Type=1 ", entity.ICCARD);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res == 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "卡未注册或卡不是内部车辆卡" }.ToString());
                    }
                    //判断卡是否重复注册
                    if (!string.IsNullOrEmpty(KeyValue))
                    {
                        sql = String.Format("select count(1) from WB_FIXEDCARD where ICCARD='{0}' and ID!='{1}'", entity.ICCARD, KeyValue);
                    }
                    else
                    {
                        sql = String.Format("select count(1) from WB_FIXEDCARD where  ICCARD='{0}'", entity.ICCARD);
                    }
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "此卡已注册不允许重新注册" }.ToString());
                    }
                    //判断司机身份证是否重复
                    if (!string.IsNullOrEmpty(KeyValue))
                    {
                        sql = String.Format("select count(1) from WB_FIXEDCARD where PSDNO='{0}' and ID!='{1}'", entity.PSDNO, KeyValue);
                    }
                    else
                    {
                        sql = String.Format("select count(1) from WB_FIXEDCARD where  PSDNO='{0}'", entity.PSDNO);
                    }
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "司机身份证已注册不允许重新注册" }.ToString());
                    }
                    string msg = "";
                    //判断IC卡是否有未完成业务
                    if (!string.IsNullOrEmpty(KeyValue))
                    {

                        old_entity = repositoryfactory.Repository().FindEntity(KeyValue);
                        sql = String.Format("select count(1) from wb_weight_zx where ICCARD='{0}' and finish=0", old_entity.ICCARD);
                        res = DataFactory.Database().FindCountBySql(sql);
                        if (res > 0)
                        {
                            msg = string.Format("IC卡号{0}有未完成的业务！", old_entity.ICCARD);
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = "msg" }.ToString());
                        }
                    }

                    sql = String.Format("select count(1) from wb_weight where  ICCARD='{0}' and finish=0", entity.ICCARD);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        msg = string.Format("IC卡号{0}有未完成的业务！", entity.ICCARD);
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "msg" }.ToString());
                    }

                }
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from WB_FIXEDCARD where  ID='{0}'", entity.ID);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.ID = this.BillCode();
                    }
                    entity.Create();
                    database.Insert(entity, isOpenTrans);

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

        //空车审批
        [LoginAuthorize]
        public ActionResult  CheckOrder(string KeyValue,string SIGNDATE, string MEMO)
        {
            WB_FIXEDCARD entity = repositoryfactory.Repository().FindEntity(KeyValue);
            WB_FIXEDCARD_CHECK entityaudit = new WB_FIXEDCARD_CHECK();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().Account;
            try
            {
                string Message = "审批成功。";
                if (!string.IsNullOrEmpty(SIGNDATE))
                {
                    entity.SIGNDATE = SIGNDATE;
                }
                database.Update(entity, isOpenTrans);

                entityaudit.Create();
                entityaudit.MAINID = KeyValue;
                entityaudit.SIGNDATE = SIGNDATE;
                entityaudit.MEMO = MEMO;
                database.Insert(entityaudit, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "固定卡绑定审批成功");
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "固定卡绑定审批失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        public ActionResult Delete(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("WB_FIXEDCARD", "ID", KeyValue);
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
            WB_FIXEDCARD entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        public ActionResult GetScript()
        {
            string strJson = "";
            strJson += "var key = '201003220000';var NiceKey='201003220000';";
            strJson += " function readData(pie_arr) {";
            strJson += " var iRet = CardOpen(); if (iRet == 0) { for (var i = 0; i < pie_arr.length; i++) { var pie = pie_arr[i]; if(pie=='11'){ iRet = CardCheckPwdStatus(pie, NiceKey);}else{ iRet = CardCheckPwdStatus(pie, key);} if (iRet == 0) { getVal(pie); } } iRet = CardClose(); }";
            strJson += "}";
            strJson += " function writeData(pie_arr) {";
            strJson += "   var iRet = CardOpen(); if (iRet == 0) {   for (var i = 0; i < pie_arr.length; i++) { var pie = pie_arr[i];if(pie=='11'){ iRet = CardCheckPwdStatus(pie, NiceKey);}else{ iRet = CardCheckPwdStatus(pie, key);} if (iRet == 0) { setVal(pie); } } iRet = CardClose(); }";
            strJson += "}";
            strJson += "   function CardCheckPwdStatus(pie, key) {";
            strJson += "        var res = 1; MWRFATL.RF_LoadKey(0, pie, key); var iRet = MWRFATL.LastRet; if (iRet) { res = 1; $('#msg').text('读写器装载密码失败！'); } else { MWRFATL.RF_Authentication(0, pie); iRet = MWRFATL.LastRet; if (iRet) { res = 2; $('#msg').text('验证密码失败！'); } else { res = 0; } } return res;";
            strJson += " }";
            return Content(strJson);

        }
    }
}
