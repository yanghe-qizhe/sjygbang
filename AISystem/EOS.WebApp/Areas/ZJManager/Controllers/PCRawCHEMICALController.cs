using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using EOS.DataAccess;
using EOS.Repository;

namespace EOS.WebApp.Areas.ZJManager.Controllers
{
    public class PCRawCHEMICALController : PublicController<BD_PCRAWCY>
    {
        BD_PCRAWCYBll pcbll = new BD_PCRAWCYBll();
        VBD_PCRAWCYBll vpcbll = new VBD_PCRAWCYBll();

        #region 页面返回
        public ActionResult PCRawCHEMICALList()
        {
            return View();
        }
        public ActionResult PCRawCHEMICALForm()
        {
            return View();
        }
        public ActionResult PCRawCHEMICALAdd()
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
                    VBD_ZJRESULT model = database.FindEntityByWhere<VBD_ZJRESULT>(" AND PRINTCODE='" + KeyValue + "' AND NOT EXISTS (SELECT 1 FROM BD_ZJRECEIVE B WHERE VBD_ZJRESULT.ID=B.ZJRESULTID AND B.RECEIVETYPE='CHE')  AND EXISTS (SELECT 1 FROM BASE_ZJFA C WHERE VBD_ZJRESULT.ZJFA=C.FANO AND NVL(C.ISHAVECHEMICAL,'0')='1') ");
                    if (model.ID != null)
                    {
                        List<VBD_ZJRESULTDETAILS> entitzjitem = new List<VBD_ZJRESULTDETAILS>();

                        ResultMessage message = vpcbll.GetLoadCHEZJItem(model.ID);
                        if (message.Code == 1)
                        {
                            entitzjitem = message.Content as List<VBD_ZJRESULTDETAILS>;
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
            VBD_ZJRESULT model = database.FindEntityByWhere<VBD_ZJRESULT>(" AND ID='" + KeyValue + "'");

            //List<VBD_PCRAWCYDETAILS> entitydetails = DataFactory.Database().FindList<VBD_PCRAWCYDETAILS>("MAINID", CYID);
            //return Content("{\"M\":[" + model.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
            return Content("{\"M\":[" + model.ToJson() + "]}");
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
        public ActionResult GetOrderList(string PRINTCODE, string CHESTATUS, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJRESULT> ListData = vpcbll.GetOrderList(PRINTCODE, CHESTATUS, StartTime, EndTime, jqgridparam);
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
        public ActionResult GetOrderEntryList(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_ZJRESULTDETAILS> ListData = vpcbll.GetOrderEntryList(MAINID, jqgridparam);
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
        public ActionResult PCRawCHECheckZJBZ(string ZJITEMNO, string ZJFA, string ZJValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(ZJITEMNO))
                {
                    message = vpcbll.CheckZJBZForPHY("CHE", ZJITEMNO, ZJFA, ZJValue);
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
                    message = vpcbll.GetLoadCHEZJItem(KeyValue);
                    if (message.Code == 1)
                    {
                        return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = (message.Content as List<VBD_ZJRESULTDETAILS>).ToJson() }.ToString());
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
        /// 化学检测结果提交
        /// </summary>
        /// <param name="PCRAWCHEData">质检采样批次主键</param>
        /// <param name="ZJRESULTID">质检结果主表ID</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCHESave(string KeyValue, string PCRAWCHEData, string CHEMemo)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(PCRAWCHEData) && !string.IsNullOrEmpty(KeyValue))
                {
                    message = pcbll.PCRAWCHESave(KeyValue, PCRAWCHEData, CHEMemo);
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
        public ActionResult PCRawCHESYSave(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = pcbll.PCRAWCHESYSave(KeyValue);
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
        /// 化学检测结果 完成操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawCHEFinish(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = pcbll.PCRawCHEFinish(KeyValue);
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
        public ActionResult PCRawCHEUNFinish(string KeyValue)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = pcbll.PCRawCHEUNFinish(KeyValue);
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
