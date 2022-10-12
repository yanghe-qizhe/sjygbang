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
    public class EOS_SLJHORDER
    {
 
        /// <summary>
        /// 采样明细
        /// </summary>
        public List<Z_ITEM_DEMAND_SUM> DETAILS { get; set; }

    }
}
