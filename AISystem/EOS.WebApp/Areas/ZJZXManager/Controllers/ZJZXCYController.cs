using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using EOS.Business;
using EOS.Utilities;
using System.Diagnostics;
using EOS.Repository;
using EOS.DataAccess;

namespace EOS.WebApp.Areas.ZJZXManager.Controllers
{
    public class ZJZXCYController : PublicController<QC_ZJZXCY>
    {
        QC_ZJZXCYBll bll = new QC_ZJZXCYBll();
        QC_ZJZXCYCJBLL cjbll = new QC_ZJZXCYCJBLL();
        VQC_ZJZXCYBll vbll = new VQC_ZJZXCYBll();
        VQC_ZJZXCYDETAILSBll vbllitem = new VQC_ZJZXCYDETAILSBll();
        VZJZXQYBLL vzjzxbll = new VZJZXQYBLL();
        #region 页面返回
        public ActionResult ZJZXCYAdd()
        {
            return View();
        }
        public ActionResult ZJZXCYList()
        {
            return View();
        }
        public ActionResult ZJZXCYSelectWeight()
        {
            ViewBag.FormID = "ZJZXCYSelectWeight";
            return View();
        }
        public ActionResult ZJZXCYAddCar()
        {
            return View();
        }
        public ActionResult ZJZXCYDeleteCar()
        {
            return View();
        }
        public ActionResult ZJZXCYSelectFHUnit()
        {
            return View();
        }
        public ActionResult ZJZXCYCJForm()
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
            VQC_ZJZXCY entity = DataFactory.Database().FindEntity<VQC_ZJZXCY>(KeyValue);
            List<VQC_ZJZXCYDETAILS> entitydetails = DataFactory.Database().FindList<VQC_ZJZXCYDETAILS>("MAINID", KeyValue, "CRETIME", "ASC");
            return Content("{\"M\":[" + entity.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
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
        public ActionResult GetList(string BILLNO, string StartTime, string EndTime, string MATERIALNAME, string LEADERDEPAS, string CARNAME, string CHECKTYPE, string CHECKISSEND, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCY> ListData = vbll.GetList(BILLNO, StartTime, EndTime, MATERIALNAME, LEADERDEPAS, CARNAME, CHECKTYPE, CHECKISSEND, jqgridparam);
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
                List<VQC_ZJZXCYDETAILS> ListData = vbllitem.GetListItem(MAINID, jqgridparam);
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
        /// 转序汽运过磅单
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="MATERIAL"></param>
        /// <param name="SUPPLY"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZXWeightList(string StartTime, string EndTime, string PK_MARBASCLASS, string MATERIAL, string SUPPLY, string PK_MARBASCLASSNAME, string MATERIALNAME, string SUPPLYNAME, string CARS, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJZXQY> ListData = vzjzxbll.GetZXWeightList(StartTime, EndTime, PK_MARBASCLASS, MATERIAL, SUPPLY, PK_MARBASCLASSNAME, MATERIALNAME, SUPPLYNAME, CARS, jqgridparam);
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
        /// 通过转序过磅单号获取磅单信息
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
                    return Content(vzjzxbll.PCGetQYData(KeyValue));
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
        public ActionResult GetPCZXCYAddCarList(string KeyValue, string StartTime, string EndTime, string CARNAME, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJZXQY> ListData = vzjzxbll.GetPCZXCYAddCarList(KeyValue, StartTime, EndTime, CARNAME, jqgridparam);
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
        /// 移除组批车辆明细列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetPCZXCYDeleteCarList(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCYDETAILS> ListData = vbllitem.GetListItem(KeyValue, jqgridparam);
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
        /// 获取发货单位
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetZXFHUnitList(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VZJZXQY> ListData = vzjzxbll.GetZXFHUnitList(KeyValue, jqgridparam);
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
                ResultMessage message = bll.PCZXCYDelete(KeyValue);

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
        public ActionResult PCZXCYFinishNew(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.PCZXCYFinishNew(KeyValue);
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
        public ActionResult PCZXCYUNFinish(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.PCZXCYUNFinish(KeyValue);
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
        public ActionResult PCZXCYDeleteItem(string KeyValue, string KeyList)
        {
            try
            {
                ResultMessage message = bll.PCZXCYDeleteItem(KeyValue, KeyList);

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
        /// 批量采样
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCZXALLCY(string KeyValue, string type)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.PCZXALLCY(KeyValue, type);
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
        /// 选择发货单位插入磅单 
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="PK_LEADERDEPAS"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCZXALLCYForLEADERDEPAS(string KeyValue, string PK_LEADERDEPAS, string LEADERDEPAS)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.PCZXALLCYForLEADERDEPAS(KeyValue, PK_LEADERDEPAS, LEADERDEPAS);
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



        #endregion

        #region 表单提交

        /// 提交表单 
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="PCRAWCYDetailJson">子表数据</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJZXCYSAVE(QC_ZJZXCY entity, string KeyValue, string ZJZXCYDetailJson)
        {
            ResultMessage message = new ResultMessage();
            int IsOk = 0;
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.ZJZXCYEditSave(entity, KeyValue, ZJZXCYDetailJson);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                    //if (IsOk == 2)
                    //{
                    //    return Content(new JsonMessage { Success = false, Code = "0", Message = "当前批号已存在,请重新输入批号" }.ToString());
                    //}
                    //else if (IsOk == 3)
                    //{
                    //    return Content(new JsonMessage { Success = false, Code = "3", Message = "已采样的车辆明细有部分超出当前的日期时间段,请先移除超出部分的车辆" }.ToString());
                    //}
                    //else
                    //{
                    //    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
                    //}
                }
                else
                {

                    message = bll.ZJZXCYSave(entity, ZJZXCYDetailJson);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                    //if (IsOk == 2)
                    //{
                    //    return Content(new JsonMessage { Success = false, Code = "0", Message = "当前批号已存在,请重新输入批号" }.ToString());
                    //}
                    //else
                    //{
                    //    return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
                    //}
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
        public ActionResult PCZXCYAddCarItem(string KeyValue, string BILLNOLIST)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = vzjzxbll.PCZXCYAddCarItem(KeyValue, BILLNOLIST);
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


        #endregion

        #region 表单验证 获取当前数据行的状态
        /// <summary>
        /// 获取当前数据行的状态
        /// </summary>
        /// <param name="KeyValue">主表ID</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZXStatus(string KeyValue)
        {
            try
            {
                IDatabase database = DataFactory.Database();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    VQC_ZJZXCY model = database.FindEntity<VQC_ZJZXCY>(KeyValue);

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

        #region  抽检部分

        /// <summary>
        /// 获取抽检数据当前状态
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZXCJStatus(string ID)
        {
            try
            {
                IDatabase database = DataFactory.Database();
                if (!string.IsNullOrEmpty(ID))
                {
                    VQC_ZJZXCYCJFORAPPHAND model = database.FindEntity<VQC_ZJZXCYCJFORAPPHAND>(ID);

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


        /// <summary>
        /// 获取抽检列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="CarsName"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetCJList(string TaskNo, string StartTime, string EndTime, string ISCJ, string ISCY, string MATERIALNAME, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCYCJFORAPPHAND> ListData = vbll.GetCJList(TaskNo, StartTime, EndTime, ISCJ, ISCY, MATERIALNAME, jqgridparam);
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
        /// 抽检操作
        /// </summary>
        /// <param name="KeyValue">采样ID</param>
        /// <param name="ID">发货单ID</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCZXCYCJ(string KeyValue, string ID)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    message = cjbll.PCZXCYCJ(KeyValue, ID);
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
        /// 取消抽检操作
        /// </summary>
        /// <param name="KeyValue">采样ID</param>
        /// <param name="ID">发货单ID</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCZXCYUNCJ(string KeyValue, string ID)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    message = cjbll.PCZXCYUNCJ(KeyValue, ID);
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
        /// 采样操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCZXCY_CY(string KeyValue, string ID)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    message = cjbll.PCZXCY_CY(KeyValue, ID);
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
        /// 采样操作
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="ID"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCZXCY_UNCY(string KeyValue, string ID)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    message = cjbll.PCZXCY_UNCY(KeyValue, ID);
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

        #endregion
    }
}
