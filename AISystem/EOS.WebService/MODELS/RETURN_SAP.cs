using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    [Serializable]
    public class RETURN_OA
    {
        /// <summary>
        /// 消息类型0:成功 500:错误
        /// </summary>
        public string CODE { get; set; } = "500";
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MSG { get; set; } = "";
    }
    [Serializable]
    public class RETURN_QKL
    {
        /// <summary>
        /// 消息类型OK:成功，允许撤审 E:错误，不允许撤审
        /// </summary>
        public string STATE { get; set; } = "";
        /// <summary>
        /// 消息内容
        /// </summary>
        public string MSG { get; set; } = "";
    }
    [Serializable]
    public class RETURN_SAP
    { 
        /// <summary>
        /// 消息类型S:成功，允许撤审 E:错误，不允许撤审
        /// </summary>
        public string STATUS { get; set; } = "";
        /// <summary>
        /// 消息内容
        /// </summary>
        public string REMSG { get; set; } = "";
        /// <summary>
        /// 数据
        /// </summary>
        public REDATA REDATA { get; set; } = null;


    }
}

 