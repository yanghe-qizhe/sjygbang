using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class EOS_ZTTQM001
    {
        /// <summary>
        /// 检验批次号
        /// </summary>
        public string PRUEFLOS { get; set; }
        /// <summary>
        /// 采购订单号
        /// </summary>
        public string EBELN { get; set; }
        /// <summary>
        /// 订单行项目
        /// </summary>
        public string ITEM_LINE { get; set; }
        /// <summary>
        /// 物料号
        /// </summary>
        public string MATNR { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string MATNRNAME { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string MATERIALTYPE { get; set; }
        /// <summary>
        /// 供应商编号
        /// </summary>
        public string SUPPLY { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SUPPLYNAME { get; set; }

        /// <summary>
        /// 磅单号
        /// </summary>
        public string POUND_ORDER { get; set; }
        /// <summary>
        /// //批次号10位的批次号,格式：yymmdd0000
        /// </summary>
        public string CHARG { get; set; }
        /// <summary>
        /// 毛重/到货数量
        /// </summary>
        public double GROSS_WEIGHT { get; set; } = 0;
        /// <summary>
        /// 包装件数
        /// </summary>
        public string PACKAGES { get; set; }
        /// <summary>
        /// 库存地点
        /// </summary>
        public string STOCK_LOCATION { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 来货日期
        /// </summary>
        public string DELIVERY_DATE { get; set; }
        /// <summary>
        /// 采购员
        /// </summary>
        public string BUYER { get; set; }
        /// <summary>
        /// 验收人员
        /// </summary>
        public string ACCEPTANCE { get; set; }
        /// <summary>
        /// 是否属于检验时间较长物料，是（0）/否（1）
        /// </summary>
        public string LONG_MATERIAL { get; set; } = "1";
        /// <summary>
        /// 检验类型
        ///JYLX40 化工辅料检验
        ///JYLX41 木浆检验
        ///JYLX42 废纸棍检验 YG30
        ///JYLX43 木纤维检验 
        ///JYLX44 其他包装物
        /// </summary>
        public string INSPECTION_TYPE { get; set; }
        /// <summary>
        /// 任务状态
        /// </summary>
        public string TASK_STATUS { get; set; } = "NEW";
        /// <summary>
        /// 任务类型
        /// </summary>
        public string TASK_TYPE { get; set; } = "RWLX04";

        /// <summary>
        /// 创建时间
        /// </summary>
        public string CREATED_DATE_TIME { get; set; } = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        /// <summary>
        /// 创建人
        /// </summary>
        public string CREATED_USER { get; set; } = "无人值守检验";

        public string WGTYPE { get; set; }
        public string ISZJ { get; set; }
        public string ZJRESULT { get; set; }
        public string ZJRESULT1 { get; set; }


        public string PZBILLNO { get; set; }

        public string MAKTX { get; set; } //MYT/LIUYAN  221009 新增物料描述字段传值到SAP

    }
}