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
    /// 采样信息及检验项目
    /// </summary>
    public class EOS_CYORDER  
    {
        /// <summary>
        /// 采样主表
        /// </summary>
        public VBD_PCRAWCY MODEL { get; set; }
        /// <summary>
        /// 采样明细
        /// </summary>
        public List<VBD_PCRAWCYDETAILS> DETAILS { get; set; }

        /// <summary>
        /// 采样物料检验项目
        /// </summary>
        public List<VBase_ZJFAConfig_B> ITEMSDETAILS { get; set; }
    }
}
