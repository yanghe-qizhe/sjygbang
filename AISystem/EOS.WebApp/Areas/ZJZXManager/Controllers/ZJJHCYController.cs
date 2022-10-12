using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using EOS.Utilities;
using System.Diagnostics;
using EOS.Business;
using EOS.DataAccess;
using EOS.Repository;

namespace EOS.WebApp.Areas.ZJJHManager.Controllers
{
    public class ZJJHCYController : PublicController<QC_ZJJHCY>
    {
        QC_ZJJHCYBLL bll = new QC_ZJJHCYBLL();
        VQC_ZJJHCYBLL vbll = new VQC_ZJJHCYBLL();
        VQC_ZJJHCYDETAILSBLL vbllitem = new VQC_ZJJHCYDETAILSBLL();
        VZJJHQYBLL vzjjhbll = new VZJJHQYBLL();

        #region 页面返回
        public ActionResult ZJJHCYAdd()
        {
            return View();
        }
        public ActionResult ZJJHCYList()
        {
            return View();
        }
        public ActionResult ZJJHCYSelectWeight()
        {
            ViewBag.FormID = "ZJJHCYSelectWeight";
            return View();
        }
        public ActionResult ZJJHCYSelectFHD()
        {
            ViewBag.FormID = "ZJJHCYSelectFHD";
            return View();
        }
        public ActionResult ZJJHCYSelectDHD()
        {
            ViewBag.FormID = "ZJJHCYSelectDHD";
            return View();
        }
        public ActionResult ZJJHCYAddCar()
        {
            return View();
        }
        public ActionResult ZJJHCYDeleteCar()
        {
            return View();
        }
        public ActionResult ZJJHCYSelectType()
        {
            return View();
        }
        /// <summary>
        /// 重写 采样组批 编辑页面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public override ActionResult SetForm(string KeyValue)
        {
            VQC_ZJJHCY entity = DataFactory.Database().FindEntity<VQC_ZJJHCY>(KeyValue);
            List<VQC_ZJJHCYDETAILS> entitydetails = DataFactory.Database().FindList<VQC_ZJJHCYDETAILS>("MAINID", KeyValue, "CRETIME", "ASC");
            return Content("{\"M\":[" + entity.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
        }
        #endregion

        #region 旧的选单方式  选过磅单
        /// <summary>
        /// 焦化汽运过磅单
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="MATERIAL"></param>
        /// <param name="SUPPLY"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetJHWeightList(string StartTime, string EndTime, string MATERIAL, string RECEIVE, string MATERIALNAME, string RECEIVENAME, string CARS, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJJHQY> ListData = vzjjhbll.GetJHWeightList(StartTime, EndTime, MATERIAL, RECEIVE, MATERIALNAME, RECEIVENAME, CARS, jqgridparam);
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
        /// 通过过磅单号获取磅单信息
        /// </summary>
        /// <param name="KeyValue">PK_ORDER</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCGetQYData(string KeyValue)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    return Content(vzjjhbll.PCGetQYData(KeyValue));
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
        /// 移入组批明细 查询通知单列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPCJHCYAddCarList(string KeyValue, string StartTime, string EndTime, string CARNAME, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJJHQY> ListData = vzjjhbll.GetPCJHCYAddCarList(KeyValue, StartTime, EndTime, CARNAME, jqgridparam);
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
        public ActionResult GetList(string BILLNO, string StartTime, string EndTime, string MATERIALNAME, string RECEIVENAME, string CARNAME, string CHECKTYPE, string CHECKISSEND, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJJHCY> ListData = vbll.GetList(BILLNO, StartTime, EndTime, MATERIALNAME, RECEIVENAME, CARNAME, CHECKTYPE, CHECKISSEND, jqgridparam);
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
        /// 获取采样车辆明细信息
        /// </summary>
        /// <param name="MAINID"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetListItem(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJJHCYDETAILSWEIGHT> ListData = vbllitem.GetListItem(MAINID, jqgridparam);
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
        /// 焦化汽运发货单
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="MATERIAL"></param>
        /// <param name="RECEIVE"></param>
        /// <param name="MATERIALNAME"></param>
        /// <param name="RECEIVENAME"></param>
        /// <param name="CARS"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetJHFHDList(string StartTime, string EndTime, string MATERIAL, string RECEIVE, string MATERIALNAME, string RECEIVENAME, string CARS, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJJHQY_NEW> ListData = vzjjhbll.GetJHFHDList(StartTime, EndTime, MATERIAL, RECEIVE, MATERIALNAME, RECEIVENAME, CARS, jqgridparam);
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
        /// 焦化汽运到货单
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="MATERIAL"></param>
        /// <param name="RECEIVE"></param>
        /// <param name="MATERIALNAME"></param>
        /// <param name="RECEIVENAME"></param>
        /// <param name="CARS"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetJHDHDList(string StartTime, string EndTime, string MATERIAL, string RECEIVE, string MATERIALNAME, string RECEIVENAME, string CARS, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJQY> ListData = vzjjhbll.GetJHDHDList(StartTime, EndTime, MATERIAL, RECEIVE, MATERIALNAME, RECEIVENAME, CARS, jqgridparam);
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
        /// 通过发货单号获取发货单信息
        /// </summary>
        /// <param name="KeyValue">PK_ORDER</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCGetQYFHDData(string KeyValue)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    return Content(vzjjhbll.PCGetQYFHDData(KeyValue));
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
        /// 通过发到单号获取发货单信息
        /// </summary>
        /// <param name="KeyValue">PK_ORDER</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCGetQYDHDData(string KeyValue)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    return Content(vzjjhbll.PCGetQYDHDData(KeyValue));
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
        /// 移入组批明细 查询发货单列表  或 收货单列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <param name="DEF6">CG  XS </param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPCJHCYAddCarListFHD(string KeyValue, string StartTime, string EndTime, string CARNAME, string DEF6, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJJHQY_NEW> ListData_VZJJHQY_NEW = new List<VZJJHQY_NEW>();
                List<VZJQYNOFINISH> ListData_VZJQYNOFINISH = new List<VZJQYNOFINISH>();
                if (DEF6 == "XS")
                {
                    ListData_VZJJHQY_NEW = vzjjhbll.GetPCJHCYAddCarListFHD(KeyValue, StartTime, EndTime, CARNAME, jqgridparam);
                }
                else if (DEF6 == "CG")
                {
                    ListData_VZJQYNOFINISH = vzjjhbll.GetPCJHCYAddCarListDHD(KeyValue, StartTime, EndTime, CARNAME, jqgridparam);
                }
                object o = DEF6 == "XS" ? (object)ListData_VZJJHQY_NEW : (object)ListData_VZJQYNOFINISH;
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = o
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
        /// 移除组批车辆明细列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetPCJHCYDeleteCarList(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJJHCYDETAILSWEIGHT> ListData = vbllitem.GetListItem(KeyValue, jqgridparam);
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
        /// 删除数据  
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual ActionResult DeleteItem(string KeyValue)
        {
            try
            {
                ResultMessage message = bll.PCJHCYDelete(KeyValue);

                WriteLog(message.Code, KeyValue, message.Message);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// 提交表单 页面的完成 
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJJHCYFinishNew(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.PCJHCYFinishNew(KeyValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中批号,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 取消完成
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJJHCYUNFinish(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.PCJHCYUNFinish(KeyValue);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中批号,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
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
        public ActionResult PCJHCYDeleteItem(string KeyValue, string KeyList)
        {
            try
            {
                ResultMessage message = bll.PCJHCYDeleteItem(KeyValue, KeyList);

                WriteLog(message.Code, KeyValue, message.Message);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
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
        public ActionResult ZJJHCYSAVE(QC_ZJJHCY entity, string KeyValue, string ZJJHCYDetailJson)
        {
            int IsOk = 0;
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IsOk = bll.ZJJHCYEditSave(entity, KeyValue, ZJJHCYDetailJson);
                    if (IsOk == 2)
                    {
                        return Content(new JsonMessage { Success = false, Code = "0", Message = "当前批号已存在,请重新输入批号" }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
                    }
                }
                else
                {

                    IsOk = bll.ZJJHCYSave(entity, ZJJHCYDetailJson);
                    if (IsOk == 2)
                    {
                        return Content(new JsonMessage { Success = false, Code = "0", Message = "当前批号已存在,请重新输入批号" }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 移入 通知单数据移入到指定批次
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="BILLNOLIST"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCJHCYAddCarItemFHD(string KeyValue, string BILLNOLIST, string DEF6)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    if (DEF6 == "XS")
                    {
                        message = vzjjhbll.PCJHCYAddCarItemFHD(KeyValue, BILLNOLIST);
                    }
                    else if (DEF6 == "CG")
                    {
                        message = vzjjhbll.PCJHCYAddCarItemDHD(KeyValue, BILLNOLIST);
                    }
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中批号,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }


        /// <summary>
        /// 保存 批次的 销售、采购 状态   DEF6字段
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult ZJJHSetTypeDEF6(string KeyValue, string Type)
        { 
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    ResultMessage message = bll.ZJJHSetTypeDEF6(KeyValue, Type);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "0", Message = "批次编号为空,请重试" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 表单验证 获取当前数据行的状态
        /// <summary>
        /// 获取当前数据行的状态
        /// </summary>
        /// <param name="KeyValue">主表ID</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetJHStatus(string KeyValue)
        {
            try
            {
                IDatabase database = DataFactory.Database();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    VQC_ZJJHCY model = database.FindEntity<VQC_ZJJHCY>(KeyValue);

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
    }
}
