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
    public class ZJZXCYCHEMICALController : PublicController<VQC_ZJZXRESULT>
    {
        VQC_ZJZXRESULTBLL VBLL = new VQC_ZJZXRESULTBLL();
        VQC_ZJZXRESULTDETAILSBLL VBLLITEM = new VQC_ZJZXRESULTDETAILSBLL();
        #region 页面返回
        public ActionResult ZJZXCYCHEMICALList()
        {
            return View();
        }
        public ActionResult ZJZXCYCHEForm()
        {
            return View();
        }
        public ActionResult ZJZXCYCHEFormUpdate()
        {
            return View();
        }
        public ActionResult ZJZXCYCHEMICALAdd()
        {
            return View();
        }

        /// <summary>
        /// 获取化学质检数据、采样组批明细数据 条码
        /// </summary>
        /// <param name="KeyValue">条码</param>
        /// <returns></returns>
        public ActionResult SetFormCode(string KeyValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IDatabase database = DataFactory.Database();
                    VQC_ZJZXRESULT model = database.FindEntityByWhere<VQC_ZJZXRESULT>(" AND PRINTCODE='" + KeyValue + "' AND NOT EXISTS (SELECT 1 FROM QC_ZJZXRECEIVE B WHERE VQC_ZJZXRESULT.ID=B.ZJRESULTID AND B.RECEIVETYPE='CHE')  AND EXISTS (SELECT 1 FROM BASE_ZJFA C WHERE VQC_ZJZXRESULT.ZJFA=C.FANO AND NVL(C.ISHAVECHEMICAL,'0')='1') ");
                    if (model.ID != null)
                    {
                        List<VQC_ZJZXRESULTDETAILS> entitzjitem = new List<VQC_ZJZXRESULTDETAILS>();
                        ResultMessage message = VBLL.GetLoadCHEZJItem(model.ID);
                        if (message.Code == 1)
                        {
                            entitzjitem = message.Content as List<VQC_ZJZXRESULTDETAILS>;
                            string returnjson = "{\"M\":[" + model.ToJson() + "],\"D\":" + entitzjitem.ToJson() + "}";
                            return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = returnjson }.ToString());
                        }
                        else
                        {
                            return Content(new JsonMessage { Success = false, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                        }
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "请检查条码是否正确,或该批次已收样,或不包含化学检测项" }.ToString());
                    }
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未读取到条码号,请稍后重试" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 获取物理质检数据、采样组批明细数据 
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult SetFormMain(string KeyValue, string CYID)
        {
            IDatabase database = DataFactory.Database();
            VQC_ZJZXRESULT model = database.FindEntityByWhere<VQC_ZJZXRESULT>(" AND ID='" + KeyValue + "'");
            return Content("{\"M\":[" + model.ToJson() + "]}");
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
        public ActionResult GetZXCHEList(string PRINTCODE, string StartTime, string EndTime, string CHESTATUS, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXRESULT> ListData = VBLL.GetOrderListForCHE(" PCRAWTYPE<>'2' ", PRINTCODE, StartTime, EndTime, CHESTATUS, jqgridparam);
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
        /// 获取物理质检结果明细表信息
        /// </summary>
        /// <param name="MAINID"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZXCHEZJItemList(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXRESULTDETAILS> ListData = VBLLITEM.GetZXCHEZJItemList(MAINID, jqgridparam);
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
        /// 判断当前质检值是否合格
        /// </summary>
        /// <param name="ZJITEMNO"></param>
        /// <param name="KeyValue">ZJRESULTID</param>
        /// <param name="ZJValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXCHECheckZJBZ(string ZJITEMNO, string ZJFA, string ZJValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(ZJITEMNO))
                {
                    message = VBLL.CheckZJBZForCHE("CHE", ZJITEMNO, ZJFA, ZJValue);
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

        /// <summary>
        /// 加载化学质检项目
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult LoadCHEZJItem(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.GetLoadCHEZJItem(KeyValue);
                    if (message.Code == 1)
                    {
                        return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = (message.Content as List<VQC_ZJZXRESULTDETAILS>).ToJson() }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = false, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                    }
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
        /// 物理检测结果提交
        /// </summary>
        /// <param name="KeyValue">质检结果主表ID</param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXCHESave(string KeyValue, string ZXCHEData, string CHEMemo, string SaveType)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(ZXCHEData) && !string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.ZXCHESave(KeyValue, ZXCHEData, CHEMemo, SaveType);
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
        /// 化学检测收样记录
        /// </summary>
        /// <param name="KeyValue">条码号</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXCHESYSave(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.ZXCHESYSave(KeyValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "条码号不能为空" }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 物理检测结果 完成操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXCHEFinish(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.ZXCHEFinish(KeyValue);
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
        /// 取消 物理检测结果 完成操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZXCHEUNFinish(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.ZXCHEUNFinish(KeyValue);
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
        /// 获取 烧结矿/高炉渣/转炉渣 质检数据 采集
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
                    string Code = model.BILLNO;
                    message = bll.GetZJCJ(Code);
                    if (message.Code == 1)
                    {
                        m = (DC_CRN_JZHL_SMX14_INFO)message.Content;
                        string strsql = "SELECT * FROM QC_ZJZXRESULTDETAILS WHERE CYID='" + KeyValue + "'";

                        database = DataFactory.Database();
                        List<QC_ZJZXRESULTDETAILS> q = database.FindListBySql<QC_ZJZXRESULTDETAILS>(strsql);

                        #region 匹配质检项

                        ConvertHY.ConvertHYMain(ref q, m, model);

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
