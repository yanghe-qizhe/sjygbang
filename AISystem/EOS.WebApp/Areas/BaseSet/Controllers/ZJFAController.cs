using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using EOS.DataAccess;
using EOS.Repository;
using System.Data.Common;
using System.Collections;
using EOS.Entity;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class ZJFAController : PublicController<Base_ZJFA>
    {
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        VBase_ZJFABll base_zjfabll = new VBase_ZJFABll();
        VBase_ZJFAItemBll base_zjfaitembll = new VBase_ZJFAItemBll();
        IDatabase database = DataFactory.Database();
        string ModuleId = "";


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
        public ActionResult ZJFAForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ViewBag.FANO = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }

        [LoginAuthorize]
        public ActionResult ZJFAIndex()
        {
            return View();
        }
        public ActionResult ZJFAProjectList()
        {
            return View();
        }

        public ActionResult ZJFAProjectAdd()
        {
            ViewBag.FormID = "ZJFAProjectAdd";
            return View();
        }
        public ActionResult ZJFAProjectAddPaste()
        {
            ViewBag.FormID = "ZJFAProjectAddPaste";
            return View();
        }

        public ActionResult ZJFAProjectEdit()
        {
            return View();
        }
        public ActionResult ZJFAMaterialType()
        {
            return View();
        }
        public ActionResult ZJFAProjectEditAll()
        {
            ViewBag.FormID = "ZJFAProjectEditAll";
            return View();
        }
        /// <summary>
        /// 重写 质检方案 编辑页面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public override ActionResult SetForm(string KeyValue)
        {
            VBase_ZJFA entity = DataFactory.Database().FindEntity<VBase_ZJFA>(KeyValue);
            return Content(entity.ToJson());
        }

        /// <summary>
        /// 质检方案项目 编辑页面
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormForFAITEM(string KeyValue)
        {
            VBase_ZJFAItem entity = DataFactory.Database().FindEntity<VBase_ZJFAItem>(KeyValue);
            return Content(entity.ToJson());
        }

        /// <summary>
        /// 获取复制过来的质检项目
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJItemModel(string KeyValue)
        {
            List<VBase_ZJFAItem> model = base_zjfaitembll.GetZJFAItemList(KeyValue);
            return Content(model.ToJson());
        }

        #endregion

        #region 数据列表
        /// <summary>
        /// 质检方案列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="Checked"></param>
        /// <param name="Type"></param>
        /// <param name="MaterialName"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJFAList(string NAME, string StartTime, string EndTime, string Checked, string Type, string MaterialName, string DEF7, string DEF8, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBase_ZJFA> ListData = base_zjfabll.GetOrderList(NAME, StartTime, EndTime, Checked, Type, MaterialName, DEF7, DEF8, jqgridparam);
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
        /// 质检方案 质检项 数据列表
        /// </summary>
        /// <param name="FANO"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJFAProjectList(string FANO, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBase_ZJFAItem> ListData = base_zjfaitembll.GetZJFAProjectList(FANO, jqgridparam);
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
        /// 质检方案提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SubmitOrderForm(Base_ZJFA entity, string KeyValue, string ModuleId)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                Base_SysLog log = new Base_SysLog();

                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    //编辑
                    log.PRECHANGE_M = database.FindEntity<Base_ZJFA>(KeyValue).ToJson();

                    entity.Modify(KeyValue);
                    entity.SPELLCODE = PinyinHelper.PinyinString(entity.FANAME);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    //新增
                    entity.Create();
                    base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                    string sql = String.Format("select count(1) from Base_ZJFA where  FANO='{0}'", entity.FANO);
                    int res = DataFactory.Database().FindCountBySql(sql);
                    if (res > 0)
                    {
                        this.ModuleId = ModuleId;
                        entity.FANO = this.BillCode();
                    }
                    entity.SPELLCODE = PinyinHelper.PinyinString(entity.FANAME);
                    database.Insert(entity, isOpenTrans);

                }
                database.Commit();

                log.AFTERCHANGE_M = entity.ToJson();
                log.LOGCONTENT = KeyValue == "" ? "新增质检方案。" : "编辑质检方案。";
                log.Remark = log.LOGCONTENT;
                VBase_ZJFA mkeyword = database.FindEntity<VBase_ZJFA>(entity.FANO);
                log.KeyWord = mkeyword.ToJson();

                Base_SysLogBll.Instance.WriteLogContent<Base_ZJFA>(entity, log, KeyValue == "" ? OperationType.Add : OperationType.Update, "1", "", false);

                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 质检方案 质检项新增提交
        /// </summary>
        /// <param name="KeyValue">方案编号</param>
        /// <param name="TYPE">方案类型</param>
        /// <param name="ENABLED">是否有效 1 有效 0无效</param>
        /// <param name="ZJFAProjectDetailJson">方案明细 质检项</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJFAProjectSave(string FANO, string TYPE, string ENABLED, string ZJFAProjectDetailJson)
        {
            try
            {
                int IsOk = 0;
                IsOk = base_zjfaitembll.ZJFAProjectSave(FANO, TYPE, ENABLED, ZJFAProjectDetailJson);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 质检方案 质检项编辑
        /// </summary>
        /// <param name="KeyValue">方案编号</param>
        /// <param name="TYPE">方案类型</param> 
        /// <param name="ZJFAProjectDetailJson">方案明细 质检项</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJFAProjectEditAllSave(string FANO, string TYPE, string ZJFAProjectDetailJson)
        {
            try
            {
                int IsOk = 0;
                IsOk = base_zjfaitembll.ZJFAProjectEditAllSave(FANO, TYPE, ZJFAProjectDetailJson);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "操作成功。" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }


        /// <summary>
        /// 质检方案项目 编辑 提交
        /// </summary>
        /// <param name="KeyValue">质检方案项目 ID </param> 
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ZJFAITEMSave(Base_ZJFAItem entity, string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.ID = KeyValue;
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    return Content(new JsonMessage { Success = true, Code = "-1", Message = "ID 不能为空,请稍候重试" }.ToString());
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败，错误：" + ex.Message }.ToString());
            }
        }

        #endregion

        #region 表单操作
        /// <summary>
        /// 删除数据  质检方案中的质检项目
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns> 
        [LoginAuthorize]
        public ActionResult DeleteFAItem(string KeyValue, string FANO, string ITEMNO)
        {
            try
            {
                //删除前验证

                if (string.IsNullOrEmpty(KeyValue) || string.IsNullOrEmpty(FANO) || string.IsNullOrEmpty(ITEMNO))
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "删除失败,请稍后重试" }.ToString());
                }

                ResultMessage message = base_zjfaitembll.DeleteFAItem(KeyValue, FANO, ITEMNO);

                WriteLog(message.Code, KeyValue, message.Content.ToString());
                return Content(new JsonMessage { Success = message.Code == 1, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 删除数据  质检方案
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult DeleteFA(string KeyValue)
        {
            string[] array = KeyValue.Split(',');
            try
            {
                //删除前验证

                DbTransaction isOpenTrans = database.BeginTrans();

                var Message = "删除失败。";

                Hashtable ht = new Hashtable();
                ht.Add("FANO", KeyValue);
                int IsOk = new RepositoryFactory<Base_ZJFAItem>().Repository().Delete("Base_ZJFAItem", ht, isOpenTrans);
                IsOk = new RepositoryFactory<Base_ZJFA>().Repository().Delete("Base_ZJFA", ht, isOpenTrans);
                database.Commit();
                if (IsOk > 0)
                {
                    Message = "删除成功。";
                }

                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 生成之间方案编码
        /// </summary>
        /// <param name="MaterialID"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult CreateZJFACode(string MaterialID)
        {
            try
            {
                ResultMessage message = new ResultMessage();
                if (!string.IsNullOrEmpty(MaterialID))
                {
                    message = base_zjfabll.CreateZJFACode(MaterialID);
                    return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = true, Code = "-1", Message = "物料不能为空" }.ToString());
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
