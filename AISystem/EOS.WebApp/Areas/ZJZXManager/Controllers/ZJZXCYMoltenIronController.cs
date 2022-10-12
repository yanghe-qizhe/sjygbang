using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using EOS.Business;
using System.Diagnostics;
using EOS.Utilities;
using EOS.DataAccess;
using EOS.Repository;
using System.Data.Common;
using System.Text;
using System.Data;

namespace EOS.WebApp.Areas.ZJZXManager.Controllers
{
    public class ZJZXCYMoltenIronController : PublicController<QC_ZJZXCY>
    {
        QC_ZJZXCYBll bll = new QC_ZJZXCYBll();
        VQC_ZJZXCYBll vbll = new VQC_ZJZXCYBll();
        VQC_ZJZXCYDETAILSBll vbllitem = new VQC_ZJZXCYDETAILSBll();
        VZJZXQYBLL vzjzxbll = new VZJZXQYBLL();
        VQC_ZJZXRESULTDETAILSBLL zjresultbll = new VQC_ZJZXRESULTDETAILSBLL();
        #region 页面返回
        public ActionResult ZJZXCYMoltenIronList()
        {
            return View();
        }
        public ActionResult ZJZXCYMoltenIronAdd()
        {
            return View();
        }
        /// <summary>
        /// 获取铁水编辑数据
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetMoltenData(string KeyValue)
        {
            VQC_ZJZXCY entity = DataFactory.Database().FindEntity<VQC_ZJZXCY>(KeyValue);
            List<VQC_ZJZXCYDETAILS> entitydetails = DataFactory.Database().FindList<VQC_ZJZXCYDETAILS>("AND MAINID IN (SELECT ID FROM QC_ZJZXCY WHERE DEF12='" + entity.DEF12 + "') ORDER BY DEF12 ");

            return Content("{\"M\":[" + entity.ToJson() + "],\"D\":" + entitydetails.ToJson() + "}");
        }
        #endregion

