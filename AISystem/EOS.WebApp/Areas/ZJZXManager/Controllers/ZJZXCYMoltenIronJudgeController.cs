using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using EOS.Entity;
using EOS.Repository;
using EOS.DataAccess;
using System.Text;

namespace EOS.WebApp.Areas.ZJZXManager.Controllers
{
    public class ZJZXCYMoltenIronJudgeController : Controller
    {
        QC_ZJZXCYBll bll = new QC_ZJZXCYBll();
        VQC_ZJZXCYBll vbll = new VQC_ZJZXCYBll();
        #region  页面返回
        public ActionResult ZJZXCYMoltenIronJudgeList()
        {
            return View();
        }
        public ActionResult ZJZXCYMoltenIronJudgeAdd()
        {
            return View();
        }
        public ActionResult ZJZXCYMoltenIronJudgePeopleAdd()
        {
            return View();
        }
        public ActionResult ZJZXCYMoltenIronJudgeWeight()
        {
            return View();
        }
        public ActionResult ZJZXCYSelectWeight()
        {
            return View();
        }
        /// <summary>
        /// 获取铁水匹配罐号重量数据
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetMoltenData(string KeyValue)
        {
            VQC_ZJZXCY entity = DataFactory.Database().FindEntity<VQC_ZJZXCY>(KeyValue);
            List<VQC_ZJZXCYDETAILS> entitydetails = DataFactory.Database().FindList<VQC_ZJZXCYDETAILS>("AND MAINID IN (SELECT ID FROM QC_ZJZXCY WHERE DEF12='" + entity.DEF12 + "' AND NVL(DEF15,'0')='0') ORDER BY DEF12 ");

            if (entity.DEF15 == "1")
            {
                //已移除
                return Content("{\"M\":[" + new VQC_ZJZXCY().ToJson() + "],\"D\":" + new List<VQC_ZJZXCYDETAILS>().ToJson() + "}");
            }

            if (entity.DEF14 == "1")
            {
                //已匹配
                return Content("{\"M\":[" + entity.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
            }
            else
            {
                int count = entitydetails.Count;//罐的数量 其中包括铁水主批次
                string orderby = "";
                List<VQC_ZJZXCYDETAILS> temp = entitydetails.FindAll(delegate(VQC_ZJZXCYDETAILS a)
                {
                    return !string.IsNullOrEmpty(a.DEF10) && a.DEF15 != "1";
                });
                if (temp.Count > 0)
                {
                    List<WB_RAILWAY_DTL> allmodel;
                    string fx = temp[0].DEF10.ToString();
                    if (fx.IndexOf('南') > -1)
                    {
                        fx = "南";
                        orderby = " DESC ";
                        //model = DataFactory.Database().FindListBySql<WB_RAILWAY_DTL>("SELECT * FROM WB_RAILWAY_DTL WHERE STOVENUM='" + entity.BILLNO + "' AND STATUS=1 AND TYPE=4 AND ROWNUM<" + count + " ORDER BY MAOTIME " + orderby + " ");
                        allmodel = DataFactory.Database().FindListBySql<WB_RAILWAY_DTL>("SELECT * FROM WB_RAILWAY_DTL WHERE STOVENUM='" + entity.BILLNO + "' AND STATUS=1 AND TYPE=4 ORDER BY MAOTIME " + orderby + " ");
                    }
                    else
                    {
                        fx = "北";
                        orderby = " ASC ";
                        //model = DataFactory.Database().FindListBySql<WB_RAILWAY_DTL>("SELECT * FROM WB_RAILWAY_DTL WHERE STOVENUM='" + entity.BILLNO + "' AND STATUS=1 AND TYPE=4 AND ROWNUM<" + count + " ORDER BY MAOTIME " + orderby + " ");
                        allmodel = DataFactory.Database().FindListBySql<WB_RAILWAY_DTL>("SELECT * FROM WB_RAILWAY_DTL WHERE STOVENUM='" + entity.BILLNO + "' AND STATUS=1 AND TYPE=4 ORDER BY MAOTIME " + orderby + " ");
                    }

                    //List<WB_RAILWAY_DTL> model = DataFactory.Database().FindListBySql<WB_RAILWAY_DTL>("SELECT * FROM WB_RAILWAY_DTL WHERE STOVENUM='" + entity.BILLNO + "' AND STATUS=1 AND TYPE=4 AND ROWNUM<" + count + " ORDER BY MAOTIME " + orderby + " ");
                    //List<WB_RAILWAY_DTL> model = DataFactory.Database().FindListBySql<WB_RAILWAY_DTL>("SELECT * FROM WB_RAILWAY_DTL WHERE ROWNUM<" + count + "  ORDER BY MAOTIME " + orderby + " ");

                    if (allmodel.Count < temp.Count)
                    {
                        return Content("{\"Error\":[\"磅单数量不足，目前磅单数量" + allmodel.Count + "\"]}");
                    }

                    List<WB_RAILWAY_DTL> model = allmodel.OrderBy(a => a.MAOTIME).Take(temp.Count).ToList();

                    decimal firstmao = 0, firstpi = 0, firstfact = 0;
                    decimal summao = 0, sumpi = 0, sumfact = 0;
                    if (allmodel.Count > 0)
                    {
                        decimal tempdecimal = 0;
                        if (decimal.TryParse(allmodel.Sum(a => a.MAOAMOUNT).ToString(), out tempdecimal))
                        {
                            summao = tempdecimal;
                        }
                        tempdecimal = 0;
                        if (decimal.TryParse(allmodel.Sum(a => a.PIAMOUNT).ToString(), out tempdecimal))
                        {
                            sumpi = tempdecimal;
                        }
                        tempdecimal = 0;
                        if (decimal.TryParse(allmodel.Sum(a => a.FACTAMOUNT).ToString(), out tempdecimal))
                        {
                            sumfact = tempdecimal;
                        }

                        int j = 0;
                        for (int i = 0; i < temp.Count + 1; i++)
                        {
                            if (entitydetails[i].DEF11 == "炉号+流水")
                            {
                                entitydetails[i].DEF4 = summao.ToString(); //毛
                                entitydetails[i].DEF5 = sumpi.ToString();  //皮
                                entitydetails[i].DEF6 = sumfact.ToString();//净
                            }
                            else
                            {
                                entitydetails[i].BILLNO = model[j].BILLNO; //磅单
                                entitydetails[i].DEF14 = model[j].MAOTIME.ToString();//毛重时间
                                entitydetails[i].CARS = model[j].CARNUM.ToString();//罐号
                                if (i == count - 1)
                                {
                                    //最后一罐  重量=总重量-前面的罐重量
                                    entitydetails[i].DEF4 = (summao - firstmao).ToString();//毛
                                    entitydetails[i].DEF5 = (sumpi - firstpi).ToString(); //皮
                                    entitydetails[i].DEF6 = (sumfact - firstfact).ToString();//净
                                }
                                else
                                {
                                    entitydetails[i].DEF4 = model[j].MAOAMOUNT.ToString();//毛
                                    entitydetails[i].DEF5 = model[j].PIAMOUNT.ToString(); //皮
                                    entitydetails[i].DEF6 = model[j].FACTAMOUNT.ToString();//净

                                    tempdecimal = 0;
                                    if (decimal.TryParse(model[j].MAOAMOUNT.ToString(), out tempdecimal))
                                    {
                                        firstmao += tempdecimal;
                                    }
                                    tempdecimal = 0;
                                    if (decimal.TryParse(model[j].PIAMOUNT.ToString(), out tempdecimal))
                                    {
                                        firstpi += tempdecimal;
                                    }
                                    tempdecimal = 0;
                                    if (decimal.TryParse(model[j].FACTAMOUNT.ToString(), out tempdecimal))
                                    {
                                        firstfact += tempdecimal;
                                    }
                                }
                                j++;
                            }
                        }
                    }

                    return Content("{\"M\":[" + entity.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
                }
                else
                {
                    return Content("{\"M\":[" + entity.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
                }
            }



        }

        /// <summary>
        /// 获取铁水匹配罐号重量数据  手工匹配
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetMoltenDataPeople(string KeyValue)
        {
            VQC_ZJZXCY entity = DataFactory.Database().FindEntity<VQC_ZJZXCY>(KeyValue);
            List<VQC_ZJZXCYDETAILS> entitydetails = DataFactory.Database().FindList<VQC_ZJZXCYDETAILS>("AND MAINID IN (SELECT ID FROM QC_ZJZXCY WHERE DEF12='" + entity.DEF12 + "' AND NVL(DEF15,'0')='0' ) ORDER BY DEF12 ");

            if (entity.DEF15 == "1")
            {
                //已移除
                return Content("{\"M\":[" + new VQC_ZJZXCY().ToJson() + "],\"D\":" + new List<VQC_ZJZXCYDETAILS>().ToJson() + "}");
            }

            return Content("{\"M\":[" + entity.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");

            //if (entity.DEF14 == "1")
            //{
            //    //已匹配
            //    return Content("{\"M\":[" + entity.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
            //} 
        }

        #endregion

        #region 数据列表
        /// <summary>
        /// 获取铁水转序列表
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetMoltenIronList(string BILLNO, string StartTime, string EndTime, string DEF14, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCY> ListData = vbll.GetMoltenIronList(BILLNO, StartTime, EndTime, DEF14, jqgridparam);
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
        /// 铁水明细批次
        /// </summary>
        /// <param name="MAINID"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetMoltenIronListItem(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCYDETAILS> ListData = vbll.GetMoltenIronListItem(MAINID, jqgridparam);
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
        /// 获取铁水磅单列表
        /// </summary>
        /// <param name="LH"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetWeightForMolten(string LH, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VWB_RAILWAY_DTL> ListData = vbll.GetWeightForMolten(LH, jqgridparam);
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
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <param name="ZJZXCYDetailJson"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MoltenWeightSAVE(QC_ZJZXCY entity, string KeyValue, string Details, string PITYPE)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.MoltenWeightSAVE(entity, KeyValue, Details, PITYPE);
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
        /// 铁水批量匹配净重
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJZXCYMoltenIronWeightSave(string DATA)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = bll.ZJZXCYMoltenIronWeightSave(DATA);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中批次,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 清除匹配状态
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJZXCYClearStatus(string KeyValue)
        {
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    IDatabase database = DataFactory.Database();
                    StringBuilder strsql = new StringBuilder();
                    strsql.AppendFormat("UPDATE QC_ZJZXCY SET DEF14=0 WHERE ID='{0}'", KeyValue);
                    database.ExecuteBySql(strsql);
                    return Content(new JsonMessage { Success = true, Code = "1", Message = "操作成功" }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中批次,请重新选择" }.ToString());
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
