using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;

namespace EOS.WebService
{
    /// <summary>
    /// MESWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class MESWebService : System.Web.Services.WebService
    {


        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }

        /// <summary>
        /// 获取采样及检验项目
        /// </summary>
        /// <param name="BILLNO">检验批号</param>
        /// <param name="ICCARD">IC卡序号</param>
        /// <returns></returns>
        [WebMethod(Description = "获取采样及检验项目")]
        public EOS_CYORDER GetCYInfoByItems(string BILLNO, string ICCARD)
        {
            EOS_CYORDER cyorder = new EOS_CYORDER();
            StringBuilder strSql = new StringBuilder();

            IDatabase database = DataFactory.Database();
            VBD_PCRAWCY entity = null;
            VBD_ZJRESULTBll vorderbll = new VBD_ZJRESULTBll();
            //string JsonData = "";

            List<VBD_PCRAWCYDETAILS> list = null;
            List<VBase_ZJFAConfig_B> listitems = null;
            if (!string.IsNullOrEmpty(BILLNO))
            {
                entity = database.FindEntityByWhere<VBD_PCRAWCY>(string.Format(" AND nvl(ISZY,0)=1 and PRINTCODE='{0}'", BILLNO));
            }
            if (!string.IsNullOrEmpty(ICCARD))
            {
                entity = database.FindEntityByWhere<VBD_PCRAWCY>(string.Format(" AND nvl(ISZY,0)=1 and DEF6='{0}'", ICCARD));
            }

            if (entity != null)
            {
                cyorder.MODEL = entity;
                if (!string.IsNullOrEmpty(entity.ID))
                {
                    strSql.Append(@"SELECT * FROM VBD_PCRAWCYDETAILS WHERE 1=1");
                    strSql.Append(string.Format(" AND MAINID ='{0}'", entity.ID));
                    list = database.FindListBySql<VBD_PCRAWCYDETAILS>(strSql.ToString());
                    cyorder.DETAILS = list;
                }
                string Supply = "";
                if (!string.IsNullOrEmpty(entity.SUPPLY))
                {
                    Supply = entity.SUPPLY.Trim();
                }

                string Material = "";
                if (!string.IsNullOrEmpty(entity.MATERIAL))
                {
                    Material = entity.MATERIAL.Trim();
                }

                string MaterialSpec = "";
                if (!string.IsNullOrEmpty(entity.MATERIALSPEC))
                {
                    MaterialSpec = entity.MATERIALSPEC.Trim();
                }

                listitems = vorderbll.GetOrderEntryListHY(Supply, Material, MaterialSpec);
                if (listitems.Count == 0)
                {
                    listitems = vorderbll.GetOrderEntryListHY("", Material, MaterialSpec);
                    if (listitems.Count == 0)
                    {
                        listitems = vorderbll.GetOrderEntryListHY(Supply, Material);
                        if (listitems.Count == 0)
                        {
                            listitems = vorderbll.GetOrderEntryListHY("", Material);
                        }
                    }
                }
                cyorder.ITEMSDETAILS = listitems;
            }
            return cyorder;
        }

