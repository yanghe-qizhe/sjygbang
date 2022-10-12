using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class EOS_Z_GM_POST
    {
        //业务码
        public string GM_CODE { get; set; } = "04";
        /// <summary>
        /// 凭证日期
        /// </summary>
        public string DOC_DATE { get; set; } = DateTime.Now.ToString("yyyy-MM-dd");

        /// <summary>
        /// 移动类型
        /// </summary>
        public string MOVE_TYPE { get; set; } = "311";

        /// <summary>
        /// 计量单位
        /// </summary>
        public string ENTRY_UOM { get; set; } = "KG";
        /// <summary>
        /// 物料编码
        /// </summary>
        public string MATERIAL { get; set; } = "";
        /// <summary>
        /// 原批次
        /// </summary>
        public string BATCH { get; set; } = "";
        /// <summary>
        /// 现批次
        /// </summary>
        public string MOVE_BATCH { get; set; } = "";
        /// <summary>
        /// 数量
        /// </summary>
        public decimal ENTRY_QNT { get; set; } = 0;
        /// <summary>
        /// 标识 
        /// </summary>
        public bool ENTRY_QNTSpecified { get; set; } = true;
        /// <summary>
        /// 发货仓库
        /// </summary>
        public string STGE_LOC { get; set; } = "";
        /// <summary>
        /// 收货仓库
        /// </summary>
        public string MOVE_STLOC { get; set; } = "";

        /// <summary>
        /// 原工厂
        /// </summary>
        public string PLANT { get; set; } = "";

        /// <summary>
        /// 收货工厂
        /// </summary>
        public string MOVE_PLANT { get; set; } = "";

        public string HEADER_TXT { get; set; }

    }
}