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
using EOS.WebApp.Areas.ZJManager.Models;
using EOS.WebApp.SxtjModels;

namespace EOS.WebApp.Areas.ZJManager.Controllers
{
    public class AV_ZJRESULT
    {
        public string ZJITEM { get; set; }
        public string ZJITEMNAME { get; set; }
        public string ZJVALUE { get; set; }
    }
    public class PCRawCheckController : PublicController<BD_ZJRESULT>
    {
        string ModuleId = "";
        BD_PCRAWCYBll pcbll = new BD_PCRAWCYBll();
        VBD_PCRAWCYBll vpcbll = new VBD_PCRAWCYBll();
        VBD_ZJRESULTBll vorderbll = new VBD_ZJRESULTBll();
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
        public ActionResult CheckReportIndex1()
        {
            ViewBag.CreateUser = ManageProvider.Provider.Current().UserName;
            return View();
        }

        //化验录入 
        [LoginAuthorize]
        public ActionResult PCRawCheckIndex()
        {
            ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            return View();
        }

        //包材化验
        [LoginAuthorize]
        public ActionResult PCRawCheckIndexBC()
        {
            ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            return View();
        }
        //木片化验
        [LoginAuthorize]
        public ActionResult PCRawCheckIndexMP()
        {
            ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            return View();
        }
        //废纸化验
        [LoginAuthorize]
        public ActionResult PCRawCheckIndexFZ()
        {
            ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            return View();
        }
        //化验审批
        [LoginAuthorize]
        public ActionResult PCRawCheckIndex1()
        {
            ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            return View();
        }

        //原料采购部扣重
        [LoginAuthorize]
        public ActionResult PCRawCheckIndex2()
        {
            return View();
        }
        //化工采购部扣重
        [LoginAuthorize]
        public ActionResult PCRawCheckIndexHG2()
        {
            return View();
        }
        //原料木片扣杂
        [LoginAuthorize]
        public ActionResult PCRawCheckIndex3()
        {
            return View();
        }
        //包材化验审批
        [LoginAuthorize]
        public ActionResult PCRawCheckIndex4()
        {
            return View();
        }
        //木片化验审批
        [LoginAuthorize]
        public ActionResult PCRawCheckIndex5()
        {
            return View();
        }

