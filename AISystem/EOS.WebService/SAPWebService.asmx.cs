using EOS.Cache;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Utilities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Xml;
using UtilPlugin.Http;
using EOS.WebService.EntityModels;

namespace EOS.WebService
{
    /// <summary>
    /// SAPWebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    [System.Web.Script.Services.ScriptService]

    public class SAPWebService : System.Web.Services.WebService
    {
        /// <summary>
        /// 日志记录
        /// </summary>
        public static DataAccess.LogHelper log = DataAccess.LogFactory.GetLogger(typeof(SAPWebService));
        /// <summary>
        /// 数据库连接
        /// </summary>
        private SqlSugar.SqlSugarClient Db = DbService.GetDB("ConnectionString");
        private SqlSugar.SqlSugarClient DbCS = DbService.GetDB("ConnectionStringCS");

        #region 测试机

        #region 基础档案

        /// <summary>
        /// SAP物料下载
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "SAP物料下载")]
        public RETURN_SAP SAP_94(string strjson)
        {
            SAPSERVICES_94.SI_ZFMM094Client sap = new SAPSERVICES_94.SI_ZFMM094Client("HTTPS_PORT94");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_94.ZFMM094 mm94 = new SAPSERVICES_94.ZFMM094();

            var resulte = sap.SI_ZFMM094(mm94);
            foreach (var model in resulte.T_RETURN)
            {
                var count = DbCS.Queryable<BD_MATERIAL>().Where(c => c.ID == model.MATNR).Count();
                if (count == 0)
                {
                    BD_MATERIAL obj = new BD_MATERIAL()
                    {
                        ID = model.MATNR,
                        CODE = model.MATNR,
                        NAME = model.MAKTX,
                        SHORTNAME = model.MAKTX,
                        SPELLCODE = model.MAKTX.ToPinYin(),
                        PK_MARBASCLASS = model.MTART,//物料类型
                        PK_GROUP = model.MATKL,//物料组
                        MATERIALSPEC = "",
                        DEF1 = model.MEINS,
                        DEF2 = model.MEINS,
                        ISUSE = "1",
                        DATATYPE = "1"
                    };
                    DbCS.Insertable(obj).ExecuteCommand();
                }
            }
            RETURN_SAP ret = new RETURN_SAP();

            if (resulte.T_RETURN.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            return ret;
        }


        /// <summary>
        /// SAP供应商下载
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "SAP供应商下载")]
        public RETURN_SAP SAP_95(string strjson)
        {
            SAPSERVICES_95.SI_ZFMM095Client sap = new SAPSERVICES_95.SI_ZFMM095Client("HTTPS_PORT95");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_95.ZFMM095 mm95 = new SAPSERVICES_95.ZFMM095();

            var resulte = sap.SI_ZFMM095(mm95);
            foreach (var model in resulte.T_RETURN)
            {
                var count = DbCS.Queryable<BD_SUPPLY>().Where(c => c.ID == model.LIFNR).Count();
                if (count == 0)
                {
                    BD_SUPPLY obj = new BD_SUPPLY()
                    {
                        ID = model.LIFNR,
                        CODE = model.LIFNR,
                        NAME = model.NAME1,
                        SHORTNAME = model.NAME1,
                        SPELLCODE = model.NAME1.ToPinYin(),
                        ISUSE = "1",
                        DATATYPE = "1"
                    };
                    DbCS.Insertable(obj).ExecuteCommand();
                    count = DbCS.Queryable<Base_UserKS>().Where(r => r.ACCOUNT == model.LIFNR).Count();
                    if (count == 0)
                    {
                        #region 供应商用户
                        Base_UserKS user = new Base_UserKS();
                        user.USERID = model.LIFNR;
                        user.CODE = model.LIFNR;
                        user.PASSWORD = "0c164caaa094a707";
                        user.REALNAME = model.NAME1;
                        user.ACCOUNT = model.LIFNR;
                        user.MOBILE = "";
                        user.ENABLED = 1;
                        user.TYPE = 2;
                        user.STYPE = 0;
                        user.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        user.CREATEUSER = "同步注册";
                        DbCS.Insertable(user).ExecuteCommand();

                        Base_DataScopePermission userPerm = new Base_DataScopePermission();
                        userPerm.DataScopePermissionId = CommonHelper.GetGuid;
                        userPerm.ModuleId = "999eae8b-1d09-4678-9b5f-2901205f1d98";
                        userPerm.ObjectId = model.LIFNR;
                        userPerm.ResourceId = model.LIFNR;
                        userPerm.SortCode = 0;
                        userPerm.Category = "6";
                        userPerm.CreateDate = DateTime.Now;
                        userPerm.CreateUserId = "";
                        userPerm.CreateUserName = "同步注册";
                        userPerm.ScopeType = "1";
                        DbCS.Insertable(userPerm).ExecuteCommand();
                        #endregion
                    }
                }
            }
            RETURN_SAP ret = new RETURN_SAP();
            if (resulte.T_RETURN.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }

            return ret;
        }

        #endregion

        #region 采购订单业务 
        /// <summary>
        /// SAP标准采购订单下载(调试通过不使用)
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "SAP标准采购订单下载")]
        public RETURN_SAP SAP_92(string strjson)
        {
            SAPSERVICES_92.SI_ZFMM092_OUTClient sap = new SAPSERVICES_92.SI_ZFMM092_OUTClient("HTTPS_PORT92");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];

            #region ZFMM092_ITEM
            SAPSERVICES_92.ZFMM092_ITEM[] arr = new SAPSERVICES_92.ZFMM092_ITEM[1];
            SAPSERVICES_92.ZFMM092_ITEM zfmm092 = new SAPSERVICES_92.ZFMM092_ITEM();
            zfmm092.AEDAT = "";
            zfmm092.EBELN = "";
            zfmm092.EBELP = "";
            zfmm092.LIFNR = "";
            zfmm092.MATNR = "";
            zfmm092.MENGE = 0;
            zfmm092.MENGESpecified = false;
            zfmm092.WERKS = "";
            arr[0] = zfmm092;
            #endregion

            #region ZFMM092
            SAPSERVICES_92.ZFMM092 mm92 = new SAPSERVICES_92.ZFMM092();
            mm92.I_EBELN = "";
            mm92.I_LIFNR = "";
            mm92.I_MATNR = "";
            mm92.T_ITEM = arr;
            #endregion
            var resulte = sap.SI_ZFMM092_OUT(mm92);
            int count = 0;
            int res = 0;
            #region 初始化
            string VBILLCODE = "";//采购凭证
            string PK_ORDER = "";
            string BILLTYPE = "";//采购凭证类别
            string MODIFIEDTIME = "";//更改日期
            string DBILLDATE = "";//采购凭证日期
            string DMAKEDATE = "";//创建日期
            string CREATIONTIME = "";//创建日期
            string FORDERSTATUS = "";//凭证状态
            string PK_MATERIAL = "";//物料编号
            string MATERIAL = "";//物料描述
            string PK_SUPPLIER = "";//供应商编号
            string SUPPLYNAME = "";//供应商名称
            string CROWNO = "";//采购凭证行项目
            string PK_ORDER_B = "";//明细PK 
            string PK_ORG = "";//采购组织
            string PK_DEPT = "";//采购组
            string RECCOMPANYID = "";//工厂
            string PURUNITNAME = "";//订单单位
            decimal NASTNUM = 0;//采购订单数量
            decimal SECQUANTITY = 0;//采购订单数量
            string SECPURUNITNAME = "";//订单单位
            decimal NQTUNITNUM = 0; //价格单位数量
            string CQTUNITID = "";//订单价格单位
            decimal NPRICE = 0;//净价
            #endregion
            foreach (var model in resulte.T_ITEM)
            {
                #region 初始化值
                VBILLCODE = model.EBELN;//采购凭证
                PK_ORDER = model.EBELN;
                BILLTYPE = "";//采购凭证类别
                MODIFIEDTIME = string.Format("{0} 00:00:00", model.AEDAT);//更改日期
                DBILLDATE = string.Format("{0} 00:00:00", model.AEDAT);//采购凭证日期
                DMAKEDATE = string.Format("{0} 00:00:00", model.AEDAT);//创建日期
                CREATIONTIME = string.Format("{0} 00:00:00", model.AEDAT);//创建日期
                FORDERSTATUS = "";//凭证状态
                PK_MATERIAL = model.MATNR;//物料编号
                MATERIAL = "";//物料描述
                PK_SUPPLIER = model.LIFNR;//供应商编号
                SUPPLYNAME = "";//供应商名称
                CROWNO = model.EBELP;//采购凭证行项目
                PK_ORDER_B = string.Format("{0}{1}", PK_ORDER, CROWNO);//明细PK 
                PK_ORG = "";//采购组织
                PK_DEPT = "";//采购组
                RECCOMPANYID = model.WERKS;//工厂
                PURUNITNAME = "";//订单单位
                NASTNUM = model.MENGE;//采购订单数量
                SECQUANTITY = model.MENGE;//采购订单数量
                SECPURUNITNAME = "";//订单单位
                NQTUNITNUM = 0; //价格单位数量
                CQTUNITID = "";//订单价格单位
                NPRICE = 0;//净价
                #endregion

                #region 主表
                var order = new NC_PO_ORDER();
                order.VBILLCODE = VBILLCODE;//订单编号(采购凭证)
                order.PK_ORDER = PK_ORDER;//订单主键
                order.BILLTYPE = BILLTYPE;
                order.DBILLDATE = DBILLDATE;//订单日期;
                order.PK_SUPPLIER = PK_SUPPLIER;//供应商
                order.PK_ORG = PK_ORG;//组织机构
                order.PK_DEPT = PK_DEPT;//组织机构

                if (FORDERSTATUS == "1")
                {
                    order.FORDERSTATUS = 1;
                }
                else
                {
                    order.FORDERSTATUS = 0;
                }
                order.CARSNUM = 0;
                order.CREATIONTIME = CREATIONTIME;//创建日期;
                order.DMAKEDATE = DMAKEDATE;//修改日期;
                order.MODIFIEDTIME = MODIFIEDTIME;//修改日期;
                order.DATATYPE = "1";
                count = Db.Queryable<NC_PO_ORDER>().Where(r => r.PK_ORDER == order.PK_ORDER).Count();
                if (count > 0)
                {
                    Db.Updateable(order).ExecuteCommand();
                }
                else
                {
                    Db.Insertable(order).ExecuteCommand();
                }
                #endregion

                #region 明细表
                var orderitem = new NC_PO_ORDER_B();
                orderitem.DBILLDATE = DBILLDATE;
                orderitem.PK_ORDER = PK_ORDER;
                orderitem.PK_ORDER_B = PK_ORDER_B;
                orderitem.CROWNO = CROWNO;//采购订单行号
                orderitem.PK_SUPPLIER = PK_SUPPLIER;//供应商
                orderitem.PK_MATERIAL = PK_MATERIAL;//物料
                orderitem.PURUNITNAME = PURUNITNAME;
                orderitem.NASTNUM = NASTNUM;//采购订单数量
                orderitem.SECPURUNITNAME = SECPURUNITNAME;
                orderitem.SECQUANTITY = SECQUANTITY;//采购订单数量
                orderitem.NQTUNITNUM = NQTUNITNUM;//价格单位数量
                orderitem.CQTUNITID = CQTUNITID;//订单价格单位
                orderitem.NPRICE = NPRICE;//净价
                orderitem.RECCOMPANYID = model.WERKS;//工厂
                count = Db.Queryable<NC_PO_ORDER>().Where(r => r.PK_ORDER == order.PK_ORDER).Count();
                if (count > 0)
                {
                    res += Db.Updateable(orderitem).ExecuteCommand();
                }
                else
                {
                    res += Db.Insertable(orderitem).ExecuteCommand();
                }
                #endregion
            }
            RETURN_SAP ret = new RETURN_SAP();
            if (resulte.T_ITEM.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            return ret;
        }

        /// <summary>
        /// SAP标准采购订单关闭(调试通过不使用)
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "SAP标准采购订单关闭")]
        public RETURN_SAP SAP_96(string strjson)
        {

            SAPSERVICES_96.SI_ZFMM096_OUTClient mat = new SAPSERVICES_96.SI_ZFMM096_OUTClient("HTTPS_PORT96");
            mat.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            mat.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            #region ZTFMM096

            SAPSERVICES_96.ZFMM096 mm96 = new SAPSERVICES_96.ZFMM096();
            mm96.T_OUTTAB = new SAPSERVICES_96.ZSFMM096[1];

            #region ZSFMM096
            SAPSERVICES_96.ZSFMM096 zsfmm096 = new SAPSERVICES_96.ZSFMM096();
            zsfmm096.PHAS0 = "";
            zsfmm096.PHAS2 = "";
            zsfmm096.PHAS3 = "";
            zsfmm096.AUFNR = "";
            zsfmm096.WERKS = "9000";
            mm96.T_OUTTAB[0] = zsfmm096;
            #endregion

            mm96.T_RETURN = new SAPSERVICES_96.ZTFMM096[1];
            SAPSERVICES_96.ZTFMM096 ztfmm096 = new SAPSERVICES_96.ZTFMM096();
            mm96.T_RETURN[0] = ztfmm096;
            #endregion


            var resulte = mat.SI_ZFMM096_OUT(mm96);
            foreach (var model in resulte.T_OUTTAB)
            {

            }

            RETURN_SAP ret = new RETURN_SAP();
            if (resulte.T_OUTTAB.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            return ret;
        }

        /// <summary>
        /// SAP采购订单下载(项目使用)
        /// </summary>
        /// <param name="strzaedat">创建时间</param>
        /// <param name="strzbudat">修改时间</param>
        /// <param name="strwerks">工厂</param>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "SAP采购订单下载")]
        public RETURN_SAP SAP_116(string strzaedat, string strzbudat, string vbillcode, string strwerks, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            //if (string.IsNullOrEmpty(strzaedat))
            //{
            //    strzaedat = DateTime.Now.ToString("yyyy-MM-dd");
            //}
            //if (string.IsNullOrEmpty(strwerks))
            //{
            //    strwerks = "1000";
            //}
            #region 初始化接口
            SAPSERVICES_116.SI_ZFMM116_OUTClient sap = new SAPSERVICES_116.SI_ZFMM116_OUTClient("HTTPS_PORT116");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            #endregion

            #region 输入参数ZFMM116

            SAPSERVICES_116.ZFMM116 mm116 = new SAPSERVICES_116.ZFMM116();
            mm116.I_EBELN = vbillcode;//采购凭证
            mm116.I_LIFNR = "";//供应商编号
            mm116.I_MATNR = "";//物料编号
            mm116.I_PROCSTAT = "";//批准状态
            mm116.I_WERKS = "";//工厂
            #region 工厂 WERKS
            //if (!string.IsNullOrEmpty(strwerks))
            //{
            //    mm116.I_WERKS = strwerks;
            //}
            //else { }
            #endregion
            mm116.I_ZAEDAT = strzaedat;//创建时间
            mm116.I_ZBUDAT = strzbudat;//修改时间
            mm116.T_OUTTAB = new SAPSERVICES_116.ZSMM116[1];
            #region 明细 ZSMM116
            SAPSERVICES_116.ZSMM116 zsmm116 = new SAPSERVICES_116.ZSMM116();
            zsmm116.BPRME = "";
            zsmm116.BSART = "";
            zsmm116.EBELN = "";
            zsmm116.EBELP = "";
            zsmm116.EKGRP = "";
            zsmm116.EKORG = "";
            zsmm116.LIFNR = "";
            zsmm116.MATNR = "";
            zsmm116.MEINS = "";
            zsmm116.MENGE = 0;
            zsmm116.NAME1 = "";
            zsmm116.MENGESpecified = false;
            zsmm116.NETPR = 0;
            zsmm116.NETPRSpecified = false;
            zsmm116.PEINH = 0;
            zsmm116.PEINHSpecified = false;
            zsmm116.PROCSTAT = "";
            zsmm116.TXZ01 = "";
            zsmm116.WERKS = "";
            zsmm116.ZAEDAT = "";
            zsmm116.ZBUDAT = "";

            #endregion
            mm116.T_OUTTAB[0] = zsmm116;
            #endregion

            //调用接口
            var resulte = sap.SI_ZFMM116_OUT(mm116);
            int count = 0;
            foreach (var model in resulte.T_OUTTAB)
            {
                #region 初始化值
                string VBILLCODE = model.EBELN;//采购凭证
                string PK_ORDER = model.EBELN;//采购订单主键
                string BILLTYPE = model.BSART;//采购凭证类别
                string DBILLDATE = string.Format("{0} 00:00:00", model.ZBUDAT);//采购凭证日期
                string FORDERSTATUS = model.PROCSTAT;//凭证状态
                string PK_SUPPLIER = model.LIFNR;//供应商编号
                string SUPPLYNAME = model.NAME1;//供应商名称
                string PK_ORG = model.EKORG;//采购公司
                string PK_DEPT = model.EKGRP;//采购分组
                string MODIFIEDTIME = string.Format("{0} 00:00:00", model.ZAEDAT);//更改日期
                string DMAKEDATE = string.Format("{0} 00:00:00", model.ZBUDAT);//创建日期
                string CREATIONTIME = string.Format("{0} 00:00:00", model.ZBUDAT);//创建日期

                string CROWNO = model.EBELP;//采购凭证行项目
                string PK_ORDER_B = string.Format("{0}{1}", PK_ORDER, CROWNO);
                string PK_MATERIAL = model.MATNR;//物料编号
                string MATERIAL = model.TXZ01;//物料描述
                string PK_MARBASCLASS = model.MTART;//物料分组
                string PK_MATERIALGROUP = model.MTART;//物料类型
                string MATERIALBARCODE = "";// model.EXTWG;//外部物料组

                string PURUNITNAME = model.MEINS;//订单单位
                decimal NASTNUM = model.MENGE;//采购订单数量
                string SECPURUNITNAME = model.MEINS;//订单单位
                decimal SECQUANTITY = model.MENGE;//采购订单数量
                decimal NQTUNITNUM = model.PEINH; //价格单位数量
                string CQTUNITID = model.BPRME;//订单价格单位
                decimal NPRICE = model.NETPR;//净价
                string RECCOMPANYID = model.WERKS;//工厂
                string BARRIVECLOSE = model.LOEKZ;//L删除 空未删除 
                string VBDEF4 = model.UEBTK;
                #endregion

                if (!string.IsNullOrEmpty(PK_MATERIAL))
                {
                    //if (string.IsNullOrEmpty(ELIKZ))
                    //{
                    #region 主表
                    var order = new NC_PO_ORDER();
                    order.VBILLCODE = VBILLCODE;//订单编号(采购凭证)
                    order.PK_ORDER = PK_ORDER;//订单主键
                    order.BILLTYPE = BILLTYPE;
                    order.DBILLDATE = DBILLDATE;//订单日期;
                    order.PK_SUPPLIER = PK_SUPPLIER;//供应商
                    order.PK_ORG = PK_ORG;//组织组织
                    order.PK_DEPT = PK_DEPT;//组织部门
                                            //FORDERSTATUS
                    if (FORDERSTATUS == "02")
                    {
                        order.FORDERSTATUS = 0;
                    }
                    else
                    {
                        order.FORDERSTATUS = 1;
                    }
                    order.CARSNUM = 0;
                    order.CREATIONTIME = CREATIONTIME;//创建日期;
                    order.DMAKEDATE = DMAKEDATE;//修改日期;
                    order.MODIFIEDTIME = MODIFIEDTIME;//修改日期;
                    order.DATATYPE = "1";
                    count = DbCS.Queryable<NC_PO_ORDER>().Where(r => r.PK_ORDER == order.PK_ORDER).Count();
                    if (count > 0)
                    {
                        DbCS.Updateable(order).ExecuteCommand();
                    }
                    else
                    {
                        DbCS.Insertable(order).ExecuteCommand();
                    }
                    #endregion

                    #region 明细表
                    var orderitem = new NC_PO_ORDER_B();
                    orderitem.DBILLDATE = DBILLDATE;
                    orderitem.PK_ORDER = PK_ORDER;
                    orderitem.PK_ORDER_B = PK_ORDER_B;
                    orderitem.CROWNO = CROWNO;//采购订单行号
                    orderitem.PK_SUPPLIER = PK_SUPPLIER;//供应商
                    orderitem.PK_MATERIAL = PK_MATERIAL;//物料
                    orderitem.PURUNITNAME = PURUNITNAME;
                    orderitem.NASTNUM = NASTNUM;//采购订单数量
                    orderitem.SECPURUNITNAME = SECPURUNITNAME;
                    orderitem.SECQUANTITY = NASTNUM;//采购订单数量
                    orderitem.NQTUNITNUM = NQTUNITNUM;//价格单位数量
                    orderitem.CQTUNITID = CQTUNITID;//订单价格单位
                    orderitem.NPRICE = NPRICE;//净价
                    orderitem.RECCOMPANYID = RECCOMPANYID;//工厂
                    orderitem.BARRIVECLOSE = "0";
                    if (BARRIVECLOSE == "L")
                    {
                        orderitem.BARRIVECLOSE = "1";
                    }

                    count = DbCS.Queryable<NC_PO_ORDER_B>().Where(r => r.PK_ORDER_B == PK_ORDER_B).Count();
                    if (count > 0)
                    {
                        DbCS.Updateable(orderitem).ExecuteCommand();
                    }
                    else
                    {
                        DbCS.Insertable(orderitem).ExecuteCommand();
                    }
                    #endregion

                    #region 基础档案
                    if (!string.IsNullOrEmpty(PK_SUPPLIER))
                    {
                        count = DbCS.Queryable<BD_SUPPLY>().Where(c => c.ID == PK_SUPPLIER).Count();
                        if (count == 0)
                        {
                            BD_SUPPLY sup_obj = new BD_SUPPLY()
                            {
                                ID = PK_SUPPLIER,
                                CODE = PK_SUPPLIER,
                                NAME = SUPPLYNAME,
                                SHORTNAME = SUPPLYNAME,
                                SPELLCODE = SUPPLYNAME.ToPinYin(),
                                ISUSE = "1",
                                DATATYPE = "1"
                            };
                            DbCS.Insertable(sup_obj).ExecuteCommand();
                        }

                        count = DbCS.Queryable<Base_UserKS>().Where(r => r.USERID == PK_SUPPLIER).Count();
                        if (count == 0)
                        {
                            #region 供应商用户
                            Base_UserKS user = new Base_UserKS();
                            user.USERID = model.LIFNR;
                            user.CODE = model.LIFNR;
                            user.PASSWORD = "0c164caaa094a707";
                            user.REALNAME = model.NAME1;
                            user.ACCOUNT = model.LIFNR;
                            user.MOBILE = "";
                            user.ENABLED = 1;
                            user.TYPE = 2;
                            user.STYPE = 0;
                            user.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            user.CREATEUSER = "同步注册";
                            DbCS.Insertable(user).ExecuteCommand();
                            #endregion

                            #region 供应商用户权限表
                            Base_DataScopePermission userPerm = new Base_DataScopePermission();
                            userPerm.DataScopePermissionId = CommonHelper.GetGuid;
                            userPerm.ModuleId = "999eae8b-1d09-4678-9b5f-2901205f1d98";
                            userPerm.ObjectId = model.LIFNR;
                            userPerm.ResourceId = model.LIFNR;
                            userPerm.SortCode = 0;
                            userPerm.Category = "6";
                            userPerm.CreateDate = DateTime.Now;
                            userPerm.CreateUserId = "";
                            userPerm.CreateUserName = "同步注册";
                            userPerm.ScopeType = "1";
                            DbCS.Insertable(userPerm).ExecuteCommand();
                            #endregion
                        }
                    }

                    if (!string.IsNullOrEmpty(PK_MATERIAL))
                    {
                        count = DbCS.Queryable<BD_MATERIAL>().Where(c => c.ID == PK_MATERIAL).Count();
                        if (count == 0)
                        {
                            BD_MATERIAL mat_obj = new BD_MATERIAL()
                            {
                                ID = PK_MATERIAL,
                                CODE = PK_MATERIAL,
                                NAME = MATERIAL,
                                SHORTNAME = MATERIAL,
                                SPELLCODE = MATERIAL.ToPinYin(),
                                PK_MARBASCLASS = PK_MARBASCLASS,//物料类型
                                PK_GROUP = PK_MATERIALGROUP,//物料组
                                MATERIALBARCODE = MATERIALBARCODE,//外部物料组
                                MATERIALSPEC = "",
                                DEF1 = model.MEINS,
                                DEF2 = model.MEINS,
                                ISUSE = "1",
                                ISJL = 1,
                                DATATYPE = "1"
                            };
                            DbCS.Insertable(mat_obj).ExecuteCommand();
                        }
                    }
                    #endregion
                    //}
                }
            }

            if (resulte.T_OUTTAB.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            return ret;
        }

        #endregion

        #region 采购收货业务

        [WebMethod(Description = "AJAX国外废纸收货(二次计量调用)/化工收货推送(一次计量推送)")]
        public RETURN_SAP SAP_POST1(string EOS_GM_CODE, string DOC_DATE, string PSTNG_DATE, string PO_NUMBER, string PO_ITEM, string MATERIAL, string BATCH, decimal ENTRY_QNT, string STGE_LOC, string PLANT, string MOVE_TYPE, string LIFNR, decimal MENGE, decimal NETPR, string MVT_IND)
        {
            #region 
            EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            entity.EOS_GM_CODE = EOS_GM_CODE;
            entity.DOC_DATE = DOC_DATE;
            entity.PSTNG_DATE = PSTNG_DATE;
            entity.BILL_OF_LADING = "";
            entity.GR_GI_SLIP_NO = "";
            entity.PO_NUMBER = PO_NUMBER;//订单编号
            entity.PO_ITEM = PO_ITEM;//订单项目编号
            entity.MATERIAL = MATERIAL;//物料
            entity.BATCH = BATCH;//批次
            entity.ENTRY_QNT = ENTRY_QNT;//数量
            entity.STGE_LOC = STGE_LOC;// "Y490";//库存地点
            entity.PLANT = PLANT;//工厂
            entity.MOVE_TYPE = MOVE_TYPE;//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            entity.MVT_IND = MVT_IND;//移动标识,B,F,B是采购订单  F是生产订单
            entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
            #endregion

            #region  ZMSEG_C
            entity.LIFNR = LIFNR;//供应商或债券人的账户
            entity.EBELN = string.Format("00000{0}", PO_NUMBER);//采购凭证编号
            entity.MATNR = MATERIAL;//物料编号
            entity.MENGE = entity.MENGE;//采购订单数量
            entity.EINDT = PSTNG_DATE;//项目交货日期
            entity.WERKS = PLANT;//工厂
            entity.NETPR = NETPR;//净价
            entity.PQ = "N";//纸质类型
            #endregion 
            return SAP_POST(entity, "");
        }

        /// <summary>
        /// 国外废纸收货(二次计量调用)/化工收货推送(一次计量推送)
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "国外废纸收货(二次计量调用)/化工收货推送(一次计量推送)")]
        public RETURN_SAP SAP_POST(EOS_Z_PO_POST entity, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();

            #region 测试
            //EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            //entity.EOS_GM_CODE = "01";
            //entity.DOC_DATE = "2021-09-28";// DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PSTNG_DATE = "2021-09-28";//DateTime.Now.ToString("yyyy-MM-dd");
            //entity.BILL_OF_LADING = "";
            //entity.GR_GI_SLIP_NO = "";
            //entity.PO_NUMBER = "4700007834";//订单编号
            //entity.PO_ITEM = "00010";//订单项目编号
            //entity.MATERIAL = "000000000003010087";//物料
            //entity.BATCH = "2021092802";//批次
            //entity.ENTRY_QNT = 160;//数量
            //entity.STGE_LOC = "Y089";// "Y490";//库存地点
            //entity.PLANT = "1000";//工厂
            //entity.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            //entity.MVT_IND = "B";//移动标识,B,F,B是采购订单  F是生产订单
            //entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.EOS_GM_CODE))
            {
                entity.EOS_GM_CODE = "01";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(entity.PSTNG_DATE))
            {
                entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }

            #endregion

            #region 初始化接口
            #region  Z_PO_POST
            SAPSERVICES_ZP1.SI_Z_PO_POST_OUTClient sap = new SAPSERVICES_ZP1.SI_Z_PO_POST_OUTClient("HTTPS_PORTPOST");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_ZP1.Z_PO_POST z_po_post = new SAPSERVICES_ZP1.Z_PO_POST();
            z_po_post.GOODSMVT_CODE = new SAPSERVICES_ZP1.BAPI2017_GM_CODE();
            z_po_post.GOODSMVT_CODE.GM_CODE = entity.EOS_GM_CODE;//01采购订单收货,02生产订单收货
            z_po_post.GOODSMVT_HEADER = new SAPSERVICES_ZP1.BAPI2017_GM_HEAD_01();
            z_po_post.GOODSMVT_HEADER.DOC_DATE = entity.DOC_DATE;//DateTime.Now.ToString("yyyy-MM-dd");//凭证中的凭证日期,当前系统日期
            z_po_post.GOODSMVT_HEADER.PSTNG_DATE = entity.PSTNG_DATE;// DateTime.Now.ToString("yyyy-MM-dd");//凭证中的过账日期-可人工修改重新传SAP,系统当前日期
            z_po_post.GOODSMVT_HEADER.BILL_OF_LADING = entity.BILL_OF_LADING;//收货时提单号
            z_po_post.GOODSMVT_HEADER.GR_GI_SLIP_NO = entity.GR_GI_SLIP_NO;//收货/发货单编号
            z_po_post.GOODSMVT_HEADER.HEADER_TXT = "无人计量采购过账";//凭证抬头文本
            #endregion

            #region GOODSMVT_HEADER
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO = "";//参考凭证编号
            //z_po_post.GOODSMVT_HEADER.PR_UNAME = "";//用户名 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIP = "";//收货/发货单打印版本 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIPX = "";//相关用户数据字段的已更新信息
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO_LONG = "";//参考凭证编号(对相关性参看长文本)
            //z_po_post.GOODSMVT_HEADER.BILL_OF_LADING_LONG = "";//组的检配单编号(根据：参见长文本)
            //z_po_post.GOODSMVT_HEADER.BAR_CODE = "";//条形码输入
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账 
            #endregion

            #region 初始化ZMSEG_C
            z_po_post.ZMSEG_C = new SAPSERVICES_ZP1.ZMSEG_C();
            #region 注释
            //z_po_post.ZMSEG_C.MANDT = "";//集团
            //z_po_post.ZMSEG_C.MBLNR = "";//物料凭证编号
            //z_po_post.ZMSEG_C.MJAHR = "";//物料凭证的年份
            //z_po_post.ZMSEG_C.CHEHO = "";//车牌号
            //z_po_post.ZMSEG_C.XNHAO = "";//箱号
            //z_po_post.ZMSEG_C.CTNER = "";//柜型
            //z_po_post.ZMSEG_C.JKDAT = "";//进口日期
            //z_po_post.ZMSEG_C.LIFN1 = "";//供应商或债券人的账户
            //z_po_post.ZMSEG_C.NAME9 = "";//名称
            #endregion
            z_po_post.ZMSEG_C.LIFNR = entity.LIFNR;//供应商或债券人的账户
            z_po_post.ZMSEG_C.EBELN = entity.EBELN;//采购凭证编号
            z_po_post.ZMSEG_C.MATNR = entity.MATNR;//物料编号
            z_po_post.ZMSEG_C.MENGE = entity.MENGE;//采购订单数量
            z_po_post.ZMSEG_C.MENGESpecified = true;
            z_po_post.ZMSEG_C.EINDT = entity.EINDT;//项目交货日期
            z_po_post.ZMSEG_C.WERKS = entity.WERKS;//工厂
            z_po_post.ZMSEG_C.NETPR = entity.NETPR;//净价
            z_po_post.ZMSEG_C.NETPRSpecified = true;
            z_po_post.ZMSEG_C.PQ = entity.PQ;//纸质类型
            #endregion

            #region 初始化 GOODSMVT_ITEM
            z_po_post.GOODSMVT_ITEM = new SAPSERVICES_ZP1.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_ZP1.BAPI2017_GM_ITEM_CREATE gm_item = new SAPSERVICES_ZP1.BAPI2017_GM_ITEM_CREATE();
            gm_item.MATERIAL = entity.MATERIAL;//物料
            gm_item.PLANT = entity.PLANT;//工厂
            gm_item.STGE_LOC = entity.STGE_LOC;//库存地点
            gm_item.BATCH = entity.BATCH;//批次
            gm_item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            gm_item.ENTRY_QNT = entity.ENTRY_QNT;//数量
            gm_item.ENTRY_QNTSpecified = true;
            gm_item.PO_NUMBER = entity.PO_NUMBER;//订单编号
            gm_item.PO_ITEM = entity.PO_ITEM;//订单项目编号
            gm_item.ORDERID = entity.ORDERID;//订单编号
            gm_item.ORDER_ITNO = entity.ORDER_ITNO;//订单项目编号
            gm_item.MVT_IND = entity.MVT_IND;//移动标识,B,F,B是采购订单  F是生产订单
            gm_item.SPEC_STOCK = entity.SPEC_STOCK;//特殊库存标识,K寄售,空

            #region   GOODSMVT_ITEM

            gm_item.S_ORD_ITEM = "000000";
            gm_item.SCHED_LINE = "0000";
            gm_item.PO_PR_QNT = 0;
            gm_item.RESERV_NO = "0000000000";
            gm_item.RES_ITEM = "0000";
            gm_item.MOVE_REAS = "0000";
            gm_item.PROFIT_SEGM_NO = "0000000000";
            gm_item.AMOUNT_LC = 0;
            gm_item.AMOUNT_SV = 0;
            gm_item.REF_DOC_IT = "0000";
            gm_item.VAL_S_ORD_ITEM = "000000";
            gm_item.DELIV_NUMB_TO_SEARCH = "000000";
            gm_item.SU_PL_STCK_1 = 0;
            gm_item.ST_UN_QTYY_1 = 0;
            gm_item.SU_PL_STCK_2 = 0;
            gm_item.ST_UN_QTYY_2 = 0;
            gm_item.MATITEM_TR_CANCEL = "0000";
            gm_item.LINE_ID = "000000";
            gm_item.PARENT_ID = "000000";
            gm_item.LINE_DEPTH = "00";
            gm_item.QUANTITY = 0;
            gm_item.EARMARKED_ITEM = "000";

            #region 注释
            // gm_item.STCK_TYPE = "2";//库存类型:化工：2质量检验 废纸：空 非限制使用 3已冻结
            ////单位
            //gm_item.ENTRY_UOM = "";
            ////计量单位ISO代码
            //gm_item.ENTRY_UOM_ISO = "";
            ////采购订单价格单位的数量
            //gm_item.PO_PR_QNT = 0;
            ////订单价格单位(采购)
            //gm_item.ORDERPR_UN = "";
            ////计量单位ISO代码
            //gm_item.ORDERPR_UN_ISO = "";
            //gm_item.PO_NUMBER = "4300004787";//采购订单号
            //gm_item.PO_ITEM = "10";//采购凭证的项目编号
            //gm_item.VENDOR = "106544";//供应商账户号

            //gm_item.SHIPPING = "";//装运指令
            //gm_item.COMP_SHIP = "";//依照装运通知
            //gm_item.NO_MORE_GR = "";//交货已完成标识 
            //gm_item.ITEM_TEXT = "";//项目文本 
            //gm_item.GR_RCPT = "";//收货方 
            //gm_item.UNLOAD_PT = "";//卸货点
            //gm_item.COSTCENTER = "";//成本中心
            //gm_item.VAL_TYPE = "";//评估类型
            //gm_item.CUSTOMER = "";//客户的账户编号
            //gm_item.SALES_ORD = "";//销售订单数
            //gm_item.S_ORD_ITEM = "";//销售订单中的项目编号
            #endregion
            z_po_post.GOODSMVT_ITEM[0] = gm_item;
            #endregion

            #endregion

            #region 初始化 RETURN
            z_po_post.RETURN = new SAPSERVICES_ZP1.BAPIRET2[1];
            SAPSERVICES_ZP1.BAPIRET2 bapiret = new SAPSERVICES_ZP1.BAPIRET2();

            //bapiret.ID = "";
            //bapiret.TYPE = "";
            //bapiret.NUMBER = "";
            //bapiret.MESSAGE = "";
            //bapiret.LOG_NO = "";
            //bapiret.LOG_MSG_NO = "";
            //bapiret.MESSAGE_V1 = "";
            //bapiret.MESSAGE_V2 = "";
            //bapiret.MESSAGE_V3 = "";
            //bapiret.MESSAGE_V4 = "";
            //bapiret.PARAMETER = "";
            //bapiret.ROW = "";
            //bapiret.FIELD = "";
            //bapiret.SYSTEM = "";
            z_po_post.RETURN[0] = bapiret;
            #endregion


            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_PO_POST_OUT(z_po_post);
            if (resulte.RC == "0")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                string PRUEFLOS = resulte.PRUEFLOS;//检验批号
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                //物料凭证号(推送单据号) MATERIALDOCUMENT = "5003845836"
                String MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                REDATA redata = new REDATA();
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;//凭证编号
                redata.PRUEFLOS = PRUEFLOS;
                redata.E_POSNR = resulte.E_POSNR;
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    ret.REMSG = string.Format("失败：{0}", resulte.RETURN[0].MESSAGE);
                }
                ret.REDATA = null;
            }
            #endregion

            return ret;
        }



        /// <summary>
        /// 国外废纸收货(二次计量调用)/化工收货推送(一次计量推送)
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "国外废纸收货(二次计量调用)/化工收货推送(一次计量推送)")]
        public RETURN_SAP SAP_POSTCS(string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();

            #region 测试
            //EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            //entity.EOS_GM_CODE = "01";
            //entity.DOC_DATE =  DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PSTNG_DATE =  DateTime.Now.ToString("yyyy-MM-dd");
            //entity.BILL_OF_LADING = "";
            //entity.GR_GI_SLIP_NO = "";
            //entity.PO_NUMBER = "4700007845";//订单编号
            //entity.PO_ITEM = "00010";//订单项目编号
            //entity.PLANT = "1000";//工厂
            //entity.MATERIAL = "3010025";//物料
            //entity.BATCH = "2021100101";//批次
            //entity.ENTRY_QNT = 2000;//数量
            //entity.STGE_LOC = "XB05";// "Y490";//库存地点
            //entity.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            //entity.MVT_IND = "B";//移动标识,B,F,B是采购订单  F是生产订单
            //entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空

            //entity.LIFNR = "104509";
            //entity.EBELN = "4700007845";
            //entity.MATNR = "000000000003010025";
            //entity.EINDT = DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PQ = "N";
            //entity.NETPR = 2021;
            //entity.WERKS = "1000";
            #endregion

            #region 初始化接口
            #region Z_PO_POST
            SAPSERVICES_ZP1.SI_Z_PO_POST_OUTClient sap = new SAPSERVICES_ZP1.SI_Z_PO_POST_OUTClient("HTTPS_PORTPOST");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_ZP1.Z_PO_POST z_po_post = new SAPSERVICES_ZP1.Z_PO_POST();
            z_po_post.GOODSMVT_CODE = new SAPSERVICES_ZP1.BAPI2017_GM_CODE();
            z_po_post.GOODSMVT_CODE.GM_CODE = "01";//01采购订单收货,02生产订单收货
            z_po_post.GOODSMVT_HEADER = new SAPSERVICES_ZP1.BAPI2017_GM_HEAD_01();
            z_po_post.GOODSMVT_HEADER.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");//凭证中的凭证日期,当前系统日期
            z_po_post.GOODSMVT_HEADER.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");//凭证中的过账日期-可人工修改重新传SAP,系统当前日期
            z_po_post.GOODSMVT_HEADER.BILL_OF_LADING = "";//收货时提单号
            z_po_post.GOODSMVT_HEADER.GR_GI_SLIP_NO = "";//收货/发货单编号
            z_po_post.GOODSMVT_HEADER.HEADER_TXT = "无人计量采购过账";//凭证抬头文本
            #endregion

            #region GOODSMVT_HEADER
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO = "";//参考凭证编号
            //z_po_post.GOODSMVT_HEADER.PR_UNAME = "";//用户名 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIP = "";//收货/发货单打印版本 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIPX = "";//相关用户数据字段的已更新信息
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO_LONG = "";//参考凭证编号(对相关性参看长文本)
            //z_po_post.GOODSMVT_HEADER.BILL_OF_LADING_LONG = "";//组的检配单编号(根据：参见长文本)
            //z_po_post.GOODSMVT_HEADER.BAR_CODE = "";//条形码输入
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账 
            #endregion

            #region 初始化ZMSEG_C
            z_po_post.ZMSEG_C = new SAPSERVICES_ZP1.ZMSEG_C();
            //z_po_post.ZMSEG_C.MANDT = "";//集团
            //z_po_post.ZMSEG_C.MBLNR = "";//物料凭证编号
            //z_po_post.ZMSEG_C.MJAHR = "";//物料凭证的年份

            //z_po_post.ZMSEG_C.CHEHO = "";//车牌号
            //z_po_post.ZMSEG_C.XNHAO = "";//箱号
            //z_po_post.ZMSEG_C.CTNER = "";//柜型
            //z_po_post.ZMSEG_C.JKDAT = "";//进口日期
            //z_po_post.ZMSEG_C.LIFN1 = "";//供应商或债券人的账户
            //z_po_post.ZMSEG_C.NAME9 = "";//名称


            z_po_post.ZMSEG_C.LIFNR = "";//供应商或债券人的账户
            z_po_post.ZMSEG_C.EBELN = "";//采购凭证编号
            z_po_post.ZMSEG_C.MATNR = "";//物料编号
            z_po_post.ZMSEG_C.MENGE = 0;//采购订单数量
            z_po_post.ZMSEG_C.MENGESpecified = true;
            z_po_post.ZMSEG_C.EINDT = "";//项目交货日期
            z_po_post.ZMSEG_C.WERKS = "";//工厂
            z_po_post.ZMSEG_C.NETPR = 0;//净价
            z_po_post.ZMSEG_C.NETPRSpecified = true;
            z_po_post.ZMSEG_C.PQ = "";//纸质类型
            #endregion

            #region 初始化 GOODSMVT_ITEM
            z_po_post.GOODSMVT_ITEM = new SAPSERVICES_ZP1.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_ZP1.BAPI2017_GM_ITEM_CREATE gm_item = new SAPSERVICES_ZP1.BAPI2017_GM_ITEM_CREATE();

            gm_item.MATERIAL = "000000000003010025";//物料
            gm_item.PLANT = "1000";//工厂
            gm_item.STGE_LOC = "XB05";//库存地点
            gm_item.BATCH = "2021101101";//批次
            gm_item.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            gm_item.ENTRY_QNT = 1;//数量
            gm_item.ENTRY_QNTSpecified = true;
            gm_item.PO_NUMBER = "";//订单编号
            gm_item.PO_ITEM = "0000";//订单项目编号
            gm_item.ORDERID = "000001000067";
            gm_item.ORDER_ITNO = "0002";
            gm_item.MVT_IND = "F";//移动标识,B,F,B是采购订单  F是生产订单

            gm_item.S_ORD_ITEM = "000000";
            gm_item.SCHED_LINE = "0000";
            gm_item.PO_PR_QNT = 0;
            gm_item.RESERV_NO = "0000000000";
            gm_item.RES_ITEM = "0000";
            gm_item.MOVE_REAS = "0000";
            gm_item.PROFIT_SEGM_NO = "0000000000";
            gm_item.AMOUNT_LC = 0;
            gm_item.AMOUNT_SV = 0;
            gm_item.REF_DOC_IT = "0000";
            gm_item.VAL_S_ORD_ITEM = "000000";
            gm_item.DELIV_NUMB_TO_SEARCH = "000000";
            gm_item.SU_PL_STCK_1 = 0;
            gm_item.ST_UN_QTYY_1 = 0;
            gm_item.SU_PL_STCK_2 = 0;
            gm_item.ST_UN_QTYY_2 = 0;
            gm_item.MATITEM_TR_CANCEL = "0000";
            gm_item.LINE_ID = "000000";
            gm_item.PARENT_ID = "000000";
            gm_item.LINE_DEPTH = "00";
            gm_item.QUANTITY = 0;
            gm_item.EARMARKED_ITEM = "000";

            gm_item.SPEC_STOCK = "";//特殊库存标识,K寄售,空

            z_po_post.GOODSMVT_ITEM[0] = gm_item;
            #endregion

            #region 初始化 RETURN
            z_po_post.RETURN = new SAPSERVICES_ZP1.BAPIRET2[1];
            SAPSERVICES_ZP1.BAPIRET2 bapiret = new SAPSERVICES_ZP1.BAPIRET2();

            //bapiret.ID = "";
            //bapiret.TYPE = "";
            //bapiret.NUMBER = "";
            //bapiret.MESSAGE = "";
            //bapiret.LOG_NO = "";
            //bapiret.LOG_MSG_NO = "";
            //bapiret.MESSAGE_V1 = "";
            //bapiret.MESSAGE_V2 = "";
            //bapiret.MESSAGE_V3 = "";
            //bapiret.MESSAGE_V4 = "";
            //bapiret.PARAMETER = "";
            //bapiret.ROW = "";
            //bapiret.FIELD = "";
            //bapiret.SYSTEM = "";
            z_po_post.RETURN[0] = bapiret;
            #endregion
            #endregion
            #region 执行接口
            var resulte = sap.SI_Z_PO_POST_OUT(z_po_post);
            if (resulte.RC == "0")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                string PRUEFLOS = resulte.PRUEFLOS;//检验批号
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                //物料凭证号(推送单据号) MATERIALDOCUMENT = "5003845836"
                String MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                REDATA redata = new REDATA();
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;//凭证编号
                redata.PRUEFLOS = PRUEFLOS;
                redata.E_POSNR = resulte.E_POSNR;
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    ret.REMSG = string.Format("失败：{0}", resulte.RETURN[0].MESSAGE);
                }
                ret.REDATA = null;
            }
            #endregion

            return ret;
        }


        /// <summary>
        /// CO生产订单收货过账(区块链预约二次计量推送)
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "CO生产订单收货过账(区块链预约二次计量推送)")]
        public RETURN_SAP SAP_POST110(EOS_Z_PO_POST entity, string strjsonw)
        {
            RETURN_SAP ret = new RETURN_SAP();
            #region 测试数据
            //EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            //entity.EOS_GM_CODE = "01";//01采购订单收货,02生产订单收货
            //entity.DOC_DATE = "2021-08-13";// DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PSTNG_DATE = "2021-08-13";//DateTime.Now.ToString("yyyy-MM-dd");
            //entity.BILL_OF_LADING = "";
            //entity.GR_GI_SLIP_NO = "";
            //entity.ORDERID = "1000070";//订单编号
            //entity.ORDER_ITNO = "3";//订单项目编号
            //entity.MATERIAL = "000000000003010003";//物料
            //entity.BATCH = "2021091303";//批次
            //entity.ENTRY_QNT = 2;//数量
            //entity.STGE_LOC = "Y001";//库存地点  
            //entity.PLANT = "1000";//工厂
            //entity.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            //entity.MVT_IND = "F";//移动标识,B,F,B是采购订单  F是生产订单
            //entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.EOS_GM_CODE))
            {
                entity.EOS_GM_CODE = "01";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(entity.PSTNG_DATE))
            {
                entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }

            #endregion

            #region 初始化接口
            SAPSERVICES_ZP1.SI_Z_PO_POST_OUTClient sap = new SAPSERVICES_ZP1.SI_Z_PO_POST_OUTClient("HTTPS_PORTPOST");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];

            SAPSERVICES_ZP1.Z_PO_POST z_po_post = new SAPSERVICES_ZP1.Z_PO_POST();
            z_po_post.GOODSMVT_CODE = new SAPSERVICES_ZP1.BAPI2017_GM_CODE();
            z_po_post.GOODSMVT_CODE.GM_CODE = entity.EOS_GM_CODE;//01采购订单收货,02生产订单收货
            z_po_post.GOODSMVT_HEADER = new SAPSERVICES_ZP1.BAPI2017_GM_HEAD_01();
            z_po_post.GOODSMVT_HEADER.DOC_DATE = entity.DOC_DATE;//DateTime.Now.ToString("yyyy-MM-dd");//凭证中的凭证日期,当前系统日期
            z_po_post.GOODSMVT_HEADER.PSTNG_DATE = entity.PSTNG_DATE;// DateTime.Now.ToString("yyyy-MM-dd");//凭证中的过账日期-可人工修改重新传SAP,系统当前日期
            z_po_post.GOODSMVT_HEADER.BILL_OF_LADING = entity.BILL_OF_LADING;//收货时提单号
            z_po_post.GOODSMVT_HEADER.GR_GI_SLIP_NO = entity.GR_GI_SLIP_NO;//收货/发货单编号
            z_po_post.GOODSMVT_HEADER.HEADER_TXT = "无人计量采购过账";//凭证抬头文本
            #endregion

            #region GOODSMVT_HEADER
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO = "";//参考凭证编号
            //z_po_post.GOODSMVT_HEADER.PR_UNAME = "";//用户名 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIP = "";//收货/发货单打印版本 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIPX = "";//相关用户数据字段的已更新信息
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO_LONG = "";//参考凭证编号(对相关性参看长文本)
            //z_po_post.GOODSMVT_HEADER.BILL_OF_LADING_LONG = "";//组的检配单编号(根据：参见长文本)
            //z_po_post.GOODSMVT_HEADER.BAR_CODE = "";//条形码输入
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账 
            #endregion


            #region  初始化 ZMSEG_C
            z_po_post.ZMSEG_C = new SAPSERVICES_ZP1.ZMSEG_C();
            //z_po_post.ZMSEG_C.MANDT = "";//集团
            //z_po_post.ZMSEG_C.MBLNR = "";//物料凭证编号
            //z_po_post.ZMSEG_C.MJAHR = "";//物料凭证的年份
            //z_po_post.ZMSEG_C.LIFNR = "";//供应商或债券人的账户
            //z_po_post.ZMSEG_C.CHEHO = "";//车牌号
            //z_po_post.ZMSEG_C.XNHAO = "";//箱号
            //z_po_post.ZMSEG_C.CTNER = "";//柜型
            //z_po_post.ZMSEG_C.JKDAT = "";//进口日期
            //z_po_post.ZMSEG_C.LIFN1 = "";//供应商或债券人的账户
            //z_po_post.ZMSEG_C.NAME9 = "";//名称
            //z_po_post.ZMSEG_C.EBELN = "";//采购凭证编号
            //z_po_post.ZMSEG_C.MATNR = "";//物料编号
            //z_po_post.ZMSEG_C.MENGE = 0;//采购订单数量
            //z_po_post.ZMSEG_C.EINDT = "";//项目交货日期
            //z_po_post.ZMSEG_C.WERKS = "";//工厂
            //z_po_post.ZMSEG_C.NETPR = 0;//净价
            //z_po_post.ZMSEG_C.PQ = "";//纸质类型
            #endregion

            #region 初始化 GOODSMVT_ITEM
            z_po_post.GOODSMVT_ITEM = new SAPSERVICES_ZP1.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_ZP1.BAPI2017_GM_ITEM_CREATE gm_item = new SAPSERVICES_ZP1.BAPI2017_GM_ITEM_CREATE();
            gm_item.ORDERID = entity.ORDERID;//订单编号
            gm_item.ORDER_ITNO = entity.ORDER_ITNO;//订单项目编号
            gm_item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            gm_item.MVT_IND = entity.MVT_IND;//移动标识,B,F,B是采购订单  F是生产订单
            gm_item.SPEC_STOCK = entity.SPEC_STOCK;//特殊库存标识,K寄售,空
            gm_item.STGE_LOC = entity.STGE_LOC;//库存地点
            gm_item.BATCH = entity.BATCH;//批次
            gm_item.ENTRY_QNT = entity.ENTRY_QNT;//数量
            gm_item.ENTRY_QNTSpecified = entity.ENTRY_QNTSpecified;
            gm_item.PLANT = entity.PLANT;//工厂
            gm_item.MATERIAL = entity.MATERIAL;//物料
            #endregion

            #region   GOODSMVT_ITEM

            // gm_item.STCK_TYPE = "2";//库存类型:化工：2质量检验 废纸：空 非限制使用 3已冻结
            ////单位
            //gm_item.ENTRY_UOM = "";
            ////计量单位ISO代码
            //gm_item.ENTRY_UOM_ISO = "";
            ////采购订单价格单位的数量
            //gm_item.PO_PR_QNT = 0;
            ////订单价格单位(采购)
            //gm_item.ORDERPR_UN = "";
            ////计量单位ISO代码
            //gm_item.ORDERPR_UN_ISO = "";
            //gm_item.PO_NUMBER = "4300004787";//采购订单号
            //gm_item.PO_ITEM = "10";//采购凭证的项目编号
            //gm_item.VENDOR = "106544";//供应商账户号

            //gm_item.SHIPPING = "";//装运指令
            //gm_item.COMP_SHIP = "";//依照装运通知
            //gm_item.NO_MORE_GR = "";//交货已完成标识 
            //gm_item.ITEM_TEXT = "";//项目文本 
            //gm_item.GR_RCPT = "";//收货方 
            //gm_item.UNLOAD_PT = "";//卸货点
            //gm_item.COSTCENTER = "";//成本中心
            //gm_item.VAL_TYPE = "";//评估类型
            //gm_item.CUSTOMER = "";//客户的账户编号
            //gm_item.SALES_ORD = "";//销售订单数
            //gm_item.S_ORD_ITEM = "";//销售订单中的项目编号
            //gm_item.SCHED_LINE = "";//销售订单交货计划
            z_po_post.GOODSMVT_ITEM[0] = gm_item;
            #endregion

            #region 初始化 RETURN
            z_po_post.RETURN = new SAPSERVICES_ZP1.BAPIRET2[1];
            SAPSERVICES_ZP1.BAPIRET2 bapiret = new SAPSERVICES_ZP1.BAPIRET2();
            //bapiret.ID = "";
            //bapiret.TYPE = "";
            //bapiret.NUMBER = "";
            //bapiret.MESSAGE = "";
            //bapiret.LOG_NO = "";
            //bapiret.LOG_MSG_NO = "";
            //bapiret.MESSAGE_V1 = "";
            //bapiret.MESSAGE_V2 = "";
            //bapiret.MESSAGE_V3 = "";
            //bapiret.MESSAGE_V4 = "";
            //bapiret.PARAMETER = "";
            //bapiret.ROW = "";
            //bapiret.FIELD = "";
            //bapiret.SYSTEM = "";
            z_po_post.RETURN[0] = bapiret;
            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_PO_POST_OUT(z_po_post);
            if (resulte.RC == "0")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                string E_POSNR = resulte.E_POSNR;
                string PRUEFLOS = resulte.PRUEFLOS;
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                //物料凭证号(推送单据号) MATERIALDOCUMENT = "5003845836"
                String MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                REDATA redata = new REDATA();
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;
                redata.PRUEFLOS = PRUEFLOS;
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    ret.REMSG = string.Format("失败：{0}", resulte.RETURN[0].MESSAGE);
                }
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }

        /// <summary>
        /// 国内废纸收货推送(二次计量调用)
        /// </summary>
        /// <param name="entity">参数对象</param>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "国内废纸收货推送(二次计量调用)")]
        public RETURN_SAP SAP_POST2(EOS_Z_PO_POST2 entity, string strjsonw)
        {
            //返回对象
            RETURN_SAP ret = new RETURN_SAP();
            #region 测试数据
            //EOS_Z_PO_POST2 entity = new EOS_Z_PO_POST2();
            //entity.EOS_GM_CODE = "02";
            //entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PO_NUMBER = "4700007833";//订单号
            //entity.PO_ITEM = "10";//订单行号
            //entity.MOVE_TYPE = "101";  //移动类型：废纸101,化工,包装物103
            //entity.MVT_IND = "B";//移动标识
            //entity.SPEC_STOCK = "";//特殊库存标识
            //entity.ENTRY_QNT = 1;//数量
            //entity.ENTRY_QNTSpecified = true;
            //entity.STGE_LOC = "Y490";//库存地点
            //entity.BATCH = "202107";//批次
            //entity.MATERIAL = "000000000003010064";//物料
            //entity.PLANT = "9000";//工厂
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.EOS_GM_CODE))
            {
                entity.EOS_GM_CODE = "02";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(entity.PSTNG_DATE))
            {
                entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }

            #endregion

            #region 初始化接口
            SAPSERVICES_ZP2.SI_Z_PO_POST2_OUTClient sap = new SAPSERVICES_ZP2.SI_Z_PO_POST2_OUTClient("HTTPS_PORTPOST2");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_ZP2.Z_PO_POST2 z_po_post = new SAPSERVICES_ZP2.Z_PO_POST2();
            z_po_post.GOODSMVT_CODE = new SAPSERVICES_ZP2.BAPI2017_GM_CODE();
            z_po_post.GOODSMVT_CODE.GM_CODE = "02";
            z_po_post.GOODSMVT_HEADER = new SAPSERVICES_ZP2.BAPI2017_GM_HEAD_01();
            z_po_post.GOODSMVT_HEADER.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");//凭证中的凭证日期
            z_po_post.GOODSMVT_HEADER.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");//凭证中的过账日期

            #endregion

            #region  初始化 GOODSMVT_ITEM
            z_po_post.GOODSMVT_ITEM = new SAPSERVICES_ZP2.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_ZP2.BAPI2017_GM_ITEM_CREATE gm_item = new SAPSERVICES_ZP2.BAPI2017_GM_ITEM_CREATE();
            gm_item.PO_NUMBER = entity.PO_NUMBER;//订单号
            gm_item.PO_ITEM = entity.PO_ITEM;//订单行号
            gm_item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型：废纸101,化工,包装物103
            gm_item.MVT_IND = entity.MVT_IND;//移动标识
            gm_item.SPEC_STOCK = entity.SPEC_STOCK;//特殊库存标识
            gm_item.ENTRY_QNT = entity.ENTRY_QNT;//数量
            gm_item.ENTRY_QNTSpecified = entity.ENTRY_QNTSpecified;
            gm_item.STGE_LOC = entity.STGE_LOC;//库存地点
            gm_item.BATCH = entity.BATCH;//批次
            gm_item.MATERIAL = entity.MATERIAL;//物料
            gm_item.PLANT = entity.PLANT;//工厂
            z_po_post.GOODSMVT_ITEM[0] = gm_item;
            #endregion

            #region RETURN
            z_po_post.RETURN = new SAPSERVICES_ZP2.BAPIRET2[1];
            SAPSERVICES_ZP2.BAPIRET2 bapiret = new SAPSERVICES_ZP2.BAPIRET2();
            z_po_post.RETURN[0] = bapiret;
            z_po_post.PINP = "";
            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_PO_POST2_OUT(z_po_post);

            if (resulte.RC == "0")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                REDATA redata = new REDATA();
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                //物料凭证号(推送单据号) MATERIALDOCUMENT = "5003845836"
                string MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;
                redata.PRUEFLOS = "";
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    ret.REMSG = string.Format("失败：{0}", resulte.RETURN[0].MESSAGE);
                }
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }


        /// <summary>
        ///  SAP化工-包装物收货返回检验批(二次计量后调用)
        /// </summary>
        /// <param name="EOS_ZFMM098">参数对象</param>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = " SAP化工-包装物105收货+检验批号使用决策")]
        public RETURN_SAP SAP_98(EOS_ZFMM098 entity, string strjson)
        {
            #region  测试数据
            //EOS_ZFMM098 entity = new EOS_ZFMM098();
            //entity.PRUEFLOS = "10000191350";//检验批次编号
            //entity.LGORT = "WJ01";//库存地点
            //entity.FXZKC = "8";
            //entity.THJH = "2";
            //entity.JYJG = "2";
            //entity.VCODE = "001";
            #endregion

            #region 初始化接口
            SAPSERVICES_98.SI_ZFMM098_OUTClient sap = new SAPSERVICES_98.SI_ZFMM098_OUTClient("HTTPS_PORT98");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            #endregion

            #region 赋值
            SAPSERVICES_98.ZFMM098 mm98 = new SAPSERVICES_98.ZFMM098();
            if (!string.IsNullOrEmpty(entity.PRUEFLOS))
            {
                mm98.I_PRUEFLOS = entity.PRUEFLOS;// "10000191350";//检验批次编号
            }
            if (!string.IsNullOrEmpty(entity.LGORT))
            {
                mm98.I_LGORT = entity.LGORT;// "WJ01";//过账到未限制库存的数量的存储位置(库存地点)
            }
            if (!string.IsNullOrEmpty(entity.FXZKC))
            {
                mm98.I_FXZKC = entity.FXZKC;//将过账到非限制使用库存的数量(净重-扣重)
            }
            if (!string.IsNullOrEmpty(entity.THJH))
            {
                mm98.I_THJH = entity.THJH;//以退货到供应商形式过账的数量(皮+扣重)
            }
            if (!string.IsNullOrEmpty(entity.JYJG))
            {
                mm98.I_JYJG = entity.JYJG;//检验结果数(SAP提供获取检验结果数)
            }
            else
            {
                mm98.I_JYJG = "";
            }
            if (!string.IsNullOrEmpty(entity.VCODE))
            {
                mm98.I_VCODE = entity.VCODE;//使用决策代码(默认:001)
            }
            #endregion

            #region 执行接口
            var resulte = sap.SI_ZFMM098_OUT(mm98);
            RETURN_SAP ret = new RETURN_SAP();
            if (resulte.E_RETURN.TYPE == "S")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = string.Format("失败：{0}", resulte.E_RETURN.MESSAGE);
                ret.REDATA = null;
            }
            #endregion

            return ret;
        }

        /// <summary>
        /// AJAX调用
        /// </summary>
        /// <param name="strjson"></param>
        /// <returns></returns>
        [WebMethod(Description = " SAP化工-包装物105收货+检验批号使用决策")]
        public RETURN_SAP SAP_98A(string PRUEFLOS, string LGORT, string FXZKC, string THJH, string JYJG, string VCODE, string strjson)
        {
            EOS_ZFMM098 entity = new EOS_ZFMM098();
            entity.PRUEFLOS = PRUEFLOS;//检验批号
            entity.LGORT = LGORT;//库存地点
            entity.FXZKC = FXZKC; //(净重 - 扣重)
            entity.THJH = THJH;//(皮+扣重)
            entity.JYJG = JYJG;//检验结果数
            if (string.IsNullOrEmpty(VCODE))
            {
                VCODE = "001";
            }
            else
            {
                entity.VCODE = VCODE;
            }
            return SAP_98(entity, "");
        }


        /// <summary>
        /// CO生产订单收货过账(区块链预约二次计量推送)(不使用)
        /// </summary>
        /// <param name="strjson"></param>
        /// <returns></returns>
        [WebMethod(Description = " CO生产订单收货过账(区块链预约二次计量推送)")]
        public RETURN_SAP SAP_110(EOS_ZFMM110 entity, string strjson)
        {
            #region 测试数据
            //EOS_ZFMM110 entity = new EOS_ZFMM110();
            //entity.AUFNR = "1000070";//订单号
            //entity.CHARG = "2021091301";//批号
            //entity.LGORT = "Y001";//库存地点
            //entity.ERFMG = 10;//数量
            #endregion

            #region 初始化接口
            SAPSERVICES_110.SI_ZFMM110_OUTClient sap = new SAPSERVICES_110.SI_ZFMM110_OUTClient("HTTPS_PORT110");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            #endregion

            #region 初始化 ZFMM110
            SAPSERVICES_110.ZFMM110 mm110 = new SAPSERVICES_110.ZFMM110();
            mm110.I_AUFNR = entity.AUFNR;// "1000070";//订单号
            mm110.I_CHARG = entity.CHARG;// "2021091301";//批号
            mm110.I_LGORT = entity.LGORT;// "Y001";//库存地点
            mm110.I_ERFMG = entity.ERFMG;// 10;//数量
            #endregion

            #region 执行接口
            var resulte = sap.SI_ZFMM110_OUT(mm110);
            RETURN_SAP ret = new RETURN_SAP();
            if (resulte.E_RETURN.TYPE == "S")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                REDATA redata = new REDATA();
                string E_MBLNR = resulte.E_MBLNR;
                string E_MJAHR = resulte.E_MJAHR;
                redata.E_MBLNR = E_MBLNR;
                redata.E_MJAHR = E_MJAHR;
                redata.PRUEFLOS = "";
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = string.Format("失败：{0}", resulte.E_RETURN.MESSAGE);
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }


        /// <summary>
        /// 货物移库
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "货物移库")]
        public RETURN_SAP SAP_GMPOST(EOS_Z_GM_POST entity, string strjsonw)
        {
            #region 测试参数

            //EOS_Z_GM_POST entity = new EOS_Z_GM_POST();
            //entity.GM_CODE = "04";
            //entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            //entity.MOVE_TYPE = "311";//移动类型
            //entity.ENTRY_UOM = "KG";//计量单位
            //entity.MATERIAL = "000000000003010064";
            //entity.BATCH = "2021091302";//原批次
            //entity.MOVE_BATCH = "2021091302";//现批次
            //entity.ENTRY_QNT = 1;
            //entity.ENTRY_QNTSpecified = true;
            //entity.STGE_LOC = "Y490";//发货仓库
            //entity.MOVE_STLOC = "Y502";// "Y502";//收货仓库
            //entity.PLANT = "9000";//原工厂
            //entity.MOVE_PLANT = "9000";//收货工厂 
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.GM_CODE))
            {
                entity.GM_CODE = "04";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }
            entity.ENTRY_QNTSpecified = true;

            #endregion

            #region 验证
            RETURN_SAP ret = new RETURN_SAP();
            if (entity.STGE_LOC == entity.MOVE_STLOC)
            {
                ret.STATUS = "E";
                ret.REMSG = "发货仓库和收货仓库不能相同！";
                ret.REDATA = null;
                return ret;
            }
            #endregion

            #region 初始化接口
            SAPSERVICES_HWYK.SI_Z_GM_POST_OUTClient sap = new SAPSERVICES_HWYK.SI_Z_GM_POST_OUTClient("HTTPS_PORTGMPOST");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_HWYK.Z_GM_POST z_gm_post = new SAPSERVICES_HWYK.Z_GM_POST();
            z_gm_post.GOODSMVT_CODE = new SAPSERVICES_HWYK.BAPI2017_GM_CODE();
            z_gm_post.GOODSMVT_CODE.GM_CODE = entity.GM_CODE;//04
            z_gm_post.GOODSMVT_HEADER = new SAPSERVICES_HWYK.BAPI2017_GM_HEAD_01();
            z_gm_post.GOODSMVT_HEADER.DOC_DATE = entity.DOC_DATE;// DateTime.Now.ToString("yyyy-MM-dd");
            #endregion

            #region 初始化 GOODSMVT_ITEM
            z_gm_post.GOODSMVT_ITEM = new SAPSERVICES_HWYK.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_HWYK.BAPI2017_GM_ITEM_CREATE item = new SAPSERVICES_HWYK.BAPI2017_GM_ITEM_CREATE();
            item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型
            item.ENTRY_UOM = entity.ENTRY_UOM;//计量单位
            item.MATERIAL = entity.MATERIAL;
            item.BATCH = entity.BATCH;//原批次
            item.MOVE_BATCH = entity.MOVE_BATCH;//现批次
            item.ENTRY_QNT = entity.ENTRY_QNT;
            item.ENTRY_QNTSpecified = entity.ENTRY_QNTSpecified;
            item.STGE_LOC = entity.STGE_LOC;//发货仓库
            item.MOVE_STLOC = entity.MOVE_STLOC;// "Y502";//收货仓库
            item.PLANT = entity.PLANT;//原工厂
            item.MOVE_PLANT = entity.MOVE_PLANT;//收货工厂 
            z_gm_post.GOODSMVT_ITEM[0] = item;
            #endregion

            #region  初始化 RETURN
            z_gm_post.RETURN = new SAPSERVICES_HWYK.BAPIRET2[1];
            SAPSERVICES_HWYK.BAPIRET2 bapiret2 = new SAPSERVICES_HWYK.BAPIRET2();
            z_gm_post.RETURN[0] = bapiret2;
            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_GM_POST_OUT(z_gm_post);
            if (resulte.RC == "0" && resulte.MATDOCUMENTYEAR != "0000")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                REDATA redata = new REDATA();
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                string MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;
                redata.PRUEFLOS = "";
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    string REMSG = "";
                    foreach (SAPSERVICES_HWYK.BAPIRET2 pi in resulte.RETURN)
                    {
                        REMSG += pi.MESSAGE;
                    }
                    ret.REMSG = string.Format("失败：{0}", REMSG);
                    ret.REDATA = null;
                }
            }
            #endregion
            return ret;
        }


        /// <summary>
        /// 新货物移库
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "新货物移库")]
        public RETURN_SAP SAP_GMPOST4(EOS_Z_GM_POST entity, string strjsonw)
        {
            #region 测试参数

            //EOS_Z_GM_POST entity = new EOS_Z_GM_POST();
            //entity.GM_CODE = "04";
            //entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            //entity.MOVE_TYPE = "311";//移动类型
            //entity.ENTRY_UOM = "KG";//计量单位
            //entity.MATERIAL = "000000000003010064";
            //entity.BATCH = "2021091302";//原批次
            //entity.MOVE_BATCH = "2021091302";//现批次
            //entity.ENTRY_QNT = 1;
            //entity.ENTRY_QNTSpecified = true;
            //entity.STGE_LOC = "Y490";//发货仓库
            //entity.MOVE_STLOC = "Y502";// "Y502";//收货仓库
            //entity.PLANT = "9000";//原工厂
            //entity.MOVE_PLANT = "9000";//收货工厂 
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.GM_CODE))
            {
                entity.GM_CODE = "04";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }
            entity.ENTRY_QNTSpecified = true;

            #endregion

            #region 验证
            RETURN_SAP ret = new RETURN_SAP();
            if (entity.STGE_LOC == entity.MOVE_STLOC)
            {
                ret.STATUS = "E";
                ret.REMSG = "发货仓库和收货仓库不能相同！";
                ret.REDATA = null;
                return ret;
            }
            #endregion

            #region 初始化接口
            SAPSERVICES_HWYKCS.SI_Z_GM_POST4_OUTClient sap = new SAPSERVICES_HWYKCS.SI_Z_GM_POST4_OUTClient("HTTPS_PORTGMPOST4");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_HWYKCS.Z_GM_POST4 z_gm_post = new SAPSERVICES_HWYKCS.Z_GM_POST4();
            z_gm_post.GOODSMVT_CODE = new SAPSERVICES_HWYKCS.BAPI2017_GM_CODE();
            z_gm_post.GOODSMVT_CODE.GM_CODE = entity.GM_CODE;//04
            z_gm_post.GOODSMVT_HEADER = new SAPSERVICES_HWYKCS.BAPI2017_GM_HEAD_01();
            z_gm_post.GOODSMVT_HEADER.DOC_DATE = entity.DOC_DATE;// DateTime.Now.ToString("yyyy-MM-dd");
            z_gm_post.GOODSMVT_HEADER.PSTNG_DATE = entity.DOC_DATE;
            #endregion

            #region 初始化 GOODSMVT_ITEM
            z_gm_post.GOODSMVT_ITEM = new SAPSERVICES_HWYKCS.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_HWYKCS.BAPI2017_GM_ITEM_CREATE item = new SAPSERVICES_HWYKCS.BAPI2017_GM_ITEM_CREATE();
            item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型
            item.ENTRY_UOM = entity.ENTRY_UOM;//计量单位
            item.MATERIAL = entity.MATERIAL;
            item.BATCH = entity.BATCH;//原批次
            item.MOVE_BATCH = entity.MOVE_BATCH;//现批次
            item.ENTRY_QNT = entity.ENTRY_QNT;
            item.ENTRY_QNTSpecified = entity.ENTRY_QNTSpecified;
            item.STGE_LOC = entity.STGE_LOC;//发货仓库
            item.MOVE_STLOC = entity.MOVE_STLOC;// "Y502";//收货仓库
            item.PLANT = entity.PLANT;//原工厂
            item.MOVE_PLANT = entity.MOVE_PLANT;//收货工厂 
            z_gm_post.GOODSMVT_ITEM[0] = item;
            #endregion

            #region  初始化 RETURN
            z_gm_post.RETURN = new SAPSERVICES_HWYKCS.BAPIRET2[1];
            SAPSERVICES_HWYKCS.BAPIRET2 bapiret2 = new SAPSERVICES_HWYKCS.BAPIRET2();
            z_gm_post.RETURN[0] = bapiret2;
            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_GM_POST_OUT(z_gm_post);
            if (resulte.RC == "0" && resulte.MATDOCUMENTYEAR != "0000")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                REDATA redata = new REDATA();
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                string MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;
                redata.PRUEFLOS = "";
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    string REMSG = "";
                    foreach (SAPSERVICES_HWYKCS.BAPIRET2 pi in resulte.RETURN)
                    {
                        REMSG += pi.MESSAGE;
                    }
                    ret.REMSG = string.Format("失败：{0}", REMSG);
                    ret.REDATA = null;
                }
            }
            #endregion
            return ret;
        }
        #endregion

        #region 质检业务

        [WebMethod(Description = "AJAX质检检验计划(一次计量收货成功返回检验批号，凭证号推送)")]
        public RETURN_SAP MES_PURCHASE1(string WERKS, string EBELN, string ITEM_LINE, string PRUEFLOS, string MATNR, string POUND_ORDER, string CHARG, double GROSS_WEIGHT, string STOCK_LOCATION, string DELIVERY_DATE, string BUYER, string ACCEPTANCE)
        {

            EOS_ZTTQM001 entity = new EOS_ZTTQM001();
            #region 测试数据
            entity.WERKS = WERKS;//工厂
            entity.EBELN = EBELN; //采购订单号
            entity.ITEM_LINE = ITEM_LINE;// 订单行项目  
            entity.PRUEFLOS = PRUEFLOS;//检验批次号
            entity.MATNR = MATNR;//物料号
            entity.POUND_ORDER = POUND_ORDER; //磅单号
            entity.CHARG = CHARG;//批次号10位的批次号,格式：yymmdd0000
            entity.GROSS_WEIGHT = GROSS_WEIGHT;//毛重/到货数量
            entity.PACKAGES = "";//包装件数
            entity.STOCK_LOCATION = STOCK_LOCATION;//库存地点
            entity.DELIVERY_DATE = DELIVERY_DATE;//来货日期
            entity.BUYER = BUYER;//采购员
            entity.ACCEPTANCE = ACCEPTANCE;//验收人员
            entity.LONG_MATERIAL = "1";//是否属于检验时间较长物料，是（0）/否（1）
            entity.INSPECTION_TYPE = "JYLX40";//检验类型
            entity.TASK_STATUS = "NEW";//任务状态
            entity.TASK_TYPE = "RWLX04";//任务类型
            entity.CREATED_DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//创建时间
            entity.CREATED_USER = "无人值守质检";//创建人
            #endregion
            return MES_PURCHASE(entity, "");
        }

        /// <summary>
        /// 质检检验计划
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "质检检验计划")]
        public RETURN_SAP MES_PURCHASE(EOS_ZTTQM001 entity, string strjsonw)
        {
            RETURN_SAP ret = new RETURN_SAP();

            #region 测试数据
            //EOS_ZTTQM001 entity = new EOS_ZTTQM001();
            //entity.WERKS = "7000";//工厂
            //entity.EBELN = "4100083819"; //采购订单号
            //entity.ITEM_LINE = "10";// 订单行项目  
            //entity.PRUEFLOS = "10000208388";//检验批次号
            //entity.MATNR = "4030022";//物料号
            //entity.POUND_ORDER = "CG21080700098"; //磅单号
            //entity.CHARG = "2106290001";//批次号10位的批次号,格式：yymmdd0000
            //entity.GROSS_WEIGHT = 22;//毛重/到货数量
            //entity.PACKAGES = "";//包装件数
            //entity.STOCK_LOCATION = "XB06";//库存地点
            //entity.DELIVERY_DATE = "2021-06-29";//来货日期
            //entity.BUYER = "秦国强";//采购员
            //entity.ACCEPTANCE = "刘丽娟";//验收人员
            //entity.LONG_MATERIAL = "1";//是否属于检验时间较长物料，是（0）/否（1）
            //entity.INSPECTION_TYPE = "JYLX40";//检验类型
            //entity.TASK_STATUS = "NEW";//任务状态
            //entity.TASK_TYPE = "RWLX04";//任务类型
            //entity.CREATED_DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//创建时间
            //entity.CREATED_USER = "无人值守质检";//创建人
            #endregion

            #region  初始化固定值
            if (string.IsNullOrEmpty(entity.CREATED_USER))
            {
                entity.CREATED_USER = "无人值守质检";//创建人 
            }
            if (string.IsNullOrEmpty(entity.CREATED_DATE_TIME))
            {
                entity.CREATED_DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//创建时间
            }
            if (string.IsNullOrEmpty(entity.TASK_TYPE))
            {
                entity.TASK_TYPE = "RWLX04";//任务类型
            }
            if (string.IsNullOrEmpty(entity.TASK_STATUS))
            {
                entity.TASK_STATUS = "NEW";//任务状态
            }
            #endregion

            #region 初始化接口
            SAPSERVICES_PURCHASE.SI_Z_ME_OAPURCHASEClient sap = new SAPSERVICES_PURCHASE.SI_Z_ME_OAPURCHASEClient("HTTPS_PORTPURCHASE");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_PURCHASE.Z_ME_OAPURCHASE z_me_purchase = new SAPSERVICES_PURCHASE.Z_ME_OAPURCHASE();
            #region 初始化 ZTTQM001
            z_me_purchase.T_ZTTQM001 = new SAPSERVICES_PURCHASE.ZTTQM001[1];
            SAPSERVICES_PURCHASE.ZTTQM001 zttqm001 = new SAPSERVICES_PURCHASE.ZTTQM001();
            #region 不可为空值
            zttqm001.PRUEFLOS = entity.PRUEFLOS;//检验批次号
            zttqm001.EBELN = entity.EBELN; //采购订单号

            zttqm001.ITEM_LINE = entity.ITEM_LINE;// 订单行项目
            zttqm001.MATNR = entity.MATNR;//物料号
            zttqm001.POUND_ORDER = entity.POUND_ORDER; //磅单号
            zttqm001.CHARG = entity.CHARG;//批次号10位的批次号,格式：yymmdd0000
            zttqm001.GROSS_WEIGHT = entity.GROSS_WEIGHT;//毛重/到货数量
            zttqm001.PACKAGES = entity.PACKAGES;//包装件数
            zttqm001.STOCK_LOCATION = entity.STOCK_LOCATION;//库存地点
            zttqm001.WERKS = entity.WERKS;//工厂
            zttqm001.DELIVERY_DATE = entity.DELIVERY_DATE;//来货日期
            zttqm001.BUYER = entity.BUYER;//采购员
            zttqm001.ACCEPTANCE = entity.ACCEPTANCE;//验收人员
            zttqm001.LONG_MATERIAL = entity.LONG_MATERIAL;//是否属于检验时间较长物料，是（0）/否（1）
            zttqm001.INSPECTION_TYPE = entity.INSPECTION_TYPE;//检验类型
            zttqm001.TASK_STATUS = entity.TASK_STATUS;//任务状态
            zttqm001.TASK_TYPE = entity.TASK_TYPE;//任务类型
            zttqm001.CREATED_DATE_TIME = entity.CREATED_DATE_TIME;
            zttqm001.CREATED_USER = entity.CREATED_USER;
            #endregion

            #region  可为空值
            zttqm001.CLOSE_REASON = "";
            zttqm001.CONFIRMED_DATE_TIME = "";// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            zttqm001.CONFIRMED_USER = "";
            zttqm001.EBELP = "";
            zttqm001.EXPLANATION = "";
            zttqm001.FLOW_CODE = "";
            zttqm001.INSPECTOR = "";
            zttqm001.INSPECT_END_TIME = "";
            zttqm001.INSPECT_START_TIME = "";
            zttqm001.INSPSAMPLE = "";
            zttqm001.MAKTX = "";
            zttqm001.MANDT = "";
            zttqm001.MBLNR = "";
            zttqm001.MESSAGE = "";
            zttqm001.MODIFIED_DATE_TIME = "";
            zttqm001.MODIFIED_USER = "";
            zttqm001.PROCESS_JUDGE = "";
            zttqm001.RESULT_JUDGE = "";
            zttqm001.STATUS = "";
            zttqm001.SYNCHRONOUS = "";
            #endregion
            z_me_purchase.T_ZTTQM001[0] = zttqm001;
            #endregion
            #region 初始化 ZTTQM001
            z_me_purchase.T_ZTTQM002 = new SAPSERVICES_PURCHASE.ZTTQM002[1];
            SAPSERVICES_PURCHASE.ZTTQM002 zttqm002 = new SAPSERVICES_PURCHASE.ZTTQM002();

            #region 不可为空值
            zttqm002.PRUEFLOS = entity.PRUEFLOS;//检验批次号
            zttqm002.WERKS = entity.WERKS;//工厂
            //zttqm002.INSPCHAR =""; //检验特征编号 不需要传输,ME内有取值逻辑
            zttqm002.INSPSAMPLE = "1";//检验次数
            zttqm002.VERWMERKM = entity.ITEM_LINE;// 主检验特征 QM0087（外观）/QM0086（检测报告）(分为两组数据传输结果）
            zttqm002.TASK_RESULT = entity.MATNR;//检验结果 201-001外观合格 204-001 有，字迹清晰
            zttqm002.TASK_STATUS = entity.TASK_STATUS;//任务状态 IF两个指标的判定结果均为PASS，则为NEW；IF两个指标判定结果存在FAIL,则为DONE
            zttqm002.RESULT_JUDGE = "";//判定结果 PASS/FAIL
            zttqm002.CHARG = entity.CHARG;//批次号10位的批次号,格式：yymmdd0000
            zttqm002.CREATED_DATE_TIME = entity.CREATED_DATE_TIME;//创建时间
            zttqm002.CREATED_USER = entity.CREATED_USER;//创建人
            #endregion

            #region  可为空值
            zttqm002.INSPECTOR = "";//检验人员
            zttqm002.INSPECT_END_TIME = "";//检验结束时间
            zttqm002.INSPECT_START_TIME = "";//检验开始时间
            zttqm002.MANDT = "";
            zttqm002.MODIFIED_DATE_TIME = "";//最后更新时间
            zttqm002.MODIFIED_USER = "";//最后更新人
            zttqm002.CONFIRMED_DATE_TIME = "";//确认时间
            zttqm002.CONFIRMED_USER = "";//确认人

            #endregion
            z_me_purchase.T_ZTTQM002[0] = zttqm002;
            #endregion
            #endregion

            #region 执行接口
            //log.Error($"质检计划:{entity.PRUEFLOS}开始执行！");
            var resulte = sap.SI_Z_ME_OAPURCHASE(z_me_purchase);
            if (resulte.E_CODE == "S")
            {
                //log.Error($"质检计划:{entity.PRUEFLOS}执行成功");
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                ret.REDATA = null;
            }
            else
            {
                //log.Error($"质检计划:{entity.PRUEFLOS}执行失败！");
                ret.STATUS = "E";
                string E_MESSAGE = resulte.E_MESSAGE;
                ret.REMSG = string.Format("失败：{0}", E_MESSAGE);
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }
        /// <summary>
        /// 上料调拨上传MES
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "上料调拨上传MES")]
        public RETURN_SAP MES_SLDB(string workShop, string storageLocation, string item, string batch, string transferType, string strjson)
        {
            MESSERVICES_SLDB.LoadometerWeighingTransferWSClient db = new MESSERVICES_SLDB.LoadometerWeighingTransferWSClient("LoadometerWeighingTransferWSPort");

            #region 初始化
            MESSERVICES_SLDB.loadometerWeighingTransferRequest rq = new MESSERVICES_SLDB.loadometerWeighingTransferRequest();
            rq.site = "1000";
            rq.userId = "ME_USER";
            rq.lineRequestList = new MESSERVICES_SLDB.loadometerWeighingTransferLineRequest[1];
            MESSERVICES_SLDB.loadometerWeighingTransferLineRequest wr = new MESSERVICES_SLDB.loadometerWeighingTransferLineRequest();
            //SITE 站点缺少
            wr.workShop = workShop;// "D30031";//车间
            wr.storageLocation = storageLocation;// "XB05";// 线边仓
            wr.item = item;// "3030040"; //物料
            wr.batch = batch;// "2021092801";//批次
            wr.qty = "3";//数量
            wr.transferType = transferType;// "IN";//记录类型: IN(调入该线边仓）、OUT（调出该线边仓）
            wr.dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// "2021-09-28 18:58:00";//称重时间
            wr.vendor = "";//供应商可为空
            rq.lineRequestList[0] = wr;
            MESSERVICES_SLDB.executeLoadometerWeighingTransfer gb = new MESSERVICES_SLDB.executeLoadometerWeighingTransfer();
            gb.request = rq;

            var credential = db.ClientCredentials.UserName;
            credential.UserName = "ME_USER";
            credential.Password = "Init1234";
            #endregion

            MESSERVICES_SLDB.executeLoadometerWeighingTransferResponse resulte = db.executeLoadometerWeighingTransfer(gb);
            RETURN_SAP ret = new RETURN_SAP();
            if (resulte.@return.code == 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "上传数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "上传数据失败！";
                ret.REDATA = null;
            }

            return ret;
        }


        /// <summary>
        /// 测试机：底浆原材料301小磅上料上传MES
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "测试机：底浆原材料301小磅上料上传MES")]
        public RETURN_SAP MES_DJ301XBSLCS(EOS_SLWEIGHT entity, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            if (string.IsNullOrEmpty(entity.BATCH))
            {
                ret.STATUS = "E";
                ret.REMSG = "批号不能为空！";
                ret.REDATA = null;
            }
            MESSERVICES_XBSLCS.RawMaterialFeedingServiceWSClient db = new MESSERVICES_XBSLCS.RawMaterialFeedingServiceWSClient("RawMaterialFeedingServiceWSPortCS");
            #region 初始化
            MESSERVICES_XBSLCS.rawMaterialFeedingRequest rq = new MESSERVICES_XBSLCS.rawMaterialFeedingRequest();
            rq.site = entity.SITE;// 站点"1000";
            rq.userId = "ME_USER";
            rq.rowRequestList = new MESSERVICES_XBSLCS.rawMaterialFeedingRowRequest[1];
            MESSERVICES_XBSLCS.rawMaterialFeedingRowRequest wr = new MESSERVICES_XBSLCS.rawMaterialFeedingRowRequest();
            wr.storageLocation = entity.STORAGELOCATION;// "XB05";// 线边仓
            wr.item = entity.ITEM.Replace("00000000000", "");// "3030040"; //物料
            wr.batch = entity.BATCH;// "2021092801";//批次
            wr.qty = entity.QTY;//数量
            wr.pulpType = "DJ";// "DJ";//浆原料类型（无人值守底浆原材料调用时，固定传入DJ）物料组：301
            wr.dateTime = entity.DATETIME;// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// "2021-09-28 18:58:00";//称重时间
            wr.itemMark = entity.ITEMMARK;//材料标识，1标识直进车，2标识化验包，3标识垛位调拨
            wr.shift = entity.SHIFT;//班次，ABC(夜白中)
            wr.shiftDate = entity.SHIFTDATE;//班次日期，格式2021-11-29
            rq.rowRequestList[0] = wr;
            var credential = db.ClientCredentials.UserName;
            credential.UserName = "ME_USER";
            credential.Password = "Init1234";
            #endregion

            MESSERVICES_XBSLCS.doRawMaterialFeeding gb = new MESSERVICES_XBSLCS.doRawMaterialFeeding();
            gb.rawMaterialFeedingRequest = rq;
            MESSERVICES_XBSLCS.doRawMaterialFeedingResponse resulte = db.doRawMaterialFeeding(gb);
            if (resulte.@return.code == 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "上传数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = $"上传数据失败！{resulte.@return.message}";
                ret.REDATA = null;
            }

            return ret;
        }

        #endregion

        #region 无人值守调用SAP接口


        /// <summary>
        /// 无人值守采购磅单：化工、包装物一次计量推送接口
        /// </summary>
        /// <param name="materialtype">化工:YG40、包装物:YG60:</param>
        /// <returns>物料类型为空时默认化工YG40推送。</returns>
        [WebMethod(Description = "无人值守采购磅单：化工、包装物一次计量推送接口")]
        public RETURN_SAP WRZS_HGBZWFINISTWEIGHT(string materialtype)
        {
            RETURN_SAP ret = new RETURN_SAP();
            if (string.IsNullOrEmpty(materialtype))
            {
                materialtype = "YG40";//默认化工
                //ret.STATUS = "E";
                //ret.REMSG = "物料分类不能为空！";
                //ret.REDATA = null;
                //return ret;
            }

            var list = DbCS.Queryable<VWB_WEIGHT_FINISHSH>()
                  // .Where(c => c.FINISH == "0")
                  .Where(c => c.UPLOAD == "0")
                .Where(c => c.PK_MARBASCLASS == materialtype)
              .OrderBy(c => c.WEIGHTDATE).ToList();

            EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            #region 初始数据
            string EOS_GM_CODE = "01";
            string DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            string PSTNG_DATE = "";
            string PO_NUMBER = "";
            string PO_ITEM = "";
            string MATERIAL = "";
            string BATCH = "";
            decimal ENTRY_QNT = 0;
            string STGE_LOC = "";
            string PLANT = "";
            string MOVE_TYPE = "103";
            string MVT_IND = "B";
            string JLBILLNO = "";
            double GROSS = 0.0;
            string MATBATCHNO = "";
            #endregion
            //log.Error("一次计量推送:开始");
            //log.Error(string.Format("一次计量推送:明细记录数{0}", list.Count()));
            int successnum = 0;
            foreach (var data in list)
            {
                //log.Error(string.Format("一次计量推送:过磅号{0}", data.BILLNO));
                if (!string.IsNullOrEmpty(data.WEIGHTDATE))
                {
                    PSTNG_DATE = Convert.ToDateTime(data.WEIGHTDATE).ToString("yyyy-MM-dd");
                }
                else
                {
                    PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                }
                #region 
                JLBILLNO = data.BILLNO;
                PO_NUMBER = data.PK_BILL;
                PO_ITEM = data.PK_BILL_B.Replace(data.PK_BILL, "");
                MATERIAL = data.PK_MATERIAL;
                BATCH = data.MATBATCHNO;
                ENTRY_QNT = data.GROSS;
                STGE_LOC = data.PK_STORE;
                PLANT = data.PK_RECEIVEORG;
                GROSS = Convert.ToDouble(data.GROSS);
                MATBATCHNO = data.MATBATCHNO;
                entity.EOS_GM_CODE = EOS_GM_CODE;
                entity.DOC_DATE = DOC_DATE;
                entity.PSTNG_DATE = PSTNG_DATE;
                entity.PO_NUMBER = PO_NUMBER;//订单编号
                entity.PO_ITEM = PO_ITEM;//订单项目编号
                entity.MATERIAL = MATERIAL;//物料
                entity.BATCH = BATCH;//批次
                entity.ENTRY_QNT = ENTRY_QNT;//数量
                entity.STGE_LOC = STGE_LOC;// "Y490";//库存地点
                entity.PLANT = PLANT;//工厂
                entity.MOVE_TYPE = MOVE_TYPE;//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
                entity.MVT_IND = MVT_IND;//移动标识,B,F,B是采购订单  F是生产订单
                entity.BILL_OF_LADING = "";
                entity.GR_GI_SLIP_NO = "";
                entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
                #endregion
                ret = SAP_POST(entity, "");
                if (ret.STATUS == "S")
                {
                    string PZBILLNO = ret.REDATA.MATERIALDOCUMENT;
                    string ZJBILLNO = ret.REDATA.PRUEFLOS;
                    successnum++;
                    //log.Error(string.Format("一次计量推送:预约单明细{0}推送成功！", data.PK_ORDER_B));
                    DbCS.Ado.ExecuteCommand($"update DP_POCARSORDER_DTL set UPLOAD=1,ZJBILLNO='{ZJBILLNO}',PZBILLNO='{PZBILLNO}'  where ID='{data.PK_ORDER_B}'");
                    //调用质检计划
                    MES_PURCHASE1(PLANT, PO_NUMBER, PO_ITEM, ZJBILLNO, MATERIAL, JLBILLNO, MATBATCHNO, GROSS, "", PSTNG_DATE, data.CARS, "计量生成");
                }
                else
                {
                    //log.Error(string.Format("一次计量推送:过磅号{0}推送失败！", data.BILLNO));
                }
            }
            string msg = string.Format("本次推送车数：{0},成功{1}条！", list.Count(), successnum);
            //log.Error(msg);
            //log.Error("一次计量推送:结束");
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }


        /// <summary>
        /// 无人值守采购磅单：化工、包装物一次计量推送接口
        /// </summary>
        /// <param name="materialtype">物料分类</param>
        /// <returns>为空时所有分类</returns>
        [WebMethod(Description = "无人值守采购磅单二次计量推送接口")]
        public RETURN_SAP WRZS_SECONDWEIGHT(string materialtype)
        {

            RETURN_SAP ret = new RETURN_SAP();

            var list = DbCS.Queryable<VWB_WEIGHTBS>()
                 .Where(c => c.FINISH == "1")
                    .Where(c => c.UPLOAD == "0")
              .WhereIF(!string.IsNullOrEmpty(materialtype), c => c.PK_MARBASCLASS == materialtype)
              .OrderBy(c => c.WEIGHTDATE).ToList();
            EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            #region 初始数据
            string EOS_GM_CODE = "01";
            string DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            string PSTNG_DATE = "";
            string PO_NUMBER = "";
            string PO_ITEM = "";
            string MATERIAL = "";
            string BATCH = "";
            decimal ENTRY_QNT = 0;
            string STGE_LOC = "";
            string PLANT = "";
            string MOVE_TYPE = "105";
            string MVT_IND = "B";
            #endregion
            //log.Error("二次计量推送:开始");
            //log.Error(string.Format("二次计量推送:明细记录数{0}", list.Count()));
            int successnum = 0;
            foreach (var data in list)
            {
                //log.Error(string.Format("二次计量推送:过磅号{0}", data.BILLNO));
                if (!string.IsNullOrEmpty(data.WEIGHTDATE))
                {
                    PSTNG_DATE = Convert.ToDateTime(data.WEIGHTDATE).ToString("yyyy-MM-dd");
                }
                else
                {
                    PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                }
                #region 
                PO_NUMBER = data.PK_BILL;
                PO_ITEM = data.PK_BILL_B.Replace(data.PK_BILL, "");
                MATERIAL = data.PK_MATERIAL;
                BATCH = data.BATHNO;
                ENTRY_QNT = data.SUTTLE;
                STGE_LOC = data.PK_STORE;
                PLANT = data.PK_RECEIVEORG;
                entity.EOS_GM_CODE = EOS_GM_CODE;
                entity.DOC_DATE = DOC_DATE;
                entity.PSTNG_DATE = PSTNG_DATE;
                entity.PO_NUMBER = PO_NUMBER;//订单编号
                entity.PO_ITEM = PO_ITEM;//订单项目编号
                entity.MATERIAL = MATERIAL;//物料
                entity.BATCH = BATCH;//批次
                entity.ENTRY_QNT = ENTRY_QNT;//数量
                entity.STGE_LOC = STGE_LOC;// "Y490";//库存地点
                entity.PLANT = PLANT;//工厂
                entity.MOVE_TYPE = MOVE_TYPE;//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
                entity.MVT_IND = MVT_IND;//移动标识,B,F,B是采购订单  F是生产订单
                entity.BILL_OF_LADING = "";
                entity.GR_GI_SLIP_NO = "";
                entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空

                #endregion
                ret = SAP_POST(entity, "");
                if (ret.STATUS == "S")
                {
                    successnum++;
                    //log.Error(string.Format("二次计量推送:过磅号{0}推送成功！", data.BILLNO));
                    DbCS.Ado.ExecuteCommand($"update WB_WEIGHT set UPLOAD=1  where BILLNO='{data.BILLNO}'");
                }
                else
                {
                    //log.Error(string.Format("二次计量推送:过磅号{0}推送失败！", data.BILLNO));
                }
            }
            string msg = string.Format("本次推送车数：{0},成功{1}条！", list.Count(), successnum);
            //log.Error(msg);
            //log.Error("二次计量推送:结束");
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }


        /// <summary>
        /// 测试机：无人值守底浆磅单MES推送接口
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "测试机：无人值守底浆磅单MES推送接口")]
        public RETURN_SAP WRZS_MESDJWEIGHTCS(string ROWNUMBER, string SITE, string BDATE, string EDATE, string PK_STORE, string MATERIAL, string SHIFT, string ITEM_MARK)
        {
            RETURN_SAP ret = new RETURN_SAP();
            if (string.IsNullOrEmpty(BDATE))
            {
                BDATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(EDATE))
            {
                EDATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(SHIFT))
            {
                SHIFT = "A";
            }
            if (string.IsNullOrEmpty(ITEM_MARK))
            {
                ITEM_MARK = "1";
            }

            #region 查询数据 
            var list = Db.Queryable<VWB_WEIGHT_XB_MES>()
              .Where($"SHIFT_DATE>='{BDATE}' and SHIFT_DATE<='{EDATE}'")
              .Where(c => c.SHIFT == SHIFT)
              .Where(c => c.ITEM_MARK == ITEM_MARK)
               .WhereIF(!string.IsNullOrEmpty(SITE), c => c.SITE == SITE)
             .WhereIF(!string.IsNullOrEmpty(PK_STORE), c => c.STORAGELOCATION == PK_STORE)
             .WhereIF(!string.IsNullOrEmpty(MATERIAL), c => c.ITEM == MATERIAL)
              .WhereIF(!string.IsNullOrEmpty(ROWNUMBER), c => c.ROWNUMBER == ROWNUMBER)
              .OrderBy(c => c.SHIFT_DATE).ToList();
            #endregion

            EOS_SLWEIGHT entity = new EOS_SLWEIGHT();
            int successnum = 0;
            string sql = "";
            foreach (var data in list)
            {
                if (data.QTY > 0)
                {
                    #region  初始化
                    entity.SITE = data.SITE;//站点
                    entity.STORAGELOCATION = data.STORAGELOCATION;//线边仓库
                    entity.ITEM = data.ITEM;//物料
                    entity.BATCH = data.BATCH;//批号
                    entity.DATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//时间
                    entity.QTY = $"{data.QTY}";//数量
                    entity.SHIFTDATE = data.SHIFT_DATE;
                    entity.SHIFT = data.SHIFT;
                    entity.ITEMMARK = data.ITEM_MARK;
                    #endregion
                    if (!string.IsNullOrEmpty(data.ITEM))
                    {
                        ret = MES_DJ301XBSLCS(entity, "");
                    }
                }
                else
                {
                    #region 重量为0不上传
                    ret.STATUS = "E";
                    ret.REMSG = "重量为0不能上传!";
                    #endregion
                }

                if (ret.STATUS == "S")
                {
                    successnum++;
                }
                else
                {
                    log.Error(string.Format("MES上料计量推送:过磅号{0}推送失败,原因{1}！", ret.REMSG));
                }

            }
            string msg = string.Format("本次推送车数：{0},成功{1}条！", list.Count(), successnum);
            log.Error(msg);
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }
        #endregion
        #endregion


        #region 开发机
        /// <summary>
        /// 开发机:SAP采购订单下载(项目使用)
        /// </summary>
        /// <param name="strzaedat">创建时间</param>
        /// <param name="strzbudat">修改时间</param>
        /// <param name="strwerks">工厂</param>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "开发机:SAP采购订单下载")]
        public RETURN_SAP SAP_116KF(string strzaedat, string strzbudat, string vbillcode, string strwerks, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            //if (string.IsNullOrEmpty(strzaedat))
            //{
            //    strzaedat = DateTime.Now.ToString("yyyy-MM-dd");
            //}
            //if (string.IsNullOrEmpty(strwerks))
            //{
            //    strwerks = "1000";
            //}
            #region 初始化接口
            SAPSERVICES_116KF.SI_ZFMM116_OUTClient sap = new SAPSERVICES_116KF.SI_ZFMM116_OUTClient("HTTPS_PORT116KF");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            #endregion

            #region 输入参数ZFMM116

            SAPSERVICES_116KF.ZFMM116 mm116 = new SAPSERVICES_116KF.ZFMM116();
            mm116.I_EBELN = vbillcode;//采购凭证
            mm116.I_LIFNR = "";//供应商编号
            mm116.I_MATNR = "";//物料编号
            mm116.I_PROCSTAT = "";//批准状态
            mm116.I_WERKS = "";//工厂
            #region 工厂 WERKS
            //if (!string.IsNullOrEmpty(strwerks))
            //{
            //    mm116.I_WERKS = strwerks;
            //}
            //else { }
            #endregion
            mm116.I_ZAEDAT = strzaedat;//创建时间
            mm116.I_ZBUDAT = strzbudat;//修改时间
            mm116.T_OUTTAB = new SAPSERVICES_116KF.ZSMM116[1];
            #region 明细 ZSMM116
            SAPSERVICES_116KF.ZSMM116 zsmm116 = new SAPSERVICES_116KF.ZSMM116();
            zsmm116.BPRME = "";
            zsmm116.BSART = "";
            zsmm116.EBELN = "";
            zsmm116.EBELP = "";
            zsmm116.EKGRP = "";
            zsmm116.EKORG = "";
            zsmm116.LIFNR = "";
            zsmm116.MATNR = "";
            zsmm116.MEINS = "";
            zsmm116.MENGE = 0;
            zsmm116.NAME1 = "";
            zsmm116.MENGESpecified = false;
            zsmm116.NETPR = 0;
            zsmm116.NETPRSpecified = false;
            zsmm116.PEINH = 0;
            zsmm116.PEINHSpecified = false;
            zsmm116.PROCSTAT = "";
            zsmm116.TXZ01 = "";
            zsmm116.WERKS = "";
            zsmm116.ZAEDAT = "";
            zsmm116.ZBUDAT = "";

            #endregion
            mm116.T_OUTTAB[0] = zsmm116;
            #endregion

            //调用接口
            var resulte = sap.SI_ZFMM116_OUT(mm116);
            int count = 0;
            foreach (var model in resulte.T_OUTTAB)
            {
                #region 初始化值
                string VBILLCODE = model.EBELN;//采购凭证
                string PK_ORDER = model.EBELN;//采购订单主键
                string BILLTYPE = model.BSART;//采购凭证类别
                string DBILLDATE = string.Format("{0} 00:00:00", model.ZBUDAT);//采购凭证日期
                string FORDERSTATUS = model.PROCSTAT;//凭证状态
                string PK_SUPPLIER = model.LIFNR;//供应商编号
                string SUPPLYNAME = model.NAME1;//供应商名称
                string PK_ORG = model.EKORG;//采购公司
                string PK_DEPT = model.EKGRP;//采购分组
                string MODIFIEDTIME = string.Format("{0} 00:00:00", model.ZAEDAT);//更改日期
                string DMAKEDATE = string.Format("{0} 00:00:00", model.ZBUDAT);//创建日期
                string CREATIONTIME = string.Format("{0} 00:00:00", model.ZBUDAT);//创建日期

                string CROWNO = model.EBELP;//采购凭证行项目
                string PK_ORDER_B = string.Format("{0}{1}", PK_ORDER, CROWNO);
                string PK_MATERIAL = model.MATNR;//物料编号
                string MATERIAL = model.TXZ01;//物料描述
                string PK_MARBASCLASS = model.MTART;//物料类型
                string PK_MATERIALGROUP = model.MATKL;//物料组
                string PURUNITNAME = model.MEINS;//订单单位
                decimal NASTNUM = model.MENGE;//采购订单数量
                string SECPURUNITNAME = model.MEINS;//订单单位
                decimal SECQUANTITY = model.MENGE;//采购订单数量
                decimal NQTUNITNUM = model.PEINH; //价格单位数量
                string CQTUNITID = model.BPRME;//订单价格单位
                decimal NPRICE = model.NETPR;//净价
                string RECCOMPANYID = model.WERKS;//工厂
                string BARRIVECLOSE = model.LOEKZ;//L删除 空未删除 
                string VBDEF4 = model.UEBTK;
                string ELIKZ = model.ELIKZ;

                #endregion

                if (!string.IsNullOrEmpty(PK_MATERIAL))
                {
                    if (string.IsNullOrEmpty(ELIKZ))
                    {
                        #region 主表

                        var order = new NC_PO_ORDER();
                        order.VBILLCODE = VBILLCODE;//订单编号(采购凭证)
                        order.PK_ORDER = PK_ORDER;//订单主键
                        order.BILLTYPE = BILLTYPE;
                        order.DBILLDATE = DBILLDATE;//订单日期;
                        order.PK_SUPPLIER = PK_SUPPLIER;//供应商
                        order.PK_ORG = PK_ORG;//组织组织
                        order.PK_DEPT = PK_DEPT;//组织部门
                                                //FORDERSTATUS
                        if (FORDERSTATUS == "02")
                        {
                            order.FORDERSTATUS = 0;
                        }
                        else
                        {
                            order.FORDERSTATUS = 1;
                        }
                        order.CARSNUM = 0;
                        order.CREATIONTIME = CREATIONTIME;//创建日期;
                        order.DMAKEDATE = DMAKEDATE;//修改日期;
                        order.MODIFIEDTIME = MODIFIEDTIME;//修改日期;
                        order.DATATYPE = "1";
                        count = DbCS.Queryable<NC_PO_ORDER>().Where(r => r.PK_ORDER == order.PK_ORDER).Count();
                        if (count > 0)
                        {
                            DbCS.Updateable(order).ExecuteCommand();
                        }
                        else
                        {
                            DbCS.Insertable(order).ExecuteCommand();
                        }
                        #endregion

                        #region 明细表
                        var orderitem = new NC_PO_ORDER_B();
                        orderitem.DBILLDATE = DBILLDATE;
                        orderitem.PK_ORDER = PK_ORDER;
                        orderitem.PK_ORDER_B = PK_ORDER_B;
                        orderitem.CROWNO = CROWNO;//采购订单行号
                        orderitem.PK_SUPPLIER = PK_SUPPLIER;//供应商
                        orderitem.PK_MATERIAL = PK_MATERIAL;//物料
                        orderitem.PURUNITNAME = PURUNITNAME;
                        orderitem.NASTNUM = NASTNUM;//采购订单数量
                        orderitem.SECPURUNITNAME = SECPURUNITNAME;
                        orderitem.SECQUANTITY = NASTNUM;//采购订单数量
                        orderitem.NQTUNITNUM = NQTUNITNUM;//价格单位数量
                        orderitem.CQTUNITID = CQTUNITID;//订单价格单位
                        orderitem.NPRICE = NPRICE;//净价
                        orderitem.RECCOMPANYID = RECCOMPANYID;//工厂
                        orderitem.BARRIVECLOSE = "0";
                        if (BARRIVECLOSE == "L")
                        {
                            orderitem.BARRIVECLOSE = "1";
                        }

                        count = DbCS.Queryable<NC_PO_ORDER_B>().Where(r => r.PK_ORDER_B == PK_ORDER_B).Count();
                        if (count > 0)
                        {
                            DbCS.Updateable(orderitem).ExecuteCommand();
                        }
                        else
                        {
                            DbCS.Insertable(orderitem).ExecuteCommand();
                        }
                        #endregion

                        #region 基础档案

                        if (!string.IsNullOrEmpty(PK_SUPPLIER))
                        {
                            #region 供应商
                            count = DbCS.Queryable<BD_SUPPLY>().Where(c => c.ID == PK_SUPPLIER).Count();
                            if (count == 0)
                            {
                                BD_SUPPLY sup_obj = new BD_SUPPLY()
                                {
                                    ID = PK_SUPPLIER,
                                    CODE = PK_SUPPLIER,
                                    NAME = SUPPLYNAME,
                                    SHORTNAME = SUPPLYNAME,
                                    SPELLCODE = SUPPLYNAME.ToPinYin(),
                                    ISUSE = "1",
                                    DATATYPE = "1"
                                };
                                DbCS.Insertable(sup_obj).ExecuteCommand();
                            }
                            #endregion

                            count = DbCS.Queryable<Base_UserKS>().Where(r => r.USERID == PK_SUPPLIER).Count();
                            if (count == 0)
                            {
                                #region 供应商用户
                                Base_UserKS user = new Base_UserKS();
                                user.USERID = model.LIFNR;
                                user.CODE = model.LIFNR;
                                user.PASSWORD = "0c164caaa094a707";
                                user.REALNAME = model.NAME1;
                                user.ACCOUNT = model.LIFNR;
                                user.MOBILE = "";
                                user.ENABLED = 1;
                                user.TYPE = 2;
                                user.STYPE = 0;
                                user.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                user.CREATEUSER = "同步注册";
                                DbCS.Insertable(user).ExecuteCommand();
                                #endregion

                                #region 供应商用户权限表
                                Base_DataScopePermission userPerm = new Base_DataScopePermission();
                                userPerm.DataScopePermissionId = CommonHelper.GetGuid;
                                userPerm.ModuleId = "999eae8b-1d09-4678-9b5f-2901205f1d98";
                                userPerm.ObjectId = model.LIFNR;
                                userPerm.ResourceId = model.LIFNR;
                                userPerm.SortCode = 0;
                                userPerm.Category = "6";
                                userPerm.CreateDate = DateTime.Now;
                                userPerm.CreateUserId = "";
                                userPerm.CreateUserName = "同步注册";
                                userPerm.ScopeType = "1";
                                DbCS.Insertable(userPerm).ExecuteCommand();
                                #endregion
                            }
                        }

                        #region 物料
                        if (!string.IsNullOrEmpty(PK_MATERIAL))
                        {
                            count = DbCS.Queryable<BD_MATERIAL>().Where(c => c.ID == PK_MATERIAL).Count();
                            if (count == 0)
                            {
                                BD_MATERIAL mat_obj = new BD_MATERIAL()
                                {
                                    ID = PK_MATERIAL,
                                    CODE = PK_MATERIAL,
                                    NAME = MATERIAL,
                                    SHORTNAME = MATERIAL,
                                    SPELLCODE = MATERIAL.ToPinYin(),
                                    PK_MARBASCLASS = PK_MARBASCLASS,//物料类型
                                    PK_GROUP = PK_MATERIALGROUP,//物料组
                                    MATERIALSPEC = "",
                                    DEF1 = model.MEINS,
                                    DEF2 = model.MEINS,
                                    ISUSE = "1",
                                    ISJL = 1,
                                    DATATYPE = "1"
                                };
                                DbCS.Insertable(mat_obj).ExecuteCommand();
                            }
                        }
                        #endregion

                        #endregion
                    }
                }
            }

            if (resulte.T_OUTTAB.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            return ret;
        }


        #endregion


        #region 生产机

        #region 基础档案

        /// <summary>
        /// 生产机：SAP物料下载
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：生产机SAP物料下载")]
        public RETURN_SAP SAP_94ZS(string strjson)
        {
            SAPSERVICES_94ZS.SI_ZFMM094Client sap = new SAPSERVICES_94ZS.SI_ZFMM094Client("HTTPS_PORT94ZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_94ZS.ZFMM094 mm94 = new SAPSERVICES_94ZS.ZFMM094();

            var resulte = sap.SI_ZFMM094(mm94);
            foreach (var model in resulte.T_RETURN)
            {
                var count = Db.Queryable<BD_MATERIAL>().Where(c => c.ID == model.MATNR).Count();
                if (count == 0)
                {
                    BD_MATERIAL obj = new BD_MATERIAL()
                    {
                        ID = model.MATNR,
                        CODE = model.MATNR,
                        NAME = model.MAKTX,
                        SHORTNAME = model.MAKTX,
                        SPELLCODE = model.MAKTX.ToPinYin(),
                        PK_MARBASCLASS = model.MTART,//物料类型
                        PK_GROUP = model.MATKL,//物料组
                        MATERIALSPEC = "",
                        DEF1 = model.MEINS,
                        DEF2 = model.MEINS,
                        ISUSE = "1",
                        DATATYPE = "1"
                    };
                    Db.Insertable(obj).ExecuteCommand();
                }
            }
            RETURN_SAP ret = new RETURN_SAP();

            if (resulte.T_RETURN.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            return ret;
        }

        /// <summary>
        /// 生产机：SAP117物料下载
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：生产机SAP117物料下载")]
        public RETURN_SAP SAP_117ZS(EOS_ZFMM117 entity, string strjson)
        {
            SAPSERVICES_117ZS.SI_ZFMM117_OUTClient sap = new SAPSERVICES_117ZS.SI_ZFMM117_OUTClient("HTTPS_PORT117ZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_117ZS.ZFMM117 mm117 = new SAPSERVICES_117ZS.ZFMM117();
            if (!string.IsNullOrEmpty(entity.I_MTART))
            {
                mm117.I_MTART = entity.I_MTART;//物料类型YG30
            }
            if (!string.IsNullOrEmpty(entity.I_MATKL))
            {
                mm117.I_MATKL = entity.I_MATKL;//物料组301
            }

            if (!string.IsNullOrEmpty(entity.I_EXTWG))
            {
                mm117.I_EXTWG = entity.I_EXTWG;//外部物料组311
            }

            if (!string.IsNullOrEmpty(entity.I_MATNR))
            {
                mm117.I_MATNR = entity.I_MATNR;//编号
            }
            if (!string.IsNullOrEmpty(entity.I_MAKTX))
            {
                mm117.I_MAKTX = entity.I_MAKTX;//物料名称
            }
            if (!string.IsNullOrEmpty(entity.I_WERKS))
            {
                mm117.I_WERKS = entity.I_WERKS;//工厂
            }
            if (!string.IsNullOrEmpty(entity.I_EKGRP))
            {
                mm117.I_EKGRP = entity.I_EKGRP;//采购组
            }

            #region T_OUTTAB
            mm117.T_OUTTAB = new SAPSERVICES_117ZS.ZSMM117[1];
            SAPSERVICES_117ZS.ZSMM117 zsmm117 = new SAPSERVICES_117ZS.ZSMM117();
            zsmm117.EKGRP = "";
            zsmm117.EXTWG = "";
            zsmm117.MAKTX = "";
            zsmm117.MATKL = "";
            zsmm117.MATNR = "";
            zsmm117.MTART = "";
            zsmm117.WERKS = "";
            mm117.T_OUTTAB[0] = zsmm117;
            #endregion
            var resulte = sap.SI_ZFMM117_OUT(mm117);
            foreach (var model in resulte.T_OUTTAB)
            {
                #region 初始化
                BD_MATERIAL obj = new BD_MATERIAL()
                {
                    ID = model.MATNR,
                    CODE = model.MATNR,
                    NAME = model.MAKTX,
                    SHORTNAME = model.MAKTX,
                    SPELLCODE = model.MAKTX.ToPinYin(),
                    PK_MARBASCLASS = model.MATKL,//物料组
                    PK_GROUP = model.MTART,//物料类型
                    MATERIALBARCODE = model.EXTWG,//外部物料组
                    MATERIALSPEC = "",
                    DEF1 = "KG",
                    DEF2 = "KG",
                    //DEF1 = model.MEINS,
                    //DEF2 = model.MEINS,
                    ISJL = 1,
                    ISUSE = "1",
                    DATATYPE = "1",
                    CRETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                    CREUSER = "接口同步"
                };
                #endregion
                var count = Db.Queryable<BD_MATERIAL>().Where(c => c.ID == model.MATNR).Count();
                if (count == 0)
                {
                    Db.Insertable(obj).ExecuteCommand();
                }
                else
                {
                    if (entity.OPENTYPE == "0")
                    {
                        Db.Updateable(obj).ExecuteCommand();
                    }
                }
            }
            RETURN_SAP ret = new RETURN_SAP();

            if (resulte.T_OUTTAB.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            return ret;
        }


        /// <summary>
        /// Ajax生产机：SAP117物料下载
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "Ajax生产机：生产机SAP117物料下载")]
        public RETURN_SAP SAP_117ZS1(string id, string name, string matgroup, string matclass, string wbmatgrop, string werks, string opentype, string strjson)
        {
            EOS_ZFMM117 entity = new EOS_ZFMM117();
            if (!string.IsNullOrEmpty(id))
            {
                entity.I_MATNR = id;//编号
            }
            if (!string.IsNullOrEmpty(name))
            {
                entity.I_MAKTX = name;//物料名称
            }

            if (!string.IsNullOrEmpty(werks))
            {
                entity.I_WERKS = werks;//工厂
            }
            if (!string.IsNullOrEmpty(matgroup))
            {
                entity.I_MATKL = matgroup;//物料组
            }
            if (!string.IsNullOrEmpty(matclass))
            {
                entity.I_MTART = matclass;//物料类型
            }
            if (!string.IsNullOrEmpty(wbmatgrop))
            {
                entity.I_EXTWG = wbmatgrop;//外部物料组
            }
            entity.I_EKGRP = "";//采购组

            if (!string.IsNullOrEmpty(opentype))
            {
                entity.OPENTYPE = opentype;
            }
            else
            {
                entity.OPENTYPE = "0";
            }

            return SAP_117ZS(entity, "");
        }

        /// <summary>
        /// 生产机：SAP供应商下载
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：SAP供应商下载")]
        public RETURN_SAP SAP_95ZS(string id, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            SAPSERVICES_95ZS.SI_ZFMM095Client sap = new SAPSERVICES_95ZS.SI_ZFMM095Client("HTTPS_PORT95ZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_95ZS.ZFMM095 mm95 = new SAPSERVICES_95ZS.ZFMM095();

            var resulte = sap.SI_ZFMM095(mm95);
            string LIFNR = "";
            foreach (var model in resulte.T_RETURN)
            {
                if (!string.IsNullOrEmpty(id))
                {
                    LIFNR = model.LIFNR.Replace("0000", "");
                    if (LIFNR != id)
                    {
                        continue;
                    }
                }

                var count = Db.Queryable<BD_SUPPLY>().Where(c => c.ID == model.LIFNR).Count();
                if (count == 0)
                {
                    BD_SUPPLY obj = new BD_SUPPLY()
                    {
                        ID = model.LIFNR,
                        CODE = model.LIFNR,
                        NAME = model.NAME1,
                        SHORTNAME = model.NAME1,
                        SPELLCODE = model.NAME1.ToPinYin(),
                        ISUSE = "1",
                        DATATYPE = "1"
                    };
                    Db.Insertable(obj).ExecuteCommand();
                    string acccount = string.Format($"{model.LIFNR.ToInt64()}");
                    count = Db.Queryable<Base_UserKS>().Where(r => r.ACCOUNT == acccount).Count();
                    if (count == 0)
                    {
                        #region 供应商用户
                        Base_UserKS user = new Base_UserKS();
                        user.USERID = model.LIFNR;
                        user.CODE = model.LIFNR;
                        user.PASSWORD = "0c164caaa094a707";
                        user.REALNAME = model.NAME1;
                        user.ACCOUNT = acccount;
                        user.MOBILE = "";
                        user.ENABLED = 1;
                        user.TYPE = 2;
                        user.STYPE = 0;
                        user.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        user.CREATEUSER = "同步注册";
                        Db.Insertable(user).ExecuteCommand();

                        Base_DataScopePermission userPerm = new Base_DataScopePermission();
                        userPerm.DataScopePermissionId = CommonHelper.GetGuid;
                        userPerm.ModuleId = "999eae8b-1d09-4678-9b5f-2901205f1d98";
                        userPerm.ObjectId = model.LIFNR;
                        userPerm.ResourceId = model.LIFNR;
                        userPerm.SortCode = 0;
                        userPerm.Category = "6";
                        userPerm.CreateDate = DateTime.Now;
                        userPerm.CreateUserId = "";
                        userPerm.CreateUserName = "同步注册";
                        userPerm.ScopeType = "1";
                        Db.Insertable(userPerm).ExecuteCommand();
                        #endregion
                    }
                    else
                    {

                    }
                }
            }

            if (resulte.T_RETURN.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }

            return ret;
        }

        #endregion

        #region 生产机：采购订单业务 

        /// <summary>
        /// 生产机：SAP标准采购订单下载(调试通过不使用)
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：SAP标准采购订单下载")]
        public RETURN_SAP SAP_92ZS(string strjson)
        {
            SAPSERVICES_92ZS.SI_ZFMM092_OUTClient sap = new SAPSERVICES_92ZS.SI_ZFMM092_OUTClient("HTTPS_PORT92ZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];

            #region ZFMM092_ITEM
            SAPSERVICES_92ZS.ZFMM092_ITEM[] arr = new SAPSERVICES_92ZS.ZFMM092_ITEM[1];
            SAPSERVICES_92ZS.ZFMM092_ITEM zfmm092 = new SAPSERVICES_92ZS.ZFMM092_ITEM();
            zfmm092.AEDAT = "";
            zfmm092.EBELN = "";
            zfmm092.EBELP = "";
            zfmm092.LIFNR = "";
            zfmm092.MATNR = "";
            zfmm092.MENGE = 0;
            zfmm092.MENGESpecified = false;
            zfmm092.WERKS = "";
            arr[0] = zfmm092;
            #endregion

            #region ZFMM092
            SAPSERVICES_92ZS.ZFMM092 mm92 = new SAPSERVICES_92ZS.ZFMM092();
            mm92.I_EBELN = "";
            mm92.I_LIFNR = "";
            mm92.I_MATNR = "";
            mm92.T_ITEM = arr;
            #endregion
            var resulte = sap.SI_ZFMM092_OUT(mm92);
            int count = 0;
            int res = 0;

            #region 初始化值
            #endregion
            #region 初始化值
            string VBILLCODE = "";//采购凭证
            string PK_ORDER = "";
            string BILLTYPE = "";//采购凭证类别
            string MODIFIEDTIME = "";//更改日期
            string DBILLDATE = "";//采购凭证日期
            string DMAKEDATE = "";//创建日期
            string CREATIONTIME = "";//创建日期
            string FORDERSTATUS = "";//凭证状态
            string PK_MATERIAL = "";//物料编号
            string MATERIAL = "";//物料描述
            string PK_SUPPLIER = "";//供应商编号
            string SUPPLYNAME = "";//供应商名称
            string CROWNO = "";//采购凭证行项目
            string PK_ORDER_B = "";//明细PK 
            string PK_ORG = "";//采购组织
            string PK_DEPT = "";//采购组
            string RECCOMPANYID = "";//工厂
            string PURUNITNAME = "";//订单单位
            decimal NASTNUM = 0;//采购订单数量
            decimal SECQUANTITY = 0;//采购订单数量
            string SECPURUNITNAME = "";//订单单位
            decimal NQTUNITNUM = 0; //价格单位数量
            string CQTUNITID = "";//订单价格单位
            decimal NPRICE = 0;//净价
            #endregion

            foreach (var model in resulte.T_ITEM)
            {
                #region 初始化值
                VBILLCODE = model.EBELN;//采购凭证
                PK_ORDER = model.EBELN;
                BILLTYPE = "";//采购凭证类别
                MODIFIEDTIME = string.Format("{0} 00:00:00", model.AEDAT);//更改日期
                DBILLDATE = string.Format("{0} 00:00:00", model.AEDAT);//采购凭证日期
                DMAKEDATE = string.Format("{0} 00:00:00", model.AEDAT);//创建日期
                CREATIONTIME = string.Format("{0} 00:00:00", model.AEDAT);//创建日期
                FORDERSTATUS = "";//凭证状态
                PK_MATERIAL = model.MATNR;//物料编号
                MATERIAL = "";//物料描述
                PK_SUPPLIER = model.LIFNR;//供应商编号
                SUPPLYNAME = "";//供应商名称
                CROWNO = model.EBELP;//采购凭证行项目
                PK_ORDER_B = string.Format("{0}{1}", PK_ORDER, CROWNO);//明细PK 
                PK_ORG = "";//采购组织
                PK_DEPT = "";//采购组
                RECCOMPANYID = model.WERKS;//工厂
                PURUNITNAME = "";//订单单位
                NASTNUM = model.MENGE;//采购订单数量
                SECQUANTITY = model.MENGE;//采购订单数量
                SECPURUNITNAME = "";//订单单位
                NQTUNITNUM = 0; //价格单位数量
                CQTUNITID = "";//订单价格单位
                NPRICE = 0;//净价
                #endregion

                #region 主表
                var order = new NC_PO_ORDER();
                order.VBILLCODE = VBILLCODE;//订单编号(采购凭证)
                order.PK_ORDER = PK_ORDER;//订单主键
                order.BILLTYPE = BILLTYPE;
                order.DBILLDATE = DBILLDATE;//订单日期;
                order.PK_SUPPLIER = PK_SUPPLIER;//供应商
                order.PK_ORG = PK_ORG;//组织机构
                order.PK_DEPT = PK_DEPT;//组织机构

                if (FORDERSTATUS == "1")
                {
                    order.FORDERSTATUS = 1;
                }
                else
                {
                    order.FORDERSTATUS = 0;
                }
                order.CARSNUM = 0;
                order.CREATIONTIME = CREATIONTIME;//创建日期;
                order.DMAKEDATE = DMAKEDATE;//修改日期;
                order.MODIFIEDTIME = MODIFIEDTIME;//修改日期;
                order.DATATYPE = "1";
                count = Db.Queryable<NC_PO_ORDER>().Where(r => r.PK_ORDER == order.PK_ORDER).Count();
                if (count > 0)
                {
                    Db.Updateable(order).ExecuteCommand();
                }
                else
                {
                    Db.Insertable(order).ExecuteCommand();
                }
                #endregion

                #region 明细表
                var orderitem = new NC_PO_ORDER_B();
                orderitem.DBILLDATE = DBILLDATE;
                orderitem.PK_ORDER = PK_ORDER;
                orderitem.PK_ORDER_B = PK_ORDER_B;
                orderitem.CROWNO = CROWNO;//采购订单行号
                orderitem.PK_SUPPLIER = PK_SUPPLIER;//供应商
                orderitem.PK_MATERIAL = PK_MATERIAL;//物料
                orderitem.PURUNITNAME = PURUNITNAME;
                orderitem.NASTNUM = NASTNUM;//采购订单数量
                orderitem.SECPURUNITNAME = SECPURUNITNAME;
                orderitem.SECQUANTITY = SECQUANTITY;//采购订单数量
                orderitem.NQTUNITNUM = NQTUNITNUM;//价格单位数量
                orderitem.CQTUNITID = CQTUNITID;//订单价格单位
                orderitem.NPRICE = NPRICE;//净价
                orderitem.RECCOMPANYID = model.WERKS;//工厂
                count = Db.Queryable<NC_PO_ORDER>().Where(r => r.PK_ORDER == order.PK_ORDER).Count();
                if (count > 0)
                {
                    res += Db.Updateable(orderitem).ExecuteCommand();
                }
                else
                {
                    res += Db.Insertable(orderitem).ExecuteCommand();
                }
                #endregion
            }
            RETURN_SAP ret = new RETURN_SAP();
            if (resulte.T_ITEM.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            return ret;
        }

        /// <summary>
        /// 生产机：SAP标准采购订单关闭(调试通过不使用)
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：SAP标准采购订单关闭")]
        public RETURN_SAP SAP_96ZS(string strjson)
        {

            SAPSERVICES_96ZS.SI_ZFMM096_OUTClient mat = new SAPSERVICES_96ZS.SI_ZFMM096_OUTClient("HTTPS_PORT96ZS");
            mat.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            mat.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            #region ZTFMM096

            SAPSERVICES_96ZS.ZFMM096 mm96 = new SAPSERVICES_96ZS.ZFMM096();
            mm96.T_OUTTAB = new SAPSERVICES_96ZS.ZSFMM096[1];

            #region ZSFMM096
            SAPSERVICES_96ZS.ZSFMM096 zsfmm096 = new SAPSERVICES_96ZS.ZSFMM096();
            zsfmm096.PHAS0 = "";
            zsfmm096.PHAS2 = "";
            zsfmm096.PHAS3 = "";
            zsfmm096.AUFNR = "";
            zsfmm096.WERKS = "9000";
            mm96.T_OUTTAB[0] = zsfmm096;
            #endregion

            mm96.T_RETURN = new SAPSERVICES_96ZS.ZTFMM096[1];
            SAPSERVICES_96ZS.ZTFMM096 ztfmm096 = new SAPSERVICES_96ZS.ZTFMM096();
            mm96.T_RETURN[0] = ztfmm096;
            #endregion


            var resulte = mat.SI_ZFMM096_OUT(mm96);
            foreach (var model in resulte.T_OUTTAB)
            {

            }

            RETURN_SAP ret = new RETURN_SAP();
            if (resulte.T_OUTTAB.Length > 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            return ret;
        }

        /// <summary>
        /// 生产机：SAP采购订单下载(项目使用)
        /// </summary>
        /// <param name="strzaedat">创建时间</param>
        /// <param name="strzbudat">修改时间</param>
        /// <param name="strwerks">工厂</param>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：SAP采购订单下载")]
        public RETURN_SAP SAP_116ZS(string strzaedat, string strzbudat, string vbillcode, string strwerks, string supply, string type, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            //if (string.IsNullOrEmpty(strzaedat))
            //{
            //    strzaedat = DateTime.Now.ToString("yyyy-MM-dd");
            //}
            //if (string.IsNullOrEmpty(strwerks))
            //{
            //    strwerks = "1000";
            //}
            #region 初始化接口
            SAPSERVICES_116ZS.SI_ZFMM116_OUTClient sap = new SAPSERVICES_116ZS.SI_ZFMM116_OUTClient("HTTPS_PORT116ZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            #endregion

            #region 输入参数ZFMM116


            SAPSERVICES_116ZS.ZFMM116 mm116 = new SAPSERVICES_116ZS.ZFMM116();
            mm116.I_EBELN = vbillcode;//采购凭证
            mm116.I_LIFNR = "";//供应商编号
            if (!string.IsNullOrEmpty(supply))
            {
                mm116.I_LIFNR = supply;
            }
            mm116.I_MATNR = "";//物料编号
            mm116.I_PROCSTAT = "";//批准状态
            mm116.I_WERKS = "";//工厂
            #region 工厂 WERKS
            if (!string.IsNullOrEmpty(strwerks))
            {
                mm116.I_WERKS = strwerks;
            }
            #endregion


            mm116.I_ZAEDAT = strzaedat;//创建时间
            mm116.I_ZBUDAT = strzbudat;//修改时间
            mm116.T_OUTTAB = new SAPSERVICES_116ZS.ZSMM116[1];
            #region 明细 ZSMM116
            SAPSERVICES_116ZS.ZSMM116 zsmm116 = new SAPSERVICES_116ZS.ZSMM116();
            zsmm116.BPRME = "";
            //采购凭证类型
            zsmm116.BSART = "";
            if (!string.IsNullOrEmpty(type))
            {
                zsmm116.BSART = type;
            }
            zsmm116.EBELN = "";
            zsmm116.EBELP = "";
            zsmm116.EKGRP = "";
            zsmm116.EKORG = "";
            zsmm116.LIFNR = "";
            zsmm116.MATNR = "";
            zsmm116.MEINS = "";
            zsmm116.MENGE = 0;
            zsmm116.NAME1 = "";
            zsmm116.MENGESpecified = false;
            zsmm116.NETPR = 0;
            zsmm116.NETPRSpecified = false;
            zsmm116.PEINH = 0;
            zsmm116.PEINHSpecified = false;
            zsmm116.PROCSTAT = "";
            zsmm116.TXZ01 = "";
            zsmm116.WERKS = "";
            zsmm116.ZAEDAT = "";
            zsmm116.ZBUDAT = "";

            #endregion
            mm116.T_OUTTAB[0] = zsmm116;
            #endregion

            //调用接口

            log.Error($"采购订单接收:{strwerks}开始");
            var resulte = sap.SI_ZFMM116_OUT(mm116);
            int count = 0;
            int m_count = 0;
            //log.Error(string.Format("采购订单接收:明细记录数{0}", resulte.T_OUTTAB.Count()));

            #region 初始化值
            string VBILLCODE = "";//采购凭证
            string PK_ORDER = "";//采购订单主键
            string BILLTYPE = "";//采购凭证类别
            string DBILLDATE = "";//采购凭证日期
            string FORDERSTATUS = "";//凭证状态
            string PK_SUPPLIER = "";//供应商编号
            string SUPPLYNAME = "";//供应商名称
            string PK_ORG = "";//采购公司
            string PK_DEPT = "";//采购分组
            string MODIFIEDTIME = "";//更改日期
            string DMAKEDATE = "";//创建日期
            string CREATIONTIME = "";//创建日期

            string CROWNO = "";//采购凭证行项目
            string PK_ORDER_B = string.Format("{0}{1}", PK_ORDER, CROWNO);
            string PK_MATERIAL = "";//物料编号
            string MATERIAL = "";//物料描述
            string PK_MARBASCLASS = "";//物料分组
            string PK_MATERIALGROUP = "";//物料类型
            string MATERIALBARCODE = "";// model.EXTWG;//外部物料组

            string PURUNITNAME = "";//订单单位
            decimal NASTNUM = 0;//采购订单数量
            string SECPURUNITNAME = "";//订单单位
            decimal SECQUANTITY = 0;//采购订单数量
            decimal NQTUNITNUM = 0; //价格单位数量
            string CQTUNITID = "";//订单价格单位
            decimal NPRICE = 0;//净价
            string RECCOMPANYID = "";//工厂
            string BARRIVECLOSE = "";//L删除 空未删除 
            string VBDEF4 = "";//是否过量收货 X是 空否
            string ELIKZ = "";//交货已完成 X是 空否
            #endregion
            foreach (var model in resulte.T_OUTTAB)
            {
                #region 初始化值
                VBILLCODE = model.EBELN;//采购凭证
                PK_ORDER = model.EBELN;//采购订单主键
                BILLTYPE = model.BSART;//采购凭证类别
                DBILLDATE = string.Format("{0} 00:00:00", model.ZBUDAT);//采购凭证日期
                FORDERSTATUS = model.PROCSTAT;//凭证状态
                PK_SUPPLIER = model.LIFNR;//供应商编号
                SUPPLYNAME = model.NAME1;//供应商名称
                PK_ORG = model.EKORG;//采购公司
                PK_DEPT = model.EKGRP;//采购分组
                MODIFIEDTIME = string.Format("{0} 00:00:00", model.ZAEDAT);//更改日期
                DMAKEDATE = string.Format("{0} 00:00:00", model.ZBUDAT);//创建日期
                CREATIONTIME = string.Format("{0} 00:00:00", model.ZBUDAT);//创建日期

                CROWNO = model.EBELP;//采购凭证行项目
                PK_ORDER_B = string.Format("{0}{1}", PK_ORDER, CROWNO);
                PK_MATERIAL = model.MATNR;//物料编号
                MATERIAL = model.TXZ01;//物料描述
                PK_MARBASCLASS = model.MATKL;//物料分组
                PK_MATERIALGROUP = model.MTART;//物料类型
                MATERIALBARCODE = "";// model.EXTWG;//外部物料组

                PURUNITNAME = model.MEINS;//订单单位
                NASTNUM = model.MENGE;//采购订单数量
                SECPURUNITNAME = model.MEINS;//订单单位
                SECQUANTITY = model.MENGE;//采购订单数量
                NQTUNITNUM = model.PEINH; //价格单位数量
                CQTUNITID = model.BPRME;//订单价格单位
                NPRICE = model.NETPR;//净价
                RECCOMPANYID = model.WERKS;//工厂
                BARRIVECLOSE = model.LOEKZ;//L删除 空未删除 
                VBDEF4 = model.UEBTK;//是否过量收货 X是 空否
                ELIKZ = model.ELIKZ;//交货已完成 X是 空否

                #endregion
                // //log.Error("采购订单接收:开始");
                //if (ELIKZ == "X")
                //{
                //    FORDERSTATUS = "00";
                //}

                if (!string.IsNullOrEmpty(PK_MATERIAL))
                {
                    //未收货
                    //if (string.IsNullOrEmpty(ELIKZ))
                    //{
                    // log.Error($"采购订单接收:订单编号{VBILLCODE}、订单行号{CROWNO}");
                    #region 主表
                    var order = new NC_PO_ORDER();
                    order.VBILLCODE = VBILLCODE;//订单编号(采购凭证)
                    order.PK_ORDER = PK_ORDER;//订单主键
                    order.BILLTYPE = BILLTYPE;
                    order.DBILLDATE = DBILLDATE;//订单日期;
                    order.PK_SUPPLIER = PK_SUPPLIER;//供应商
                    order.PK_ORG = PK_ORG;//组织组织
                    order.PK_DEPT = PK_DEPT;//组织部门

                    //FORDERSTATUS
                    //if (FORDERSTATUS == "02" || FORDERSTATUS == "05")
                    //{
                    order.FORDERSTATUS = 0;
                    //}
                    //else
                    //{
                    //    order.FORDERSTATUS = 1;
                    //}

                    order.CARSNUM = 0;
                    order.CREATIONTIME = CREATIONTIME;//创建日期;
                    order.DMAKEDATE = DMAKEDATE;//修改日期;
                    order.MODIFIEDTIME = MODIFIEDTIME;//修改日期;
                    order.DATATYPE = "1";
                    count = Db.Queryable<NC_PO_ORDER>().Where(r => r.PK_ORDER == order.PK_ORDER).Count();
                    if (count > 0)
                    {
                        Db.Updateable(order).ExecuteCommand();
                    }
                    else
                    {
                        Db.Insertable(order).ExecuteCommand();
                    }
                    #endregion

                    #region 明细表
                    var orderitem = new NC_PO_ORDER_B();
                    orderitem.DBILLDATE = DBILLDATE;
                    orderitem.PK_ORDER = PK_ORDER;
                    orderitem.PK_ORDER_B = PK_ORDER_B;
                    orderitem.CROWNO = CROWNO;//采购订单行号
                    orderitem.PK_SUPPLIER = PK_SUPPLIER;//供应商
                    orderitem.PK_MATERIAL = PK_MATERIAL;//物料
                    orderitem.PURUNITNAME = PURUNITNAME;
                    orderitem.NASTNUM = NASTNUM;//采购订单数量
                    orderitem.SECPURUNITNAME = SECPURUNITNAME;
                    orderitem.SECQUANTITY = NASTNUM;//采购订单数量
                    orderitem.NQTUNITNUM = NQTUNITNUM;//价格单位数量
                    orderitem.CQTUNITID = CQTUNITID;//订单价格单位
                    orderitem.NPRICE = NPRICE;//净价
                    orderitem.RECCOMPANYID = RECCOMPANYID;//工厂
                    orderitem.BARRIVECLOSE = "0";
                    if (ELIKZ == "X" || BARRIVECLOSE == "L")
                    {
                        ///交货已完成 X是 空否  
                     //L删除 空未删除 
                        orderitem.BARRIVECLOSE = "1";
                    }

                    //if (BARRIVECLOSE == "L")
                    //{
                    //    orderitem.BARRIVECLOSE = "1";
                    //}

                    orderitem.VBDEF4 = "0";
                    if (BARRIVECLOSE == "X")
                    {
                        orderitem.VBDEF4 = "1";
                    }
                    orderitem.RECCOMPANYID = RECCOMPANYID;//工厂
                    count = Db.Queryable<NC_PO_ORDER_B>().Where(r => r.PK_ORDER_B == PK_ORDER_B).Count();
                    if (count > 0)
                    {
                        Db.Updateable(orderitem).ExecuteCommand();
                    }
                    else
                    {
                        Db.Insertable(orderitem).ExecuteCommand();
                    }
                    #endregion

                    #region 基础档案
                    if (!string.IsNullOrEmpty(PK_SUPPLIER))
                    {
                        count = Db.Queryable<BD_SUPPLY>().Where(c => c.ID == PK_SUPPLIER).Count();
                        if (count == 0)
                        {
                            BD_SUPPLY sup_obj = new BD_SUPPLY()
                            {
                                ID = PK_SUPPLIER,
                                CODE = PK_SUPPLIER,
                                NAME = SUPPLYNAME,
                                SHORTNAME = SUPPLYNAME,
                                SPELLCODE = SUPPLYNAME.ToPinYin(),
                                ISUSE = "1",
                                DATATYPE = "1"
                            };
                            Db.Insertable(sup_obj).ExecuteCommand();
                        }
                        count = Db.Queryable<Base_UserKS>().Where(r => r.USERID == PK_SUPPLIER).Count();
                        if (count == 0)
                        {
                            #region 供应商用户
                            Base_UserKS user = new Base_UserKS();
                            user.USERID = model.LIFNR;
                            user.CODE = model.LIFNR;
                            user.PASSWORD = "0c164caaa094a707";
                            user.REALNAME = model.NAME1;
                            user.COMPANY = model.LIFNR;
                            user.COMPANYNAME = model.NAME1;
                            if (!string.IsNullOrEmpty(model.LIFNR))
                            {
                                user.ACCOUNT = model.LIFNR.Replace("0000", "");
                            }
                            else
                            {
                                user.ACCOUNT = model.LIFNR;
                            }

                            user.MOBILE = "";
                            user.ENABLED = 1;
                            user.TYPE = 2;
                            user.STYPE = 0;
                            user.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            user.CREATEUSER = "同步注册";
                            Db.Insertable(user).ExecuteCommand();
                            #endregion

                            #region 供应商用户权限表
                            Base_DataScopePermission userPerm = new Base_DataScopePermission();
                            userPerm.DataScopePermissionId = CommonHelper.GetGuid;
                            userPerm.ModuleId = "999eae8b-1d09-4678-9b5f-2901205f1d98";
                            userPerm.ObjectId = model.LIFNR;
                            userPerm.ResourceId = model.LIFNR;
                            userPerm.SortCode = 0;
                            userPerm.Category = "6";
                            userPerm.CreateDate = DateTime.Now;
                            userPerm.CreateUserId = "";
                            userPerm.CreateUserName = "同步注册";
                            userPerm.ScopeType = "1";
                            Db.Insertable(userPerm).ExecuteCommand();
                            #endregion
                        }
                    }

                    count = Db.Queryable<BD_MATERIAL>().Where(c => c.ID == PK_MATERIAL).Count();
                    if (count == 0)
                    {
                        BD_MATERIAL mat_obj = new BD_MATERIAL()
                        {
                            ID = PK_MATERIAL,
                            CODE = PK_MATERIAL,
                            NAME = MATERIAL,
                            SHORTNAME = MATERIAL,
                            SPELLCODE = MATERIAL.ToPinYin(),
                            PK_MARBASCLASS = PK_MARBASCLASS,//物料类型
                            PK_GROUP = PK_MATERIALGROUP,//物料组
                            MATERIALBARCODE = MATERIALBARCODE,//外部物料组
                            MATERIALSPEC = "",
                            DEF1 = model.MEINS,
                            DEF2 = model.MEINS,
                            ISUSE = "1",
                            ISJL = 1,
                            DATATYPE = "1"


                        };
                        Db.Insertable(mat_obj).ExecuteCommand();
                    }

                    #endregion
                    m_count++;
                    //}
                }
            }
            log.Error($"本次订单接收：{resulte.T_OUTTAB.Count()}条,成功{m_count}条！");
            if (resulte.T_OUTTAB.Length > 0)
            {
                log.Error(string.Format("采购订单接收:成功！"));
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                log.Error(string.Format("采购订单接收:失败！"));
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            log.Error($"采购订单接收:{strwerks}结束");
            return ret;
        }


        /// <summary>
        /// 生产机：SAP采购订单下载(项目使用)
        /// </summary>
        /// sxy/myt  SAP订单同步到WRZS 方法重写   20220718
        /// <param name="strzaedat">创建时间</param>
        /// <param name="strzbudat">修改时间</param>
        /// <param name="strwerks">工厂</param>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：SAP采购订单下载")]
        public RETURN_SAP SAP_116ZS01(string strzaedat, string strzbudat, string vbillcode, string strwerks, string supply, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            #region 初始化值
            string VBILLCODE = "";//采购凭证
            string PK_ORDER = "";//采购订单主键
            string BILLTYPE = "";//采购凭证类别
            string DBILLDATE = "";//采购凭证日期
            string FORDERSTATUS = "";//凭证状态
            string PK_SUPPLIER = "";//供应商编号
            string SUPPLYNAME = "";//供应商名称
            string PK_ORG = "";//采购公司
            string PK_DEPT = "";//采购分组
            string MODIFIEDTIME = "";//更改日期
            string DMAKEDATE = "";//创建日期
            string CREATIONTIME = "";//创建日期

            string CROWNO = "";//采购凭证行项目
            string PK_ORDER_B = string.Format("{0}{1}", PK_ORDER, CROWNO);
            string PK_MATERIAL = "";//物料编号
            string MATERIAL = "";//物料描述
            string PK_MARBASCLASS = "";//物料分组
            string PK_MATERIALGROUP = "";//物料类型
            string MATERIALBARCODE = "";// model.EXTWG;//外部物料组

            string PURUNITNAME = "";//订单单位
            decimal NASTNUM = 0;//采购订单数量
            string SECPURUNITNAME = "";//订单单位
            decimal SECQUANTITY = 0;//采购订单数量
            decimal NQTUNITNUM = 0; //价格单位数量
            string CQTUNITID = "";//订单价格单位
            decimal NPRICE = 0;//净价
            string RECCOMPANYID = "";//工厂
            string BARRIVECLOSE = "";//L删除 空未删除 
            string VBDEF4 = "";//是否过量收货 X是 空否
            string ELIKZ = "";//交货已完成 X是 空否
            string VBDEF3 = "";//SAP修改时间传入
            string ZBUDAT = "";
            string ZBUDATJ = "";//拼接用
            string ZAEDAT = "";
            string ZAEDATJ = "";//拼接用
            #endregion
            SugarParameter[] parametersmain = null;
            string sql = @"select * from VNC_PO_ORDER_MAIN where MAINTYPE in ('1','2')";//订单主表
            if (!string.IsNullOrEmpty(vbillcode))//条件拼接
            {
                sql += string.Format("and EBELN = '{0}'", vbillcode);
            }
            if (!string.IsNullOrEmpty(supply))
            {
                sql += string.Format("and LIFNR = '{0}'", supply);
            }
            if (!string.IsNullOrEmpty(strwerks))
            {
                sql += string.Format("and EKORG = '{0}'", strwerks);
            }
            DataTable maindataTable = Db.Ado.GetDataTable(sql, parametersmain);
            if (maindataTable.Rows.Count > 0)
            {
                for (int m = 0; m < maindataTable.Rows.Count; m++)
                {
                    #region 初始化值
                    ZBUDAT = maindataTable.Rows[m]["ZBUDAT"].ToString();
                    ZBUDATJ = ZBUDAT.Substring(0, 4) + "-" + ZBUDAT.Substring(4, 2) + "-" + ZBUDAT.Substring(6, 2);//拼接“-”创建日期
                    ZAEDAT = maindataTable.Rows[m]["ZAEDAT"].ToString();
                    ZAEDATJ = ZAEDAT.Substring(0, 4) + "-" + ZAEDAT.Substring(4, 2) + "-" + ZAEDAT.Substring(6, 2);//拼接“-”修改日期

                    VBILLCODE = maindataTable.Rows[m]["EBELN"].ToString();//采购凭证
                    BILLTYPE = maindataTable.Rows[m]["BSART"].ToString();//采购凭证类别
                    FORDERSTATUS = maindataTable.Rows[m]["PROCSTAT"].ToString();//凭证状态
                    PK_ORG = maindataTable.Rows[m]["EKORG"].ToString();//采购公司
                    PK_DEPT = maindataTable.Rows[m]["EKGRP"].ToString();//采购分组
                    MODIFIEDTIME = string.Format("{0} 00:00:00", ZBUDATJ);//更改日期
                    DMAKEDATE = string.Format("{0} 00:00:00", ZBUDATJ);//创建日期
                    CREATIONTIME = string.Format("{0} 00:00:00", ZBUDATJ);//创建日期
                    VBDEF3 = ZAEDAT;//SAP修改日期
                    PK_SUPPLIER = maindataTable.Rows[m]["LIFNR"].ToString();//供应商编号
                    DBILLDATE = string.Format("{0} 00:00:00", ZBUDATJ);//采购凭证日期
                    #endregion
                    #region 主表
                    var order = new NC_PO_ORDER();
                    order.VBILLCODE = VBILLCODE;//订单编号(采购凭证)
                    order.PK_ORDER = VBILLCODE;//订单主键
                    order.BILLTYPE = BILLTYPE;//采购凭证类别
                    order.DBILLDATE = DBILLDATE;//订单日期;
                    order.PK_SUPPLIER = PK_SUPPLIER;//供应商
                    order.PK_ORG = PK_ORG;//组织组织
                    order.PK_DEPT = PK_DEPT;//组织部门
                    order.FORDERSTATUS = 0;
                    order.CARSNUM = 0;
                    order.CREATIONTIME = CREATIONTIME;//创建日期;
                    order.DMAKEDATE = DMAKEDATE;//修改日期;
                    order.MODIFIEDTIME = MODIFIEDTIME;//修改日期;
                    order.DATATYPE = "1";
                    order.VDEF3 = VBDEF3;//SAP修改时间传入
                    if (maindataTable.Rows[m]["MAINTYPE"].ToString() == "1")
                    {
                        Db.Insertable(order).ExecuteCommand();
                    }
                    else if (maindataTable.Rows[m]["MAINTYPE"].ToString() == "2")
                    {
                        Db.Updateable(order).ExecuteCommand();
                    }
                    #endregion
                }
            }
            SugarParameter[] parameters = null;
            string sqldetail = @"select * from VNC_PO_ORDER_SAP  where 1=1 ";//订单明细表
            if (!string.IsNullOrEmpty(vbillcode))
            {
                sqldetail += string.Format("and EBELN = '{0}'", vbillcode);
            }
            if (!string.IsNullOrEmpty(supply))
            {
                sqldetail += string.Format("and LIFNR = '{0}'", supply);
            }
            if (!string.IsNullOrEmpty(strwerks))
            {
                sqldetail += string.Format("and EKORG = '{0}'", strwerks);
            }
            DataTable dataTable = Db.Ado.GetDataTable(sqldetail, parameters);
            int count = 0;
            int d_count = 0;
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    #region 初始化值
                    ZBUDAT = dataTable.Rows[i]["ZBUDAT"].ToString();
                    ZBUDATJ = ZBUDAT.Substring(0, 4) + "-" + ZBUDAT.Substring(4, 2) + "-" + ZBUDAT.Substring(6, 2);//拼接“-”创建日期
                    ZAEDAT = dataTable.Rows[i]["ZAEDAT"].ToString();

                    PK_ORDER = dataTable.Rows[i]["EBELN"].ToString();//采购订单主键
                    DBILLDATE = string.Format("{0} 00:00:00", ZBUDATJ);//采购凭证日期
                    PK_SUPPLIER = dataTable.Rows[i]["LIFNR"].ToString();//供应商编号
                    SUPPLYNAME = dataTable.Rows[i]["NAME1"].ToString();//供应商名称
                    VBDEF3 = ZAEDAT;//SAP修改时间传入
                    CROWNO = dataTable.Rows[i]["EBELP"].ToString();//采购凭证行项目
                    PK_ORDER_B = string.Format("{0}{1}", PK_ORDER, CROWNO);
                    PK_MATERIAL = dataTable.Rows[i]["MATNR"].ToString();//物料编号
                    MATERIAL = dataTable.Rows[i]["TXZ01"].ToString();//物料描述
                    PK_MARBASCLASS = dataTable.Rows[i]["MATKL"].ToString();//物料分组
                    PK_MATERIALGROUP = dataTable.Rows[i]["MTART"].ToString();//物料类型
                    MATERIALBARCODE = "";// dataTable.Rows[i]["EBELN"].ToString()EXTWG;//外部物料组
                    PURUNITNAME = dataTable.Rows[i]["MEINS"].ToString();//订单单位
                    NASTNUM = dataTable.Rows[i]["MENGE"].ToString().ToDecimal();//采购订单数量
                    SECPURUNITNAME = dataTable.Rows[i]["MEINS"].ToString();//订单单位
                    SECQUANTITY = dataTable.Rows[i]["MENGE"].ToString().ToDecimal();//采购订单数量
                    NQTUNITNUM = dataTable.Rows[i]["PEINH"].ToString().ToDecimal(); //价格单位数量
                    CQTUNITID = dataTable.Rows[i]["BPRME"].ToString();//订单价格单位
                    NPRICE = dataTable.Rows[i]["NETPR"].ToString().ToDecimal();//净价
                    RECCOMPANYID = dataTable.Rows[i]["WERKS"].ToString();//工厂
                    BARRIVECLOSE = dataTable.Rows[i]["LOEKZ"].ToString();//L删除 空未删除 
                    VBDEF4 = dataTable.Rows[i]["UEBTK"].ToString();//是否过量收货 X是 空否
                    ELIKZ = dataTable.Rows[i]["ELIKZ"].ToString();//交货已完成 X是 空否
                    #endregion
                    if (!string.IsNullOrEmpty(PK_MATERIAL))
                    {
                        #region 明细表
                        var orderitem = new NC_PO_ORDER_B();
                        orderitem.DBILLDATE = DBILLDATE;//订单凭证日期
                        orderitem.PK_ORDER = PK_ORDER;//订单编号
                        orderitem.PK_ORDER_B = PK_ORDER_B;//订单号拼行项目
                        orderitem.CROWNO = CROWNO;//采购订单行号
                        orderitem.PK_SUPPLIER = PK_SUPPLIER;//供应商
                        orderitem.PK_MATERIAL = PK_MATERIAL;//物料
                        orderitem.PURUNITNAME = PURUNITNAME;
                        orderitem.NASTNUM = NASTNUM;//采购订单数量
                        orderitem.SECPURUNITNAME = SECPURUNITNAME;
                        orderitem.SECQUANTITY = NASTNUM;//采购订单数量
                        orderitem.NQTUNITNUM = NQTUNITNUM;//价格单位数量
                        orderitem.CQTUNITID = CQTUNITID;//订单价格单位
                        orderitem.NPRICE = NPRICE;//净价
                        orderitem.RECCOMPANYID = RECCOMPANYID;//工厂
                        orderitem.BARRIVECLOSE = "0";
                        if (ELIKZ == "X" || BARRIVECLOSE == "L")
                        {
                            ///交货已完成 X是 空否  
                            //L删除 空未删除 
                            orderitem.BARRIVECLOSE = "1";
                        }
                        orderitem.VBDEF4 = "0";
                        if (VBDEF4 == "X")
                        {
                            orderitem.VBDEF4 = "1";
                        }
                        orderitem.VBDEF3 = VBDEF3;//SAP修改时间传入
                        if (dataTable.Rows[i]["DETAILTYPE"].ToString() == "1")
                        {
                            Db.Insertable(orderitem).ExecuteCommand();
                        }
                        else if (dataTable.Rows[i]["DETAILTYPE"].ToString() == "2")
                        {
                            Db.Updateable(orderitem).ExecuteCommand();
                        }
                        #endregion

                        #region 基础档案
                        if (!string.IsNullOrEmpty(PK_SUPPLIER))
                        {
                            count = Db.Queryable<BD_SUPPLY>().Where(c => c.ID == PK_SUPPLIER).Count();
                            if (count == 0)
                            {
                                BD_SUPPLY sup_obj = new BD_SUPPLY()
                                {
                                    ID = PK_SUPPLIER,
                                    CODE = PK_SUPPLIER,
                                    NAME = SUPPLYNAME,
                                    SHORTNAME = SUPPLYNAME,
                                    SPELLCODE = SUPPLYNAME.ToPinYin(),
                                    ISUSE = "1",
                                    DATATYPE = "1"
                                };
                                 Db.Insertable(sup_obj).ExecuteCommand();
                            }
                            count = Db.Queryable<Base_UserKS>().Where(r => r.USERID == PK_SUPPLIER).Count();
                            if (count == 0)
                            {
                                #region 供应商用户
                                Base_UserKS user = new Base_UserKS();
                                user.USERID = PK_SUPPLIER;
                                user.CODE = PK_SUPPLIER;
                                user.PASSWORD = "0c164caaa094a707";
                                user.REALNAME = SUPPLYNAME;
                                user.COMPANY = PK_SUPPLIER;
                                user.COMPANYNAME = SUPPLYNAME;
                                if (!string.IsNullOrEmpty(PK_SUPPLIER))
                                {
                                    user.ACCOUNT = PK_SUPPLIER.Replace("0000", "");
                                }
                                else
                                {
                                    user.ACCOUNT = PK_SUPPLIER;
                                }

                                user.MOBILE = "";
                                user.ENABLED = 1;
                                user.TYPE = 2;
                                user.STYPE = 0;
                                user.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                user.CREATEUSER = "同步注册";
                                Db.Insertable(user).ExecuteCommand();
                                #endregion

                                #region 供应商用户权限表
                                Base_DataScopePermission userPerm = new Base_DataScopePermission();
                                userPerm.DataScopePermissionId = CommonHelper.GetGuid;
                                userPerm.ModuleId = "999eae8b-1d09-4678-9b5f-2901205f1d98";
                                userPerm.ObjectId = PK_SUPPLIER;
                                userPerm.ResourceId = PK_SUPPLIER;
                                userPerm.SortCode = 0;
                                userPerm.Category = "6";
                                userPerm.CreateDate = DateTime.Now;
                                userPerm.CreateUserId = "";
                                userPerm.CreateUserName = "同步注册";
                                userPerm.ScopeType = "1";
                                Db.Insertable(userPerm).ExecuteCommand();
                                #endregion
                            }
                        }
                        count = Db.Queryable<BD_MATERIAL>().Where(c => c.ID == PK_MATERIAL).Count();
                        if (count == 0)
                        {
                            BD_MATERIAL mat_obj = new BD_MATERIAL()
                            {
                                ID = PK_MATERIAL,
                                CODE = PK_MATERIAL,
                                NAME = MATERIAL,
                                SHORTNAME = MATERIAL,
                                SPELLCODE = MATERIAL.ToPinYin(),
                                PK_MARBASCLASS = PK_MARBASCLASS,//物料类型
                                PK_GROUP = PK_MATERIALGROUP,//物料组
                                MATERIALBARCODE = MATERIALBARCODE,//外部物料组
                                MATERIALSPEC = "",
                                DEF1 = PURUNITNAME,
                                DEF2 = PURUNITNAME,
                                ISUSE = "1",
                                ISJL = 1,
                                DATATYPE = "1"
                            };
                            Db.Insertable(mat_obj).ExecuteCommand();
                        }
                        #endregion
                        d_count++;
                    }
                }
            }
            var resulte = dataTable.Rows;
            log.Error($"本次订单接收：{resulte.Count}条,成功{d_count}条！");
            if (resulte.Count > 0)
            {
                log.Error(string.Format("采购订单接收:成功！"));
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                log.Error(string.Format("采购订单接收:失败！"));
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            log.Error($"采购订单接收:{strwerks}结束");
            return ret;
        }


        /// <summary>
        /// 生产机：SAP采购订单下载(项目使用)
        /// </summary>
        /// <param name="strzaedat">创建时间</param>
        /// <param name="strzbudat">修改时间</param>
        /// <param name="strwerks">工厂</param>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：SAP采购订单下载")]
        public RETURN_SAP SAP_116ZS_New(string strzaedat, string strzbudat, string vbillcode, string strwerks, string supply, string type, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            //调用接口

            log.Error($"采购订单接收:{strwerks}开始");

            int count = 0;
            int m_count = 0;
            //log.Error(string.Format("采购订单接收:明细记录数{0}", T_OUTTAB.Count()));
            #region 初始化值
            string VBILLCODE = "";//采购凭证
            string PK_ORDER = "";//采购订单主键
            string BILLTYPE = "";//采购凭证类别
            string DBILLDATE = "";//采购凭证日期
            string FORDERSTATUS = "";//凭证状态
            string PK_SUPPLIER = "";//供应商编号
            string SUPPLYNAME = "";//供应商名称
            string PK_ORG = "";//采购公司
            string PK_DEPT = "";//采购分组
            string MODIFIEDTIME = "";//更改日期
            string DMAKEDATE = "";//创建日期
            string CREATIONTIME = "";//创建日期

            string CROWNO = "";//采购凭证行项目
            string PK_ORDER_B = string.Format("{0}{1}", PK_ORDER, CROWNO);
            string PK_MATERIAL = "";//物料编号
            string MATERIAL = "";//物料描述
            string PK_MARBASCLASS = "";//物料分组
            string PK_MATERIALGROUP = "";//物料类型
            string MATERIALBARCODE = "";// model.EXTWG;//外部物料组

            string PURUNITNAME = "";//订单单位
            decimal NASTNUM = 0;//采购订单数量
            string SECPURUNITNAME = "";//订单单位
            decimal SECQUANTITY = 0;//采购订单数量
            decimal NQTUNITNUM = 0; //价格单位数量
            string CQTUNITID = "";//订单价格单位
            decimal NPRICE = 0;//净价
            string RECCOMPANYID = "";//工厂
            string BARRIVECLOSE = "";//L删除 空未删除 
            string VBDEF4 = "";//是否过量收货 X是 空否
            string ELIKZ = "";//交货已完成 X是 空否
            #endregion
            #region 查询数据
            var T_OUTTAB = Db.Queryable<VNC_PO_ORDER_SAP>()
            .WhereIF(!string.IsNullOrEmpty(strwerks), c => c.WERKS == strwerks)
             .ToPageList(1, 100);
            #endregion
            foreach (var model in T_OUTTAB)
            {
                #region 初始化值
                VBILLCODE = model.EBELN;//采购凭证
                PK_ORDER = model.EBELN;//采购订单主键
                BILLTYPE = model.BSART;//采购凭证类别
                DBILLDATE = string.Format("{0} 00:00:00", model.ZBUDAT);//采购凭证日期
                FORDERSTATUS = model.PROCSTAT;//凭证状态
                PK_SUPPLIER = model.LIFNR;//供应商编号
                SUPPLYNAME = model.NAME1;//供应商名称
                PK_ORG = model.EKORG;//采购公司
                PK_DEPT = model.EKGRP;//采购分组
                MODIFIEDTIME = string.Format("{0} 00:00:00", model.ZAEDAT);//更改日期
                DMAKEDATE = string.Format("{0} 00:00:00", model.ZBUDAT);//创建日期
                CREATIONTIME = string.Format("{0} 00:00:00", model.ZBUDAT);//创建日期

                CROWNO = model.EBELP;//采购凭证行项目
                PK_ORDER_B = string.Format("{0}{1}", PK_ORDER, CROWNO);
                PK_MATERIAL = model.MATNR;//物料编号
                MATERIAL = model.TXZ01;//物料描述
                PK_MARBASCLASS = model.MATKL;//物料分组
                PK_MATERIALGROUP = model.MTART;//物料类型
                MATERIALBARCODE = "";// model.EXTWG;//外部物料组

                PURUNITNAME = model.MEINS;//订单单位
                NASTNUM = model.MENGE;//采购订单数量
                SECPURUNITNAME = model.MEINS;//订单单位
                SECQUANTITY = model.MENGE;//采购订单数量
                NQTUNITNUM = model.PEINH; //价格单位数量
                CQTUNITID = model.BPRME;//订单价格单位
                NPRICE = model.NETPR;//净价
                RECCOMPANYID = model.WERKS;//工厂
                BARRIVECLOSE = model.LOEKZ;//L删除 空未删除 
                VBDEF4 = model.UEBTK;//是否过量收货 X是 空否
                ELIKZ = model.ELIKZ;//交货已完成 X是 空否

                #endregion
                // //log.Error("采购订单接收:开始");
                //if (ELIKZ == "X")
                //{
                //    FORDERSTATUS = "00";
                //}

                if (!string.IsNullOrEmpty(PK_MATERIAL))
                {
                    //未收货
                    //if (string.IsNullOrEmpty(ELIKZ))
                    //{
                    // log.Error($"采购订单接收:订单编号{VBILLCODE}、订单行号{CROWNO}");
                    #region 主表
                    var order = new NC_PO_ORDER();
                    order.VBILLCODE = VBILLCODE;//订单编号(采购凭证)
                    order.PK_ORDER = PK_ORDER;//订单主键
                    order.BILLTYPE = BILLTYPE;
                    order.DBILLDATE = DBILLDATE;//订单日期;
                    order.PK_SUPPLIER = PK_SUPPLIER;//供应商
                    order.PK_ORG = PK_ORG;//组织组织
                    order.PK_DEPT = PK_DEPT;//组织部门

                    //FORDERSTATUS
                    //if (FORDERSTATUS == "02" || FORDERSTATUS == "05")
                    //{
                    order.FORDERSTATUS = 0;
                    //}
                    //else
                    //{
                    //    order.FORDERSTATUS = 1;
                    //}

                    order.CARSNUM = 0;
                    order.CREATIONTIME = CREATIONTIME;//创建日期;
                    order.DMAKEDATE = DMAKEDATE;//修改日期;
                    order.MODIFIEDTIME = MODIFIEDTIME;//修改日期;
                    order.DATATYPE = "1";
                    count = Db.Queryable<NC_PO_ORDER>().Where(r => r.PK_ORDER == order.PK_ORDER).Count();
                    if (count > 0)
                    {
                        Db.Updateable(order).ExecuteCommand();
                    }
                    else
                    {
                        Db.Insertable(order).ExecuteCommand();
                    }
                    #endregion

                    #region 明细表
                    var orderitem = new NC_PO_ORDER_B();
                    orderitem.DBILLDATE = DBILLDATE;
                    orderitem.PK_ORDER = PK_ORDER;
                    orderitem.PK_ORDER_B = PK_ORDER_B;
                    orderitem.CROWNO = CROWNO;//采购订单行号
                    orderitem.PK_SUPPLIER = PK_SUPPLIER;//供应商
                    orderitem.PK_MATERIAL = PK_MATERIAL;//物料
                    orderitem.PURUNITNAME = PURUNITNAME;
                    orderitem.NASTNUM = NASTNUM;//采购订单数量
                    orderitem.SECPURUNITNAME = SECPURUNITNAME;
                    orderitem.SECQUANTITY = NASTNUM;//采购订单数量
                    orderitem.NQTUNITNUM = NQTUNITNUM;//价格单位数量
                    orderitem.CQTUNITID = CQTUNITID;//订单价格单位
                    orderitem.NPRICE = NPRICE;//净价
                    orderitem.RECCOMPANYID = RECCOMPANYID;//工厂
                    orderitem.BARRIVECLOSE = "0";
                    if (ELIKZ == "X" || BARRIVECLOSE == "L")
                    {
                        ///交货已完成 X是 空否  
                     //L删除 空未删除 
                        orderitem.BARRIVECLOSE = "1";
                    }

                    //if (BARRIVECLOSE == "L")
                    //{
                    //    orderitem.BARRIVECLOSE = "1";
                    //}

                    orderitem.VBDEF4 = "0";
                    if (BARRIVECLOSE == "X")
                    {
                        orderitem.VBDEF4 = "1";
                    }
                    orderitem.RECCOMPANYID = RECCOMPANYID;//工厂
                    count = Db.Queryable<NC_PO_ORDER_B>().Where(r => r.PK_ORDER_B == PK_ORDER_B).Count();
                    if (count > 0)
                    {
                        Db.Updateable(orderitem).ExecuteCommand();
                    }
                    else
                    {
                        Db.Insertable(orderitem).ExecuteCommand();
                    }
                    #endregion

                    #region 基础档案
                    if (!string.IsNullOrEmpty(PK_SUPPLIER))
                    {
                        count = Db.Queryable<BD_SUPPLY>().Where(c => c.ID == PK_SUPPLIER).Count();
                        if (count == 0)
                        {
                            BD_SUPPLY sup_obj = new BD_SUPPLY()
                            {
                                ID = PK_SUPPLIER,
                                CODE = PK_SUPPLIER,
                                NAME = SUPPLYNAME,
                                SHORTNAME = SUPPLYNAME,
                                SPELLCODE = SUPPLYNAME.ToPinYin(),
                                ISUSE = "1",
                                DATATYPE = "1"
                            };
                            Db.Insertable(sup_obj).ExecuteCommand();
                        }
                        count = Db.Queryable<Base_UserKS>().Where(r => r.USERID == PK_SUPPLIER).Count();
                        if (count == 0)
                        {
                            #region 供应商用户
                            Base_UserKS user = new Base_UserKS();
                            user.USERID = model.LIFNR;
                            user.CODE = model.LIFNR;
                            user.PASSWORD = "0c164caaa094a707";
                            user.REALNAME = model.NAME1;
                            user.COMPANY = model.LIFNR;
                            user.COMPANYNAME = model.NAME1;
                            if (!string.IsNullOrEmpty(model.LIFNR))
                            {
                                user.ACCOUNT = model.LIFNR.Replace("0000", "");
                            }
                            else
                            {
                                user.ACCOUNT = model.LIFNR;
                            }

                            user.MOBILE = "";
                            user.ENABLED = 1;
                            user.TYPE = 2;
                            user.STYPE = 0;
                            user.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            user.CREATEUSER = "同步注册";
                            Db.Insertable(user).ExecuteCommand();
                            #endregion

                            #region 供应商用户权限表
                            Base_DataScopePermission userPerm = new Base_DataScopePermission();
                            userPerm.DataScopePermissionId = CommonHelper.GetGuid;
                            userPerm.ModuleId = "999eae8b-1d09-4678-9b5f-2901205f1d98";
                            userPerm.ObjectId = model.LIFNR;
                            userPerm.ResourceId = model.LIFNR;
                            userPerm.SortCode = 0;
                            userPerm.Category = "6";
                            userPerm.CreateDate = DateTime.Now;
                            userPerm.CreateUserId = "";
                            userPerm.CreateUserName = "同步注册";
                            userPerm.ScopeType = "1";
                            Db.Insertable(userPerm).ExecuteCommand();
                            #endregion
                        }
                    }

                    count = Db.Queryable<BD_MATERIAL>().Where(c => c.ID == PK_MATERIAL).Count();
                    if (count == 0)
                    {
                        BD_MATERIAL mat_obj = new BD_MATERIAL()
                        {
                            ID = PK_MATERIAL,
                            CODE = PK_MATERIAL,
                            NAME = MATERIAL,
                            SHORTNAME = MATERIAL,
                            SPELLCODE = MATERIAL.ToPinYin(),
                            PK_MARBASCLASS = PK_MARBASCLASS,//物料类型
                            PK_GROUP = PK_MATERIALGROUP,//物料组
                            MATERIALBARCODE = MATERIALBARCODE,//外部物料组
                            MATERIALSPEC = "",
                            DEF1 = model.MEINS,
                            DEF2 = model.MEINS,
                            ISUSE = "1",
                            ISJL = 1,
                            DATATYPE = "1"


                        };
                        Db.Insertable(mat_obj).ExecuteCommand();
                    }

                    #endregion
                    m_count++;
                    //}
                }
            }
            log.Error($"本次订单接收：{ T_OUTTAB.Count()}条,成功{m_count}条！");
            if (T_OUTTAB.Count > 0)
            {
                log.Error(string.Format("采购订单接收:成功！"));
                ret.STATUS = "S";
                ret.REMSG = "获取数据成功！";
                ret.REDATA = null;
            }
            else
            {
                log.Error(string.Format("采购订单接收:失败！"));
                ret.STATUS = "E";
                ret.REMSG = "获取数据失败！";
                ret.REDATA = null;
            }
            log.Error($"采购订单接收:{strwerks}结束");
            return ret;
        }


        #endregion

        #region 采购收货业务

        /// <summary>
        /// 生产机：AJAX国外废纸收货(二次计量调用)/化工收货推送(一次计量推送)
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "生产机：AJAX国外废纸收货(二次计量调用)/化工收货推送(一次计量推送)")]
        public RETURN_SAP SAP_POSTZS1(string EOS_GM_CODE, string DOC_DATE, string PSTNG_DATE, string PO_NUMBER, string PO_ITEM, string MATERIAL, string BATCH, decimal ENTRY_QNT, string STGE_LOC, string PLANT, string MOVE_TYPE, string MVT_IND)
        {
            #region 
            EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            entity.EOS_GM_CODE = EOS_GM_CODE;
            entity.DOC_DATE = DOC_DATE;
            entity.PSTNG_DATE = PSTNG_DATE;
            entity.BILL_OF_LADING = "";
            entity.GR_GI_SLIP_NO = "";
            entity.PO_NUMBER = PO_NUMBER;//订单编号
            entity.PO_ITEM = PO_ITEM;//订单项目编号
            entity.MATERIAL = MATERIAL;//物料
            entity.BATCH = BATCH;//批次
            entity.ENTRY_QNT = ENTRY_QNT;//数量
            entity.STGE_LOC = STGE_LOC;// "Y490";//库存地点
            entity.PLANT = PLANT;//工厂
            entity.MOVE_TYPE = MOVE_TYPE;//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            entity.MVT_IND = MVT_IND;//移动标识,B,F,B是采购订单  F是生产订单
            entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
            #endregion
            return SAP_POSTZS(entity, "");
        }

        /// <summary>
        /// 生产机：国外废纸收货(二次计量调用)/化工收货推送(一次计量推送)
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：国外废纸收货(二次计量调用)/化工收货推送(一次计量推送)")]
        public RETURN_SAP SAP_POSTZS(EOS_Z_PO_POST entity, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();

            #region 测试
            //EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            //entity.EOS_GM_CODE = "01";
            //entity.DOC_DATE = "2021-09-28";// DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PSTNG_DATE = "2021-09-28";//DateTime.Now.ToString("yyyy-MM-dd");
            //entity.BILL_OF_LADING = "";
            //entity.GR_GI_SLIP_NO = "";
            //entity.PO_NUMBER = "4700007834";//订单编号
            //entity.PO_ITEM = "00010";//订单项目编号
            //entity.MATERIAL = "000000000003010087";//物料
            //entity.BATCH = "2021092802";//批次
            //entity.ENTRY_QNT = 160;//数量
            //entity.STGE_LOC = "Y089";// "Y490";//库存地点
            //entity.PLANT = "1000";//工厂
            //entity.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            //entity.MVT_IND = "B";//移动标识,B,F,B是采购订单  F是生产订单
            //entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.EOS_GM_CODE))
            {
                entity.EOS_GM_CODE = "01";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(entity.PSTNG_DATE))
            {
                entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }

            #endregion

            #region 初始化接口
            SAPSERVICES_ZPZS1.SI_Z_PO_POST_OUTClient sap = new SAPSERVICES_ZPZS1.SI_Z_PO_POST_OUTClient("HTTPS_PORTPOSTZS");

            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"];
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_ZPZS1.Z_PO_POST z_po_post = new SAPSERVICES_ZPZS1.Z_PO_POST();
            z_po_post.GOODSMVT_CODE = new SAPSERVICES_ZPZS1.BAPI2017_GM_CODE();
            z_po_post.GOODSMVT_CODE.GM_CODE = entity.EOS_GM_CODE;//01采购订单收货,02生产订单收货
            z_po_post.GOODSMVT_HEADER = new SAPSERVICES_ZPZS1.BAPI2017_GM_HEAD_01();
            z_po_post.GOODSMVT_HEADER.DOC_DATE = entity.DOC_DATE;//DateTime.Now.ToString("yyyy-MM-dd");//凭证中的凭证日期,当前系统日期
            z_po_post.GOODSMVT_HEADER.PSTNG_DATE = entity.PSTNG_DATE;// DateTime.Now.ToString("yyyy-MM-dd");//凭证中的过账日期-可人工修改重新传SAP,系统当前日期
            z_po_post.GOODSMVT_HEADER.BILL_OF_LADING = entity.BILL_OF_LADING;//收货时提单号
            z_po_post.GOODSMVT_HEADER.REF_DOC_NO = entity.BILL_OF_LADING;//收货时提单号 
            z_po_post.GOODSMVT_HEADER.GR_GI_SLIP_NO = entity.GR_GI_SLIP_NO;//收货/发货单编号 
            z_po_post.GOODSMVT_HEADER.HEADER_TXT = entity.HEADER_TXT;// "无人计量采购过账";//凭证抬头文本



            #region GOODSMVT_HEADER
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO = "";//参考凭证编号
            //z_po_post.GOODSMVT_HEADER.PR_UNAME = "";//用户名 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIP = "";//收货/发货单打印版本 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIPX = "";//相关用户数据字段的已更新信息
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO_LONG = "";//参考凭证编号(对相关性参看长文本)
            //z_po_post.GOODSMVT_HEADER.BILL_OF_LADING_LONG = "";//组的检配单编号(根据：参见长文本)
            //z_po_post.GOODSMVT_HEADER.BAR_CODE = "";//条形码输入
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账 
            #endregion

            #region 初始化ZMSEG_C
            z_po_post.ZMSEG_C = new SAPSERVICES_ZPZS1.ZMSEG_C();
            #region 注释
            //z_po_post.ZMSEG_C.MANDT = "";//集团
            //z_po_post.ZMSEG_C.MBLNR = "";//物料凭证编号
            //z_po_post.ZMSEG_C.MJAHR = "";//物料凭证的年份
            //z_po_post.ZMSEG_C.CHEHO = "";//车牌号
            //z_po_post.ZMSEG_C.XNHAO = "";//箱号
            //z_po_post.ZMSEG_C.CTNER = "";//柜型
            //z_po_post.ZMSEG_C.JKDAT = "";//进口日期
            //z_po_post.ZMSEG_C.LIFN1 = "";//供应商或债券人的账户
            //z_po_post.ZMSEG_C.NAME9 = "";//名称
            #endregion
            z_po_post.ZMSEG_C.LIFNR = entity.LIFNR;//供应商或债券人的账户
            z_po_post.ZMSEG_C.EBELN = entity.EBELN;//采购凭证编号
            z_po_post.ZMSEG_C.MATNR = entity.MATNR;//物料编号
            z_po_post.ZMSEG_C.MENGE = entity.MENGE;//采购订单数量
            z_po_post.ZMSEG_C.MENGESpecified = true;
            z_po_post.ZMSEG_C.EINDT = entity.EINDT;//项目交货日期
            z_po_post.ZMSEG_C.WERKS = entity.WERKS;//工厂
            z_po_post.ZMSEG_C.NETPR = entity.NETPR;//净价
            z_po_post.ZMSEG_C.NETPRSpecified = true;
            z_po_post.ZMSEG_C.PQ = entity.PQ;//纸质类型
            #endregion

            #region 初始化 GOODSMVT_ITEM
            z_po_post.GOODSMVT_ITEM = new SAPSERVICES_ZPZS1.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_ZPZS1.BAPI2017_GM_ITEM_CREATE gm_item = new SAPSERVICES_ZPZS1.BAPI2017_GM_ITEM_CREATE();

            gm_item.MATERIAL = entity.MATERIAL;//物料
            gm_item.PLANT = entity.PLANT;//工厂
            gm_item.STGE_LOC = entity.STGE_LOC;//库存地点
            gm_item.BATCH = entity.BATCH;//批次
            gm_item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            gm_item.ENTRY_QNT = entity.ENTRY_QNT;//数量
            gm_item.ENTRY_QNTSpecified = true;
            gm_item.PO_NUMBER = entity.PO_NUMBER;//订单编号
            gm_item.PO_ITEM = entity.PO_ITEM;//订单项目编号
            gm_item.ORDERID = entity.ORDERID;//订单编号
            gm_item.ORDER_ITNO = entity.ORDER_ITNO;//订单项目编号
            gm_item.MVT_IND = entity.MVT_IND;//移动标识,B,F,B是采购订单  F是生产订单
            gm_item.SPEC_STOCK = entity.SPEC_STOCK;//特殊库存标识,K寄售,空
            gm_item.VENDRBATCH = entity.VENDRBATCH;//计量单号
            #region   GOODSMVT_ITEM
            gm_item.S_ORD_ITEM = "000000";
            gm_item.SCHED_LINE = "0000";
            gm_item.PO_PR_QNT = 0;
            gm_item.RESERV_NO = "0000000000";
            gm_item.RES_ITEM = "0000";
            gm_item.MOVE_REAS = "0000";
            gm_item.PROFIT_SEGM_NO = "0000000000";
            gm_item.AMOUNT_LC = 0;
            gm_item.AMOUNT_SV = 0;
            gm_item.REF_DOC_IT = "0000";
            gm_item.VAL_S_ORD_ITEM = "000000";
            gm_item.DELIV_NUMB_TO_SEARCH = "000000";
            gm_item.SU_PL_STCK_1 = 0;
            gm_item.ST_UN_QTYY_1 = 0;
            gm_item.SU_PL_STCK_2 = 0;
            gm_item.ST_UN_QTYY_2 = 0;
            gm_item.MATITEM_TR_CANCEL = "0000";
            gm_item.LINE_ID = "000000";
            gm_item.PARENT_ID = "000000";
            gm_item.LINE_DEPTH = "00";
            gm_item.QUANTITY = 0;
            gm_item.EARMARKED_ITEM = "000";

            #region 注释
            // gm_item.STCK_TYPE = "2";//库存类型:化工：2质量检验 废纸：空 非限制使用 3已冻结
            ////单位
            //gm_item.ENTRY_UOM = "";
            ////计量单位ISO代码
            //gm_item.ENTRY_UOM_ISO = "";
            ////采购订单价格单位的数量
            //gm_item.PO_PR_QNT = 0;
            ////订单价格单位(采购)
            //gm_item.ORDERPR_UN = "";
            ////计量单位ISO代码
            //gm_item.ORDERPR_UN_ISO = "";
            //gm_item.PO_NUMBER = "4300004787";//采购订单号
            //gm_item.PO_ITEM = "10";//采购凭证的项目编号
            //gm_item.VENDOR = "106544";//供应商账户号

            //gm_item.SHIPPING = "";//装运指令
            //gm_item.COMP_SHIP = "";//依照装运通知
            //gm_item.NO_MORE_GR = "";//交货已完成标识 
            //gm_item.ITEM_TEXT = "";//项目文本 
            //gm_item.GR_RCPT = "";//收货方 
            //gm_item.UNLOAD_PT = "";//卸货点
            //gm_item.COSTCENTER = "";//成本中心
            //gm_item.VAL_TYPE = "";//评估类型
            //gm_item.CUSTOMER = "";//客户的账户编号
            //gm_item.SALES_ORD = "";//销售订单数
            //gm_item.S_ORD_ITEM = "";//销售订单中的项目编号
            #endregion
            z_po_post.GOODSMVT_ITEM[0] = gm_item;
            #endregion

            #endregion

            #region 初始化 RETURN
            z_po_post.RETURN = new SAPSERVICES_ZPZS1.BAPIRET2[1];
            SAPSERVICES_ZPZS1.BAPIRET2 bapiret = new SAPSERVICES_ZPZS1.BAPIRET2();

            //bapiret.ID = "";
            //bapiret.TYPE = "";
            //bapiret.NUMBER = "";
            //bapiret.MESSAGE = "";
            //bapiret.LOG_NO = "";
            //bapiret.LOG_MSG_NO = "";
            //bapiret.MESSAGE_V1 = "";
            //bapiret.MESSAGE_V2 = "";
            //bapiret.MESSAGE_V3 = "";
            //bapiret.MESSAGE_V4 = "";
            //bapiret.PARAMETER = "";
            //bapiret.ROW = "";
            //bapiret.FIELD = "";
            //bapiret.SYSTEM = "";
            z_po_post.RETURN[0] = bapiret;
            #endregion

            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_PO_POST_OUT(z_po_post);
            if (resulte.RC == "0")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                string PRUEFLOS = resulte.PRUEFLOS;//检验批号
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                //物料凭证号(推送单据号) MATERIALDOCUMENT = "5003845836"
                String MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                string E_POSNR = resulte.E_POSNR;
                REDATA redata = new REDATA();
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;//凭证编号
                redata.PRUEFLOS = PRUEFLOS;
                redata.E_POSNR = E_POSNR;
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    ret.REMSG = string.Format("失败：{0}", resulte.RETURN[0].MESSAGE);
                }
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }



        /// <summary>
        /// 生产机：化工收货推送(一次计量推送)
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：化工收货推送(一次计量推送)")]
        public RETURN_SAP SAP_POST1ZS(EOS_Z_PO_POST entity, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();

            #region 测试
            //EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            //entity.EOS_GM_CODE = "01";
            //entity.DOC_DATE = "2021-09-28";// DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PSTNG_DATE = "2021-09-28";//DateTime.Now.ToString("yyyy-MM-dd");
            //entity.BILL_OF_LADING = "";
            //entity.GR_GI_SLIP_NO = "";
            //entity.PO_NUMBER = "4700007834";//订单编号
            //entity.PO_ITEM = "00010";//订单项目编号
            //entity.MATERIAL = "000000000003010087";//物料
            //entity.BATCH = "2021092802";//批次
            //entity.ENTRY_QNT = 160;//数量
            //entity.STGE_LOC = "Y089";// "Y490";//库存地点
            //entity.PLANT = "1000";//工厂
            //entity.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            //entity.MVT_IND = "B";//移动标识,B,F,B是采购订单  F是生产订单
            //entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.EOS_GM_CODE))
            {
                entity.EOS_GM_CODE = "01";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(entity.PSTNG_DATE))
            {
                entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }

            #endregion

            #region 初始化接口
            SAPSERVICES_ZPZS3.SI_Z_PO_POST1_OUTClient sap = new SAPSERVICES_ZPZS3.SI_Z_PO_POST1_OUTClient("HTTPS_PORTPOSTZS3");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_ZPZS3.Z_PO_POST1 z_po_post = new SAPSERVICES_ZPZS3.Z_PO_POST1();
            z_po_post.GOODSMVT_CODE = new SAPSERVICES_ZPZS3.BAPI2017_GM_CODE();
            z_po_post.GOODSMVT_CODE.GM_CODE = entity.EOS_GM_CODE;//01采购订单收货,02生产订单收货
            z_po_post.GOODSMVT_HEADER = new SAPSERVICES_ZPZS3.BAPI2017_GM_HEAD_01();
            z_po_post.GOODSMVT_HEADER.DOC_DATE = entity.DOC_DATE;//DateTime.Now.ToString("yyyy-MM-dd");//凭证中的凭证日期,当前系统日期
            z_po_post.GOODSMVT_HEADER.PSTNG_DATE = entity.PSTNG_DATE;// DateTime.Now.ToString("yyyy-MM-dd");//凭证中的过账日期-可人工修改重新传SAP,系统当前日期
            z_po_post.GOODSMVT_HEADER.BILL_OF_LADING = entity.BILL_OF_LADING;//收货时提单号
            z_po_post.GOODSMVT_HEADER.GR_GI_SLIP_NO = entity.GR_GI_SLIP_NO;//收货/发货单编号
            z_po_post.GOODSMVT_HEADER.HEADER_TXT = entity.HEADER_TXT;// "无人计量采购过账";//凭证抬头文本

            #region GOODSMVT_HEADER
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO = "";//参考凭证编号
            //z_po_post.GOODSMVT_HEADER.PR_UNAME = "";//用户名 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIP = "";//收货/发货单打印版本 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIPX = "";//相关用户数据字段的已更新信息
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO_LONG = "";//参考凭证编号(对相关性参看长文本)
            //z_po_post.GOODSMVT_HEADER.BILL_OF_LADING_LONG = "";//组的检配单编号(根据：参见长文本)
            //z_po_post.GOODSMVT_HEADER.BAR_CODE = "";//条形码输入
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账 
            #endregion

            #region 初始化ZMSEG_C
            z_po_post.ZMSEG_C = new SAPSERVICES_ZPZS3.ZMSEG_C();
            #region 注释
            //z_po_post.ZMSEG_C.MANDT = "";//集团
            //z_po_post.ZMSEG_C.MBLNR = "";//物料凭证编号
            //z_po_post.ZMSEG_C.MJAHR = "";//物料凭证的年份
            //z_po_post.ZMSEG_C.CHEHO = "";//车牌号
            //z_po_post.ZMSEG_C.XNHAO = "";//箱号
            //z_po_post.ZMSEG_C.CTNER = "";//柜型
            //z_po_post.ZMSEG_C.JKDAT = "";//进口日期
            //z_po_post.ZMSEG_C.LIFN1 = "";//供应商或债券人的账户
            //z_po_post.ZMSEG_C.NAME9 = "";//名称
            #endregion
            z_po_post.ZMSEG_C.LIFNR = entity.LIFNR;//供应商或债券人的账户
            z_po_post.ZMSEG_C.EBELN = entity.EBELN;//采购凭证编号
            z_po_post.ZMSEG_C.MATNR = entity.MATNR;//物料编号
            z_po_post.ZMSEG_C.MENGE = entity.MENGE;//采购订单数量
            z_po_post.ZMSEG_C.MENGESpecified = true;
            z_po_post.ZMSEG_C.EINDT = entity.EINDT;//项目交货日期
            z_po_post.ZMSEG_C.WERKS = entity.WERKS;//工厂
            z_po_post.ZMSEG_C.NETPR = entity.NETPR;//净价
            z_po_post.ZMSEG_C.NETPRSpecified = true;
            z_po_post.ZMSEG_C.PQ = entity.PQ;//纸质类型
            #endregion

            #region 初始化 GOODSMVT_ITEM
            z_po_post.GOODSMVT_ITEM = new SAPSERVICES_ZPZS3.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_ZPZS3.BAPI2017_GM_ITEM_CREATE gm_item = new SAPSERVICES_ZPZS3.BAPI2017_GM_ITEM_CREATE();

            gm_item.MATERIAL = entity.MATERIAL;//物料
            gm_item.PLANT = entity.PLANT;//工厂
            gm_item.STGE_LOC = entity.STGE_LOC;//库存地点
            gm_item.BATCH = entity.BATCH;//批次
            gm_item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            gm_item.ENTRY_QNT = entity.ENTRY_QNT;//数量
            gm_item.ENTRY_QNTSpecified = true;
            gm_item.PO_NUMBER = entity.PO_NUMBER;//订单编号
            gm_item.PO_ITEM = entity.PO_ITEM;//订单项目编号
            gm_item.ORDERID = entity.ORDERID;//订单编号
            gm_item.ORDER_ITNO = entity.ORDER_ITNO;//订单项目编号
            gm_item.MVT_IND = entity.MVT_IND;//移动标识,B,F,B是采购订单  F是生产订单
            gm_item.SPEC_STOCK = entity.SPEC_STOCK;//特殊库存标识,K寄售,空

            #region   GOODSMVT_ITEM
            gm_item.S_ORD_ITEM = "000000";
            gm_item.SCHED_LINE = "0000";
            gm_item.PO_PR_QNT = 0;
            gm_item.RESERV_NO = "0000000000";
            gm_item.RES_ITEM = "0000";
            gm_item.MOVE_REAS = "0000";
            gm_item.PROFIT_SEGM_NO = "0000000000";
            gm_item.AMOUNT_LC = 0;
            gm_item.AMOUNT_SV = 0;
            gm_item.REF_DOC_IT = "0000";
            gm_item.VAL_S_ORD_ITEM = "000000";
            gm_item.DELIV_NUMB_TO_SEARCH = "000000";
            gm_item.SU_PL_STCK_1 = 0;
            gm_item.ST_UN_QTYY_1 = 0;
            gm_item.SU_PL_STCK_2 = 0;
            gm_item.ST_UN_QTYY_2 = 0;
            gm_item.MATITEM_TR_CANCEL = "0000";
            gm_item.LINE_ID = "000000";
            gm_item.PARENT_ID = "000000";
            gm_item.LINE_DEPTH = "00";
            gm_item.QUANTITY = 0;
            gm_item.EARMARKED_ITEM = "000";

            #region 注释
            // gm_item.STCK_TYPE = "2";//库存类型:化工：2质量检验 废纸：空 非限制使用 3已冻结
            ////单位
            //gm_item.ENTRY_UOM = "";
            ////计量单位ISO代码
            //gm_item.ENTRY_UOM_ISO = "";
            ////采购订单价格单位的数量
            //gm_item.PO_PR_QNT = 0;
            ////订单价格单位(采购)
            //gm_item.ORDERPR_UN = "";
            ////计量单位ISO代码
            //gm_item.ORDERPR_UN_ISO = "";
            //gm_item.PO_NUMBER = "4300004787";//采购订单号
            //gm_item.PO_ITEM = "10";//采购凭证的项目编号
            //gm_item.VENDOR = "106544";//供应商账户号

            //gm_item.SHIPPING = "";//装运指令
            //gm_item.COMP_SHIP = "";//依照装运通知
            //gm_item.NO_MORE_GR = "";//交货已完成标识 
            //gm_item.ITEM_TEXT = "";//项目文本 
            //gm_item.GR_RCPT = "";//收货方 
            //gm_item.UNLOAD_PT = "";//卸货点
            //gm_item.COSTCENTER = "";//成本中心
            //gm_item.VAL_TYPE = "";//评估类型
            //gm_item.CUSTOMER = "";//客户的账户编号
            //gm_item.SALES_ORD = "";//销售订单数
            //gm_item.S_ORD_ITEM = "";//销售订单中的项目编号
            #endregion
            z_po_post.GOODSMVT_ITEM[0] = gm_item;
            #endregion

            #endregion

            #region 初始化 RETURN
            z_po_post.RETURN = new SAPSERVICES_ZPZS3.BAPIRET2[1];
            SAPSERVICES_ZPZS3.BAPIRET2 bapiret = new SAPSERVICES_ZPZS3.BAPIRET2();

            //bapiret.ID = "";
            //bapiret.TYPE = "";
            //bapiret.NUMBER = "";
            //bapiret.MESSAGE = "";
            //bapiret.LOG_NO = "";
            //bapiret.LOG_MSG_NO = "";
            //bapiret.MESSAGE_V1 = "";
            //bapiret.MESSAGE_V2 = "";
            //bapiret.MESSAGE_V3 = "";
            //bapiret.MESSAGE_V4 = "";
            //bapiret.PARAMETER = "";
            //bapiret.ROW = "";
            //bapiret.FIELD = "";
            //bapiret.SYSTEM = "";
            z_po_post.RETURN[0] = bapiret;
            #endregion

            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_PO_POST1_OUT(z_po_post);
            if (resulte.RC == "0")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                string PRUEFLOS = resulte.PRUEFLOS;//检验批号
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                //物料凭证号(推送单据号) MATERIALDOCUMENT = "5003845836"
                String MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                string E_POSNR = resulte.E_POSNR;
                REDATA redata = new REDATA();
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;//凭证编号
                redata.PRUEFLOS = PRUEFLOS;
                redata.E_POSNR = E_POSNR;
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    ret.REMSG = string.Format("失败：{0}", resulte.RETURN[0].MESSAGE);
                }
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }


        /// <summary>
        /// 生产机：CO生产订单收货过账(区块链预约二次计量推送)
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：CO生产订单收货过账(区块链预约二次计量推送)")]
        public RETURN_SAP SAP_POST110ZS(EOS_Z_PO_POST entity, string strjsonw)
        {
            RETURN_SAP ret = new RETURN_SAP();
            #region 测试数据
            //EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            //entity.EOS_GM_CODE = "01";//01采购订单收货,02生产订单收货
            //entity.DOC_DATE = "2021-08-13";// DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PSTNG_DATE = "2021-08-13";//DateTime.Now.ToString("yyyy-MM-dd");
            //entity.BILL_OF_LADING = "";
            //entity.GR_GI_SLIP_NO = "";
            //entity.ORDERID = "1000070";//订单编号
            //entity.ORDER_ITNO = "3";//订单项目编号
            //entity.MATERIAL = "000000000003010003";//物料
            //entity.BATCH = "2021091303";//批次
            //entity.ENTRY_QNT = 2;//数量
            //entity.STGE_LOC = "Y001";//库存地点  
            //entity.PLANT = "1000";//工厂
            //entity.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            //entity.MVT_IND = "F";//移动标识,B,F,B是采购订单  F是生产订单
            //entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.EOS_GM_CODE))
            {
                entity.EOS_GM_CODE = "01";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(entity.PSTNG_DATE))
            {
                entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }

            #endregion

            #region 初始化接口
            SAPSERVICES_ZPZS1.SI_Z_PO_POST_OUTClient sap = new SAPSERVICES_ZPZS1.SI_Z_PO_POST_OUTClient("HTTPS_PORTPOSTZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];

            SAPSERVICES_ZPZS1.Z_PO_POST z_po_post = new SAPSERVICES_ZPZS1.Z_PO_POST();
            z_po_post.GOODSMVT_CODE = new SAPSERVICES_ZPZS1.BAPI2017_GM_CODE();
            z_po_post.GOODSMVT_CODE.GM_CODE = entity.EOS_GM_CODE;//01采购订单收货,02生产订单收货
            z_po_post.GOODSMVT_HEADER = new SAPSERVICES_ZPZS1.BAPI2017_GM_HEAD_01();
            z_po_post.GOODSMVT_HEADER.DOC_DATE = entity.DOC_DATE;//DateTime.Now.ToString("yyyy-MM-dd");//凭证中的凭证日期,当前系统日期
            z_po_post.GOODSMVT_HEADER.PSTNG_DATE = entity.PSTNG_DATE;// DateTime.Now.ToString("yyyy-MM-dd");//凭证中的过账日期-可人工修改重新传SAP,系统当前日期
            z_po_post.GOODSMVT_HEADER.BILL_OF_LADING = entity.BILL_OF_LADING;//收货时提单号
            z_po_post.GOODSMVT_HEADER.GR_GI_SLIP_NO = entity.GR_GI_SLIP_NO;//收货/发货单编号
            z_po_post.GOODSMVT_HEADER.HEADER_TXT = "无人计量采购过账";//凭证抬头文本
            #endregion

            #region GOODSMVT_HEADER
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO = "";//参考凭证编号
            //z_po_post.GOODSMVT_HEADER.PR_UNAME = "";//用户名 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIP = "";//收货/发货单打印版本 
            //z_po_post.GOODSMVT_HEADER.VER_GR_GI_SLIPX = "";//相关用户数据字段的已更新信息
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账
            //z_po_post.GOODSMVT_HEADER.REF_DOC_NO_LONG = "";//参考凭证编号(对相关性参看长文本)
            //z_po_post.GOODSMVT_HEADER.BILL_OF_LADING_LONG = "";//组的检配单编号(根据：参见长文本)
            //z_po_post.GOODSMVT_HEADER.BAR_CODE = "";//条形码输入
            //z_po_post.GOODSMVT_HEADER.EXT_WMS = "";//外部WMS 的控制过账 
            #endregion


            #region  初始化 ZMSEG_C
            z_po_post.ZMSEG_C = new SAPSERVICES_ZPZS1.ZMSEG_C();
            //z_po_post.ZMSEG_C.MANDT = "";//集团
            //z_po_post.ZMSEG_C.MBLNR = "";//物料凭证编号
            //z_po_post.ZMSEG_C.MJAHR = "";//物料凭证的年份
            //z_po_post.ZMSEG_C.LIFNR = "";//供应商或债券人的账户
            //z_po_post.ZMSEG_C.CHEHO = "";//车牌号
            //z_po_post.ZMSEG_C.XNHAO = "";//箱号
            //z_po_post.ZMSEG_C.CTNER = "";//柜型
            //z_po_post.ZMSEG_C.JKDAT = "";//进口日期
            //z_po_post.ZMSEG_C.LIFN1 = "";//供应商或债券人的账户
            //z_po_post.ZMSEG_C.NAME9 = "";//名称
            //z_po_post.ZMSEG_C.EBELN = "";//采购凭证编号
            //z_po_post.ZMSEG_C.MATNR = "";//物料编号
            //z_po_post.ZMSEG_C.MENGE = 0;//采购订单数量
            //z_po_post.ZMSEG_C.EINDT = "";//项目交货日期
            //z_po_post.ZMSEG_C.WERKS = "";//工厂
            //z_po_post.ZMSEG_C.NETPR = 0;//净价
            //z_po_post.ZMSEG_C.PQ = "";//纸质类型
            #endregion

            #region 初始化 GOODSMVT_ITEM
            z_po_post.GOODSMVT_ITEM = new SAPSERVICES_ZPZS1.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_ZPZS1.BAPI2017_GM_ITEM_CREATE gm_item = new SAPSERVICES_ZPZS1.BAPI2017_GM_ITEM_CREATE();
            gm_item.ORDERID = entity.ORDERID;//订单编号
            gm_item.ORDER_ITNO = entity.ORDER_ITNO;//订单项目编号
            gm_item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
            gm_item.MVT_IND = entity.MVT_IND;//移动标识,B,F,B是采购订单  F是生产订单
            gm_item.SPEC_STOCK = entity.SPEC_STOCK;//特殊库存标识,K寄售,空
            gm_item.STGE_LOC = entity.STGE_LOC;//库存地点
            gm_item.BATCH = entity.BATCH;//批次
            gm_item.ENTRY_QNT = entity.ENTRY_QNT;//数量
            gm_item.ENTRY_QNTSpecified = entity.ENTRY_QNTSpecified;
            gm_item.PLANT = entity.PLANT;//工厂
            gm_item.MATERIAL = entity.MATERIAL;//物料
            #endregion

            #region   GOODSMVT_ITEM

            // gm_item.STCK_TYPE = "2";//库存类型:化工：2质量检验 废纸：空 非限制使用 3已冻结
            ////单位
            //gm_item.ENTRY_UOM = "";
            ////计量单位ISO代码
            //gm_item.ENTRY_UOM_ISO = "";
            ////采购订单价格单位的数量
            //gm_item.PO_PR_QNT = 0;
            ////订单价格单位(采购)
            //gm_item.ORDERPR_UN = "";
            ////计量单位ISO代码
            //gm_item.ORDERPR_UN_ISO = "";
            //gm_item.PO_NUMBER = "4300004787";//采购订单号
            //gm_item.PO_ITEM = "10";//采购凭证的项目编号
            //gm_item.VENDOR = "106544";//供应商账户号

            //gm_item.SHIPPING = "";//装运指令
            //gm_item.COMP_SHIP = "";//依照装运通知
            //gm_item.NO_MORE_GR = "";//交货已完成标识 
            //gm_item.ITEM_TEXT = "";//项目文本 
            //gm_item.GR_RCPT = "";//收货方 
            //gm_item.UNLOAD_PT = "";//卸货点
            //gm_item.COSTCENTER = "";//成本中心
            //gm_item.VAL_TYPE = "";//评估类型
            //gm_item.CUSTOMER = "";//客户的账户编号
            //gm_item.SALES_ORD = "";//销售订单数
            //gm_item.S_ORD_ITEM = "";//销售订单中的项目编号
            //gm_item.SCHED_LINE = "";//销售订单交货计划
            z_po_post.GOODSMVT_ITEM[0] = gm_item;
            #endregion

            #region 初始化 RETURN
            z_po_post.RETURN = new SAPSERVICES_ZPZS1.BAPIRET2[1];
            SAPSERVICES_ZPZS1.BAPIRET2 bapiret = new SAPSERVICES_ZPZS1.BAPIRET2();
            //bapiret.ID = "";
            //bapiret.TYPE = "";
            //bapiret.NUMBER = "";
            //bapiret.MESSAGE = "";
            //bapiret.LOG_NO = "";
            //bapiret.LOG_MSG_NO = "";
            //bapiret.MESSAGE_V1 = "";
            //bapiret.MESSAGE_V2 = "";
            //bapiret.MESSAGE_V3 = "";
            //bapiret.MESSAGE_V4 = "";
            //bapiret.PARAMETER = "";
            //bapiret.ROW = "";
            //bapiret.FIELD = "";
            //bapiret.SYSTEM = "";
            z_po_post.RETURN[0] = bapiret;
            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_PO_POST_OUT(z_po_post);
            if (resulte.RC == "0")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                string E_POSNR = resulte.E_POSNR;
                string PRUEFLOS = resulte.PRUEFLOS;
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                //物料凭证号(推送单据号) MATERIALDOCUMENT = "5003845836"
                String MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                REDATA redata = new REDATA();
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;
                redata.PRUEFLOS = PRUEFLOS;
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    ret.REMSG = string.Format("失败：{0}", resulte.RETURN[0].MESSAGE);
                }
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }


        /// <summary>
        /// 生产机：国内废纸收货推送(二次计量调用)
        /// </summary>
        /// <param name="entity">参数对象</param>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：国内废纸收货推送(二次计量调用)")]
        public RETURN_SAP SAP_POSTZS2(EOS_Z_PO_POST2 entity, string strjsonw)
        {
            //返回对象
            RETURN_SAP ret = new RETURN_SAP();
            #region 测试数据
            //EOS_Z_PO_POST2 entity = new EOS_Z_PO_POST2();
            //entity.EOS_GM_CODE = "02";
            //entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            //entity.PO_NUMBER = "4700007833";//订单号
            //entity.PO_ITEM = "10";//订单行号
            //entity.MOVE_TYPE = "101";  //移动类型：废纸101,化工,包装物103
            //entity.MVT_IND = "B";//移动标识
            //entity.SPEC_STOCK = "";//特殊库存标识
            //entity.ENTRY_QNT = 1;//数量
            //entity.ENTRY_QNTSpecified = true;
            //entity.STGE_LOC = "Y490";//库存地点
            //entity.BATCH = "202107";//批次
            //entity.MATERIAL = "000000000003010064";//物料
            //entity.PLANT = "9000";//工厂
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.EOS_GM_CODE))
            {
                entity.EOS_GM_CODE = "02";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(entity.PSTNG_DATE))
            {
                entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }

            #endregion

            #region 初始化接口
            SAPSERVICES_ZPZS2.SI_Z_PO_POST2_OUTClient sap = new SAPSERVICES_ZPZS2.SI_Z_PO_POST2_OUTClient("HTTPS_PORTPOST2ZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_ZPZS2.Z_PO_POST2 z_po_post = new SAPSERVICES_ZPZS2.Z_PO_POST2();
            z_po_post.GOODSMVT_CODE = new SAPSERVICES_ZPZS2.BAPI2017_GM_CODE();
            z_po_post.GOODSMVT_CODE.GM_CODE = entity.EOS_GM_CODE;

            z_po_post.GOODSMVT_HEADER = new SAPSERVICES_ZPZS2.BAPI2017_GM_HEAD_01();
            z_po_post.GOODSMVT_HEADER.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");//凭证中的凭证日期
            z_po_post.GOODSMVT_HEADER.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");//凭证中的过账日期
            z_po_post.GOODSMVT_HEADER.HEADER_TXT = entity.HEADER_TXT;//文本描述
            z_po_post.GOODSMVT_HEADER.BILL_OF_LADING = entity.BILL_OF_LADING;//提单号
            z_po_post.GOODSMVT_HEADER.REF_DOC_NO = entity.BILL_OF_LADING;//提单号
            z_po_post.GOODSMVT_HEADER.GR_GI_SLIP_NO = entity.GR_GI_SLIP_NO;//提单号
            z_po_post.WEIGHT = entity.WEIGHT;//单包重
            z_po_post.PINP = entity.PINP;//品牌


            #endregion

            #region  初始化 GOODSMVT_ITEM
            z_po_post.GOODSMVT_ITEM = new SAPSERVICES_ZPZS2.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_ZPZS2.BAPI2017_GM_ITEM_CREATE gm_item = new SAPSERVICES_ZPZS2.BAPI2017_GM_ITEM_CREATE();
            gm_item.PO_NUMBER = entity.PO_NUMBER;//订单号
            gm_item.PO_ITEM = entity.PO_ITEM;//订单行号
            gm_item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型：废纸101,化工,包装物103
            gm_item.MVT_IND = entity.MVT_IND;//移动标识
            gm_item.SPEC_STOCK = entity.SPEC_STOCK;//特殊库存标识
            gm_item.ENTRY_QNT = entity.ENTRY_QNT;//数量
            gm_item.ENTRY_QNTSpecified = entity.ENTRY_QNTSpecified;
            gm_item.STGE_LOC = entity.STGE_LOC;//库存地点
            gm_item.BATCH = entity.BATCH;//批次
            gm_item.MATERIAL = entity.MATERIAL;//物料
            gm_item.PLANT = entity.PLANT;//工厂
            z_po_post.GOODSMVT_ITEM[0] = gm_item;
            #endregion

            #region RETURN
            z_po_post.RETURN = new SAPSERVICES_ZPZS2.BAPIRET2[1];
            SAPSERVICES_ZPZS2.BAPIRET2 bapiret = new SAPSERVICES_ZPZS2.BAPIRET2();
            z_po_post.RETURN[0] = bapiret;
            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_PO_POST2_OUT(z_po_post);

            if (resulte.RC == "0")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                REDATA redata = new REDATA();
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                //物料凭证号(推送单据号) MATERIALDOCUMENT = "5003845836"
                string MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;
                redata.PRUEFLOS = "";
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    ret.REMSG = string.Format("失败：{0}", resulte.RETURN[0].MESSAGE);
                }
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }


        /// <summary>
        /// 生产机： SAP化工-包装物收货返回检验批(二次计量后调用)
        /// </summary>
        /// <param name="EOS_ZFMM098">参数对象</param>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = " 生产机：SAP化工-包装物105收货+检验批号使用决策")]
        public RETURN_SAP SAP_98ZS(EOS_ZFMM098 entity, string strjson)
        {
            #region  测试数据
            //EOS_ZFMM098 entity = new EOS_ZFMM098();
            //entity.PRUEFLOS = "10000191350";
            //entity.LGORT = "WJ01";
            //entity.FXZKC = "8";
            //entity.THJH = "2";
            //entity.JYJG = "2";
            //entity.VCODE = "001";
            #endregion

            #region 初始化接口
            SAPSERVICES_98ZS.SI_ZFMM098_OUTClient sap = new SAPSERVICES_98ZS.SI_ZFMM098_OUTClient("HTTPS_PORT98ZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            #endregion

            #region 赋值
            SAPSERVICES_98ZS.ZFMM098 mm98 = new SAPSERVICES_98ZS.ZFMM098();
            if (!string.IsNullOrEmpty(entity.PRUEFLOS))
            {
                mm98.I_PRUEFLOS = entity.PRUEFLOS;// "10000191350";//检验批次编号
            }
            if (!string.IsNullOrEmpty(entity.LGORT))
            {
                mm98.I_LGORT = entity.LGORT;// "WJ01";//过账到未限制库存的数量的存储位置(库存地点)
            }
            if (!string.IsNullOrEmpty(entity.FXZKC))
            {
                mm98.I_FXZKC = entity.FXZKC;//将过账到非限制使用库存的数量(净重-扣重)
            }
            if (!string.IsNullOrEmpty(entity.THJH))
            {
                mm98.I_THJH = entity.THJH;//以退货到供应商形式过账的数量(皮+扣重)
            }
            if (!string.IsNullOrEmpty(entity.JYJG))
            {
                mm98.I_JYJG = entity.JYJG;//检验结果数(SAP提供获取检验结果数)
            }
            else
            {
                mm98.I_JYJG = "";
            }
            if (!string.IsNullOrEmpty(entity.VCODE))
            {
                mm98.I_VCODE = entity.VCODE;//使用决策代码(默认:001)
            }


            #endregion

            #region 执行接口



            var resulte = sap.SI_ZFMM098_OUT(mm98);
            RETURN_SAP ret = new RETURN_SAP();
            REDATA redata = new REDATA();
            redata.MATDOCUMENTYEAR = "";
            redata.MATERIALDOCUMENT = "";
            redata.PRUEFLOS = "";
            redata.E_MBLNR = "";
            redata.E_MJAHR = "";
            redata.E_POSNR = "";
            if (resulte.E_RETURN.TYPE == "S")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = string.Format("失败：{0}", resulte.E_RETURN.MESSAGE);
                ret.REDATA = redata;
            }
            #endregion

            return ret;
        }


        /// <summary>
        /// AJAX调用
        /// </summary>
        /// <param name="strjson"></param>
        /// <returns></returns>
        [WebMethod(Description = "AJAX生产机：SAP化工-包装物收货返回检验批(二次计量后调用)")]
        public RETURN_SAP SAP_98ZSAjax(string PRUEFLOS, string LGORT, string FXZKC, string THJH, string JYJG, string VCODE, string strjson)
        {
            EOS_ZFMM098 entity = new EOS_ZFMM098();
            entity.PRUEFLOS = PRUEFLOS;//检验批号
            entity.LGORT = LGORT;//库存地点
            entity.FXZKC = FXZKC; //(净重 - 扣重)
            entity.THJH = THJH;//(皮+扣重)
            entity.JYJG = JYJG;//检验结果数
            if (string.IsNullOrEmpty(VCODE))
            {
                VCODE = "001";
            }
            else
            {
                entity.VCODE = VCODE;
            }
            return SAP_98ZS(entity, "");
        }


        /// <summary>
        /// 生产机：CO生产订单收货过账(区块链预约二次计量推送)(不使用)
        /// </summary>
        /// <param name="strjson"></param>
        /// <returns></returns>
        [WebMethod(Description = " 生产机：CO生产订单收货过账(区块链预约二次计量推送)")]
        public RETURN_SAP SAP_110ZS(EOS_ZFMM110 entity, string strjson)
        {
            #region 测试数据
            //EOS_ZFMM110 entity = new EOS_ZFMM110();
            //entity.AUFNR = "1000070";//订单号
            //entity.CHARG = "2021091301";//批号
            //entity.LGORT = "Y001";//库存地点
            //entity.ERFMG = 10;//数量
            #endregion

            #region 初始化接口
            SAPSERVICES_110ZS.SI_ZFMM110_OUTClient sap = new SAPSERVICES_110ZS.SI_ZFMM110_OUTClient("HTTPS_PORT110ZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            #endregion

            #region 初始化 ZFMM110
            SAPSERVICES_110ZS.ZFMM110 mm110 = new SAPSERVICES_110ZS.ZFMM110();
            mm110.I_AUFNR = entity.AUFNR;// "1000070";//订单号
            mm110.I_CHARG = entity.CHARG;// "2021091301";//批号
            mm110.I_LGORT = entity.LGORT;// "Y001";//库存地点
            mm110.I_ERFMG = entity.ERFMG;// 10;//数量
            #endregion

            #region 执行接口
            var resulte = sap.SI_ZFMM110_OUT(mm110);
            RETURN_SAP ret = new RETURN_SAP();
            if (resulte.E_RETURN.TYPE == "S")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                REDATA redata = new REDATA();
                string E_MBLNR = resulte.E_MBLNR;
                string E_MJAHR = resulte.E_MJAHR;
                redata.E_MBLNR = E_MBLNR;
                redata.E_MJAHR = E_MJAHR;
                redata.PRUEFLOS = "";
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = string.Format("失败：{0}", resulte.E_RETURN.MESSAGE);
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }

        /// <summary>
        /// 生产机：化工、外包装物检验批、批号维护 
        /// </summary>
        /// <param name="strjson"></param>
        /// <returns></returns>
        [WebMethod(Description = " 生产机：化工、外包装物检验批、批号维护")]
        public RETURN_SAP SAP_115ZS(EOS_ZFMM115 entity, string strjson)
        {
            #region 测试数据
            //EOS_ZFMM115 entity = new EOS_ZFMM115();
            //entity.PRUEFLOS = "1000070";//订单号
            //entity.CHARG = "2021091301";//批号

            #endregion

            #region 初始化接口
            SAPSERVICES_115ZS.SI_ZFMM115_OUTClient sap = new SAPSERVICES_115ZS.SI_ZFMM115_OUTClient("HTTPS_PORT115ZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            #endregion

            #region 初始化 ZFMM110
            //I_PRUEFLOS，检验批号。I_CHARG，批次号
            SAPSERVICES_115ZS.ZFMM115 mm115 = new SAPSERVICES_115ZS.ZFMM115();
            mm115.I_PRUEFLOS = entity.PRUEFLOS; //检验批号
            mm115.I_CHARG = entity.CHARG; //批次号

            #endregion

            #region 执行接口
            var resulte = sap.SI_ZFMM115_OUT(mm115);
            RETURN_SAP ret = new RETURN_SAP();
            if (resulte.E_RETURN.TYPE == "S")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                REDATA redata = new REDATA();
                redata.E_MBLNR = "";
                redata.E_MJAHR = "";
                redata.PRUEFLOS = "";
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = string.Format("失败：{0}", resulte.E_RETURN.MESSAGE);
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }

        /// <summary>
        /// 生产机：货物移库
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：货物移库")]
        public RETURN_SAP SAP_GMPOSTZS(EOS_Z_GM_POST entity, string strjsonw)
        {
            #region 测试参数

            //EOS_Z_GM_POST entity = new EOS_Z_GM_POST();
            //entity.GM_CODE = "04";
            //entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            //entity.MOVE_TYPE = "311";//移动类型
            //entity.ENTRY_UOM = "KG";//计量单位
            //entity.MATERIAL = "000000000003010064";
            //entity.BATCH = "2021091302";//原批次
            //entity.MOVE_BATCH = "2021091302";//现批次
            //entity.ENTRY_QNT = 1;
            //entity.ENTRY_QNTSpecified = true;
            //entity.STGE_LOC = "Y490";//发货仓库
            //entity.MOVE_STLOC = "Y502";// "Y502";//收货仓库
            //entity.PLANT = "9000";//原工厂
            //entity.MOVE_PLANT = "9000";//收货工厂 
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.GM_CODE))
            {
                entity.GM_CODE = "04";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }
            entity.ENTRY_QNTSpecified = true;

            #endregion

            #region 验证
            RETURN_SAP ret = new RETURN_SAP();
            if (entity.STGE_LOC == entity.MOVE_STLOC)
            {
                ret.STATUS = "E";
                ret.REMSG = "发货仓库和收货仓库不能相同！";
                ret.REDATA = null;
                return ret;
            }
            #endregion

            #region 初始化接口
            SAPSERVICES_HWYKZS.SI_Z_GM_POST_OUTClient sap = new SAPSERVICES_HWYKZS.SI_Z_GM_POST_OUTClient("HTTPS_PORTGMPOSTZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_HWYKZS.Z_GM_POST z_gm_post = new SAPSERVICES_HWYKZS.Z_GM_POST();
            z_gm_post.GOODSMVT_CODE = new SAPSERVICES_HWYKZS.BAPI2017_GM_CODE();
            z_gm_post.GOODSMVT_CODE.GM_CODE = entity.GM_CODE;//04
            z_gm_post.GOODSMVT_HEADER = new SAPSERVICES_HWYKZS.BAPI2017_GM_HEAD_01();
            z_gm_post.GOODSMVT_HEADER.DOC_DATE = entity.DOC_DATE;// DateTime.Now.ToString("yyyy-MM-dd");
            z_gm_post.GOODSMVT_HEADER.HEADER_TXT = entity.HEADER_TXT;//凭证文本
            #endregion

            #region 初始化 GOODSMVT_ITEM
            z_gm_post.GOODSMVT_ITEM = new SAPSERVICES_HWYKZS.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_HWYKZS.BAPI2017_GM_ITEM_CREATE item = new SAPSERVICES_HWYKZS.BAPI2017_GM_ITEM_CREATE();
            item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型
            item.ENTRY_UOM = entity.ENTRY_UOM;//计量单位
            item.MATERIAL = entity.MATERIAL;
            item.BATCH = entity.BATCH;//原批次
            item.MOVE_BATCH = entity.MOVE_BATCH;//现批次
            item.ENTRY_QNT = entity.ENTRY_QNT;
            item.ENTRY_QNTSpecified = entity.ENTRY_QNTSpecified;
            item.STGE_LOC = entity.STGE_LOC;//发货仓库
            item.MOVE_STLOC = entity.MOVE_STLOC;// "Y502";//收货仓库
            item.PLANT = entity.PLANT;//原工厂
            item.MOVE_PLANT = entity.MOVE_PLANT;//收货工厂 
            z_gm_post.GOODSMVT_ITEM[0] = item;
            #endregion

            #region  初始化 RETURN
            z_gm_post.RETURN = new SAPSERVICES_HWYKZS.BAPIRET2[1];
            SAPSERVICES_HWYKZS.BAPIRET2 bapiret2 = new SAPSERVICES_HWYKZS.BAPIRET2();
            z_gm_post.RETURN[0] = bapiret2;
            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_GM_POST_OUT(z_gm_post);
            if (resulte.RC == "0" && resulte.MATDOCUMENTYEAR != "0000")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                REDATA redata = new REDATA();
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                string MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;
                redata.PRUEFLOS = "";
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    string REMSG = "";
                    foreach (SAPSERVICES_HWYKZS.BAPIRET2 pi in resulte.RETURN)
                    {
                        REMSG += pi.MESSAGE;
                    }
                    ret.REMSG = string.Format("失败：{0}", REMSG);
                    ret.REDATA = null;
                }
            }
            #endregion
            return ret;
        }
        /// <summary>
        /// 生产机：新货物移库
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：新货物移库")]
        public RETURN_SAP SAP_GMPOST4ZS(EOS_Z_GM_POST entity, string strjsonw)
        {
            REDATA redata = new REDATA();

            #region 测试参数

            //EOS_Z_GM_POST entity = new EOS_Z_GM_POST();
            //entity.GM_CODE = "04";
            //entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            //entity.MOVE_TYPE = "311";//移动类型
            //entity.ENTRY_UOM = "KG";//计量单位
            //entity.MATERIAL = "000000000003010064";
            //entity.BATCH = "2021091302";//原批次
            //entity.MOVE_BATCH = "2021091302";//现批次
            //entity.ENTRY_QNT = 1;
            //entity.ENTRY_QNTSpecified = true;
            //entity.STGE_LOC = "Y490";//发货仓库
            //entity.MOVE_STLOC = "Y502";// "Y502";//收货仓库
            //entity.PLANT = "9000";//原工厂
            //entity.MOVE_PLANT = "9000";//收货工厂 
            #endregion

            #region 初始化必填固定值
            if (string.IsNullOrEmpty(entity.GM_CODE))
            {
                entity.GM_CODE = "04";
            }
            if (string.IsNullOrEmpty(entity.DOC_DATE))
            {
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (entity.ENTRY_QNTSpecified == false)
            {
                entity.ENTRY_QNTSpecified = true;
            }
            entity.ENTRY_QNTSpecified = true;

            #endregion

            #region 验证
            RETURN_SAP ret = new RETURN_SAP();
            if (entity.STGE_LOC == entity.MOVE_STLOC)
            {
                ret.STATUS = "S";
                ret.REMSG = "发货仓库和收货仓库不能相同！";
                redata.MATDOCUMENTYEAR = "";
                redata.MATERIALDOCUMENT = "";
                redata.PRUEFLOS = "";
                ret.REDATA = redata;
                return ret;
            }
            #endregion

            #region 初始化接口
            SAPSERVICES_HWYKSC.SI_Z_GM_POST4_OUTClient sap = new SAPSERVICES_HWYKSC.SI_Z_GM_POST4_OUTClient("HTTPS_PORTGMPOST4ZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_HWYKSC.Z_GM_POST4 z_gm_post = new SAPSERVICES_HWYKSC.Z_GM_POST4();
            z_gm_post.GOODSMVT_CODE = new SAPSERVICES_HWYKSC.BAPI2017_GM_CODE();
            z_gm_post.GOODSMVT_CODE.GM_CODE = entity.GM_CODE;//04
            z_gm_post.GOODSMVT_HEADER = new SAPSERVICES_HWYKSC.BAPI2017_GM_HEAD_01();
            z_gm_post.GOODSMVT_HEADER.DOC_DATE = entity.DOC_DATE;// DateTime.Now.ToString("yyyy-MM-dd");
            z_gm_post.GOODSMVT_HEADER.PSTNG_DATE = entity.DOC_DATE;
            z_gm_post.GOODSMVT_HEADER.HEADER_TXT = entity.HEADER_TXT;// "生产上料移库";
            #endregion

            #region 初始化 GOODSMVT_ITEM
            z_gm_post.GOODSMVT_ITEM = new SAPSERVICES_HWYKSC.BAPI2017_GM_ITEM_CREATE[1];
            SAPSERVICES_HWYKSC.BAPI2017_GM_ITEM_CREATE item = new SAPSERVICES_HWYKSC.BAPI2017_GM_ITEM_CREATE();
            item.MOVE_TYPE = entity.MOVE_TYPE;//移动类型
            item.ENTRY_UOM = entity.ENTRY_UOM;//计量单位
            if (entity.MATERIAL.Length >= 18)
            {
                item.MATERIAL = entity.MATERIAL.Replace("00000000000", "");
            }
            else
            {
                item.MATERIAL = entity.MATERIAL;
            }

            item.BATCH = entity.BATCH;//原批次
            item.MOVE_BATCH = entity.MOVE_BATCH;//现批次
            item.ENTRY_QNT = entity.ENTRY_QNT;
            item.ENTRY_QNTSpecified = entity.ENTRY_QNTSpecified;
            item.STGE_LOC = entity.STGE_LOC;//发货仓库
            item.MOVE_STLOC = entity.MOVE_STLOC;// "Y502";//收货仓库
            item.PLANT = entity.PLANT;//原工厂
            item.MOVE_PLANT = entity.MOVE_PLANT;//收货工厂 
            z_gm_post.GOODSMVT_ITEM[0] = item;
            #endregion

            #region  初始化 RETURN
            z_gm_post.RETURN = new SAPSERVICES_HWYKSC.BAPIRET2[1];
            SAPSERVICES_HWYKSC.BAPIRET2 bapiret2 = new SAPSERVICES_HWYKSC.BAPIRET2();
            z_gm_post.RETURN[0] = bapiret2;
            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_GM_POST_OUT(z_gm_post);
            if (resulte.RC == "0" && resulte.MATDOCUMENTYEAR != "0000")
            {
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                ///
                string MATDOCUMENTYEAR = resulte.MATDOCUMENTYEAR;
                string MATERIALDOCUMENT = resulte.MATERIALDOCUMENT;
                redata.MATDOCUMENTYEAR = MATDOCUMENTYEAR;
                redata.MATERIALDOCUMENT = MATERIALDOCUMENT;
                redata.PRUEFLOS = "";
                ret.REDATA = redata;
            }
            else
            {
                ret.STATUS = "E";
                if (resulte.RETURN.Length > 0)
                {
                    string REMSG = "";
                    foreach (SAPSERVICES_HWYKSC.BAPIRET2 pi in resulte.RETURN)
                    {
                        REMSG += pi.MESSAGE;
                    }
                    ret.REMSG = string.Format("失败：{0}", REMSG);
                    ret.REDATA = null;
                }
            }
            #endregion
            return ret;
        }



        [WebMethod(Description = "生产机：货物移库")]
        public RETURN_SAP SAP_GMPOST4ZS1(string GM_CODE, string DOC_DATE, string MOVE_TYPE, string ENTRY_UOM, string MATERIAL, string BATCH, string MOVE_BATCH, decimal ENTRY_QNT, string STGE_LOC, string MOVE_STLOC, string PLANT, string MOVE_PLANT, string strjsonw)
        {
            #region 参数
            EOS_Z_GM_POST entity = new EOS_Z_GM_POST();
            entity.GM_CODE = GM_CODE;// "04";
            entity.DOC_DATE = DOC_DATE;// DateTime.Now.ToString("yyyy-MM-dd");
            entity.MOVE_TYPE = MOVE_TYPE;// "311";//移动类型
            entity.ENTRY_UOM = ENTRY_UOM;// "KG";//计量单位
            entity.MATERIAL = MATERIAL;// "000000000003010064";
            entity.BATCH = BATCH;// "2021091302";//原批次
            entity.MOVE_BATCH = MOVE_BATCH;// "2021091302";//现批次
            entity.ENTRY_QNT = ENTRY_QNT;// 1;
            entity.ENTRY_QNTSpecified = true;
            entity.STGE_LOC = STGE_LOC;// "Y490";//发货仓库
            entity.MOVE_STLOC = MOVE_STLOC;// "Y502";// "Y502";//收货仓库
            entity.PLANT = PLANT;// "9000";//原工厂
            entity.MOVE_PLANT = MOVE_PLANT;// "9000";//收货工厂 
            #endregion
            return SAP_GMPOST4ZS(entity, strjsonw);
        }

        #endregion

        #region 质检业务
        /// <summary>
        /// 生产机：AJAX质检检验计划(一次计量收货成功返回检验批号，凭证号推送)
        /// </summary>
        /// <param name="WERKS">工厂</param>
        /// <param name="EBELN">采购订单号</param>
        /// <param name="ITEM_LINE">订单行项目</param>
        /// <param name="PRUEFLOS">检验批次号</param>
        /// <param name="MATNR">物料号</param>
        /// <param name="POUND_ORDER">磅单号</param>
        /// <param name="CHARG">批次号</param>
        /// <param name="GROSS_WEIGHT">毛重</param>
        /// <param name="STOCK_LOCATION">库存地点</param>
        /// <param name="DELIVERY_DATE">来货日期</param>
        /// <param name="BUYER">采购员</param>
        /// <param name="ACCEPTANCE">验收人员</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：AJAX质检检验计划(一次计量收货成功返回检验批号，凭证号推送)")]
        public RETURN_SAP MES_PURCHASEZS1(string WERKS, string EBELN, string ITEM_LINE, string PRUEFLOS, string MATNR, string MATNRNAME, string POUND_ORDER, string CHARG, double GROSS_WEIGHT, string STOCK_LOCATION, string DELIVERY_DATE, string BUYER, string ACCEPTANCE, string SUPPLY, string SUPPLYNAME, string WGTYPE, string ISZJ, string MATERIALTYPE, string PZBILLNO, string TEL, string PK_STORE)
        {

            EOS_ZTTQM001 entity = new EOS_ZTTQM001();
            #region 测试数据
            entity.WERKS = WERKS;//工厂
            entity.EBELN = EBELN; //采购订单号
            entity.ITEM_LINE = ITEM_LINE;// 订单行项目  
            entity.PRUEFLOS = PRUEFLOS;//检验批次号
            entity.MATNR = MATNR;//物料号
            entity.MATNRNAME = MATNRNAME;//物料名称
            entity.MATERIALTYPE = MATERIALTYPE;
            entity.SUPPLY = SUPPLY;//供应商
            entity.SUPPLYNAME = SUPPLYNAME;//供应商名称

            entity.POUND_ORDER = POUND_ORDER; //磅单号
            entity.CHARG = CHARG;//批次号10位的批次号,格式：yymmdd0000
            entity.GROSS_WEIGHT = GROSS_WEIGHT;//毛重/到货数量
            entity.PACKAGES = "";//包装件数
            //entity.STOCK_LOCATION = STOCK_LOCATION;//库存地点  // 此字段为空弃置不用  新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
            entity.DELIVERY_DATE = DELIVERY_DATE;//来货日期
            entity.BUYER = BUYER;//采购员
            entity.ACCEPTANCE = ACCEPTANCE;//验收人员
            entity.LONG_MATERIAL = "1";//是否属于检验时间较长物料，是（0）/否（1）
            entity.INSPECTION_TYPE = "JYLX40";//检验类型
            entity.TASK_STATUS = "NEW";//任务状态
            entity.TASK_TYPE = "RWLX04";//任务类型
            entity.CREATED_DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//创建时间
            entity.CREATED_USER = "无人值守质检";//创建人
            entity.WGTYPE = WGTYPE;// 外观
            entity.ISZJ = ISZJ;// 质检报告
            entity.PZBILLNO = PZBILLNO;
            // 新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
            entity.MAKTX = TEL;
            entity.STOCK_LOCATION = PK_STORE;
            // 新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
            #endregion
            return MES_PURCHASEZS(entity, "");
        }

        /// <summary>
        /// 生产机：质检检验计划
        /// </summary>
        /// <param name="strjsonw"></param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：质检检验计划")]
        public RETURN_SAP MES_PURCHASEZS(EOS_ZTTQM001 entity, string strjsonw)
        {
            RETURN_SAP ret = new RETURN_SAP();

            #region 测试数据
            //EOS_ZTTQM001 entity = new EOS_ZTTQM001();
            //entity.WERKS = "7000";//工厂
            //entity.EBELN = "4100083819"; //采购订单号
            //entity.ITEM_LINE = "10";// 订单行项目  
            //entity.PRUEFLOS = "10000208388";//检验批次号
            //entity.MATNR = "4030022";//物料号
            //entity.POUND_ORDER = "CG21080700098"; //磅单号
            //entity.CHARG = "2106290001";//批次号10位的批次号,格式：yymmdd0000
            //entity.GROSS_WEIGHT = 22;//毛重/到货数量
            //entity.PACKAGES = "";//包装件数
            //entity.STOCK_LOCATION = "XB06";//库存地点
            //entity.DELIVERY_DATE = "2021-06-29";//来货日期
            //entity.BUYER = "秦国强";//采购员
            //entity.ACCEPTANCE = "刘丽娟";//验收人员
            //entity.LONG_MATERIAL = "1";//是否属于检验时间较长物料，是（0）/否（1）
            //entity.INSPECTION_TYPE = "JYLX40";//检验类型
            //entity.TASK_STATUS = "NEW";//任务状态
            //entity.TASK_TYPE = "RWLX04";//任务类型
            //entity.CREATED_DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//创建时间
            //entity.CREATED_USER = "无人值守质检";//创建人
            #endregion

            #region  初始化固定值
            if (string.IsNullOrEmpty(entity.CREATED_USER))
            {
                entity.CREATED_USER = "无人值守质检";//创建人 
            }
            if (string.IsNullOrEmpty(entity.CREATED_DATE_TIME))
            {
                entity.CREATED_DATE_TIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//创建时间
            }
            if (string.IsNullOrEmpty(entity.TASK_TYPE))
            {
                entity.TASK_TYPE = "RWLX04";//任务类型
            }
            if (string.IsNullOrEmpty(entity.TASK_STATUS))
            {
                entity.TASK_STATUS = "NEW";//任务状态
            }
            //liuyan/myt 220822 手持机外观判定和检测报告若没选择则默认为合格状态
            if (string.IsNullOrEmpty(entity.WGTYPE))
            {
                entity.WGTYPE = "外观合格";//外观判定
            }
            if (string.IsNullOrEmpty(entity.ISZJ))
            {
                entity.ISZJ = "有，字迹清晰";//检测报告
            }
            //liuyan/myt 220822
            #endregion

            #region 初始化接口
            SAPSERVICES_PURCHASEZS.SI_Z_ME_OAPURCHASEClient sap = new SAPSERVICES_PURCHASEZS.SI_Z_ME_OAPURCHASEClient("HTTPS_PORTPURCHASEZS");
            sap.ClientCredentials.UserName.UserName = ConfigurationManager.AppSettings["SAPUser"]; ;
            sap.ClientCredentials.UserName.Password = ConfigurationManager.AppSettings["SAPPwd"];
            SAPSERVICES_PURCHASEZS.Z_ME_OAPURCHASE z_me_purchase = new SAPSERVICES_PURCHASEZS.Z_ME_OAPURCHASE();

            #region 初始化 ZTTQM001
            z_me_purchase.T_ZTTQM001 = new SAPSERVICES_PURCHASEZS.ZTTQM001[1];
            SAPSERVICES_PURCHASEZS.ZTTQM001 zttqm001 = new SAPSERVICES_PURCHASEZS.ZTTQM001();
            #region 不可为空值
            zttqm001.PRUEFLOS = entity.PRUEFLOS;//检验批次号
            zttqm001.EBELN = entity.EBELN; //采购订单号
            zttqm001.ITEM_LINE = entity.ITEM_LINE;// 订单行项目
            zttqm001.MATNR = entity.MATNR;//物料号
            zttqm001.POUND_ORDER = entity.POUND_ORDER; //磅单号
            zttqm001.CHARG = entity.CHARG;//批次号10位的批次号,格式：yymmdd0000
            zttqm001.GROSS_WEIGHT = entity.GROSS_WEIGHT;//毛重/到货数量
            zttqm001.GROSS_WEIGHTSpecified = true;
            zttqm001.PACKAGES = entity.PACKAGES;//包装件数
            zttqm001.STOCK_LOCATION = entity.STOCK_LOCATION;//库存地点    // 新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
            zttqm001.WERKS = entity.WERKS;//工厂
            zttqm001.DELIVERY_DATE = entity.DELIVERY_DATE;//来货日期
            zttqm001.BUYER = entity.BUYER;//采购员
            zttqm001.ACCEPTANCE = entity.ACCEPTANCE;//验收人员
            zttqm001.LONG_MATERIAL = entity.LONG_MATERIAL;//是否属于检验时间较长物料，是（0）/否（1）
            zttqm001.INSPECTION_TYPE = entity.INSPECTION_TYPE;//检验类型
            zttqm001.TASK_STATUS = entity.TASK_STATUS;//任务状态
            zttqm001.TASK_TYPE = entity.TASK_TYPE;//任务类型
            zttqm001.CREATED_DATE_TIME = entity.CREATED_DATE_TIME;
            zttqm001.CREATED_USER = entity.CREATED_USER;

            zttqm001.MAKTX = entity.MAKTX; //司机电话 此字段在zttqm001表中空置，故用来存储电话号  // 新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009

            #endregion

            #region  可为空值
            zttqm001.CLOSE_REASON = "";
            zttqm001.CONFIRMED_DATE_TIME = "";// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            zttqm001.CONFIRMED_USER = "";
            zttqm001.EBELP = "";
            zttqm001.EXPLANATION = "";
            zttqm001.FLOW_CODE = "";
            zttqm001.INSPECTOR = "";
            zttqm001.INSPECT_END_TIME = "";
            zttqm001.INSPECT_START_TIME = "";
            zttqm001.INSPSAMPLE = "";
            //zttqm001.MAKTX = "";   // 新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
            zttqm001.MANDT = "";
            zttqm001.MBLNR = "";
            zttqm001.MESSAGE = "";
            zttqm001.MODIFIED_DATE_TIME = "";
            zttqm001.MODIFIED_USER = "";
            zttqm001.PROCESS_JUDGE = "";
            zttqm001.RESULT_JUDGE = "";
            zttqm001.STATUS = "";
            zttqm001.SYNCHRONOUS = "";
            #endregion
            z_me_purchase.T_ZTTQM001[0] = zttqm001;
            #endregion

            #region 初始化 ZTTQM002
            bool isTrue = false;
            EOS_OAWEIGHT oa_weight = new EOS_OAWEIGHT();//OA
            if (!string.IsNullOrEmpty(entity.WGTYPE) && !string.IsNullOrEmpty(entity.ISZJ))
            {
                if (entity.WGTYPE == "外观合格" && entity.ISZJ == "有，字迹清晰")
                {
                    isTrue = true;
                }
                #region 第一个参数
                z_me_purchase.T_ZTTQM002 = new SAPSERVICES_PURCHASEZS.ZTTQM002[2];
                SAPSERVICES_PURCHASEZS.ZTTQM002 zttqm002 = new SAPSERVICES_PURCHASEZS.ZTTQM002();
                zttqm002.WERKS = entity.WERKS;
                zttqm002.PRUEFLOS = entity.PRUEFLOS;
                zttqm002.VERWMERKM = "QM0087";
                zttqm002.TASK_RESULT = entity.WGTYPE;
                zttqm002.RESULT_JUDGE = entity.WGTYPE == "外观合格" ? "PASS" : "FAIL";
                zttqm002.TASK_STATUS = isTrue ? "NEW" : "DONE";
                zttqm002.INSPECT_START_TIME = DateTime.Now.ToString("yyyy-MM-dd");
                zttqm002.INSPECTOR = "质检平台";
                zttqm002.CREATED_DATE_TIME = entity.CREATED_DATE_TIME;
                zttqm002.CREATED_USER = entity.CREATED_USER;
                zttqm002.INSPSAMPLE = "000001";//liuyan /myt 220819 "000000"改为"000001"
                zttqm002.ZMAX = 0;
                zttqm002.ZMIN = 0;
                zttqm002.CHARG = entity.CHARG;//liuyan /myt 220819 新增批次号传输
                z_me_purchase.T_ZTTQM002[0] = zttqm002;
                #endregion

                #region 第二个参数
                SAPSERVICES_PURCHASEZS.ZTTQM002 zttqm002_1 = new SAPSERVICES_PURCHASEZS.ZTTQM002();
                zttqm002_1.WERKS = entity.WERKS;
                zttqm002_1.PRUEFLOS = entity.PRUEFLOS;
                zttqm002_1.VERWMERKM = "QM0086";
                zttqm002_1.TASK_RESULT = entity.ISZJ;
                zttqm002_1.RESULT_JUDGE = entity.ISZJ == "有，字迹清晰" ? "PASS" : "FAIL";
                zttqm002_1.TASK_STATUS = isTrue ? "NEW" : "DONE";
                zttqm002_1.INSPECT_START_TIME = DateTime.Now.ToString("yyyy-MM-dd");
                zttqm002_1.INSPECTOR = "质检平台";
                zttqm002_1.CREATED_DATE_TIME = entity.CREATED_DATE_TIME;
                zttqm002_1.CREATED_USER = entity.CREATED_USER;
                zttqm002_1.INSPSAMPLE = "000001";//liuyan /myt 220819 "000000"改为"000001"
                zttqm002_1.ZMAX = 0;
                zttqm002_1.ZMIN = 0;
                zttqm002_1.CHARG = entity.CHARG;//liuyan /myt 220819 新增批次号传输
                z_me_purchase.T_ZTTQM002[1] = zttqm002_1;
                #endregion
                // liuyan/myt 220914 新增逻辑 若明细行为DONE则将主表状态也改为DONE
                if(zttqm002_1.TASK_STATUS == "DONE" && zttqm002.TASK_STATUS == "DONE")
                {
                    z_me_purchase.T_ZTTQM001[0].TASK_STATUS = "DONE";
                }
                // liuyan/myt 220914 新增逻辑 若明细行为DONE则将主表状态也改为DONE
                if (isTrue == false)
                {
                    #region OA赋值
                    oa_weight.title = $"{entity.MATNRNAME}(退货)";//自定义标题(物料描述+验收结果（退货）)
                    //物料类别(化工辅料（0）/ 包装物（检验批）（2）)
                    if (entity.MATERIALTYPE == "YG40")
                    {
                        oa_weight.wllb = "0";
                    }
                    else
                    {
                        oa_weight.wllb = "2";
                    }
                    oa_weight.cgddh1 = entity.EBELN;// 采购订单号
                    oa_weight.jcbh = entity.POUND_ORDER;//进场编号（磅单号）
                    oa_weight.sfsyjysjjcwl = "1";//是否属于检验时间较长物料(是 / 否)
                    oa_weight.cgddh2mx = entity.EBELN;// 采购订单号
                    oa_weight.hxm2mx = entity.ITEM_LINE;//行项目
                    oa_weight.wlh2mx = entity.MATNR;// 物料号
                    oa_weight.wlms2mx = entity.MATNRNAME;//    物料描述
                    oa_weight.dw2mx = "吨";//     单位
                    oa_weight.bzjsmx = entity.PACKAGES;//    包装件数(没有时默认传空)
                    oa_weight.gc = entity.WERKS;//     工厂
                    oa_weight.dhsl = $"{entity.GROSS_WEIGHT}";//     毛重 / 到货数量
                    oa_weight.gysbh2mx = entity.SUPPLY;//  供应商编号
                    oa_weight.gysmc2mx = entity.SUPPLYNAME;//   供应商名称
                    oa_weight.wlpzh2mx = entity.PZBILLNO;// 物料凭证号(103入库产生的物料凭证)
                    oa_weight.jypmxT = entity.PRUEFLOS;//检验批
                    oa_weight.pc2mx = entity.CHARG;//批次
                    #endregion
                }
            }
            else
            {
                isTrue = true;
                if (entity.WGTYPE != "外观合格" || entity.ISZJ != "有，字迹清晰")
                {
                    isTrue = false;
                }
                z_me_purchase.T_ZTTQM002 = new SAPSERVICES_PURCHASEZS.ZTTQM002[1];
                SAPSERVICES_PURCHASEZS.ZTTQM002 zttqm002 = new SAPSERVICES_PURCHASEZS.ZTTQM002();
                if (!string.IsNullOrEmpty(entity.WGTYPE))
                {
                    #region 外观
                    zttqm002.WERKS = entity.WERKS;
                    zttqm002.PRUEFLOS = entity.PRUEFLOS;
                    zttqm002.VERWMERKM = "QM0087";
                    zttqm002.TASK_RESULT = entity.WGTYPE;
                    zttqm002.TASK_STATUS = "DONE";
                    zttqm002.RESULT_JUDGE = entity.WGTYPE == "外观合格" ? "PASS" : "FAIL";
                    zttqm002.INSPECT_START_TIME = DateTime.Now.ToString("yyyy-MM-dd");
                    zttqm002.INSPECTOR = "质检平台";
                    zttqm002.CREATED_DATE_TIME = entity.CREATED_DATE_TIME;
                    zttqm002.CREATED_USER = entity.CREATED_USER;
                    zttqm002.INSPSAMPLE = "000000";
                    zttqm002.ZMAX = 0;
                    zttqm002.ZMIN = 0;
                    #endregion
                }
                else if (!string.IsNullOrEmpty(entity.ISZJ))
                {
                    if (entity.ISZJ != "有，字迹清晰")
                    {
                        #region 是否质检
                        zttqm002.WERKS = entity.WERKS;
                        zttqm002.PRUEFLOS = entity.PRUEFLOS;
                        zttqm002.VERWMERKM = "QM0086";
                        zttqm002.TASK_RESULT = entity.ISZJ;
                        zttqm002.RESULT_JUDGE = entity.WGTYPE == "有，字迹清晰" ? "PASS" : "FAIL";
                        zttqm002.TASK_STATUS = "DONE";
                        zttqm002.INSPECT_START_TIME = DateTime.Now.ToString("yyyy-MM-dd");
                        zttqm002.INSPECTOR = "质检平台";
                        zttqm002.CREATED_DATE_TIME = entity.CREATED_DATE_TIME;
                        zttqm002.CREATED_USER = entity.CREATED_USER;
                        zttqm002.INSPSAMPLE = "000000";
                        zttqm002.ZMAX = 0;
                        zttqm002.ZMIN = 0;
                        #endregion
                    }
                }
                z_me_purchase.T_ZTTQM002[0] = zttqm002;
                // liuyan/myt 220914 新增逻辑 若明细行为DONE则将主表状态也改为DONE
                z_me_purchase.T_ZTTQM001[0].TASK_STATUS = "DONE";                
                // liuyan/myt 220914 新增逻辑 若明细行为DONE则将主表状态也改为DONE
                if (isTrue == false)
                {
                    #region OA赋值
                    oa_weight.title = $"{entity.MATNRNAME}(退货)";//自定义标题(物料描述+验收结果（退货）)
                    //物料类别(化工辅料（0）/包装物（检验批）（2）)
                    if (entity.MATERIALTYPE == "YG40")
                    {
                        oa_weight.wllb = "0";
                    }
                    else
                    {
                        oa_weight.wllb = "2";
                    }
                    oa_weight.cgddh1 = entity.EBELN;// 采购订单号
                    oa_weight.jcbh = entity.POUND_ORDER;//进场编号（磅单号）
                    oa_weight.sfsyjysjjcwl = "1";//是否属于检验时间较长物料(是 / 否)
                    oa_weight.cgddh2mx = entity.EBELN;// 采购订单号
                    oa_weight.hxm2mx = entity.ITEM_LINE;//行项目
                    oa_weight.wlh2mx = entity.MATNR;// 物料号
                    oa_weight.wlms2mx = entity.MATNRNAME;//    物料描述
                    oa_weight.dw2mx = "吨";//     单位
                    oa_weight.bzjsmx = entity.PACKAGES;//    包装件数(没有时默认传空)
                    oa_weight.gc = entity.WERKS;//     工厂
                    oa_weight.dhsl = $"{entity.GROSS_WEIGHT}";//     毛重 / 到货数量
                    oa_weight.gysbh2mx = entity.SUPPLY;//  供应商编号
                    oa_weight.gysmc2mx = entity.SUPPLYNAME;//   供应商名称
                    oa_weight.wlpzh2mx = entity.PZBILLNO;// 物料凭证号(103入库产生的物料凭证)
                    oa_weight.jypmxT = entity.PRUEFLOS;//检验批
                    oa_weight.pc2mx = entity.CHARG;//批次
                    #endregion
                }
            }
            if (isTrue == false)
            {
                //OA退货流程
                RETURN_OA ret_oa = WRZS_OAWEIGHTSELECTZS(oa_weight);
            }


            #endregion
            #endregion

            #region 执行接口
            var resulte = sap.SI_Z_ME_OAPURCHASE(z_me_purchase);
            if (resulte.E_CODE == "S")
            {
                log.Error($"质检计划:{entity.PRUEFLOS}执行成功");
                ret.STATUS = "S";
                ret.REMSG = "成功！";
                ret.REDATA = null;
            }
            else
            {
                log.Error($"质检计划:{entity.PRUEFLOS}执行失败！");
                ret.STATUS = "E";
                string E_MESSAGE = resulte.E_MESSAGE;
                ret.REMSG = string.Format("失败：{0}", E_MESSAGE);
                ret.REDATA = null;
            }
            #endregion
            return ret;
        }


        /// <summary>
        /// AJAX生产机：生产机上料调拨上传MES
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：上料调拨上传MES")]
        public RETURN_SAP MES_SLDBZS1(string SITE, string WORKSHOP, string STORAGELOCATION, string ITEM, string BATCH, string DATETIME, string TRANSFERTYPE)
        {
            #region 
            EOS_SLWEIGHT entity = new EOS_SLWEIGHT();
            entity.SITE = SITE;//站点
            entity.WORKSHOP = WORKSHOP;//车间
            entity.STORAGELOCATION = STORAGELOCATION;//线边仓
            entity.ITEM = ITEM;//物料
            entity.BATCH = BATCH;//批号
            entity.DATETIME = DATETIME;//称重时间
            entity.TRANSFERTYPE = TRANSFERTYPE;//类型：IN/OUT
            #endregion
            return MES_SLDBZS(entity, "");
        }

        /// <summary>
        /// 生产机：生产机上料调拨上传MES
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：上料调拨上传MES")]
        public RETURN_SAP MES_SLDBZS(EOS_SLWEIGHT entity, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            if (string.IsNullOrEmpty(entity.BATCH))
            {
                ret.STATUS = "E";
                ret.REMSG = "批号不能为空！";
                ret.REDATA = null;
            }
            MESSERVICES_SLDBZS.LoadometerWeighingTransferWSClient db = new MESSERVICES_SLDBZS.LoadometerWeighingTransferWSClient("LoadometerWeighingTransferWSPortZS");
            #region 初始化
            MESSERVICES_SLDBZS.loadometerWeighingTransferRequest rq = new MESSERVICES_SLDBZS.loadometerWeighingTransferRequest();
            rq.site = entity.SITE;// 站点"1000";
            rq.userId = "ME_USER";
            rq.lineRequestList = new MESSERVICES_SLDBZS.loadometerWeighingTransferLineRequest[1];
            MESSERVICES_SLDBZS.loadometerWeighingTransferLineRequest wr = new MESSERVICES_SLDBZS.loadometerWeighingTransferLineRequest();
            //SITE 站点缺少
            wr.workShop = entity.WORKSHOP;// "D30031";//车间
            wr.storageLocation = entity.STORAGELOCATION;// "XB05";// 线边仓
            wr.item = entity.ITEM.Replace("00000000000", "");// "3030040"; //物料
            wr.batch = entity.BATCH;// "2021092801";//批次
            wr.qty = entity.QTY;//数量

            wr.dateTime = entity.DATETIME;// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// "2021-09-28 18:58:00";//称重时间
            wr.transferType = entity.TRANSFERTYPE;// "IN";//记录类型: IN(调入该线边仓）、OUT（调出该线边仓）
            wr.vendor = "";//供应商可为空
            rq.lineRequestList[0] = wr;
            MESSERVICES_SLDBZS.executeLoadometerWeighingTransfer gb = new MESSERVICES_SLDBZS.executeLoadometerWeighingTransfer();
            gb.request = rq;

            var credential = db.ClientCredentials.UserName;
            credential.UserName = "ME_USER";
            credential.Password = "Init1234";
            #endregion

            MESSERVICES_SLDBZS.executeLoadometerWeighingTransferResponse resulte = db.executeLoadometerWeighingTransfer(gb);

            if (resulte.@return.code == 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "上传数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = $"上传数据失败！{resulte.@return.message}";
                ret.REDATA = null;
            }

            return ret;
        }



        /// <summary>
        /// 生产机：底浆原材料301小磅上料上传MES
        /// </summary>
        /// <param name="strjson">可为空</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：底浆原材料301小磅上料上传MES")]
        public RETURN_SAP MES_DJ301XBSLZS(EOS_SLWEIGHT entity, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            if (string.IsNullOrEmpty(entity.BATCH))
            {
                ret.STATUS = "E";
                ret.REMSG = "批号不能为空！";
                ret.REDATA = null;
            }

            MESSERVICES_XBSLZS.RawMaterialFeedingServiceWSClient db = new MESSERVICES_XBSLZS.RawMaterialFeedingServiceWSClient("RawMaterialFeedingServiceWSPortZS");
            #region 初始化
            MESSERVICES_XBSLZS.rawMaterialFeedingRequest rq = new MESSERVICES_XBSLZS.rawMaterialFeedingRequest();
            rq.site = entity.SITE;// 站点"1000";
            rq.userId = "ME_USER";
            rq.rowRequestList = new MESSERVICES_XBSLZS.rawMaterialFeedingRowRequest[1];
            MESSERVICES_XBSLZS.rawMaterialFeedingRowRequest wr = new MESSERVICES_XBSLZS.rawMaterialFeedingRowRequest();
            wr.storageLocation = entity.STORAGELOCATION;// "XB05";// 线边仓
            wr.item = entity.ITEM.Replace("00000000000", "");// "3030040"; //物料
            wr.batch = entity.BATCH;// "2021092801";//批次
            wr.qty = entity.QTY;//数量
            wr.pulpType = "DJ";// "DJ";//浆原料类型（无人值守底浆原材料调用时，固定传入DJ）物料组：301
            wr.dateTime = entity.DATETIME;// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// "2021-09-28 18:58:00";//称重时间
            wr.itemMark = entity.ITEMMARK;//材料标识，1标识直进车，2标识化验包，3标识垛位调拨
            wr.shift = entity.SHIFT;//班次，ABC(夜白中)
            wr.shiftDate = entity.SHIFTDATE;//班次日期，格式2021-11-29
            rq.rowRequestList[0] = wr;
            var credential = db.ClientCredentials.UserName;
            credential.UserName = "ME_USER";
            credential.Password = "Init1234";
            #endregion

            MESSERVICES_XBSLZS.doRawMaterialFeeding gb = new MESSERVICES_XBSLZS.doRawMaterialFeeding();
            gb.rawMaterialFeedingRequest = rq;
            MESSERVICES_XBSLZS.doRawMaterialFeedingResponse resulte = db.doRawMaterialFeeding(gb);
            if (resulte.@return.code == 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "上传数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = $"上传数据失败！{resulte.@return.message}";
                ret.REDATA = null;
            }

            return ret;
        }


        public RETURN_SAP MES_DJ301XBSLZS1(EOS_SLWEIGHT entity, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            if (string.IsNullOrEmpty(entity.BATCH))
            {
                ret.STATUS = "E";
                ret.REMSG = "批号不能为空！";
                ret.REDATA = null;
            }
            RawMaterialFeedingServiceWSService db1 = new RawMaterialFeedingServiceWSService();
            db1.Timeout = 1800000;
            db1.Credentials = new System.Net.NetworkCredential("ME_USER", "Init1234");
            rawMaterialFeedingRequest rq1 = new rawMaterialFeedingRequest();
            rq1.site = entity.SITE;// 站点"1000";
            rq1.userId = "ME_USER";
            rq1.rowRequestList = new rawMaterialFeedingRowRequest[1];

            rawMaterialFeedingRowRequest wr1 = new rawMaterialFeedingRowRequest();
            wr1.storageLocation = entity.STORAGELOCATION;// "XB05";// 线边仓
            wr1.item = entity.ITEM.Replace("00000000000", "");// "3030040"; //物料
            wr1.batch = entity.BATCH;// "2021092801";//批次
            wr1.qty = entity.QTY;//数量
            wr1.pulpType = "DJ";// "DJ";//浆原料类型（无人值守底浆原材料调用时，固定传入DJ）物料组：301
            wr1.dateTime = entity.DATETIME;// DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");// "2021-09-28 18:58:00";//称重时间
            wr1.itemMark = entity.ITEMMARK;//材料标识，1标识直进车，2标识化验包，3标识垛位调拨
            wr1.shift = entity.SHIFT;//班次，ABC(夜白中)
            wr1.shiftDate = entity.SHIFTDATE;//班次日期，格式2021-11-29
            rq1.rowRequestList[0] = wr1;
            doRawMaterialFeeding gb1 = new doRawMaterialFeeding();
            gb1.rawMaterialFeedingRequest = rq1;
            doRawMaterialFeedingResponse resulte = db1.doRawMaterialFeeding(gb1);
            if (resulte.@return.code == 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "上传数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = $"上传数据失败！{resulte.@return.message}";
                ret.REDATA = null;
            }
            return ret;
        }


        public RETURN_SAP MES_DJ302XBSLZS2(EOS_SLWEIGHT entity, string strjson)
        {
            RETURN_SAP ret = new RETURN_SAP();
            if (string.IsNullOrEmpty(entity.BATCH))
            {
                ret.STATUS = "E";
                ret.REMSG = "批号不能为空！";
                ret.REDATA = null;
            }
            string url = ConfigurationManager.AppSettings["MESDJURL"].ToString();
            XmlNode xmlNode1;
            string strSenddata = "";//这个是要发送的报文数据
            Hashtable ht = new Hashtable();
            ht.Add("xmlIn", strSenddata);
            xmlNode1 = WebServiceCaller.QuerySoapWebService(url, "doRawMaterialFeeding", ht);//接口地址，方法名，发送报文

            doRawMaterialFeedingResponse resulte = JsonConvert.DeserializeObject<doRawMaterialFeedingResponse>("");
            if (resulte.@return.code == 0)
            {
                ret.STATUS = "S";
                ret.REMSG = "上传数据成功！";
                ret.REDATA = null;
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = $"上传数据失败！{resulte.@return.message}";
                ret.REDATA = null;
            }
            return ret;
        }
        #endregion

        #region 生产机：无人值守调用SAP接口

        /// <summary>
        /// 生产机：无人值守采购订单：接收接口
        /// </summary>
        /// <param name="strzaedat">开始时间</param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：无人值守采购订单：接收接口")]
        public RETURN_SAP WRZS_POORDER(string strzaedat)
        {
            RETURN_SAP ret = new RETURN_SAP();
            #region 是否启用
            BASE_SYSTEMSET config = Db.Queryable<BASE_SYSTEMSET>()
                .Where(c => c.FIELDNAME == "WRZS_POORDER")
                .Take(1).First();
            if (config != null)
            {
                if (config.FVALUE == "0")
                {
                    ret.STATUS = "E";
                    ret.REMSG = "采购订单推送接口停用，请联系管理员启用接口！";
                    REDATA redata = new REDATA();
                    redata.MATDOCUMENTYEAR = "";
                    redata.MATERIALDOCUMENT = "";
                    redata.PRUEFLOS = "";
                    redata.E_POSNR = "";
                    ret.REDATA = redata;
                    return ret;
                }
            }
            #endregion

            #region 初始数据
            ////开始时间
            //if (string.IsNullOrEmpty(strzaedat))
            //{
            //    strzaedat = DateTime.Now.ToString("yyyy-MM-dd");
            //}

            //string strwerks = ConfigurationManager.AppSettings["STRWERKS"];
            //if (string.IsNullOrEmpty(strwerks))
            //{
            //    strwerks = "1000";
            //}
            #endregion
            //string[] arr = strwerks.Split(',');
            //foreach (string werks in arr)
            //{
            //    ret = SAP_116ZS(strzaedat, "", "", werks, "", "", "");
            //}
            ret = SAP_116ZS01(strzaedat, "", "", "", "", "");
            return ret;
        }

        /// <summary>
        /// 生产机：无人值守采购磅单：化工、包装物一次计量推送接口
        /// </summary>
        /// <param name="materialtype">化工:YG40、包装物:YG60:</param>
        /// <returns>物料类型为空时默认化工YG40推送。</returns>
        [WebMethod(Description = "生产机：无人值守采购磅单：化工、包装物一次计量推送接口")]
        public RETURN_SAP WRZS_HGBZWFINISTWEIGHTZS(string materialtype, string BILLNO)
        {
            RETURN_SAP ret = new RETURN_SAP();
            REDATA redata = new REDATA();
            #region 是否启用
            if (string.IsNullOrEmpty(BILLNO))
            {
                BASE_SYSTEMSET config = Db.Queryable<BASE_SYSTEMSET>()
                 .Where(c => c.FIELDNAME == "WRZS_HGBZWFINISTWEIGHTZS")
                 .Take(1).First();
                if (config != null)
                {
                    if (config.FVALUE == "0")
                    {
                        ret.STATUS = "E";
                        ret.REMSG = "化工包装物一次计量接口停用，请联系管理员启用接口！";

                        redata.MATDOCUMENTYEAR = "";
                        redata.MATERIALDOCUMENT = "";
                        redata.PRUEFLOS = "";
                        redata.E_POSNR = "";
                        ret.REDATA = redata;
                        return ret;
                    }
                }
            }
            #endregion

            #region 查询数据
            //if (string.IsNullOrEmpty(materialtype))
            //{
            //    materialtype = "YG40";//默认化工
            //}
            string[] mattypeList = new string[] { "YG40"};//sxy/myt 原逻辑{ "YG40", "YG60" }，去掉YG60
            var list = Db.Queryable<VWB_WEIGHT_FINISHSH>()
                //.Where(c => c.ISUSE == "7")
                .Where(c => c.UPLOAD == "0")
                .WhereIF(string.IsNullOrEmpty(BILLNO), c => c.ISERROR == "0")
                .Where(c => c.STATUS != "3")
                 .Where(c => mattypeList.Contains(c.MATERIALTYPE))
                .WhereIF(!string.IsNullOrEmpty(BILLNO), c => c.BILLNO == BILLNO)
              .OrderBy(c => c.WEIGHTDATE).ToList();
            #endregion
            EOS_Z_PO_POST entity = new EOS_Z_PO_POST();

            #region 初始数据
            string EOS_GM_CODE = "01";
            string DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            string PSTNG_DATE = "";
            string PO_NUMBER = "";
            string PO_ITEM = "";
            string MATERIAL = "";
            string MATERIALNAME = "";
            string SUPPLY = "";
            string SUPPLYNAME = "";
            string BATCH = "";
            decimal ENTRY_QNT = 0;
            string STGE_LOC = "";
            string PLANT = "";
            string MOVE_TYPE = "103";
            string MVT_IND = "B";
            string JLBILLNO = "";
            double GROSS = 0.0;
            string MATBATCHNO = "";
            string ACCEPTANCE = "";
            string WGTYPE = "";
            string ISZJ = "";
            string ZJRESULT = "";
            string ZJRESULT1 = "";
            string MATERIALTYPE = "";
            string CARSNAME = "";
            #endregion

            int successnum = 0;
            string sql = "";
            string PZBILLNO = "";
            string ZJBILLNO = "";
            string key = "";

            foreach (var data in list)
            {
                key = $"{data.PK_ORDER_B}_ONE";
                log.Error($"一次计量收货:过磅号{data.BILLNO}！");
                if (DataCache.IsExist(key) == true)
                {
                    ret.STATUS = "E";
                    ret.REMSG = "当前数据已收货！";
                    redata.MATDOCUMENTYEAR = "";
                    redata.MATERIALDOCUMENT = "";
                    redata.PRUEFLOS = "";
                    redata.E_POSNR = "";
                    ret.REDATA = redata;
                }
                else if (!string.IsNullOrEmpty(data.PZBILLNO) || data.UPLOAD == "1")
                {
                    ret.STATUS = "E";
                    ret.REMSG = $"当前数据已收货{data.PZBILLNO}！";
                    redata.MATDOCUMENTYEAR = "";
                    redata.MATERIALDOCUMENT = "";
                    redata.PRUEFLOS = "";
                    redata.E_POSNR = "";
                    ret.REDATA = redata;
                }
                else
                {
                    //创建执行缓存,5分种有效期
                    DataCache.Insert(key, key, 5);
                    if (!string.IsNullOrEmpty(data.WEIGHTDATE))
                    {
                        PSTNG_DATE = Convert.ToDateTime(data.WEIGHTDATE).ToString("yyyy-MM-dd");
                    }
                    else
                    {
                        PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                    }

                    #region 初始化值
                    JLBILLNO = data.BILLNO;
                    PO_NUMBER = data.PK_BILL;
                    PO_ITEM = data.PK_BILL_B.Replace(data.PK_BILL, "");
                    MATERIAL = data.PK_MATERIAL;
                    MATERIALNAME = data.MATERIAL;
                    MATERIALTYPE = data.MATERIALTYPE;
                    SUPPLY = data.PK_SENDORG;
                    SUPPLYNAME = data.SENDORG;
                    BATCH = data.MATBATCHNO;
                    ENTRY_QNT = data.GROSS;
                    //STGE_LOC = data.PK_STORE; // 此字段为空弃置不用  新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
                    PLANT = data.PK_RECEIVEORG;
                    GROSS = Convert.ToDouble(data.GROSS);
                    MATBATCHNO = data.MATBATCHNO;
                    ACCEPTANCE = data.ACCOUNT;
                    WGTYPE = data.WGTYPE;
                    ISZJ = data.ISZJ;
                    ZJRESULT = data.ZJRESULT;
                    ZJRESULT1 = data.ZJRESULT1;
                    CARSNAME = data.CARS;
                    entity.EOS_GM_CODE = EOS_GM_CODE;
                    entity.DOC_DATE = DOC_DATE;
                    entity.PSTNG_DATE = PSTNG_DATE;
                    entity.PO_NUMBER = PO_NUMBER;//订单编号
                    entity.PO_ITEM = PO_ITEM;//订单项目编号
                    entity.MATERIAL = MATERIAL;//物料

                    entity.BATCH = BATCH;//批次
                    entity.ENTRY_QNT = ENTRY_QNT;//数量
                    //entity.STGE_LOC = STGE_LOC;// "Y490";//库存地点 // 此字段为空弃置不用  新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
                    entity.PLANT = PLANT;//工厂
                    entity.MOVE_TYPE = MOVE_TYPE;//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
                    entity.MVT_IND = MVT_IND;//移动标识,B,F,B是采购订单  F是生产订单
                    entity.BILL_OF_LADING = "";
                    entity.GR_GI_SLIP_NO = "";
                    entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
                    entity.HEADER_TXT = "化工、包装物一次计量收货";
                    entity.PQ = "";
                    #endregion
                    //调用质检计划
                    //测试
                    //PZBILLNO = data.PZBILLNO;
                    //ZJBILLNO = data.ZJBILLNO;
                    //ret = MES_PURCHASEZS1(PLANT, PO_NUMBER, PO_ITEM, ZJBILLNO, MATERIAL, MATERIALNAME, JLBILLNO, MATBATCHNO, GROSS, "", PSTNG_DATE, CARSNAME, ACCEPTANCE, SUPPLY, SUPPLYNAME, WGTYPE, ISZJ, MATERIALTYPE, PZBILLNO);
                    ret = SAP_POST1ZS(entity, "");
                }
                if (ret.STATUS == "S")
                {
                    #region 成功
                    PZBILLNO = ret.REDATA.MATERIALDOCUMENT;
                    ZJBILLNO = ret.REDATA.PRUEFLOS;
                    successnum++;
                    log.Error($"一次计量推送:过磅号{data.BILLNO}检验批号{ZJBILLNO}、凭证编号{PZBILLNO}成功！");
                    Db.Ado.ExecuteCommand($"update DP_POCARSORDER_DTL set UPLOAD=1,ZJBILLNO='{ZJBILLNO}',PZBILLNO='{PZBILLNO}',PZDATE='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'  where ID='{data.PK_ORDER_B}'");
                    // 新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
                    string TEL = ""; string PK_STORE = "";
                    var addhgfirst = Db.Queryable<VAPP_HANDORDER_ADDHGFIRST>().Where(c => c.BILLNO == data.BILLNO).ToList();
                    if (addhgfirst.Count > 0)
                    {
                        PK_STORE = addhgfirst.First().PK_STORE;
                        TEL = addhgfirst.First().TEL;
                    }
                    // 新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
                    //调用质检计划
                    ret = MES_PURCHASEZS1(PLANT, PO_NUMBER, PO_ITEM, ZJBILLNO, MATERIAL, MATERIALNAME, JLBILLNO, MATBATCHNO, GROSS, "", PSTNG_DATE, CARSNAME, ACCEPTANCE, SUPPLY, SUPPLYNAME, WGTYPE, ISZJ, MATERIALTYPE, PZBILLNO, TEL, PK_STORE);
                    #endregion
                }
                else
                {
                    Db.Ado.ExecuteCommand($"update DP_POCARSORDER_DTL set ISERROR=1,PZDATE='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'  where ID='{data.PK_ORDER_B}'");
                    sql = IF_LOG(data.BILLNO, "WRZS_FINISTWEIGHTZS", ret.REMSG);
                    Db.Ado.ExecuteCommand(sql);
                    log.Error($"一次计量推送:过磅号{data.BILLNO}失败！");
                }
            }
            string msg = $"推送车数：{list.Count()},成功{successnum}条！";
            log.Error(msg);
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;

        }


        /// <summary>
        /// 生产机：无人值守采购磅单二次计量推送接口
        /// </summary>
        /// <param name="materialtype">物料分类</param>
        /// <returns>为空时所有分类</returns>
        [WebMethod(Description = "生产机：无人值守采购磅单二次计量推送接口")]
        public RETURN_SAP WRZS_SECONDWEIGHTZS(string materialtype, string BILLNO)
        {
            String url = $"{ ConfigHelper.AppSettings("QKLURL")}";
            //HttpPost1(url, BILLNO);
            RETURN_SAP ret = new RETURN_SAP();
            RETURN_SAP ret1 = new RETURN_SAP();
            REDATA redata = new REDATA();
            ret.STATUS = "E";

            bool upload1 = true;//化工状态
            #region 是否启用
            BASE_SYSTEMSET config = null;
            if (string.IsNullOrEmpty(BILLNO))
            {
                config = Db.Queryable<BASE_SYSTEMSET>()
            .Where(c => c.FIELDNAME == "WRZS_SECONDWEIGHTZS")
            .Take(1).First();

                if (config != null)
                {
                    if (config.FVALUE == "0")
                    {
                        ret.STATUS = "E";
                        ret.REMSG = "二次计量过账接口停用，请联系管理员启用接口！";
                        redata.MATDOCUMENTYEAR = "";
                        redata.MATERIALDOCUMENT = "";
                        redata.PRUEFLOS = "";
                        redata.E_POSNR = "";
                        ret.REDATA = redata;
                        return ret;
                    }
                }
            }
            #endregion

            #region 查询数据
            var list = Db.Queryable<VAPP_HANDORDER_SECOND>()
                    .Where(c => c.UPLOAD == "0")
                  .Where(c => c.WEIGHTTYPE == "0")
                   .Where(c => c.FINISH == "1")
                  //.Where(c => c.JZXSUTTLE > 0)
                  // .Where(c => c.JSUPLOAD == "1")
                  .WhereIF(string.IsNullOrEmpty(BILLNO), c => c.JSUPLOAD == "1")
                  .WhereIF(string.IsNullOrEmpty(BILLNO), c => c.ISERROR == "0")
                    .WhereIF(!string.IsNullOrEmpty(BILLNO), c => c.BILLNO == BILLNO)
              .WhereIF(!string.IsNullOrEmpty(materialtype), c => c.PK_PARENT == materialtype)
              .OrderBy(c => c.WEIGHTDATE).ToPageList(1, 1);
            #endregion

            EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            EOS_Z_PO_POST2 entity3 = new EOS_Z_PO_POST2();
            EOS_ZFMM098 entity1 = new EOS_ZFMM098();
            EOS_Z_GM_POST entity2 = new EOS_Z_GM_POST();
            int successnum = 0;
            decimal ENTRY_QNT = 0;
            decimal MDCZ = 0;
            string MATERIALBARCODE = "";
            string vbillcode = "";
            string crowno = "";
            string pk_material = "";
            string sql = "";
            EOS_QKLWEIGHT weight = new EOS_QKLWEIGHT();
            RETURN_QKL qkl = new RETURN_QKL();
            string key = "";
            foreach (var data in list)
            {
                key = $"{data.ID}";
                log.Error($"二次计量推送:过磅号{data.BILLNO}！");
                if (DataCache.IsExist(key) == true)
                {
                    ret.STATUS = "E";
                    ret.REMSG = "当前数据已过账！";
                    redata.MATDOCUMENTYEAR = "";
                    redata.MATERIALDOCUMENT = "";
                    redata.PRUEFLOS = "";
                    redata.E_POSNR = "";
                    ret.REDATA = redata;
                }
                else if (!string.IsNullOrEmpty(data.GZPZBILLNO) || data.UPLOAD == "1")
                {
                    ret.STATUS = "E";
                    ret.REMSG = $"当前数据已过账{data.GZPZBILLNO}！";
                    redata.MATDOCUMENTYEAR = "";
                    redata.MATERIALDOCUMENT = "";
                    redata.PRUEFLOS = "";
                    redata.E_POSNR = "";
                    ret.REDATA = redata;
                }
                else
                {
                    //创建执行缓存,5分种有效期
                    DataCache.Insert(key, data.ID, 5);
                    MATERIALBARCODE = data.MATERIALBARCODE;//外部物料组
                    vbillcode = data.VBILLCODE;//订单编号
                    crowno = data.PK_BILL_B.Replace(data.PK_BILL, "");//订单项目编号
                    pk_material = data.PK_MATERIAL;  //物料
                    #region 判定物料
                    if (!string.IsNullOrEmpty(data.GPMATERIAL))
                    {
                        var listob = Db.Queryable<VNC_PO_ORDER_B>().Where(r => r.PK_ORDER_B == data.GPORDERNO).ToList();
                        var ob = listob.FirstOrDefault();
                        vbillcode = ob.VBILLCODE;//订单项目编号 
                        crowno = ob.CROWNO;
                        pk_material = ob.PK_MATERIAL;
                    }
                    #endregion

                    #region 区块链数据上传
                    if (data.PCBILLFROM == "3" && data.UPLOAD3 == "0")
                    {
                        #region 初始化
                        weight.ID = data.MID;
                        weight.PK_ORDER = data.MID;
                        weight.BILLNO = data.BILLNO;
                        weight.PK_RECEIVRORG = data.PK_RECEIVEORG;
                        weight.RECEIVRORG = data.RECEIVEORG;
                        weight.WEIGHTDATE = data.WEIGHTDATE;
                        weight.LIGHTDATE = data.LIGHTDATE;
                        weight.CRETIME = data.CRETIME;
                        weight.CREUSER = data.CREUSER;
                        weight.STORENAME = data.STORE;
                        weight.GROSS = data.GROSS;
                        weight.TARE = data.TARE;
                        weight.SUTTLE = $"{data.SUTTLE}";
                        weight.SUTTLE2 = $"{data.JZXSUTTLE}";
                        weight.BATCHNO = data.DEF9;
                        weight.PDAKZ = data.PDAKZ2;
                        weight.KS = data.PDAKZ3;
                        weight.HZ = data.PDAKZ4;
                        weight.LJ = data.PDAKZ5;
                        weight.PRICE = $"{data.JZXSUTTLE1}";
                        weight.CHARGE = $"{data.JZXSUTTLE1 * data.JZXSUTTLE / 1000}";
                        weight.VBILLCODE = vbillcode;
                        weight.CROWNO = crowno;
                        weight.PK_MATERIAL = data.PK_MATERIAL.Replace("00000000000", "");
                        weight.MATERIALNAME = data.MATERIALNAME;
                        weight.GPMATERIAL = data.PK_MATERIAL.Replace("00000000000", "");
                        weight.GPMATERIALNAME = data.MATERIALNAME;
                        weight.DBMATERIAL = data.DBMATERIAL;
                        weight.DBMATERIALNAME = data.DBMATERIALNAME;
                        if (!string.IsNullOrEmpty(data.GPMATERIAL))
                        {
                            weight.GPMATERIAL = data.GPMATERIAL.Replace("00000000000", "");
                            weight.GPMATERIALNAME = data.GPMATERIALNAME;
                        }
                        weight.DEF5 = data.SHBATCHNO;
                        weight.AMOUNT = $"{ data.YFSUTTLE}";
                        #endregion
                        qkl = WRZS_QKLWEIGHTSELECTZS(weight);
                    }
                    #endregion

                    if (data.JZXSUTTLE == 0)
                    {
                        #region 结算量为0不上传
                        ret.STATUS = "E";
                        ret.REMSG = "结算量为0不能上传!";
                        redata.MATDOCUMENTYEAR = "";
                        redata.MATERIALDOCUMENT = "";
                        redata.PRUEFLOS = "";
                        redata.E_POSNR = "";
                        ret.REDATA = redata;
                        #endregion
                    }
                    else
                    {
                        if (data.BILLTYPE == "YG15")
                        {
                            //生产订单
                            #region 订单
                            if (!string.IsNullOrEmpty(data.GPMATERIAL))
                            {
                                var listob = Db.Queryable<VNC_PO_ORDER_B>().Where(r => r.PK_ORDER_B == data.GPORDERNO).ToList();
                                var ob = listob.FirstOrDefault();
                                vbillcode = ob.VBILLCODE;//订单项目编号 
                                crowno = ob.CROWNO;
                                pk_material = ob.PK_MATERIAL;
                            }
                            else
                            {
                                var listob = Db.Queryable<VNC_PO_ORDER_B>().Where(r => r.PK_ORDER == data.PK_BILL && r.PK_ORDER_B == data.PK_BILL_B).ToList();
                                var ob = listob.FirstOrDefault();
                                vbillcode = ob.VBILLCODE;//订单项目编号 
                                crowno = ob.CROWNO;
                                pk_material = ob.PK_MATERIAL;
                            }
                            #endregion

                            #region  生产订单过账

                            #region  生产订单过账参数
                            entity.VENDRBATCH = data.BILLNO;
                            entity.EOS_GM_CODE = "01";
                            entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                            entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                            entity.PO_NUMBER = "";
                            entity.PO_ITEM = "00000";
                            entity.ORDERID = vbillcode;//订单编号
                            entity.ORDER_ITNO = crowno;//订单项目编号
                            entity.MATERIAL = pk_material;  //物料
                            entity.BATCH = data.BATCHNO; //批次
                            entity.ENTRY_QNT = data.JZXSUTTLE;
                            if (entity.ENTRY_QNT == 0)
                            {
                                entity.ENTRY_QNT = Convert.ToDecimal(data.SUTTLE);
                            }
                            entity.STGE_LOC = data.PK_STORE;// "Y490";//库存地点
                            entity.PLANT = data.PK_RECEIVEORG;//工厂
                            entity.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
                            entity.MVT_IND = "F";//移动标识,B,F,B是采购订单  F是生产订单
                            entity.LIFNR = "";
                            entity.EBELN = "";
                            entity.MATNR = "";
                            entity.EINDT = "";
                            entity.MENGE = 0;
                            entity.NETPR = 0;
                            entity.WERKS = "";
                            entity.PQ = "";
                            entity.BILL_OF_LADING = "";
                            entity.GR_GI_SLIP_NO = "";
                            entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空  
                            if (entity.ORDERID.Length == 7)
                            {
                                entity.ORDERID = string.Format("00000{0}", entity.ORDERID);
                            }
                            entity.HEADER_TXT = "无人计量采购生产订单过账";

                            #endregion

                            ret = SAP_POSTZS(entity, "");
                            #endregion
                        }
                        else if (data.BILLTYPE == "YG16")
                        {
                            #region 散件订单直接更新状态
                            ret.STATUS = "S";
                            ret.REMSG = "散件订单直接更新为已推单!";
                            redata.MATDOCUMENTYEAR = "";
                            redata.MATERIALDOCUMENT = "";
                            redata.PRUEFLOS = "";
                            redata.E_POSNR = "";
                            ret.REDATA = redata;
                            #endregion
                        }
                        else
                        {
                            #region 其它过账
                            if (data.MATERIALGROUP == "YG40" )//sxy/myt 原逻辑(data.MATERIALGROUP == "YG40" || data.MATERIALGROUP == "YG60") ，干掉YG60 220825
                            {
                                #region 是否启用
                                if (string.IsNullOrEmpty(BILLNO))
                                {
                                    config = Db.Queryable<BASE_SYSTEMSET>()
                                .Where(c => c.FIELDNAME == "WRZS_HGSECONDWEIGHTZS")
                                .Take(1).First();
                                    if (config != null)
                                    {
                                        if (config.FVALUE == "0")
                                        {
                                            ret.STATUS = "E";
                                            ret.REMSG = "化工二次计量过账接口停用，请联系管理员启用接口！";
                                            redata.MATDOCUMENTYEAR = "";
                                            redata.MATERIALDOCUMENT = "";
                                            redata.PRUEFLOS = "";
                                            redata.E_POSNR = "";
                                            ret.REDATA = redata;
                                            upload1 = false;
                                        }
                                    }

                                    #region 是否一车多料
                                    //sxy/myt 220905 新增逻辑化工辅料一车一料允许自动过账，一车多料（是否拼单==1）
                                    DP_POCARSORDER dp_pocarsorder = Db.Queryable<DP_POCARSORDER>()
                                    .Where(c => c.ID == data.MID)
                                    .Take(1).First();
                                    if (dp_pocarsorder.ISMULTI == "1")
                                    {
                                        ret.STATUS = "E";
                                        ret.REMSG = "化工二次计量过账接口停用，一车多料请手动过账！";
                                        redata.MATDOCUMENTYEAR = "";
                                        redata.MATERIALDOCUMENT = "";
                                        redata.PRUEFLOS = "";
                                        redata.E_POSNR = "";
                                        ret.REDATA = redata;
                                        upload1 = false;
                                    }
                                    //sxy/myt 220905 新增逻辑化工辅料一车一料允许自动过账
                                    #endregion
                                }
                                #endregion

                                #region 化工包装物二次推送
                                if (!string.IsNullOrEmpty(data.ZJBILLNO) && upload1 == true)
                                {
                                    #region 初始化
                                    //I_PRUEFLOS = 10000191445，I_VCODE = 001，I_FXZKC = 23，I_THJH = 10，I_LGORT = WJ01
                                    entity1.PRUEFLOS = data.ZJBILLNO;//检验批号
                                    entity1.LGORT = data.PK_STORE;//库存地点
                                    entity1.FXZKC = $"{data.SUTTLE}";
                                    if (data.SUTTLE > 0)
                                    {
                                        if (data.JZXSUTTLE > 0)
                                        {
                                            entity1.FXZKC = $"{ data.JZXSUTTLE}";//(净重 - 扣重)
                                        }
                                        entity1.THJH = $"{Convert.ToDecimal(data.GROSS) - data.JZXSUTTLE }";//$"{Convert.ToDecimal(data.PDAKZ2) + Convert.ToDecimal(data.TARE)+ chazzhi}";//(皮+扣重)
                                        entity1.VCODE = "001";
                                    }
                                    else
                                    {
                                        entity1.THJH = data.GROSS;
                                        entity1.VCODE = "005";
                                    }
                                    entity1.JYJG = "";//检验结果数
                                    #endregion
                                    EOS_ZFMM115 zfmm115 = new EOS_ZFMM115();
                                    zfmm115.PRUEFLOS = data.ZJBILLNO;
                                    zfmm115.CHARG = data.MATBATCHNO;
                                    ret = SAP_115ZS(zfmm115, "");
                                    if (ret.STATUS == "S" || ret.REMSG.Contains("系统状态 UD 是活动的"))
                                    {
                                        ret = SAP_98ZS(entity1, "");
                                    }
                                }
                                else
                                {
                                    ret.REMSG = "一次磅单未生成检验计划!";
                                    redata.MATDOCUMENTYEAR = "";
                                    redata.MATERIALDOCUMENT = "";
                                    redata.PRUEFLOS = "";
                                    redata.E_POSNR = "";
                                    ret.REDATA = redata;
                                }

                                #endregion
                            }
                            else if (data.MATERIALGROUP == "YG50")
                            {
                                #region 辅助材料业务先不上传
                                ret.STATUS = "E";
                                ret.REMSG = "辅助材料业务先不上传!";
                                redata.MATDOCUMENTYEAR = "";
                                redata.MATERIALDOCUMENT = "";
                                redata.PRUEFLOS = "";
                                redata.E_POSNR = "";
                                ret.REDATA = redata;
                                #endregion
                                #region  注释
                                //柴油类
                                //#region  初始化
                                //entity.EOS_GM_CODE = "01";
                                //entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                                //entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                                //entity.PO_NUMBER = vbillcode;//订单编号
                                //entity.PO_ITEM = crowno;//订单项目编号
                                //entity.MATERIAL = pk_material;  //物料
                                //entity.BATCH = data.BATCHNO; //批次
                                //entity.ENTRY_QNT = data.JZXSUTTLE;
                                //if (entity.ENTRY_QNT == 0)
                                //{
                                //    entity.ENTRY_QNT = Convert.ToDecimal(data.SUTTLE);
                                //}
                                //entity.STGE_LOC = data.PK_STORE;// "Y490";//库存地点
                                //entity.PLANT = data.PK_RECEIVEORG;//工厂
                                //entity.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
                                //entity.MVT_IND = "B";//移动标识,B,F,B是采购订单  F是生产订单
                                //entity.BILL_OF_LADING = "";
                                //entity.GR_GI_SLIP_NO = "";
                                //entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
                                //entity.HEADER_TXT = "无人计量采购过账";
                                //#region 311参数
                                //entity.EBELN = "";//采购凭证编号
                                //entity.MATNR = "";//物料编号
                                //entity.LIFNR = "";//供应商
                                //entity.MENGE = 0;//采购订单数量(过磅量)
                                //entity.EINDT = "";//项目交货日期
                                //entity.WERKS = "";//工厂
                                //entity.NETPR = 0;//净价
                                //entity.PQ = "N";//纸质类型
                                //#endregion


                                //#endregion
                                ////过账执行
                                //ret = SAP_POSTZS(entity, "");
                                #endregion
                            }
                            else
                            {
                                #region  初始化
                                //if (data.TYPE == "7"|| MATERIALBARCODE == "318")
                                if (data.TYPE == "7")
                                {
                                    #region 木浆
                                    entity3.EOS_GM_CODE = "01";
                                    entity3.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                                    entity3.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                                    entity3.PO_NUMBER = vbillcode;//订单编号
                                    entity3.PO_ITEM = crowno;//订单项目编号
                                    entity3.MATERIAL = pk_material;  //物料
                                    entity3.BATCH = data.BATCHNO; //批次
                                    entity3.ENTRY_QNT = data.JZXSUTTLE;
                                    entity3.STGE_LOC = data.PK_STORE;// "Y490";//库存地点
                                    entity3.PLANT = data.PK_RECEIVEORG;//工厂
                                    entity3.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
                                    entity3.MVT_IND = "B";//移动标识,B,F,B是采购订单  F是生产订单
                                    entity3.SPEC_STOCK = "";//特殊库存标识,K寄售,空
                                    entity3.WEIGHT = Convert.ToDouble(data.DBAMOUNT);//单包重量
                                    entity3.BILL_OF_LADING = data.TDBILLNO;//提单号
                                    entity3.GR_GI_SLIP_NO = data.TDBILLNO;//提单号
                                    entity3.PINP = data.MATERIALLEVEL;//品牌
                                    entity3.HEADER_TXT = "无人计量木浆采购过账";
                                    #endregion
                                }
                                else
                                {
                                    if (MATERIALBARCODE == "318")
                                    {
                                        #region 初始化
                                        entity.VENDRBATCH = data.BILLNO;
                                        entity.EOS_GM_CODE = "01";
                                        entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                                        entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                                        entity.PO_NUMBER = vbillcode;//订单编号
                                        entity.PO_ITEM = crowno;//订单项目编号
                                        entity.MATERIAL = pk_material;  //物料
                                        entity.BATCH = data.BATCHNO; //批次
                                        entity.ENTRY_QNT = data.JZXSUTTLE;
                                        entity.STGE_LOC = data.PK_STORE;// "Y490";//库存地点
                                        entity.PLANT = data.PK_RECEIVEORG;//工厂
                                        entity.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
                                        entity.MVT_IND = "B";//移动标识,B,F,B是采购订单  F是生产订单
                                        entity.BILL_OF_LADING = data.TDBILLNO;//提单号
                                        entity.GR_GI_SLIP_NO = data.TDBILLNO;//提单号
                                        entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
                                        entity.HEADER_TXT = "无人计量采购过账";
                                        entity.PQ = "";//纸质类型
                                        #endregion
                                    }
                                    else
                                    {
                                        #region 初始化
                                        entity.VENDRBATCH = data.BILLNO;
                                        entity.EOS_GM_CODE = "01";
                                        entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                                        entity.PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                                        entity.PO_NUMBER = vbillcode;//订单编号
                                        entity.PO_ITEM = crowno;//订单项目编号
                                        entity.MATERIAL = pk_material;  //物料
                                        entity.BATCH = data.BATCHNO; //批次
                                        entity.ENTRY_QNT = data.JZXSUTTLE;
                                        entity.STGE_LOC = data.PK_STORE;// "Y490";//库存地点
                                        entity.PLANT = data.PK_RECEIVEORG;//工厂
                                        entity.MOVE_TYPE = "101";//移动类型：废纸101,化工,包装物103。103不产生会计凭证，只有物料凭证。
                                        entity.MVT_IND = "B";//移动标识,B,F,B是采购订单  F是生产订单
                                        entity.BILL_OF_LADING = data.TDBILLNO;//提单号
                                        entity.GR_GI_SLIP_NO = data.TDBILLNO;//提单号
                                        entity.SPEC_STOCK = "";//特殊库存标识,K寄售,空
                                        entity.HEADER_TXT = "无人计量采购过账";
                                        entity.PQ = "";//纸质类型
                                        #endregion
                                    }

                                }
                                #endregion
                                #region 311废纸过账
                                if (MATERIALBARCODE == "311")
                                {
                                    #region 311参数
                                    entity.VENDRBATCH = data.BILLNO;
                                    entity.EBELN = vbillcode;//采购凭证编号
                                    entity.MATNR = pk_material;//物料编号
                                    entity.LIFNR = data.PK_SUPPLIER;//供应商
                                                                    //采购订单数量(过磅量)
                                    entity.MENGE = data.JZXSUTTLE;
                                    //if (entity.MENGE == 0)
                                    //{
                                    //    entity.MENGE = data.SUTTLE;
                                    //}
                                    entity.EINDT = entity.PSTNG_DATE;//项目交货日期
                                    entity.WERKS = data.PK_RECEIVEORG;//工厂
                                    entity.NETPR = data.JZXSUTTLE1;//净价
                                    entity.PQ = "N";//纸质类型
                                    #endregion
                                    //过账执行
                                    ret = SAP_POSTZS(entity, "");
                                }
                                else if (MATERIALBARCODE == "314")
                                {
                                    //国内废纸314
                                    if (!string.IsNullOrEmpty(BILLNO))
                                    {
                                        #region 314参数
                                        entity.VENDRBATCH = data.BILLNO;
                                        entity.EBELN = vbillcode;//采购凭证编号
                                        entity.MATNR = pk_material;//物料编号
                                        entity.LIFNR = data.PK_SUPPLIER;//供应商
                                                                        //采购订单数量(过磅量)
                                        entity.MENGE = data.JZXSUTTLE;
                                        entity.EINDT = entity.PSTNG_DATE;//项目交货日期
                                        entity.WERKS = data.PK_RECEIVEORG;//工厂
                                        entity.NETPR = data.JZXSUTTLE1;//净价
                                        entity.PQ = "N";//纸质类型
                                        #endregion
                                        //过账执行
                                        ret = SAP_POSTZS(entity, "");
                                    }
                                    else
                                    {
                                        #region 314不自动上传
                                        ret.STATUS = "E";
                                        ret.REMSG = "国内废纸314不自动上传!";
                                        redata.MATDOCUMENTYEAR = "";
                                        redata.MATERIALDOCUMENT = "";
                                        redata.PRUEFLOS = "";
                                        redata.E_POSNR = "";
                                        ret.REDATA = redata;
                                        #endregion
                                    }
                                }
                                else if (MATERIALBARCODE == "315" || MATERIALBARCODE == "316" || MATERIALBARCODE == "317")
                                {
                                    if (!string.IsNullOrEmpty(BILLNO))
                                    {
                                        if (MATERIALBARCODE == "317")
                                        {
                                            //科迈原材料
                                            ret = SAP_POSTZS(entity, "");
                                        }
                                        else
                                        {
                                            #region 315,316,317参数
                                            entity.EBELN = vbillcode;//采购凭证编号
                                            entity.MATNR = pk_material;//物料编号
                                            entity.LIFNR = data.PK_SUPPLIER;//供应商
                                                                            //采购订单数量(过磅量)
                                            entity.MENGE = data.JZXSUTTLE;
                                            entity.EINDT = entity.PSTNG_DATE;//项目交货日期
                                            entity.WERKS = data.PK_RECEIVEORG;//工厂
                                            entity.NETPR = data.JZXSUTTLE1;//净价
                                            entity.PQ = "";//纸质类型
                                            #endregion
                                            //过账执行
                                            ret = SAP_POSTZS(entity, "");
                                        }
                                    }
                                    else
                                    {
                                        #region 315,316,317不自动上传
                                        ret.STATUS = "E";
                                        ret.REMSG = string.Format($"{MATERIALBARCODE}不自动上传!");
                                        redata.MATDOCUMENTYEAR = "";
                                        redata.MATERIALDOCUMENT = "";
                                        redata.PRUEFLOS = "";
                                        redata.E_POSNR = "";
                                        ret.REDATA = redata;
                                        #endregion
                                    }
                                }
                                else if (MATERIALBARCODE == "313")
                                {
                                    if (data.TYPE == "7")
                                    {
                                        if (data.MDSUTTLE > 0 && data.YXSUTTLE == 0)
                                        {
                                            #region 最后一车计算
                                            var SumData = Db.Queryable<VAPP_HANDORDER_SECOND>()
                                            .Where(c => c.TDBILLNO == data.TDBILLNO)
                                            .Where(c => c.UPLOAD == "1").ToList()
                                            .GroupBy(x => new { x.TDBILLNO }).Select(y => new
                                            {
                                                TDBILLNO = y.Key.TDBILLNO,
                                                JZXSUTTLE = y.Sum(x => x.JZXSUTTLE),
                                                MDSUTTLE = y.Max(x => x.MDSUTTLE)
                                            });
                                            foreach (var item in SumData)
                                            {
                                                MDCZ = item.MDSUTTLE - item.JZXSUTTLE;
                                                if (MDCZ > 0 && MDCZ < 10)
                                                {
                                                    entity3.ENTRY_QNT = entity3.ENTRY_QNT + MDCZ;
                                                }
                                                else if (MDCZ < 0)
                                                {
                                                    entity3.ENTRY_QNT = entity3.ENTRY_QNT + MDCZ;
                                                }
                                            }
                                            //decimal danBaoZ = Math.Round((data.MDSUTTLE / Decimal.Parse(data.MDMINAMOUNT)), 3);
                                            //if ((danBaoZ * Decimal.Parse(data.MINAMOUNT)) < (MDCZ - 10))
                                            //{
                                            //    entity3.ENTRY_QNT = data.JSSUTTLE * Decimal.Parse(data.JSAMOUNT1);
                                            //}
                                            //else if ((danBaoZ * Decimal.Parse(data.MINAMOUNT)) >= (MDCZ - 10))
                                            //{
                                            //    entity3.ENTRY_QNT = MDCZ;
                                            //}
                                            if (MDCZ != 0)
                                            {
                                                Db.Ado.ExecuteCommand($"update APP_HANDORDER set ISMJ=1,JZXSUTTLE='{entity3.ENTRY_QNT}'  where ID='{data.ID}'");
                                            }
                                            #endregion
                                        }
                                        ret = SAP_POSTZS2(entity3, "");
                                    }
                                    else
                                    {
                                        //过账执行
                                        #region 化积浆业务先不上传
                                        ret.STATUS = "E";
                                        ret.REMSG = "化积浆业务先不上传!";
                                        redata.MATDOCUMENTYEAR = "";
                                        redata.MATERIALDOCUMENT = "";
                                        redata.PRUEFLOS = "";
                                        redata.E_POSNR = "";
                                        ret.REDATA = redata;
                                        #endregion
                                    }
                                }
                                else if (MATERIALBARCODE == "318")
                                {
                                    if (string.IsNullOrEmpty(BILLNO))
                                    {
                                        #region 化积浆业务先不上传
                                        ret.STATUS = "E";
                                        ret.REMSG = "化积浆业务先不上传!";
                                        redata.MATDOCUMENTYEAR = "";
                                        redata.MATERIALDOCUMENT = "";
                                        redata.PRUEFLOS = "";
                                        redata.E_POSNR = "";
                                        ret.REDATA = redata;
                                        #endregion
                                    }
                                    else
                                    {
                                        entity.HEADER_TXT = "无人计量化积浆采购过账";
                                        ret = SAP_POSTZS(entity, "");
                                    }
                                }
                                else if (MATERIALBARCODE == "312")
                                {
                                    //if (string.IsNullOrEmpty(BILLNO))
                                    //{
                                    //    #region 进口废纸业务先不上传
                                    //    ret.STATUS = "E";
                                    //    ret.REMSG = "进口废纸业务先不上传!";
                                    //    redata.MATDOCUMENTYEAR = "";
                                    //    redata.MATERIALDOCUMENT = "";
                                    //    redata.PRUEFLOS = "";
                                    //    redata.E_POSNR = "";
                                    //    ret.REDATA = redata;
                                    //    #endregion
                                    //}
                                    //else
                                    //{
                                    if (data.ISTYPE == "0" && data.ISBATCH == "1")
                                    {
                                        #region 按实重质检扣重业务先不上传
                                        ret.STATUS = "E";
                                        ret.REMSG = "按实重质检扣重业务先不上传!";
                                        redata.MATDOCUMENTYEAR = "";
                                        redata.MATERIALDOCUMENT = "";
                                        redata.PRUEFLOS = "";
                                        redata.E_POSNR = "";
                                        ret.REDATA = redata;
                                        #endregion
                                    }
                                    entity.BILL_OF_LADING = data.TDBILLNO;
                                    entity.GR_GI_SLIP_NO = data.TDBILLNO;
                                    //过账执行
                                    ret = SAP_POSTZS(entity, "");
                                    //}
                                }
                                else
                                {
                                    if (string.IsNullOrEmpty(BILLNO))
                                    {
                                        #region 其它业务先不上传
                                        ret.STATUS = "E";
                                        ret.REMSG = "其它业务先不上传!";
                                        redata.MATDOCUMENTYEAR = "";
                                        redata.MATERIALDOCUMENT = "";
                                        redata.PRUEFLOS = "";
                                        redata.E_POSNR = "";
                                        ret.REDATA = redata;
                                        #endregion
                                    }
                                    else
                                    {
                                        //过账执行
                                        ret = SAP_POSTZS(entity, "");
                                    }
                                }
                                #endregion
                            }
                            #endregion
                        }
                    }
                }
                if (ret.STATUS == "S")
                {
                    DataCache.RemoveAllCache(key);
                    successnum++;
                    log.Error($"二次计量推送:过磅号{data.BILLNO}成功,凭证编号{ret.REDATA.MATERIALDOCUMENT}！");
                    Db.Ado.ExecuteCommand($"update APP_HANDORDER set UPLOAD=1,PZBILLNO='{ret.REDATA.MATERIALDOCUMENT}',PZROWNO='{ret.REDATA.E_POSNR}',PZDATE='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',ZJBILLNO='{ret.REDATA.PRUEFLOS}'  where ID='{data.ID}'");
                    if (data.BILLTYPE == "YG15" || MATERIALBARCODE == "311")
                    {
                        if (data.UPLOAD1 == "0")
                        {
                            #region  货物移库参数

                            //ENTRY_QNT = data.JSSUTTLE;

                            //if (ENTRY_QNT > 0)
                            //{
                            //    entity2.GM_CODE = "04";
                            //    entity2.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                            //    entity2.MOVE_TYPE = "311";//移动类型
                            //    entity2.ENTRY_UOM = "KG";//计量单位
                            //    entity2.MATERIAL = pk_material;
                            //    entity2.ENTRY_QNT = ENTRY_QNT;
                            //    entity2.ENTRY_QNTSpecified = true;
                            //    entity2.STGE_LOC = data.PK_STORE;//发货仓库
                            //    entity2.BATCH = data.BATCHNO;//原批次
                            //    entity2.MOVE_STLOC = data.PK_STOREQX;// "Y502";//收货仓库
                            //    entity2.MOVE_BATCH = data.PK_STOREQX;//现批次
                            //    entity2.PLANT = data.PK_RECEIVEORG;//原工厂
                            //    entity2.MOVE_PLANT = data.PK_RECEIVEORG;//收货工厂 
                            //    entity2.HEADER_TXT = "验包上料移库";
                            //    //生产订单、311数据货物移库
                            //    ret1 = SAP_GMPOST4ZS(entity2, "");
                            //    if (ret1.STATUS == "S")
                            //    {
                            //        //log.Error(string.Format("废纸抽包计量移库推送:过磅号{0}推送成功！", data.BILLNO));
                            //        Db.Ado.ExecuteCommand($"update APP_HANDORDER set UPLOAD1=1,PZBILLNO1='{ret1.REDATA.MATERIALDOCUMENT}',PZROWNO1='{ret1.REDATA.E_POSNR}',PZDATE1='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'  where ID='{data.ID}'");
                            //        Db.Ado.ExecuteCommand($"update WB_WEIGHT_XB set UPLOAD1=1,PZBILLNO1='{ret1.REDATA.MATERIALDOCUMENT}',PZROWNO1='{ret1.REDATA.E_POSNR}',PZDATE1='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'  where PK_ORDER_B='{data.ID}'");
                            //    }
                            //}
                            #endregion
                        }
                    }
                }
                else
                {
                    Db.Ado.ExecuteCommand($"update APP_HANDORDER set ISERROR=1,PZDATE='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'  where ID='{data.ID}'");
                    sql = IF_LOG(data.BILLNO, "WRZS_SECONDWEIGHTZS", ret.REMSG);
                    Db.Ado.ExecuteCommand(sql);
                    log.Error($"二次计量推送: 过磅号{data.BILLNO}失败,原因{ret.REMSG}！");
                }
            }
            string msg = $"本次推送车数：{list.Count()},成功{successnum}条！{ ret.REMSG}";
            log.Error(msg);
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }


        private string IF_LOG(string BILLNO, string IF_CODE, string REMSG)
        {
            string sql = "";
            sql = $"insert into IF_LOG(ID,BILL_NO,IF_NAME,IF_REQUEST,IF_RESPOND,IF_MSG,BEGIN_TIME,END_TIME,IF_RESTIME,IF_IN_SATU,IF_TYPE,CRETIME,DY_OS,SS_OS,IF_CODE) values('{CommonHelper.GetGuid}','{BILLNO}','采购二次计量推送','失败','','{ REMSG}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',0,'失败','推送','{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}','MQIS','C','{IF_CODE}')";
            return sql;
        }


        /// <summary>
        /// 生产机：无人值守抽包计量移库接口
        /// </summary>
        /// <param name="materialtype">物料分类</param>
        /// <returns>为空时所有分类</returns>
        [WebMethod(Description = "生产机：无人值守抽包计量移库接口")]
        public RETURN_SAP WRZS_SECONDCBWEIGHTZS(string materialtype, string BILLNO)
        {
            RETURN_SAP ret = new RETURN_SAP();
            ret.STATUS = "E";
            REDATA redata = new REDATA();
            redata.MATDOCUMENTYEAR = "";
            redata.MATERIALDOCUMENT = "";
            redata.PRUEFLOS = "";
            ret.REDATA = redata;
            if (string.IsNullOrEmpty(BILLNO))
            {
                #region 是否启用
                BASE_SYSTEMSET config = Db.Queryable<BASE_SYSTEMSET>()
           .Where(c => c.FIELDNAME == "WRZS_SECONDCBWEIGHTZS")
           .Take(1).First();

                if (config != null)
                {
                    if (config.FVALUE == "0")
                    {
                        ret.STATUS = "E";
                        ret.REMSG = "小磅抽包移库接口停用，请联系管理员启用接口！";
                        redata.MATDOCUMENTYEAR = "";
                        redata.MATERIALDOCUMENT = "";
                        redata.PRUEFLOS = "";
                        redata.E_POSNR = "";
                        ret.REDATA = redata;
                        return ret;
                    }
                }
                #endregion
            }

            #region 查询数据
            var list = Db.Queryable<VAPP_HANDORDER_SECOND>()

               .Where(c => c.TYPE == "5")
               .Where(c => c.UPLOAD1 == "0")
                .Where(c => c.UPLOAD == "1")
                .WhereIF(string.IsNullOrEmpty(BILLNO), c => c.ISERROR1 == "0")
               .Where(c => c.JSSUTTLE > 0)
               .Where(c => c.STATUS == "1")
               .WhereIF(!string.IsNullOrEmpty(BILLNO), c => c.BILLNO == BILLNO)
               .WhereIF(!string.IsNullOrEmpty(materialtype), c => c.PK_PARENT == materialtype)
              .OrderBy(c => c.WEIGHTDATE).ToPageList(1, 2);
            #endregion

            EOS_Z_GM_POST entity = new EOS_Z_GM_POST();
            int successnum = 0;
            int errornum = 0;

            string MATERIALBARCODE = "";
            string sql = "";
            string key = "";
            foreach (var data in list)
            {
                key = $"{data.ID}YK";
                if (DataCache.IsExist(key) == true)
                {
                    ret.STATUS = "E";
                    ret.REMSG = "当前数据已移库！";
                    redata.MATDOCUMENTYEAR = "";
                    redata.MATERIALDOCUMENT = "";
                    redata.PRUEFLOS = "";
                    redata.E_POSNR = "";
                    ret.REDATA = redata;
                }
                else if (!string.IsNullOrEmpty(data.PZBILLNO1) || data.UPLOAD1 == "1")
                {
                    ret.STATUS = "E";
                    ret.REMSG = $"当前数据已移库{data.PZBILLNO1}！";
                    redata.MATDOCUMENTYEAR = "";
                    redata.MATERIALDOCUMENT = "";
                    redata.PRUEFLOS = "";
                    redata.E_POSNR = "";
                    ret.REDATA = redata;
                }
                else
                {
                    //创建执行缓存,2分种有效期
                    DataCache.Insert(key, data.ID, 2);
                    #region  调用接口
                    entity.ENTRY_QNT = data.JSSUTTLE;
                    if (string.IsNullOrEmpty(data.GPMATERIAL))
                    {
                        entity.MATERIAL = data.PK_MATERIAL;
                    }
                    else
                    {
                        entity.MATERIAL = data.GPMATERIAL;
                    }
                    if (data.PK_STORE == data.PK_STOREQX)
                    {
                        log.Error(string.Format("废纸抽包计量移库推送:过磅号{0},仓库相同。", data.BILLNO));
                        ret.STATUS = "S";
                        ret.REMSG = "发货仓库和收货仓库不能相同！";
                    }
                    else if (string.IsNullOrEmpty(entity.MATERIAL))
                    {
                        ret.REMSG = "物料编号为空！";
                    }
                    else if (entity.ENTRY_QNT == 0)
                    {
                        ret.REMSG = "结算重量为0";
                    }
                    else
                    {
                        #region 结算量大于0
                        entity.GM_CODE = "04";
                        entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                        entity.MOVE_TYPE = "311";//移动类型
                        entity.ENTRY_UOM = "KG";//计量单位
                        entity.ENTRY_QNTSpecified = true;
                        entity.STGE_LOC = data.PK_STORE;//发货仓库
                        entity.BATCH = data.BATCHNO;//原批次
                        entity.MOVE_STLOC = data.PK_STOREQX;// "Y502";//收货仓库
                        entity.MOVE_BATCH = data.PK_STOREQX;//现批次
                        entity.PLANT = data.PK_RECEIVEORG;//原工厂
                        entity.MOVE_PLANT = data.PK_RECEIVEORG;//收货工厂 
                        entity.HEADER_TXT = "验包上料移库";
                        MATERIALBARCODE = data.MATERIALBARCODE;
                        ret = SAP_GMPOST4ZS(entity, "");
                        #endregion
                    }
                    if (ret.STATUS == "S")
                    {
                        successnum++;
                        log.Error(string.Format("废纸抽包计量移库推送:过磅号{0}推送成功！", data.BILLNO));
                        Db.Ado.ExecuteCommand($"update APP_HANDORDER set UPLOAD1=1,PZBILLNO1='{ret.REDATA.MATERIALDOCUMENT}',PZROWNO1='{ret.REDATA.E_POSNR}',PZDATE1='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'  where ID='{data.ID}'");
                        Db.Ado.ExecuteCommand($"update WB_WEIGHT_XB set UPLOAD1=1,PZBILLNO1='{ret.REDATA.MATERIALDOCUMENT}',PZROWNO1='{ret.REDATA.E_POSNR}',PZDATE1='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}'  where PK_ORDER_B='{data.ID}'");
                    }
                    else
                    {
                        errornum++;
                        Db.Ado.ExecuteCommand($"update APP_HANDORDER set ISERROR1=1  where ID='{data.ID}'");
                        sql = IF_LOG(data.BILLNO, "WRZS_SECONDCBWEIGHTZS", ret.REMSG);
                        Db.Ado.ExecuteCommand(sql);
                        log.Error(string.Format("废纸抽包计量移库推送:过磅号{0}推送失败！,原因{1}", data.BILLNO, ret.REMSG));
                    }
                    #endregion
                }
            }
            string msg = string.Format("本次推送车数：{0},成功{1}条，失败{2}条！", list.Count(), successnum, errornum);
            log.Error(msg);
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }

        /// <summary>
        /// 生产机：无人值守小磅计量移库接口
        /// </summary>
        /// <param name="materialtype">物料分类</param>
        /// <returns>为空时所有分类</returns>
        [WebMethod(Description = "生产机：无人值守小磅计量移库接口")]
        public RETURN_SAP WRZS_SECONDXBWEIGHTZS(string materialtype, string BILLNO)
        {
            RETURN_SAP ret = new RETURN_SAP();
            ret.STATUS = "E";
            #region 是否有效
            if (string.IsNullOrEmpty(BILLNO))
            {
                BASE_SYSTEMSET config = Db.Queryable<BASE_SYSTEMSET>()
      .Where(c => c.FIELDNAME == "WRZS_SECONDXBWEIGHTZS")
      .Take(1).First();
                if (config != null)
                {
                    if (config.FVALUE == "0")
                    {
                        ret.STATUS = "E";
                        ret.REMSG = "小磅上料移库接口停用，请联系管理员启用接口！";
                        REDATA redata = new REDATA();
                        redata.MATDOCUMENTYEAR = "";
                        redata.MATERIALDOCUMENT = "";
                        redata.PRUEFLOS = "";
                        redata.E_POSNR = "";
                        ret.REDATA = redata;
                        return ret;
                    }
                }
            }
            #endregion
            //Db.DbFirst.Where("VWB_WEIGHT_XB").CreateClassFile("D:\\Demo\\2", "Models");
            

            #region 查询数据
            var list = Db.Queryable<VWB_WEIGHT_XB>()
               // .Where(c => c.TYPE == "1")
               .Where(c => c.UPLOAD1 == "0")
              .Where(c => c.COMPUTERTYPE == "0")
               .Where(c => c.JSSUTTLE > 0)
                .WhereIF(string.IsNullOrEmpty(BILLNO), c => c.ISERROR1 == "0")
               .WhereIF(!string.IsNullOrEmpty(BILLNO), c => c.BILLNO == BILLNO)
               .WhereIF(!string.IsNullOrEmpty(materialtype), c => c.PK_PARENT == materialtype)
              .OrderBy(c => c.WEIGHTDATE).ToPageList(1, 2);
            #endregion

            EOS_Z_GM_POST entity = new EOS_Z_GM_POST();

            //log.Error("小磅计量移库推送:开始");
            //log.Error(string.Format("小磅计量移库推送:明细记录数{0}", list.Count()));
            int successnum = 0;
            int errornum = 0;
            string sql = "";
            string key = "";
            foreach (VWB_WEIGHT_XB data in list)
            {
                key = $"{data.BILLNO}";
                // log.Error($"小磅计量推送:过磅号{data.BILLNO}！");
                if (DataCache.IsExist(key) == true)
                {
                    ret.STATUS = "E";
                    ret.REMSG = "当前数据已移库！";
                    ret.REDATA = null;
                }
                else
                {
                    //创建执行缓存,2分种有效期
                    DataCache.Insert(key, data.BILLNO, 2);
                    #region 移库推送
                    //log.Error(string.Format("小磅计量移库推送:过磅号{0}", data.BILLNO));
                    #region  初始化
                    entity.GM_CODE = "04";
                    entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                    entity.MOVE_TYPE = "311";//移动类型
                    entity.ENTRY_UOM = "KG";//计量单位
                    entity.MATERIAL = data.PK_MATERIAL;
                    entity.ENTRY_QNT = data.JSSUTTLE;
                    entity.ENTRY_QNTSpecified = true;
                    entity.STGE_LOC = data.PK_RECEIVESTORE;//发货仓库
                    entity.BATCH = data.BATHNO;//原批次
                    entity.MOVE_STLOC = data.PK_RECEIVESTORE1;// "Y502";//收货仓库
                    entity.MOVE_BATCH = data.RECBATCHNO;//现批次
                    entity.PLANT = data.PK_RECEIVEORG;//原工厂
                    entity.MOVE_PLANT = data.PK_RECEIVEORG;//收货工厂 
                    entity.HEADER_TXT = "生产上料移库";
                    #endregion
                    if (!string.IsNullOrEmpty(entity.MATERIAL))
                    {
                        ret = SAP_GMPOST4ZS(entity, "");
                    }
                    else
                    {
                        ret.REMSG = "物料不能为空!";
                    }

                    if (ret.STATUS == "S" && ret.REDATA.MATDOCUMENTYEAR != "0000")
                    {
                        successnum++;
                        log.Error(string.Format("小磅计量移库推送:过磅号{0}推送成功！", data.BILLNO));
                        Db.Ado.ExecuteCommand($"update WB_WEIGHT_XB set UPLOAD1=1,PZBILLNO1='{ret.REDATA.MATERIALDOCUMENT}',PZROWNO1='{ret.REDATA.E_POSNR}',PZDATE1='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',PDAKZ2={(data.PDAKZ2 != null ? data.PDAKZ2 : "0")},YFPIECE={data.JSSUTTLE}  where BILLNO='{data.BILLNO}'");
                    }
                    else
                    {
                        errornum++;
                        Db.Ado.ExecuteCommand($"update WB_WEIGHT_XB set ISERROR1=1  where BILLNO='{data.BILLNO}'");
                        sql = IF_LOG(data.BILLNO, "WRZS_SECONDXBWEIGHTZS", ret.REMSG);
                        Db.Ado.ExecuteCommand(sql);
                        log.Error(string.Format("小磅计量移库推送:过磅号{0}推送失败！原因{1}", data.BILLNO, ret.REMSG));
                    }
                    #endregion
                }
            }
            string msg = string.Format("本次推送车数：{0},成功{1}条，失败{2}条！", list.Count(), successnum, errornum);
            log.Error(msg);
            //log.Error("小磅计量移库推送:结束");
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            //木浆
            RETURN_SAP ret_mj = new RETURN_SAP();
            ret_mj.STATUS = "E";
            ret_mj = WRZS_SECONDXBMJWEIGHTZS(materialtype, BILLNO);
            ret.REMSG = $"{ret.REMSG}、木浆：{ret_mj.REMSG}";
            //破包
            RETURN_SAP ret_pb = new RETURN_SAP();
            ret_pb.STATUS = "E";
            ret_pb = WRZS_SECONDXBPBWEIGHTZS(materialtype, BILLNO);
            ret.REMSG = $"{ret.REMSG}、破包：{ret_pb.REMSG}";

            return ret;
        }


        /// <summary>
        /// 生产机：无人值守木浆移库接口
        /// </summary>
        /// <param name="materialtype">物料分类</param>
        /// <returns>为空时所有分类</returns>
        [WebMethod(Description = "生产机：无人值守木浆移库接口")]
        public RETURN_SAP WRZS_SECONDXBMJWEIGHTZS(string materialtype, string BILLNO)
        {
            RETURN_SAP ret = new RETURN_SAP();
            ret.STATUS = "E";
            #region 是否有效
            if (string.IsNullOrEmpty(BILLNO))
            {
                BASE_SYSTEMSET config = Db.Queryable<BASE_SYSTEMSET>()
      .Where(c => c.FIELDNAME == "WRZS_SECONDXBMJWEIGHTZS")
      .Take(1).First();
                if (config != null)
                {
                    if (config.FVALUE == "0")
                    {
                        ret.STATUS = "E";
                        ret.REMSG = "木浆上料移库接口停用，请联系管理员启用接口！";
                        REDATA redata = new REDATA();
                        redata.MATDOCUMENTYEAR = "";
                        redata.MATERIALDOCUMENT = "";
                        redata.PRUEFLOS = "";
                        redata.E_POSNR = "";
                        ret.REDATA = redata;
                        return ret;
                    }
                }
            }
            #endregion

            #region 查询数据
            var list = Db.Queryable<VWB_WEIGHT_XB1>()
               .Where(c => c.UPLOAD1 == "0")
              .Where(c => c.COMPUTERTYPE == "0")
               .Where(c => c.JSSUTTLE > 0)
                .WhereIF(string.IsNullOrEmpty(BILLNO), c => c.ISERROR1 == "0")
               .WhereIF(!string.IsNullOrEmpty(BILLNO), c => c.BILLNO == BILLNO)
               .WhereIF(!string.IsNullOrEmpty(materialtype), c => c.PK_PARENT == materialtype)
              .OrderBy(c => c.WEIGHTDATE).ToPageList(1, 2);
            #endregion

            EOS_Z_GM_POST entity = new EOS_Z_GM_POST();

            int successnum = 0;
            int errornum = 0;
            string sql = "";
            foreach (var data in list)
            {
                #region  初始化
                entity.GM_CODE = "04";
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                entity.MOVE_TYPE = "311";//移动类型
                entity.ENTRY_UOM = "KG";//计量单位
                entity.MATERIAL = data.PK_MATERIAL;
                entity.ENTRY_QNT = data.JSSUTTLE;
                entity.ENTRY_QNTSpecified = true;
                entity.STGE_LOC = data.PK_RECEIVESTORE;//发货仓库
                entity.BATCH = data.BATHNO;//原批次
                entity.MOVE_STLOC = data.PK_RECEIVESTORE1;// "Y502";//收货仓库
                entity.MOVE_BATCH = data.RECBATCHNO;//现批次
                entity.PLANT = data.PK_RECEIVEORG;//原工厂
                entity.MOVE_PLANT = data.PK_RECEIVEORG;//收货工厂 
                entity.HEADER_TXT = "生产木浆上料移库";
                #endregion
                if (!string.IsNullOrEmpty(entity.MATERIAL))
                {
                    ret = SAP_GMPOST4ZS(entity, "");
                }
                else
                {
                    ret.REMSG = "物料不能为空!";
                }

                if (ret.STATUS == "S" && ret.REDATA.MATDOCUMENTYEAR != "0000")
                {
                    successnum++;
                    log.Error(string.Format("木浆上料移库推送:过磅号{0}推送成功！", data.BILLNO));
                    Db.Ado.ExecuteCommand($"update WB_WEIGHT_XB set UPLOAD1=1,PZBILLNO1='{ret.REDATA.MATERIALDOCUMENT}',PZROWNO1='{ret.REDATA.E_POSNR}',PZDATE1='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',PDAKZ2={(data.PDAKZ2 != null ? data.PDAKZ2 : "0")},YFPIECE={data.JSSUTTLE}  where BILLNO='{data.BILLNO}'");
                }
                else
                {
                    errornum++;
                    Db.Ado.ExecuteCommand($"update WB_WEIGHT_XB set ISERROR1=1  where BILLNO='{data.BILLNO}'");
                    sql = IF_LOG(data.BILLNO, "WRZS_SECONDXBMJWEIGHTZS", ret.REMSG);
                    Db.Ado.ExecuteCommand(sql);
                    log.Error(string.Format("木浆上料移库推送:过磅号{0}推送失败！原因{1}", data.BILLNO, ret.REMSG));
                }
            }
            string msg = string.Format("本次推送车数：{0},成功{1}条，失败{2}条！", list.Count(), successnum, errornum);
            log.Error(msg);
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }


        /// <summary>
        /// 生产机：无人值守破包、化工移库接口移库接口
        /// </summary>
        /// <param name="materialtype">物料分类</param>
        /// <returns>为空时所有分类</returns>
        [WebMethod(Description = "生产机：无人值守破包、化工移库接口")]
        public RETURN_SAP WRZS_SECONDXBPBWEIGHTZS(string materialtype, string BILLNO)
        {
            RETURN_SAP ret = new RETURN_SAP();
            ret.STATUS = "E";
            #region 是否有效
            if (string.IsNullOrEmpty(BILLNO))
            {
                BASE_SYSTEMSET config = Db.Queryable<BASE_SYSTEMSET>()
      .Where(c => c.FIELDNAME == "WRZS_SECONDXBPBWEIGHTZS")
      .Take(1).First();
                if (config != null)
                {
                    if (config.FVALUE == "0")
                    {
                        ret.STATUS = "E";
                        ret.REMSG = "破包上料移库接口停用，请联系管理员启用接口！";
                        REDATA redata = new REDATA();
                        redata.MATDOCUMENTYEAR = "";
                        redata.MATERIALDOCUMENT = "";
                        redata.PRUEFLOS = "";
                        redata.E_POSNR = "";
                        ret.REDATA = redata;
                        return ret;
                    }
                }
            }
            #endregion

            #region 查询数据
            var list = Db.Queryable<VWB_WEIGHT_XBYK>()
               .Where(c => c.UPLOAD1 == "0")
              .Where(c => c.COMPUTERTYPE == "0")
               // .Where(c => c.TYPE == "6")
               .Where(c => c.JSSUTTLE > 0)
                .WhereIF(string.IsNullOrEmpty(BILLNO), c => c.ISERROR1 == "0")
               .WhereIF(!string.IsNullOrEmpty(BILLNO), c => c.BILLNO == BILLNO)
               .WhereIF(!string.IsNullOrEmpty(materialtype), c => c.PK_PARENT == materialtype)
              .OrderBy(c => c.WEIGHTDATE).ToPageList(1, 2);
            #endregion

            EOS_Z_GM_POST entity = new EOS_Z_GM_POST();

            int successnum = 0;
            int errornum = 0;
            string sql = "";
            foreach (var data in list)
            {
                #region  初始化
                entity.GM_CODE = "04";
                entity.DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                entity.MOVE_TYPE = "311";//移动类型
                entity.ENTRY_UOM = "KG";//计量单位
                entity.MATERIAL = data.PK_MATERIAL;
                entity.ENTRY_QNT = data.JSSUTTLE;
                entity.ENTRY_QNTSpecified = true;
                entity.STGE_LOC = data.PK_RECEIVESTORE;//发货仓库
                entity.BATCH = data.BATHNO;//原批次
                entity.MOVE_STLOC = data.PK_RECEIVESTORE1;// "Y502";//收货仓库
                entity.MOVE_BATCH = data.RECBATCHNO;//现批次
                entity.PLANT = data.PK_RECEIVEORG;//原工厂
                entity.MOVE_PLANT = data.PK_RECEIVEORG;//收货工厂 
                if (data.TYPE == "6")
                {
                    entity.HEADER_TXT = "生产破包上料移库";
                }
                else
                {
                    entity.HEADER_TXT = "生产化工上料移库";
                }

                #endregion
                if (!string.IsNullOrEmpty(entity.MATERIAL))
                {
                    ret = SAP_GMPOST4ZS(entity, "");
                }
                else
                {
                    ret.REMSG = "物料不能为空!";
                }

                if (ret.STATUS == "S" && ret.REDATA.MATDOCUMENTYEAR != "0000")
                {
                    successnum++;
                    log.Error(string.Format("破包上料移库推送:过磅号{0}推送成功！", data.BILLNO));
                    Db.Ado.ExecuteCommand($"update WB_WEIGHT_XB set UPLOAD1=1,PZBILLNO1='{ret.REDATA.MATERIALDOCUMENT}',PZROWNO1='{ret.REDATA.E_POSNR}',PZDATE1='{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',PDAKZ2={(data.PDAKZ2 != null ? data.PDAKZ2 : "0")},YFPIECE={data.JSSUTTLE}  where BILLNO='{data.BILLNO}'");
                }
                else
                {
                    errornum++;
                    Db.Ado.ExecuteCommand($"update WB_WEIGHT_XB set ISERROR1=1  where BILLNO='{data.BILLNO}'");
                    sql = IF_LOG(data.BILLNO, "WRZS_SECONDXBMJWEIGHTZS", ret.REMSG);
                    Db.Ado.ExecuteCommand(sql);
                    log.Error(string.Format("破包上料移库推送:过磅号{0}推送失败！原因{1}", data.BILLNO, ret.REMSG));
                }
            }
            string msg = string.Format("本次推送车数：{0},成功{1}条，失败{2}条！", list.Count(), successnum, errornum);
            log.Error(msg);
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }

        /// <summary>
        /// 生产机：无人值守采购磅单：化工、包装物一次计量推送接口
        /// </summary>
        /// <param name="materialtype">化工:YG40、包装物:YG60:</param>
        /// <returns>物料类型为空时默认化工YG40推送。</returns>
        [WebMethod(Description = "生产机：无人值守采购磅单：化工、包装物一次计量推送接口")]
        public RETURN_SAP WRZS_HGBZWFINISTWEIGHTZS_EXECZJ(string materialtype, string PCBILLNO)
        {
            RETURN_SAP ret = new RETURN_SAP();
            if (string.IsNullOrEmpty(materialtype))
            {
                materialtype = "YG40";//默认化工
            }

            if (string.IsNullOrEmpty(PCBILLNO))
            {
                ret.STATUS = "E";
                ret.REMSG = "派车单号不能为空！";
                ret.REDATA = null;
                return ret;
            }

            var list = Db.Queryable<VWB_WEIGHT_FINISHSH>()
                .Where(c => c.MATERIALTYPE == materialtype)
                .Where(c => c.PK_ORDER_B == PCBILLNO)
              .OrderBy(c => c.WEIGHTDATE).ToList();


            EOS_Z_PO_POST entity = new EOS_Z_PO_POST();
            #region 初始数据

            string DOC_DATE = DateTime.Now.ToString("yyyy-MM-dd");
            string PSTNG_DATE = "";
            string PO_NUMBER = "";
            string PO_ITEM = "";
            string MATERIAL = "";
            string MATERIALNAME = "";
            string MATERIALTYPE = "";
            string SUPPLY = "";
            string SUPPLYNAME = "";
            string BATCH = "";
            decimal ENTRY_QNT = 0;
            string STGE_LOC = "";
            string PLANT = "";
            string JLBILLNO = "";
            double GROSS = 0.0;
            string MATBATCHNO = "";
            string PZBILLNO = "";
            string ZJBILLNO = "";
            string WGTYPE = "";
            string ISZJ = "";
            string ZJRESULT = "";
            string ZJRESULT1 = "";
            #endregion
            //log.Error("一次计量推送质检:开始");
            //log.Error(string.Format("一次计量推送质检:明细记录数{0}", list.Count()));
            int successnum = 0;
            foreach (var data in list)
            {
                //log.Error(string.Format("一次计量推送质检:过磅号{0}", data.BILLNO));
                if (!string.IsNullOrEmpty(data.WEIGHTDATE))
                {
                    PSTNG_DATE = Convert.ToDateTime(data.WEIGHTDATE).ToString("yyyy-MM-dd");
                }
                else
                {
                    PSTNG_DATE = DateTime.Now.ToString("yyyy-MM-dd");
                }

                #region 初始化值
                JLBILLNO = data.BILLNO;
                PO_NUMBER = data.PK_BILL;
                PO_ITEM = data.PK_BILL_B.Replace(data.PK_BILL, "");
                MATERIAL = data.PK_MATERIAL;
                MATERIALNAME = data.MATERIAL;
                MATERIALTYPE = data.MATERIALTYPE;
                SUPPLY = data.PK_SENDORG;
                SUPPLYNAME = data.SENDORG;
                BATCH = data.MATBATCHNO;
                ENTRY_QNT = data.GROSS;
                //STGE_LOC = data.PK_STORE; // 此字段为空弃置不用  新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
                PLANT = data.PK_RECEIVEORG;
                GROSS = Convert.ToDouble(data.GROSS);
                MATBATCHNO = data.MATBATCHNO;
                PZBILLNO = data.PZBILLNO;
                ZJBILLNO = data.ZJBILLNO;
                WGTYPE = data.WGTYPE;
                ISZJ = data.ISZJ;
                ZJRESULT = data.ZJRESULT;
                ZJRESULT1 = data.ZJRESULT1;
                #endregion
                //调用质检计划
                // 新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
                string TEL = ""; string PK_STORE = "";
                var addhgfirst = Db.Queryable<VAPP_HANDORDER_ADDHGFIRST>().Where(c => c.BILLNO == data.BILLNO).ToList();
                if (addhgfirst.Count > 0)
                {
                    PK_STORE = addhgfirst.First().PK_STORE;
                    TEL = addhgfirst.First().TEL;
                }
                // 新增化工辅料一次收货传输司机电话及收货仓库到SAP  myt/liuyan 221009
                ret = MES_PURCHASEZS1(PLANT, PO_NUMBER, PO_ITEM, ZJBILLNO, MATERIAL, MATERIALNAME, JLBILLNO, MATBATCHNO, GROSS, "", PSTNG_DATE, "计量生成", "计量生成", SUPPLY, SUPPLYNAME, WGTYPE, ISZJ, MATERIALTYPE, PZBILLNO, TEL, PK_STORE);
                if (ret.STATUS == "S")
                {
                    successnum++;
                    //log.Error(string.Format("一次计量推送质检:预约单明细{0}推送成功！", data.PK_ORDER_B));
                }
                else
                {
                    //log.Error(string.Format("一次计量推送质检:过磅号{0}推送失败！", data.BILLNO));
                }
            }
            string msg = string.Format("本次推送车数：{0},成功{1}条！", list.Count(), successnum);
            //log.Error(msg);
            //log.Error("一次计量推送质检:结束");
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }


        #endregion

        #region 生产机：无人值守调用MES接口
        /// <summary>
        /// 生产机：无人值守上料磅单MES推送接口
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "生产机：无人值守上料磅单MES推送接口")]
        public RETURN_SAP WRZS_MESSLWEIGHTZS(string BILLNO)
        {
            RETURN_SAP ret = new RETURN_SAP();
            #region 是否有效
            if (string.IsNullOrEmpty(BILLNO))
            {
                BASE_SYSTEMSET config = Db.Queryable<BASE_SYSTEMSET>()
             .Where(c => c.FIELDNAME == "WRZS_MESSLWEIGHTZS")
             .Take(1).First();

                if (config != null)
                {
                    if (config.FVALUE == "0")
                    {
                        ret.STATUS = "E";
                        ret.REMSG = "上料磅单MES推送接口停用，请联系管理员启用接口！";
                        REDATA redata = new REDATA();
                        redata.MATDOCUMENTYEAR = "";
                        redata.MATERIALDOCUMENT = "";
                        redata.PRUEFLOS = "";
                        redata.E_POSNR = "";
                        ret.REDATA = redata;
                        return ret;
                    }
                }
            }
            #endregion
            #region 查询数据 
            var list = Db.Queryable<VWB_WEIGHT_XB>()
                 //时间条件
                 //.Where($"CREATEDATE>='2021-10-15 00:00:00' and CREATEDATE<='2021-10-15 23:59:59'")
                 //.Where(c => c.FINISH == "1")
                 .Where(c => c.UPLOAD == "0")
                 .WhereIF(string.IsNullOrEmpty(BILLNO), c => c.ISERROR == "0")
                 .Where(c => c.JSSUTTLE > 0)
                 .Where(c => c.TYPE == "1")
                 //.Where(c => c.STORE == "链板")
                 .Where(c => c.SUTTLE > 0)
                 .WhereIF(!string.IsNullOrEmpty(BILLNO), c => c.BILLNO == BILLNO)
              .OrderBy(c => c.WEIGHTDATE).ToPageList(1, 2);
            #endregion
            EOS_SLWEIGHT entity = new EOS_SLWEIGHT();

            #region 初始数据
            string SITE = "1000";
            string WORKSHOP = "";
            string STORAGELOCATION = "";
            string ITEM = "";
            string BATCH = "";
            string QTY = "0";
            string TRANSFERTYPE = "IN";
            string DATETIME = DateTime.Now.ToString("yyyy-MM-dd");
            #endregion

            ////log.Error("MES上料计量推送:开始");
            ////log.Error(string.Format("MES上料计量推送:明细记录数{0}", list.Count()));
            int successnum = 0;
            string sql = "";
            foreach (var data in list)
            {
                if (data.JSSUTTLE > 0)
                {
                    ////log.Error(string.Format("MES上料计量推送:过磅号{0}", data.BILLNO));
                    #region  初始化
                    ITEM = data.PK_MATERIAL;
                    if (!string.IsNullOrEmpty(data.PK_RECEIVEORG))
                    {
                        SITE = data.PK_RECEIVEORG;
                    }
                    WORKSHOP = data.PK_SENDORG;
                    STORAGELOCATION = data.PK_RECEIVESTORE1;
                    BATCH = data.RECBATCHNO;//现批号
                    QTY = string.Format("{0}", data.JSSUTTLE);
                    DATETIME = data.WEIGHTDATE;
                    if (data.STORE == "链板")
                    {
                        TRANSFERTYPE = "IN";
                    }
                    else
                    {
                        TRANSFERTYPE = "OUT";
                    }
                    entity.SITE = SITE;//站点
                    entity.WORKSHOP = WORKSHOP;//车间
                    entity.STORAGELOCATION = STORAGELOCATION;//线边仓库
                    entity.ITEM = ITEM;//物料

                    entity.BATCH = BATCH;//批号
                    entity.DATETIME = DATETIME;//时间
                    entity.QTY = QTY;//数量
                    entity.TRANSFERTYPE = TRANSFERTYPE;
                    #endregion
                    if (!string.IsNullOrEmpty(ITEM))
                    {
                        ret = MES_SLDBZS(entity, "");
                    }
                }
                else
                {
                    #region 结算量为0不上传
                    ret.STATUS = "E";
                    ret.REMSG = "结算量为0不能上传!";
                    #endregion
                }

                if (ret.STATUS == "S")
                {
                    successnum++;
                    log.Error(string.Format("MES上料计量推送:过磅号{0}推送成功！", data.BILLNO));
                    Db.Ado.ExecuteCommand($"update WB_WEIGHT_XB set UPLOAD=1  where BILLNO='{data.BILLNO}'");
                }
                else
                {
                    Db.Ado.ExecuteCommand($"update WB_WEIGHT_XB set ISERROR=1  where BILLNO='{data.BILLNO}'");
                    sql = IF_LOG(data.BILLNO, "WRZS_MESSLWEIGHTZS", ret.REMSG);
                    Db.Ado.ExecuteCommand(sql);
                    log.Error(string.Format("MES上料计量推送:过磅号{0}推送失败,原因{1}！", data.BILLNO, ret.REMSG));
                }

            }
            string msg = string.Format("本次推送车数：{0},成功{1}条！", list.Count(), successnum);
            log.Error(msg);
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }


        /// <summary>
        /// 生产机：无人值守底浆磅单MES推送接口
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "生产机：无人值守底浆磅单MES推送接口")]
        public RETURN_SAP WRZS_MESDJWEIGHTZS(string ROWNUMBER, string SITE, string BDATE, string EDATE, string PK_STORE, string MATERIAL, string SHIFT, string ITEM_MARK)
        {
            RETURN_SAP ret = new RETURN_SAP();
            if (string.IsNullOrEmpty(BDATE))
            {
                BDATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(EDATE))
            {
                EDATE = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (string.IsNullOrEmpty(SHIFT))
            {
                SHIFT = "A";
            }
            //if (string.IsNullOrEmpty(ITEM_MARK))
            //{
            //    ITEM_MARK = "1";
            //}

            #region 查询数据 
            var list = Db.Queryable<VWB_WEIGHT_XB_MES>()
             .Where($"SHIFT_DATE>='{BDATE}' and SHIFT_DATE<='{EDATE}'")
              .Where(c => c.SHIFT == SHIFT)
               .Where(c => c.UPLOAD == "0")
              .WhereIF(!string.IsNullOrEmpty(ITEM_MARK), c => c.ITEM_MARK == ITEM_MARK)
               .WhereIF(!string.IsNullOrEmpty(SITE), c => c.SITE == SITE)
             .WhereIF(!string.IsNullOrEmpty(PK_STORE), c => c.STORAGELOCATION == PK_STORE)
             .WhereIF(!string.IsNullOrEmpty(MATERIAL), c => c.ITEM == MATERIAL)
              .WhereIF(!string.IsNullOrEmpty(ROWNUMBER), c => c.ROWNUMBER == ROWNUMBER)
              .OrderBy(c => c.SHIFT_DATE).ToList();
            #endregion

            EOS_SLWEIGHT entity = new EOS_SLWEIGHT();
            int successnum = 0;
            string sql = "";
            #region 更新状态
            if (ITEM_MARK == "1")
            {
                sql = "update  app_handorder set UPLOAD4=1 where UPLOAD4=0 and ID in(select ID from vapp_handorder where iszjd = 1 and type = 5 and pk_marbasclass = '301' and nvl(def7,'0')!= '0'";
                sql += $" and ZJTIME>='{BDATE} 00:00:00' and ZJTIME<='{EDATE} 23:59:59'";
                if (SHIFT == "A")
                {
                    sql += $" and DEF7='夜班'";
                }
                else if (SHIFT == "B")
                {
                    sql += $" and DEF7='白班'";
                }
                else if (SHIFT == "C")
                {
                    sql += $" and DEF7='中班'";
                }
                if (!string.IsNullOrEmpty(SITE))
                {
                    sql += $" and PK_RECEIVEORG='{SITE}'";
                }
                if (!string.IsNullOrEmpty(PK_STORE))
                {
                    sql += $" and PK_STORE='{PK_STORE}'";
                }
                if (!string.IsNullOrEmpty(MATERIAL))
                {
                    sql += $" and (PK_MATERIAL='{MATERIAL}' or GPMATERIAL='{MATERIAL}')";
                }
                sql += ")";
            }
            else if (ITEM_MARK == "2")
            {
                sql = "update  wb_weight_xb set UPLOAD4=1 where UPLOAD4=0 and BILLNO in(select BILLNO from vwb_weight_xbcb where PK_MARBASCLASS='301'  and COMPUTERTYPE=0 and JSSUTTLE>0";
                sql += $" and WEIGHTDATE>='{BDATE} 00:00:00' and WEIGHTDATE<='{EDATE} 23:59:59'";
                if (SHIFT == "C")
                {
                    sql += $" and WEIGHTDATE>substr(WEIGHTDATE,0,10)|| ' 16:00:00'";
                }
                else if (SHIFT == "B")
                {
                    sql += $" and WEIGHTDATE<substr(WEIGHTDATE,0,10)|| ' 16:00:00'";
                }

                if (!string.IsNullOrEmpty(SITE))
                {
                    sql += $" and PK_RECEIVEORG='{SITE}'";
                }
                if (!string.IsNullOrEmpty(PK_STORE))
                {
                    sql += $" and PK_RECEIVESTORE1='{PK_STORE}'";
                }
                if (!string.IsNullOrEmpty(MATERIAL))
                {
                    sql += $" and (PK_MATERIAL='{MATERIAL}')";
                }
                sql += ")";
            }
            else if (ITEM_MARK == "3")
            {
                sql = "update  wb_weight_xb set UPLOAD4=1 where UPLOAD4=0 and BILLNO in(select BILLNO from vwb_weight_xb where type=1 and PK_MARBASCLASS = '301' and COMPUTERTYPE = 0 and JSSUTTLE> 0";
                sql += $" and WEIGHTDATE>='{BDATE} 00:00:00' and WEIGHTDATE<='{EDATE} 23:59:59'";
                if (SHIFT == "A")
                {
                    sql += $" and DEF4='夜班'";
                }
                else if (SHIFT == "B")
                {
                    sql += $" and DEF4='白班'";
                }
                else if (SHIFT == "C")
                {
                    sql += $" and DEF4='中班'";
                }

                if (!string.IsNullOrEmpty(SITE))
                {
                    sql += $" and PK_RECEIVEORG='{SITE}'";
                }
                if (!string.IsNullOrEmpty(PK_STORE))
                {
                    sql += $" and PK_RECEIVESTORE1='{PK_STORE}'";
                }
                if (!string.IsNullOrEmpty(MATERIAL))
                {
                    sql += $" and (PK_MATERIAL='{MATERIAL}')";
                }
                sql += ")";
            }
            //执行更新状态
            if (!string.IsNullOrEmpty(sql))
            {
                try
                {
                    Db.Ado.ExecuteCommand(sql);
                }
                catch
                {

                }

            }
            #endregion
            //            vapp_handorder where
            //iszjd = 1 and type = 5 and pk_marbasclass = '301' and nvl(def7,'0')!= '0'
            //and PK_RECEIVEORG = ''
            //and(GPMATERIAL = '' or PK_MATERIAL = '')
            //and DEF7 = ''
            //and BATCHNO = ''
            //and ZJTIME = ''
            //and PK_STORE = ''

            foreach (var data in list)
            {
                if (data.QTY > 0)
                {
                    #region  初始化
                    entity.SITE = data.SITE;//站点
                    entity.STORAGELOCATION = data.STORAGELOCATION;//线边仓库
                    entity.ITEM = data.ITEM;//物料
                    entity.BATCH = data.BATCH;//批号
                    entity.DATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//时间
                    entity.QTY = $"{data.QTY}";//数量
                    entity.SHIFTDATE = data.SHIFT_DATE;
                    entity.SHIFT = data.SHIFT;
                    entity.ITEMMARK = data.ITEM_MARK;
                    #endregion
                    if (!string.IsNullOrEmpty(data.ITEM))
                    {
                        ret = MES_DJ301XBSLZS1(entity, "");
                    }
                }
                else
                {
                    #region 重量为0不上传
                    ret.STATUS = "E";
                    ret.REMSG = "重量为0不能上传!";
                    #endregion
                }

                if (ret.STATUS == "S")
                {
                    if (!string.IsNullOrEmpty(sql))
                    {
                        Db.Ado.ExecuteCommand(sql);
                    }
                    successnum++;
                }
                else
                {
                    log.Error(string.Format("MES上料计量推送:推送失败,原因{0}！", ret.REMSG));
                }

            }
            string msg = string.Format("本次推送车数：{0},成功{1}条！", list.Count(), successnum);
            log.Error(msg);
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }

        /// <summary>
        /// 生产机：无人值守底浆磅单MES自动推送接口
        /// </summary>
        /// <returns></returns>
        [WebMethod(Description = "生产机：无人值守底浆磅单MES自动推送接口")]
        public RETURN_SAP WRZS_MESDJWEIGHTZDTP(string keyvalue)
        {
            RETURN_SAP ret = new RETURN_SAP();
            string BDATE = DateTime.Now.ToString("yyyy-MM-dd");
            string EDATE = DateTime.Now.ToString("yyyy-MM-dd");

            string SHIFT = "";
            string now = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//通过当前时间确定班组
            if (string.Compare(now, DateTime.Now.ToString("yyyy-MM-dd 00:00:00"), true) >= 0 && string.Compare(now, DateTime.Now.ToString("yyyy-MM-dd 07:59:59"), true) <= 0)
            {
                SHIFT = "A";
            }
            else if (string.Compare(now, DateTime.Now.ToString("yyyy-MM-dd 08:00:00"), true) >= 0 && string.Compare(now, DateTime.Now.ToString("yyyy-MM-dd 15:59:59"), true) <= 0)
            {
                SHIFT = "B";
            }
            else if (string.Compare(now, DateTime.Now.ToString("yyyy-MM-dd 16:00:00"), true) >= 0 && string.Compare(now, DateTime.Now.ToString("yyyy-MM-dd 23:59:59"), true) <= 0)
            {
                SHIFT = "C";
            }
            #region 查询数据 
            var list = Db.Queryable<VWB_WEIGHT_XB_MES>()
             .Where($"SHIFT_DATE>='{BDATE}' and SHIFT_DATE<='{EDATE}'")
             .Where(c => c.SHIFT == SHIFT)
             .Where(c => c.UPLOAD == "0")
              // .WhereIF(!string.IsNullOrEmpty(ITEM_MARK), c => c.ITEM_MARK == ITEM_MARK)
              //  .WhereIF(!string.IsNullOrEmpty(SITE), c => c.SITE == SITE)
              //.WhereIF(!string.IsNullOrEmpty(PK_STORE), c => c.STORAGELOCATION == PK_STORE)
              //.WhereIF(!string.IsNullOrEmpty(MATERIAL), c => c.ITEM == MATERIAL)
              // .WhereIF(!string.IsNullOrEmpty(ROWNUMBER), c => c.ROWNUMBER == ROWNUMBER)
              .OrderBy(c => c.SHIFT_DATE).ToList();
            #endregion

            EOS_SLWEIGHT entity = new EOS_SLWEIGHT();
            int successnum = 0;
            string sql = "";
            if (list.Count > 0)
            {
                foreach (var data in list)
                {
                    if (data.QTY > 0)
                    {
                        #region  初始化
                        entity.SITE = data.SITE;//站点
                        entity.STORAGELOCATION = data.STORAGELOCATION;//线边仓库
                        entity.ITEM = data.ITEM;//物料
                        entity.BATCH = data.BATCH;//批号
                        entity.DATETIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");//时间
                        entity.QTY = $"{data.QTY}";//数量
                        entity.SHIFTDATE = data.SHIFT_DATE;
                        entity.SHIFT = data.SHIFT;
                        entity.ITEMMARK = data.ITEM_MARK;
                        #endregion
                        if (!string.IsNullOrEmpty(data.ITEM))
                        {
                            ret = MES_DJ301XBSLZS1(entity, "");
                        }

                        #region 更新状态
                        if (data.ITEM_MARK == "1")
                        {
                            sql = "update  app_handorder set UPLOAD4=1 where UPLOAD4=0 and ID in(select ID from vapp_handorder where iszjd = 1 and type = 5 and pk_marbasclass = '301' and nvl(def7,'0')!= '0'";
                            sql += $" and ZJTIME>='{BDATE} 00:00:00' and ZJTIME<='{EDATE} 23:59:59' and DEF7= '{data.SHIFTNAME}'";

                            if (!string.IsNullOrEmpty(data.SITE))
                            {
                                sql += $" and PK_RECEIVEORG='{data.SITE}'";
                            }
                            if (!string.IsNullOrEmpty(data.STORAGELOCATION))
                            {
                                sql += $" and PK_STORE='{data.STORAGELOCATION}'";
                            }
                            if (!string.IsNullOrEmpty(data.ITEM))
                            {
                                sql += $" and (PK_MATERIAL='{data.ITEM}' or GPMATERIAL='{data.ITEM}')";
                            }
                            sql += ")";
                        }
                        else if (data.ITEM_MARK == "2")
                        {
                            sql = "update  wb_weight_xb set UPLOAD4=1 where UPLOAD4=0 and BILLNO in(select BILLNO from vwb_weight_xbcb where PK_MARBASCLASS='301'  and COMPUTERTYPE=0 and JSSUTTLE>0";
                            sql += $" and WEIGHTDATE>='{BDATE} 00:00:00' and WEIGHTDATE<='{EDATE} 23:59:59' ";
                            if (SHIFT == "C")
                            {
                                sql += $" and WEIGHTDATE>substr(WEIGHTDATE,0,10)|| ' 16:00:00'";
                            }
                            else if (SHIFT == "B")
                            {
                                sql += $" and WEIGHTDATE<substr(WEIGHTDATE,0,10)|| ' 16:00:00'";
                            }
                            if (!string.IsNullOrEmpty(data.SITE))
                            {
                                sql += $" and PK_RECEIVEORG='{data.SITE}'";
                            }
                            if (!string.IsNullOrEmpty(data.STORAGELOCATION))
                            {
                                sql += $" and PK_RECEIVESTORE1='{data.STORAGELOCATION}'";
                            }
                            if (!string.IsNullOrEmpty(data.ITEM))
                            {
                                sql += $" and PK_MATERIAL='{data.ITEM}'";
                            }
                            sql += ")";
                        }
                        else if (data.ITEM_MARK == "3")
                        {
                            sql = "update  wb_weight_xb set UPLOAD4=1 where UPLOAD4=0 and BILLNO in(select BILLNO from vwb_weight_xb where type=1 and PK_MARBASCLASS = '301' and COMPUTERTYPE = 0 and JSSUTTLE> 0";
                            sql += $" and WEIGHTDATE>='{BDATE} 00:00:00' and WEIGHTDATE<='{EDATE} 23:59:59' and DEF4 = '{data.SHIFTNAME}'";
                            if (!string.IsNullOrEmpty(data.SITE))
                            {
                                sql += $" and PK_RECEIVEORG='{data.SITE}'";
                            }
                            if (!string.IsNullOrEmpty(data.STORAGELOCATION))
                            {
                                sql += $" and PK_RECEIVESTORE1='{data.STORAGELOCATION}'";
                            }
                            if (!string.IsNullOrEmpty(data.ITEM))
                            {
                                sql += $" and PK_MATERIAL='{data.ITEM}'";
                            }
                            sql += ")";
                        }
                        #endregion
                    }
                    else
                    {
                        #region 重量为0不上传
                        ret.STATUS = "E";
                        ret.REMSG = "重量为0不能上传!";
                        #endregion
                    }

                    if (ret.STATUS == "S")
                    {
                        if (!string.IsNullOrEmpty(sql))
                        {
                            Db.Ado.ExecuteCommand(sql);
                        }
                        successnum++;
                    }
                    else
                    {
                        log.Error(string.Format("MES上料计量自动推送:推送失败,原因{0}！", ret.REMSG));
                    }

                }
            }
            string msg = string.Format("本次MES底浆自动推送车数：{0},成功{1}条！", list.Count(), successnum);
            log.Error(msg);
            ret.REMSG = msg;
            ret.STATUS = "S";
            ret.REDATA = null;
            return ret;
        }

        /// <summary>
        /// 区块链计量回写
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        public RETURN_QKL WRZS_QKLWEIGHTSELECTZS(EOS_QKLWEIGHT wb)
        {
            RETURN_QKL ret = new RETURN_QKL();
            string strjson = "";
            try
            {
                //轨迹查询
                String url = $"{ ConfigHelper.AppSettings("QKLURL")}";
                StringBuilder builder = new StringBuilder();
                string data = wb.ToJson();
                strjson = HttpPost1(url, data);

                ret = JsonConvert.DeserializeObject<RETURN_QKL>(strjson);
                if (ret.STATE == "ok")
                {
                    log.Error($"二次计量区块链推送: 过磅号{wb.ID}成功！");
                    Db.Ado.ExecuteCommand($"update APP_HANDORDER set UPLOAD3=1 where ID='{wb.ID}'");
                }
                else
                {
                    log.Error($"二次计量区块链推送: 过磅号{wb.ID}失败,原因{ret.MSG}！");
                }
            }
            catch (Exception ex)
            {
                ret.STATE = "fail";
                ret.MSG = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 区块链作废回传 
        /// </summary>  
        /// fhh/myt  无人值守作废区块链数据回传区块链 220915
        /// <param name="wb"></param>
        /// <returns></returns>
        [WebMethod(Description = "生产机：作废区块链数据回传区块链")]
        public RETURN_QKL WRZS_QKLINVALIDZS(string pcbillcode)
        {
            RETURN_QKL ret = new RETURN_QKL();
            EOS_QKLWEIGHT qkl = new EOS_QKLWEIGHT();
            string strjson = "";
            try
            {
                //轨迹查询
                qkl.Reservation_Number = pcbillcode;
                qkl.Invalid = "作废";
                string data = qkl.ToJson();
                strjson = HttpPostQKLInvalid(data);
                ret = JsonConvert.DeserializeObject<RETURN_QKL>(strjson);
                if (ret.STATE == "ok")
                {
                    log.Error($"作废回传区块链: 派车单号{pcbillcode}成功！");
                }
                else
                {
                    log.Error($"作废回传区块链: 派车单号{pcbillcode}失败,原因{ret.MSG}！");
                }
            }
            catch (Exception ex)
            {
                ret.STATE = "fail";
                ret.MSG = ex.Message;
            }
            return ret;
        }



        /// <summary>
        /// 九车间led调用显示
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        [WebMethod(Description = "九车间led调用显示")]
        public string LEDSHOW(string P1, string P2, string P3, string P4, string P5, string P9, string P10)
        {
            string result = "";
            string strjson = "";
            try
            {
                //轨迹查询
                String url = "http://192.168.100.213:8500/?P1=" + P1 + "&P2=" + P2 + "&P3=" + P3 + "&P4=" + P4 + "&P5=" + P5 + "&P6=" + "''" + "&P7=" + "''" + "&P8=test0004&P9=" + P9 + "&P10=" + P10;
                strjson = HttpPostLed(url);
                if(strjson == "OK")
                {
                    log.Error($"过磅单号：{P10}LED屏幕展示成功");
                    result = "TRUE";
                }
                else
                {
                    log.Error($"过磅单号：{P10}LED屏幕展示失败");
                    result = "FALSE";
                }      
            }
            catch (Exception ex)
            {
                log.Error($"捕获异常：{ex.Message}");
                result = ex.Message;
            }
            return result;
        }


        /// <summary>
        /// 生产机：无人值守采购磅单二次计量推送区块链接口
        /// </summary>
        /// <param name="materialtype">物料分类</param>
        /// <returns>为空时所有分类</returns>
        [WebMethod(Description = "生产机：无人值守采购磅单二次计量推送区块链接口")]
        public RETURN_SAP WRZS_QKLSECONDWEIGHTZS(string materialtype, string BILLNO)
        {
            RETURN_SAP ret = new RETURN_SAP();
            REDATA redata = new REDATA();
            ret.STATUS = "E";
            #region 是否启用
            BASE_SYSTEMSET config = null;
            if (string.IsNullOrEmpty(BILLNO))
            {
                config = Db.Queryable<BASE_SYSTEMSET>()
            .Where(c => c.FIELDNAME == "WRZS_QKLSECONDWEIGHTZS")
            .Take(1).First();
                if (config != null)
                {
                    if (config.FVALUE == "0")
                    {
                        ret.STATUS = "E";
                        ret.REMSG = "二次计量过账接口停用，请联系管理员启用接口！";
                        redata.MATDOCUMENTYEAR = "";
                        redata.MATERIALDOCUMENT = "";
                        redata.PRUEFLOS = "";
                        redata.E_POSNR = "";
                        ret.REDATA = redata;
                        return ret;
                    }
                }
            }
            #endregion

            #region 查询数据
            var list = Db.Queryable<VAPP_HANDORDER_SECOND>()
                   .Where(c => c.UPLOAD3 == "0")
                  .Where(c => c.WEIGHTTYPE == "0")
                   .Where(c => c.FINISH == "1")
                     .Where(c => c.JSUPLOAD == "1")
                    .Where(c => c.PCBILLFROM == "3")
                  .WhereIF(string.IsNullOrEmpty(BILLNO), c => c.ISERROR == "0")
                    .WhereIF(!string.IsNullOrEmpty(BILLNO), c => c.BILLNO == BILLNO)
              .WhereIF(!string.IsNullOrEmpty(materialtype), c => c.PK_PARENT == materialtype)
              .OrderBy(c => c.WEIGHTDATE).ToPageList(1, 2);
            #endregion
            EOS_QKLWEIGHT weight = new EOS_QKLWEIGHT();
            RETURN_QKL qkl = null;
            string vbillcode = "", crowno = "", pk_material = "";
            foreach (var data in list)
            {
                #region 区块链数据上传
                if (data.PCBILLFROM == "3" && data.UPLOAD3 == "0")
                {
                    vbillcode = data.VBILLCODE;//订单编号
                    crowno = data.PK_BILL_B.Replace(data.PK_BILL, "");//订单项目编号
                    pk_material = data.PK_MATERIAL;  //物料
                    #region 初始化
                    weight.ID = data.MID;
                    weight.PK_ORDER = data.MID;
                    weight.BILLNO = data.BILLNO;
                    weight.PK_RECEIVRORG = data.PK_RECEIVEORG;
                    weight.RECEIVRORG = data.RECEIVEORG;
                    weight.WEIGHTDATE = data.WEIGHTDATE;
                    weight.LIGHTDATE = data.LIGHTDATE;
                    weight.CRETIME = data.CRETIME;
                    weight.CREUSER = data.CREUSER;
                    weight.STORENAME = data.STORE;
                    weight.GROSS = data.GROSS;
                    weight.TARE = data.TARE;
                    weight.SUTTLE = $"{data.SUTTLE}";
                    weight.SUTTLE2 = $"{data.JZXSUTTLE}";
                    weight.BATCHNO = data.DEF9;
                    weight.PDAKZ = data.PDAKZ2;
                    weight.KS = data.PDAKZ3;
                    weight.HZ = data.PDAKZ4;
                    weight.LJ = data.PDAKZ5;
                    weight.PRICE = $"{data.JZXSUTTLE1}";
                    weight.CHARGE = $"{data.JZXSUTTLE1 * data.JZXSUTTLE / 1000}";
                    weight.VBILLCODE = vbillcode;
                    weight.CROWNO = crowno;
                    weight.PK_MATERIAL = data.PK_MATERIAL.Replace("00000000000", "");
                    weight.MATERIALNAME = data.MATERIALNAME;
                    weight.GPMATERIAL = data.PK_MATERIAL.Replace("00000000000", "");
                    weight.GPMATERIALNAME = data.MATERIALNAME;
                    weight.DBMATERIAL = data.DBMATERIAL;
                    weight.DBMATERIALNAME = data.DBMATERIALNAME;
                    if (!string.IsNullOrEmpty(data.GPMATERIAL))
                    {
                        weight.GPMATERIAL = data.GPMATERIAL.Replace("00000000000", "");
                        weight.GPMATERIALNAME = data.GPMATERIALNAME;
                    }
                    weight.DEF5 = data.SHBATCHNO;
                    weight.AMOUNT = $"{ data.YFSUTTLE}";
                    #endregion
                    qkl = WRZS_QKLWEIGHTSELECTZS(weight);
                }
                #endregion
                if (qkl.STATE == "ok")
                {
                    ret.STATUS = "S";
                }
                ret.REMSG = qkl.MSG;
            }
            return ret;

        }


        /// <summary>
        /// 生产机：无人值守干磨浆附件复检推送OA接口
        /// </summary>
        /// <param></param>
        /// <returns>为空时所有分类</returns>
        [WebMethod(Description = "正式机：无人值守干磨浆复检推送OA接口")]
        public RETURN_SAP WRZS_OADRYRETESTZS(string KeyValue)
        {
            RETURN_SAP ret = new RETURN_SAP();
            string material = "";  //物料编码
            string tdbillno = "";   //提单号
            string cretime = "";    //创建时间
            string burstzs = "";   //耐破指数
            string ash = "";         //灰分
            string jld = "";  //胶蜡点
            string rjd = "";  //叩解度
            string wetweight = "";       //湿重
            string dlc = "";   //裂断长
            string water = "";      //水分
            string foldnum = "";     //耐折次数
            string org = "";    //工厂
            string supplier = "";  //供应商
            string pk_supplier = ""; //供应商编码
            string mdsuttle = "";    //码单重量
            decimal sumsuttle = 0;  //提单净重和
            decimal constant = (decimal)0.99; //比较用的数字

            var list = Db.Queryable<BD_ZJRESULT>().Where(c => c.ID == KeyValue).ToList();
            if (list.Count > 0)
            {
                cretime = list.First().CRETIME.Substring(0,10);
                tdbillno = list.First().PRINTCODE;
                material = list.First().MATERIAL;
                if (material != "000000000003010082") 
                {
                    ret.STATUS = "E";
                    ret.REMSG = "非干磨浆，不触发OA复检流程！";
                    return ret;
                }
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "质检主表未查询到信息！";
                return ret;
            }
            var listdtl = Db.Queryable<BD_ZJRESULTDETAILS>().Where(c => c.MAINID == KeyValue).ToList();
            if (listdtl.Count > 0)
            {
                foreach (var dtldate in listdtl)
                {
                    switch (dtldate.ZJITEM)
                    {
                        case "YQM002":
                            burstzs = dtldate.ZJVALUE;
                            break;
                        case "YQM006":
                            ash = dtldate.ZJVALUE;
                            break;
                        case "ZJ000567":
                            water = dtldate.ZJVALUE;
                            break;
                        case "YQM011":
                            jld = dtldate.ZJVALUE;
                            break;
                        case "YQM012":
                            dlc = dtldate.ZJVALUE;
                            break;
                        case "YQM013":
                            rjd = dtldate.ZJVALUE;
                            break;
                        case "YQM009":
                            wetweight = dtldate.ZJVALUE;
                            break;
                        case "YQM003":
                            foldnum = dtldate.ZJVALUE;
                            break;
                    }
                }
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "质检明细表未查询到信息！";
                return ret;
            }
            var listmj = Db.Queryable<VAPP_HANDORDER1>().Where(c => c.TDBILLNO == tdbillno).ToList();
            if (listmj.Count > 0)
            {
                org = listmj.First().PK_RECEIVEORG;
                supplier = listmj.First().SENDORG;
                pk_supplier = listmj.First().PK_SENDORG;
                mdsuttle = listmj.First().MDSUTTLE;
                foreach(var date in listmj)
                {
                    if(date.SUTTLE == null)
                    {
                        date.SUTTLE = 0;
                    }
                    sumsuttle += date.SUTTLE.ToString().ToDecimal();
                }
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "木浆收货管理未查询到信息！";
                return ret;
            }
            if(foldnum.ToDecimal() < 35 || ((sumsuttle*(100-water.ToDecimal())/88)/mdsuttle.ToDecimal()) < constant)
            {
                decimal xszlb = (((sumsuttle * (100 - water.ToDecimal()) / 88) / mdsuttle.ToDecimal() - 1) * 100);
                EOS_OARECHECK oa = new EOS_OARECHECK();
                #region 传参赋值
                oa.requestname = supplier + "干磨浆复检";
                oa.gsdm = org;
                oa.gysbm = pk_supplier;
                oa.gysmc = supplier;
                oa.tdh = tdbillno;
                oa.dycjysj = cretime;
                oa.sf = water;
                oa.hf = ash;
                oa.jld = jld;
                oa.rjd = rjd;
                oa.sz = wetweight;
                oa.npzs = burstzs;
                oa.dlc = dlc;
                oa.nzcs = foldnum;
                oa.xszlb = Math.Round(xszlb, 2).ToString();
                #endregion
                RETURN_OA res = WRZS_OARECHECKZS(oa);
                if(res.CODE == "1")
                {
                    ret.STATUS = "S";
                    ret.REMSG = "触发OA干磨浆复检流程成功！流程编号：" + res.MSG ;
                    return ret;
                }
                else
                {
                    ret.STATUS = "E";
                    ret.REMSG = "触发OA干磨浆复检流程失败！失败原因：" + res.MSG;
                    return ret;
                }
            }
            else
            {
                ret.STATUS = "E";
                ret.REMSG = "质检合格，无需触发复检流程";
                return ret;
            }
        }

            /// <summary>
            /// 第一次计量质检不合格传OA
            /// </summary>
            /// <param name="wb"></param>
            /// <returns></returns>
            public RETURN_OA WRZS_OAWEIGHTSELECTCS(EOS_OAWEIGHT wb)
        {
            RETURN_OA ret = new RETURN_OA();
            string strjson = "";
            try
            {
                //轨迹查询
                String url = $"{ ConfigHelper.AppSettings("OAURL")}";
                StringBuilder builder = new StringBuilder();
                string data = wb.ToJson();
                strjson = HttpPost1(url, data);
                ret = JsonConvert.DeserializeObject<RETURN_OA>(strjson);
                if (ret.CODE == "0")
                {
                    log.Error($"一次计量不合格OA推送: 过磅号{wb.jcbh}成功！");
                    Db.Ado.ExecuteCommand($"update APP_HANDORDER set UPLOAD3=1 where ID='{wb.jcbh}'");
                }
                else
                {
                    log.Error($"一次计量不合格OA推送: 过磅号{wb.jcbh}失败,原因{ret.MSG}！");
                }
            }
            catch (Exception ex)
            {
                ret.CODE = "0";
                ret.MSG = ex.Message;
            }
            return ret;
        }


        /// <summary>
        /// 第一次计量质检不合格传OA
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        public RETURN_OA WRZS_OAWEIGHTSELECTZS(EOS_OAWEIGHT wb)
        {
            RETURN_OA ret = new RETURN_OA();
            //string strjson = "";
            try
            {
                #region 默认字段赋值
                string sqr = "1994";
                string sqrbm = "150";
                string sqrgw = "1523";
                string sqrq = DateTime.Now.ToString("yyyy-MM-dd");
                string sfjys = "1";
                string clfs = "2";
                string ysjg = "2";
                string lcfqfs = "0";
                #endregion
                OAWORKFLOWSERVICES.WorkflowServicePortTypeClient workflow = new OAWORKFLOWSERVICES.WorkflowServicePortTypeClient();
                OAWORKFLOWSERVICES.WorkflowRequestInfo in0 = new OAWORKFLOWSERVICES.WorkflowRequestInfo();

                #region 主表结构
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("title", wb.title.ToString());//标题
                dic.Add("sqr", sqr.ToString());//申请人
                dic.Add("sqrbm", sqrbm.ToString());//申请人部门
                dic.Add("sqrgw", sqrgw.ToString());//申请人岗位
                dic.Add("sqrq", sqrq.ToString());//申请日期
                dic.Add("sfjys", sfjys.ToString());//是否仅校验 默认否（1）
                dic.Add("wllb", wb.wllb.ToString());//物料类别
                dic.Add("shfs", "0");//操作方式（默认采购订单）
                dic.Add("cgddh1", wb.cgddh1.ToString());//采购订单号
                dic.Add("jcbh", wb.jcbh.ToString());//进厂编号（磅单号）
                dic.Add("clfs", clfs.ToString());//处理方式（默认退货）
                dic.Add("sfsyjysjjcwl", wb.sfsyjysjjcwl.ToString());//是否属于检验时间较长物料
                dic.Add("lcfqfs", lcfqfs.ToString());// 流程发起方式
                #endregion

                #region 子表结构
                List<Dictionary<string, string>> dicclist = new List<Dictionary<string, string>>();
                Dictionary<string, string> dicc = new Dictionary<string, string>();
                dicc.Add("cgddh2mx", wb.cgddh2mx.ToString());// 采购订单号
                dicc.Add("hxm2mx", wb.hxm2mx.ToString());//行项目
                dicc.Add("wlh2mx", wb.wlh2mx.ToString());//物料号
                dicc.Add("wlms2mx", wb.wlms2mx.ToString());//物料描述
                dicc.Add("dw2mx", wb.dw2mx.ToString());//单位
                dicc.Add("bzjsmx", wb.bzjsmx.ToString());//包装件数
                dicc.Add("gc", wb.gc.ToString());//工厂
                dicc.Add("dhsl", wb.dhsl.ToString());//到货数量（毛重）
                dicc.Add("gysbh2mx", wb.gysbh2mx.ToString());//供应商编号
                dicc.Add("gysmc2mx", wb.gysmc2mx.ToString());//供应商名称
                dicc.Add("ysjg", ysjg.ToString());//验收结果
                dicc.Add("wlpzh2mx", wb.wlpzh2mx.ToString());//物料凭证号
                dicc.Add("jypmx", wb.jypmxT.ToString());//检验批
                dicc.Add("pc2mx", wb.pc2mx.ToString());//批次

                dicclist.Add(dicc);//将字典Dictionary对象插入到泛型集合
                #endregion

                #region 主表赋值处理
                OAWORKFLOWSERVICES.WorkflowRequestTableField[] jhm = new OAWORKFLOWSERVICES.WorkflowRequestTableField[dic.Count()];
                int i = 0;
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    OAWORKFLOWSERVICES.WorkflowRequestTableField Field = new OAWORKFLOWSERVICES.WorkflowRequestTableField();
                    Field.fieldName = kvp.Key;//字段名
                    Field.fieldValue = kvp.Value;//字段值
                    Field.view = true;//字段是否可见
                    Field.edit = true;//字段是否可编辑 
                    jhm[i] = Field;
                    i++;
                }
                OAWORKFLOWSERVICES.WorkflowMainTableInfo inm = new OAWORKFLOWSERVICES.WorkflowMainTableInfo();//主表
                OAWORKFLOWSERVICES.WorkflowRequestTableRecord[] mrecords = new OAWORKFLOWSERVICES.WorkflowRequestTableRecord[1];//主表只有一条记录
                mrecords[0] = new OAWORKFLOWSERVICES.WorkflowRequestTableRecord();
                mrecords[0].workflowRequestTableFields = jhm;
                inm.requestRecords = mrecords;
                #endregion

                #region 子表赋值处理
                //int m = 0;
                OAWORKFLOWSERVICES.WorkflowDetailTableInfo[] dinfo = new OAWORKFLOWSERVICES.WorkflowDetailTableInfo[2];
                OAWORKFLOWSERVICES.WorkflowRequestTableRecord[] drecords = new OAWORKFLOWSERVICES.WorkflowRequestTableRecord[1];//子表只有一条记录
                OAWORKFLOWSERVICES.WorkflowDetailTableInfo ind = new OAWORKFLOWSERVICES.WorkflowDetailTableInfo();//子表
                OAWORKFLOWSERVICES.WorkflowDetailTableInfo indk = new OAWORKFLOWSERVICES.WorkflowDetailTableInfo();//子表2
                foreach (Dictionary<string, string> obj in dicclist)
                {
                    int n = 0;
                    drecords[0] = new OAWORKFLOWSERVICES.WorkflowRequestTableRecord();
                    OAWORKFLOWSERVICES.WorkflowRequestTableField[] jhd = new OAWORKFLOWSERVICES.WorkflowRequestTableField[obj.Count()];//（14）子表的14个字段
                    foreach (KeyValuePair<string, string> kvp in obj)
                    {
                        OAWORKFLOWSERVICES.WorkflowRequestTableField Field = new OAWORKFLOWSERVICES.WorkflowRequestTableField();
                        Field.fieldName = kvp.Key;//字段名
                        Field.fieldValue = kvp.Value;//字段值
                        Field.view = true;//字段是否可见
                        Field.edit = true;//字段是否可编辑 
                        jhd[n] = Field;
                        n++;
                    }
                    drecords[0].workflowRequestTableFields = jhd;
                }
                ind.workflowRequestTableRecords = drecords;

                dinfo[0] = indk;
                dinfo[1] = ind;
                #endregion

                #region 主结构赋值
                OAWORKFLOWSERVICES.WorkflowBaseInfo baseinfo = new OAWORKFLOWSERVICES.WorkflowBaseInfo();
                baseinfo.workflowId = "1827";//流程id
                in0.workflowBaseInfo = baseinfo;
                in0.canEdit = true;
                in0.canView = true;
                in0.creatorId = "1994";
                in0.mustInputRemark = false;
                in0.isnextflow = "1";
                in0.requestLevel = "0";
                in0.requestName = wb.title.ToString();
                in0.workflowMainTableInfo = inm;
                in0.workflowDetailTableInfos = dinfo;
                string res = workflow.doCreateWorkflowRequest(in0, 1994);// 调用接口（in0主结构，1994创建人id）
                #endregion

                if (res.ToDecimal() > 0)
                {
                    log.Error($"一次计量不合格OA推送: 过磅号{wb.jcbh}成功,流程编号{res}！");
                    Db.Ado.ExecuteCommand($"update APP_HANDORDER set UPLOAD3=1 where ID='{wb.jcbh}'");
                    ret.CODE = "1";
                    ret.MSG = res;
                }
                else
                {
                    log.Error($"一次计量不合格OA推送: 过磅号{wb.jcbh}失败,原因{res}！");
                    ret.CODE = "0";
                    ret.MSG = res;
                }
            }
            catch (Exception ex)
            {
                ret.CODE = "0";
                ret.MSG = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 第一次计量质检不合格传OA测试
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        public RETURN_OA WRZS_OARECHECKCS(EOS_OARECHECK wb)
        {
            RETURN_OA ret = new RETURN_OA();
            //string strjson = "";
            try
            {
                #region 默认字段赋值
                string sqr = "2380";
                //string sqrbm = "150";
                //string sqrgw = "1523";
                string sqrq = DateTime.Now.ToString("yyyy-MM-dd");
                //string sfjys = "1";
                //string clfs = "2";
                //string ysjg = "2";
                string lcfqfs = "1";
                #endregion
                OAWORKFLOWSERVICESCS.WorkflowServicePortTypeClient workflow = new OAWORKFLOWSERVICESCS.WorkflowServicePortTypeClient();
                OAWORKFLOWSERVICESCS.WorkflowRequestInfo in0 = new OAWORKFLOWSERVICESCS.WorkflowRequestInfo();

                #region 主表结构
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("requestname", wb.requestname.ToString());//标题
                dic.Add("sqr", sqr.ToString());//申请人
                dic.Add("sqrq", sqrq.ToString());//申请时间
                dic.Add("lcfqfs", lcfqfs.ToString());//流程发起方式
                dic.Add("gsdm", wb.gsdm.ToString());//公司代码
                dic.Add("gysbm", "0000" + wb.gysbm.ToString());//供应商编码
                dic.Add("gysmc", wb.gysmc.ToString());//供应商名称
                dic.Add("tdh", wb.tdh.ToString());//提单号
                dic.Add("dycjysj", wb.dycjysj.ToString());//第一次检验时间
                #endregion

                #region 子表结构
                List<Dictionary<string, string>> dicclist = new List<Dictionary<string, string>>();
                Dictionary<string, string> dicc = new Dictionary<string, string>();
                dicc.Add("hyzb", "第一次检验结果");//
                dicc.Add("sf", wb.sf.ToString());// 水分
                dicc.Add("hf", wb.hf.ToString());//灰分
                dicc.Add("jld", wb.jld.ToString());//胶蜡点
                dicc.Add("rjd", wb.rjd.ToString());//叩解度
                dicc.Add("sz", wb.sz.ToString());//湿重
                dicc.Add("npzs", wb.npzs.ToString());//耐破指数
                dicc.Add("dlz", wb.dlc.ToString());//裂断长
                dicc.Add("nzcs", wb.nzcs.ToString());//耐折次数
                dicc.Add("xszlb", wb.xszlb.ToString());//销售重量比（折算重量-销售重量）/销售重量 ≥-1%

                dicclist.Add(dicc);//将字典Dictionary对象插入到泛型集合
                #endregion

                #region 主表赋值处理
                OAWORKFLOWSERVICESCS.WorkflowRequestTableField[] mcollection = new OAWORKFLOWSERVICESCS.WorkflowRequestTableField[dic.Count()];
                int i = 0;
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    OAWORKFLOWSERVICESCS.WorkflowRequestTableField field = new OAWORKFLOWSERVICESCS.WorkflowRequestTableField();
                    field.fieldName = kvp.Key;//字段名
                    field.fieldValue = kvp.Value;//字段值
                    field.view = true;//字段是否可见
                    field.edit = true;//字段是否可编辑 
                    mcollection[i] = field;
                    i++;
                }
                OAWORKFLOWSERVICESCS.WorkflowMainTableInfo minfo = new OAWORKFLOWSERVICESCS.WorkflowMainTableInfo();
                OAWORKFLOWSERVICESCS.WorkflowRequestTableRecord[] mrecords = new OAWORKFLOWSERVICESCS.WorkflowRequestTableRecord[1];
                mrecords[0] = new OAWORKFLOWSERVICESCS.WorkflowRequestTableRecord();
                mrecords[0].workflowRequestTableFields = mcollection;
                minfo.requestRecords = mrecords;
                #endregion

                #region 子表赋值处理
                OAWORKFLOWSERVICESCS.WorkflowDetailTableInfo[] dinfo = new OAWORKFLOWSERVICESCS.WorkflowDetailTableInfo[1];
                OAWORKFLOWSERVICESCS.WorkflowRequestTableRecord[] drecords = new OAWORKFLOWSERVICESCS.WorkflowRequestTableRecord[1];
                OAWORKFLOWSERVICESCS.WorkflowDetailTableInfo ind = new OAWORKFLOWSERVICESCS.WorkflowDetailTableInfo();//子表
                foreach (Dictionary<string, string> obj in dicclist)
                {
                    int n = 0;
                    drecords[0] = new OAWORKFLOWSERVICESCS.WorkflowRequestTableRecord();
                    OAWORKFLOWSERVICESCS.WorkflowRequestTableField[] dcollection = new OAWORKFLOWSERVICESCS.WorkflowRequestTableField[obj.Count()];
                    foreach (KeyValuePair<string, string> kvp in obj)
                    {
                        OAWORKFLOWSERVICESCS.WorkflowRequestTableField field = new OAWORKFLOWSERVICESCS.WorkflowRequestTableField();
                        field.fieldName = kvp.Key;//字段名
                        field.fieldValue = kvp.Value;//字段值
                        field.view = true;//字段是否可见
                        field.edit = true;//字段是否可编辑 
                        dcollection[n] = field;
                        n++;
                    }
                    drecords[0].workflowRequestTableFields = dcollection;
                }
                ind.workflowRequestTableRecords = drecords;

                dinfo[0] = ind;
                #endregion

                #region 主结构赋值
                OAWORKFLOWSERVICESCS.WorkflowBaseInfo baseinfo = new OAWORKFLOWSERVICESCS.WorkflowBaseInfo();
                baseinfo.workflowId = "9872";
                in0.workflowBaseInfo = baseinfo;
                in0.canEdit = true;
                in0.canView = true;
                in0.creatorId = "2380";
                in0.mustInputRemark = false;
                in0.isnextflow = "0";
                in0.requestLevel = "0";
                in0.requestName = wb.requestname.ToString();
                in0.workflowMainTableInfo = minfo;
                in0.workflowDetailTableInfos = dinfo;
                string res = workflow.doCreateWorkflowRequest(in0, 2380);// 调用接口（in0主结构，1994创建人id）
                #endregion

                if (res.ToDecimal() > 0)
                {
                    log.Error($"OA干磨浆复检流程触发成功: 提单号{wb.tdh}成功,流程编号{res}！");
                    ret.CODE = "1";
                    ret.MSG = res;
                }
                else
                {
                    log.Error($"OA干磨浆复检流程触发失败: 提单号{wb.tdh}失败,原因{res}！");
                    ret.CODE = "0";
                    ret.MSG = res;
                }
            }
            catch (Exception ex)
            {
                ret.CODE = "0";
                ret.MSG = ex.Message;
            }
            return ret;
        }

        /// <summary>
        /// 第一次计量质检不合格传OA正式
        /// </summary>
        /// <param name="wb"></param>
        /// <returns></returns>
        public RETURN_OA WRZS_OARECHECKZS(EOS_OARECHECK wb)
        {
            RETURN_OA ret = new RETURN_OA();
            //string strjson = "";
            try
            {
                #region 默认字段赋值
                string sqr = "2380";
                //string sqrbm = "150";
                //string sqrgw = "1523";
                string sqrq = DateTime.Now.ToString("yyyy-MM-dd");
                //string sfjys = "1";
                //string clfs = "2";
                //string ysjg = "2";
                string lcfqfs = "1";
                #endregion
                OAWORKFLOWSERVICES.WorkflowServicePortTypeClient workflow = new OAWORKFLOWSERVICES.WorkflowServicePortTypeClient();
                OAWORKFLOWSERVICES.WorkflowRequestInfo in0 = new OAWORKFLOWSERVICES.WorkflowRequestInfo();

                #region 主表结构
                Dictionary<string, string> dic = new Dictionary<string, string>();
                dic.Add("requestname", wb.requestname.ToString());//标题
                dic.Add("sqr", sqr.ToString());//申请人
                dic.Add("sqrq", sqrq.ToString());//申请时间
                dic.Add("lcfqfs", lcfqfs.ToString());//流程发起方式
                dic.Add("gsdm", wb.gsdm.ToString());//公司代码
                dic.Add("gysbm", "0000" + wb.gysbm.ToString());//供应商编码
                dic.Add("gysmc", wb.gysmc.ToString());//供应商名称
                dic.Add("tdh", wb.tdh.ToString());//提单号
                dic.Add("dycjysj", wb.dycjysj.ToString());//第一次检验时间
                #endregion

                #region 子表结构
                List<Dictionary<string, string>> dicclist = new List<Dictionary<string, string>>();
                Dictionary<string, string> dicc = new Dictionary<string, string>();
                dicc.Add("hyzb", "第一次检验结果");//
                dicc.Add("sf", wb.sf.ToString());// 水分
                dicc.Add("hf", wb.hf.ToString());//灰分
                dicc.Add("jld", wb.jld.ToString());//胶蜡点
                dicc.Add("rjd", wb.rjd.ToString());//叩解度
                dicc.Add("sz", wb.sz.ToString());//湿重
                dicc.Add("npzs", wb.npzs.ToString());//耐破指数
                dicc.Add("dlz", wb.dlc.ToString());//裂断长
                dicc.Add("nzcs", wb.nzcs.ToString());//耐折次数
                dicc.Add("xszlb", wb.xszlb.ToString());//销售重量比（折算重量-销售重量）/销售重量 ≥-1%

                dicclist.Add(dicc);//将字典Dictionary对象插入到泛型集合
                #endregion

                #region 主表赋值处理
                OAWORKFLOWSERVICES.WorkflowRequestTableField[] mcollection = new OAWORKFLOWSERVICES.WorkflowRequestTableField[dic.Count()];
                int i = 0;
                foreach (KeyValuePair<string, string> kvp in dic)
                {
                    OAWORKFLOWSERVICES.WorkflowRequestTableField field = new OAWORKFLOWSERVICES.WorkflowRequestTableField();
                    field.fieldName = kvp.Key;//字段名
                    field.fieldValue = kvp.Value;//字段值
                    field.view = true;//字段是否可见
                    field.edit = true;//字段是否可编辑 
                    mcollection[i] = field;
                    i++;
                }
                OAWORKFLOWSERVICES.WorkflowMainTableInfo minfo = new OAWORKFLOWSERVICES.WorkflowMainTableInfo();
                OAWORKFLOWSERVICES.WorkflowRequestTableRecord[] mrecords = new OAWORKFLOWSERVICES.WorkflowRequestTableRecord[1];
                mrecords[0] = new OAWORKFLOWSERVICES.WorkflowRequestTableRecord();
                mrecords[0].workflowRequestTableFields = mcollection;
                minfo.requestRecords = mrecords;
                #endregion

                #region 子表赋值处理
                OAWORKFLOWSERVICES.WorkflowDetailTableInfo[] dinfo = new OAWORKFLOWSERVICES.WorkflowDetailTableInfo[1];
                OAWORKFLOWSERVICES.WorkflowRequestTableRecord[] drecords = new OAWORKFLOWSERVICES.WorkflowRequestTableRecord[1];
                OAWORKFLOWSERVICES.WorkflowDetailTableInfo ind = new OAWORKFLOWSERVICES.WorkflowDetailTableInfo();//子表
                foreach (Dictionary<string, string> obj in dicclist)
                {
                    int n = 0;
                    drecords[0] = new OAWORKFLOWSERVICES.WorkflowRequestTableRecord();
                    OAWORKFLOWSERVICES.WorkflowRequestTableField[] dcollection = new OAWORKFLOWSERVICES.WorkflowRequestTableField[obj.Count()];
                    foreach (KeyValuePair<string, string> kvp in obj)
                    {
                        OAWORKFLOWSERVICES.WorkflowRequestTableField field = new OAWORKFLOWSERVICES.WorkflowRequestTableField();
                        field.fieldName = kvp.Key;//字段名
                        field.fieldValue = kvp.Value;//字段值
                        field.view = true;//字段是否可见
                        field.edit = true;//字段是否可编辑 
                        dcollection[n] = field;
                        n++;
                    }
                    drecords[0].workflowRequestTableFields = dcollection;
                }
                ind.workflowRequestTableRecords = drecords;

                dinfo[0] = ind;
                #endregion

                #region 主结构赋值
                OAWORKFLOWSERVICES.WorkflowBaseInfo baseinfo = new OAWORKFLOWSERVICES.WorkflowBaseInfo();
                baseinfo.workflowId = "49047";
                in0.workflowBaseInfo = baseinfo;
                in0.canEdit = true;
                in0.canView = true;
                in0.creatorId = "2380";
                in0.mustInputRemark = false;
                in0.isnextflow = "0";
                in0.requestLevel = "0";
                in0.requestName = wb.requestname.ToString();
                in0.workflowMainTableInfo = minfo;
                in0.workflowDetailTableInfos = dinfo;
                string res = workflow.doCreateWorkflowRequest(in0, 2380);// 调用接口（in0主结构，1994创建人id）
                #endregion

                if (res.ToDecimal() > 0)
                {
                    log.Error($"OA干磨浆复检流程触发成功: 提单号{wb.tdh}成功,流程编号{res}！");
                    ret.CODE = "1";
                    ret.MSG = res;
                }
                else
                {
                    log.Error($"OA干磨浆复检流程触发失败: 提单号{wb.tdh}失败,原因{res}！");
                    ret.CODE = "0";
                    ret.MSG = res;
                }
            }
            catch (Exception ex)
            {
                ret.CODE = "0";
                ret.MSG = ex.Message;
            }
            return ret;
        }



        #endregion

        /// <summary>
        /// 收货登记OA
        /// </summary>
        /// <returns></returns>
        public RETURN_SAP OA_FINISHWEIGHT()
        {
            RETURN_SAP ret = new RETURN_SAP();
            REDATA redata = new REDATA();
            ret.STATUS = "E";
            OAWORKFLOWSERVICES.WorkflowServicePortTypeClient workflow = new WebService.OAWORKFLOWSERVICES.WorkflowServicePortTypeClient();
            OAWORKFLOWSERVICES.WorkflowRequestInfo in0 = new OAWORKFLOWSERVICES.WorkflowRequestInfo();
            in0.canEdit = true;
            in0.canView = true;
            in0.creatorId = "838";
            in0.mustInputRemark = false;
            in0.requestLevel = "0";
            in0.requestName = "退货";
            in0.workflowBaseInfo = null;
            in0.workflowMainTableInfo = null;
            in0.workflowDetailTableInfos = null;
            workflow.doCreateWorkflowRequest(in0, 1827);
            return ret;
        }
        #endregion

        public string HttpPost(string url, string body)
        {
            Encoding encoding = Encoding.UTF8;
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(OnRemoteCertificateValidationCallback);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.Accept = "application/json";
            request.ContentType = "application/json;charset=UTF-8";
            byte[] buffer = encoding.GetBytes(body);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
        public static string HttpPost1(string url, string body)
        {
            Encoding encoding = Encoding.UTF8;
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            client.UserAgent = "apifox/1.0.0 (https://www.apifox.cn)";
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            return response.Content;
        }

        public static string HttpPostLed(string url)
        {
            Encoding encoding = Encoding.UTF8;
            var client = new RestClient(url);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            client.UserAgent = "apifox/1.0.0 (https://www.apifox.cn)";
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            return response.Content;
        }

        public static string HttpPostQKLInvalid(string info)
        {
            var client = new RestClient("http://zhit.sunshinepaper.com.cn:8081/sanshine/api/UpdateStatus");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            client.UserAgent = "apifox/1.0.0 (https://www.apifox.cn)";
            request.AddHeader("Content-Type", "application/json");
            var body = info;
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            //Console.WriteLine(response.Content);
            return response.Content;
        }

        public static string HttpPost2(string url, string body, Encoding encode = null)
        {
            string result = "";
            try
            {
                var webClient = new WebClient { Encoding = Encoding.UTF8 };
                if (encode != null)
                {
                    webClient.Encoding = encode;
                }
                var sendDate = Encoding.GetEncoding("UTF-8").GetBytes(body);
                webClient.Headers.Add("ContentType", "application/json;charset=UTF-8");
                webClient.Headers.Add("ContentLength", sendDate.Length.ToString());
                var readDate = webClient.UploadData(url, "POST", sendDate);
                result = Encoding.GetEncoding("UTF-8").GetString(readDate);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
        public static string HttpPost(string url, string pars, string contentType = "")
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pars);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentLength = bytes.Length;
            //request.ContentType = "text/xml";
            if (!string.IsNullOrEmpty(contentType))
                request.ContentType = contentType;
            request.Credentials = CredentialCache.DefaultCredentials;

            Stream writer = request.GetRequestStream();
            writer.Write(bytes, 0, bytes.Length);
            writer.Close();

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            string text = readStream.ReadToEnd();
            response.Close();
            readStream.Close();
            return text;
        }


        public string HttpPostFrom(string url, string data)
        {
            data = data.TrimEnd('&');
            string htmlAll = "";
            try
            {
                string SendMessageAddress = url;//请求链接
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SendMessageAddress);
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(OnRemoteCertificateValidationCallback);
                request.Method = "POST";
                request.AllowAutoRedirect = true;
                request.Timeout = 20 * 1000;
                request.ContentType = "application/x-www-form-urlencoded";
                //request.Headers.Add("x-cherun-auth-key", "LarxMbndsxfGwoYAqsfJSPPU42l04cb3");
                //string PostData = "a=1&b=2";//请求参数格式
                string PostData = data;//请求参数
                byte[] byteArray = Encoding.Default.GetBytes(PostData);
                request.ContentLength = byteArray.Length;
                using (Stream newStream = request.GetRequestStream())
                {
                    newStream.Write(byteArray, 0, byteArray.Length);//写入参数
                    newStream.Close();
                }

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream rspStream = response.GetResponseStream();
                using (StreamReader reader = new StreamReader(rspStream, Encoding.UTF8))
                {
                    htmlAll = reader.ReadToEnd();
                    rspStream.Close();
                }
                response.Close();
            }
            catch (Exception ex)
            {

                string s = ex.Message;
            }
            return htmlAll;
        }

        private bool OnRemoteCertificateValidationCallback(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; // 认证正常，没有错误
        }
        private static bool OnRemoteCertificateValidationCallback2(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true; // 认证正常，没有错误
        }
    }


}

