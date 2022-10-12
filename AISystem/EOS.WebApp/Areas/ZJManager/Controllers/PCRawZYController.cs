using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using System.Data;
using Newtonsoft.Json.Linq;
using EOS.DataAccess;
using System.Data.Common;
using EOS.Repository;
using System.Text;

namespace EOS.WebApp.Areas.ZJManager.Controllers
{
    public class RandomHelper
    {
        /// <summary>
        ///生成制定位数的随机码（数字）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string GenerateRandomCode(int length)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var r = new Random(Guid.NewGuid().GetHashCode());
                result.Append(r.Next(0, 10));
            }
            return result.ToString();
        }
    }

    public class PCRawZYController : PublicController<BD_PCRAWCY>
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

        private string BillCode(string ModuleId)
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }

        //制样明细报表
        [LoginAuthorize]
        public ActionResult ZYReportIndex()
        {
            return View();
        }
        //采样记录报表
        [LoginAuthorize]
        public ActionResult ZJReportIndex()
        {
            return View();
        }
        #region 页面返回
        [LoginAuthorize]
        public ActionResult PCRawZYList()
        {
            ViewBag.FormID = "PCRawZYList";
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawZYList1()
        {
            ViewBag.FormID = "PCRawZYList1";
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawZYSelect()
        {
            return View();
        }
        [LoginAuthorize]
        public ActionResult SelectCYData()
        {
            ViewBag.FormID = "SelectCYData";
            ModuleId = Request["ModuleId"];
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawZYAddCY()
        {
            ViewBag.FormID = "PCRawZYAddCY";
            return View();
        }


        [LoginAuthorize]
        public ActionResult PCRawZYForm()
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
        public ActionResult PCRawZYForm1()
        {
            ViewBag.CreateUserName = ManageProvider.Provider.Current().Account;
            return View();
        }
        [LoginAuthorize]
        public ActionResult PCRawZYSJForm()
        {
            ViewBag.SYUSER = ManageProvider.Provider.Current().UserName;
            return View();
        }
        [LoginAuthorize]
        public ActionResult SelectJYCYData()
        {
            ViewBag.SYUSER = ManageProvider.Provider.Current().UserName;
            return View();
        }
        [LoginAuthorize]
        public ActionResult SelectJYCYSGData()
        {
            ViewBag.SYUSER = ManageProvider.Provider.Current().UserName;
            return View();
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// 获取采样组批制样列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPCRawZYList(string BILLNO, string HANDBILLNO, string PRINTCODE, string DEF6, string PK_ORG, string MATERIALNAME, string MATERIALSPEC, string ISSEND, string TYPE, string UPLOAD, string PCRAWTYPE, string ZYUSER, string CYTYPE, string CQDEF1, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCY> ListData = vpcbll.GetOrderListForZY(BILLNO, HANDBILLNO, PRINTCODE, DEF6, PK_ORG, MATERIALNAME, MATERIALSPEC, ISSEND, TYPE, UPLOAD, PCRAWTYPE, ZYUSER, CYTYPE, CQDEF1, StartTime, EndTime, jqgridparam);
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
        /// 制样新增页面 选择采样批次（已完成、未送检、未制样）
        /// </summary>
        /// <param name="BILLNO"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetPCRawZYSelectList(string BILLNO, string DEF6, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBD_PCRAWCY> ListData = vpcbll.GetOrderListForZYSelect(BILLNO, DEF6, StartTime, EndTime, jqgridparam);
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

        public ActionResult GetEntity(string KeyValue)
        {
            VBD_PCRAWCY entity = DataFactory.Database().FindEntity<VBD_PCRAWCY>(KeyValue);
            return Content(entity.ToJson());
        }

        public ActionResult GetEntityHY(string KeyValue)
        {
            VBD_PCRAWCY entity = vpcbll.GetEntityHY(KeyValue);
            return Content(entity.ToJson());
        }

        [LoginAuthorize]
        public ActionResult GetOrderEntryListHY(string Supply, string Material)
        {
            try
            {
                var JsonData = new
                {
                    rows = vpcbll.GetOrderEntryListHY(Supply, Material),
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
        /// 取消送检
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult UNSendCheck(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = pcbll.UNSendCheck(DATA);
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
        /// 送检
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CheckCYData(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = pcbll.SendCYData(DATA);
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

        //原煤组批送检
        public ActionResult CheckCYDataZP(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = pcbll.SendCYDataZP(DATA);
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
        /// 取消制样
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult UNCheck(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = pcbll.UNCheck(DATA);
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
        /// 送检匹配质检方案 多选方案 查询匹配列表 返回
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CheckCYDataSelect(string DATA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(DATA))
                {
                    message = pcbll.SendCYDataSelect(DATA);
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
        /// 送检自动匹配质检方案 保存
        /// </summary>
        /// <param name="PCRAWZJFA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult PCRawZJFASendSave(string PCRAWZJFA)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(PCRAWZJFA))
                {
                    message = pcbll.SendCYDataSave(PCRAWZJFA);
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
        /// 新增多选采样批次，制样操作
        /// </summary>
        /// <param name="DATA"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MutilZYSave(string KeyValue, string ModuleId)
        {
            try
            {
                this.ModuleId = ModuleId;
                IDatabase database = DataFactory.Database();
                DbTransaction isOpenTrans = database.BeginTrans();
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string BILLNO = this.BillCode();

                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, "408009a5-d0cc-4c10-8b1b-b570ccd3e675", isOpenTrans);
                    string SBILLNO = this.BillCode("408009a5-d0cc-4c10-8b1b-b570ccd3e675");
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, "91f34372-948c-4fc1-b207-325c9a30935b", isOpenTrans);
                    string MYBILLNO = this.BillCode("91f34372-948c-4fc1-b207-325c9a30935b");
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, "3dd03694-3945-426d-a863-f8fc43f4c3e2", isOpenTrans);
                    string YBILLNO = this.BillCode("3dd03694-3945-426d-a863-f8fc43f4c3e2");
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, "c72602ef-ec40-46cc-b21f-f9089db7b0ba", isOpenTrans);
                    string GBILLNO = this.BillCode("c72602ef-ec40-46cc-b21f-f9089db7b0ba");
                    //制样
                    message = pcbll.MutilZYSave(KeyValue, BILLNO, SBILLNO, MYBILLNO, YBILLNO, GBILLNO);

                    #region 传质检
                    int success = 1;
                    System.Text.StringBuilder strSql = new System.Text.StringBuilder();

                    List<VBD_PCRAWCY> entity_list = vpcbll.GetOrderList(KeyValue);
                    foreach (VBD_PCRAWCY entity in entity_list)
                    {
                        success = 1;
                        List<VBD_PCRAWCYDETAILS> list = vpcbll.GetOrderEntryList(entity.ID);
                        string zj_msg = "";
                        if (entity.SUMKZ == 1)
                        {
                            #region 颗粒灰
                            foreach (VBD_PCRAWCYDETAILS entity_details in list)
                            {
                                if (!string.IsNullOrEmpty(entity_details.DEF5))
                                {
                                    JObject model = new JObject();
                                    model.Add("jyp", entity_details.DEF5);//检验批
                                    model.Add("qym", entity.BILLNO);//取样码
                                    model.Add("zym", entity.PRINTCODE);//制样码
                                    model.Add("wlmc", entity.MATERIALNAME);//物料名称
                                    model.Add("wlbm", entity.MATERIAL);//物料编号
                                    model.Add("gys", entity.SUPPLYNAME);//供应商
                                    model.Add("usern", ManageProvider.Provider.Current().Account);//登录用户
                                    model.Add("rq", entity.CYDATE);//日期
                                    model.Add("qy_user", entity.CREUSER);//取样人1
                                    model.Add("qy_user2", entity.CREUSER);//取样人2
                                    model.Add("Gz", entity.GBILLNO);//G值 
                                    model.Add("Yz", entity.YBILLNO);//Y值 
                                    model.Add("sym", entity.SBILLNO);//水码 
                                    model.Add("Yjyp", entity.DEF5);//原检验批
                                    model.Add("jypxh", entity_details.DEF6);//检验批序号
                                    model.Add("my", entity.ISMYM);//煤岩


                                }
                            }
                            #endregion
                        }
                        else
                        {
                            #region 其它原料
                            var groupby = list.GroupBy(c => new { c.SUPPLYNAME, c.MATERIALNAME, c.DEF5 })
                               .Select(c => new
                               {
                                   SUPPLYNAME = c.Key.SUPPLYNAME,
                                   MATERIALNAME = c.Key.MATERIALNAME,
                                   DEF5 = c.Key.DEF5,
                                   count = c.Count()
                               });
                            foreach (var item in groupby)
                            {
                                if (!string.IsNullOrEmpty(item.DEF5))
                                {
                                    JObject model = new JObject();
                                    model.Add("jyp", item.DEF5);//检验批
                                    model.Add("qym", entity.BILLNO);//取样码
                                    model.Add("zym", entity.PRINTCODE);//制样码
                                    model.Add("wlmc", entity.MATERIALNAME);//物料名称
                                    model.Add("wlbm", entity.MATERIAL);//物料编号
                                    model.Add("gys", entity.SUPPLYNAME);//供应商
                                    model.Add("usern", ManageProvider.Provider.Current().Account);//登录用户
                                    model.Add("rq", entity.CYDATE);//日期
                                    model.Add("qy_user", entity.CREUSER);//取样人1
                                    model.Add("qy_user2", entity.CREUSER);//取样人2
                                    model.Add("Gz", entity.GBILLNO);//G值 
                                    model.Add("Yz", entity.YBILLNO);//Y值 
                                    model.Add("sym", entity.SBILLNO);//水码 
                                    model.Add("Yjyp", "");//原检验批
                                    model.Add("jypxh", 0);//检验批序号
                                    model.Add("my", entity.ISMYM);//煤岩

                                }
                            }
                            #endregion
                        }



                        if (zj_msg.IndexOf("success") >= 0)
                        {
                            strSql.Clear();
                            strSql.Append(string.Format("update BD_PCRAWCY set UPLOAD=1 where ID='{1}'", 1, entity.ID));
                            repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                        }
                    }
                    #endregion

                    database.Commit();
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
        /// 鑫跃制样并推质检
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="JYUSER"></param>
        /// <param name="JYUSER1"></param>
        /// <param name="SYUSER"></param>
        /// <param name="SYUSER1"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MutilZYSaveFinish(string KeyValue, string JYUSER, string JYUSER1, string SYUSER, string SYUSER1, string ModuleId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "操作成功。";
                this.ModuleId = ModuleId;
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    #region 验证
                    BD_PCRAWCY editentity = repositoryfactory.Repository().FindEntity(KeyValue);
                    if (editentity.ISZY == "1")
                    {
                        string msg = string.Format("采样单号:{0}已制样!", editentity.BILLNO);
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = msg }.ToString());
                    }
                    #endregion



                    #region 更新采样单
                    editentity.Modify(KeyValue);
                    editentity.ISZY = "1";
                    editentity.TYPE = "4";

                    editentity.JYUSER = JYUSER;
                    editentity.JYUSER1 = JYUSER1;
                    editentity.SYUSER = SYUSER;
                    editentity.SYUSER1 = SYUSER1;
                    string JYDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    editentity.JYDATE = JYDATE;
                    editentity.JYDATE = JYDATE;
                    editentity.DEF14 = ManageProvider.Provider.Current().UserName;
                    editentity.DEF15 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    database.Update(editentity, isOpenTrans);
                    database.Commit();
                    #endregion
                    return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中采样单号！" }.ToString());
                }
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog("", OperationType.Add, "-1", "原料制样异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 总厂制样并推质检
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <param name="JYUSER"></param>
        /// <param name="JYUSER1"></param>
        /// <param name="SYUSER"></param>
        /// <param name="SYUSER1"></param>
        /// <param name="ModuleId"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult MutilZYSaveFinish1(string KeyValue, string JYUSER, string JYUSER1, string SYUSER, string SYUSER1, string DEF14, string ModuleId)
        {
            if (string.IsNullOrEmpty(DEF14))
            {
                DEF14 = ManageProvider.Provider.Current().UserName;
            }
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "操作成功。";
                this.ModuleId = ModuleId;
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    BD_PCRAWCY editentity = repositoryfactory.Repository().FindEntity(KeyValue);
                    if (editentity.ISZY == "1")
                    {
                        string msg = string.Format("采样单号:{0}已制样!", editentity.BILLNO);
                        return Content(new JsonMessage { Success = false, Code = "-1", Message = msg }.ToString());
                    }
                    WB_MATERIAL_JYPBll jyp_bll = new WB_MATERIAL_JYPBll();
                    WB_MATERIAL_JYP jyp_entity = jyp_bll.GetEntity(editentity.MATERIAL);
                    string BILLNO = "", SBILLNO = "", MYBILLNO = "", YBILLNO = "", GBILLNO = "";


                    #region 更新采样单
                    editentity.Modify(KeyValue);
                    editentity.ISZY = "1";//是否制样
                    editentity.TYPE = "4";//制样
                    editentity.ISSEND = "0";//送检状态
                    editentity.JYUSER = JYUSER;//交样人甲
                    editentity.JYUSER1 = JYUSER1;
                    editentity.SYUSER = SYUSER;//接样人甲
                    editentity.SYUSER1 = SYUSER1;
                    string JYDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    editentity.JYDATE = JYDATE;//交样时间
                    editentity.DEF14 = DEF14;//制样人 ManageProvider.Provider.Current().UserName;
                    editentity.DEF15 = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//制样时间
                    database.Update(editentity, isOpenTrans);
                    database.Commit();
                    #endregion

                    return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());

                }
                else
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "未选中采样单号！" }.ToString());
                }
            }
            catch (Exception ex)
            {
                database.Rollback();
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "原料制样异常错误：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "原料制样异常错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 递归生码
        /// </summary>
        /// <param name="BILLNO"></param>
        /// <param name="type">1 水码，2 Y码/岩码，3 G码，4 总厂制样码</param>
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
            int res = DataFactory.Database().FindCountBySql(sql);
            if (res > 0)
            {
                BILLNO = SetBillNo(type, strformat);
                BILLNO = CreateBillNo(BILLNO, type, strformat);
            }
            return BILLNO;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="type">1 水码，2 Y码/岩码，3 G码，4 总厂制样码</param>
        /// <returns></returns>
        public string SetBillNo(int type)
        {
            string billno = "";
            Random r = new Random();
            int num = 1;
            if (type == 1)
            {
                num = r.Next(301, 401);
                billno = string.Format("SM{0}{1}", DateTime.Now.ToString("yyMMdd"), num);
            }
            else if (type == 2)
            {
                num = r.Next(501, 601);
                billno = string.Format("YM{0}{1}", DateTime.Now.ToString("yyMMdd"), num);
            }
            else if (type == 3)
            {
                num = r.Next(701, 801);
                billno = string.Format("GM{0}{1}", DateTime.Now.ToString("yyMMdd"), num);
            }
            else if (type == 4)
            {
                num = r.Next(101, 601);

                billno = string.Format("ZY{0}{1}", DateTime.Now.ToString("yyMMdd"), num);
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
                num = r.Next(301, 401);
                billno = string.Format("SM{0}{1}", strformat, num);
            }
            else if (type == 2)
            {
                num = r.Next(501, 601);
                billno = string.Format("YM{0}{1}", strformat, num);
            }
            else if (type == 3)
            {
                num = r.Next(701, 801);
                billno = string.Format("GM{0}{1}", strformat, num);
            }
            else if (type == 4)
            {
                num = r.Next(101, 601);
                billno = string.Format("ZY{0}{1}", strformat, num);
            }
            return billno;
        }


        #endregion

        [LoginAuthorize]
        public ActionResult PCRawZYSave(BD_PCRAWCY entity, string KeyValue, string ModuleId)
        {
            #region 验证

            #endregion
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                //处理主头
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    BD_PCRAWCY edit_entity = repositoryfactory.Repository().FindEntity(KeyValue);
                    entity.Modify(KeyValue);
                    edit_entity.MEMO = entity.MEMO;
                    database.Update(edit_entity, isOpenTrans);
                }

                string Account = ManageProvider.Provider.Current().Account;
                string message = string.Format("制样单号:{0}，编辑成功。", entity.PRINTCODE);
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

        #region 鑫跃打印制样码

        public ActionResult ZYPrintCode(string KeyValue)
        {
            VBD_PCRAWCYPRINTBll orderbll = new VBD_PCRAWCYPRINTBll();
            DataSet dataSet = orderbll.ZYPrintCode(KeyValue, "1");
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, false);
            }
            return View();
        }

        public ActionResult BDZYPrintCode(string Parm_Key_Value)
        {
            VBD_PCRAWCYPRINTBll orderbll = new VBD_PCRAWCYPRINTBll();
            DataSet dataSet = orderbll.BDZYPrintCode(Parm_Key_Value, "1");
            if (dataSet.Tables[0].Rows.Count > 0)
            {
                XMLReportData.GenDetailData(this.Response, dataSet, false);
            }
            return View();
        }


        #endregion

        #region 总厂打印制样码



        public ActionResult BDZYPrintCode1(string Parm_Key_Value)
        {
            VBD_PCRAWCYPRINTBll orderbll = new VBD_PCRAWCYPRINTBll();
            DataSet dataSet = orderbll.BDZYPrintCode(Parm_Key_Value, "2");
            return Content(dataSet.GetXml());
        }

        public ActionResult ZYPrintCode1(string KeyValue)
        {
            VBD_PCRAWCYPRINTBll orderbll = new VBD_PCRAWCYPRINTBll();
            DataSet dataSet = orderbll.ZYPrintCode(KeyValue, "2");
            return Content(dataSet.GetXml());
        }
        #endregion

        public ActionResult ZYDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = vpcbll.ZYDataSetReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }


        //采样记录
        public ActionResult ZJDataSetReport(string Parm_Key_Value)
        {
            DataSet dataSet = vpcbll.ZJDataSetReport(Parm_Key_Value);
            return Content(dataSet.GetXml());
        }
    }
}
