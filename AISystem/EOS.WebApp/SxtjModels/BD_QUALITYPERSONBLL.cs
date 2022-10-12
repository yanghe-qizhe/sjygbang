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
    public class BD_QUALITYPERSONBLL : RepositoryFactory<BD_QUALITYPERSON>
    {
        public List<BD_QUALITYPERSON> GetOrderList(string KeyValue)
        {
            JqGridParam jqgridparam = new JqGridParam();
            jqgridparam.rows = 50;
            jqgridparam.page = 0;
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM BD_QUALITYPERSON WHERE 1=1 ");

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

        public List<BD_QUALITYPERSON> GetOrderList2(string KeyValue)
        {
            JqGridParam jqgridparam = new JqGridParam();
            jqgridparam.rows = 50;
            jqgridparam.page = 0;
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM BD_QUALITYPERSON WHERE VALID='1' ");

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

        public BD_QUALITYPERSON GetOrder(string KeyValue)
        {
            JqGridParam jqgridparam = new JqGridParam();
            jqgridparam.rows = 50;
            jqgridparam.page = 0;
            StringBuilder strSql = new StringBuilder();
            List<DbParameter> parameter = new List<DbParameter>();
            strSql.Append(@"SELECT * FROM BD_QUALITYPERSON WHERE 1=1 ");
            if (!string.IsNullOrEmpty(KeyValue))
            {
                strSql.Append(" AND ID='" + KeyValue + "'");
                //strSql.Append(" AND MAINID=" + DbHelper.DbParmChar + "MAINID ");
                //parameter.Add(DbFactory.CreateDbParameter(DbHelper.DbParmChar + "MAINID", KeyValue));
            }

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
            return Repository().FindEntityBySql(strSql.ToString());
        }
    }
}