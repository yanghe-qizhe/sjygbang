using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class SearchInfo
    {
        public PageInfo Page { set; get; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { set; get; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { set; get; }
        /// <summary>
        /// 物料
        /// </summary>
        public string Materiel { set; get; }
        /// <summary>
        /// 车号
        /// </summary>
        public string Cars { set; get; }
 
        /// <summary>
        /// 发货单位
        /// </summary>
        public string Send { set; get; }
        /// <summary>
        /// 收货单位
        /// </summary>
        public string Receive { set; get; }
        /// <summary>
        /// 组织机构
        /// </summary>
        public string Org { set; get; }
        /// <summary>
        /// 运输单位
        /// </summary>
        public string TrafficCompany { set; get; }
        /// <summary>
        /// 仓库
        /// </summary>
        public string Store { set; get; }
        /// <summary>
        /// 单号
        /// </summary>
        public string BillNo { set; get; }
        /// <summary>
        /// 派车单号
        /// </summary>
        public string PCBillNo { set; get; }
        /// <summary>
        /// 订单单号
        /// </summary>
        public string VBillCode { set; get; }

        /// <summary>
        /// 批次号
        /// </summary>
        public string BATCHNO { set; get; }

        /// <summary>
        /// 制样号
        /// </summary>
        public string PRINTCODE { set; get; }
        /// <summary>
        /// IC卡序号
        /// </summary>
        public string ICCARD { set; get; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public string UserId { set; get; }

        /// <summary>
        /// 用户名称 
        /// </summary>
        public string UserName { set; get; }

      
        /// <summary>
        /// IsUse类型
        /// </summary>
        public string IsUse { set; get; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Finish { set; get; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { set; get; }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string Def1 { set; get; }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string Def2 { set; get; }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string Def3 { set; get; }
        /// <summary>
        /// 预留字段
        /// </summary>
        public string Def4 { set; get; }
    }
}