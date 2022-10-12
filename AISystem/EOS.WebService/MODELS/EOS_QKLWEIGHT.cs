using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EOS.WebService
{
    public class EOS_QKLWEIGHT
    {
        public string ID { get; set; }
        public string BILLNO { get; set; }
        public string WEIGHTDATE { get; set; }
        public string LIGHTDATE { get; set; }
        public string GROSS { get; set; }
        public string TARE { get; set; }
        public string SUTTLE { get; set; }
        public string RECEIVRORG { get; set; }
        public string PK_RECEIVRORG { get; set; }
        public string STORENAME { get; set; }
        public string PK_ORDER { get; set; }
        public string VBILLCODE { get; set; }
        public string PK_BILL_B { get; set; }
        public string BATCHNO { get; set; }
        public string CREUSER { get; set; }
        public string CRETIME { get; set; }
        /// <summary>
        /// 扣重
        /// </summary>
        public string PDAKZ { get; set; }
        /// <summary>
        /// 结算重量
        /// </summary>
        public string SUTTLE2 { get; set; }
        /// <summary>
        /// 水分
        /// </summary>
        public string KS { get; set; }
        /// <summary>
        /// 花纸
        /// </summary>
        public string HZ { get; set; }
        /// <summary>
        /// 垃圾
        /// </summary>
        public string LJ { get; set; }


        public string PK_MATERIAL { get; set; }
        public string MATERIALNAME { get; set; }
        public string GPMATERIAL { get; set; }
        public string GPMATERIALNAME { get; set; }
        public string DBMATERIAL { get; set; }
        public string DBMATERIALNAME { get; set; }
        public string CROWNO { get; set; }

        public string PRICE { get; set; }
        public string CHARGE { get; set; }

        public string DEF5 { get; set; }
        public string AMOUNT { get; set; }
        // fhh/myt 220915 无人值守作废同步区块链作废 新增字段
        public string Reservation_Number { get; set; }//派车单号
        public string Invalid { get; set; }//作废标志
        // fhh/myt 220915 无人值守作废同步区块链作废 新增字段
    }


    public class EOS_OAWEIGHT
    {
        /// <summary>
        /// 自定义标题(物料描述+验收结果（退货）)
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 物料类别(化工辅料（0）/包装物（检验批）（2）)
        /// </summary>
        public string wllb { get; set; }
        /// <summary>
        /// 采购订单号
        /// </summary>
        public string cgddh1 { get; set; }
        /// <summary>
        /// 进场编号（磅单号）
        /// </summary>
        public string jcbh { get; set; }
        /// <summary>
        /// 是否属于检验时间较长物料(是/否)
        /// </summary>
        public string sfsyjysjjcwl { get; set; }
        /// <summary>
        /// 采购订单号
        /// </summary>
        public string cgddh2mx { get; set; }
        /// <summary>
        /// 行项目
        /// </summary>
        public string hxm2mx { get; set; }
        /// <summary>
        /// 物料号
        /// </summary>
        public string wlh2mx { get; set; }
        /// <summary>
        /// 物料描述
        /// </summary>
        public string wlms2mx { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string dw2mx { get; set; }
        /// <summary>
        /// 包装件数(没有时默认传空)
        /// </summary>
        public string bzjsmx { get; set; }
        /// <summary>
        /// 工厂
        /// </summary>
        public string gc { get; set; }
        /// <summary>
        /// 毛重/到货数量
        /// </summary>
        public string dhsl { get; set; }
        /// <summary>
        ///供应商编号
        /// </summary>
        public string gysbh2mx { get; set; }

        /// <summary>
        ///供应商名称
        /// </summary>
        public string gysmc2mx { get; set; }
        /// <summary>
        ///物料凭证号(103入库产生的物料凭证)
        /// </summary>
        public string wlpzh2mx { get; set; }
        /// <summary>
        ///	检验批
        /// </summary>
        public string jypmxT { get; set; }
        /// <summary>
        ///批次
        /// </summary>
        public string pc2mx { get; set; }

    }

    public class EOS_OARECHECK
    {
        /// <summary>
        /// 自定义标题(供应商+物料+复检)
        /// </summary>
        public string requestname { get; set; }
        /// <summary>
        /// 公司代码
        /// </summary>
        public string gsdm { get; set; }
        /// <summary>
        /// 供应商编码
        /// </summary>
        public string gysbm { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string gysmc { get; set; }
        /// <summary>
        /// 提单号
        /// </summary>
        public string tdh { get; set; }
        /// <summary>
        /// 第一次检验时间
        /// </summary>
        public string dycjysj { get; set; }
        /// <summary>
        /// 水分
        /// </summary>
        public string sf { get; set; }
        /// <summary>
        /// 灰分
        /// </summary>
        public string hf { get; set; }
        /// <summary>
        /// 胶蜡点
        /// </summary>
        public string jld { get; set; }
        /// <summary>
        /// 叩解度
        /// </summary>
        public string rjd { get; set; }
        /// <summary>
        /// 湿重
        /// </summary>
        public string sz { get; set; }
        /// <summary>
        /// 耐破指数
        /// </summary>
        public string npzs { get; set; }
        /// <summary>
        /// 裂断长
        /// </summary>
        public string dlc { get; set; }
        /// <summary>
        /// 耐折次数
        /// </summary>
        public string nzcs { get; set; }
        /// <summary>
        /// 销售重量比
        /// </summary>
        public string xszlb { get; set; }
    }
}