using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Utilities;
using System.Diagnostics;
using EOS.Business;
using EOS.Entity;
using EOS.DataAccess;
using EOS.Repository;
using System.Data.Common;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Threading;
using System.Text.RegularExpressions;

namespace EOS.WebApp.Areas.ZJGCManager.Controllers
{

    public class ZJGCCheckController : PublicController<BD_ZJGCRESULT>
    {
        string ModuleId = "";

        QC_ZJGCCYBll pcbll = new QC_ZJGCCYBll();
        VQC_ZJGCCYBll vpcbll = new VQC_ZJGCCYBll();
        VBD_ZJGCRESULTBll vorderbll = new VBD_ZJGCRESULTBll();
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


        #region 页面返回
        public ActionResult CheckReportIndex()
        {
            ViewBag.CreateUser = ManageProvider.Provider.Current().UserName;
            return View();
        }
        //化验录入 
        [LoginAuthorize]
        public ActionResult ZJGCCheckIndex()
        {
            return View();
        }

        //化验审批
        [LoginAuthorize]
        public ActionResult ZJGCCheckIndex1()
        {
            return View();
        }


        [LoginAuthorize]
        public ActionResult ZJGCCheckForm()
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

        #endregion

        #region 数据列表
        /// <summary>
        /// 获取采样化学检测列表
        /// </summary>
        /// <param name="PRINTCODE"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderList(string BILLNO, string PRINTCODE, string CHESTATUS, string ZJRESULT, string AUDITSTATUS, string MATERIAL, string MATERIALNAME, string MATERIALSPEC, string PK_ORG, string CREUSER, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJGCRESULT> ListData = vorderbll.GetOrderList(BILLNO, PRINTCODE, CHESTATUS, ZJRESULT, AUDITSTATUS, MATERIAL, MATERIALNAME, MATERIALSPEC, PK_ORG, CREUSER, StartTime, EndTime, jqgridparam);
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





        public ActionResult SetFormControl(string KeyValue)
        {
            VBD_ZJGCRESULT entity = vorderbll.GetEntity(KeyValue);
            return Content(entity.ToJson());
        }



        public ActionResult GetEntityHY(string KeyValue)
        {
            KeyValue = KeyValue.Trim();
            //验证
            string sql = String.Format("select count(1) from BD_ZJGCRESULT where PRINTCODE='{0}'", KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该批次已生成化验单！" }.ToString());
            }
            VQC_ZJGCCYBll vpcbll = new VQC_ZJGCCYBll();
            VQC_ZJGCCY entity = vpcbll.GetEntityHY(KeyValue);
            return Content(entity.ToJson());
        }



        /// <summary>
        /// 获取质检结果明细表信息
        /// </summary>
        /// <param name="MAINID"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderEntryList(string KeyValue)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJGCRESULTDETAILS> ListData = vorderbll.GetOrderEntryList(KeyValue);
                var JsonData = new
                {
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
        public ActionResult GetOrderEntryListHY(string Supply, string Material)
        {
            try
            {
                Supply = Supply.Trim();
                Material = Material.Trim();
                List<VBase_ZJFAConfig_B> list = vorderbll.GetOrderEntryListHY(Supply, Material);
                if (list.Count == 0)
                {
                    list = vorderbll.GetOrderEntryListHY("", Material);
                }
                var JsonData = new
                {
                    rows = list,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        #endregion


        #region 业务操作
        public static bool IsNumeric(string value)
        {
            return Regex.IsMatch(value, @"^[+-]?\d*[.]?\d*$");
        }

        //化验保存
        [HttpPost]
        public ActionResult SubmitOrderForm(BD_ZJGCRESULT entity, string OrderEntryJson, string KeyValue, string ModuleId)
        {
            #region 验证

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {  //明细行数据
                List<BD_ZJGCRESULTDETAILS> OrderEntryList = OrderEntryJson.JonsToList<BD_ZJGCRESULTDETAILS>();

                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    if (!ManageProvider.Provider.Current().IsSystem)
                    {
                        int ZCUser = ManageProvider.Provider.Current().ZCUser;
                        if (ZCUser == 0 && entity.CREUSER != ManageProvider.Provider.Current().UserName)
                        {
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("化验单号:{0}，创建人为:{1}，您没有权限修改！", entity.BILLNO, entity.CREUSER) }.ToString());
                        }
                    }
                    database.Delete<BD_ZJGCRESULTDETAILS>("MAINID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    string sql = String.Format("select count(1) from BD_ZJGCRESULT where PRINTCODE='{0}' ", entity.PRINTCODE);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样单号:{0}已生成化验单！", entity.PRINTCODE) }.ToString());
                    }
                    sql = String.Format("select count(1) from QC_ZJGCCY where PRINTCODE='{0}' ", entity.PRINTCODE);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res == 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样单号:{0}不存在！", entity.PRINTCODE) }.ToString());
                    }
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from BD_ZJGCRESULT where  BILLNO='{0}'", entity.BILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                }


                //处理明细
                int index = 1;
                string ZJRESULTMEMO = "";
                //bool bol = false;
                foreach (BD_ZJGCRESULTDETAILS orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.ZJITEM))
                    {
                        orderentry.Create();
                        orderentry.MAINID = entity.ID;
                        database.Insert(orderentry, isOpenTrans);
                        if (!string.IsNullOrEmpty(orderentry.ZJITEMNAME))
                        {
                            ZJRESULTMEMO += string.Format("{0}{1}:{2}({3});", orderentry.ZJITEMNAME, orderentry.DEF1, orderentry.ZJVALUE, (orderentry.ZJBZRESULT == "0" ? "合格" : "不合格"));
                        }
                        index++;
                    }
                }

                //更新为采样化验
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update QC_ZJGCCY set TYPE=5,DEF10='{0}' where BILLNO='{1}'", entity.BILLNO, entity.CYID));
                database.ExecuteBySql(strSql, isOpenTrans);
                //更新检验结果
                strSql.Clear();
                ZJRESULTMEMO = ZJRESULTMEMO.TrimEnd(';');
                strSql.Append(string.Format("update BD_ZJGCRESULT set ZJRESULTMEMO='{0}' where ID='{1}'", ZJRESULTMEMO, entity.ID));
                database.ExecuteBySql(strSql, isOpenTrans);
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
            BD_ZJGCRESULT entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (!ManageProvider.Provider.Current().IsSystem && entity.CREUSER != ManageProvider.Provider.Current().UserName)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前数据只有创建人才能删除！" }.ToString());
            }
            if (entity.AUDITSTATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前数据已审批，请取消审批后删除数据！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("BD_ZJGCRESULT", "ID", KeyValue);
                database.Delete("BD_ZJGCRESULTDETAILS", "MAINID", KeyValue);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update QC_ZJGCCY set TYPE=1,DEF10='{0}' where BILLNO='{1}'", "", entity.CYID));
                database.ExecuteBySql(strSql, isOpenTrans);
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
        /// 化学检测结果 审批操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CHECheck(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = vorderbll.PCRawCheck(KeyValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 取消审批 化学检测结果 审批操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CHEUNCheck(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = vorderbll.PCRawUNCheck(KeyValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中批次" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }


        public ActionResult CheckReport(string Parm_Key_Value, string Type)
        {
            DataSet dataSet = vorderbll.CheckReport(Parm_Key_Value, Type);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, false);
            }
            return null;
        }

        #endregion
    }
}
