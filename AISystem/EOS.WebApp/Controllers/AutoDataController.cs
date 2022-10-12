using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Newtonsoft.Json;
using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;


namespace EOS.WebApp.Controllers
{
    public class AutoDataController : Controller
    {
        /// <summary>
        /// 查询前面50条用户信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult User(string keywords)
        {
            Base_UserBll base_userbll = new Base_UserBll();
            DataTable ListData = base_userbll.OptionUserList(keywords);
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// 查询前面50条质检用户信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult ZJUser(string keywords, string type)
        {
            Base_UserBll base_userbll = new Base_UserBll();
            DataTable ListData = base_userbll.OptionUserList(keywords, type);
            return Content(ListData.ToJson());
        }

        public ActionResult KSUser(string keywords, string kstype)
        {
            Base_UserKSBll base_userbll = new Base_UserKSBll();
            DataTable ListData = base_userbll.OptionList(keywords, kstype);
            return Content(ListData.ToJson());
        }


        /// <summary>
        /// 查询前面50条车辆信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult Cars(string keywords)
        {
            BD_CarsBll bd_bll = new BD_CarsBll();
            DataTable ListData = bd_bll.OptionAllList(keywords);
            return Content(ListData.ToJson());
        }

        public ActionResult DCars(string keywords)
        {
            BD_CarsBll bd_bll = new BD_CarsBll();
            DataTable ListData = bd_bll.DOptionList(keywords);
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// 查询前面50条司机信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult Drivers(string keywords)
        {
            BD_DriverBll bd_bll = new BD_DriverBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }

        public ActionResult DDrivers(string keywords)
        {
            BD_DriverBll bd_bll = new BD_DriverBll();
            DataTable ListData = bd_bll.DOptionList(keywords);
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// 查询前面50条卡注册信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult RegICCard(string keywords)
        {
            DP_REGICCARDBll bd_bll = new DP_REGICCARDBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }

        public ActionResult WRegICCard(string keywords)
        {
            DP_REGICCARDBll bd_bll = new DP_REGICCARDBll();
            DataTable ListData = bd_bll.WOptionList(keywords);
            return Content(ListData.ToJson());
        }

        //内部IC卡
        public ActionResult NRegICCard(string keywords)
        {
            DP_REGICCARDBll bd_bll = new DP_REGICCARDBll();
            DataTable ListData = bd_bll.NOptionList(keywords);
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// 查询前面50条客户信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult Customer(string keywords)
        {
            BD_CustomerBll bd_bll = new BD_CustomerBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }

        public ActionResult CustomerCode(string keywords)
        {
            BD_CustomerBll bd_bll = new BD_CustomerBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }

        public ActionResult QCustomer(string keywords)
        {
            BD_MQIS_CUSTOMERBll bd_bll = new BD_MQIS_CUSTOMERBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// 查询前面50条供应商信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult Supply(string keywords)
        {
            BD_SupplyBll bd_bll = new BD_SupplyBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// 查询前面50条磅房信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult WeightConfig(string keywords)
        {
            WB_SYS_WEIGHTCONFIGBll bd_bll = new WB_SYS_WEIGHTCONFIGBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }

        public ActionResult QSupply(string keywords)
        {
            BD_MQIS_SUPPLYBll bd_bll = new BD_MQIS_SUPPLYBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 查询前面50条物料信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult Material(string keywords, string istype)
        {
            BD_MaterialBll bd_bll = new BD_MaterialBll();
            DataTable ListData = bd_bll.OptionList(keywords, istype);
            return Content(ListData.ToJson());
        }
        public ActionResult GXMaterial(string keywords, string STATIONID)
        {
            BD_MaterialBll bd_bll = new BD_MaterialBll();
            DataTable ListData = bd_bll.GXOptionList(keywords, STATIONID);
            return Content(ListData.ToJson());
        }
        public ActionResult GCMaterial(string keywords, string istype)
        {
            BD_MaterialBll bd_bll = new BD_MaterialBll();
            DataTable ListData = bd_bll.GCOptionList(keywords, istype);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 查询前面50条物料信息（返回JSON）  所有物料
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public ActionResult MaterialAll(string keywords)
        {
            BD_MaterialBll bd_bll = new BD_MaterialBll();
            DataTable ListData = bd_bll.OptionListAll(keywords);
            return Content(ListData.ToJson());
        }
        public ActionResult MaterialCode(string keywords, string istype)
        {
            BD_MaterialBll bd_bll = new BD_MaterialBll();
            DataTable ListData = bd_bll.OptionList(keywords, istype);
            return Content(ListData.ToJson());
        }
        public ActionResult QMaterial(string keywords)
        {
            BD_MQIS_MATERIALBll bd_bll = new BD_MQIS_MATERIALBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 不合格产品名称
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public ActionResult BHGMaterial(string keywords)
        {
            BD_BHGMATERIALBll bd_bll = new BD_BHGMATERIALBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }
        //内控质量标准管理
        public ActionResult NKGBMANAGER(string keywords)
        {
            PD_GBMANAGERBll bd_bll = new PD_GBMANAGERBll();
            DataTable ListData = bd_bll.OptionList(keywords, "0");
            return Content(ListData.ToJson());
        }

        //国标质量标准管理
        public ActionResult GBMANAGER(string keywords)
        {
            PD_GBMANAGERBll bd_bll = new PD_GBMANAGERBll();
            DataTable ListData = bd_bll.OptionList(keywords, "1");
            return Content(ListData.ToJson());
        }

        public ActionResult GBMANAGERSELECT(string keywords, string GBTYPE, string GBCHECKTYPE)
        {
            PD_GBMANAGERBll bd_bll = new PD_GBMANAGERBll();
            DataTable ListData = bd_bll.OptionListSELECT(keywords, GBTYPE, GBCHECKTYPE);
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// 查询前面50条物料辅助信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult MaterialSpec(string keywords, string material)
        {
            BD_MATERIAL_SPECBll bd_bll = new BD_MATERIAL_SPECBll();
            DataTable ListData = bd_bll.OptionList(keywords, material);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 查询前面50条班组信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult WTEAM(string keywords)
        {
            BD_WTEAMBll bd_bll = new BD_WTEAMBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 查询前面50条秤点信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult SCALE(string keywords)
        {
            BD_SCALEBll bd_bll = new BD_SCALEBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 查询前面50条火车皮信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult Trains(string keywords)
        {
            BD_TRAINSBll bd_bll = new BD_TRAINSBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 查询前面50条承运商信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult TrafficCompany(string keywords)
        {
            BD_CARRIERBll bd_bll = new BD_CARRIERBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 查询前面50条仓库信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult Store(string keywords)
        {
            BD_STORDOCBll bd_bll = new BD_STORDOCBll();
            DataTable ListData = bd_bll.OptionList(keywords, "");
            return Content(ListData.ToJson());
        }
        public ActionResult GSStore(string keywords)
        {
            BD_STORDOCBll bd_bll = new BD_STORDOCBll();
            DataTable ListData = bd_bll.OptionList(keywords, "1");
            return Content(ListData.ToJson());
        }
        public ActionResult U9Store(string keywords)
        {
            BD_STORDOCBll bd_bll = new BD_STORDOCBll();
            DataTable ListData = bd_bll.OptionList(keywords, "2");
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 查询前50条部门信息(返回JSON)
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <param name="compid">上级部门</param>
        /// <returns></returns>
        public ActionResult Department(string keywords, string compid)
        {
            Base_DepartmentBll bd_bll = new Base_DepartmentBll();
            DataTable ListData = bd_bll.OptionList(keywords, compid);
            return Content(ListData.ToJson());
        }


        public ActionResult Dictionary(string keywords, string items)
        {
            VBASE_DATADICTIONARYDETAILBll base_bll = new VBASE_DATADICTIONARYDETAILBll();
            DataTable ListData = base_bll.OptionList(keywords, items);
            return Content(ListData.ToJson());
        }



        /// <summary>
        /// 查询前面50条质检化验信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <param name="items">质检方案ID</param>
        /// <returns></returns>
        public ActionResult ZJFAITEM(string keywords, string items)
        {
            VBase_ZJFAItemBll bd_bll = new VBase_ZJFAItemBll();
            DataTable ListData = bd_bll.OptionList(keywords, items);
            return Content(ListData.ToJson());
        }

        public ActionResult ZJITEMS(string keywords, string zjtype)
        {
            VBase_ZJItemBll bd_bll = new VBase_ZJItemBll();
            DataTable ListData = bd_bll.OptionList(keywords, zjtype);
            return Content(ListData.ToJson());
        }

        public ActionResult ZJFA(string keywords, string material)
        {
            VBase_ZJFABll bd_bll = new VBase_ZJFABll();
            DataTable ListData = bd_bll.OptionList(keywords, material);
            return Content(ListData.ToJson());
        }

        /// <summary>
        /// 质检列绑定
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public ActionResult ZJCOLUMN(string keywords, string type)
        {
            BASE_ZJCOLUMNBll bd_bll = new BASE_ZJCOLUMNBll();
            DataTable ListData = bd_bll.OptionList(keywords, type);
            return Content(ListData.ToJson());
        }

        public ActionResult Company(string keywords)
        {
            Base_CompanyBll bd_bll = new Base_CompanyBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }
        public ActionResult Company1(string keywords)
        {
            Base_CompanyBll bd_bll = new Base_CompanyBll();
            DataTable ListData = bd_bll.OptionList1(keywords);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// 查询前面50条供应商信息（返回JSON）
        /// </summary>
        /// <param name="keywords">查询关键字</param>
        /// <returns></returns>
        public ActionResult SourceArea(string keywords)
        {
            BD_YlBll bd_bll = new BD_YlBll();
            DataTable ListData = bd_bll.OptionList(keywords);
            return Content(ListData.ToJson());
        }
    }
}
