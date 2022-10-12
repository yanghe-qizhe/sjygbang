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

namespace EOS.WebApp.Areas.CommonModule.Controllers
{
    public class PDAPOWEREntity
    {
        public string PK_STORDOC { get; set; }
        public string STORDOCNAME { get; set; }
    }
    public class PdaPowerController : PublicController<APP_HANDUSER>
    {
        string ModuleId = "";
        APP_HANDUSERBll orderbll = new APP_HANDUSERBll();
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
        public ActionResult PdaPowerFForm()
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
        public ActionResult PdaPowerIndex()
        {
            return View();
        }
        //明细
        [LoginAuthorize]
        public ActionResult PdaPowerDetail()
        {
            return View();
        }
        /// <summary>
        /// 用户权限
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult AllotPermission()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PdaPowerFQuery()
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
        public ActionResult GetOrderList(string TaskNo,string USERGROUP, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<APP_HANDUSER> ListData = orderbll.GetOrderList(TaskNo, USERGROUP, StartTime, EndTime, jqgridparam);
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
        /// 工序查询
        /// </summary>
        /// <param name="TaskNo"></param>
        /// <param name="StoveNum"></param>
        /// <param name="Acceptunit"></param>
        /// <param name="Material"></param>
        /// <param name="Scale"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetSerOrderList(string TaskNo, string Name, string Pk_Stordoc, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                APP_HANDUSERBll vorderbll = new APP_HANDUSERBll();
                List<APP_HANDUSER> ListData = vorderbll.GetSerOrderList(TaskNo, Name, Pk_Stordoc, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(APP_HANDUSER entity, string KeyValue, string ModuleId, string OrderEntryJson)
        {
            IDatabase database = DataFactory.Database();
            entity.NUM = entity.ID;
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<PDAPOWEREntity> OrderEntryList = OrderEntryJson.JonsToList<PDAPOWEREntity>();
                string storeidlist = "", storenamelist = "";
                entity.PWD = APP_HANDUSER.Encrypt(entity.PWD);
                foreach (PDAPOWEREntity orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.PK_STORDOC))
                    {
                        storeidlist += orderentry.PK_STORDOC + ",";
                        storenamelist += orderentry.STORDOCNAME + ",";
                    }
                }
                string sql = "";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from APP_HANDUSER where ACCOUNT='{0}' and ID!='{1}'", entity.ACCOUNT, KeyValue);
                }
                else
                {
                    sql = String.Format("select count(1) from APP_HANDUSER where ACCOUNT='{0}'", entity.ACCOUNT);
                }
                int res = DataFactory.Database().FindCountBySql(sql);

                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("此用户工号：{0}已存在", entity.ACCOUNT) }.ToString());
                }
                if (entity.DEF2 == "" || entity.DEF2 == null)
                {
                    entity.DEF1 = "";
                }
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    entity.DEF1 = storeidlist.TrimEnd(',');
                    entity.DEF2 = storenamelist.TrimEnd(',');
                    if (entity.DEF2 == "" || entity.DEF2 == null)
                    {
                        entity.DEF1 = "";
                    }
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    sql = String.Format("select count(1) from APP_HANDUSER where  ID='{0}'", entity.ID);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        entity.ID = this.BillCode();
                    }
                    entity.Create();
                    entity.DEF1 = storeidlist.TrimEnd(',');
                    entity.DEF2 = storenamelist.TrimEnd(',');
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
            //验证
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("APP_HANDUSER", "ID", KeyValue);
                //DEL_CARSORDERS(KeyValue, database, isOpenTrans);
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
        /// 权限授权提交事件
        /// </summary>
        /// <param name="ModuleId">访问权限值</param>
        /// <param name="ModuleButtonId">操作权限值</param>
        /// <param name="ViewDetailId">视图权限值</param>
        /// <param name="ObjectId">对象ID</param>
        /// <param name="Category">分类</param>
        /// <returns></returns>
        public ActionResult AuthorizedSubmit(string ModuleId, string StoreId,  string ObjectId, string Category)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                List<DbParameter> parameter = new List<DbParameter>();
                parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + "ObjectId", ObjectId));
                parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + "Category", Category));
                #region 访问
                string[] arrayModuleId = ModuleId.Split(',');
                StringBuilder sbDelete_Module = new StringBuilder("delete from Base_ModuleScjPermission where ObjectId =" + DbHelper.DbParmChar + "ObjectId AND Category=" + DbHelper.DbParmChar + "Category");
                database.ExecuteBySql(sbDelete_Module, parameter.ToArray(), isOpenTrans);
                int index = 1;
                foreach (var item in arrayModuleId)
                {
                    if (item.Length > 0)
                    {
                        Base_ModuleScjPermission entity = new Base_ModuleScjPermission();
                        entity.ModulePermissionId = CommonHelper.GetGuid;
                        entity.ObjectId = ObjectId;
                        entity.Category = Category;
                        entity.ModuleId = item;
                        entity.SortCode = index;
                        entity.Create();
                        database.Insert(entity, isOpenTrans);
                        index++;
                    }
                }
                #endregion

                #region 操作
                string[] arrayStoreId = StoreId.Split(',');
                StringBuilder sbDelete_Store = new StringBuilder("delete from Base_ObjectUserRelationScj where ObjectId = " + DbHelper.DbParmChar + "ObjectId AND Category=" + DbHelper.DbParmChar + "Category");
                database.ExecuteBySql(sbDelete_Store, parameter.ToArray(), isOpenTrans);
                index = 1;
                foreach (var item in arrayStoreId)
                {
                    if (item.Length > 0)
                    {
                        Base_ObjectUserRelationScj entity = new Base_ObjectUserRelationScj();
                        entity.ObjectUserRelationId = CommonHelper.GetGuid;
                        entity.ObjectId = ObjectId;
                        entity.UserId = item;
                        entity.Category = Category;
                        entity.SortCode = index;
                        entity.CompanyId = "";
                        entity.Create();
                        index++;
                        database.Insert(entity, isOpenTrans);
                        index++;
                    }
                }
                #endregion

      
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = "操作成功。" }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
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
            APP_HANDUSER entity = repositoryfactory.Repository().FindEntity(KeyValue);
            entity.PWD = APP_HANDUSER.Decrypt(entity.PWD);//解密
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

    }
}
