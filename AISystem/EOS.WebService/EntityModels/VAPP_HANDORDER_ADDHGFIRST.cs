using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService.EntityModels
{
    public class VAPP_HANDORDER_ADDHGFIRST
    {
            /// <summary>
            /// 磅单号
            /// </summary>
            public string BILLNO { get; set; }
            /// <summary>
            /// 库存地点
            /// </summary>
            public string PK_STORE { get; set; }
            /// <summary>
            /// 司机电话
            /// </summary>
            public string TEL { get; set; }
    }
}