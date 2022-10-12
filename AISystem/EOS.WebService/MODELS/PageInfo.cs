using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class PageInfo
    {
        /// <summary>
        /// 页数
        /// </summary>
        public int PageIndex { set; get; }
        /// <summary>
        /// 条数
        /// </summary>
        public int PageSize { set; get; }
        /// <summary>
        /// 总数
        /// </summary>
        public int TotalNumber { set; get; }
        /// <summary>
        /// 数据
        /// </summary>
        public object Data { set; get; }


        public string SortName { get; set; }
        public string SortOrder { get; set; }
    }
}