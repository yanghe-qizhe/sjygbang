using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using SqlSugar;

namespace EOS.WebService
{
    public class DbConfig
    {
        public string ConnStr { set; get; }
        public DbType Type { set; get; }
    }
    /// <summary>
    /// 
    /// </summary>
    public static class DbService
    {
        public static string ConnName { set; get; }
        public static Dictionary<string, DbConfig> ListDbConfig = new Dictionary<string, DbConfig>();

        static DbService()
        {
            ListDbConfig = new Dictionary<string, DbConfig>();
            foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings)
            {
                DbType type = DbType.Oracle;
                bool isTrue = true;
                switch (item.ProviderName)
                {
                    case "SqlServer":
                        type = DbType.SqlServer;
                        break;
                    case "MySql":
                        type = DbType.MySql;
                        break;
                    case "Oracle":
                        type = DbType.Oracle;
                        break;
                    case "Sqlite":
                        type = DbType.Sqlite;
                        break;
                    default:
                        isTrue = false;
                        break;
                }
                if (isTrue)
                {
                    ListDbConfig.Add(item.Name, new DbConfig()
                    {
                        ConnStr = item.ConnectionString,
                        Type = type
                    });
                }

            }
        }
        /// <summary>
        ///服务命令
        /// </summary>
        /// <typeparam name="Outsourcing">外包服务类型</typeparam>
        /// <param name="func"></param>
        public static void Command<Outsourcing>(Action<SqlSugarClient, Outsourcing> func,
            Action<Exception> Exfunc = null) where Outsourcing : class, new()
        {
            Command<Outsourcing>(func, ConnName, Exfunc);
        }
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static SqlSugarClient GetDB(string connName)
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ListDbConfig[connName].ConnStr, //必填
                DbType = ListDbConfig[connName].Type, //必填
                IsAutoCloseConnection = true, //默认false
                InitKeyType = InitKeyType.SystemTable
            }); //默认SystemTable
        }
        /// <summary>
        /// 创建数据库连接
        /// </summary>
        /// <param name="connName"></param>
        /// <returns></returns>
        public static SqlSugarClient GetDB()
        {
            return new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = ListDbConfig[ConnName].ConnStr, //必填
                DbType = ListDbConfig[ConnName].Type, //必填
                IsAutoCloseConnection = true, //默认false
                InitKeyType = InitKeyType.SystemTable
            }); //默认SystemTable
        }
        /// <summary>
        ///服务命令
        /// </summary>
        /// <typeparam name="Outsourcing">外包服务类型</typeparam>
        /// <param name="func"></param>
        public static void Command<Outsourcing>(Action<SqlSugarClient, Outsourcing> func, string ConnName, Action<Exception> Exfunc) where Outsourcing : class, new()
        {
            try
            {
                //if (string.IsNullOrEmpty(ConnName))
                //    ConnName = "Conn";
                using (SqlSugarClient Db = GetDB(ConnName))
                {
                    var o = new Outsourcing();
                    func(Db, o);
                    o = null;//及时释放对象 
                    //_db 会在http请求结束前执行 dispose 
                }
            }
            catch (Exception ex)
            {
                //在这里可以处理所有controller的异常
                //获错误写入日志
                if (Exfunc != null)
                {
                    Exfunc(ex);
                }
                else
                {
                    throw ex;
                }
            }
        }
    }
}