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

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class ZJMonthTargetController : PublicController<BASE_ZJMONTHTARGET>
    {
        BASE_ZJMONTHTARGETBLL zjmonth = new BASE_ZJMONTHTARGETBLL();
        #region 页面返回
        public ActionResult ZJMonthTargetForm()
        {
            ViewBag.FormID = "ZJMonthTargetForm";
            return View();
        }

        [LoginAuthorize]
        public ActionResult ZJMonthTargetIndex()
        {
            return View();
        }
        #endregion


        #region 数据列表

        /// <summary>
        /// 获取质检月度指标
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="STIME"></param>
        /// <param name="ETIME"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetZJMonthTargetList(string NAME, string STIME, string ETIME, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBASE_ZJMONTHTARGET> ListData = zjmonth.GetOrderList(NAME, STIME, ETIME, jqgridparam);
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
        public override ActionResult SubmitForm(BASE_ZJMONTHTARGET entity, string KeyValue)
        {
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            ResultMessage message = new ResultMessage();
            try
            {
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    //编辑
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    //新增
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                }
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

        #region 表单操作
        /// <summary>
        /// 删除数据  
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual ActionResult DeleteZJMonthTarget(string KeyValue)
        {
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";
                int IsOk = new RepositoryFactory<BASE_ZJMONTHTARGET>().Repository().Delete(array);
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

        #endregion
    }
}