        /// <summary>
        /// 化验单保存
        /// </summary>
        /// <param name="info">化验单实体对象</param>
        /// <returns></returns>
        [WebMethod(Description = "化验单保存")]
        public StatusMsg SaveZJRESULT(EOS_ZJRESULTORDER info)
        {
            int res = 0;
            StatusMsg msg = new StatusMsg();
            msg.Create();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                msg.IFWhere(!(string.IsNullOrEmpty(info.PRINTCODE)), string.Format("制样单号:{0}不能为空！", info.PRINTCODE));
                if (!msg.Status) return msg;
                BD_ZJRESULT entity = new BD_ZJRESULT();
                #region 初始始化
                entity.BILLNO = info.BILLNO;
                entity.CREUSER = info.CREUSER;
                entity.CRETIME = info.CRETIME;
                entity.ZJFA = info.ZJFA;
                entity.CYID = info.CYID;
                entity.PCRAWTYPE = info.PCRAWTYPE;
                entity.PRINTCODE = info.PRINTCODE;
                entity.CZTYPE = info.CZTYPE;
                entity.MATERIAL = info.MATERIAL;
                entity.MATERIALSPEC = info.MATERIALSPEC;
                entity.PK_ORG = info.PK_ORG;
                entity.ORGNAME = info.ORGNAME;
                entity.CYTYPE = info.CYTYPE;
                entity.DEF10 = info.DEF10;
                entity.ZJRESULT = info.ZJRESULT;
                entity.CHECKNUM = info.CHECKNUM;

                entity.MEMO = info.MEMO;
                #endregion

                //明细行数据
                List<BD_ZJRESULTDETAILS> OrderEntryList = info.DETAILS;
                string sql = String.Format("select * from BD_ZJRESULT where PRINTCODE='{0}' ", entity.PRINTCODE);
                BD_ZJRESULT entity_up = database.FindEntityBySql<BD_ZJRESULT>(sql);

                string Message = "新增成功。";
                if (!string.IsNullOrEmpty(entity_up.ID))
                {
                    database.Delete<BD_ZJRESULTDETAILS>("MAINID", entity_up.ID, isOpenTrans);
                    entity.Modify(entity_up.ID);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    //制样单是否已生成化验单
                    // msg.IFWhere(!(entity_up == null), string.Format("当前制样单号:{0}已生成化验单！", entity.PRINTCODE));

                    sql = String.Format("select count(1) from BD_PCRAWCY where PRINTCODE='{0}' ", entity.PRINTCODE);
                    res = database.FindCountBySql(sql);
                    msg.IFWhere(!(res == 0), string.Format("当前制样单号:{0}不存在！", entity.PRINTCODE));
                    if (!msg.Status) return msg;
                    string ModuleId = ConfigurationManager.AppSettings["ZJMODELID"].Trim();
                    base_coderulebll.OccupyBillCode("System", ModuleId, isOpenTrans);
                    entity.BILLNO = base_coderulebll.GetBillCode3F("System", ModuleId);
                    entity.ID = Guid.NewGuid().ToString();
                    entity.PHYSTATUS = "0";
                    entity.CHESTATUS = "0";
                    entity.WATERSTATUS = "0";
                    entity.AUDITSTATUS = "0";
                    entity.ALLAUDITSTATUS = "0";
                    entity.AUDITRELEASE = "0";
                    entity.ALLAUDITRELEASE = "0";
                    database.Insert(entity, isOpenTrans);
                }


                //处理明细
                int index = 1;
                string ZJRESULTMEMO = "";
                //bool bol = false;
                foreach (BD_ZJRESULTDETAILS orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.ZJITEM))
                    {
                        orderentry.Create();
                        orderentry.MAINID = entity.ID;
                        database.Insert(orderentry, isOpenTrans);
                        if (!string.IsNullOrEmpty(orderentry.ZJITEMNAME))
                        {
                            ZJRESULTMEMO += string.Format("{0}{1}:{2}({3});", orderentry.ZJITEMNAME, orderentry.DEF1, orderentry.ZJVALUE, (orderentry.ZJBZRESULT == "0" ? "合格" : "不合格"));
                        }
                        index++;
                    }
                }

                //更新为采样化验
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=5,DEF10='{0}' where ID='{1}'", entity.BILLNO, entity.CYID));
                database.ExecuteBySql(strSql, isOpenTrans);
                ZJRESULTMEMO = ZJRESULTMEMO.TrimEnd(';');
                //更新检验结果
                strSql.Clear();
                strSql.Append(string.Format("update BD_ZJRESULT set ZJRESULTMEMO='{0}' where ID='{1}'", ZJRESULTMEMO, entity.ID));
                database.ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                msg.IFWhere(true, String.Format("编号{0}的数据保存成功！", entity.BILLNO));
                msg.Data = entity.BILLNO;
            }
            catch (Exception ex)
            {
                database.Rollback();
                msg.IFWhere(false, String.Format("编号{0}的数据保存失败！", info.PRINTCODE));
            }
            return msg;
        }

        /// <summary>
        /// 化验单删除
        /// </summary>
        /// <param name="info">化验单编号</param>
        /// <returns></returns>
        [WebMethod(Description = "化验单删除")]
        public StatusMsg DeleteZJRESULT(string BILLNO)
        {
            StatusMsg msg = new StatusMsg();
            msg.Create();
            msg.IFWhere(!(string.IsNullOrEmpty(BILLNO)), string.Format("单号:{0}不能为空！", BILLNO));
            if (!msg.Status) return msg;
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string sql = String.Format("select * from BD_ZJRESULT where BILLNO='{0}' ", BILLNO);
                BD_ZJRESULT entity = database.FindEntityBySql<BD_ZJRESULT>(sql);
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("delete BD_ZJRESULT where BILLNO='{0}'", BILLNO));
                database.ExecuteBySql(strSql, isOpenTrans);
                strSql.Clear();
                strSql.Append(string.Format("delete BD_ZJRESULTDETAILS where MAINID='{0}'", entity.ID));
                database.ExecuteBySql(strSql, isOpenTrans);
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=4,DEF10='{0}' where ID='{1}'", "", entity.CYID));
                database.ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                msg.IFWhere(true, String.Format("编号{0}的数据删除成功！", BILLNO));
            }
            catch (Exception ex)
            {
                database.Rollback();
                msg.IFWhere(false, String.Format("编号{0}的数据删除失败！", BILLNO));
            }
            return msg;
        }


        /// <summary>
        /// 获取化验单
        /// </summary>
        /// <param name="PRINTCODE">检验批号</param>
        /// <param name="BILLNO">化验单号</param>
        /// <returns></returns>
        [WebMethod(Description = "获取化验单")]
        public EOS_ZJORDER GetZJRESULT(string PRINTCODE, string BILLNO)
        {
            EOS_ZJORDER zjorder = new EOS_ZJORDER();
            StringBuilder strSql = new StringBuilder();

            IDatabase database = DataFactory.Database();
            VBD_ZJRESULT entity = null;

            List<VBD_ZJRESULTDETAILS> list = null;
            if (!string.IsNullOrEmpty(PRINTCODE))
            {
                entity = database.FindEntityByWhere<VBD_ZJRESULT>(string.Format(" AND PRINTCODE='{0}'", PRINTCODE));
            }
            if (!string.IsNullOrEmpty(BILLNO))
            {
                entity = database.FindEntityByWhere<VBD_ZJRESULT>(string.Format(" AND BILLNO='{0}'", BILLNO));
            }

            if (entity != null)
            {
                zjorder.MODEL = entity;
                if (!string.IsNullOrEmpty(entity.ID))
                {
                    strSql.Append(@"SELECT * FROM VBD_ZJRESULTDETAILS WHERE 1=1");
                    strSql.Append(string.Format(" AND MAINID ='{0}'", entity.ID));
                    list = database.FindListBySql<VBD_ZJRESULTDETAILS>(strSql.ToString());
                    zjorder.DETAILS = list;
                }

            }
            return zjorder;
        }


        /// <summary>
        /// 获取移库计量单信息
        /// </summary>
        /// <param name="info">查询对象</param>
        /// <returns></returns>
        [WebMethod(Description = "获取移库计量单信息")]
        public JSONDATA GetWeightZX(SearchInfo info)
        {
            StringBuilder strSql = new StringBuilder();
            Stopwatch watch = CommonHelper.TimerStart();
            IDatabase database = DataFactory.Database();
            VWB_WEIGHTBSZXBll orderbll = new VWB_WEIGHTBSZXBll();
            JqGridParam jqgridparam = new JqGridParam();
            jqgridparam.page = info.Page.PageIndex;
            jqgridparam.rows = info.Page.PageSize;

            if (string.IsNullOrEmpty(info.Page.SortName))
            {
                jqgridparam.sidx = "";
            }
            else
            {
                jqgridparam.sidx = info.Page.SortName;
            }
            if (string.IsNullOrEmpty(info.Page.SortOrder))
            {
                jqgridparam.sord = "desc";
            }
            else
            {
                jqgridparam.sord = info.Page.SortOrder;
            }
            List<VWB_WEIGHTBS_ZX> ListData = orderbll.GetZXOrderList("", info.Send, info.Receive, info.Materiel, "", info.Cars, "", "", "1", "", info.StartTime, info.EndTime, jqgridparam);
            JSONDATA JsonData = new JSONDATA();
            JsonData.total = info.Page.PageSize;
            JsonData.page = info.Page.PageIndex;
            JsonData.records = info.Page.TotalNumber;
            JsonData.costtime = CommonHelper.TimerEnd(watch);
            JsonData.rows = ListData;
            return JsonData;
        }


        /// <summary>
        /// 生成上料计划单
        /// </summary>
        /// <param name="entity">计划对象</param>
        /// <returns></returns>
        [WebMethod(Description = "生成上料计划单")]
        public StatusMsg SaveSLJHOrder(Z_ITEM_DEMAND_SUM entity)
        {
            int res = 0;
            StatusMsg msg = new StatusMsg();
            msg.Create();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                msg.IFWhere(!(string.IsNullOrEmpty(entity.HANDLE)), string.Format("计划单号:{0}不能为空！", entity.HANDLE));
                if (!msg.Status) return msg;
                msg.IFWhere(!(string.IsNullOrEmpty(entity.SITE)), string.Format("计划单号站点:{0}不能为空！", entity.SITENAME));
                if (!msg.Status) return msg;
                msg.IFWhere(!(string.IsNullOrEmpty(entity.WORK_SHOP_BO)), string.Format("计划单号车间:{0}不能为空！", entity.WORK_SHOP_BONAME));
                if (!msg.Status) return msg;
                msg.IFWhere(!(string.IsNullOrEmpty(entity.PRODUCTION_LINE_BO)), string.Format("计划单号产线:{0}不能为空！", entity.PRODUCTION_LINE_BONAME));
                if (!msg.Status) return msg;
                msg.IFWhere(!(string.IsNullOrEmpty(entity.OPERATION_BO)), string.Format("计划单号工序:{0}不能为空！", entity.OPERATION_BONAME));
                if (!msg.Status) return msg;
                msg.IFWhere(!(string.IsNullOrEmpty(entity.ITEM_BO)), string.Format("计划单号物料:{0}不能为空！", entity.ITEM_BONAME));
                if (!msg.Status) return msg;

                string sql = String.Format("select * from Z_ITEM_DEMAND_SUM where HANDLE='{0}' ", entity.HANDLE);
                Z_ITEM_DEMAND_SUM entity_up = database.FindEntityBySql<Z_ITEM_DEMAND_SUM>(sql);
                if (entity.ITEM_BO.Length == 7)
                {
                    entity.ITEM_BO = $"00000000000{ entity.ITEM_BO }";
                }
                if (!string.IsNullOrEmpty(entity_up.ID))
                {
                    entity.Modify(entity_up.ID);
                    //if (entity.DEMAND_DATE.Length == 10)
                    //{
                    //    entity.DEMAND_DATE = string.Format("{0} 00:00:00", entity.DEMAND_DATE);
                    //}
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    string ModuleId = ConfigurationManager.AppSettings["SLMODELID"].Trim();
                    base_coderulebll.OccupyBillCode("System", ModuleId, isOpenTrans);

                    entity.ID = base_coderulebll.GetBillCode3F("System", ModuleId);
                    //if (entity.DEMAND_DATE.Length == 10)
                    //{
                    //    entity.DEMAND_DATE = string.Format("{0} 00:00:00", entity.DEMAND_DATE);
                    //}
                    database.Insert(entity, isOpenTrans);
                }
                database.Commit();
                msg.Msg = String.Format("编号{0}的数据保存成功！", entity.HANDLE);
                List<string> list = new List<string>();
                list.Add(entity.HANDLE);
                msg.REDATA = list;
            }
            catch (Exception ex)
            {
                database.Rollback();
                msg.IFWhere(false, String.Format("编号{0}的数据保存失败！", entity.HANDLE));
            }
            return msg;
        }

        /// <summary>
        /// 批量生成上料计划单
        /// </summary>
        /// <param name="Model">批量对象</param>
        /// <returns></returns>
        [WebMethod(Description = "批量生成上料计划单")]
        public StatusMsg SaveSLJHBatchOrder(EOS_SLJHORDER Model)
        {
            StatusMsg msg = new StatusMsg();
            msg.Create();
            //明细行数据
            List<Z_ITEM_DEMAND_SUM> OrderEntryList = Model.DETAILS;

            msg.IFWhere(!(OrderEntryList.Count == 0), string.Format("批量上料计划数据不能为空！"));
            if (!msg.Status) return msg;
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();

            try
            {
                List<string> str_list = new List<string>();
                Z_ITEM_DEMAND_SUM entity_up = null;
                string ModuleId = ConfigurationManager.AppSettings["SLMODELID"].Trim();
                base_coderulebll.OccupyBillCode("System", ModuleId, isOpenTrans);

                string ID = base_coderulebll.GetBillCode3F("System", ModuleId);
                int i = 1;
                foreach (Z_ITEM_DEMAND_SUM entity in OrderEntryList)
                {
                    if (entity.ITEM_BO.Length == 7)
                    {
                        entity.ITEM_BO = $"00000000000{ entity.ITEM_BO }";
                    }
                    str_list.Add(entity.HANDLE);
                    msg.IFWhere(!(string.IsNullOrEmpty(entity.HANDLE)), string.Format("计划单号:{0}不能为空！", entity.HANDLE));
                    if (!msg.Status) return msg;
                    //msg.IFWhere(!(string.IsNullOrEmpty(entity.SITE)), string.Format("计划单号站点:{0}不能为空！", entity.SITENAME));
                    //if (!msg.Status) return msg;
                    //msg.IFWhere(!(string.IsNullOrEmpty(entity.WORK_SHOP_BO)), string.Format("计划单号车间:{0}不能为空！", entity.WORK_SHOP_BONAME));
                    //if (!msg.Status) return msg;
                    //msg.IFWhere(!(string.IsNullOrEmpty(entity.PRODUCTION_LINE_BO)), string.Format("计划单号产线:{0}不能为空！", entity.PRODUCTION_LINE_BONAME));
                    //if (!msg.Status) return msg;
                    //msg.IFWhere(!(string.IsNullOrEmpty(entity.OPERATION_BO)), string.Format("计划单号工序:{0}不能为空！", entity.OPERATION_BONAME));
                    //if (!msg.Status) return msg;
                    //msg.IFWhere(!(string.IsNullOrEmpty(entity.ITEM_BO)), string.Format("计划单号物料:{0}不能为空！", entity.ITEM_BONAME));
                    //if (!msg.Status) return msg;
                    string sql = String.Format("select * from Z_ITEM_DEMAND_SUM where HANDLE='{0}' ", entity.HANDLE);
                    entity_up = database.FindEntityBySql<Z_ITEM_DEMAND_SUM>(sql);
                    if (!string.IsNullOrEmpty(entity_up.ID))
                    {
                        //if (entity_up.DEMAND_DATE.Length == 10)
                        //{
                        //    entity_up.DEMAND_DATE = string.Format("{0} 00:00:00", entity_up.DEMAND_DATE);
                        //}
                        entity.Modify(entity_up.ID);
                        database.Update(entity, isOpenTrans);
                    }
                    else
                    {
                        entity.Create();
                        entity.ID = string.Format("{0}{1:00}", ID, i);
                        entity.DATATYPE = "1";
                        //if (entity.DEMAND_DATE.Length == 10)
                        //{
                        //    entity.DEMAND_DATE = string.Format("{0} 00:00:00", entity.DEMAND_DATE);
                        //}
                        database.Insert(entity, isOpenTrans);
                    }
                    i++;
                }
                database.Commit();
                msg.Msg = String.Format("批量数据保存成功！");
                msg.REDATA = str_list;
            }
            catch (Exception ex)
            {
                database.Rollback();
                msg.IFWhere(false, String.Format("批量数据保存失败{0}！", ex.Message));
            }
            return msg;
        }


        /// <summary>
        /// 化工辅料扣重比例回写
        /// </summary>
        /// <param name="BILLNO">检验批号</param>
        /// <param name="KZAMOUNT">扣重比例</param>
        /// <param name="CZTYPE">处置方式</param>
        /// <returns></returns>
        [WebMethod(Description = "化工辅料扣重比例回写")]
        public StatusMsg UpdateKZBL(string BILLNO, string KZAMOUNT, string CZTYPE)
        {
            int res = 0;
            StatusMsg msg = new StatusMsg();
            msg.Create();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                msg.IFWhere(!(string.IsNullOrEmpty(BILLNO)), string.Format("检验计划:{0}不能为空！", BILLNO));
                if (!msg.Status) return msg;
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update DP_POCARSORDER_DTL set KZAMOUNT='{0}',CZTYPE='{1}' where ZJBILLNO='{2}'", KZAMOUNT, CZTYPE, BILLNO));
                database.ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                msg.IFWhere(true, String.Format("检验计划编号{0}的扣重比例保存成功！", BILLNO));
            }
            catch (Exception ex)
            {
                database.Rollback();
                msg.IFWhere(false, String.Format("检验计划编号{0}的扣重比例保存失败！", BILLNO));
            }
            return msg;
        }


        [WebMethod(Description = "上料计划执行情况")]
        public List<SLJHExecQk> GetSLJHList(string billno, string bdate, string edate)
        {
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"select c.HANDLE,count(1) CARSNUM,max(c.NET_DEMAND_CONFIRMED_QTY) as NET_DEMAND_CONFIRMED_QTY,sum(a.SUTTLE) as SUTTLE from wb_weight_xb a left join Z_ITEM_DEMAND_SUM_DTL b on a.PK_ORDER=b.ID left join  Z_ITEM_DEMAND_SUM c on b.MAINID=c.ID where c.datatype=1 and a.type>=1 and a.type<=2  ");
            if (!string.IsNullOrEmpty(billno))
            {
                strSql.Append($" and c.HANDLE='{billno}'");
            }
            if (!string.IsNullOrEmpty(bdate))
            {
                strSql.Append($" and a.WEIGHTDATE>='{bdate}'");
            }

            if (!string.IsNullOrEmpty(edate))
            {
                strSql.Append($" and a.WEIGHTDATE<='{edate}'");
            }
            strSql.Append("group by c.HANDLE");

            List<SLJHExecQk> list = DataFactory.Database().FindListBySql<SLJHExecQk>(strSql.ToString());
            return list;
        }

    }

    public class SLJHExecQk
    {
        public string HANDLE { get; set; }
        public string CARSNUM { get; set; }
        public string NET_DEMAND_CONFIRMED_QTY { get; set; }
        public string SUTTLE { get; set; }
    }
}
