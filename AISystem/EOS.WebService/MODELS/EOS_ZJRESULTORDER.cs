using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using EOS.DataAccess.Attributes;
using EOS.Entity;

namespace EOS.WebService
{
    /// <summary>
    /// 化验
    /// </summary>
    public class EOS_ZJRESULTORDER
    {

        #region 获取/设置 字段值
        /// <summary>
        /// 化验单号
        /// </summary>
        public string BILLNO { get;set;}
        /// <summary>
        /// 制单人
        /// </summary>
        public string CREUSER { get; set; }
        /// <summary>
        /// 制单日期
        /// </summary>
        public string CRETIME { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ZJFA { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string CYID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string PCRAWTYPE { get; set; }
        /// <summary>
        /// 制样扫码
        /// </summary>
        public string PRINTCODE { get; set; }
        /// <summary>
        /// 处置方式
        /// </summary>
        public string CZTYPE { get; set; }
        /// <summary>
        /// 物料
        /// </summary>
        public string MATERIAL { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string MATERIALSPEC { get; set; }
        /// <summary>
        /// 接收公司
        /// </summary>
        public string PK_ORG { get; set; }
        /// <summary>
        /// 接收公司
        /// </summary>
        public string ORGNAME { get; set; }
        /// <summary>
        /// 化验类型
        /// </summary>
        public int CYTYPE { get; set; }
        /// <summary>
        /// 化验人
        /// </summary>
        public string DEF10 { get; set; }
        /// <summary>
        /// 合格判定
        /// </summary>
        public string ZJRESULT { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string MEMO { get; set; }

        public int CHECKNUM { get; set; }
        
        #endregion
        /// <summary>
        /// 采样明细
        /// </summary>
        public List<BD_ZJRESULTDETAILS> DETAILS { get; set; }

    }
}
