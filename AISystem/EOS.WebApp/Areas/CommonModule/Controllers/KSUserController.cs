using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EOS.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// 用户管理控制器
    /// </summary>
    public class KSUserController : PublicController<Base_UserKS>
    {
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        Base_UserKSBll base_bll = new Base_UserKSBll();

        #region 客商用户公共方法
        /// <summary>
        /// 获取自动单据内码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }
        /// <summary>
        /// 列表（返回Json）
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetSPageListJson(string keyword, string Company, string Type, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<Base_UserKS> ListData = base_bll.GetSOrderList(keyword, Company, Type, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetCPageListJson(string keyword, string Company, string Type, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<Base_UserKS> ListData = base_bll.GetCOrderList(keyword, Company, Type, StartTime, EndTime, jqgridparam);
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
        /// 列表（返回Json）
        /// </summary>
        /// <param name="keyword">关键字</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetYPageListJson(string keyword, string Company, string Type, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<Base_UserKS> ListData = base_bll.GetYOrderList(keyword, Company, Type, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(Base_UserKS entity, string ModuleId, string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string sql = "";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from Base_UserKS where ACCOUNT='{0}' and USERID!='{1}'", entity.ACCOUNT, KeyValue);
                }
                else
                {
                    sql = String.Format("select count(1) from Base_UserKS where ACCOUNT='{0}'", entity.ACCOUNT);
                }
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "登录账号已存在" }.ToString());
                }

                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql = String.Format("select count(1) from Base_UserKS where MOBILE='{0}' and USERID!='{1}'", entity.MOBILE, KeyValue);
                }
                else
                {
                    sql = String.Format("select count(1) from Base_UserKS where MOBILE='{0}'", entity.MOBILE);
                }
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "手机号已存在" }.ToString());
                }
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    sql = String.Format("select count(1) from Base_UserKS where  CODE='{0}'", entity.CODE);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.CODE = this.BillCode();
                    }
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                    Base_DataScopePermissionBll.Instance.AddScopeDefault("999eae8b-1d09-4678-9b5f-2901205f1d98", entity.USERID, entity.COMPANY, "6", isOpenTrans);
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
        /// 返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            Base_UserKS entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        #endregion

        #region 供应商用户
        /// <summary>
        /// 供应商用户
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult KSSUserForm()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.Code = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        //供应商用户
        [LoginAuthorize]
        public ActionResult SupplyUserIndex()
        {
            return View();
        }
        //选择供应商
        [LoginAuthorize]
        public ActionResult SelectedSupply()
        {
            return View();
        }
        #endregion

        #region 客户用户
        /// 客户用户表单页
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult KSCUserForm()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.Code = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        //客户用户列表页
        [LoginAuthorize]
        public ActionResult CustomerUserIndex()
        {
            return View();
        }
        //选择客户
        [LoginAuthorize]
        public ActionResult SelectedCustomer()
        {
            return View();
        }
        #endregion

        #region 承运商用户
        /// <summary>
        /// 承运商用户
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult KSYUserForm()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.Code = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }
        //承运商用户
        [LoginAuthorize]
        public ActionResult TrafficUserIndex()
        {
            return View();
        }
        //选择承运商用户
        [LoginAuthorize]
        public ActionResult SelectedTrafficCompay()
        {
            return View();
        }
        #endregion


        #region 客商用户数据权限
        [LoginAuthorize]
        public ActionResult DataPermissionIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult DataPermissionIndex1()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult DataPermissionForm()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult DataPermissionForm1()
        {
            return View();
        }
        /// <summary>
        /// 提交订单表单（新增、编辑）
        /// </summary>
        /// <param name="KeyValue">订单主键</param>
        /// <param name="entity">订单实体</param>
        /// <param name="POOrderEntryJson">订单明细</param>
        /// <returns></returns>
        public ActionResult SubmitDataPermissionForm(string OrderEntryJson)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<DataScopePermissionEntity> OrderEntryList = OrderEntryJson.JonsToList<DataScopePermissionEntity>();

                //处理主头
                string Message = "新增成功。";
                string UserId = ManageProvider.Provider.Current().UserId;
                foreach (DataScopePermissionEntity poorderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(poorderentry.ObjectId))
                    {
                        //验证
                        string sql = String.Format("select count(1) from Base_DataScopePermission where ResourceId='{0}' and ObjectId='{1}'", poorderentry.ResourceId, poorderentry.ObjectId);
                        int res = DataFactory.Database().FindCountBySql(sql);
                        if (res == 0)
                        {
                            Base_DataScopePermission poorder = new Base_DataScopePermission();
                            poorder.Create();
                            poorder.ModuleId = "999eae8b-1d09-4678-9b5f-2901205f1d98";
                            poorder.ObjectId = poorderentry.ObjectId;
                            poorder.ResourceId = poorderentry.ResourceId;
                            poorder.SortCode = 0;
                            poorder.Category = "6";
                            database.Insert(poorder, isOpenTrans);
                        }


                    }
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


        [LoginAuthorize]
        public ActionResult DeleteUserData(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("Base_DataScopePermission", "DATASCOPEPERMISSIONID", KeyValue);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //权限实体类
        public class DataScopePermissionEntity
        {
            public string ObjectId { get; set; }
            public string OBJECTNAME { get; set; }
            public string ResourceId { get; set; }
            public string RESOURCENAME { get; set; }
            public string Category { get; set; }
        }
        #endregion




        #region 修改登录密码
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            ViewBag.Account = Request["Account"];
            return View();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="KeyValue">主键</param>
        /// <param name="Password">新密码</param>
        /// <returns></returns>
        public ActionResult ResetPasswordSubmit(string KeyValue, string Password)
        {
            try
            {
                int IsOk = 0;
                Base_UserKS base_user = new Base_UserKS();
                base_user = repositoryfactory.Repository().FindEntity(KeyValue);
                base_user.USERID = KeyValue;
                base_user.ENABLED = 1;
                base_user.PASSWORD = DESEncrypt.Encrypt(Password).ToLower();
                IsOk = repositoryfactory.Repository().Update(base_user);
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk.ToString(), "修改密码");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "密码修改成功。" }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, "-1", "密码修改失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "密码修改失败：" + ex.Message }.ToString());
            }
        }
        #endregion
    }
}