        #region 表单列表
        /// <summary>
        /// 获取采样组批列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetIronList(string BILLNO, string StartTime, string EndTime, string MATERIALNAME, string CHECKTYPE, string CHECKISSEND, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXCY> ListData = vbll.GetIronList(BILLNO, StartTime, EndTime, MATERIALNAME, CHECKTYPE, CHECKISSEND, jqgridparam);
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
        /// 获取所有的质检项目值、质检结果
        /// </summary>
        /// <param name="CYID"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetIronZJItemList(string CYID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VQC_ZJZXRESULTDETAILS> ListData = zjresultbll.GetZXALLAuditZJItemListForCYID(CYID, jqgridparam);
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
        /// 获取最新的两炉质检结果
        /// </summary>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetIronZJItemListNow(string Columns, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                ResultMessage message = vbll.GetIronZJItemListNow(Columns, jqgridparam);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = message.Content,
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
        /// 获取铁水固定 物料 和 固定方案   
        /// </summary>
        /// <returns></returns>
        public ActionResult GetMaterial()
        {
            try
            {
                //铁水  1001A1100000000LV53Q
                string MATERIAL = "1001A1100000000LV53Q";
                ResultMessage message = new ResultMessage();
                IDatabase database = DataFactory.Database();

                List<VBase_ZJFA> mlist = database.FindList<VBase_ZJFA>(" AND MATERIALID='" + MATERIAL + "' AND ENABLED=1 AND TYPE='78341346-7108-443d-a20c-dd3a061ec999' ");

                if (mlist.Count > 0)
                {
                    message.Code = 1;
                    message.Message = mlist.First<VBase_ZJFA>().ToJson();
                }
                else
                {
                    message.Code = -1;
                    message.Message = "未查询到铁水的质检方案,请先维护质检方案";
                }

                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// 提交表单 页面的完成 
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCZXCYFinishMolten(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.PCZXCYFinishMolten(KeyValue);
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
        public ActionResult PCZXCYUNFinishMolten(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.PCZXCYUNFinishMolten(KeyValue);
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
        /// 通过高炉号  获取流水
        /// </summary>
        /// <param name="GL">高炉号</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetLS(string GL)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                IDatabase database = DataFactory.Database();

                string TEMP = "00000";
                StringBuilder strsql = new StringBuilder();
                strsql.AppendFormat("SELECT MAX(DEF4) AS DEF4 FROM QC_ZJZXCY WHERE PCRAWTYPE=2 AND DEF3='" + GL + "' ");
                DataTable dt = database.FindTableBySql(strsql.ToString());

                if (dt != null && dt.Rows.Count > 0)
                {
                    TEMP = string.IsNullOrEmpty(dt.Rows[0]["DEF4"].ToString()) ? TEMP : dt.Rows[0]["DEF4"].ToString();
                }

                string t = "00000000";
                int lsnum = Convert.ToInt32(TEMP) + 1;


                message.Code = 1;
                message.Message = t.Substring(0, 5 - lsnum.ToString().Length) + lsnum;

                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 删除数据  
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual ActionResult PCZXCYDeleteForMolten(string KeyValue)
        {
            try
            {
                ResultMessage message = bll.PCZXCYDeleteForMolten(KeyValue);

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

        /// <summary>
        /// 铁水采样提交
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult MoltenSAVE(QC_ZJZXCY entity, string Details, string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = bll.ZXMOLTENCYSave(entity, Details, KeyValue);
                    return Content(new JsonMessage { Success = false, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    message = bll.ZXMOLTENCYSave(entity, Details, KeyValue);
                    return Content(new JsonMessage { Success = false, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }


        #endregion


        #region 临时处理 由于移入移除批次内的明细 导致NC传输的不对 例如（传NC10车后，又给当前批次内移入了几车）
        /// <summary>
        /// 临时处理 由于移入移除批次内的明细 导致NC传输的不对 例如（传NC10车后，又给当前批次内移入了几车）
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        //public ActionResult MoltenSAVE(string KeyValue)
        //{
        //    IDatabase database = DataFactory.Database();
        //    DbTransaction isOpenTrans = database.BeginTrans();
        //    ResultMessage message = new ResultMessage();
        //    try
        //    {
        //        string sql = "select a.* from bd_pcrawcydetails a where a.mainid='" + KeyValue + "' and not exists (";
        //        sql += " select 1 from qc_zjresultdetailsnc b where a.pcid=b.pcid and a.pcitemid=b.pcitemid  ";
        //        sql += ")";

        //        List<BD_PCRAWCYDETAILS> mlist = database.FindListBySql<BD_PCRAWCYDETAILS>(sql);
        //        QC_ZJRESULTNC mqc = database.FindEntityByWhere<QC_ZJRESULTNC>(" and cyid='" + KeyValue + "' ");

        //        StringBuilder strsql2 = new StringBuilder();
        //        strsql2.Append("Insert into QC_ZJRESULTDETAILSNC(ID,CYID,MAINID,PCID,PCITEMID,UPLOAD,CRETIME,CREUSER) VALUES(");
        //        StringBuilder strsql = new StringBuilder();
        //        int i = 0;
        //        foreach (BD_PCRAWCYDETAILS m in mlist)
        //        {
        //            strsql.Clear();
        //            strsql.AppendFormat("{0} ", strsql2.ToString());
        //            strsql.AppendFormat("'{0}',", Guid.NewGuid().ToString());
        //            strsql.AppendFormat("'{0}',", KeyValue);
        //            strsql.AppendFormat("'{0}',", mqc.ID);
        //            strsql.AppendFormat("'{0}',", m.PCID);
        //            strsql.AppendFormat("'{0}',", m.PCITEMID);
        //            strsql.AppendFormat("'{0}',", "0");
        //            strsql.AppendFormat("'{0}',", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        //            strsql.AppendFormat("'{0}')", ManageProvider.Provider.Current().UserName);
        //            int a = database.ExecuteBySql(strsql, isOpenTrans);
        //            i = i + a;
        //        }

        //        database.Commit();

        //        message.Code = 1;
        //        message.Content = "操作成功";

        //        return Content(new JsonMessage { Success = true, Code = "1", Message = "操作成功" + i.ToString() }.ToString());
        //    }
        //    catch (Exception ex)
        //    {
        //        return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败" }.ToString());
        //    }
        //}
        #endregion

    }
}
