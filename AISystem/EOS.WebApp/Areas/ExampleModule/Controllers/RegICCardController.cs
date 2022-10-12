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
    /// 订单管理
    /// </summary>
    public class RegICCardController : PublicController<DP_REGICCARD>
    {
        string ModuleId = "";
        DP_REGICCARDBll iccardbll = new DP_REGICCARDBll();
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
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
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
        [LoginAuthorize]
        public override ActionResult Index()
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
        public ActionResult GetList(string ICNUMBER, string ICNO, string ISUSE, string TYPE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<DP_REGICCARD> ListData = iccardbll.GetOrderList(ICNUMBER, ICNO, ISUSE, TYPE, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(DP_REGICCARD entity, string KeyValue, string ModuleId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证
                string sql = "";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from DP_REGICCARD where ICNO='{0}' and ID!='{1}'", entity.ICNO, KeyValue);
                }
                else
                {
                    sql = String.Format("select count(1) from DP_REGICCARD where ICNO='{0}'", entity.ICNO);
                }
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "卡序号已存在" }.ToString());
                }
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from DP_REGICCARD where ICNUMBER='{0}' and ID!='{1}'", entity.ICNUMBER, KeyValue);
                }
                else
                {
                    sql = String.Format("select count(1) from DP_REGICCARD where  ICNUMBER='{0}'", entity.ICNUMBER);
                }
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "卡面编号已存在" }.ToString());
                }

                entity.ICNUMBER = entity.ICNUMBER.Replace("W", "").Replace("N", "").Replace("C", "");

                if (entity.TYPE == 1)
                {
                    entity.ICNUMBER = string.Format("W{0}", entity.ICNUMBER);
                }
                else if (entity.TYPE == 3)
                {
                    entity.ICNUMBER = string.Format("N{0}", entity.ICNUMBER);
                }
                else
                {
                    entity.ICNUMBER = string.Format("C{0}", entity.ICNUMBER);
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
                    sql = String.Format("select count(1) from DP_REGICCARD where  BILLNO='{0}'", entity.BILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
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

        /// <summary>
        /// 【卡注册管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            DP_REGICCARD entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
        public ActionResult GetEntity(string KeyValue)
        {
            DP_REGICCARD entity = iccardbll.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
        public ActionResult GetICNEntity(string KeyValue)
        {
            DP_REGICCARD entity = iccardbll.GetICNumberToEntity(KeyValue);
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