        //成纸指标补录
        [LoginAuthorize]
        public ActionResult PCRawCheckIndexBL()
        {
            ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            return View();
        }
        public ActionResult SELECTCHECKUSER()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCheckFormCZZBBL()//成纸指标补录编辑页
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
        public ActionResult PCRawCheckForm()
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
        public ActionResult zjResultLr()
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
        public ActionResult PCRawCheckFormFZ()
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
        public ActionResult PCRawCheckForm1()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult PCRawCheckForm2()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCheckFormHG2()
        {
            return View();
        }
        // 原料木片扣重
        [LoginAuthorize]
        public ActionResult PCRawCheckForm3()
        {
            return View();
        }
        // 原料木片扣重审批
        [LoginAuthorize]
        public ActionResult PCRawCheckForm4()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult SELECZJRESULT()
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
        public ActionResult GetOrderList(string BILLNO, string PRINTCODE, string CHESTATUS, string ISKZ, string ZJRESULT, string AUDITSTATUS, string MATERIAL, string MATERIALNAME, string MATERIALSPEC, string PK_ORG, string PCRAWTYPE, string CARS, string PK_STORE, string CQDEF1, string CREUSER,  string DEF6, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJRESULT> ListData = vorderbll.GetOrderList(BILLNO, PRINTCODE, CHESTATUS, ISKZ, ZJRESULT, AUDITSTATUS, MATERIAL, MATERIALNAME, MATERIALSPEC, PK_ORG, PCRAWTYPE, CARS, PK_STORE, CQDEF1, CREUSER, DEF6,StartTime, EndTime, jqgridparam);
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
        public ActionResult GetAVGOrderList(string PRINTCODE, string PCRAWTYPE, string DATETYPE, string CQDEF1, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJRESULT> ListData = vorderbll.GetAVGOrderList(PRINTCODE, PCRAWTYPE, DATETYPE, CQDEF1, StartTime, EndTime, jqgridparam);
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

        //原料采购部扣重
        [LoginAuthorize]
        public ActionResult GetOrderList1(string PRINTCODE, string PK_ORG, string SUPPLY, string CARS, string CARSNAME, string AUDITSTATUS, string MATERIAL, string MATERIALNAME, string MATERIALSPEC, string PCRAWTYPE, string CQDEF1, string CYUSER, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJRESULT_KZ> ListData = vorderbll.GetOrderListKZ(PRINTCODE, PK_ORG, SUPPLY, MATERIAL, MATERIALNAME, MATERIALSPEC, CARS, CARSNAME, AUDITSTATUS, PCRAWTYPE, "", "1", "", CQDEF1, CYUSER, StartTime, EndTime, jqgridparam);
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
        //木片全部扣重
        [LoginAuthorize]
        public ActionResult GetOrderList2(string PRINTCODE, string PK_ORG, string SUPPLY, string CARS, string CARSNAME, string AUDITSTATUS, string MATERIAL, string MATERIALNAME, string MATERIALSPEC, string PCRAWTYPE, string DATETYPE, string ISKZ, string ISKZ1, string CQDEF1, string CYUSER, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJRESULT_KZ> ListData = vorderbll.GetOrderListKZ(PRINTCODE, PK_ORG, SUPPLY, MATERIAL, MATERIALNAME, MATERIALSPEC, CARS, CARSNAME, AUDITSTATUS, PCRAWTYPE, DATETYPE, ISKZ, ISKZ1, CQDEF1, CYUSER, StartTime, EndTime, jqgridparam);
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
            VBD_ZJRESULT entity = vorderbll.GetEntity(KeyValue);
            return Content(entity.ToJson());
        }

        //通过制样码获取化验主表信息
        public ActionResult SetFormControl1(string KeyValue)
        {
            VBD_ZJRESULT entity = vorderbll.GetPrintCodeToEntity(KeyValue);
            return Content(entity.ToJson());
        }


        public ActionResult GetEntityHY(string KeyValue)
        {
            KeyValue = KeyValue.Trim();
            //验证
            VBD_ZJRESULT entity_zj = DataFactory.Database().FindEntityByWhere<VBD_ZJRESULT>(string.Format(" AND PRINTCODE='{0}'", KeyValue));
            if (entity_zj.CHESTATUS == "1")
            {
                var JsonData = new
                {
                    zjtype = 3,
                    message = "该批次生成化验单已完成！",
                };
                return Content(JsonData.ToJson());
            }
            else
            {
                if (!string.IsNullOrEmpty(entity_zj.BILLNO))
                {
                    var JsonData = new
                    {
                        zjtype = 1,
                        rowdata = entity_zj
                    };
                    return Content(JsonData.ToJson());
                }
                else
                {
                    VBD_PCRAWCYBll vpcbll = new VBD_PCRAWCYBll();
                    VBD_PCRAWCY entity = vpcbll.GetEntityHY(KeyValue);
                    string CARS = "", CARSNAME = "";
                    List<VBD_PCRAWCYDETAILS> List = vpcbll.GetOrderEntryList(entity.ID);
                    if (List.Count > 0)
                    {
                        VBD_PCRAWCYDETAILS details = List[0];
                        CARS = details.CARS;
                        CARSNAME = details.CARSNAME;
                    }
                    var JsonData = new
                    {
                        zjtype = 0,
                        CARS = CARS,
                        CARSNAME = CARSNAME,
                        rowdata = entity
                    };
                    return Content(JsonData.ToJson());
                }
            }
        }


        public ActionResult GetICCardEntity(string KeyValue)
        {
            KeyValue = KeyValue.Trim();
            VBD_PCRAWCYBll vpcbll = new VBD_PCRAWCYBll();
            VBD_PCRAWCY entity = vpcbll.GetICCardToEntity1(KeyValue);
            if (!string.IsNullOrEmpty(entity.ID))
            {
                //验证
                VBD_ZJRESULT entity_zj = DataFactory.Database().FindEntityByWhere<VBD_ZJRESULT>(string.Format(" AND PRINTCODE='{0}'", entity.PRINTCODE));
                if (entity_zj.CHESTATUS == "1")
                {
                    var JsonData = new
                    {
                        zjtype = 3,
                        message = $"IC卡序号{KeyValue}的批次{entity.PRINTCODE}生成化验单已完成！"
                    };
                    return Content(JsonData.ToJson());
                }
                else
                {
                    if (!string.IsNullOrEmpty(entity_zj.BILLNO))
                    {
                        var JsonData = new
                        {
                            zjtype = 1,
                            rowdata = entity_zj
                        };
                        return Content(JsonData.ToJson());
                    }
                    else
                    {
                        string CARS = "", CARSNAME = "";
                        List<VBD_PCRAWCYDETAILS> List = vpcbll.GetOrderEntryList(entity.ID);
                        if (List.Count > 0)
                        {
                            VBD_PCRAWCYDETAILS details = List[0];
                            CARS = details.CARS;
                            CARSNAME = details.CARSNAME;
                        }
                        var JsonData = new
                        {
                            zjtype = 0,
                            CARS = CARS,
                            CARSNAME = CARSNAME,
                            rowdata = entity
                        };
                        return Content(JsonData.ToJson());
                    }
                }
            }
            else
            {
                var JsonData = new
                {
                    zjtype = 3,
                    message = $"IC卡序号{KeyValue}获取数据失败！",
                };
                return Content(JsonData.ToJson());
            }

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
                List<VBD_ZJRESULTDETAILS> ListData = vorderbll.GetOrderEntryList(KeyValue);
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

        /// <summary>
        /// 获取质检结果明细表信息2
        /// </summary>
        /// <param name="MAINID"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderEntryList2(string KeyValue)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<BD_ZJRESULTLR> ListData = new BD_ZJRESULTLRBLL().GetOrderList(KeyValue);
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

        public ActionResult GetOrderEntryListFZ(string KeyValue)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJRESULTDETAILS> ListData = vorderbll.GetOrderEntryListFZ(KeyValue);
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
        public ActionResult GetOrderEntryListBZ(string KeyValue)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJRESULTDETAILS_BZ> ListData = vorderbll.GetOrderEntryListBZ(KeyValue);
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


        [LoginAuthorize]
        public ActionResult GetOrderEntryListHY1(string Supply, string Material, string MaterialSpec)
        {
            try
            {
                Supply = Supply.Trim();
                Material = Material.Trim();
                List<VBase_ZJFAConfig_B> list = vorderbll.GetOrderEntryListHY(Supply, Material, MaterialSpec);
                if (list.Count == 0)
                {
                    list = vorderbll.GetOrderEntryListHY("", Material, MaterialSpec);
                    if (list.Count == 0)
                    {
                        list = vorderbll.GetOrderEntryListHY(Supply, Material);
                        if (list.Count == 0)
                        {
                            list = vorderbll.GetOrderEntryListHY("", Material);
                        }
                    }
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
        //成纸指标补录保存
        [HttpPost]
        public ActionResult SubmitOrderFormBL(string dl, string hd, string np, string nz, string kzqd, string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                IDatabase database = DataFactory.Database();
                DbTransaction isOpenTrans = database.BeginTrans();
                string sql = string.Format("SELECT count(1) NUMS FROM BD_ZJRESULT WHERE BILLNO = '{0}'", id);
                int count = DataFactory.Database().FindCountBySql(sql);
                if (count == 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("查无此化验码{0}！", id) }.ToString());
                }
                else
                {
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    strSql.Clear();
                    strSql.Append(string.Format("update BD_ZJRESULT set DEF1='{0}',DEF2='{1}',DEF3='{2}',DEF4='{3}',DEF5='{4}' where BILLNO = '{5}'", dl, hd, np, nz, kzqd, id));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    database.Commit();
                    return Content(new JsonMessage { Success = true, Code = "1", Message = "保存成功！" }.ToString());
                }
            }
            else
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "化验单号不能为空！" }.ToString());
            }
        }

        //化验保存
        [HttpPost]
        public ActionResult SubmitOrderForm(BD_ZJRESULT entity, string OrderEntryJson, string PCRAWTYPE, string KeyValue, string ModuleId)
        {
            #region 验证
            if (PCRAWTYPE == "5")
            {
                if (!string.IsNullOrEmpty(entity.PRINTCODE))
                {
                    // select COUNT(1) num from VBD_ZJRESULT_KZ where AUDITSTATUS=1 AND printcode='ZY02200917150004379280'
                    string sql = String.Format("SELECT count(1) NUMS FROM VBD_ZJRESULT_KZ WHERE  AUDITSTATUS=1 and PRINTCODE='{0}'", entity.PRINTCODE);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("制样码:{0}的木片扣杂已提交！", entity.PRINTCODE) }.ToString());
                    }
                }
            }

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {  //明细行数据
                List<BD_ZJRESULTDETAILS> OrderEntryList = OrderEntryJson.JonsToList<BD_ZJRESULTDETAILS>();

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
                    database.Delete<BD_ZJRESULTDETAILS>("MAINID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {

                    string sql = String.Format("select count(1) from BD_ZJRESULT where PRINTCODE='{0}' ", entity.PRINTCODE);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样单号:{0}已生成化验单！", entity.PRINTCODE) }.ToString());
                    }
                    sql = String.Format("select count(1) from BD_PCRAWCY where PRINTCODE='{0}' ", entity.PRINTCODE);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res == 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样单号:{0}不存在！", entity.PRINTCODE) }.ToString());
                    }

