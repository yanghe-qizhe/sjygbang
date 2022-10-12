using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Utilities;
using EOS.Entity;
using EOS.Business;
using System.Data;
using EOS.Business.ZJSettlement;

namespace EOS.WebApp.Areas.ReportManager.Controllers
{
    public class PrintViewReportController : PublicController<VWB_WEIGHTBS>
    {
        VWB_WEIGHTBSBll orderbll = new VWB_WEIGHTBSBll();
        VDP_PSORDERBll psOrderBll = new VDP_PSORDERBll();

        [LoginAuthorize]
        public ActionResult PrintViewReportIndex()
        {
            return View();
        }

        // 补打条码
        public ActionResult BDPrintCodeReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //补打过程条码
        public ActionResult BDGCPrintCodeReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult SelectMaterialType()
        {
            return View();
        }
        public ActionResult SelectMaterial()
        {
            return View();
        }
        public ActionResult SelectMaterialForZJ()
        {
            return View();
        }

        /// <summary>
        /// 车辆出入信息报表
        /// </summary>
        /// <returns></returns>
        public ActionResult CARSORDERReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }


        /// <summary>
        /// 司机信息校验报表报表
        /// </summary>
        /// <returns></returns>
        public ActionResult DRIVERSJYReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }


        //物料价格查询报表
        public ActionResult MaterialPriceReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult SupplyPriceReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult SourceAreaPriceReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        /// <summary>
        /// 验级员抽号记录报表
        /// </summary>
        /// <returns></returns>
        public ActionResult DRAWNUMBERHISTORYReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        #region 各报表页面
        //物流报表

        public ActionResult WLReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //结算报表

        public ActionResult JSReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //销售报表

        public ActionResult SalesReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //基础数据报表

        public ActionResult BaseSetReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //采购报表

        public ActionResult PurchaseReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        public ActionResult WeightReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        public ActionResult PurchaseReportIndex1()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult PurchaseReportIndex2()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult PurchaseReportIndexKM()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult PurchaseReportIndexMJ()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult PurchaseReportIndexJK()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult PurchaseReportIndexHD()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        
        public ActionResult YLReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult SalesReportIndexHD()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        
        public ActionResult PORECEIVEReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //其它入库报表
        public ActionResult OtherRKReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //其它出库报表
        public ActionResult OtherCKReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //小磅计量报表
        public ActionResult OtherDbReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //汽运倒运报表
        public ActionResult OtherDbReportIndex1()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //化工移库报表
        public ActionResult OtherDbReportIndex2()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        /// <summary>
        /// 化验包使用情况明细
        /// </summary>
        /// <returns></returns>
        public ActionResult OtherDbReportIndex3()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //铁水转序报表
        public ActionResult TSWeightReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //皮带称转序报表
        public ActionResult PDCWeightReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //汽运转序报表
        public ActionResult GXWeightReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //汽运转序报表
        public ActionResult GXWeightGPReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //工序计量核准
        public ActionResult GXWeightHZReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //工序点支根重
        public ActionResult GXWeightDZGZReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }


        //原料检测报表
        public ActionResult ZJReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //原料质检报告
        public ActionResult ZJResultReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //原料质检报告
        public ActionResult ZJResultCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //废钢质检报告
        public ActionResult ZJStellResultCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }


        //转序质检报表
        public ActionResult ZJZXReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }


        //转序质检报告
        public ActionResult ZJZXResultReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //转序质检报告
        public ActionResult ZJZXResultCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //煤气分析
        public ActionResult ZJEnvironmentGASReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //生产水分析
        [LoginAuthorize]
        public ActionResult ZJEnvironmentWaterSCReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //锅炉水分析
        public ActionResult ZJEnvironmentWaterGLReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //环境监测

        public ActionResult ZJEnvironmentReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //短流程质检报告
        public ActionResult ZJShortResultReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //短流程质检报告
        public ActionResult ZJShortResultCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //焦化质检报表

        public ActionResult ZJJHReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //焦化质检报告

        public ActionResult ZJJHResultReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //焦化质检报告

        public ActionResult ZJJHResultCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }


        //质检日报表 
        public ActionResult ZJDayReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //质检日报表
        public ActionResult ZJDayReportCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //质量日报告
        public ActionResult ZJDayMassReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //质量日报告
        public ActionResult ZJDayMassReportCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //质检生铁质检月报
        public ActionResult ZJMonthReportMoltenIron()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //质检生铁质检月报
        public ActionResult ZJMonthReportMoltenIronCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //质检机烧矿质量报表
        public ActionResult ZJMonthReportJSK()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //质检机烧矿质量报表
        public ActionResult ZJMonthReportJSKCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //质检质量统计表
        public ActionResult ZJMonthReportZLTJ()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //质检质量统计表
        public ActionResult ZJMonthReportZLTJCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //以质论价质检质量统计表
        public ActionResult ZJMonthReportYZLJ()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //以质论价质检质量统计表
        public ActionResult ZJMonthReportYZLJCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        //焦化部门质检报告 
        public ActionResult ZJDeptJHResultReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //炼钢部门质检报告 
        public ActionResult ZJDeptLGResultReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //炼铁部门质检报告 
        public ActionResult ZJDeptLTResultReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //熔剂部门质检报告 
        public ActionResult ZJDeptRJResultReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //部门质检报告
        public ActionResult ZJDeptResultCreateIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }


        //采购入场月报 
        public ActionResult PCMonthReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //转序月报 
        public ActionResult ZXMonthReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //产量月报 
        public ActionResult CLMonthReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //炼钢产量月报 
        public ActionResult LGCLMonthReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //轧钢产量月报 
        public ActionResult ZGCLMonthReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //轧钢转序月报 
        public ActionResult ZGZXMonthReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //焦化外销月报 
        public ActionResult JHWXMonthReport()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult DisplayViewReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        public ActionResult DisplayViewReportIndex1()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }
        //火运销售打印报表
        public ActionResult HYWeightOutReportIndex()
        {
            ViewBag.Company = ConfigHelper.AppSettings("Company");
            return View();
        }

        #endregion

        #region 产品标准报表
        /// <summary>
        /// 产品标准报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_CPBZReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_CPBZReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        #endregion


        #region 物流报表
        public ActionResult WLEntityReport(string Parm_Key_Value)
        {
            IList<VWB_WEIGHTBS> list = orderbll.GetCKEntityList(Parm_Key_Value);
            string str = XMLHelperToList<VWB_WEIGHTBS>.EntityToXml(list);

            return Content(str);
        }

        public ActionResult WLDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetWLDataSetList(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        public ActionResult WLDataSetReportA(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetWLDataSetListA(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        public ActionResult WLLoad(string BillNo, string Customer, string Material, string Cars, string Status, string StartTime, string EndTime)
        {
            IList<VWB_WEIGHTBS> list = orderbll.GetCKOrderList(BillNo, Customer, Material, Cars, Status, "3", "", StartTime, EndTime);
            string str = XMLHelperToList<VWB_WEIGHTBS>.EntityToXml(list);
            return Content(str);
        }

        #endregion

        #region 结算报表
        public ActionResult JSEntityReport(string Parm_Key_Value)
        {
            IList<VWB_WEIGHTBS> list = orderbll.GetCKEntityList(Parm_Key_Value);
            string str = XMLHelperToList<VWB_WEIGHTBS>.EntityToXml(list);
            return Content(str);
        }

        public ActionResult JSDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = psOrderBll.GetDataSetList(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        public ActionResult JSLoad(string BillNo, string Customer, string Material, string Cars, string Status, string StartTime, string EndTime)
        {
            IList<VWB_WEIGHTBS> list = orderbll.GetCKOrderList(BillNo, Customer, Material, Cars, Status, "3", "", StartTime, EndTime);
            string str = XMLHelperToList<VWB_WEIGHTBS>.EntityToXml(list);
            return Content(str);
        }

        #endregion

        #region 销售报表
        public ActionResult SalesEntityReport(string Parm_Key_Value)
        {
            IList<VWB_WEIGHTBS> list = orderbll.GetCKEntityList(Parm_Key_Value);
            string str = XMLHelperToList<VWB_WEIGHTBS>.EntityToXml(list);
            //Response.Write(str);
            //Response.End();
            return Content(str);
        }

        public ActionResult SalesDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetCKDataSetList(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        public ActionResult SalesLoad(string BillNo, string Customer, string Material, string Cars, string Status, string StartTime, string EndTime)
        {
            IList<VWB_WEIGHTBS> list = orderbll.GetCKOrderList(BillNo, Customer, Material, Cars, Status, "3", "", StartTime, EndTime);
            string str = XMLHelperToList<VWB_WEIGHTBS>.EntityToXml(list);
            return Content(str);
        }

        #endregion

        #region 连铸工序
        /// <summary>
        /// 铸坯存放记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGXReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGXReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 逐批性能查询
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZPXNReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.ZPXNReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 逐批质量报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZPZLReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.ZPZLReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 逐批质量汇总报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZPZLSUMReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.ZPZLSUMReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 重量偏差记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZLPCReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.ZLPCReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }



        /// <summary>
        /// 生产轧废报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult BHZFReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.BHZFReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 外形尺寸记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GJCCReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GJCCReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 力学检测报告
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LXReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LXReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 钢材质检日报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GCZJDayReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GCZJDayReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 炼钢连铸坯不合格报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_LGLZPBHGReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_LGLZPBHGReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 钢材不合格品报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GCBHGReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_GCBHGReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 钢材送样记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GCSYReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_GCSYReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 逐炉中包化验记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_ZLHYReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_ZLHYReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 钢坯冷检检验记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GPLJReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_ZLHYReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }



        /// <summary>
        /// 钢材检验报告单
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GCCheckReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_GCCheckReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 钢坯挑废记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GPTFReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_GPTFReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 钢坯回炉记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GPHLReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_GPHLReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 钢坯出炉记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GPCLReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_GPCLReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 钢坯入炉记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GPRLReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_GPRLReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 钢坯收坯记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GPSPReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_GPSPReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 钢坯发坯记录
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GPFPReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_GPSPReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 炼钢连铸坯非计划
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_GPUNPLANReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.LZGX_GPUNPLANReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 按炉送钢卡片
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_ALKPReport(string Parm_Key_Value, string Type)
        {
            DataSet dataSet = orderbll.LZGX_ALKPReport(Parm_Key_Value, Type);
            return Content(dataSet.GetXml());
        }
        /// <summary>
        /// 按批送钢卡片
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult LZGX_APKPReport(string Parm_Key_Value, string Type)
        {
            DataSet dataSet = orderbll.LZGX_APKPReport(Parm_Key_Value, Type);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 打印火运钢材磅单
        /// </summary>
        /// <param name="Parm_Key_Value"></param>

        /// <returns></returns>
        public ActionResult HYWeightOutReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.HYWeightOutReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        #endregion

        #region 其它业务报表


        public ActionResult OtherRKDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetQTRKDataSetList(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        public ActionResult OtherCKDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetQTCKDataSetList(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        #endregion


        #region 基础数据报表数据源

        public ActionResult BaseSetDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.BaseSetDataSetReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 车辆派车单报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult CarsOrderDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.CarsOrderDataSetReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        #endregion

        #region 铁水转序报表


        public ActionResult OtherDbDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.OtherDbDataSetList(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        public ActionResult OtherDbDataSetReport3(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.OtherDbDataSetList3(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        public ActionResult OtherTSDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetTSDataSetList(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }



        #endregion


        #region 皮带称报表
        public ActionResult OtherPDCDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetPDCDataSetList(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        #endregion

        #region 工序转序报表


        public ActionResult GXWeightDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetGXDataSetList(Parm_Key_Value);
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, false);
            }
            return View();
        }

        #endregion

        #region 采购报表
        public ActionResult PurchaseEntityReport(string Parm_Key_Value)
        {
            IList<VWB_WEIGHTBS> list = orderbll.GetRKEntityList(Parm_Key_Value);
            string str = XMLHelperToList<VWB_WEIGHTBS>.EntityToXml(list);
            return Content(str);
        }

        public ActionResult PurchaseDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetRKDataSetList(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
       
        public ActionResult PurchaseDataSetReport1(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetRKDataSetList1(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        public ActionResult PurchaseLoad(string BillNo, string Supply, string Material, string Cars, string Status, string StartTime, string EndTime)
        {
            IList<VWB_WEIGHTBS> list = orderbll.GetRKOrderList(BillNo, Supply, Material, Cars, Status, "0", StartTime, EndTime);
            string str = XMLHelperToList<VWB_WEIGHTBS>.EntityToXml(list);
            return Content(str);
        }


        #endregion


        #region 采购收料

        public ActionResult PORECEIVEDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetRKSLDataSetList(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        #endregion

        #region 原料质检报表
        /// <summary>
        /// 原料检测报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZJReportList(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJReportList(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }

        /// <summary>
        /// 原料组批报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZJZPReportList(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJZPReportList(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }

        /// <summary>
        /// 原料检验部-过磅单合格率统计
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZJGBDHGLReportList(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJGBDHGLReportList(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true);
            return null;
        }

        /// <summary>
        /// 原料质检报告
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        //public ActionResult ZJResultReportList(string Parm_Key_Value)
        //{
        //    DataSet dataSet = orderbll.GetZJZPReportList(Parm_Key_Value);
        //    XMLReportData.GenDetailData(this.Response, dataSet, true);
        //    return null;
        //}

        /// <summary>
        /// 获取质检项目列表
        /// </summary>
        /// <param name="Material"></param>
        /// <returns></returns>
        public ActionResult GetZJCloum(string Parm_Key_Value)
        {
            ResultMessage message = orderbll.GetZJCloum(Parm_Key_Value);
            return Content(message.ToString());
        }

        /// <summary>
        /// 获取质检结果
        /// </summary>
        /// <returns></returns>
        public ActionResult GetZJResultData(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJResultData(Parm_Key_Value);
            return Content(dataSet.ToJson());
        }

        /// <summary>
        /// 获取废钢质检结果
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetZJStellResultData(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJStellResultData(Parm_Key_Value);
            return Content(dataSet.ToJson());
        }
        #endregion

        #region 转序质检报表
        /// <summary>
        /// 转序检测报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZJZXReportList(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJZXReportList(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }
        /// <summary>
        /// 转序组批报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZJZXZPReportList(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJZXZPReportList(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }

        /// <summary>
        /// 获取质检结果
        /// </summary>
        /// <returns></returns>
        public ActionResult GetZJZXResultData(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJZXResultData(Parm_Key_Value);
            return Content(dataSet.ToJson());
        }

        /// <summary>
        /// 获取质检项目列表
        /// </summary>
        /// <param name="Material"></param>
        /// <returns></returns>
        public ActionResult GetZJZXCloum(string Parm_Key_Value)
        {
            ResultMessage message = orderbll.GetZJZXCloum(Parm_Key_Value);
            return Content(message.ToString());
        }
        #endregion

        #region 短流程
        /// <summary>
        /// 短流程 质检报告
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetZJShortResultData(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJShortResultData(Parm_Key_Value);
            return Content(dataSet.ToJson());
        }
        /// <summary>
        /// 获取短流程质检项目列表
        /// </summary>
        /// <param name="Material"></param>
        /// <returns></returns>
        public ActionResult GetZJShortCloum(string Parm_Key_Value)
        {
            ResultMessage message = orderbll.GetZJShortCloum(Parm_Key_Value);
            return Content(message.ToString());
        }
        #endregion

        #region 质检焦化报表
        /// <summary>
        /// 焦化检测报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZJJHReportList(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJJHReportList(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }

        /// <summary>
        /// 焦化组批报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZJJHZPReportList(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJJHZPReportList(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }

        /// <summary>
        /// 获取质检焦化项目列表
        /// </summary>
        /// <param name="Material"></param>
        /// <returns></returns>
        public ActionResult GetZJJHCloum(string Parm_Key_Value)
        {
            ResultMessage message = orderbll.GetZJJHCloum(Parm_Key_Value);
            return Content(message.ToString());
        }

        /// <summary>
        /// 获取质检焦化结果
        /// </summary>
        /// <returns></returns>
        public ActionResult GetZJJHResultData(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJJHResultData(Parm_Key_Value);
            return Content(dataSet.ToJson());
        }


        #endregion

        #region 能源动力分析
        /// <summary>
        /// 煤气分析
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZJEnvironmentGASReportList(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.ZJEnvironmentGASReportList(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }

        /// <summary>
        /// 水分析
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZJEnvironmentWaterReportList(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.ZJEnvironmentWaterReportList(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }

        /// <summary>
        /// 环境监测
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult ZJEnvironmentReportList(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.ZJEnvironmentReportList(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }
        #endregion

        #region 质检日报表
        /// <summary>
        /// 质检日报表
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult GetZJDayResultData(string ID, string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJDayResultData(ID, Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }
        /// <summary>
        /// 获取日报表动态化验项目列
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetZJDayResultDataColumn(string Parm_Key_Value)
        {
            ResultMessage message = orderbll.GetZJDayResultDataColumn(Parm_Key_Value);
            return Content(message.ToString());
        }
        #endregion

        #region 质量日报告
        /// <summary>
        /// 质量日报告
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public ActionResult GetZJDayMassResultData(string ID, string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJDayMassResultData(ID, Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }
        /// <summary>
        /// 获取日报告动态化验项目列
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetZJDayMassResultDataColumn(string Parm_Key_Value)
        {
            ResultMessage message = orderbll.GetZJDayMassResultDataColumn(Parm_Key_Value);
            return Content(message.ToString());
        }

        #endregion

        #region 质检部门报告
        /// <summary>
        /// 获取质检部门项目列表
        /// </summary>
        /// <param name="Material"></param>
        /// <returns></returns>
        public ActionResult GetZJDeptCloum(string Parm_Key_Value)
        {
            ResultMessage message = orderbll.GetZJDeptCloum(Parm_Key_Value);
            return Content(message.ToString());
        }

        /// <summary>
        /// 获取质检部门报告结果
        /// </summary>
        /// <returns></returns>
        public ActionResult GetZJDeptResultData(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJDeptResultData(Parm_Key_Value);
            return Content(dataSet.ToJson());
        }

        #endregion

        #region 原料质检仲裁报告
        /// <summary>
        /// 打印原料仲裁报告
        /// </summary>
        /// <param name="ZCBillNo"></param>
        /// <returns></returns>
        public ActionResult GetZJPCRawZCReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJPCRawZCReport(Parm_Key_Value);
            XMLReportData.GenDetailData(this.Response, dataSet, true); return null;
        }
        #endregion

        #region 月报
        /// <summary>
        /// 采购月报
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetPCMonthReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetPCMonthReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 转序月报
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetZXMonthReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZXMonthReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 轧钢转序月报
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetZGZXMonthReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZGZXMonthReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }



        /// <summary>
        /// 产量月报
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetCLMonthReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetCLMonthReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 炼钢产量月报
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetLGCLMonthReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetLGCLMonthReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 轧钢产量月报
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetZGCLMonthReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZGCLMonthReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }

        /// <summary>
        /// 焦化外销月报
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetJHWXMonthReport(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetJHWXMonthReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
        #endregion

        #region 质检月报
        /// <summary>
        /// 生铁检验报告
        /// </summary>
        /// <returns></returns>
        public ActionResult GetZJMonthReportMoltenIron(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJMonthReportMoltenIron(Parm_Key_Value);
            return Content(dataSet.ToJson());
        }
        #endregion

        #region 质量统计报告
        /// <summary>
        /// 质量统计表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns>
        public ActionResult GetZJMonthReportZLTJDataColumn(string Parm_Key_Value)
        {
            ResultMessage message = orderbll.GetZJMonthReportZLTJDataColumn(Parm_Key_Value);
            return Content(message.ToString());
        }
        /// <summary>
        /// 质量统计表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns> 
        public ActionResult GetZJMonthReportZLTJData(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJMonthReportZLTJData(Parm_Key_Value);
            return Content(dataSet.ToJson());
        }
        #endregion

        #region 以质论价质量统计表
        /// <summary>
        /// 以质论价质量统计表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns> 
        public ActionResult GetZJMonthReportYZLJData(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJMonthReportYZLJData(Parm_Key_Value);
            return Content(dataSet.ToJson());
        }
        #endregion

        #region 机烧矿质量报表
        /// <summary>
        /// 机烧矿质量报表
        /// </summary>
        /// <param name="Parm_Key_Value"></param>
        /// <returns></returns> 
        public ActionResult GetZJMonthReportJSKData(string Parm_Key_Value)
        {
            DataSet dataSet = orderbll.GetZJMonthReportJSKData(Parm_Key_Value);
            return Content(dataSet.ToJson());
        }
        #endregion
    }
}
