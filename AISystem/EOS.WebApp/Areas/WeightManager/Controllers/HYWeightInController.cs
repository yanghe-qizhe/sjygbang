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

namespace EOS.WebApp.Areas.WeightManager.Controllers
{
    public class HYWeightInController : PublicController<WB_RAILWAY_DTL>
    {
        string ModuleId = "";
        WB_RAILWAY_DTLBll orderbll = new WB_RAILWAY_DTLBll();
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

        //
        // GET: /WeightManager/WeightIn/
        [LoginAuthorize]
        public ActionResult HYWeightInIndex()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult HYWeightInForm()
        {
            string op = Request["op"];
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue) || op == "add")
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
                ViewBag.DepartmentId = ManageProvider.Provider.Current().DepartmentId;
                ViewBag.DepartmentName = ManageProvider.Provider.Current().DepartmentName;
                ViewBag.DepartmentPId = ManageProvider.Provider.Current().DepartmentPId;
                ViewBag.DepartmentPName = ManageProvider.Provider.Current().DepartmentPName;
            }
            return View();
        }

        public ActionResult HYWeightInQuery()
        {
            return View();
        }
        /// <summary>
        /// 设计要求采购订单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SELECTPOORDERS()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult SELECTHYPCBillSOF()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult InvalidForm()
        {
            return View();
        }

        /// 采购磅单（返回Json）
        [LoginAuthorize]
        public ActionResult GetOrderList(string BillNo, string VBillCode, string ContractBillNo, string DHBillNo, string Supply, string Material, string Cars, string UPLOAD, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_RAILWAY_DTL> ListData = orderbll.GetRKOrderList(BillNo, VBillCode, ContractBillNo, DHBillNo, Supply, Material, Cars, "2,3", UPLOAD, StartTime, EndTime, jqgridparam);
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
        public ActionResult SubmitOrderForm(string KeyValue, WB_RAILWAY_DTL entity, string ModuleId)
        {
            #region 验证
            //验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {

                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    WB_RAILWAY_DTL obj = repositoryfactory.Repository().FindEntity(KeyValue);
                    entity.Modify(KeyValue);

                    if (!string.IsNullOrEmpty(obj.PK_EID) && !string.IsNullOrEmpty(entity.PK_EID))
                    {
                        if (obj.PK_EID != entity.PK_EID)
                        {
                            strSql.Clear();
                            strSql.Append(string.Format("update DP_HYPOCARSORDER set IsUse='{0}' where ID='{1}'", 1, obj.PK_EID));
                            repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                        }
                    }
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from WB_WEIGHT where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    if (entity.BALANCEMETHOD == "JSFS01")
                    {
                        //长钢结算-静态衡
                        entity.TYPE = "2";
                    }
                    else
                    {
                        //供应商结算-动态衡
                        entity.TYPE = "3";
                    }
                    entity.DEF9 = "1";
                    entity.WEIGHTER = ManageProvider.Provider.Current().UserName;// "磅房管理员";
                    database.Insert(entity, isOpenTrans);
                }
                if (!string.IsNullOrEmpty(entity.PK_EID))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_HYPOCARSORDER set IsUse='{0}' where ID='{1}'", 4, entity.PK_EID));
                    repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                }

                #region 收料

                APP_HANDHYORDER app_entity = new APP_HANDHYORDER();
                app_entity.Create();

                //重写
                app_entity.BILLNO = entity.BILLNO;
                app_entity.ICCARD = "";
                app_entity.BILLFROM = "1";
                app_entity.OPERTYPE = "1";
                app_entity.TYPE = "3";

                app_entity.PK_STORE = entity.PK_RECEIVESTORE;
                app_entity.STORE = entity.RECEIVESTORE;

                app_entity.PDAYF = string.Format("{0}", entity.CALCAMOUNT);

                app_entity.PDANO = "/";

                app_entity.PDAKZ1 = entity.KOUAMOUNT.ToString();
                app_entity.PDAKZ2 = "0";

                app_entity.PK_DEPARTMENT = entity.PK_DEPARTMENT;
                app_entity.DEPARTMENT = entity.DEPARTMENTNAME;
                app_entity.PK_LEADERDEPA = entity.PK_COMPANY;
                app_entity.LEADERDEPA = entity.COMPANYNAME;

                app_entity.STATUS = "1";
                app_entity.OPERUSER = ManageProvider.Provider.Current().UserName;
                app_entity.OPERTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    database.Update(app_entity, isOpenTrans);
                }
                else
                {
                    database.Insert(app_entity, isOpenTrans);
                }
                #endregion
                database.Commit();
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
            string sql = String.Format("select count(1) from WB_RAILWAY_DTL where BILLNO='{0}' and ISVALID=1", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "只有状态为作废的计量单能删除操作！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("WB_RAILWAY_DTL", "BILLNO", KeyValue);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //审批
        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            WB_RAILWAY_DTL entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ADUIT == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "此数据已审批！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "审批成功。";
                entity.ADUIT = "1";
                entity.ADUITDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                entity.ADUITUSER = ManageProvider.Provider.Current().UserName;
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

        //手动推单
        [LoginAuthorize]
        public ActionResult HandOrder(string KeyValue)
        {
            //验证
            WB_RAILWAY_DTL entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ADUIT == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "此数据未审批！" }.ToString());
            }

            try
            {
                string Message = "推单成功。";

                if (true)
                {
                    return Content(new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}:{1}", Message, "") }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", "") }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }



        //作废
        [LoginAuthorize]
        public ActionResult InvalidOrder(string KeyValue, string MEMO)
        {
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            //验证
            WB_RAILWAY_DTL entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ISVALID == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该火运采购计量单已作废！" }.ToString());
            }
            if (entity.UPLOAD == "1" && ManageProvider.Provider.Current().Account != "System")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该火运采购计量单已推单，请与管理员联系！" }.ToString());
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "作废成功。";
                entity.ISVALID = "0";
                entity.DEF9 = string.Format("作废:{0};", MEMO);
                database.Update(entity, isOpenTrans);
                if (!string.IsNullOrEmpty(entity.PK_EID))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_HYPOCARSORDER set IsUse='{0}' where ID='{1}'", 5, entity.PK_EID));
                    repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                }
                database.Commit();
                string msg = string.Format("火运采购计量单{0}车号{1}毛{2}皮{3}作废成功。{4}", entity.BILLNO, entity.CARNUM, entity.MAOAMOUNT, entity.PIAMOUNT, MEMO);
                string Account = ManageProvider.Provider.Current().Account;
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", msg);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 【到货单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            WB_RAILWAY_DTL entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            VNC_PO_ORDER_B porder = DataFactory.Database().FindEntityBySql<VNC_PO_ORDER_B>(string.Format("select * from vnc_po_order_bhy where PK_ORDER_B='{0}'", entity.PK_TASK_DTL));
            JsonData = JsonData.Insert(1, "\"NASTNUM\":\"" + porder.NASTNUM + "\",");
            JsonData = JsonData.Insert(1, "\"YLNUM\":\"" + porder.YLNUM + "\",");
            return Content(JsonData);
        }
    }
}
