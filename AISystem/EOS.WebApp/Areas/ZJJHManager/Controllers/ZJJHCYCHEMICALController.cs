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

namespace EOS.WebApp.Areas.ZJJHManager.Controllers
{
    public class ZJJHCYCHEMICALController : PublicController<VQC_ZJJHRESULT>
    {
        VQC_ZJJHRESULTBLL VBLL = new VQC_ZJJHRESULTBLL();
        VQC_ZJJHRESULTDETAILSBLL VBLLITEM = new VQC_ZJJHRESULTDETAILSBLL();
        #region 页面返回
        public ActionResult ZJJHCYCHEMICALList()
        {
            return View();
        }
        public ActionResult ZJJHCYCHEForm()
        {
            return View();
        }

        public ActionResult ZJJHCYCHEMICALAdd()
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
                    VQC_ZJJHRESULT model = database.FindEntityByWhere<VQC_ZJJHRESULT>(" AND PRINTCODE='" + KeyValue + "' AND NOT EXISTS (SELECT 1 FROM QC_ZJJHRECEIVE B WHERE VQC_ZJJHRESULT.ID=B.ZJRESULTID AND B.RECEIVETYPE='CHE')  AND EXISTS (SELECT 1 FROM BASE_ZJFA C WHERE VQC_ZJJHRESULT.ZJFA=C.FANO AND NVL(C.ISHAVECHEMICAL,'0')='1') ");
                    if (model.ID != null)
                    {
                        List<VQC_ZJJHRESULTDETAILS> entitzjitem = new List<VQC_ZJJHRESULTDETAILS>();
                        ResultMessage message = VBLL.GetLoadCHEZJItem(model.ID);
                        if (message.Code == 1)
                        {
                            entitzjitem = message.Content as List<VQC_ZJJHRESULTDETAILS>;
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
            VQC_ZJJHRESULT model = database.FindEntityByWhere<VQC_ZJJHRESULT>(" AND ID='" + KeyValue + "'");
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
        public ActionResult GetJHCHEList(string PRINTCODE, string StartTime, string EndTime, string CHESTATUS, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJJHRESULT> ListData = VBLL.GetOrderListForCHE(PRINTCODE, StartTime, EndTime, CHESTATUS, jqgridparam);
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
        public ActionResult GetJHCHEZJItemList(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJJHRESULTDETAILS> ListData = VBLLITEM.GetJHCHEZJItemList(MAINID, jqgridparam);
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
        public ActionResult JHCHECheckZJBZ(string ZJITEMNO, string ZJFA, string ZJValue)
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
                        return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = (message.Content as List<VQC_ZJJHRESULTDETAILS>).ToJson() }.ToString());
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
        public ActionResult JHCHESave(string KeyValue, string JHCHEData, string CHEMemo)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(JHCHEData) && !string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.JHCHESave(KeyValue, JHCHEData, CHEMemo);
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
        public ActionResult JHCHESYSave(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.JHCHESYSave(KeyValue);
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
        public ActionResult JHCHEFinish(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.JHCHEFinish(KeyValue);
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
        public ActionResult JHCHEUNFinish(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = VBLL.JHCHEUNFinish(KeyValue);
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


        #endregion

    }
}
