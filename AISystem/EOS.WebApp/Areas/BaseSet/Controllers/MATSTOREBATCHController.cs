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


namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class MATSTOREBATCHController : PublicController<BD_MATSTORE_BATCHNO>
    {
        string ModuleId = "";
        VBD_MATSTORE_BATCHNOBll vorderbll = new VBD_MATSTORE_BATCHNOBll();
        BD_MATSTORE_BATCHNOBll orderbll = new BD_MATSTORE_BATCHNOBll();
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
        /// 发货通知单表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MATSTOREBATCHForm()
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
        //列表
        [LoginAuthorize]
        public ActionResult MATSTOREBATCHIndex()
        {
            return View();
        }
        //明细
        [LoginAuthorize]
        public ActionResult MATSTOREBATCHDetail()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MATSTOREBATCHQuery()
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
        public ActionResult GetOrderList(string PK_STORDOC, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                
                List<BD_MATSTORE_BATCHNO> ListData = orderbll.GetOrderList(PK_STORDOC, StartTime, EndTime, jqgridparam);
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
        /// 查询
        /// </summary>
        /// <param name="PK_STORDOC"></param>
        /// <param name="PK_MATERIAL"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderList1(string BATCHNO,string PK_STORE, string PK_MATERIAL, string MATERIALGROUP,string MATERIALTYPE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
             
                List<VBD_MATSTORE_BATCHNO> ListData = vorderbll.GetOrderList(BATCHNO, PK_STORE, PK_MATERIAL,MATERIALGROUP, MATERIALTYPE, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(BD_MATSTORE_BATCHNO entity, string KeyValue, string ModuleId)
        {
            #region 验证
            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
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
        ///删除 
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            BD_MATSTORE_BATCHNO entity = repositoryfactory.Repository().FindEntity(KeyValue);
            //验证
            string sql = String.Format("select count(1) from BD_MATSTORE_BATCHNO where ID='{0}' and isuse=1", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "单据启用中，不能删除操作，请停用再删除！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                entity.ISDEL = "1";
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
        //启用审批 
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            BD_MATSTORE_BATCHNO entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据是已启用状态，无需重复启用！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "启用成功。";
                entity.ISUSE = "1";
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
        //停用反审
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            BD_MATSTORE_BATCHNO entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISUSE == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已停用，请勿重复停用！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "停用成功。";
                entity.ISUSE = "0";
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
        /// <summary>
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            BD_MATSTORE_BATCHNO entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

    }
}
