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
    public class  HYRECMATERIAController : PublicController<APP_HANDHYORDER>
    {
        string ModuleId = "";
        APP_HANDHYORDERBll orderbll = new APP_HANDHYORDERBll();
        VWB_RAILWAY_DTL_HYSHBll vpcbll = new VWB_RAILWAY_DTL_HYSHBll();

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
        /// 新增、编辑页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult HYBillRECFForm()//POBillCOFForm
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                //ViewBag.BillNo = this.BillCode();
                //ViewBag. = this.BillCode();
                ViewBag.ACCEPTUNITCODE = ManageProvider.Provider.Current().DepartmentPId;
                ViewBag.ACCEPTUNITNAME = ManageProvider.Provider.Current().DepartmentPName;
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }

        /// <summary>
        /// 新增、编辑页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult HYBillRECFEdit()
        {
            return View();
        }

        //列表页面
        [LoginAuthorize]
        public ActionResult HYBillRECFIndex()
        {
            return View();
        }

        //明细页面
        [LoginAuthorize]
        public ActionResult HYBillRECFDetail()
        {
            return View();
        }
        /// <summary>
        /// 查询页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult HYBillRECFQuery()
        {
            return View();
        }

        /// <summary>
        /// 点击弹出页面
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult HYSELECTBillRECF()
        {
            return View();
        }

        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="TaskNo">单据编号</param>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetOrderList(string TaskNo,string User, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            //User = ManageProvider.Provider.Current().UserId;//控制用户
            User = ManageProvider.Provider.Current().DepartmentId;//控制部门
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDHYORDERBll vorderbll = new VAPP_HANDHYORDERBll();
                List<VAPP_HANDHYORDER> ListData = vorderbll.GetOrderList(TaskNo,User,StartTime, EndTime,"3","1", jqgridparam);
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
        /// 查询（返回Json）
        /// </summary>
        /// <param name="TaskNo">单据编号</param>
        /// <param name="Material">物料</param>
        /// <param name="Store">仓库</param>
        /// <param name="Cars">车辆</param>
        /// <param name="Supply">供应商</param>
        /// <param name="StartTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetSeOrderList(string TaskNo, string Material, string Store, string Cars, string Supply,string User, string StartTime, string EndTime,JqGridParam jqgridparam)
        {
            //User = ManageProvider.Provider.Current().UserId;//控制用户
            User = ManageProvider.Provider.Current().DepartmentId;//控制部门
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VAPP_HANDHYORDERBll vorderbll = new VAPP_HANDHYORDERBll();
                List<VAPP_HANDHYORDER> ListData = vorderbll.GetSeOrderList(TaskNo, Material, Store, Cars, Supply, User, StartTime, EndTime, "3","1", jqgridparam);
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
        /// 火运过磅单显示查询
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="MATERIAL"></param>
        /// <param name="SUPPLY"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetHYWbOrderList(string StartTime, string EndTime, string BillNo, string VbillCode, string Pk_Eid, string Pk_Supply, string Supply, string Pk_Material, string Material, string Cars, string RailwayCode, string Type, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_RAILWAY_DTL_HYSH> ListData = vpcbll.GetHYWBOrderList(BillNo, VbillCode, Pk_Eid, Pk_Supply, Supply, Pk_Material,Material, Cars, RailwayCode, Type, StartTime, EndTime, jqgridparam);
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
        /// 通过火运磅单获取数据信息
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCSHGetHYData(string KeyValue)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    return Content(orderbll.GetHYData(KeyValue));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
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
            string sql = String.Format("select count(1) from APP_HANDHYORDER where ID='{0}' and STATUS!=0", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "已收货或退货确认，不能删除操作！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("APP_HANDHYORDER", "ID", KeyValue);
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
        /// 返回对象JSON 编辑 详细
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            VAPP_HANDHYORDERBll overbll = new VAPP_HANDHYORDERBll();
            VAPP_HANDHYORDER entity = overbll.GetEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        /// <summary>
        /// 编辑页面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public override ActionResult SetForm(string KeyValue)
        {
            List<VWB_RAILWAY_DTL_HYSH> entitydetails = DataFactory.Database().FindList<VWB_RAILWAY_DTL_HYSH>("BILLNO", KeyValue, "CREATEDATE", "ASC");
            return Content("{\"D\":" + entitydetails.ToJson() + "}");
        }
        /// <summary>
        /// 通过对应物料供应商
        /// </summary>
        /// <param name="ICNO"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetData(string ICNO, string MATERIAL, string SUPPLY)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(ICNO))
                {
                    return Content(vpcbll.GetData(ICNO, MATERIAL, SUPPLY));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(ICNO, OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 火运收料
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <param name="HYOrderDetailJson"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawHYSave(APP_HANDHYORDER entity, string KeyValue, string Pk_Store,string Store,string HYOrderDetailJson)
        {
           
            if (Pk_Store == "" || Pk_Store == "")
            {
                return Content(new JsonMessage { Success = false, Code = "0", Message = "收货仓库不能为空" }.ToString());
            }
            entity.PK_STORE = Pk_Store;
            entity.STORE = Store;
            entity.PK_LEADERDEPA = ManageProvider.Provider.Current().DepartmentPId;
            entity.LEADERDEPA = ManageProvider.Provider.Current().DepartmentPName;
            entity.PK_DEPARTMENT = ManageProvider.Provider.Current().DepartmentId;
            entity.DEPARTMENT = ManageProvider.Provider.Current().DepartmentName;
            int IsOk = 0;
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    //编辑
                    IsOk = orderbll.PCRawHYEdit(entity,KeyValue);
                    if (IsOk != 1)
                    {
                        return Content(new JsonMessage { Success = true, Code = "0", Message = "操作失败。" }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
                    }
                }
                else
                {
                    //新增
                    IsOk = orderbll.PCRawHYSave(entity, HYOrderDetailJson);
                    if (IsOk == 2)
                    {
                        return Content(new JsonMessage { Success = false, Code = "0", Message = "当前有已收料的磅单" }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "火运收料操作成功。" }.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
    }
}
