using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EOS.DataAccess.Attributes;
using EOS.Entity;

namespace EOS.WebService
{
    public class EOS_ZJORDER
    {  
        /// <summary>
       /// 化验主表
       /// </summary>
        public VBD_ZJRESULT MODEL { get; set; }
        /// <summary>
        /// 化验明细
        /// </summary>
        public List<VBD_ZJRESULTDETAILS> DETAILS { get; set; }
    }
}