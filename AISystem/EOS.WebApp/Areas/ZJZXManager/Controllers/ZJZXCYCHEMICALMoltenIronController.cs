using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using EOS.Entity;
using EOS.DataAccess;
using EOS.Repository;
using EOS.Business.ZJZXManager;

namespace EOS.WebApp.Areas.ZJZXManager.Controllers
{
    public class ZJZXCYCHEMICALMoltenIronController : PublicController<VQC_ZJZXRESULT>
    {
        VQC_ZJZXRESULTBLL VBLL = new VQC_ZJZXRESULTBLL();
        VQC_ZJZXRESULTDETAILSBLL VBLLITEM = new VQC_ZJZXRESULTDETAILSBLL();
        VQC_ZJZXCYBll vqcbll = new VQC_ZJZXCYBll();
        SMX14BLL smx = new SMX14BLL();
        #region 页面返回
        public ActionResult ZJZXCYCHEMICALMoltenIronList()
        {
            return View();
        }
        public ActionResult ZJZXCYCHEMICALMoltenIronAdd()
        {
            return View();
        }
        public ActionResult ZJZXCYCHEMICALMoltenIronForm()
        {
            return View();
        }
        public ActionResult ZJZXCYCHEMICALMoltenIronFormUpdate()
        {
            return View();
        }
        #endregion

        #region 数据列表

        /// <summary>
        /// 获取转序组批化学检测列表
        /// </summary>
        /// <param name="PRINTCODE"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZXCHEMoltenList(string PRINTCODE, string StartTime, string EndTime, string CHESTATUS, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                //铁水
                List<VQC_ZJZXRESULT> ListData = VBLL.GetOrderListForCHE(" PCRAWTYPE='2' AND NVL(DEF10,'0')='0' ", PRINTCODE, StartTime, EndTime, CHESTATUS, jqgridparam);
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
        /// 铁水移除样记录列表
        /// </summary>
        /// <param name="BILLNO"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJZXCYMoltenDeleteLog(string BILLNO, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                //铁水
                List<VQC_ZJZXCY> ListData = vqcbll.GetZJZXCYMoltenDeleteLog(BILLNO, StartTime, EndTime, jqgridparam);
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


        #region 表单操作
        /// <summary>
        /// 移除样
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJZXCYDELETEMolten(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.ZJZXCYDELETEMolten(KeyValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中铁水样" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "判断失败," + ex.Message.ToString() }.ToString());
            }
        }

        /// <summary>
        /// 恢复铁水样
        /// </summary>
        /// <param name="KeyValue">铁水样的批次ID</param>
        /// <param name="BillNO"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJZXCYCHEMICALMoltenIronAddBillNo(string KeyValue, string BillNO)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.ZJZXCYCHEMICALMoltenIronAddBillNo(KeyValue, BillNO);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中铁水样" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "判断失败," + ex.Message.ToString() }.ToString());
            }
        }

        /// <summary>
        /// 获取铁水质检数据 采集
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJCJ(string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            ResultMessage message = new ResultMessage();
            DC_CRN_JZHL_CCM_INFOBll bll = new DC_CRN_JZHL_CCM_INFOBll();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    QC_ZJZXCY model = database.FindEntity<QC_ZJZXCY>(KeyValue);
                    DC_CRN_JZHL_SMX14_INFO m = new DC_CRN_JZHL_SMX14_INFO();
                    //string Code = model.DEF5 + model.DEF6;
                    string Code = model.DEF5 + "-" + model.DEF6.Substring(model.DEF6.Length - 1, 1);
                    message = bll.GetZJCJ(Code);
                    if (message.Code == 1)
                    {
                        m = (DC_CRN_JZHL_SMX14_INFO)message.Content;
                        string strsql = "SELECT * FROM QC_ZJZXRESULTDETAILS WHERE CYID='" + KeyValue + "'";

                        database = DataFactory.Database();
                        List<QC_ZJZXRESULTDETAILS> q = database.FindListBySql<QC_ZJZXRESULTDETAILS>(strsql);

                        #region 匹配质检项

                        ConvertHY.ConvertHYMain(ref q, m, model);//铁水

                        //ConvertHY.ConvertHYVal(ref q, "ZJ000221", m.Si);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000222", m.P);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000223", m.S);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000220", m.Mn);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000320", m.Ti);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000226", m.Cr);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000227", m.Ni);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000228", m.Cu);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000291", m.TiO2);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000308", m.Zn);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000307", m.MnO);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000290", m.SiO2);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000305", m.Al2O3);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000304", m.MgO);
                        //ConvertHY.ConvertHYVal(ref q, "ZJ000303", m.CaO);

                        //q.Find(a => a.ZJITEM == "ZJ000221").ZJVALUE = m.Si;
                        //q.Find(a => a.ZJITEM == "ZJ000222").ZJVALUE = m.P;
                        //q.Find(a => a.ZJITEM == "ZJ000223").ZJVALUE = m.S;
                        //q.Find(a => a.ZJITEM == "ZJ000220").ZJVALUE = m.Mn;
                        //q.Find(a => a.ZJITEM == "ZJ000320").ZJVALUE = m.Ti;
                        //q.Find(a => a.ZJITEM == "ZJ000226").ZJVALUE = m.Cr;
                        //q.Find(a => a.ZJITEM == "ZJ000227").ZJVALUE = m.Ni;
                        //q.Find(a => a.ZJITEM == "ZJ000228").ZJVALUE = m.Cu;
                        #endregion

                        message.Code = 1;
                        message.Content = q.ToJson();
                    }
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中铁水样" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "判断失败," + ex.Message.ToString() }.ToString());
            }
        }

        #endregion
    }
}
