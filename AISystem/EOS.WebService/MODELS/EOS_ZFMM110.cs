using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class EOS_ZFMM110
    {
        /// <summary>
        /// 订单号
        /// </summary>
        public string AUFNR { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string CHARG { get; set; }
        /// <summary>
        /// 库存地点
        /// </summary>
        public string LGORT { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal ERFMG { get; set; }
 
    }
}