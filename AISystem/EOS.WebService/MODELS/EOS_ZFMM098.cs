using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class EOS_ZFMM098
    {
        /// <summary>
        /// 检验批次编号
        /// </summary>
        public string PRUEFLOS { get; set; }
        /// <summary>
        /// 库存地点
        /// </summary>
        public string LGORT { get; set; }
        /// <summary>
        /// 将过账到非限制使用库存的数量(净重-扣重)
        /// </summary>
        public string FXZKC { get; set; }
        /// <summary>
        /// 以退货到供应商形式过账的数量(皮+扣重)
        /// </summary>
        public string THJH { get; set; }
        /// <summary>
        /// 检验结果数(SAP提供获取检验结果数)
        /// </summary>
        public string JYJG { get; set; }
        /// <summary>
        /// 使用决策代码(默认:001)
        /// </summary>
        public string VCODE { get; set; } = "001";
    }
}