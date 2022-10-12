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

namespace EOS.WebApp.Areas.ZJManager.Controllers
{
    public class PCRawCYController : PublicController<BD_PCRAWCY>
    {
        string ModuleId = "";
        BD_PCRAWCYBll pcbll = new BD_PCRAWCYBll();
        VBD_PCRAWCYBll vpcbll = new VBD_PCRAWCYBll();

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


        [LoginAuthorize]
        public ActionResult PrintViewReportIndex()
        {
            return View();
        }
        //采样交样报表
        [LoginAuthorize]
        public ActionResult CYJYReportIndex()
        {
            return View();
        }

        //国废分拣工分拣产量统计
        [LoginAuthorize]
        public ActionResult FZFJReportIndex()
        {
            return View();
        }


        [LoginAuthorize]
        public ActionResult CYJYReportIndex1()
        {
            return View();
        }
        //采样明细报表
        [LoginAuthorize]
        public ActionResult CYReportIndex()
        {
            return View();
        }
        //采购化工采样单
        [LoginAuthorize]
        public ActionResult CYWeightInIndex()
        {
            return View();
        }

        //采购包材采样单
        [LoginAuthorize]
        public ActionResult CYWeightInIndex1()
        {
            return View();
        }

        //采购计量单生采样单
        [LoginAuthorize]
        public ActionResult CYWeightInIndex2()
        {
            return View();
        }

        //采购计量单生采样单
        [LoginAuthorize]
        public ActionResult CYWeightInIndex3()
        {
            return View();
        }

        //原料采制样查询
        [LoginAuthorize]
        public ActionResult PCRawCYIndex()
        {
            return View();
        }


        //原料
        [LoginAuthorize]
        public ActionResult PCRawCYList()
        {
            return View();
        }
        //原煤
        [LoginAuthorize]
        public ActionResult PCRawCYListYM()
        {
            return View();
        }
        //木片
        [LoginAuthorize]
        public ActionResult PCRawCYListMP()
        {
            return View();
        }
        //化工
        [LoginAuthorize]
        public ActionResult PCRawCYList1()
        {
            return View();
        }
        //包材
        [LoginAuthorize]
        public ActionResult PCRawCYList2()
        {
            return View();
        }
        //木浆
        [LoginAuthorize]
        public ActionResult PCRawCYList3()
        {
            return View();
        }
        //废纸
        [LoginAuthorize]
        public ActionResult PCRawCYList4()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCYSGList()
        {
            return View();
        }

