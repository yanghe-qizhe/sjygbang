using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Entity;
using EOS.Business;
using EOS.DataAccess;
using System.Data.Common;
using EOS.Repository;

namespace EOS.WebApp.Areas.CommonModule.Controllers
{
    public class SystemSetController : PublicController<BASE_SYSTEMSET>
    {

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm(string OrderEntryJson)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                List<BASE_SYSTEMSET> OrderEntryList = OrderEntryJson.JonsToList<BASE_SYSTEMSET>();
                //处理 
                int index = 1;
                foreach (BASE_SYSTEMSET orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.FIELDNAME))
                    {
                        database.Update(orderentry, isOpenTrans);
                        index++;
                    }
                }
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = "配置成功" }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public override ActionResult Form()
        {
            IList<BASE_SYSTEMSET> list = repositoryfactory.Repository().FindList();
            return View(list);
        }


        public ActionResult GetOrderEntryList(string KeyValue,string type)
        {
            try
            {
                string strWhere = "";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    strWhere += $" and FIELDNAME='{KeyValue}'";
                }
                if (!string.IsNullOrEmpty(type))
                {
                    strWhere += $" and TYPE={type}";
                }
                var JsonData = new
                {
                    rows = repositoryfactory.Repository().FindList(strWhere).OrderBy(c=>c.ID ),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            BASE_SYSTEMSET entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

    }
}
