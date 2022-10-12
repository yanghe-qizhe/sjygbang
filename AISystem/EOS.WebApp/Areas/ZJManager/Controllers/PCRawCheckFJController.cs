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

namespace EOS.WebApp.Areas.ZJManager.Controllers
{
    public class AV_ZJRESULTFJ
    {
        public string ZJITEM { get; set; }
        public string ZJITEMNAME { get; set; }
        public string ZJVALUE { get; set; }
    }
    public class PCRawCheckFJController : PublicController<BD_ZJRESULTFJ>
    {
        string ModuleId = "";
        BD_PCRAWCYBll pcbll = new BD_PCRAWCYBll();
        VBD_PCRAWCYBll vpcbll = new VBD_PCRAWCYBll();
        VBD_ZJRESULTFJBll vorderbll = new VBD_ZJRESULTFJBll();
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
        public ActionResult FJCheckReportIndex()
        {
            ViewBag.CreateUser = ManageProvider.Provider.Current().UserName;
            return View();
        }
        //化验录入 
        [LoginAuthorize]
        public ActionResult PCRawCheckFJIndex()
        {
            return View();
        }

        //化验审批
        [LoginAuthorize]
        public ActionResult PCRawCheckIndex1()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult PCRawCheckFJForm()
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
        public ActionResult SELECZJRESULT()
        {
            return View();
        }
        public ActionResult SELECTCHECKUSER()
        {
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
        public ActionResult GetOrderList(string BILLNO, string PRINTCODE, string CHESTATUS, string ZJRESULT, string AUDITSTATUS, string MATERIAL, string MATERIALNAME, string MATERIALSPEC, string PK_ORG, string PCRAWTYPE, string CARS, string PK_STORE, string CQDEF1, string CREUSER, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJRESULTFJ> ListData = vorderbll.GetOrderList(BILLNO, PRINTCODE, CHESTATUS, ZJRESULT, AUDITSTATUS, MATERIAL, MATERIALNAME, MATERIALSPEC, PK_ORG, PCRAWTYPE, CARS, PK_STORE, CQDEF1, CREUSER, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetAVGOrderList(string PRINTCODE, string PCRAWTYPE, string DATETYPE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJRESULTFJ> ListData = vorderbll.GetAVGOrderList(PRINTCODE, PCRAWTYPE, DATETYPE, StartTime, EndTime, jqgridparam);
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
            VBD_ZJRESULTFJ entity = vorderbll.GetEntity(KeyValue);
            return Content(entity.ToJson());
        }



        public ActionResult GetEntityHY(string KeyValue)
        {
            KeyValue = KeyValue.Trim();
            VBD_PCRAWCYBll vpcbll = new VBD_PCRAWCYBll();
            VBD_PCRAWCY entity = vpcbll.GetEntityHY(KeyValue);
            return Content(entity.ToJson());
        }

        //水分仪
        public ActionResult GetEntitySFY(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            //BD_ZJ_MOISTURE entity = database.FindEntity<BD_ZJ_MOISTURE>(KeyValue);
            BD_ZJ_MOISTURE entity = database.FindEntityBySql<BD_ZJ_MOISTURE>(string.Format("select * from (select * from BD_ZJ_MOISTURE where SAMPLEID like '%{0}%' order by CREATEDATE desc) ps where rownum<=1", KeyValue));
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
                List<VBD_ZJRESULTDETAILSFJ> ListData = vorderbll.GetOrderEntryList(KeyValue);
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
        public ActionResult SubmitOrderForm(BD_ZJRESULTFJ entity, string OrderEntryJson, string KeyValue, string ModuleId)
        {
            string sql = "";
            int res = 0;
            #region 验证

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {  //明细行数据
                List<BD_ZJRESULTDETAILSFJ> OrderEntryList = OrderEntryJson.JonsToList<BD_ZJRESULTDETAILSFJ>();

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
                    sql = String.Format("select count(1) from BD_ZJRESULTFJ where AUDITSTATUS=1 and PRINTCODE='{0}' and ID!='{1}'", entity.PRINTCODE, KeyValue);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样单号:{0}已存在确认的复检数据，不可以编辑操作！", entity.PRINTCODE) }.ToString());
                    }

                    database.Delete<BD_ZJRESULTDETAILSFJ>("MAINID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {

                    sql = String.Format("select count(1) from BD_PCRAWCY where PRINTCODE='{0}' ", entity.PRINTCODE);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res == 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样单号:{0}不存在！", entity.PRINTCODE) }.ToString());
                    }
                    if (entity.PCRAWTYPE == "5")
                    {
                        sql = String.Format("select count(1) s from VBD_ZJRESULT_KZ WHERE  AUDITSTATUS=1 and PRINTCODE='{0}' ", entity.PRINTCODE);
                        res = DataFactory.Database().FindCountBySql(sql);
                        if (res > 0)
                        {
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("复检木片制样单:{0}已扣杂提交不可以确认操作！", entity.PRINTCODE) }.ToString());
                        }
                    }

                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from BD_ZJRESULTFJ where  BILLNO='{0}'", entity.BILLNO);
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
                foreach (BD_ZJRESULTDETAILSFJ orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.ZJITEM))
                    {
                        orderentry.Create();
                        orderentry.MAINID = entity.ID;
                        database.Insert(orderentry, isOpenTrans);
                        if (!string.IsNullOrEmpty(orderentry.ZJITEMNAME) && orderentry.FJTYPE == entity.CYTYPE)
                        {
                            ZJRESULTMEMO += string.Format("{0}{1}:{2}({3});", orderentry.ZJITEMNAME, orderentry.DEF1, orderentry.ZJVALUE, (orderentry.ZJBZRESULT == "0" ? "合格" : "不合格"));
                        }
                        index++;
                    }
                }
                //更新为采样化验
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                //更新检验结果
                strSql.Clear();
                ZJRESULTMEMO = ZJRESULTMEMO.TrimEnd(';');
                strSql.Append(string.Format("update BD_ZJRESULTFJ set ZJRESULTMEMO='{0}' where ID='{1}'", ZJRESULTMEMO, entity.ID));
                database.ExecuteBySql(strSql, isOpenTrans);

                sql = String.Format("select count(1) s from BD_ZJRESULTFJ WHERE  CYTYPE=0 and PRINTCODE='{0}' ", entity.PRINTCODE);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res == 0)
                {
                    AddAYData(entity, OrderEntryList, database, isOpenTrans, ModuleId);
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

        public void AddAYData(BD_ZJRESULTFJ entity, List<BD_ZJRESULTDETAILSFJ> OrderEntryList, IDatabase database, DbTransaction isOpenTrans, string ModuleId)
        {
            entity.Create();
            entity.CYTYPE = 0;
            entity.BILLNO = string.Format("{0}01", entity.BILLNO);
            entity.ID = string.Format("{0}01", entity.ID);
            base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
            string sql = String.Format("select count(1) from BD_ZJRESULTFJ where  BILLNO='{0}'", entity.BILLNO);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                this.ModuleId = ModuleId;
                entity.BILLNO = this.BillCode();
                entity.BILLNO = string.Format("{0}01", entity.BILLNO);
            }
            database.Insert(entity, isOpenTrans);
            //处理明细
            int index = 1;
            string ZJRESULTMEMO = "";
            //bool bol = false;

            foreach (BD_ZJRESULTDETAILSFJ orderentry in OrderEntryList)
            {
                if (!string.IsNullOrEmpty(orderentry.ZJITEM))
                {
                    if (orderentry.FJTYPE == 0)
                    {
                        orderentry.Create();
                        orderentry.MAINID = entity.ID;
                        database.Insert(orderentry, isOpenTrans);
                        if (!string.IsNullOrEmpty(orderentry.ZJITEMNAME) && orderentry.FJTYPE == entity.CYTYPE)
                        {
                            ZJRESULTMEMO += string.Format("{0}{1}:{2}({3});", orderentry.ZJITEMNAME, orderentry.DEF1, orderentry.ZJVALUE, (orderentry.ZJBZRESULT == "0" ? "合格" : "不合格"));
                        }
                    }
                    index++;
                }
            }
            //更新为采样化验
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            //更新检验结果
            strSql.Clear();
            ZJRESULTMEMO = ZJRESULTMEMO.TrimEnd(';');
            strSql.Append(string.Format("update BD_ZJRESULTFJ set ZJRESULTMEMO='{0}' where ID='{1}'", ZJRESULTMEMO, entity.ID));
            database.ExecuteBySql(strSql, isOpenTrans);
        }
        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue, string PCRAWTYPE)
        {
            BD_ZJRESULTFJ entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (!ManageProvider.Provider.Current().IsSystem && entity.CREUSER != ManageProvider.Provider.Current().UserName)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前数据只有创建人才能删除！" }.ToString());
            }
            if (entity.AUDITSTATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前数据已确认，请取消确认后删除数据！" }.ToString());
            }

