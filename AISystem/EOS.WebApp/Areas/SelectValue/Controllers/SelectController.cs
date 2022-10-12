using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using EOS.Business;
using EOS.Utilities;
using System.Diagnostics;

namespace EOS.WebApp.Areas.SelectValue.Controllers
{
    public class SelectController : PublicController<BD_MATERIAL>
    {
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        BD_MaterialBll bd_bll = new BD_MaterialBll();
        VBase_ZJItemBll zjitembll = new VBase_ZJItemBll();
        VBase_ZJFABll base_zjfabll = new VBase_ZJFABll();

        #region 页面返回
        public ActionResult SelectPORDER()
        {
            return View();
        }
        public ActionResult SelectMaterial()
        {
            return View();
        }
        public ActionResult SelectZJFA()
        {
            return View();
        }
        public ActionResult SelectZJFAForMaterial()
        {
            return View();
        }
        public ActionResult SelectZJItemMultipleNoFA()
        {
            return View();
        }
        public ActionResult SelectZJItemForFANO()
        {
            return View();
        }
        #endregion

        #region 数据列表

        /// <summary>
        /// 物料列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>

        public ActionResult GetOrderList(string keyword, string istype, string datatype, string isuse, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();

                List<VBD_MATERIAL> ListData = bd_bll.GetOrderList(keyword, istype, datatype, isuse,"","", StartTime, EndTime, jqgridparam);

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
        public ActionResult GetZJFAList(string NAME, string Type, string MaterialType, string DEF1, string MATERIALID, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBase_ZJFA> ListData = base_zjfabll.GetOrderListForSelect(NAME, Type, MaterialType, DEF1, MATERIALID, jqgridparam);
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
        /// 质检项目列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="TYPE">质检项目类型 0 1 2 all</param>
        /// <param name="ZJTYPE">质检分类项目</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        public ActionResult GetZJItemList(string NAME, string TYPE, string ZJTYPE, string FANO, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBase_ZJItem> ListData = zjitembll.GetOrderList(NAME, TYPE, ZJTYPE, FANO, jqgridparam);
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
        /// 质检项目列表（返回Json）
        /// </summary>
        /// <param name="NAME"></param>
        /// <param name="FANO"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetZJItemForFANOList(string NAME, string FANO, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<VBase_ZJItem> ListData = zjitembll.GetOrderListForFANO(NAME, FANO, jqgridparam);
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
        /// 获取订单列表
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="MATERIAL"></param>
        /// <param name="SUPPLY"></param>
        /// <param name="jqgridparam"></param>
        /// <returns></returns>
        public ActionResult GetPOrderList(string keyword, string RECCOMPANYID, string SUPPLY, string MATERIAL, string matclass, string istype, string iszx, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                VNC_PO_ORDER_BBll vorderbll = new VNC_PO_ORDER_BBll();
                List<VNC_PO_ORDER_B> ListData = vorderbll.GetQYOrderList(keyword, RECCOMPANYID, SUPPLY, "", MATERIAL, matclass, istype, iszx, StartTime, EndTime, jqgridparam);

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

    }
}
