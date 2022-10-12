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

namespace EOS.WebApp.Areas.WeightManager.Controllers
{
    public class WeightPDCADDController : PublicController<PDCOrder>
    {
        string ModuleId = "";

        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm(PDCOrder entity, string OrderEntryJson, string ModuleId)
        {
            #region 验证

            #endregion
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                 string Message =  "新增成功。" ;
                List<WB_CONVOPER_PDC> OrderEntryList = OrderEntryJson.JonsToList<WB_CONVOPER_PDC>();
                int index = 1;
                string UserId = ManageProvider.Provider.Current().UserId;
                foreach (WB_CONVOPER_PDC orderentry in OrderEntryList)
                {
                    if (!string.IsNullOrEmpty(orderentry.MTID))
                    {
                        orderentry.Create();
                        base_coderulebll.OccupyBillCode(ManageProvider.Provider.Current().UserId, ModuleId, isOpenTrans);
                        orderentry.BILLNO = base_coderulebll.GetBillCode(UserId, ModuleId);
                        orderentry.STATIONID = entity.STATIONID;
                        orderentry.CCLASS = entity.CCLASS;
                        orderentry.CCLASSTIME = entity.CCLASSTIME;
                        orderentry.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        orderentry.CREATEUSER = ManageProvider.Provider.Current().UserName;

                        database.Insert(orderentry, isOpenTrans);
                        index++;
                    }
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



    }
}
