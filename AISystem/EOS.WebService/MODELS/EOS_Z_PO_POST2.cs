using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class EOS_Z_PO_POST2
    {
        /// <summary>
        /// 01采购订单收货
        /// 02生产订单收货
        /// </summary>
        public string EOS_GM_CODE { get; set; } = "02";
        /// <summary>
        ///凭证日期,默认当前系统日期
        /// </summary>
        public string DOC_DATE { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");
        /// <summary>
        /// 过账日期,默认当前系统日期,可修改
        /// </summary>
        public string PSTNG_DATE { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");



        /// <summary>
        /// 订单编号
        /// </summary>
        public string PO_NUMBER { get; set; }
        /// <summary>
        /// 订单项目编号
        /// </summary>
        public string PO_ITEM { get; set; }


        /// <summary>
        /// 订单编号
        /// </summary>
        public string ORDERID { get; set; }
        /// <summary>
        /// 订单项目编号
        /// </summary>
        public string ORDER_ITNO { get; set; }

        /// <summary>
        /// 移动类型：废纸101,化工,包装物103
        /// 103不产生会计凭证，只有物料凭证。
        /// </summary>
        public string MOVE_TYPE { get; set; } = "101";
        /// <summary>
        /// //移动标识,B,F
        /// B是采购订单、F是生产订单
        /// </summary>
        public string MVT_IND { get; set; } = "B";
        /// <summary>
        ///特殊库存标识
        ///K寄售,空
        /// </summary>
        public string SPEC_STOCK { get; set; } = "";

        /// <summary>
        /// 数量
        /// </summary>
        public decimal ENTRY_QNT { get; set; } = 0;

        public bool ENTRY_QNTSpecified { get; set; } = true;

        /// <summary>
        /// 库存地点
        /// </summary>
        public string STGE_LOC { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string BATCH { get; set; }

        /// <summary>
        /// 工厂
        /// </summary>
        public string PLANT { get; set; }
        /// <summary>
        /// 物料
        /// </summary>
        public string MATERIAL { get; set; }
        public string HEADER_TXT { get; set; }
        public string BILL_OF_LADING { get; set; }
        //提单号
        /// <summary>
        /// 收货/发货单编号(交货单)
        /// </summary>
        public string GR_GI_SLIP_NO { get; set; }
        public double WEIGHT { get; set; }
        public string PINP { get; set; }
    }
}