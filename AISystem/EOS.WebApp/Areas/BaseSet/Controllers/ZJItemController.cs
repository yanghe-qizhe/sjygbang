using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Business;
using EOS.Utilities;
using EOS.Repository;
using EOS.DataAccess;
using System.Data.Common;
using EOS.Entity;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class ZJItemController : PublicController<Base_ZJItem>
    {
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        Base_ZJItemBll base_zjitembll = new Base_ZJItemBll();
        VBase_ZJItemBll vbase_zjitembll = new VBase_ZJItemBll();
        QC_CHECKITEMBLL qcbll = new QC_CHECKITEMBLL();
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
        public ActionResult ZJItemForm()
        {
            string KeyValue = Request["KeyValue"];
            ModuleId = Request["ModuleId"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.ITEMNO = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }

        [LoginAuthorize]
        public ActionResult ZJItemIndex()
        {
            return View();
        }
        public ActionResult SelectNCZJItem()
        {
            return View();
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJItemList(string NAME, string StartTime, string EndTime, string Checked, string NC, string Type, String ZJType, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBase_ZJItem> ListData = vbase_zjitembll.GetOrderList(NAME, StartTime, EndTime, Checked, NC, Type, ZJType, jqgridparam);
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
        /// 获取NC质检项目列表
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetNCZJItemList(string NAME, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<QC_CHECKITEM> ListData = qcbll.GetNCZJItemList(NAME, jqgridparam);
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
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SubmitOrderForm(Base_ZJItem entity, string ModuleId, string KeyValue)
        {
            string sql = "";
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            ResultMessage message = new ResultMessage();
            try
            {
                //判断当前质检项目、质检类型、成分类型是否存在
                message = base_zjitembll.CheckItemName(entity, KeyValue);
                if (message.Code == 1)
                {
                    string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                    if (!string.IsNullOrEmpty(KeyValue))
                    {
                        //编辑
                        entity.Modify(KeyValue);
                        entity.SPELLCODE = PinyinHelper.PinyinString(entity.ITEMNAME);
                        database.Update(entity, isOpenTrans);
                    }
                    else
                    {
                        base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        sql = String.Format("select count(1) from Base_ZJItem where  ITEMNO='{0}'", entity.ITEMNO);
                       int res = DataFactory.Database().FindCountBySql(sql);
                        if (res > 0)
                        {
                            this.ModuleId = ModuleId;
                            entity.ITEMNO = this.BillCode();
                        }
                        //新增
                        entity.Create();
                        base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        entity.SPELLCODE = PinyinHelper.PinyinString(entity.ITEMNAME);
                        database.Insert(entity, isOpenTrans);
                         
                    }
                    database.Commit();
                    return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
                }
                else
                {
                    return Content(new JsonMessage { Success = true, Code = "-1", Message = message.Content.ToString() }.ToString());
                }
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
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
        public virtual ActionResult DeleteItem(string KeyValue)
        {
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";
                int IsOk = 0;
                //删除前验证 验证当前质检项是否在质检方案中已经存在
                int count = new RepositoryFactory<Base_ZJFAItem>().Repository().FindCount("ITEMNO", KeyValue);
                if (count > 0)
                {
                    Message = "当前质检项在质检方案中已存在，不允许删除！";
                }
                else
                {
                    IsOk = new RepositoryFactory<Base_ZJItem>().Repository().Delete(array);
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
                    }
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

        #endregion

    }
}
