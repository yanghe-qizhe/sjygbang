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
    public class LGQYTASKController : PublicController<LG_QY_TASK>
    {
        string ModuleId = "";

        LG_QY_TASKBll orderbll = new LG_QY_TASKBll();
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();

        [LoginAuthorize]
        public ActionResult LGQYTASKIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult LGJHQYTASKIndex()
        {
            return View();
        }


        [LoginAuthorize]
        public ActionResult LGQYTASKDetail()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult LGQYTASKQuery()
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
        public ActionResult GetOrderList(string BillNo, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<LG_QY_TASK> ListData = orderbll.GetOrderList(BillNo, StartTime, EndTime, jqgridparam);
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
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="BillNo">制单编号</param>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetQYOrderList(string BillNo, string Cars, string Customer, string DEF4, string Material, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VLG_QY_TASKBll vorderbll = new VLG_QY_TASKBll();
                List<VLG_QY_TASK> ListData = vorderbll.GetOrderList(BillNo, Cars, Customer, DEF4, Material, StartTime, EndTime, jqgridparam);
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

        public ActionResult GetGCOrderList(string BillNo, string Cars, string Customer, string Material, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VLG_QY_TASKBll vorderbll = new VLG_QY_TASKBll();
                List<VLG_QY_TASK> ListData = vorderbll.GetOrderList1("1", BillNo, Cars, Customer, Material, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetJHOrderList(string BillNo, string Cars, string Customer, string Material, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VLG_QY_TASKBll vorderbll = new VLG_QY_TASKBll();
                List<VLG_QY_TASK> ListData = vorderbll.GetOrderList1("2", BillNo, Cars, Customer, Material, StartTime, EndTime, jqgridparam);
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


        //出厂
        [LoginAuthorize]
        public ActionResult OutOrder(string KeyValue)
        {
            //验证
            LG_QY_TASK entity = repositoryfactory.Repository().FindEntity(KeyValue);

            if (entity.BL_STATUS == "4")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该汽运提货单已作废！" }.ToString());
            }
            //if (entity.STATUS == "完成")
            //{
            //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "该汽运提货单已完成！" }.ToString());
            //}

            if (entity.STATUS != "完成")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该汽运提货单计量未完成！" }.ToString());
            }

            string sql = String.Format("select count(1) from WB_WEIGHT where FINISH=0 and PK_TASK='{0}'", entity.TASK_ID);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("该汽运提货单有未二次计量的明细") }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "强制出厂成功。";
                //entity.STATUS = "完成";
                //database.Update(entity, isOpenTrans);

                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //门禁出厂
                strSql.Append(string.Format("update wb_weight_task set finish='1',status='出厂',OUTDATE='{1}' where PK_TASK='{0}'", entity.TASK_ID, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                //过磅出厂
                strSql.Clear();
                strSql.Append(string.Format("update wb_weight set finish='1',status='完成' where PK_TASK='{0}'", entity.TASK_ID));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                //任务单出厂
                strSql.Clear();
                strSql.Append(string.Format("update LG_QY_TASK set STATUS='完成' where TASK_ID='{0}'", entity.TASK_ID));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            VLG_QY_TASKBll vorderbll = new VLG_QY_TASKBll();
            VLG_QY_TASK entity = vorderbll.SetFormControl(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
