using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class EOS_SLWEIGHT
    {
        /// <summary>
        /// 站点
        /// </summary>
        public string SITE { get; set; }
        /// <summary>
        /// 车间
        /// </summary>
        public string WORKSHOP { get; set; }
        /// <summary>
        /// 线边仓库
        /// </summary>
        public string   STORAGELOCATION { get; set; }
        /// <summary>
        /// 物料号
        /// </summary>
        public string ITEM { get; set; }
        
        /// <summary>
        /// //批次号10位的批次号,格式：yymmdd0000
        /// </summary>
        public string BATCH { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string QTY { get; set; } 
        /// <summary>
        /// 称重时间
        /// </summary>
        public string   DATETIME { get; set; }
        /// <summary>
        /// 记录类型：IN/OUT
        /// </summary>
        public string   TRANSFERTYPE { get; set; }
        /// <summary>
        /// 供应商
        /// </summary>
        public string   VENDOR { get; set; }
        public string ITEMMARK { get; set; }
        public string SHIFT { get; set; }
        public string SHIFTDATE { get; set; }
    

    }
}