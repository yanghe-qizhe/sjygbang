using EOS.Business;
using EOS.Business;
using EOS.Entity;
using EOS.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    public class BindICCardController : PublicController<WB_ICCARD>
    {
        WB_ICCARDBLL bll = new WB_ICCARDBLL();

        #region 页面返回
        public ActionResult BindICCardList()
        {
            return View();
        }
        public ActionResult BindICCardForm()
        {
            return View();
        }
        #endregion

        #region 数据列表
        /// <summary>
        /// 查询门禁发卡数据
        /// </summary>
        /// <param name="StartTime"></param>
        /// <param name="EndTime"></param>
        /// <param name="RESERVATIONCHAR3">卡面编号</param>
        /// <param name="RESERVATIONCHAR12">身份证号</param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetOrderList(string StartTime, string EndTime, string RESERVATIONCHAR3, string RESERVATIONCHAR12, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<WB_ICCARD> ListData = bll.GetOrderList(StartTime, EndTime, RESERVATIONCHAR3, RESERVATIONCHAR12, jqgridparam);
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

        /// <summary>
        /// 保存门禁发卡
        /// </summary>
        /// <param name="modek"></param>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult SaveCard(WB_ICCARD modek, string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                message = bll.SaveCard(modek, KeyValue);
                return Content(new JsonMessage { Success = true, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());

            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 删除门禁发卡记录
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult DeleteForm(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                message = bll.DelCard(KeyValue);
                return Content(new JsonMessage { Success = message.Success, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());

            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 根据IC卡号查询卡面编号
        /// </summary>
        /// <param name="KeyValue"></param>
        /// <returns></returns>
        public ActionResult GetCardNumber(string KeyValue)
        {
            ResultMessage message = new ResultMessage();
            try
            {
                message = bll.GetICNumber(KeyValue);
                return Content(new JsonMessage { Success = message.Success, Code = message.Code.ToString(), Message = message.Content.ToString() }.ToString());

            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        public ActionResult GetScript()
        {
            string strJson = "";
            strJson += "var key = '201003220000';var NiceKey='201003220000';";
            strJson += " function readData(pie_arr) {";
            strJson += " var iRet = CardOpen(); if (iRet == 0) { for (var i = 0; i < pie_arr.length; i++) { var pie = pie_arr[i]; if(pie=='11'){ iRet = CardCheckPwdStatus(pie, NiceKey);}else{ iRet = CardCheckPwdStatus(pie, key);} if (iRet == 0) { getVal(pie); } } iRet = CardClose(); }";
            strJson += "}";
            strJson += " function writeData(pie_arr) {";
            strJson += "   var iRet = CardOpen(); if (iRet == 0) {   for (var i = 0; i < pie_arr.length; i++) { var pie = pie_arr[i];if(pie=='11'){ iRet = CardCheckPwdStatus(pie, NiceKey);}else{ iRet = CardCheckPwdStatus(pie, key);} if (iRet == 0) { setVal(pie); } } iRet = CardClose(); }";
            strJson += "}";
            strJson += "   function CardCheckPwdStatus(pie, key) {";
            strJson += "        var res = 1; MWRFATL.RF_LoadKey(0, pie, key); var iRet = MWRFATL.LastRet; if (iRet) { res = 1; $('#msg').text('读写器装载密码失败！'); } else { MWRFATL.RF_Authentication(0, pie); iRet = MWRFATL.LastRet; if (iRet) { res = 2; $('#msg').text('验证密码失败！'); } else { res = 0; } } return res;";
            strJson += " }";
            return Content(strJson);

        }
    }
}
