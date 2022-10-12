using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    public class BillSOFCHECKController : PublicController<FORMTABLE_MAIN_335>
    {
        string ModuleId = "";

        FORMTABLE_MAIN_335Bll orderbll = new FORMTABLE_MAIN_335Bll();
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        /// <summary>
        /// 获取自动单据编码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }

        //列表
        [LoginAuthorize]
        public ActionResult BillSOFCHECKIndex()
        {
            return View();
        }



        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="BillNo">制单编号</param>
        /// <param name="StartTime">制单开始时间</param>
        /// <param name="EndTime">制单结束时间</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetOrderList(string BILLNO, string SQR, string SQBM, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<FORMTABLE_MAIN_335> ListData = orderbll.GetOrderList(BILLNO, SQR, SQBM, StartTime, EndTime, jqgridparam);
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
        /// 订单明细列表（返回Json）
        /// </summary>
        /// <param name="ID">订单主键</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = orderbll.GetOrderEntryList(KeyValue),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }


        //反馈
        [LoginAuthorize]
        public ActionResult OutOrder(string KeyValue)
        {
            //验证
            FORMTABLE_MAIN_335 entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该单据已反馈！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "单据反馈成功。";
                entity.STATUS = "6";
                entity.MEMO = "反馈成功";
                database.Update(entity, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }



        //反馈
        [LoginAuthorize]
        public ActionResult HandOrder(int Key, int KeyValue)
        {
            string str = "failed";
            
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                #region  //验证
                string sql = "";
                int res = 0;
                //string sql = String.Format("select count(1) from FORMTABLE_MAIN_335 where REQUESTID='{0}' and STATUS=1", KeyValue);
                //int res = DataFactory.Database().FindCountBySql(sql);
                //if (res > 0)
                //{
                //    return Content(new JsonMessage { Success = false, Code = "-1", Message = "该申请单已反馈,无法继续反馈操作！" }.ToString());
                //}

                sql = String.Format("select count(1) from DP_PSORDER_B where PK_ORDER_B in(select PZDH from FORMTABLE_MAIN_335_DT1 where MAINID='{0}')", Key);
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "该申请不满足条件，存在未删除的检斤单，请联系计量部删除检斤单后重新提交。！" }.ToString());
                }

                #endregion
                JsonMessage jsonmeg = null;


                #region  初始始化对象
                //派车明细表实例
                DP_POCARSORDER_DTL pcdtl_entity = null;
                //派车表实例
                DP_POCARSORDER pc_entity = null;
                //收货确认实例
                APP_HANDORDER handentity = null;
                //采样明细
                BD_PCRAWCYDETAILS cydtl_entity = null;
                //计量主表
                WB_WEIGHT weight = null;
                //计量明细
                WB_WEIGHT_B weight_b = null;

                EditWeightIn(Key, KeyValue, ref pcdtl_entity, ref pc_entity, ref handentity, ref cydtl_entity, ref weight, ref weight_b, database);
                #region 采样明细
                if (cydtl_entity != null)
                {
                    if (!string.IsNullOrEmpty(cydtl_entity.PCITEMID))
                    {
                        if (cydtl_entity.MATERIAL != pcdtl_entity.PK_MATERIAL)
                        {
                           
                            return Content(new JsonMessage { Success = false, Code = "-1", Message = "该申请不满足条件，存在未删除的采样单，请联系原料检验部删除采样单后重新提交。！" }.ToString());
                        }
                        if (cydtl_entity.MATERIAL == pcdtl_entity.PK_MATERIAL)
                        {
                            database.Update(cydtl_entity, isOpenTrans);
                        }
                    }
                }
                #endregion


                //收料表
                if (handentity != null)
                {
                    if (!string.IsNullOrEmpty(handentity.ID))
                    {
                        database.Update(handentity, isOpenTrans);
                    }
                }
                //派车主表
                if (pc_entity != null)
                {
                    if (!string.IsNullOrEmpty(pc_entity.ID))
                    {
                        database.Update(pc_entity, isOpenTrans);
                    }
                }

                #region 派车明细
                if (pcdtl_entity != null)
                {
                    if (!string.IsNullOrEmpty(pcdtl_entity.ID))
                    {
                        database.Update(pcdtl_entity, isOpenTrans);
                    }
                }
                #endregion



                #region 计量主表
                if (weight != null)
                {
                    if (!string.IsNullOrEmpty(weight.BILLNO))
                    {
                        database.Update(weight, isOpenTrans);
                    }
                }
                #endregion

                #region 计量明细表
                if (weight_b != null)
                {
                    if (!string.IsNullOrEmpty(weight_b.BILLNO))
                    {
                        database.Update(weight_b, isOpenTrans);
                    }
                }
                #endregion

                #endregion


                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update FORMTABLE_MAIN_335 set STATUS=1 where REQUESTID='{0}'", KeyValue));
                res += database.ExecuteBySql(strSql, isOpenTrans);
                database.Commit();
                if (res > 0)
                {
                   
                    string Message = "反馈成功。";
                    jsonmeg = new JsonMessage { Success = true, Code = "1", Message = string.Format("{0}", Message) };
                }
                else
                {
                    jsonmeg = new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}", "反馈失败：") };
                }
                return Content(jsonmeg.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = string.Format("{0}:{1}", "操作失败：", ex.Message) }.ToString());
            }
        }


        private void EditWeightIn(int Key, int KeyValue, ref DP_POCARSORDER_DTL pcdtl_entity, ref DP_POCARSORDER pc_entity, ref APP_HANDORDER handentity, ref BD_PCRAWCYDETAILS cydtl_entity, ref WB_WEIGHT weight, ref WB_WEIGHT_B weight_b, IDatabase database)
        {
            VNC_PO_ORDER_B po_order_b = null;
            List<VNC_PO_ORDER_B> po_list = null;
            BD_STORDOC store = null;

            List<FORMTABLE_MAIN_335_DT1> list = orderbll.GetOrderEntryList1(string.Format("{0}", Key));
            foreach (FORMTABLE_MAIN_335_DT1 model in list)
            {
                //派车明细
                pcdtl_entity = database.FindEntityByWhere<DP_POCARSORDER_DTL>(string.Format(" AND ID='{0}'", model.PZDH));
                //采样明细
                cydtl_entity = database.FindEntityByWhere<BD_PCRAWCYDETAILS>(string.Format(" AND PCITEMID='{0}'", model.PZDH));
                if (pcdtl_entity != null)
                {
                    //派车主表
                    pc_entity = database.FindEntityByWhere<DP_POCARSORDER>(string.Format(" AND ID='{0}'", pcdtl_entity.PK_DP_POCARSORDER));
                    //收料表 
                    handentity = database.FindEntityByWhere<APP_HANDORDER>(string.Format(" AND ID='{0}'", pcdtl_entity.ID));
                    //过磅主表
                    weight = database.FindEntityByWhere<WB_WEIGHT>(string.Format(" AND PK_ORDER='{0}'", pcdtl_entity.PK_DP_POCARSORDER));
                    //过磅明细表
                    if (weight != null)
                    {
                        if (weight.FINISH == "1")
                        {
                            weight_b = database.FindEntityByWhere<WB_WEIGHT_B>(string.Format(" AND PK_ORDER_B='{0}'", pcdtl_entity.ID));
                        }
                    }
                }
                if (model.XXGZD == "0")
                {
                    #region 订单编号
                    //订单修改后订单号值
                    po_list = database.FindListBySql<VNC_PO_ORDER_B>(string.Format("select * from VNC_PO_ORDER_B where VBILLCODE='{0}'", model.XXHXX));
                    if (po_list.Count > 0)
                    {
                        po_order_b = po_list[0];
                    }
                    #endregion
                }
                else if (model.XXGZD == "1")
                {
                    #region 入厂厂区
                    if (model.XXHXX == "兖州厂区")
                    {
                        if (weight_b != null)
                        {
                            weight_b.DEF11 = "10001";
                        }
                        if (weight != null)
                        {
                            weight.DEF11 = "10001";
                        }
                        if (pc_entity != null)
                        {
                            pc_entity.DEF1 = "10001";
                        }
                    }
                    else if (model.XXHXX == "邹城厂区")
                    {

                        if (weight_b != null)
                        {
                            weight_b.DEF11 = "10002";
                        }
                        if (weight != null)
                        {
                            weight.DEF11 = "10002";
                        }
                        if (pc_entity != null)
                        {
                            pc_entity.DEF1 = "10002";
                        }
                    }
                    else if (model.XXHXX == "兴隆厂区")
                    {

                        if (weight_b != null)
                        {
                            weight_b.DEF11 = "10003";
                        }
                        if (weight != null)
                        {
                            weight.DEF11 = "10003";
                        }
                        if (pc_entity != null)
                        {
                            pc_entity.DEF1 = "10003";
                        }
                    }
                    else if (model.XXHXX == "颜店厂区")
                    {
                        if (weight_b != null)
                        {
                            weight_b.DEF11 = "10004";
                        }
                        if (weight != null)
                        {
                            weight.DEF11 = "10004";
                        }
                        if (pc_entity != null)
                        {
                            pc_entity.DEF1 = "10004";
                        }
                    }
                    #endregion
                }
                else if (model.XXGZD == "2")
                {
                    //货主名称
                    pc_entity.DEF2 = model.XXHXX;
                }
                else if (model.XXGZD == "3")
                {
                    #region 卸货地址
                    store = database.FindEntityByWhere<BD_STORDOC>(string.Format(" AND NAME='{0}'", model.XXHXX));
                    if (store != null)
                    {
                        if (weight_b != null)
                        {
                            weight_b.PK_RECEIVESTORE = store.ID;
                            weight_b.RECEIVESTORE = store.NAME;
                            weight_b.PK_STORE = store.ID;
                            weight_b.STORE = store.NAME;
                        }
                        if (weight != null)
                        {
                            weight.PK_RECEIVESTORE = store.ID;
                            weight.RECEIVESTORE = store.NAME;
                            weight.PK_STORE = store.ID;
                            weight.STORE = store.NAME;
                        }
                        if (handentity != null)
                        {
                            handentity.PK_STORE = store.ID;
                            handentity.STORE = store.NAME;
                        }
                    }
                    #endregion
                }
            }
            #region 订单不为空执行更新
            if (po_order_b != null)
            {
                //派车主表
                if (pc_entity != null)
                {
                    if (!string.IsNullOrEmpty(pc_entity.ID))
                    {
                        pc_entity.PK_SUPPLIER = po_order_b.PK_SUPPLIER;
                        pc_entity.SUPPLIERNAME = po_order_b.PK_SUPPLIERNAME;
                    }
                }

                #region 派车明细
                if (pcdtl_entity != null)
                {
                    if (!string.IsNullOrEmpty(pcdtl_entity.ID))
                    {
                        pcdtl_entity.VBILLCODE = po_order_b.VBILLCODE;
                        pcdtl_entity.PK_ORDER = po_order_b.PK_ORDER;
                        pcdtl_entity.PK_ORDER_B = po_order_b.PK_ORDER_B;
                        pcdtl_entity.PK_SUPPLIER = po_order_b.PK_SUPPLIER;
                        pcdtl_entity.SUPPLIERNAME = po_order_b.PK_SUPPLIERNAME;
                        pcdtl_entity.PK_MATERIAL = po_order_b.PK_MATERIAL;
                        pcdtl_entity.MATERIALNAME = po_order_b.PK_MATERIALNAME;
                        pcdtl_entity.MATERIALSPEC = po_order_b.MATERIALSPEC;
                        pcdtl_entity.MATERIALCODE = po_order_b.MATERIALCODE;
                        pcdtl_entity.NASTNUM = po_order_b.NASTNUM;
                        pcdtl_entity.DEF1 = po_order_b.PURUNITNAME;
                        pcdtl_entity.DEF2 = po_order_b.SECPURUNITNAME;
                        pcdtl_entity.PK_ORG = po_order_b.RECCOMPANYID;
                        pcdtl_entity.ORGNAME = po_order_b.RECCOMPANYNAME;
                        pcdtl_entity.PK_DEPT = po_order_b.RECORGID;
                        pcdtl_entity.DEPTNAME = po_order_b.RECORGNAME;
                        pcdtl_entity.DBILLDATE = po_order_b.DBILLDATE;
                    }
                }
                #endregion

                #region 采样明细
                if (cydtl_entity != null)
                {
                    if (!string.IsNullOrEmpty(cydtl_entity.PCITEMID))
                    {
                        if (cydtl_entity.MATERIAL == pcdtl_entity.PK_MATERIAL)
                        {
                            cydtl_entity.SUPPLY = po_order_b.PK_SUPPLIER;
                            cydtl_entity.MATERIAL = po_order_b.PK_MATERIAL;
                            cydtl_entity.MATERIALSPEC = po_order_b.MATERIALSPEC;
                            cydtl_entity.DEF3 = po_order_b.VBILLCODE;
                            cydtl_entity.DEF4 = po_order_b.PK_ORDER;

                        }
                    }
                }
                #endregion

                #region 计量主表
                if (weight != null)
                {
                    if (!string.IsNullOrEmpty(weight.BILLNO))
                    {
                        weight.PK_BILL = po_order_b.PK_ORDER;
                        weight.PK_BILL_B = po_order_b.PK_ORDER_B;
                        weight.PK_CONTRACT = po_order_b.VBILLCODE;
                        weight.PK_CONTRACT_B = po_order_b.VBILLCODE;
                        weight.PK_SENDORG = po_order_b.PK_SUPPLIER;
                        weight.SENDORG = po_order_b.PK_SUPPLIERNAME;
                        weight.PK_MATERIAL = po_order_b.PK_MATERIAL;
                        weight.MATERIAL = po_order_b.PK_MATERIALNAME;
                        weight.MATERIALKIND = po_order_b.MATERIALSPEC;
                        weight.PK_RECEIVEORG = po_order_b.RECCOMPANYID;
                        weight.RECEIVEORG = po_order_b.RECCOMPANYNAME;

                    }
                }
                #endregion

                #region 计量明细表
                if (weight_b != null)
                {
                    if (!string.IsNullOrEmpty(weight_b.BILLNO))
                    {
                        weight_b.PK_BILL = po_order_b.PK_ORDER;
                        weight_b.PK_BILL_B = po_order_b.PK_ORDER_B;
                        weight_b.PK_CONTRACT = po_order_b.VBILLCODE;
                        weight_b.PK_CONTRACT_B = po_order_b.VBILLCODE;
                        weight_b.PK_SENDORG = po_order_b.PK_SUPPLIER;
                        weight_b.SENDORG = po_order_b.PK_SUPPLIERNAME;
                        weight_b.PK_MATERIAL = po_order_b.PK_MATERIAL;
                        weight_b.MATERIAL = po_order_b.PK_MATERIALNAME;
                        weight_b.MATERIALKIND = po_order_b.MATERIALSPEC;
                        weight_b.PK_RECEIVEORG = po_order_b.RECCOMPANYID;
                        weight_b.RECEIVEORG = po_order_b.RECCOMPANYNAME;
                    }
                }
                #endregion
            }
            #endregion


        }




        /// <summary>
        /// 【发货单】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SetFormControl(string KeyValue)
        {
            FORMTABLE_MAIN_335 entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }
    }
}