            string sql = String.Format("select count(1) from BD_ZJRESULTFJ where AUDITSTATUS=1 and PRINTCODE='{0}' and ID!='{1}'", entity.PRINTCODE, KeyValue);
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样单号:{0}已存在确认的复检数据，不可以编辑操作！", entity.PRINTCODE) }.ToString());
            }

            if (PCRAWTYPE == "5")
            {
                sql = String.Format("select count(1) s from VBD_ZJRESULT_KZ WHERE  AUDITSTATUS=1 and PRINTCODE='{0}' ", entity.PRINTCODE);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("复检木片制样单:{0}已扣杂提交不可以确认操作！", entity.PRINTCODE) }.ToString());
                }
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("BD_ZJRESULTFJ", "ID", KeyValue);
                database.Delete("BD_ZJRESULTDETAILSFJ", "MAINID", KeyValue);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        //化验项判定
        [LoginAuthorize]
        public ActionResult PCRawCHECheckZJBZ(string ZJITEMNO, string ZJFA, string ZJValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(ZJITEMNO))
                {
                    message = vpcbll.CheckZJBZ(ZJITEMNO, ZJFA, ZJValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "判断失败,质检项目编号为空" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "判断失败," + ex.Message.ToString() }.ToString());
            }
        }

        //加权计算
        public ActionResult CheckData(string OrderEntryJson, string BILLNO, string PRINTCODE)
        {
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                List<BD_ZJRESULTDETAILSFJ> OrderEntryList = OrderEntryJson.JonsToList<BD_ZJRESULTDETAILSFJ>();
                List<VBD_ZJRESULTDETAILSFJ> modellist = vorderbll.GetOrderEntryList2(BILLNO, PRINTCODE);
                BD_ZJRESULTDETAILSFJ model = null;
                foreach (VBD_ZJRESULTDETAILSFJ vmodel in modellist)
                {
                    model = new BD_ZJRESULTDETAILSFJ();
                    model.FJTYPE = vmodel.FJTYPE;
                    model.ZJITEMNAME = vmodel.ITEMNAME;
                    model.ZJITEM = vmodel.ZJITEM;
                    model.ZJVALUE = vmodel.ZJVALUE;
                    model.ZJBZRESULT = vmodel.ZJBZRESULT;
                    model.ZJBZRESULTNAME = vmodel.ZJBZRESULTNAME;
                    model.DEF1 = vmodel.DEF1;
                    model.DEF10 = vmodel.DEF10;
                    OrderEntryList.Add(model);
                }

                OrderEntryList.Select(s => s.FJTYPE == 0 || s.FJTYPE == 1);
                var SumKJData = OrderEntryList.Where(z => z.FJTYPE == 0 || z.FJTYPE == 1).GroupBy(x => new { x.ZJITEM, x.ZJITEMNAME }).Select(y => new
                {
                    ZJITEM = y.Key.ZJITEM,
                    ZJITEMNAME = y.Key.ZJITEMNAME,
                    ZJVALUE = y.Average(x => Convert.ToDecimal(x.ZJVALUE)).ToString("F1")
                });
                AV_ZJRESULT avgmodel = null;
                List<AV_ZJRESULT> avglist = new List<AV_ZJRESULT>();
                foreach (var item in SumKJData)
                {
                    if (!string.IsNullOrEmpty(item.ZJITEM))
                    {
                        avgmodel = new AV_ZJRESULT();
                        avgmodel.ZJITEM = item.ZJITEM;
                        avgmodel.ZJITEMNAME = item.ZJITEMNAME;
                        avgmodel.ZJVALUE = string.Format("{0}", item.ZJVALUE);
                        avglist.Add(avgmodel);
                    }
                }
                var JsonData = new
                {
                    rows = avglist
                };
                return Content(JsonData.ToJson());

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //查询加权平均计量单
        public DataTable GetAVGZJRESULTList(string KeyValue)
        {
            string strSql = "";
            strSql = string.Format(@"select ZJITEM,ZJITEMNAME,round(AVG(is_number(ZJVALUE)),1) ZJVALUE  from BD_ZJRESULTDETAILSFJ where mainid 
in('{0}') group by ZJITEM,ZJITEMNAME", KeyValue.Replace(",", "','"));
            return DataFactory.Database().FindTableBySql(strSql);
        }


        /// <summary>
        /// 化学检测结果 完成操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CHEFinish(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = vorderbll.PCRawFinish(KeyValue);
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
        /// 取消完成 化学检测结果 完成操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CHEUNFinish(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = vorderbll.PCRawUNFinish(KeyValue);
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
        /// 化学检测结果 审批操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CHECheck(string KeyValue, string PRINTCODE, string PCRAWTYPE)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = vorderbll.PCRawCheck(KeyValue, PRINTCODE, PCRAWTYPE);
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
        public ActionResult CHEUNCheck(string KeyValue, string PRINTCODE, string PCRAWTYPE)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = vorderbll.PCRawUNCheck(KeyValue, PRINTCODE, PCRAWTYPE);
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
