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
using System.Collections;


namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    /// <summary>
    /// 订单管理
    /// </summary>
    public class BINDRFIDController : PublicController<BD_RFID>
    {
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        VBD_RFIDBll rfidbll = new VBD_RFIDBll();
        /// <summary>
        /// 获取自动单据内码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }

        [LoginAuthorize]
        public ActionResult BINDRFIDIndex()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult BINDRFIDFIXEDIndex()
        {
            return View();
        }

        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult BINDRFIDForm()
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
        public ActionResult BINDRFIDFIXEDForm()
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
        public ActionResult GetOrderList(string keyword, string Type, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_RFID> ListData = rfidbll.GetOrderList(keyword, Type, StartTime, EndTime, jqgridparam);
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

        public ActionResult GetGDOrderList(string keyword, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_RFID> ListData = rfidbll.GetOrderList(keyword, "1", StartTime, EndTime, jqgridparam);
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

        public ActionResult SubmitOrderForm(BD_RFID entity, string KeyValue, string ModuleId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //验证

                string sql = String.Format("select * from BD_CARS  where  EPCCODE='{0}' and ID!='{1}'", entity.RFID, entity.CARS);
                BD_CARS car = DataFactory.Database().FindEntityBySql<BD_CARS>(sql);
                if (!string.IsNullOrEmpty(car.ID))
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("RF卡号:{0}已登记车号为{1}", car.EPCCODE, car.NAME) }.ToString());
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
                    sql = String.Format("select count(1) from BD_RFID where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                }
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update BD_CARS set EPCCODE='{0}'where ID='{1}'", entity.RFID, entity.CARS));
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

        //启用
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            VBD_RFID entity = rfidbll.GetEntity(KeyValue);
            if (entity.ISUSE == 1)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该RF卡号已启用！" }.ToString());
            }
            string Account = ManageProvider.Provider.Current().Account;
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "启用成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update BD_RFID set ISUSE=1 where ID='{0}'", KeyValue));
                int res = repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "启用成功！");
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        //停用
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            VBD_RFID entity = rfidbll.GetEntity(KeyValue);
            if (entity.ISUSE == 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该RF卡号已停用！" }.ToString());
            }
            string Account = ManageProvider.Provider.Current().Account;
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "停用成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update BD_RFID set ISUSE=0 where ID='{0}'", KeyValue));
                int res = repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                strSql.Clear();
                strSql.Append(string.Format("update BD_CARS set EPCCODE='' where ID='{0}'", entity.CARS));
                res = repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", "停用成功！");
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
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            string sql = String.Format("select count(1) from BD_RFID where ID='{0}' and ISUSE=0", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为无效数据能删除操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("BD_RFID", "ID", KeyValue);
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
            VBD_RFID entity = rfidbll.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            JsonData = JsonData.Insert(1, "\"EPCCODE\":\"" + entity.RFID + "\",");
            JsonData = JsonData.Insert(1, "\"ID\":\"" + entity.CARS + "\",");
            JsonData = JsonData.Insert(1, "\"NAME\":\"" + entity.CARSNAME + "\",");
            return Content(JsonData);
        }


    }
}
