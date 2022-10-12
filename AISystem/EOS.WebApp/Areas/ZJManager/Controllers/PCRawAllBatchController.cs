using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using EOS.Utilities;
using System.Diagnostics;
using EOS.Business;
using EOS.Repository;
using EOS.DataAccess;
using System.Data.Common;

namespace EOS.WebApp.Areas.ZJManager.Controllers
{
    public class PCRawAllBatchController : PublicController<BD_PCRAWCY>
    {
        string ModuleId = "";

        BD_PCRAWCYBll pcbll = new BD_PCRAWCYBll();
        VBD_PCRAWCYBll vpcbll = new VBD_PCRAWCYBll();
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


        #region 页面返回
        public ActionResult PCRawAllBatchList()
        {
            return View();
        }
        public ActionResult PCRawAllBatchList1()
        {
            return View();
        }
        public ActionResult PCRawAllBatchAdd()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.BillNo = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().Account;
            }
            return View();
        }


        [LoginAuthorize]
        public ActionResult PCRawAllDeleteCar()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawAllAddCar()
        {
            return View();
        }
        public ActionResult PCRawAllBatchSelect()
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
            VBD_PCRAWCY entity = DataFactory.Database().FindEntity<VBD_PCRAWCY>(KeyValue);
            return Content(entity.ToJson());
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// List页面获取组大批列表
        /// </summary>
        /// <param name="BillNo"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetPCRawBatchList(string BillNo, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCY> ListData = vpcbll.GetPCRawBatchList(BillNo, StartTime, EndTime, "Normal", jqgridparam);
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
        /// List页面获取组大批明细批次列表
        /// </summary>
        /// <param name="MAINID"></param>
        /// <returns></returns>
        public ActionResult GetPCRawBatchListItem(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCYDETAILS> ListData = vpcbll.GetOrderListItem(MAINID, jqgridparam);
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
        /// 多选组大批列表
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="MATERIAL"></param>
        /// <param name="SUPPLY"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetPCRawCYList(string BILLNO, string PRINTCODE, string MATERIAL, string SUPPLY, string MATERIALNAME, string SUPPLYNAME, string TYPE, string PCRAWTYPE, string ISSEND, string DEF6, string iszp, string CYBUCKETNAME, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCY> ListData = vpcbll.GetPCRawCYBatchList(BILLNO, PRINTCODE, MATERIAL, SUPPLY, MATERIALNAME, SUPPLYNAME, TYPE, PCRAWTYPE, ISSEND, DEF6, iszp, CYBUCKETNAME, StartTime, EndTime, jqgridparam);
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




        public ActionResult GetPCRawBatchCYListItem(string MAINID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCY> ListData = vpcbll.GetOrderCYListItem(MAINID, jqgridparam);
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
        /// 获取选中的批次信息，通过批次主键ID
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult PCRawCYGetBatchData(string KeyValue)
        {
            try
            {
                List<string> strresult = new List<string>();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    return Content(pcbll.PCRawCYGetBatchData(KeyValue));
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
        /// 移除组批车辆明细列表
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetPCRawAllDeleteCar(string KeyValue, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCYDETAILS> ListData = vpcbll.GetOrderListItem(KeyValue, jqgridparam);
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
        /// 删除组批明细车号
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="KeyList"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawAllDeleteItem(string KeyValue, string KeyList, string BillNoList, string YMAINID, string YMAINBILLNO)
        {
            try
            {
                ResultMessage message = pcbll.PCRawAllDeleteItem(KeyValue, KeyList, BillNoList, YMAINID, YMAINBILLNO);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 移入 通知单数据移入到指定批次
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="TASKLIST"></param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawAllAddCarItem(string KeyValue, string BILLNO)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    message = pcbll.PCRawAllAddCarItem(KeyValue, BILLNO);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：未选中采样单号,请重新选择" }.ToString());
                }
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 表单操作
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual ActionResult DeleteItem1(string KeyValue)
        {
            try
            {
                ResultMessage message = pcbll.PCRawCYDelete(KeyValue);

                WriteLog(message.Code, KeyValue, message.Message);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public ActionResult DeleteItem(string KeyValue, string ZYBUCKET)
        {
            string bills = KeyValue.Replace(",", "','");
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCY set TYPE=1,ISSEND=0 where ID in(select YMAINID from BD_PCRAWCYDETAILS where MAINID in('{0}'))", bills));
                database.ExecuteBySql(strSql, isOpenTrans);
                strSql.Clear();
                strSql.Append(string.Format("update BD_PCRAWCYDETAILS set isshow=0 where MAINID in(select YMAINID from BD_PCRAWCYDETAILS where MAINID in('{0}'))", bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("delete BD_PCRAWCY where ID in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("delete BD_PCRAWCYDETAILS where MAINID in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);

                strSql.Clear();
                strSql.Append(string.Format("delete BD_ZJZY where PCRAWID in('{0}')", bills));
                database.ExecuteBySql(strSql, isOpenTrans);
                if (!string.IsNullOrEmpty(ZYBUCKET))
                {
                    string bills1 = ZYBUCKET.Replace(",", "','");
                    strSql.Clear();
                    strSql.Append(string.Format("update BD_ZYBUCKET set type=0 where ID in('{0}')", bills1));
                    database.ExecuteBySql(strSql, isOpenTrans);
                }
                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("采样单删除成功。");
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 表单提交
        /// <summary>
        /// 组大批表单提交
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <param name="PCRAWCYDetailJson"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawAllBatchSave(BD_PCRAWCY entity, string KeyValue, string PCRAWCYDetailJson)
        {
            int IsOk = 0;
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    //编辑
                    IsOk = vpcbll.PCRawBATCHCYEDITSave(entity, KeyValue, PCRAWCYDetailJson);
                    if (IsOk == 2)
                    {
                        return Content(new JsonMessage { Success = false, Code = "0", Message = "当前批号已存在,请重新输入批号" }.ToString());
                    }
                    else if (IsOk == 3)
                    {
                        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "当前批次内的下属批次不是来源于同一订单，无法组批" }.ToString());
                    }
                    else
                    {
                        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
                    }
                }
                else
                {
                    //新增
                    IsOk = vpcbll.PCRawBATCHCYSave(entity, PCRAWCYDetailJson);
                    if (IsOk == 2)
                    {
                        return Content(new JsonMessage { Success = false, Code = "0", Message = "当前批号已存在,请重新输入批号" }.ToString());
                    }
                    else if (IsOk == 3)
                    {
                        return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "当前批次内的下属批次不是来源于同一订单，无法组批" }.ToString());
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



        public ActionResult PCRawAllBatchSave1(BD_PCRAWCY entity, string KeyValue, string PCRAWCYDetailJson, string ModuleId)
        {
            #region 验证

            #endregion

            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //明细行数据
                List<BD_PCRAWCYDETAILS> OrderEntryList = PCRAWCYDetailJson.JonsToList<BD_PCRAWCYDETAILS>();
                WB_MATERIAL_JYPBll jyp_bll = new WB_MATERIAL_JYPBll();
                WB_MATERIAL_JYP jyp_entity = jyp_bll.GetEntity(entity.MATERIAL);

                string CODEDATE = DateTime.Now.ToString("yyMMdd");

                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    strSql.Clear();
                    strSql.Append(string.Format("update BD_PCRAWCY set TYPE=1,ISSEND=0 where ID in(select YMAINID from BD_PCRAWCYDETAILS where MAINID='{0}')", KeyValue));
                    repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                    database.Delete<BD_PCRAWCYDETAILS>("MAINID", KeyValue, isOpenTrans);
                    entity.Modify(KeyValue);
                    entity.ISZP = 1;
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from BD_PCRAWCY where  BILLNO='{0}'", entity.BILLNO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.BILLNO = this.BillCode();
                    }
                    entity.Create();
                    entity.ISZP = 1;
                    entity.ISZK = "1";

                    //是否明码
                    if (jyp_entity.ISMM == "1")
                    {
                        entity.HANDBILLNO = entity.BILLNO;
                        entity.PRINTCODE = entity.BILLNO;
                        entity.SBILLNO = entity.BILLNO;
                        entity.MYBILLNO = entity.BILLNO;
                        entity.YBILLNO = entity.BILLNO;
                        entity.GBILLNO = entity.BILLNO;
                    }
                    else
                    {
                        string HANDBILLNO = "", PRINTCODE = "";
                        HANDBILLNO = CreateBillNo("", 5);//取样码
                        PRINTCODE = CreateBillNo("", 4);//制样码
                        if (!string.IsNullOrEmpty(jyp_entity.BIRTHCODE))
                        {
                            entity.HANDBILLNO = string.Format("Q{0}{1}", jyp_entity.BIRTHCODE, HANDBILLNO.Substring(2));
                            entity.PRINTCODE = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            entity.SBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            entity.MYBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            entity.YBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                            entity.GBILLNO = string.Format("Z{0}{1}", jyp_entity.BIRTHCODE, PRINTCODE.Substring(2));
                        }
                        else
                        {
                            entity.HANDBILLNO = HANDBILLNO;
                            entity.PRINTCODE = PRINTCODE;
                            entity.SBILLNO = PRINTCODE;
                            entity.MYBILLNO = PRINTCODE;
                            entity.YBILLNO = PRINTCODE;
                            entity.GBILLNO = PRINTCODE;
                        }
                    }
                    sql = String.Format("select count(1) from BD_PCRAWCY where  HANDBILLNO='{0}'", entity.HANDBILLNO);
                    res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = "组批单号已存在,请您重新保存操作！" }.ToString());
                    }
                    database.Insert(entity, isOpenTrans);
                }

                //处理明细
                int index = 1;
                foreach (BD_PCRAWCYDETAILS orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.PCID))
                    {
                        //采样明细
                        orderentry.Create();
                        orderentry.MAINID = entity.ID;
                        orderentry.MAINBILLNO = entity.BILLNO;
                        orderentry.MEMO = orderentry.MEMO.Replace("null", "");
                        database.Insert(orderentry, isOpenTrans);
                        //更新为不显示
                        strSql.Clear();
                        strSql.Append(string.Format("update BD_PCRAWCYDETAILS set isshow=1 where MAINID='{0}' and PCITEMID='{1}' ", orderentry.YMAINID, orderentry.PCITEMID));
                        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                        index++;
                    }
                }

                var TotalData = OrderEntryList.GroupBy(x => new { x.YMAINID }).Select(y => new
                {
                    YMAINID = y.Key.YMAINID
                });
                foreach (var item in TotalData)
                {
                    if (!string.IsNullOrEmpty(item.YMAINID))
                    {
                        strSql.Clear();
                        strSql.Append(string.Format("update BD_PCRAWCY set TYPE=3,ISSEND=1 where ID='{0}'", item.YMAINID));
                        repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                    }
                }

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("组批单号:{0}，新增成功。", entity.BILLNO);
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message, BillNo = entity.BILLNO }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 递归生码
        /// </summary>
        /// <param name="BILLNO"></param>
        /// <param name="type">1 水码，2 Y码/岩码，3 G码，4 总厂制样码，5 总厂取样码</param>
        /// <returns></returns>
        private string CreateBillNo(string BILLNO, int type)
        {
            if (string.IsNullOrEmpty(BILLNO))
            {
                BILLNO = SetBillNo(type);
            }
            string sql = "";
            if (type == 1)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where SBILLNO='{0}'", BILLNO);
            }
            else if (type == 2)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where YBILLNO='{0}'", BILLNO);
            }
            else if (type == 3)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where GBILLNO='{0}'", BILLNO);
            }
            else if (type == 4)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where PRINTCODE='{0}'", BILLNO);
            }
            else if (type == 5)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where HANDBILLNO='{0}'", BILLNO);
            }
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                BILLNO = SetBillNo(type);
                BILLNO = CreateBillNo(BILLNO, type);
            }
            return BILLNO;
        }

        private string CreateBillNo(string BILLNO, int type, string strformat)
        {
            if (string.IsNullOrEmpty(BILLNO))
            {
                BILLNO = SetBillNo(type, strformat);
            }
            string sql = "";
            if (type == 1)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where SBILLNO='{0}'", BILLNO);
            }
            else if (type == 2)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where YBILLNO='{0}'", BILLNO);
            }
            else if (type == 3)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where GBILLNO='{0}'", BILLNO);
            }
            else if (type == 4)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where PRINTCODE='{0}'", BILLNO);
            }
            else if (type == 5)
            {
                sql = String.Format("select count(1) from BD_PCRAWCY where HANDBILLNO='{0}'", BILLNO);
            }
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                BILLNO = SetBillNo(type);
                BILLNO = CreateBillNo(BILLNO, type, strformat);
            }
            return BILLNO;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">1 水码，2 Y码，3 G码，4 制样码，5 取样码</param>
        /// <returns></returns>
        public string SetBillNo(int type)
        {
            string billno = "";
            Random r = new Random();
            int num = 1;
            if (type == 1)
            {
                num = r.Next(100000, 200000);
                billno = string.Format("SM{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), num);
            }
            else if (type == 2)
            {
                num = r.Next(200001, 300001);
                billno = string.Format("YM{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), num);
            }
            else if (type == 3)
            {
                num = r.Next(300002, 400002);
                billno = string.Format("GM{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), num);
            }
            else if (type == 4)
            {
                num = r.Next(400003, 500003);
                billno = string.Format("ZY{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), num);
            }
            else if (type == 5)
            {
                num = r.Next(500004, 600004);
                billno = string.Format("CY{0}{1}", DateTime.Now.ToString("yyMMddHHmmss"), num);
            }
            return billno;
        }


        public string SetBillNo(int type, string strformat)
        {
            string billno = "";
            Random r = new Random();
            int num = 1;
            if (type == 1)
            {
                num = r.Next(100, 200);
                billno = string.Format("SM{0}{1}", strformat, num);
            }
            else if (type == 2)
            {
                num = r.Next(201, 301);
                billno = string.Format("YM{0}{1}", strformat, num);
            }
            else if (type == 3)
            {
                num = r.Next(302, 402);
                billno = string.Format("GM{0}{1}", strformat, num);
            }
            else if (type == 4)
            {
                num = r.Next(403, 503);
                billno = string.Format("ZY{0}{1}", strformat, num);
            }
            else if (type == 5)
            {
                num = r.Next(504, 604);
                billno = string.Format("CY{0}{1}", strformat, num);
            }
            return billno;
        }
        #endregion
    }
}