                    if (!string.IsNullOrEmpty(entity.CYID))
                    {
                        BD_PCRAWCY cyentity = database.FindEntity<BD_PCRAWCY>(entity.CYID);
                        if (cyentity != null)
                        {
                            if (cyentity.PRINTCODE != entity.PRINTCODE)
                            {
                                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样PK与制样码{0}不匹配！", entity.PRINTCODE) }.ToString());
                            }
                        }
                    }

                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from BD_ZJRESULT where  BILLNO='{0}'", entity.BILLNO);
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
                foreach (BD_ZJRESULTDETAILS orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.ZJITEM))
                    {
                        orderentry.Create();
                        orderentry.MAINID = entity.ID;
                        //if (orderentry.DEF10 == "1")
                        //{
                        //    bol = IsNumeric(orderentry.ZJVALUE);
                        //    if (bol)
                        //    {
                        //        orderentry.ZJVALUE = string.Format("{0:N2}", Convert.ToDecimal(orderentry.ZJVALUE));
                        //    }
                        //}
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
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=5,DEF10='{0}' where ID='{1}'", entity.BILLNO, entity.CYID));
                database.ExecuteBySql(strSql, isOpenTrans);
                //更新检验结果
                strSql.Clear();
                //if (entity.KS > 0)
                //{
                //    ZJRESULTMEMO += string.Format("{0}{1}:{2};", "杂质2", "", entity.KS);
                //}
                ZJRESULTMEMO = ZJRESULTMEMO.TrimEnd(';');
                strSql.Append(string.Format("update BD_ZJRESULT set ZJRESULTMEMO='{0}' where ID='{1}'", ZJRESULTMEMO, entity.ID));
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

        //质检结果录入保存
        [HttpPost]
        public ActionResult ZjResultLrSave(BD_ZJRESULT entity, string OrderEntryJson, string PCRAWTYPE, string KeyValue, string ModuleId)
        {
            #region 验证
            if (PCRAWTYPE == "5")
            {
                if (!string.IsNullOrEmpty(entity.PRINTCODE))
                {
                    // select COUNT(1) num from VBD_ZJRESULT_KZ where AUDITSTATUS=1 AND printcode='ZY02200917150004379280'
                    string sql = String.Format("SELECT count(1) NUMS FROM VBD_ZJRESULT_KZ WHERE  AUDITSTATUS=1 and PRINTCODE='{0}'", entity.PRINTCODE);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("制样码:{0}的木片扣杂已提交！", entity.PRINTCODE) }.ToString());
                    }
                }
            }

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            try
            {   //原逻辑
                //明细行数据
                List<BD_ZJRESULTDETAILS> OrderEntryList2 = OrderEntryJson.JonsToList<BD_ZJRESULTDETAILS>();

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
                    database.Delete<BD_ZJRESULTDETAILS>("MAINID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {

                    string sql = String.Format("select count(1) from BD_ZJRESULT where PRINTCODE='{0}' ", entity.PRINTCODE);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样单号:{0}已生成化验单！", entity.PRINTCODE) }.ToString());
                    }
                    sql = String.Format("select count(1) from BD_PCRAWCY where PRINTCODE='{0}' ", entity.PRINTCODE);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res == 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样单号:{0}不存在！", entity.PRINTCODE) }.ToString());
                    }

                    if (!string.IsNullOrEmpty(entity.CYID))
                    {
                        BD_PCRAWCY cyentity = database.FindEntity<BD_PCRAWCY>(entity.CYID);
                        if (cyentity != null)
                        {
                            if (cyentity.PRINTCODE != entity.PRINTCODE)
                            {
                                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("当前制样PK与制样码{0}不匹配！", entity.PRINTCODE) }.ToString());
                            }
                        }
                    }

                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from BD_ZJRESULT where  BILLNO='{0}'", entity.BILLNO);
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
                foreach (BD_ZJRESULTDETAILS orderentry in OrderEntryList2)
                {
                    if (!string.IsNullOrEmpty(orderentry.ZJITEM))
                    {
                        orderentry.Create();
                        orderentry.MAINID = entity.ID;
                        //if (orderentry.DEF10 == "1")
                        //{
                        //    bol = IsNumeric(orderentry.ZJVALUE);
                        //    if (bol)
                        //    {
                        //        orderentry.ZJVALUE = string.Format("{0:N2}", Convert.ToDecimal(orderentry.ZJVALUE));
                        //    }
                        //}
                        database.Insert(orderentry, isOpenTrans);
                        if (!string.IsNullOrEmpty(orderentry.ZJITEMNAME))
                        {
                            ZJRESULTMEMO += string.Format("{0}{1}:{2}({3});", orderentry.ZJITEMNAME, orderentry.DEF1, orderentry.ZJVALUE, (orderentry.ZJBZRESULT == "0" ? "合格" : "不合格"));
                        }
                        index++;
                    }
                }

                //新增逻辑
                //明细行数据
                if (string.IsNullOrEmpty(KeyValue))
                {
                    KeyValue = entity.ID;
                }

                List<BD_ZJRESULTLR> OrderEntryList = OrderEntryJson.JonsToList<BD_ZJRESULTLR>();
                ZJRESULTMEMO = "";
                foreach (BD_ZJRESULTLR orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.ZJITEM))
                    {
                        int n = 0;
                        List<decimal> usValueList = new List<decimal>();
                        decimal[] values = new decimal[25] { orderentry.ZJVALUE1, orderentry.ZJVALUE2, orderentry.ZJVALUE3, orderentry.ZJVALUE4, orderentry.ZJVALUE5, orderentry.ZJVALUE6, orderentry.ZJVALUE7, orderentry.ZJVALUE8, orderentry.ZJVALUE9, orderentry.ZJVALUE10, orderentry.ZJVALUE11, orderentry.ZJVALUE12, orderentry.ZJVALUE13, orderentry.ZJVALUE14, orderentry.ZJVALUE15, orderentry.ZJVALUE16, orderentry.ZJVALUE17, orderentry.ZJVALUE18, orderentry.ZJVALUE19, orderentry.ZJVALUE20, orderentry.ZJVALUE21, orderentry.ZJVALUE22, orderentry.ZJVALUE23, orderentry.ZJVALUE24, orderentry.ZJVALUE25 };
                        
                        for (int i = 0; i < values.Length; i++)
                        {
                            if (values[i] != 0)
                            {
                                //usValues[n] = values[i];
                                usValueList.Add(values[i]);
                                n++;
                            }
                        }
                        decimal[] usValues = usValueList.ToArray();

                        string sql = String.Format("SELECT length(FORMAT) FROM VBase_ZJItem WHERE ITEMNO='{0}'", orderentry.ZJITEM);
                        int format = DataFactory.Database().FindCountBySql(sql);
                        int digit;
                        if (format == 1)
                        {
                            digit = 0;
                        }
                        else
                        {
                            digit = format - 2;
                        }

                        //typelr 类型:0取平均值；1取最大值；2取最小值；3取最大两个值平均值；4取最小两个值平均值；5取最大三个值平均值；6取最小三个值平均值-----------(值为0是不参与计算)
                        decimal zjValue = 0;
                        if (usValues.Length != 0)
                        {
                            if (orderentry.TYPELR == "0" && !string.IsNullOrEmpty(orderentry.TYPELR))
                            {
                                decimal sum = 0;
                                for (int i = 0; i < usValues.Length; i++)
                                {
                                    sum += usValues[i];
                                }
                                zjValue = Math.Round(sum / usValues.Length, digit);
                            }
                            else if (orderentry.TYPELR == "1" && !string.IsNullOrEmpty(orderentry.TYPELR))
                            {
                                Array.Reverse(usValues);
                                Array.Sort(usValues);
                                Array.Reverse(usValues);
                                zjValue = Math.Round(usValues[0], digit);
                            }
                            else if (orderentry.TYPELR == "2" && !string.IsNullOrEmpty(orderentry.TYPELR))
                            {
                                Array.Sort(usValues);
                                zjValue = Math.Round(usValues[0], digit);
                            }
                            else if (orderentry.TYPELR == "3" && !string.IsNullOrEmpty(orderentry.TYPELR))
                            {
                                Array.Reverse(usValues);
                                Array.Sort(usValues);
                                Array.Reverse(usValues);
                                zjValue = Math.Round((usValues[0] + usValues[1]) / 2, digit);
                            }
                            else if (orderentry.TYPELR == "4" && !string.IsNullOrEmpty(orderentry.TYPELR))
                            {
                                Array.Sort(usValues);
                                zjValue = Math.Round((usValues[0] + usValues[1]) / 2, digit);
                            }
                            else if (orderentry.TYPELR == "5" && !string.IsNullOrEmpty(orderentry.TYPELR))
                            {
                                Array.Reverse(usValues);
                                Array.Sort(usValues);
                                Array.Reverse(usValues);
                                zjValue = Math.Round((usValues[0] + usValues[1] + usValues[2]) / 3, digit);
                            }
                            else if (orderentry.TYPELR == "6" && !string.IsNullOrEmpty(orderentry.TYPELR))
                            {
                                Array.Sort(usValues);
                                zjValue = Math.Round((usValues[0] + usValues[1] + usValues[2]) / 3, digit);
                            }

                            string ZJBZRESULT = PCRawCHECheckZJBZ2(orderentry.ZJITEM, entity.ZJFA, zjValue.ToString());
                            if (ZJBZRESULT != "X")
                            {
                                orderentry.ZJBZRESULT = ZJBZRESULT;
                            }
                        }

                        string sqlCount = String.Format("SELECT count(1) NUMS FROM BD_ZJRESULTLR WHERE BILLNO='{0}' and ZJFA='{1}' and ZJITEM='{2}'", entity.BILLNO, entity.ZJFA, orderentry.ZJITEM);
                        int res = DataFactory.Database().FindCountBySql(sqlCount);
                        if (res > 0)
                        {
                            strSql.Clear();
                            strSql.Append(string.Format("update BD_ZJRESULTLR set TYPE='{0}',ZJVALUE1='{1}',ZJVALUE2='{2}',ZJVALUE3='{3}',ZJVALUE4='{4}',ZJVALUE5='{5}',ZJVALUE6='{6}'," +
                                "ZJVALUE7='{7}',ZJVALUE8='{8}',ZJVALUE9='{9}',ZJVALUE10='{10}',ZJVALUE11='{11}',ZJVALUE12='{12}',ZJVALUE13='{13}',ZJVALUE14='{14}',ZJVALUE15='{15}',ZJVALUE16='{16}'," +
                                "ZJVALUE17='{17}',ZJVALUE18='{18}',ZJVALUE19='{19}',ZJVALUE20='{20}',ZJVALUE21='{21}',ZJVALUE22='{22}',ZJVALUE23='{23}',ZJVALUE24='{24}',ZJVALUE25='{25}' " +
                                "where BILLNO='{26}' and ZJFA='{27}' and ZJITEM='{28}'",
                                orderentry.TYPELR, orderentry.ZJVALUE1, orderentry.ZJVALUE2, orderentry.ZJVALUE3, orderentry.ZJVALUE4, orderentry.ZJVALUE5, orderentry.ZJVALUE6,
                                orderentry.ZJVALUE7, orderentry.ZJVALUE8, orderentry.ZJVALUE9, orderentry.ZJVALUE10, orderentry.ZJVALUE11, orderentry.ZJVALUE12,
                                orderentry.ZJVALUE13, orderentry.ZJVALUE14, orderentry.ZJVALUE15, orderentry.ZJVALUE16, orderentry.ZJVALUE17, orderentry.ZJVALUE18,
                                orderentry.ZJVALUE19, orderentry.ZJVALUE20, orderentry.ZJVALUE21, orderentry.ZJVALUE22, orderentry.ZJVALUE23, orderentry.ZJVALUE24,
                                orderentry.ZJVALUE25, entity.BILLNO, entity.ZJFA, orderentry.ZJITEM));
                            database.ExecuteBySql(strSql, isOpenTrans);
                        }
                        else
                        {
                            strSql.Clear();
                            strSql.Append(string.Format("insert into BD_ZJRESULTLR (TYPE,BILLNO,ZJFA,ZJITEM,ZJITEMNAME,ZJVALUE1,ZJVALUE2,ZJVALUE3,ZJVALUE4,ZJVALUE5,ZJVALUE6," +
                                "ZJVALUE7,ZJVALUE8,ZJVALUE9,ZJVALUE10,ZJVALUE11,ZJVALUE12,ZJVALUE13,ZJVALUE14,ZJVALUE15,ZJVALUE16,ZJVALUE17,ZJVALUE18,ZJVALUE19,ZJVALUE20," +
                                "ZJVALUE21,ZJVALUE22,ZJVALUE23,ZJVALUE24,ZJVALUE25,DETAILSID) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}'," +
                                "'{11}','{12}','{13}','{14}','{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}','{24}','{25}','{26}','{27}','{28}','{29}','{30}')",
                                orderentry.TYPELR, entity.BILLNO, entity.ZJFA, orderentry.ZJITEM, orderentry.ZJITEMNAME, orderentry.ZJVALUE1, orderentry.ZJVALUE2, orderentry.ZJVALUE3,
                                orderentry.ZJVALUE4, orderentry.ZJVALUE5, orderentry.ZJVALUE6, orderentry.ZJVALUE7, orderentry.ZJVALUE8, orderentry.ZJVALUE9, orderentry.ZJVALUE10,
                                orderentry.ZJVALUE11, orderentry.ZJVALUE12, orderentry.ZJVALUE13, orderentry.ZJVALUE14, orderentry.ZJVALUE15, orderentry.ZJVALUE16, orderentry.ZJVALUE17,
                                orderentry.ZJVALUE18, orderentry.ZJVALUE19, orderentry.ZJVALUE20, orderentry.ZJVALUE21, orderentry.ZJVALUE22, orderentry.ZJVALUE23, orderentry.ZJVALUE24,
                                orderentry.ZJVALUE25, KeyValue));
                            database.ExecuteBySql(strSql, isOpenTrans);
                        }

                        ZJRESULTMEMO += string.Format("{0}{1}:{2}({3});", orderentry.ZJITEMNAME, orderentry.DEF1, orderentry.ZJVALUE, (orderentry.ZJBZRESULT == "0" ? "合格" : "不合格"));

                        strSql.Clear();
                        strSql.Append(string.Format("update BD_ZJRESULTDETAILS set ZJVALUE='{0}',ZJBZRESULT='{1}' where MAINID='{2}' and ZJFA='{3}' and ZJITEM='{4}'", zjValue, orderentry.ZJBZRESULT, KeyValue, entity.ZJFA, orderentry.ZJITEM));
                        database.ExecuteBySql(strSql, isOpenTrans);
                    }
                }
                //更新为采样化验
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=5,DEF10='{0}' where ID='{1}'", entity.BILLNO, entity.CYID));
                database.ExecuteBySql(strSql, isOpenTrans);

                //更新检验结果
                strSql.Clear();
                ZJRESULTMEMO = ZJRESULTMEMO.TrimEnd(';');
                strSql.Append(string.Format("update BD_ZJRESULT set ZJRESULTMEMO='{0}' where ID='{1}'", ZJRESULTMEMO, entity.ID));
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

        //扣重
        [HttpPost]
        public ActionResult SubmitOrderFormKZ(string OrderEntryJson, string ModuleId)
        {
            #region 验证

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {  //明细行数据
                List<VBD_ZJRESULT_KZ> OrderEntryList = OrderEntryJson.JonsToList<VBD_ZJRESULT_KZ>();
                string Message = "采购处置扣重成功。";
                //处理明细
                int index = 1;
                string KZDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string KZUSER = ManageProvider.Provider.Current().UserName;
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                foreach (VBD_ZJRESULT_KZ entity in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(entity.PCID))
                    {
                        //更新采样的采购处置扣重
                        strSql.Clear();
                        strSql.Append(string.Format("update BD_PCRAWCYDETAILS set WGKZ={0},DEF14='{1}',DEF15='{2}' where ID='{3}'", entity.WGKZ, KZDATE, KZUSER, entity.ID));
                        database.ExecuteBySql(strSql, isOpenTrans);
                        index++;
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

        //木片扣杂
        [HttpPost]
        public ActionResult SubmitOrderFormKZ1(string OrderEntryJson, string ModuleId)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {  //明细行数据
                List<VBD_ZJRESULT_KZ> OrderEntryList = OrderEntryJson.JonsToList<VBD_ZJRESULT_KZ>();

                #region 验证
                if (!ManageProvider.Provider.Current().IsSystem)
                {
                    int ZCUser = ManageProvider.Provider.Current().ZCUser;
                    if (ZCUser == 0)
                    {
                        //cars.Select(c => c.Name).ToArray()
                        string[] arr = OrderEntryList.Where(t => string.IsNullOrEmpty(t.ID) == false).Select(c => c.ID).ToArray();
                        if (arr.Length > 0)
                        {
                            string sql = "";
                            if (arr.Length == 1)
                            {
                                sql = string.Format("select count(1) ss from VBD_ZJRESULT_KZ where  DEF15 is not null and DEF15!='{0}' and id='{1}'", ManageProvider.Provider.Current().UserName, arr[0]); ;
                            }
                            else
                            {
                                string str = string.Join("','", arr);
                                sql = string.Format("select count(1) ss from VBD_ZJRESULT_KZ where  DEF15 is not null and DEF15!='{0}' and id in('{1}')", ManageProvider.Provider.Current().UserName, str);
                            }

                            int res = DataFactory.Database().FindCountBySql(sql);
                            if (res > 0)
                            {
                                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("选中的明细中有他人处置的信息，请您确认后在处置扣杂！") }.ToString());
                            }
                        }
                    }
                }
                #endregion


                string Message = "处置扣杂成功。";
                //处理明细
                int index = 1;
                string KZDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string KZUSER = ManageProvider.Provider.Current().UserName;
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                foreach (VBD_ZJRESULT_KZ entity in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(entity.PCID))
                    {
                        if (!string.IsNullOrEmpty(entity.ID))
                        {
                            //更新采样的采购处置扣杂
                            strSql.Clear();
                            strSql.Append(string.Format("update BD_PCRAWCYDETAILS set  KS={0},DEF14='{1}',DEF15='{2}' where ID='{3}'", entity.KS, KZDATE, KZUSER, entity.ID));
                            database.ExecuteBySql(strSql, isOpenTrans);
                        }
                        index++;
                    }
                }

                //var SumKJData = OrderEntryList.GroupBy(x => new { x.MAINID }).Select(y => new
                //   {
                //       MAINID = y.Key.MAINID,
                //       KS = y.Sum(x => x.KS)
                //   });

                //foreach (var item in SumKJData)
                //{
                //    if (!string.IsNullOrEmpty(item.MAINID))
                //    {
                //        strSql.Clear();
                //        strSql.Append(string.Format("update BD_PCRAWCY set SUMKS=nvl(SUMKS,0)+{0} where ID='{1}'", item.KS, item.MAINID));
                //        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                //        //更新
                //        strSql.Clear();
                //        strSql.Append(string.Format("update BD_ZJRESULT set KS=nvl(KS,0)+{0} where CYID='{1}'", item.KS, item.MAINID));
                //        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                //    }
                //}
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
        [HttpPost]
        public ActionResult SubmitOrderFormSP(string OrderEntryJson, string AUDITSTATUS, string ModuleId, string PCRAWTYPE)
        {
            #region 验证

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {

                //明细行数据
                List<VBD_ZJRESULT_KZ> OrderEntryList = OrderEntryJson.JonsToList<VBD_ZJRESULT_KZ>();
                //if (PCRAWTYPE == "1")
                //{
                //    string[] arrstr = OrderEntryList.Select(y => y.ID).ToArray();
                //    string strids = string.Join("','", arrstr);
                //    string sql = String.Format("SELECT count(1) NUMS FROM BD_PCRAWCYDETAILS WHERE CZTYPE=3 and  WGKZ=0 and ID in('{0}') ", strids);
                //    int res = DataFactory.Database().FindCountBySql(sql);
                //    if (res > 0)
                //    {
                //        return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("化工原料扣重数量为0，请您确认后在审批操作！") }.ToString());
                //    }
                //}
                string Message = "采购处置审批成功。";
                //处理明细
                int index = 1;
                string AUDITDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string AUDITUSER = ManageProvider.Provider.Current().UserName;
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                foreach (VBD_ZJRESULT_KZ entity in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(entity.PCID))
                    {
                        //更新采样的采购处置扣重
                        strSql.Clear();
                        strSql.Append(string.Format("update BD_PCRAWCYDETAILS set AUDITUSER='{0}',AUDITDATE='{1}',AUDITMEMO='{2}',AUDITSTATUS={3} where ID='{4}'", AUDITUSER, AUDITDATE, entity.AUDITMEMO, AUDITSTATUS, entity.ID));
                        database.ExecuteBySql(strSql, isOpenTrans);
                        index++;
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

        //化工提交
        [HttpPost]
        public ActionResult SubmitOrderFormTJ(string OrderEntryJson, string AUDITSTATUS, string ModuleId, string PCRAWTYPE)
        {
            #region 验证

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {

                //明细行数据
                List<VBD_ZJRESULT_KZ> OrderEntryList = OrderEntryJson.JonsToList<VBD_ZJRESULT_KZ>();

                string Message = "采购处置提交成功。";
                //处理明细
                int index = 1;
                string AUDITDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string AUDITUSER = ManageProvider.Provider.Current().UserName;
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                foreach (VBD_ZJRESULT_KZ entity in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(entity.PCID))
                    {
                        //更新采样的采购处置扣重
                        strSql.Clear();
                        strSql.Append(string.Format("update BD_PCRAWCYDETAILS set AUDITUSER1='{0}',AUDITDATE1='{1}',AUDITMEMO1='{2}',AUDITSTATUS1={3} where ID='{4}'", AUDITUSER, AUDITDATE, entity.AUDITMEMO, AUDITSTATUS, entity.ID));
                        database.ExecuteBySql(strSql, isOpenTrans);
                        index++;
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


        //木片审批
        [HttpPost]
        public ActionResult SubmitOrderFormSP1(string OrderEntryJson, string AUDITSTATUS, string ModuleId)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {  //明细行数据
                List<VBD_ZJRESULT_KZ> OrderEntryList = OrderEntryJson.JonsToList<VBD_ZJRESULT_KZ>();

                #region 验证
                if (AUDITSTATUS == "0")
                {
                    string[] arr = OrderEntryList.Where(t => string.IsNullOrEmpty(t.CYID) == false).Select(s => s.CYID).ToArray();
                    if (arr.Length > 0)
                    {
                        string sql = "";
                        if (arr.Length == 1)
                        {
                            sql = String.Format("SELECT count(1) NUMS FROM BD_ZJRESULT WHERE AUDITSTATUS=1 AND CYID='{0}'", arr[0]);
                        }
                        else
                        {
                            string str = string.Join("','", arr);
                            sql = String.Format("SELECT count(1) NUMS FROM BD_ZJRESULT WHERE AUDITSTATUS=1 AND CYID in('{0}')", str);
                        }
                        int res = DataFactory.Database().FindCountBySql(sql);
                        if (res > 0)
                        {
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("选中的明细中有已化验审核的信息，请您确认后在撤销提交！") }.ToString());
                        }
                    }

                    string[] arr1 = OrderEntryList.Where(t => string.IsNullOrEmpty(t.ID) == false).Select(c => c.ID).ToArray();
                    if (arr1.Length > 0)
                    {
                        string sql = "";
                        if (arr.Length == 1)
                        {
                            sql = string.Format("select count(1) ss from VBD_ZJRESULT_KZ where  AUDITUSER is not null and AUDITUSER!='{0}' and id='{1}'", ManageProvider.Provider.Current().UserName, arr1[0]); ;
                        }
                        else
                        {
                            string str = string.Join("','", arr1);
                            sql = string.Format("select count(1) ss from VBD_ZJRESULT_KZ where  AUDITUSER is not null and AUDITUSER!='{0}' and id in('{1}')", ManageProvider.Provider.Current().UserName, str);
                        }
                        int res = DataFactory.Database().FindCountBySql(sql);
                        if (res > 0)
                        {
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("选中的明细中有他人审批的信息，请您确认后在撤销审批！") }.ToString());
                        }
                    }

                }
                else
                {
                    string[] arr = OrderEntryList.Where(t => string.IsNullOrEmpty(t.ID) == false).Select(c => c.ID).ToArray();
                    if (arr.Length > 0)
                    {
                        string sql = "";
                        if (arr.Length == 1)
                        {
                            sql = string.Format("select count(1) ss from VBD_ZJRESULT_KZ where  AUDITUSER is not null and AUDITUSER!='{0}' and id='{1}'", ManageProvider.Provider.Current().UserName, arr[0]); ;
                        }
                        else
                        {
                            string str = string.Join("','", arr);
                            sql = string.Format("select count(1) ss from VBD_ZJRESULT_KZ where  AUDITUSER is not null and AUDITUSER!='{0}' and id in('{1}')", ManageProvider.Provider.Current().UserName, str);
                        }
                        int res = DataFactory.Database().FindCountBySql(sql);
                        if (res > 0)
                        {
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("选中的明细中有他人审批的信息，请您确认后在审批！") }.ToString());
                        }
                    }
                }
                #endregion
                string Message = "采购处置审批成功。";
                //处理明细
                int index = 1;
                string AUDITDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                string AUDITUSER = ManageProvider.Provider.Current().UserName;
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                decimal SUMKS = 0;
                List<BD_PCRAWCYDETAILS> list_cydtl = null;
                foreach (VBD_ZJRESULT_KZ entity in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(entity.PCID))
                    {
                        //更新采样的采购处置扣重

                        if (!string.IsNullOrEmpty(entity.MAINID))
                        {
                            if (AUDITSTATUS == "0")
                            {
                                strSql.Clear();
                                strSql.Append(string.Format("update BD_PCRAWCYDETAILS set AUDITUSER='{0}',AUDITDATE='{1}',AUDITMEMO='{2}',AUDITSTATUS={3} where ID='{4}'", "", "", "", AUDITSTATUS, entity.ID));
                                database.ExecuteBySql(strSql, isOpenTrans);
                                strSql.Clear();
                                strSql.Append(string.Format("update BD_PCRAWCY set SUMKS={0} where ID='{1}'", 0, entity.MAINID));
                                database.ExecuteBySql(strSql, isOpenTrans);
                                strSql.Clear();
                                strSql.Append(string.Format("update BD_ZJRESULT set KS={0},KZAUDITUSER='',KZAUDITDATE='',KZAUDITSTATUS=0 where CYID='{1}'", 0, entity.CYID));
                                database.ExecuteBySql(strSql, isOpenTrans);
                            }
                            else
                            {
                                strSql.Clear();
                                strSql.Append(string.Format("update BD_PCRAWCYDETAILS set AUDITUSER='{0}',AUDITDATE='{1}',AUDITMEMO='{2}',AUDITSTATUS={3} where ID='{4}'", AUDITUSER, AUDITDATE, entity.AUDITMEMO, AUDITSTATUS, entity.ID));
                                database.ExecuteBySql(strSql, isOpenTrans);
                                list_cydtl = database.FindListBySql<BD_PCRAWCYDETAILS>(string.Format("SELECT * FROM BD_PCRAWCYDETAILS WHERE MAINID='{0}'", entity.MAINID));
                                SUMKS = list_cydtl.Sum(p => p.KS);
                                //更新
                                strSql.Clear();
                                strSql.Append(string.Format("update BD_PCRAWCY set SUMKS={0} where ID='{1}'", SUMKS, entity.MAINID));
                                database.ExecuteBySql(strSql, isOpenTrans);
                                strSql.Clear();
                                strSql.Append(string.Format("update BD_ZJRESULT set KS={0},KZAUDITUSER='{1}',KZAUDITDATE='{2}',KZAUDITSTATUS=1 where CYID='{3}'", SUMKS, ManageProvider.Provider.Current().UserName, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), entity.MAINID));
                                database.ExecuteBySql(strSql, isOpenTrans);
                            }
                        }
                        index++;
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

        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            VBD_ZJRESULT entity = vorderbll.GetEntity(KeyValue);
            if (!ManageProvider.Provider.Current().IsSystem && entity.CREUSER != ManageProvider.Provider.Current().UserName)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前数据只有创建人才能删除！" }.ToString());
            }
            if (entity.AUDITSTATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前数据已审批，请取消审批后删除数据！" }.ToString());
            }
            if (entity.KZAUDITSTATUS == "1" && entity.ISTYPE == 2)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "当前数据已扣杂提交，请取消扣杂提交后删除数据！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("BD_ZJRESULT", "ID", KeyValue);
                database.Delete("BD_ZJRESULTDETAILS", "MAINID", KeyValue);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=4,DEF10='{0}' where ID='{1}'", "", entity.CYID));
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
        //化验判定
        [LoginAuthorize]
        public ActionResult CheckHY(BD_ZJRESULT entity, string OrderEntryJson, string KeyValue, string ModuleId)
        {
            IDatabase database = DataFactory.Database();
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(entity.MATERIAL))
                {
                    //List<BD_ZJRESULTDETAILS> OrderEntryList = OrderEntryJson.JonsToList<BD_ZJRESULTDETAILS>();
                    //int ss = OrderEntryList.RemoveAll(a => string.IsNullOrEmpty(a.ZJFA));
                    //if (OrderEntryList.Count > 0)
                    //{
                    ////查询物料标准
                    //List<PD_GBMATERIAL> mlist = database.FindListBySql<PD_GBMATERIAL>("SELECT * FROM PD_GBMATERIAL WHERE MATERIAL='" + entity.PK_MATERIAL + "' AND GBCHECKTYPENAME  IN ('判定标准','内控标准') AND  ISUSE=1 ");
                    //if (mlist.Count < 0)
                    //{
                    //    message.Code = -1;
                    //    message.Content = "未查询到物料标准";
                    //}
                    //else
                    //{
                    //    message.Code = 1;

                    //bool gb = false, nk = false;
                    //PD_GBMATERIAL mgb = mlist.Find(a => a.GBTYPENAME == "国标");
                    //PD_GBMATERIAL mnk = mlist.Find(a => a.GBTYPENAME == "内控");
                    //if (mgb != null)
                    //{
                    //    List<PD_GBMANAGER_DTL> manager = database.FindListBySql<PD_GBMANAGER_DTL>("SELECT * FROM PD_GBMANAGER_DTL WHERE PK_GBMANAGER='" + mgb.GBEXECNO + "' AND CHECKTYPE=2 ");
                    //    gb = CommCheck(manager, OrderEntryList);
                    //    message.Content = "\"国标\":\"" + (gb ? "合格" : "不合格") + "\",\"GB\":\"" + (gb ? "合格" : "不合格") + "\"";
                    //}
                    //else
                    //{
                    //    message.Content = "\"国标\":\"不合格\",\"GB\":\"不合格\"";
                    //}


                    //if (mnk != null)
                    //{
                    //    List<PD_GBMANAGER_DTL> manager = database.FindListBySql<PD_GBMANAGER_DTL>("SELECT * FROM PD_GBMANAGER_DTL WHERE PK_GBMANAGER='" + mnk.GBEXECNO + "' AND CHECKTYPE=2 ");

                    //    nk = //CommCheck(manager, OrderEntryList);
                    //    message.Content = "{" + (string.IsNullOrEmpty(message.Content.ToString()) ? "" : message.Content + ",") + "\"内控\":\"" + (nk ? "合格" : "不合格") + "\",\"NK\":\"" + (nk ? "合格" : "不合格") + "\"}";
                    //}
                    //else
                    //{
                    //    message.Content = "{" + (string.IsNullOrEmpty(message.Content.ToString()) ? "" : message.Content + ",") + "\"内控\":\"不合格\",\"NK\":\"不合格\"}";
                    //}

                    // }
                    //}
                    //else
                    //{
                    //    message.Code = -1;
                    //    message.Content = "明细信息为空";
                    //}

                }
                else
                {
                    message.Code = -1;
                    message.Content = "物料为空";
                }


                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
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

        //化验项判定2
        [LoginAuthorize]
        public string PCRawCHECheckZJBZ2(string ZJITEMNO, string ZJFA, string ZJValue)
        {
            ResultMessage message = new ResultMessage();
            if (!string.IsNullOrEmpty(ZJITEMNO))
            {
                message = vpcbll.CheckZJBZ(ZJITEMNO, ZJFA, ZJValue);
                return message.Code.ToString();
            }
            else
            {
                return "X";
            }
        }

        //加权计算
        public ActionResult CheckData(string KeyValue)
        {
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                List<VWB_WEIGHT_B> list = new List<VWB_WEIGHT_B>();
                VWB_WEIGHT_B model = null;
                DataTable dt = GetAVGZJRESULTList(KeyValue);
                AV_ZJRESULT avgmodel = null;
                List<AV_ZJRESULT> avglist = new List<AV_ZJRESULT>();
                //明细行数据
                foreach (DataRow row in dt.Rows)
                {
                    avgmodel = new AV_ZJRESULT();
                    avgmodel.ZJITEM = row["ZJITEM"].ToString();
                    avgmodel.ZJITEMNAME = row["ZJITEMNAME"].ToString();
                    avgmodel.ZJVALUE = row["ZJVALUE"].ToString();
                    avglist.Add(avgmodel);
                }
                if (avglist.Count > 0)
                {
                    var JsonData = new
                    {
                        rows = avglist
                    };
                    return Content(JsonData.ToJson());
                }
                else
                {
                    return null;
                }
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
            strSql = string.Format(@"select ZJITEM,ZJITEMNAME,round(AVG(is_number(ZJVALUE)),1) ZJVALUE  from BD_ZJRESULTDETAILS where mainid 
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
                    //调用触发OA流程接口 221006 myt/liuyan
                    WEBSERVICES_SAP.SAPWebServiceSoapClient sap = new WEBSERVICES_SAP.SAPWebServiceSoapClient();
                    string res = sap.WRZS_OADRYRETESTZS(KeyValue).REMSG;
                    //调用触发OA流程接口 221006 myt/liuyan
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() + res }.ToString());
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
        public ActionResult CHECheck(string KeyValue, string KeyValue1, string PCRAWTYPE)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = vorderbll.PCRawCheck(KeyValue, KeyValue1, PCRAWTYPE);
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
        public ActionResult CHEUNCheck(string KeyValue, string KeyValue1, string PCRAWTYPE)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = vorderbll.PCRawUNCheck(KeyValue, KeyValue1, PCRAWTYPE);
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
        public ActionResult CheckReport1(string Parm_Key_Value, string Type)
        {
            DataSet dataSet = vorderbll.CheckReport1(Parm_Key_Value, Type);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, false);
            }
            return null;
        }
        #endregion
    }
}