        /// <summary>
        /// 接样
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawJYList()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult PCRawCYForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().Account;
            }
            return View();
        }
        //木片
        [LoginAuthorize]
        public ActionResult PCRawCYFormMP()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().Account;
            }
            return View();
        }
        //包材
        [LoginAuthorize]
        public ActionResult PCRawCYForm2()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];

            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().Account;
            }
            return View();
        }
        //木浆
        [LoginAuthorize]
        public ActionResult PCRawCYForm3()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];

            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().Account;
            }
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCYWGForm()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCYSGForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];

            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().Account;
            }

            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCYSGForm1()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];

            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().Account;
            }

            return View();
        }
        //木片一次采样
        [LoginAuthorize]
        public ActionResult PCRawCYSGFormMP()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];

            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().Account;
            }

            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCYSGForm2()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];

            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().Account;
            }

            return View();
        }

        [LoginAuthorize]
        public ActionResult PCRawCYFormSetMaterialSupply()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult PrintCode()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult PCRawCYCardNo()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCYMuilMaterial()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCYDeleteCar()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCYAddCar()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult PCRawCYDeleteCar1()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawCYAddCar1()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult PCRawCYDetails()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SelectMaterialType()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SelectMaterial()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult SelectJYCYData()
        {
            ViewBag.SYUSER = ManageProvider.Provider.Current().UserName;
            return View();
        }
        [LoginAuthorize]
        public ActionResult SelectJYCYSGData()
        {
            ViewBag.SYUSER = ManageProvider.Provider.Current().UserName;
            return View();
        }

        [LoginAuthorize]
        public ActionResult SELECTWEIGHTS()
        {
            return View();
        }

        [LoginAuthorize]
        public ActionResult SELECTWEIGHTSHY()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SELECTPCBillSOF()
        {
            return View();
        }
        //包材取样数据
        [LoginAuthorize]
        public ActionResult SELECTPCBillSOF2()
        {
            return View();
        }
        //木浆取样数据
        [LoginAuthorize]
        public ActionResult SELECTPCBillSOF3()
        {
            return View();
        }
        //采样交接记录
        public ActionResult CYJYDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = vpcbll.CYJYDataSetReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        public ActionResult CYJYDataSetReport1(string Parm_Key_Value)
        {
            DataSet dataSet = vpcbll.CYJYDataSetReport1(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        //采样明细
        public ActionResult CYDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = vpcbll.CYDataSetReport(Parm_Key_Value);

            return Content(dataSet.GetXml());
        }
        //国废分拣工分拣产量统计
        public ActionResult FZFJDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = vpcbll.FZFJDataSetReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 重写 采样组批 编辑页面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public override ActionResult SetForm(string KeyValue)
        {
            VBD_PCRAWCY entity = DataFactory.Database().FindEntity<VBD_PCRAWCY>(KeyValue);
            return Content(entity.ToJson());
        }
        public ActionResult GetICCardEntity(string KeyValue)
        {
            VBD_PCRAWCY entity = vpcbll.GetICCardToEntity(KeyValue);
            return Content(entity.ToJson());
        }
        public ActionResult GetICCardEntity1(string KeyValue)
        {
            VBD_PCRAWCY entity = vpcbll.GetICCardToEntity1(KeyValue);
            return Content(entity.ToJson());
        }
        public ActionResult SetFormControl(string KeyValue)
        {
            VBD_PCRAWCYDETAILS entity = DataFactory.Database().FindEntity<VBD_PCRAWCYDETAILS>(KeyValue);
            return Content(entity.ToJson());
        }

        public ActionResult GetICCardEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = vpcbll.GetICCardEntryList(KeyValue),
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

        #region 数据列表
        /// <summary>
        /// 获取采样组批列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPCRawList(string BILLNO, string HANDBILLNO, string PRINTCODE, string MATERIALNAME, string MATERIALSPEC, string PK_ORG, string SUPPLYNAME, string CARS, string CHECKTYPE, string CHECKISSEND, string PCRAWTYPE, string SUMKZ, string DEF6, string DEF5, string ISZK, string CYUSER, string CYTYPE, string CQDEF1, string PK_STORE, string ISJL, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCY> ListData = vpcbll.GetOrderList(BILLNO, HANDBILLNO, PRINTCODE, MATERIALNAME, MATERIALSPEC, PK_ORG, SUPPLYNAME, CARS, CHECKTYPE, CHECKISSEND, PCRAWTYPE, SUMKZ, DEF6, DEF5, ISZK, CYUSER, CYTYPE, CQDEF1, PK_STORE, ISJL, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetPCRawJYList(string BILLNO, string MATERIALNAME, string PK_ORG, string SUPPLYNAME, string CARNAME, string CHECKISSEND, string PCRAWTYPE, string SUMKZ, string DEF6, string CQDEF1, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCY> ListData = vpcbll.GetOrderListJY(BILLNO, MATERIALNAME, PK_ORG, SUPPLYNAME, CARNAME, CHECKISSEND, PCRAWTYPE, SUMKZ, DEF6, CQDEF1, StartTime, EndTime, jqgridparam);
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
        /// 通过IC卡号获取通知单数据
        /// </summary>
        /// <param name="ICNO"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetData(string ICNO, string MATERIAL, string SUPPLY, string PCRAWID)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(ICNO))
                {
                    return Content(pcbll.GetData(ICNO, MATERIAL, SUPPLY, PCRAWID));
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
        /// 通过IC卡号获取通知单数据
        /// </summary>
        /// <param name="ICNO"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetDataForZJQY(string ID)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(ID))
                {
                    return Content(pcbll.GetDataForZJQY(ID));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(ID, OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }


        /// <summary>
        /// 采样如果一卡多物料，查询多物料去显示  根据通知单主子表主键去查询
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="DETAILSID"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetDataForDetailsID(string KeyValue, string DETAILSID)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(KeyValue) && !string.IsNullOrEmpty(DETAILSID))
                {
                    return Content(pcbll.GetDataForDetailsID(KeyValue, DETAILSID));
                }
                else
                {
                    return Content("");
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 采样多物料显示出来去选择
        /// </summary>
        /// <param name="ICNO"></param>
        /// <param name="MATERIAL"></param>
        /// <param name="SUPPLY"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetCYMuilMaterialSelect(string ICNO, string MATERIAL, string SUPPLY, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJQY> ListData = pcbll.GetCYMuilMaterialSelect(ICNO, MATERIAL, SUPPLY, jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                //return Content(pcbll.GetData(ICNO, MATERIAL, SUPPLY));
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 通过IC卡号获取自动组批信息
        /// </summary>
        /// <param name="ICNO"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetAutoZP(string ICNO, string MATERIAL, string SUPPLY)
        {
            try
            {
                if (!string.IsNullOrEmpty(ICNO))
                {
                    ResultMessage resultmessage = pcbll.GetAutoZP(ICNO, MATERIAL, SUPPLY);
                    return Content(resultmessage.ToString());
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
        /// 获取采样车辆明细信息
        /// </summary>
        /// <param name="MAINID"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPCRawListItem(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCYDETAILS> ListData = vpcbll.GetOrderListItem(MAINID, jqgridparam);
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
        public ActionResult GetPCRawListItemBC(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCYDETAILSBC> ListData = vpcbll.GetOrderListItemBC(MAINID, jqgridparam);
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

        //采样关联显示卸货地点
        [LoginAuthorize]
        public ActionResult GetPCRawListItem1(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCYDETAILS1> ListData = vpcbll.GetOrderListItem1(MAINID, jqgridparam);
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
        public ActionResult GetOrderEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = vpcbll.GetOrderEntryList(KeyValue),
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
        public ActionResult GetOrderPicEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = vpcbll.GetOrderPicEntryList(KeyValue),
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
        public ActionResult GetOrderEntryList1(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = vpcbll.GetOrderEntryList1(KeyValue),
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
        /// 火运通知单
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="MATERIAL"></param>
        /// <param name="SUPPLY"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetHYOrderList(string PCRAWID, string StartTime, string EndTime, string PK_MARBASCLASS, string MATERIAL, string SUPPLY, string PK_MARBASCLASSNAME, string MATERIALNAME, string SUPPLYNAME, string CARS, string CARSNO, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJHY> ListData = vpcbll.GetHYOrderList(PCRAWID, StartTime, EndTime, PK_MARBASCLASS, MATERIAL, SUPPLY, PK_MARBASCLASSNAME, MATERIALNAME, SUPPLYNAME, CARS, CARSNO, jqgridparam);
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
        /// 通过火运通知单号获取通知单信息
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYGetHYData(string KeyValue)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    return Content(pcbll.GetHYData(KeyValue));
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
        /// 通过卡面编号获取当前卡号
        /// </summary>
        /// <param name="CardNo"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetICNO(string CardNo, string TYPE)
        {
            ResultMessage messge = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(CardNo))
                {
                    messge = pcbll.GetCardNo(CardNo, TYPE);
                    return Content(new JsonMessage { Success = false, Code = messge.Code.ToString(), Message = messge.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "卡号不能为空" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 移除组批车辆明细列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetPCRawCYDeleteCar(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCYDETAILS> ListData = vpcbll.GetOrderListItem(KeyValue, jqgridparam);
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
        /// 移入组批明细 查询通知单列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="jqgridparam"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPCRawCYAddCar(string KeyValue, string StartTime, string EndTime, string CARNAME, string CarsBillNoType, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJQYNOFINISH> ListData = vpcbll.GetPCRawCYAddCar(KeyValue, StartTime, EndTime, CARNAME, CarsBillNoType, jqgridparam);
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
        /// 移入组批明细 查询过磅单列表  火运
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPCRawTrainCYAddCar(string KeyValue, string StartTime, string EndTime, string CARNAME, string CARSNO, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJHY> ListData = vpcbll.GetPCRawTrainCYAddCar(KeyValue, StartTime, EndTime, CARNAME, CARSNO, jqgridparam);
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


        #endregion

        #region 表单提交
        /// 提交表单 
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="PCRAWCYDetailJson">子表数据</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYSave(BD_PCRAWCY entity, string KeyValue, string PCRAWCYDetailJson, string ModuleId)
        {
            string sql = "";
            int res = 0;
            #region 验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<BD_PCRAWCYDETAILS> OrderEntryList = PCRAWCYDetailJson.JonsToList<BD_PCRAWCYDETAILS>();
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_POCARSORDER set ISCY=0 where ID in(select PCID from BD_PCRAWCYDETAILS where MAINID='{0}')", KeyValue));
                    repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                    database.Delete<BD_PCRAWCYDETAILS>("MAINID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    //entity.PRINTCODE = string.Format("ZY{0}", entity.BILLNO.Substring(2));
                    //entity.SBILLNO = string.Format("SM{0}", entity.BILLNO.Substring(2));
                    //entity.MYBILLNO = string.Format("MY{0}", entity.BILLNO.Substring(2));
                    //entity.YBILLNO = string.Format("YM{0}", entity.BILLNO.Substring(2));
                    //entity.GBILLNO = string.Format("GM{0}", entity.BILLNO.Substring(2));
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from BD_PCRAWCY where  BILLNO='{0}'", entity.BILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    entity.PRINTCODE = string.Format("ZY{0}", entity.BILLNO.Substring(2));
                    entity.SBILLNO = string.Format("SM{0}", entity.BILLNO.Substring(2));
                    entity.MYBILLNO = string.Format("MY{0}", entity.BILLNO.Substring(2));
                    entity.YBILLNO = string.Format("YM{0}", entity.BILLNO.Substring(2));
                    entity.GBILLNO = string.Format("GM{0}", entity.BILLNO.Substring(2));
                    database.Insert(entity, isOpenTrans);
                }

                #region 过滤
                bool bol = true;
                string[] arr = OrderEntryList.Select(x => x.PCITEMID).ToArray();
                string strPC = string.Join("','", arr);
                //已取样
                List<BD_PCRAWCYDETAILS> entitylist = database.FindList<BD_PCRAWCYDETAILS>(string.Format(" AND PCITEMID in('{0}') and MAINBILLNO!='{1}'", strPC, entity.BILLNO));
                string[] arr1 = entitylist.Select(x => x.PCITEMID).ToArray();
                string strPC1 = string.Join("','", arr1);
                #endregion

                //处理明细
                int index = 1;
                foreach (BD_PCRAWCYDETAILS orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.PCID))
                    {
                        if (bol)
                        {
                            //采样明细
                            orderentry.Create();
                            orderentry.MAINID = entity.ID;
                            orderentry.MAINBILLNO = entity.BILLNO;
                            orderentry.MEMO = orderentry.MEMO.Replace("null", "");
                            database.Insert(orderentry, isOpenTrans);
                            //唯一
                            strSql.Clear();
                            strSql.Append(string.Format("INSERT INTO BD_PCRAWCYDETAILSCHECK(PCITEMID,PCID) VALUES('{0}','{1}')", orderentry.PCITEMID, orderentry.PCID));
                            database.ExecuteBySql(strSql, isOpenTrans);
                            //更新是否采样状态
                            strSql.Clear();
                            strSql.Append(string.Format("update DP_POCARSORDER set ISCY=1 where ID='{0}'", orderentry.PCID));
                            database.ExecuteBySql(strSql, isOpenTrans);
                            index++;
                        }
                    }
                }

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单号:{0}，新增成功。", entity.BILLNO);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// 随机号提交表单 
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="PCRAWCYDetailJson">子表数据</param>
        /// <returns></returns>
        public ActionResult PCRawCYSave1(BD_PCRAWCY entity, string KeyValue, string PCRAWCYDetailJson, string ModuleId)
        {
            string sql = "";
            int res = 0;

            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<BD_PCRAWCYDETAILS> OrderEntryList = PCRAWCYDetailJson.JonsToList<BD_PCRAWCYDETAILS>();
                WB_MATERIAL_JYPBll jyp_bll = new WB_MATERIAL_JYPBll();
                WB_MATERIAL_JYP jyp_entity = jyp_bll.GetEntity(entity.MATERIAL);
                string CODEDATE = DateTime.Now.ToString("yyMMdd");


                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_POCARSORDER_DTL set ISCY=0 where ID in(select PCITEMID from BD_PCRAWCYDETAILS where MAINID='{0}')", KeyValue));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    database.Delete<BD_PCRAWCYDETAILS>("MAINID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    entity.CZTYPENAME = entity.CZTYPE;
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from BD_PCRAWCY where  BILLNO='{0}'", entity.BILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.CZTYPENAME = entity.CZTYPE;
                    entity.Create();

                    //是否明码
                    if (jyp_entity.ISMM == "1")
                    {
                        entity.HANDBILLNO = entity.BILLNO;
                        entity.PRINTCODE = entity.BILLNO;
                        entity.SBILLNO = entity.BILLNO;
                        if (string.IsNullOrEmpty(entity.MYBILLNO))
                        {
                            entity.MYBILLNO = entity.BILLNO;
                        }
                        entity.YBILLNO = entity.BILLNO;
                        entity.GBILLNO = entity.BILLNO;
                    }
                    else
                    {
                        string HANDBILLNO = "", PRINTCODE = "";
                        HANDBILLNO = CreateBillNo("", 5);//取样码
                        if (string.IsNullOrEmpty(entity.PRINTCODE))
                        {
                            PRINTCODE = CreateBillNo("", 4);//制样码
                        }
                        if (!string.IsNullOrEmpty(jyp_entity.BIRTHCODE))
                        {
                            entity.HANDBILLNO = string.Format("Q{0}{1}", jyp_entity.BIRTHCODE, HANDBILLNO.Substring(2));
                            if (string.IsNullOrEmpty(entity.PRINTCODE))
                            {
                                entity.PRINTCODE = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            }
                            entity.SBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            if (string.IsNullOrEmpty(entity.MYBILLNO))
                            {
                                entity.MYBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            }
                            entity.YBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            entity.GBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                        }
                        else
                        {
                            entity.HANDBILLNO = HANDBILLNO;
                            if (string.IsNullOrEmpty(entity.PRINTCODE))
                            {
                                entity.PRINTCODE = PRINTCODE;
                            }
                            entity.SBILLNO = PRINTCODE;
                            if (string.IsNullOrEmpty(entity.MYBILLNO))
                            {
                                entity.MYBILLNO = PRINTCODE;
                            }
                            entity.YBILLNO = PRINTCODE;
                            entity.GBILLNO = PRINTCODE;
                        }
                    }

                    sql = String.Format("select count(1) from BD_PCRAWCY where  HANDBILLNO='{0}'", entity.HANDBILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "采样单号已存在,请您重新保存操作！" }.ToString());
                    }

                    database.Insert(entity, isOpenTrans);
                }

                #region 过滤
                bool bol = true;
                string[] arr = OrderEntryList.Select(x => x.PCITEMID).ToArray();
                string strPC = string.Join("','", arr);
                //已取样
                List<BD_PCRAWCYDETAILS> entitylist = database.FindList<BD_PCRAWCYDETAILS>(string.Format(" AND PCITEMID in('{0}') and MAINBILLNO!='{1}'", strPC, entity.BILLNO));
                string[] arr1 = entitylist.Select(x => x.PCITEMID).ToArray();
                string strPC1 = string.Join("','", arr1);
                #endregion

                //处理明细
                int index = 1;
                //派车表实例
                VBD_MATERIAL material = null;

                foreach (BD_PCRAWCYDETAILS orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.PCID))
                    {
                        if (strPC1.Contains(orderentry.PCITEMID))
                        {
                            bol = false;
                        }
                        if (bol)
                        {
                            material = database.FindEntityByWhere<VBD_MATERIAL>(string.Format(" AND ID='{0}'", orderentry.MATERIAL));
                            //采样明细
                            orderentry.Create();
                            orderentry.MAINID = entity.ID;
                            orderentry.MAINBILLNO = entity.BILLNO;
                            orderentry.MEMO = orderentry.MEMO.Replace("null", "");
                            database.Insert(orderentry, isOpenTrans);
                            strSql.Clear();
                            strSql.Append(string.Format("INSERT INTO BD_PCRAWCYDETAILSCHECK(PCITEMID,PCID) VALUES('{0}','{1}')", orderentry.PCITEMID, orderentry.PCID));
                            database.ExecuteBySql(strSql, isOpenTrans);

                            //更新是否采样状态
                            //if (material.ISTYPE == 5 && material.ISJL == 0)
                            //{
                            //    sql = String.Format("SELECT count(1) NUMS FROM WB_WEIGHT WHERE  PK_ORDER='{0}'", orderentry.PCID);
                            //    res = DataFactory.Database().FindCountBySql(sql);
                            //    if (res == 0)
                            //    {
                            //        //包材不计量直接采样出厂
                            //        strSql.Clear();
                            //        strSql.Append(string.Format("update DP_POCARSORDER set  ISUSE=6 where ID='{0}'", orderentry.PCID));
                            //        database.ExecuteBySql(strSql, isOpenTrans);
                            //        strSql.Clear();
                            //        strSql.Append(string.Format("update DP_POCARSORDER_DTL set ISCY=1,ISUSE=2,MEMO=MEMO||'{0}' where ID='{1}'", orderentry.MEMO, orderentry.PCITEMID));
                            //        database.ExecuteBySql(strSql, isOpenTrans);
                            //        strSql.Clear();
                            //        strSql.Append(string.Format("delete DP_CARSORDER where ORDERID='{0}'", orderentry.PCID));
                            //        database.ExecuteBySql(strSql, isOpenTrans);
                            //    }
                            //}
                            //else
                            //{
                            strSql.Clear();
                            strSql.Append(string.Format("update DP_POCARSORDER_DTL set ISCY=1,MEMO=MEMO||'{0}' where ID='{1}'", orderentry.MEMO, orderentry.PCITEMID));
                            database.ExecuteBySql(strSql, isOpenTrans);
                            //}
                            index++;
                        }
                    }
                }
                database.Commit();
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单号:{0}，新增成功。", entity.BILLNO);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);

                return Content(new JsonMessage { Success = true, Code = "1", Message = Message, BillNo = entity.ID }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// 采样制卡 
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="PCRAWCYDetailJson">子表数据</param>
        /// <returns></returns>
        public ActionResult PCRawCYSave3(BD_PCRAWCY entity, string KeyValue, string PCRAWCYDetailJson, string ModuleId)
        {
            string sql = "";
            int res = 0;
            #region 验证
            //是否固定水份
            bool ischeck = false;
            string MCAAMOUNT = "0";
            BD_MaterialBll matbll = new BD_MaterialBll();
            VBD_MATERIAL mat = matbll.GetEntity(entity.MATERIAL);
            if (mat != null)
            {
                if (mat.ISMC == 1)
                {
                    ischeck = true;
                    MCAAMOUNT = mat.MCAMOUNT;
                }
            }
            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<BD_PCRAWCYDETAILS> OrderEntryList = PCRAWCYDetailJson.JonsToList<BD_PCRAWCYDETAILS>();
                WB_MATERIAL_JYPBll jyp_bll = new WB_MATERIAL_JYPBll();
                WB_MATERIAL_JYP jyp_entity = jyp_bll.GetEntity(entity.MATERIAL);
                string CODEDATE = DateTime.Now.ToString("yyMMdd");
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update DP_POCARSORDER_DTL set ISCY=0 where ID in(select PCITEMID from BD_PCRAWCYDETAILS where MAINID='{0}')", KeyValue));
                    database.ExecuteBySql(strSql, isOpenTrans);
                    database.Delete<BD_PCRAWCYDETAILS>("MAINID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    sql = String.Format("select count(1) from BD_PCRAWCY where  BILLNO='{0}'", entity.BILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    entity.ISZK = "1";
                    entity.DEF7 = entity.DEF6;

                    //是否明码
                    if (jyp_entity.ISMM == "1")
                    {
                        entity.HANDBILLNO = entity.BILLNO;
                        entity.PRINTCODE = entity.BILLNO;
                        entity.SBILLNO = entity.BILLNO;
                        entity.MYBILLNO = entity.BILLNO;
                        entity.YBILLNO = entity.BILLNO;
                        entity.GBILLNO = entity.BILLNO;
                    }
                    else
                    {
                        string HANDBILLNO = "", PRINTCODE = "";
                        HANDBILLNO = CreateBillNo("", 5);//取样码
                        PRINTCODE = CreateBillNo("", 4);//制样码
                        if (!string.IsNullOrEmpty(jyp_entity.BIRTHCODE))
                        {
                            entity.HANDBILLNO = string.Format("Q{0}{1}", jyp_entity.BIRTHCODE, HANDBILLNO.Substring(2));
                            entity.PRINTCODE = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            entity.SBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            entity.MYBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            entity.YBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            entity.GBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                        }
                        else
                        {
                            entity.HANDBILLNO = HANDBILLNO;
                            entity.PRINTCODE = PRINTCODE;
                            entity.SBILLNO = PRINTCODE;
                            entity.MYBILLNO = PRINTCODE;
                            entity.YBILLNO = PRINTCODE;
                            entity.GBILLNO = PRINTCODE;
                        }
                    }
                    //固定水分
                    if (ischeck == true)
                    {
                        entity.ISZY = "1";
                        entity.TYPE = "5";
                        entity.ISSEND = "1";
                        string hybillno = "";
                        //生成化验结果
                        VBD_ZJRESULTBll vorderbll = new VBD_ZJRESULTBll();
                        vorderbll.SaveZJRESULT(entity, MCAAMOUNT, database, isOpenTrans, out hybillno);
                        entity.DEF10 = hybillno;//化验号
                    }
                    sql = String.Format("select count(1) from BD_PCRAWCY where  HANDBILLNO='{0}'", entity.HANDBILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "采样单号已存在,请您重新保存操作！" }.ToString());
                    }
                    database.Insert(entity, isOpenTrans);
                }



                #region 过滤
                bool bol = true;
                string[] arr = OrderEntryList.Select(x => x.PCITEMID).ToArray();
                string strPC = string.Join("','", arr);
                //已取样
                List<BD_PCRAWCYDETAILS> entitylist = database.FindList<BD_PCRAWCYDETAILS>(string.Format(" AND PCITEMID in('{0}') and MAINBILLNO!='{1}'", strPC, entity.BILLNO));
                string[] arr1 = entitylist.Select(x => x.PCITEMID).ToArray();
                string strPC1 = string.Join("','", arr1);
                #endregion

                //处理明细
                int index = 1;
                foreach (BD_PCRAWCYDETAILS orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.PCID))
                    {
                        if (bol)
                        {
                            if (strPC1.Contains(orderentry.PCITEMID))
                            {
                                bol = false;
                            }
                            //采样明细
                            orderentry.Create();
                            orderentry.MAINID = entity.ID;
                            orderentry.MAINBILLNO = entity.BILLNO;
                            orderentry.MEMO = orderentry.MEMO.Replace("null", "");
                            database.Insert(orderentry, isOpenTrans);
                            strSql.Clear();
                            strSql.Append(string.Format("INSERT INTO BD_PCRAWCYDETAILSCHECK(PCITEMID,PCID) VALUES('{0}','{1}')", orderentry.PCITEMID, orderentry.PCID));
                            database.ExecuteBySql(strSql, isOpenTrans);
                            //更新是否采样状态
                            if (entity.PCRAWTYPE == "2" && orderentry.DEF7 == "0")
                            {
                                sql = String.Format("SELECT count(1) NUMS FROM WB_WEIGHT WHERE  PK_ORDER='{0}'", orderentry.PCID);
                                res = DataFactory.Database().FindCountBySql(sql);
                                if (res == 0)
                                {
                                    //包材不计量直接采样出厂
                                    strSql.Clear();
                                    strSql.Append(string.Format("update DP_POCARSORDER set  ISUSE=6 where ID='{0}'", orderentry.PCID));
                                    database.ExecuteBySql(strSql, isOpenTrans);
                                    strSql.Clear();
                                    strSql.Append(string.Format("update DP_POCARSORDER_DTL set ISCY=1,ISUSE=6,MEMO=MEMO||'{0}' where ID='{1}'", orderentry.MEMO, orderentry.PCITEMID));
                                    database.ExecuteBySql(strSql, isOpenTrans);
                                    strSql.Clear();
                                    strSql.Append(string.Format("delete DP_CARSORDER where ORDERID='{0}'", orderentry.PCID));
                                    database.ExecuteBySql(strSql, isOpenTrans);
                                }
                            }
                            else
                            {
                                strSql.Clear();
                                strSql.Append(string.Format("update DP_POCARSORDER_DTL set ISCY=1,MEMO=MEMO||'{0}' where ID='{1}'", orderentry.MEMO, orderentry.PCITEMID));
                                database.ExecuteBySql(strSql, isOpenTrans);
                            }
                            index++;
                        }
                    }
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单号:{0}，新增成功。", entity.BILLNO);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message, BillNo = entity.ID }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        /// <summary>
        /// 递归生码
        /// </summary>
        /// <param name="BILLNO"></param>
        /// <param name="type">1 水码，2 Y码/岩码，3 G码，4 总厂制样码，5 总厂取样码</param>
        /// <returns></returns>
        private string CreateBillNo(string BILLNO, int type)
        {
            if (string.IsNullOrEmpty(BILLNO))
            {
                BILLNO = SetBillNo(type);
            }
            string sql = "";
            if (type == 1)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where SBILLNO='{0}'", BILLNO);
            }
            else if (type == 2)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where YBILLNO='{0}'", BILLNO);
            }
            else if (type == 3)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where GBILLNO='{0}'", BILLNO);
            }
            else if (type == 4)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where PRINTCODE='{0}'", BILLNO);
            }
            else if (type == 5)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where HANDBILLNO='{0}'", BILLNO);
            }
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                BILLNO = SetBillNo(type);
                BILLNO = CreateBillNo(BILLNO, type);
            }
            return BILLNO;
        }

        private string CreateBillNo(string BILLNO, int type, string strformat)
        {
            if (string.IsNullOrEmpty(BILLNO))
            {
                BILLNO = SetBillNo(type, strformat);
            }
            string sql = "";
            if (type == 1)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where SBILLNO='{0}'", BILLNO);
            }
            else if (type == 2)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where YBILLNO='{0}'", BILLNO);
            }
            else if (type == 3)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where GBILLNO='{0}'", BILLNO);
            }
            else if (type == 4)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where PRINTCODE='{0}'", BILLNO);
            }
            else if (type == 5)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where HANDBILLNO='{0}'", BILLNO);
            }
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                BILLNO = SetBillNo(type);
                BILLNO = CreateBillNo(BILLNO, type, strformat);
            }
            return BILLNO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">1 水码，2 Y码，3 G码，4 制样码，5 取样码</param>
        /// <returns></returns>
        public string SetBillNo(int type)
        {
            string billno = "";
            Random r = new Random();
            int num = 1;
            if (type == 1)
            {
                num = r.Next(10000, 20000);
                billno = string.Format("SM{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfff"), num);
            }
            else if (type == 2)
            {
                num = r.Next(20001, 30001);
                billno = string.Format("YM{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfff"), num);
            }
            else if (type == 3)
            {
                num = r.Next(30002, 40002);
                billno = string.Format("GM{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfff"), num);
            }
            else if (type == 4)
            {
                num = r.Next(40003, 50003);
                billno = string.Format("ZY{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfff"), num);
            }
            else if (type == 5)
            {
                num = r.Next(50004, 60004);
                billno = string.Format("CY{0}{1}", DateTime.Now.ToString("yyMMddHHmmssfff"), num);
            }
            return billno;
        }


        public string SetBillNo(int type, string strformat)
        {
            string billno = "";
            Random r = new Random();
            int num = 1;
            if (type == 1)
            {
                num = r.Next(100, 200);
                billno = string.Format("SM{0}{1}", strformat, num);
            }
            else if (type == 2)
            {
                num = r.Next(201, 301);
                billno = string.Format("YM{0}{1}", strformat, num);
            }
            else if (type == 3)
            {
                num = r.Next(302, 402);
                billno = string.Format("GM{0}{1}", strformat, num);
            }
            else if (type == 4)
            {
                num = r.Next(403, 503);
                billno = string.Format("ZY{0}{1}", strformat, num);
            }
            else if (type == 5)
            {
                num = r.Next(504, 604);
                billno = string.Format("CY{0}{1}", strformat, num);
            }
            return billno;
        }
        /// <summary>
        /// 颗粒灰保存
        /// </summary>
        [LoginAuthorize]
        public ActionResult PCRawCYSave2(BD_PCRAWCY entity, string KeyValue, string PCRAWCYDetailJson, string ModuleId, string op)
        {
            #region 验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<BD_PCRAWCYDETAILS> OrderEntryList = PCRAWCYDetailJson.JonsToList<BD_PCRAWCYDETAILS>();


                string Message = op == "add" ? "新增成功。" : "编辑成功。";


                //处理主头
                if (!string.IsNullOrEmpty(KeyValue) && op == "edit")
                {
                    database.Delete<BD_PCRAWCYDETAILS>("MAINID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    entity.SUMKZ = 1;
                    entity.PRINTCODE = string.Format("ZY{0}", entity.BILLNO.Substring(2));
                    entity.SBILLNO = string.Format("SM{0}", entity.BILLNO.Substring(2));
                    entity.MYBILLNO = string.Format("MY{0}", entity.BILLNO.Substring(2));
                    entity.YBILLNO = string.Format("YM{0}", entity.BILLNO.Substring(2));
                    entity.GBILLNO = string.Format("GM{0}", entity.BILLNO.Substring(2));
                    database.Update(entity, isOpenTrans);
                    //处理明细
                    int index = 1;
                    foreach (BD_PCRAWCYDETAILS orderentry in OrderEntryList)
                    {
                        if (!string.IsNullOrEmpty(orderentry.PCID))
                        {
                            orderentry.Create();
                            orderentry.MAINID = entity.ID;
                            orderentry.MAINBILLNO = entity.BILLNO;
                            orderentry.MEMO = orderentry.MEMO.Replace("null", "");
                            database.Insert(orderentry, isOpenTrans);

                            index++;
                        }
                    }
                    database.Commit();
                }
                else
                {
                    int AllNum = Convert.ToInt32(entity.DEF2);
                    int SsNum = Convert.ToInt32(entity.DEF3);
                    int x = (int)Convert.ToByte('A');
                    for (int i = 1; i <= AllNum; i++)
                    {
                        base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        string sql = String.Format("select count(1) from BD_PCRAWCY where  BILLNO='{0}'", entity.BILLNO);
                        int res = DataFactory.Database().FindCountBySql(sql);
                        if (res > 0)
                        {
                            this.ModuleId = ModuleId;
                            entity.BILLNO = this.BillCode();
                        }
                        entity.Create();
                        entity.SUMKZ = 1;
                        entity.PRINTCODE = string.Format("ZY{0}", entity.BILLNO.Substring(2));
                        entity.SBILLNO = string.Format("SM{0}", entity.BILLNO.Substring(2));
                        entity.MYBILLNO = string.Format("MY{0}", entity.BILLNO.Substring(2));
                        entity.YBILLNO = string.Format("YM{0}", entity.BILLNO.Substring(2));
                        entity.GBILLNO = string.Format("GM{0}", entity.BILLNO.Substring(2));
                        if (i <= SsNum)
                        {
                            entity.DEF17 = "烧损";
                        }
                        else
                        {
                            entity.DEF17 = "";
                        }
                        database.Insert(entity, isOpenTrans);
                        //处理明细
                        int index = 1;
                        char y = Convert.ToChar(x);
                        foreach (BD_PCRAWCYDETAILS orderentry in OrderEntryList)
                        {
                            if (!string.IsNullOrEmpty(orderentry.PCID))
                            {
                                orderentry.Create();
                                orderentry.MAINID = entity.ID;
                                orderentry.MAINBILLNO = entity.BILLNO;
                                orderentry.MEMO = orderentry.MEMO.Replace("null", "");
                                if (AllNum > 1)
                                {
                                    orderentry.DEF5 = string.Format("{0}{1}", entity.DEF5, y);
                                }
                                orderentry.DEF6 = string.Format("{0}", i);
                                database.Insert(orderentry, isOpenTrans);
                                index++;
                            }
                        }
                        string Account = ManageProvider.Provider.Current().Account;
                        string message = string.Format("采样单号:{0}，新增成功。", entity.BILLNO);
                        Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                        x = x + 1;
                        database.Commit();
                    }
                }
                return Content(new JsonMessage { Success = true, Code = entity.ID, Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        [LoginAuthorize]
        public ActionResult PCRawCYWGSave(BD_PCRAWCYDETAILS entity, string KeyValue, string ModuleId)
        {
            #region 验证

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
                    BD_PCRAWCYDETAILS edit_entity = DataFactory.Database().FindEntity<BD_PCRAWCYDETAILS>(KeyValue);
                    entity.Modify(KeyValue);
                    edit_entity.WGTYPE = entity.WGTYPE;
                    edit_entity.WGKZ = entity.WGKZ;
                    edit_entity.MEMO = entity.MEMO;
                    database.Update(edit_entity, isOpenTrans);
                }

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单号:{0}，外观设置成功。", entity.MAINBILLNO);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Update, "1", message);
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
        /// 颗粒灰保存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawKLHCYSave(BD_PCRAWCY entity, string KeyValue, string ModuleId)
        {
            #region 验证

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
                    database.Delete<BD_PCRAWCYDETAILS>("MAINID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    entity.SUMKZ = 1;
                    entity.PRINTCODE = string.Format("ZY{0}", entity.BILLNO.Substring(2));
                    entity.SBILLNO = string.Format("SM{0}", entity.BILLNO.Substring(2));
                    entity.MYBILLNO = string.Format("MY{0}", entity.BILLNO.Substring(2));
                    entity.YBILLNO = string.Format("YM{0}", entity.BILLNO.Substring(2));
                    entity.GBILLNO = string.Format("GM{0}", entity.BILLNO.Substring(2));
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from BD_PCRAWCY where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    entity.SUMKZ = 1;
                    entity.CARSNUM = 1;
                    entity.PRINTCODE = string.Format("ZY{0}", entity.BILLNO.Substring(2));
                    entity.SBILLNO = string.Format("SM{0}", entity.BILLNO.Substring(2));
                    entity.MYBILLNO = string.Format("MY{0}", entity.BILLNO.Substring(2));
                    entity.YBILLNO = string.Format("YM{0}", entity.BILLNO.Substring(2));
                    entity.GBILLNO = string.Format("GM{0}", entity.BILLNO.Substring(2));
                    database.Insert(entity, isOpenTrans);
                }

                if (!string.IsNullOrEmpty(entity.DEF2))
                {
                    BD_PCRAWCYDETAILS orderentry = new BD_PCRAWCYDETAILS();
                    orderentry.Create();
                    orderentry.MAINID = entity.ID;
                    orderentry.MAINBILLNO = entity.BILLNO;
                    orderentry.PCID = "0";
                    orderentry.PCITEMID = "0";
                    orderentry.CARS = entity.DEF2;
                    orderentry.DEF5 = entity.DEF5;
                    orderentry.SUPPLY = entity.SUPPLY;
                    orderentry.MATERIAL = entity.MATERIAL;
                    orderentry.ORG = entity.DEF19;
                    orderentry.DEF3 = "0";
                    orderentry.DEF4 = "0";
                    database.Insert(orderentry, isOpenTrans);
                }


                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单号:{0}，新增成功。", entity.ID);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// 交样 
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYFinishNew(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "交接样成功。";
                string JYUSER = ManageProvider.Provider.Current().UserName;
                string JYDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=2,JYUSER='{0}',JYDATE='{1}' where ID in('{2}')", JYUSER, JYDATE, bills));
                repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单交接样成功。");
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
        public ActionResult PCRawCYFinish1(string KeyValue, string JYUSER, string JYUSER1, string SYUSER, string SYUSER1)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "交接样成功。";
                // string JYUSER = ManageProvider.Provider.Current().UserName;
                string JYDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=2,JYUSER='{0}',JYUSER1='{1}',SYUSER='{2}',SYUSER1='{3}',JYDATE='{4}',SYDATE='{4}'  where ID in('{5}')", JYUSER, JYUSER1, SYUSER, SYUSER1, JYDATE, bills));
                database.ExecuteBySql(strSql, isOpenTrans);
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单交接样成功。");
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
        public ActionResult PCRawCYFinish(string KeyValue, string JYUSER, string JYUSER1, string SYUSER, string SYUSER1)
        {
            string[] bills = KeyValue.TrimEnd(',').Split(',');
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "交接样成功。";

                string JYDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                foreach (string str in bills)
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update BD_PCRAWCY set TYPE=2,JYUSER='{0}',JYUSER1='{1}',SYUSER='{2}',SYUSER1='{3}',JYDATE='{4}',SYDATE='{4}'  where ID='{5}'", JYUSER, JYUSER1, SYUSER, SYUSER1, JYDATE, str));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单交接样成功。");
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
        /// 取消交样
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYUNFinishJY(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "取消交接样成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=1,JYUSER='',JYUSER1='',JYDATE='',SYUSER='',SYUSER1='',SYDATE='' where ID in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单取消交接样成功。");
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
        public ActionResult PCRawCYUNFinish(string KeyValue)
        {
            string[] bills = KeyValue.TrimEnd(',').Split(',');
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "取消交接样成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                foreach (string str in bills)
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update BD_PCRAWCY set TYPE=1,JYUSER='',JYUSER1='',JYDATE='',SYUSER='',SYUSER1='',SYDATE='' where ID='{0}'", str));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单取消交接样成功。");
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        ///  接样 
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYFinishNew1(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "接样成功。";
                string SYUSER = ManageProvider.Provider.Current().UserName;
                string SYDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=3,SYUSER='{0}',SYDATE='{1}' where ID in('{2}')", SYUSER, SYDATE, bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单接样成功。");
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
        /// 取消接样
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYUNFinish1(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "取消接样成功。";
                string SYUSER = "";
                string SYDATE = "";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=2,SYUSER='{0}',SYDATE='{1}' where ID in('{2}')", SYUSER, SYDATE, bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单取消接样成功。");
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
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
        /// 移入 通知单数据移入到指定批次
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="TASKLIST"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYAddCarItem(string KeyValue, string BILLNO)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = pcbll.PCRawCYAddCarItem(KeyValue, BILLNO);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中采样单号,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }



        /// <summary>
        /// 移入 通知单数据移入到指定批次  火运
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="TASKLIST"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawTrainCYAddCarItem(string KeyValue, string TASKLIST)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = pcbll.PCRawTrainCYAddCarItem(KeyValue, TASKLIST);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中采样单号,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        /// <summary>
        /// 制卡
        /// </summary>
        /// <param name="ICNO"></param>
        /// <param name="KeyValue"></param>
        /// <param name="TYPE"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYMakeCardSave(string ICNO, string KeyID, string TYPE)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(ICNO))
                {
                    if (!string.IsNullOrEmpty(KeyID))
                    {
                        message = pcbll.PCRawCYMakeCardSave(ICNO, KeyID, TYPE);
                        return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = (message.Code == 1 ? message.Content : message.Message).ToString() }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：异形卡号不能为空" }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：异形卡号不能为空" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        [LoginAuthorize]
        public ActionResult PCRawCYMakeCardSave1(string KeyID)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                message = pcbll.PCRawCYMakeCardSave1(KeyID);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());

            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 修改物料供户
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYSaveMaterialSupply(BD_PCRAWCY model)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(model.ID))
                {
                    message = pcbll.PCRawCYSaveMaterialSupply(model);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = (message.Code == 1 ? message.Content : message.Message).ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：ID不能为空,请稍后重试" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 表单操作

        /// <summary>
        /// 删除数据  
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteItem(string KeyValue)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string sql = String.Format("select count(1) from VBD_PCRAWCY where id in('{0}') and TYPE!=1 and istype!=4", bills);
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "只能删除采样状态数据,无法继续删除的操作！" }.ToString());
                }

                string Message = "删除成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();

                strSql.Clear();
                strSql.Append(string.Format("update DP_POCARSORDER_DTL set ISCY=0 where ID in(select PCITEMID from BD_PCRAWCYDETAILS where MAINID in('{0}'))", bills));
                database.ExecuteBySql(strSql, isOpenTrans);
                //删除采样验证表
                strSql.Clear();
                strSql.Append(string.Format("delete BD_PCRAWCYDETAILSCHECK where PCITEMID in(select PCITEMID from BD_PCRAWCYDETAILS where MAINID in('{0}'))", bills));
                database.ExecuteBySql(strSql, isOpenTrans);


                strSql.Clear();
                strSql.Append(string.Format("delete BD_PCRAWCY where ID in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("delete BD_PCRAWCYDETAILS where MAINID in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("delete BD_ZJZY where PCRAWID in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单:{0}删除成功。", "");
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public virtual ActionResult DeleteItem1(string KeyValue)
        {
            try
            {
                ResultMessage message = pcbll.PCRawCYDelete(KeyValue);
                WriteLog(message.Code, KeyValue, message.Message);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 删除组批明细车号
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="KeyList"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYDeleteItem(string KeyValue, string KeyList, string BillNoList)
        {
            try
            {
                ResultMessage message = pcbll.PCRawCYDeleteItem(KeyValue, KeyList, BillNoList);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 总厂删除组批明细车号
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="KeyList"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCYDeleteItem1(string KeyValue, string KeyList, string BillNoList)
        {
            try
            {
                ResultMessage message = pcbll.PCRawCYDeleteItem1(KeyValue, KeyList, BillNoList);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region  返回读卡脚本
        public ActionResult GetScript()
        {
            string strJson = "";
            strJson += "var key = '201003220000';var NiceKey='201003220000';";
            strJson += " function readData(pie_arr) {";
            strJson += " var iRet = CardOpen(); if (iRet == 0) { for (var i = 0; i < pie_arr.length; i++) { var pie = pie_arr[i]; if(pie=='11'){ iRet = CardCheckPwdStatus(pie, NiceKey);}else{ iRet = CardCheckPwdStatus(pie, key);} if (iRet == 0) { getVal(pie); } } iRet = CardClose(); }";
            strJson += "}";
            strJson += " function writeData(pie_arr) {";
            strJson += "   var iRet = CardOpen(); if (iRet == 0) {   for (var i = 0; i < pie_arr.length; i++) { var pie = pie_arr[i];if(pie=='11'){ iRet = CardCheckPwdStatus(pie, NiceKey);}else{ iRet = CardCheckPwdStatus(pie, key);} if (iRet == 0) { setVal(pie); } } iRet = CardClose(); }";
            strJson += "}";
            strJson += "   function CardCheckPwdStatus(pie, key) {";
            strJson += "        var res = 1; MWRFATL.RF_LoadKey(0, pie, key); var iRet = MWRFATL.LastRet; if (iRet) { res = 1; $('#msg').text('读写器装载密码失败！'); } else { MWRFATL.RF_Authentication(0, pie); iRet = MWRFATL.LastRet; if (iRet) { res = 2; $('#msg').text('验证密码失败！'); } else { res = 0; } } return res;";
            strJson += " }";
            return Content(strJson);
        }
        #endregion

        #region 表单验证 获取当前数据行的状态
        /// <summary>
        /// 获取当前数据行的状态
        /// </summary>
        /// <param name="KeyValue">主表ID</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetStatus(string KeyValue)
        {
            try
            {
                IDatabase database = DataFactory.Database();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    VBD_PCRAWCY model = database.FindEntity<VBD_PCRAWCY>(KeyValue);

                    if (model != null)
                    {
                        return Content(new JsonMessage { Success = true, Code = "1", Message = model.ToJson() }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "验证失败：未查询到数据" }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "验证失败：参数为空" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "验证失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 获取同一订单组批限制的状态
        public ActionResult GetZJZPStatus()
        {
            bool b = pcbll.GetZJZPStatus();
            return Content(new JsonMessage { Success = true, Code = b ? "1" : "0", Message = b ? "true" : "false" }.ToString());
        }
        #endregion
    }
}
