using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Business;
using EOS.Entity;
using EOS.DataAccess;
using EOS.Repository;
using System.Data.Common;
using System.Data;
using EOS.WebApp.Areas.BaseSet.Models;
using EOS.WebApp.SxtjModels;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class DrawNumberController : Controller
    {
        //
        // GET: /BaseSet/QUALITYPERSON/
        string ModuleId = "";
        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        BD_QUALITYPOSTBLL bd_qualitypost = new BD_QUALITYPOSTBLL();
        QC_DRAWNUMBERBLL qc_drawnumber = new QC_DRAWNUMBERBLL();
        BD_QUALITYPERSONBLL bd_qualitypersonbll = new BD_QUALITYPERSONBLL();
        BD_PCRAWCYBll pcbll = new BD_PCRAWCYBll();
        VBD_PCRAWCYBll vpcbll = new VBD_PCRAWCYBll();
        VBD_ZJRESULTBll vorderbll = new VBD_ZJRESULTBll();

        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }

        public ActionResult DrawNumberIndex()
        {
            return View();
        }

        /// <summary>
        /// 订单列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>

        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetDrawNumberList(JqGridParam jqgridparam)
        {
            try
            {
                string KeyValue = "";
                Stopwatch watch = CommonHelper.TimerStart();
                List<QC_DRAWNUMBER> ListData = qc_drawnumber.GetOrderList(KeyValue);
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        //随机抽号
        [LoginAuthorize]
        public ActionResult DrawNumbers(string KeyValue)
        {
            string Message = "随机抽号完成。";
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            try
            {
                //清除表内原数据
                strSql.Clear();
                strSql.Append(string.Format("delete from QC_DRAWNUMBER WHERE 1=1"));
                database.ExecuteBySql(strSql, isOpenTrans);

                //随机数函数
                Random rd = new Random();
                //获取所有有效验级人员
                List<BD_QUALITYPERSON> qualityPersonList = bd_qualitypersonbll.GetOrderList2(KeyValue);
                foreach (BD_QUALITYPERSON qualityPerson in qualityPersonList)
                {
                    //获取所有岗位数量不为0的岗位
                    List<BD_QUALITYPOST> qualityPostList = bd_qualitypost.GetOrderList2(KeyValue);
                    if (qualityPostList.Count() != 0)
                    {
                        int num = rd.Next(0, qualityPostList.Count());
                        BD_QUALITYPOST qualityPost = qualityPostList[num];

                        string now = DateTime.Now.ToString();
                        strSql.Clear();
                        strSql.Append(string.Format("insert into QC_DRAWNUMBER (END_DATE,WORKER_ID,WORKER_NAME,WORKER_RESULT) VALUES ('{0}','{1}','{2}','{3}')",
                            now, qualityPerson.WORKER_ID, qualityPerson.WORKER_NAME, qualityPost.WORK_ID));
                        database.ExecuteBySql(strSql, isOpenTrans);

                        //岗位表内岗位数量减一
                        string workAmount = (int.Parse(qualityPost.WORK_AMOUNT) - 1).ToString();
                        strSql.Clear();
                        strSql.Append(string.Format("update BD_QUALITYPOST set WORK_AMOUNT='{0}' where ID='{1}'",
                            workAmount, qualityPost.ID));
                        database.ExecuteBySql(strSql, isOpenTrans);

                        //更新用户组权限
                        strSql.Clear();
                        strSql.Append(string.Format("update APP_HANDUSER set USERGROUP='{0}',USERGROUPNAME='{1}' where ACCOUNT='{2}'",
                            qualityPost.WORK_ID, qualityPost.WORK_NAME, qualityPerson.WORKER_ID));
                        database.ExecuteBySql(strSql, isOpenTrans);

                        //插入数据至抽号记录表
                        strSql.Clear();
                        strSql.Append(string.Format("insert into QC_DRAWNUMBER_HISTORY (END_DATE,WORKER_ID,WORKER_NAME,WORK_RESULT) VALUES ('{0}','{1}','{2}','{3}')",
                            now, qualityPerson.WORKER_ID, qualityPerson.WORKER_NAME, qualityPost.WORK_NAME));
                        database.ExecuteBySql(strSql, isOpenTrans);

                        database.Commit();
                    }
                }

                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

    }
}
