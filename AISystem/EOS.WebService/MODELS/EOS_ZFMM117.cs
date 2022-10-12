using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class EOS_ZFMM117
    {
        /// <summary>
        /// 物料组
        /// </summary>
        public string I_MATKL { get; set; }

        /// <summary>
        /// 物料类型
        /// </summary>
        public string I_MTART { get; set; }
        /// <summary>
        /// 物料编号
        /// </summary>
        public string I_MATNR { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string I_MAKTX { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string I_WERKS { get; set; }
        /// <summary>
        /// 外部物料组
        /// </summary>
        public string I_EXTWG { get; set; }
        /// <summary>
        /// 采购组
        /// </summary>
        public string I_EKGRP { get; set; }

        /// <summary>
        /// 同步类型:0只新增，1 新增更新 
        /// </summary>
        public string OPENTYPE { get; set; } = "0";
    }
}