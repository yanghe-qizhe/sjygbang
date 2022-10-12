using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EOS.Entity;
using EOS.Utilities;
using EOS.DataAccess;
using System.Data.Common;
using EOS.Repository;
using EOS.Business;

namespace EOS.WebApp.Areas.ExampleModule.Controllers
{
    public class ICCardController : PublicController<VZC_WEIGHT>
    {
        VZC_WEIGHTBll orderbll = new VZC_WEIGHTBll();
        [LoginAuthorize]
        public ActionResult SetICCard(string KeyValue, string ICCard, string ICNumber)
        {
            //验证
            VZC_WEIGHT entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.STATUS == "出厂")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该门禁单已出厂！" }.ToString());
            }
            else if (entity.STATUS == "作废")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该门禁单已作废！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            string Account = ManageProvider.Provider.Current().Account;
            string message = "";
            try
            {

                string Message = "补卡成功。";
                entity.MEMO = "补卡成功";


                System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                strSql.Append(string.Format("update wb_weight_task set ICCARD='{0}',ICNUMBER='{1}' where PK_TASK='{2}'", ICCard, ICNumber, KeyValue));

                int res = repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                strSql.Clear();
                strSql.Append(string.Format("update wb_weight set ICCARD='{0}' where PK_TASK='{1}'", ICCard, KeyValue));
                res += repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                //更新收发货表
                strSql.Clear();
                strSql.Append(string.Format("update App_Handorder set ICCARD='{0}' where BILLNO in(select billno from wb_weight   where PK_TASK='{1}')", ICCard, KeyValue));
                res += repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);

                if (entity.DATATYPE == "1")
                {  //电商业务
                    VWB_WEIGHTBSBll orderbll = new VWB_WEIGHTBSBll();

                    string strWhere = string.Format(" where pk_task='{0}'", KeyValue);
                    VWB_WEIGHTBS entity_weight = orderbll.GetEntity(strWhere, "VWB_WEIGHTBS");
                    if (!string.IsNullOrEmpty(entity_weight.PK_ORDER_B))
                    {
                        strSql.Clear();
                        strSql.Append(string.Format("delete BASE_FULLICCARD where bl_no='{0}'", entity_weight.PK_ORDER_B));
                        res += repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);

                        strSql.Clear();
                        strSql.Append(string.Format("insert into BASE_FULLICCARD (bl_no,ICCard,CardNum,BK_Time,upload) values('{0}','{1}','{2}','{3}',0)", entity_weight.PK_ORDER_B, ICCard, ICNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                        res += repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                    }
                    else
                    {
                        LG_QY_TASKBll lg_orderbll = new LG_QY_TASKBll();
                        LG_QY_TASK lg_qy_entity = lg_orderbll.SetFormControl(KeyValue);
                        if (!string.IsNullOrEmpty(lg_qy_entity.BL_NO))
                        {
                            strSql.Clear();
                            strSql.Append(string.Format("delete BASE_FULLICCARD where bl_no='{0}'", lg_qy_entity.BL_NO));
                            res += repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                            strSql.Clear();
                            strSql.Append(string.Format("insert into BASE_FULLICCARD (bl_no,ICCard,CardNum,BK_Time,upload) values('{0}','{1}','{2}','{3}',0)", lg_qy_entity.BL_NO, ICCard, ICNumber, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                            res += repositoryfactory.Repository().ExecuteBySql(strSql, isOpenTrans);
                        }
                    }

                }
                database.Commit();
                if (string.IsNullOrEmpty(ICNumber))
                {
                    message = string.Format("IC卡{0}补卡为IC卡{1}成功", entity.ICCARD, ICCard);
                }
                else
                {
                    message = string.Format("IC卡{0}({1})补卡为IC卡{2}({3})成功", entity.ICCARD, entity.ICNUMBER, ICCard, ICNumber);
                }
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Other, "1", message);
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                if (string.IsNullOrEmpty(ICNumber))
                {
                    message = string.Format("IC卡{0}补卡为IC卡{1}失败:{2}", entity.ICCARD, ICCard, ex.Message);
                }
                else
                {
                    message = string.Format("IC卡{0}({1})补卡为IC卡{2}({3})失败:{4}", entity.ICCARD, entity.ICNUMBER, ICCard, ICNumber, ex.Message);
                }
                Base_SysLogBll.Instance.WriteLog(Account, OperationType.Login, "-1", message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        [LoginAuthorize]
        public ActionResult ICCardIndex()
        {
            return View();
        }


        [LoginAuthorize]
        public ActionResult ICCardDetail()
        {
            return View();
        }
        /// <summary>
        /// 【查询页面】
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult ICCardQuery()
        {
            return View();
        }


    }
}
