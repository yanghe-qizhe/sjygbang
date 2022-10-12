using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using System.Collections;
using System.Data.Common;
using System.Text;
using EOS.WebApp.Areas.BaseSet.Models;

namespace EOS.WebApp.SxtjModels
{
    public class QC_DRAWNUMBERBLL : RepositoryFactory<QC_DRAWNUMBER>
    {
        public List<QC_DRAWNUMBER> GetOrderList(string KeyValue)
        {
            JqGridParam jqgridparam = new JqGridParam();
            jqgridparam.rows = 50;
            jqgridparam.page = 0;
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT qd.END_DATE END_DATE,qd.WORKER_ID WORKER_ID,qd.WORKER_NAME WORKER_NAME,qd.WORKER_RESULT WORKER_RESULT,bq.WORK_NAME WORK_NAME FROM QC_DRAWNUMBER qd left join BD_QUALITYPOST bq on qd.WORKER_RESULT=bq.WORK_ID WHERE 1=1 ");

            #region  数据范围权限控制
            int IsCheckData = ManageProvider.Provider.Current().IsCheckData;
            string UserId = ManageProvider.Provider.Current().UserId;
            int ZCUser = ManageProvider.Provider.Current().ZCUser;  //是否管理员
            if (ZCUser == 0 && ManageProvider.Provider.Current().IsSystem == false)
            {
                if (IsCheckData == 1)
                {
                    //组织权限
                    strSql.Append(string.Format(" AND EXISTS (SELECT 1 FROM DP_SOCARSORDER_DTL B LEFT JOIN Base_DataScopePermission C on B.PK_ORG=C.RESOURCEID WHERE VDP_SOCARSORDER.ID=B.PK_DP_SOCARSORDER AND   C.objectid='{0}')", UserId));
                }
            }

            #endregion
            return Repository().FindListBySql(strSql.ToString());
        }
    }
}