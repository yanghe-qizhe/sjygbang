using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace EOS.WebApp
{
    /// <summary>
    /// 控制器公共基类
    /// 这样可以减少很多重复代码量
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class PublicController<TEntity> : Controller where TEntity : BaseEntity, new()
    {
        public readonly RepositoryFactory<TEntity> repositoryfactory = new RepositoryFactory<TEntity>();

        #region 列表
        /// <summary>
        /// 列表视图
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public virtual ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 绑定表格
        /// </summary>
        /// <param name="ParameterJson">查询条件</param>
        /// <param name="Gridpage">分页条件</param>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual JsonResult GridPageJson(string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<TEntity> ListData = new List<TEntity>();
                if (!string.IsNullOrEmpty(ParameterJson))
                {
                    List<DbParameter> parameter = new List<DbParameter>();
                    IList conditions = ParameterJson.JonsToList<Condition>();
                    string WhereSql = ConditionBuilder.GetWhereSql(conditions, out parameter);
                    ListData = repositoryfactory.Repository().FindListPage(WhereSql, parameter.ToArray(), ref jqgridparam);
                }
                else
                {
                    ListData = repositoryfactory.Repository().FindListPage(ref jqgridparam);
                }
                var JsonData = new
                {
                    total = jqgridparam.total,
                    page = jqgridparam.page,
                    records = jqgridparam.records,
                    costtime = CommonHelper.TimerEnd(watch),
                    rows = ListData,
                };
                return Json(JsonData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message + "\r\n条件：" + ParameterJson);
                return null;
            }
        }
        /// <summary>
        /// 绑定表格
        /// </summary>
        /// <param name="ParameterJson">查询条件</param>
        /// <param name="Gridpage">排序条件</param>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual JsonResult GridJson(string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                List<TEntity> ListData = new List<TEntity>();
                if (!string.IsNullOrEmpty(ParameterJson))
                {
                    List<DbParameter> parameter = new List<DbParameter>();
                    IList conditions = ParameterJson.JonsToList<Condition>();
                    string WhereSql = ConditionBuilder.GetWhereSql(conditions, out parameter, jqgridparam.sidx, jqgridparam.sord);
                    ListData = repositoryfactory.Repository().FindList(WhereSql, parameter.ToArray());
                }
                else
                {
                    ListData = repositoryfactory.Repository().FindList();
                }
                return Json(ListData, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message + "\r\n条件：" + ParameterJson);
                return null;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <param name="ParentId">判断是否有子节点</param>
        /// <returns></returns>
        [HttpPost]
        //[ManagerPermission(PermissionMode.Enforce)]
        [LoginAuthorize]
        public virtual ActionResult Delete(string KeyValue, string ParentId, string DeleteMark)
        {
            string[] array = KeyValue.Split(',');
            try
            {
                var Message = "删除失败。";
                int IsOk = 0;
                if (!string.IsNullOrEmpty(ParentId))
                {
                    if (repositoryfactory.Repository().FindCount("ParentId", ParentId) > 0)
                    {
                        throw new Exception("当前所选有子节点数据，不能删除。");
                    }
                }
                //直接删除
                if (string.IsNullOrEmpty(DeleteMark))
                {
                    IsOk = repositoryfactory.Repository().Delete(array);
                    if (IsOk > 0)
                    {
                        Message = "删除成功。";
                    }
                }
                else //物理删除
                {
                    string KeyField = DatabaseCommon.GetKeyField<TEntity>().ToString();
                    TEntity entity = new TEntity();
                    Type type = entity.GetType();
                    Hashtable ht_DeleteMark = new Hashtable();
                    ht_DeleteMark[KeyField] = KeyValue;
                    ht_DeleteMark["DeleteMark"] = 1;
                    IsOk = repositoryfactory.Repository().Update(type.Name, ht_DeleteMark, KeyField);
                }
                WriteLog(IsOk, array, Message);
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                WriteLog(-1, array, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 表单
        /// <summary>
        /// 明细视图
        /// </summary>
        /// <returns></returns>
        //[ManagerPermission(PermissionMode.Enforce)]
        [LoginAuthorize]
        public virtual ActionResult Detail()
        {
            return View();
        }
        /// <summary>
        /// 表单视图
        /// </summary>
        /// <returns></returns>
        //[ManagerPermission(PermissionMode.Enforce)]
        [LoginAuthorize]
        public virtual ActionResult Form()
        {
            return View();
        }
        /// <summary>
        /// 返回显示顺序号
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public virtual ActionResult SortCode()
        {
            string strCode = BaseFactory.BaseHelper().GetSortCode<TEntity>("SortCode").ToString();
            return Content(strCode);
        }
        /// <summary>
        /// 表单赋值
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [LoginAuthorize]
        public virtual ActionResult SetForm(string KeyValue)
        {
            TEntity entity = repositoryfactory.Repository().FindEntity(KeyValue);
            return Content(entity.ToJson());
        }
        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        [LoginAuthorize]
        public virtual ActionResult SubmitForm(TEntity entity, string KeyValue)
        {
            try
            {
                int IsOk = 0;
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    TEntity Oldentity = repositoryfactory.Repository().FindEntity(KeyValue);//获取没更新之前实体对象
                    entity.Modify(KeyValue);
                    IsOk = repositoryfactory.Repository().Update(entity);
                    this.WriteLog(IsOk, entity, Oldentity, KeyValue, Message);
                }
                else
                {
                    entity.Create();
                    IsOk = repositoryfactory.Repository().Insert(entity);
                    this.WriteLog(IsOk, entity, null, KeyValue, Message);
                }
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                this.WriteLog(-1, entity, null, KeyValue, "操作失败：" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        #endregion

        #region 写入作业日志
        /// <summary>
        /// 写入作业日志（新增、修改）
        /// </summary>
        /// <param name="IsOk">操作状态</param>
        /// <param name="entity">实体对象</param>
        /// <param name="Oldentity">之前实体对象</param>
        /// <param name="Message">备注信息</param>
        public void WriteLog(int IsOk, TEntity entity, TEntity Oldentity, string KeyValue, string Message = "")
        {
            if (!string.IsNullOrEmpty(KeyValue))
            {
                Base_SysLogBll.Instance.WriteLog(Oldentity, entity, OperationType.Update, IsOk.ToString(), Message);
            }
            else
            {
                Base_SysLogBll.Instance.WriteLog(entity, OperationType.Add, IsOk.ToString(), Message);
            }
        }
        /// <summary>
        /// 写入作业日志（删除操作）
        /// </summary>
        /// <param name="IsOk">操作状态</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="Message">备注信息</param>
        public void WriteLog(int IsOk, string[] KeyValue, string Message = "")
        {
            Base_SysLogBll.Instance.WriteLog<TEntity>(KeyValue, IsOk.ToString(), Message);
        }
        /// <summary>
        /// 写入作业日志（删除操作）
        /// </summary>
        /// <param name="IsOk">操作状态</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="Message">备注信息</param>
        public void WriteLog(int IsOk, string KeyValue, string Message = "")
        {
            string[] array = KeyValue.Split(',');
            Base_SysLogBll.Instance.WriteLog<TEntity>(array, IsOk.ToString(), Message);
        }
        #endregion


        #region 公共方法

        #region 中间表
        //中间表添加
        public void ADD_CARSORDER(string cars, string carsname, string epccode, string orderid, int ordertype, int type, IDatabase database, DbTransaction isOpenTrans)
        {
            DP_CARSORDER dp_carsorder = new DP_CARSORDER();
            dp_carsorder.ID = CommonHelper.GetGuid;
            dp_carsorder.ORDERID = orderid;
            dp_carsorder.CARS = cars;
            dp_carsorder.CARSNAME = carsname;
            dp_carsorder.EPCCODE = epccode;
            dp_carsorder.NCORDERTYPE = ordertype.ToString();
            dp_carsorder.STATUS = "0";
            dp_carsorder.TYPE = type.ToString();
            dp_carsorder.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            database.Insert<DP_CARSORDER>(dp_carsorder, isOpenTrans);
        }
        public void ADD_CARSORDER(string cars, string carsname, string epccode, string driver, string drivername, string idnum, string carrier, string carriername, string orderid, int ordertype, int type, IDatabase database, DbTransaction isOpenTrans)
        {
            DP_CARSORDER dp_carsorder = new DP_CARSORDER();
            dp_carsorder.ID = CommonHelper.GetGuid;
            dp_carsorder.ORDERID = orderid;
            dp_carsorder.CARS = cars;
            dp_carsorder.CARSNAME = carsname;
            dp_carsorder.EPCCODE = epccode;
            dp_carsorder.PK_DRIVER = driver;
            dp_carsorder.DRIVERNAME = drivername;
            dp_carsorder.IDNUM = idnum;
            dp_carsorder.PK_CARRIER = carrier;
            dp_carsorder.CARRIERNAME = carriername;
            dp_carsorder.NCORDERTYPE = ordertype.ToString();
            dp_carsorder.STATUS = "0";
            dp_carsorder.TYPE = type.ToString();
            dp_carsorder.CREATEDATE = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            database.Insert<DP_CARSORDER>(dp_carsorder, isOpenTrans);
        }
        //中间表编辑
        public void EDIT_CARSORDER(string cars, string carsname, string epccode, string driver, string drivername, string idnum, string carrier, string carriername, string orderid, IDatabase database, DbTransaction isOpenTrans)
        {
            DP_CARSORDER dp_carsorder = new DP_CARSORDER();
            dp_carsorder.ORDERID = orderid;
            dp_carsorder.CARS = cars;
            dp_carsorder.CARSNAME = carsname;
            dp_carsorder.EPCCODE = epccode;
            dp_carsorder.PK_DRIVER = driver;
            dp_carsorder.DRIVERNAME = drivername;
            dp_carsorder.IDNUM = idnum;
            dp_carsorder.PK_CARRIER = carrier;
            dp_carsorder.CARRIERNAME = carriername;
            dp_carsorder.DEF4 = 0;
            dp_carsorder.DEF5 = 0;
            database.Update(dp_carsorder, isOpenTrans);
        }

        public void EDIT_CARSORDER(string cars, string carsname, string epccode, string orderid, IDatabase database, DbTransaction isOpenTrans)
        {
            DP_CARSORDER dp_carsorder = new DP_CARSORDER();
            dp_carsorder.ORDERID = orderid;
            dp_carsorder.CARS = cars;
            dp_carsorder.CARSNAME = carsname;
            dp_carsorder.EPCCODE = epccode;
            dp_carsorder.DEF4 = 0;
            dp_carsorder.DEF5 = 0;
            database.Update(dp_carsorder, isOpenTrans);
        }
        //中间表删除
        public void DEL_CARSORDERS(string orderid, IDatabase database, DbTransaction isOpenTrans)
        {
            database.Delete("DP_CARSORDER", "ORDERID", orderid, isOpenTrans);
        }
        #endregion

        #region 车辆状态+(车皮号+车辆+司机)验证

        //车辆验证
        public JsonMessage CHECK_CARS_ORDER(string Cars, int type, string key, string def4)
        {
            JsonMessage jsonmsg = null;
            DataTable dt = new DataTable();
            string sql = "", id = "", isuse = "0", isusename = "", Message = "";
            if (!string.IsNullOrEmpty(Cars))
            {
                if (type == 6)
                {
                    #region 工程车辆
                    if (key != "")
                    {
                        sql = String.Format("select BillNo as id,isuse from DP_GCCARSORDER where rownum<=1 and (Cars='{0}' and BillNo<>'{1}') and isuse NOT IN (4,6) order by createdate desc ", Cars.Trim(), key);
                    }
                    else
                    {
                        sql = String.Format("select BillNo as id,isuse from DP_GCCARSORDER where rownum<=1 and (Cars='{0}') and isuse NOT IN (4,6) order by createdate desc ", Cars.Trim());
                    }
                    dt = DataFactory.Database().FindTableBySql(sql);
                    if (dt.Rows.Count > 0)
                    {
                        id = string.Format("{0}", dt.Rows[0]["id"]);
                        isuse = string.Format("{0}", dt.Rows[0]["isuse"]);
                        isusename = this.GetIsUseName(isuse);
                        Message = String.Format("此车辆己有工程车辆通知单、待出厂后进行派车，现有车辆业务单据号为:{0}，如有疑问请与销售处联系！", id);
                        jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                    }
                    #endregion
                }
                else
                {
                    #region//销售提货通知单
                    if (type == 2 && !string.IsNullOrEmpty(key))
                    {
                        if (string.IsNullOrEmpty(def4))
                        {
                            sql = String.Format("select id,isuse from DP_SOCARSORDER where rownum<=1 and (Cars='{0}' and id<>'{1}') and isuse NOT IN (4,6)  order by createdate desc ", Cars.Trim(), key);
                        }
                        else
                        {
                            sql = String.Format("select id,isuse from DP_SOCARSORDER where rownum<=1 and (Cars='{0}' and id<>'{1}') and DEF1='{2}' and isuse NOT IN (4,6)  order by createdate desc ", Cars.Trim(), key, def4);
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(def4))
                        {
                            sql = String.Format("select id,isuse from DP_SOCARSORDER where  rownum<=1 and (Cars='{0}') and isuse NOT IN (4,6)  order by createdate desc ", Cars.Trim());
                        }
                        else
                        {
                            sql = String.Format("select id,isuse from DP_SOCARSORDER where  rownum<=1 and (Cars='{0}')  and DEF1='{1}' and isuse NOT IN (4,6)  order by createdate desc ", Cars.Trim(), def4);
                        }
                    }
                    dt = DataFactory.Database().FindTableBySql(sql);
                    if (dt.Rows.Count > 0)
                    {
                        id = string.Format("{0}", dt.Rows[0]["id"]);
                        isuse = string.Format("{0}", dt.Rows[0]["isuse"]);
                        isusename = GetIsUseName(isuse);
                        Message = String.Format("此车辆己有发货通知单、待出厂后进行派车，现有车辆业务单据号为:{0}，如有疑问请与销售处联系！", id);
                        jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                    }

                    #endregion

                    #region//采购到货通知单

                  

                    if (type == 1 && !string.IsNullOrEmpty(key))
                    {
                        if (string.IsNullOrEmpty(def4))
                        {
                            sql = String.Format("select id,isuse from DP_POCARSORDER where rownum<=1 and (Cars='{0}' and id<>'{1}') and isuse NOT IN (4,6)  order by createdate desc ", Cars.Trim(), key);
                        }
                        else
                        {
                            sql = String.Format("select id,isuse from DP_POCARSORDER where rownum<=1 and (Cars='{0}' and id<>'{1}') and DEF1='{2}' and isuse NOT IN (4,6)  order by createdate desc ", Cars.Trim(), key, def4);
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(def4))
                        {
                            sql = String.Format("select id,isuse from DP_POCARSORDER where  rownum<=1 and (Cars='{0}') and isuse NOT IN (4,6)  order by createdate desc ", Cars.Trim());
                        }
                        else
                        {
                            sql = String.Format("select id,isuse from DP_POCARSORDER where  rownum<=1 and (Cars='{0}')  and DEF1='{1}' and isuse NOT IN (4,6)  order by createdate desc ", Cars.Trim(), def4);
                        }
                    }
                    dt = DataFactory.Database().FindTableBySql(sql);
                    if (dt.Rows.Count > 0)
                    {
                        id = string.Format("{0}", dt.Rows[0]["id"]);
                        isuse = string.Format("{0}", dt.Rows[0]["isuse"]);
                        isusename = GetIsUseName(isuse);
                        Message = String.Format("此车辆己有到货通知单、待出厂后进行派车，现有车辆业务单据号为:{0}，如有疑问请与采购处联系！", id);
                        jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                    }

                    #endregion

                    #region //其它采购通知单

                    if (type == 3 && key != "")
                    {
                        sql = String.Format("select id,isuse from DP_OTCARSORDER where  rownum<=1 and (Cars='{0}' and ID!='{1}') and isuse NOT IN (4,6) ", Cars, key);
                    }
                    else
                    {
                        sql = String.Format("select id,isuse from DP_OTCARSORDER where  rownum<=1 and  (Cars='{0}') and isuse NOT IN (4,6) ", Cars);
                    }
                    dt = DataFactory.Database().FindTableBySql(sql);
                    if (dt.Rows.Count > 0)
                    {
                        id = string.Format("{0}", dt.Rows[0]["id"]);
                        isuse = string.Format("{0}", dt.Rows[0]["isuse"]);
                        isusename = GetIsUseName(isuse);
                        Message = String.Format("此车辆己有其它采购通知单、待出厂后进行派车，现有车辆业务单据号为:{0}，如有疑问请与管理员联系！", id);
                        jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                    }

                    #endregion

                    #region//其它销售通知单

                    if (type == 4 && key != "")
                    {
                        sql = String.Format("select id,isuse from DP_OTCARSORDER where rownum<=1 and  (Cars='{0}' and ID!='{1}') and isuse NOT IN (4,6) ", Cars, key);
                    }
                    else
                    {
                        sql = String.Format("select id,isuse from DP_OTCARSORDER where rownum<=1 and  (Cars='{0}') and isuse NOT IN (4,6) ", Cars);
                    }
                    dt = DataFactory.Database().FindTableBySql(sql);
                    if (dt.Rows.Count > 0)
                    {
                        id = string.Format("{0}", dt.Rows[0]["id"]);
                        isuse = string.Format("{0}", dt.Rows[0]["isuse"]);
                        isusename = GetIsUseName(isuse);
                        Message = String.Format("此车辆己有其它销售通知单、待出厂后进行派车，现有车辆业务单据号为:{0}，如有疑问请与管理员联系！", id);
                        jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                    }

                    #endregion

                    #region//厂内倒运单

                    if (type == 5 && key != "")
                    {
                        sql = String.Format("select id,isuse from DP_DBCARSORDER where rownum<=1 and  (Cars='{0}' and ID!='{1}') and isuse NOT IN (4,6)  ", Cars.Trim(), key);
                    }
                    else
                    {
                        sql = String.Format("select id,isuse from DP_DBCARSORDER where rownum<=1 and  (Cars='{0}') and isuse NOT IN (4,6)  ", Cars.Trim());
                    }
                    dt = DataFactory.Database().FindTableBySql(sql);
                    if (dt.Rows.Count > 0)
                    {
                        id = string.Format("{0}", dt.Rows[0]["id"]);
                        isuse = string.Format("{0}", dt.Rows[0]["isuse"]);
                        isusename = GetDBIsUseName(isuse);
                        Message = String.Format("此车辆己有倒运通知单、待出厂后进行派车，现有车辆业务单据号为:{0}，如有疑问请与管理员联系！", id);
                        jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                    }

                    #endregion
                }
            }
            return jsonmsg;
        }
        //身份证验证
        public JsonMessage CHECK_CARDNUMBER_ORDER(string psdno, int type, string key, string def4)
        {
            JsonMessage jsonmsg = null;
            DataTable dt = new DataTable();
            string sql = "", id = "", isuse = "0", isusename = "", Message = "";
            if (!string.IsNullOrEmpty(psdno))
            {
                #region //销售提货单

                
                if (type == 2 && !string.IsNullOrEmpty(key))
                {
                    if (string.IsNullOrEmpty(def4))
                    {
                        sql = String.Format("select id,isuse from DP_SOCARSORDER where rownum<=1 and (PSDNO='{0}' and id<>'{1}') and isuse NOT IN (4,6)  order by createdate desc ", psdno.Trim(), key);
                    }
                    else
                    {
                        sql = String.Format("select id,isuse from DP_SOCARSORDER where rownum<=1 and (PSDNO='{0}' and id<>'{1}') and DEF1='{2}' and isuse NOT IN (4,6)  order by createdate desc ", psdno.Trim(), key, def4);
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(def4))
                    {
                        sql = String.Format("select id,isuse from DP_SOCARSORDER where  rownum<=1 and (PSDNO='{0}') and isuse NOT IN (4,6)  order by createdate desc ", psdno.Trim());
                    }
                    else
                    {
                        sql = String.Format("select id,isuse from DP_SOCARSORDER where  rownum<=1 and (PSDNO='{0}')  and DEF1='{1}' and isuse NOT IN (4,6)  order by createdate desc ", psdno.Trim(), def4);
                    }
                }
                dt = DataFactory.Database().FindTableBySql(sql);
                if (dt.Rows.Count > 0)
                {
                    id = string.Format("{0}", dt.Rows[0]["id"]);
                    isuse = string.Format("{0}", dt.Rows[0]["isuse"]);
                    isusename = this.GetIsUseName(isuse);
                    Message = String.Format("此身份证己有发货通知单、待出厂后进行派车，现有车辆业务单据号为:{0}，如有疑问请与管理处联系！", id);
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }

                #endregion

                #region //采购到货单
 

                if (type == 2 && !string.IsNullOrEmpty(key))
                {
                    if (string.IsNullOrEmpty(def4))
                    {
                        sql = String.Format("select id,isuse from DP_POCARSORDER where rownum<=1 and (PSDNO='{0}' and id<>'{1}') and isuse NOT IN (4,6)  order by createdate desc ", psdno.Trim(), key);
                    }
                    else
                    {
                        sql = String.Format("select id,isuse from DP_POCARSORDER where rownum<=1 and (PSDNO='{0}' and id<>'{1}') and DEF1='{2}' and isuse NOT IN (4,6)  order by createdate desc ", psdno.Trim(), key, def4);
                    }

                }
                else
                {
                    if (string.IsNullOrEmpty(def4))
                    {
                        sql = String.Format("select id,isuse from DP_POCARSORDER where  rownum<=1 and (PSDNO='{0}') and isuse NOT IN (4,6)  order by createdate desc ", psdno.Trim());
                    }
                    else
                    {
                        sql = String.Format("select id,isuse from DP_POCARSORDER where  rownum<=1 and (PSDNO='{0}')  and DEF1='{1}' and isuse NOT IN (4,6)  order by createdate desc ", psdno.Trim(), def4);
                    }
                }
                dt = DataFactory.Database().FindTableBySql(sql);
                if (dt.Rows.Count > 0)
                {
                    id = string.Format("{0}", dt.Rows[0]["id"]);
                    isuse = string.Format("{0}", dt.Rows[0]["isuse"]);
                    isusename = this.GetDHIsUseName(isuse);
                    Message = String.Format("此身份证己有到货通知单、待出厂后进行派车，现有车辆业务单据号为:{0}，如有疑问请与管理处联系！", id);
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }

                #endregion

                #region  //其它采购通知单

                if (type == 3 && key != "")
                {
                    sql = String.Format("select id,isuse from DP_OTCARSORDER where rownum<=1 and  (PSDNO='{0}' and ID!='{1}') and isuse NOT IN (4,6) ", psdno, key);
                }
                else
                {
                    sql = String.Format("select id,isuse from DP_OTCARSORDER where rownum<=1 and  (PSDNO='{0}') and isuse NOT IN (4,6) ", psdno);
                }
                dt = DataFactory.Database().FindTableBySql(sql);
                if (dt.Rows.Count > 0)
                {
                    id = string.Format("{0}", dt.Rows[0]["id"]);
                    isuse = string.Format("{0}", dt.Rows[0]["isuse"]);
                    isusename = this.GetIsUseName(isuse);
                    Message = String.Format("此身份证己有其它采购通知单、待出厂后进行派车，现有车辆业务单据号为:{0}，如有疑问请与管理处联系！", id);
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }

                #endregion

                #region  //其它销售通知单

                if (type == 4 && key != "")
                {
                    sql = String.Format("select id,isuse from DP_OTCARSORDER where rownum<=1 and  (PSDNO='{0}' and ID!='{1}') and isuse NOT IN (4,6) ", psdno, key);
                }
                else
                {
                    sql = String.Format("select id,isuse from DP_OTCARSORDER where rownum<=1 and  (PSDNO='{0}') and isuse NOT IN (4,6) ", psdno);
                }
                dt = DataFactory.Database().FindTableBySql(sql);
                if (dt.Rows.Count > 0)
                {
                    id = string.Format("{0}", dt.Rows[0]["id"]);
                    isuse = string.Format("{0}", dt.Rows[0]["isuse"]);
                    isusename = this.GetIsUseName(isuse);
                    Message = String.Format("此身份证己有其它销售通知单、待出厂后进行派车，现有车辆业务单据号为:{0}，如有疑问请与管理处联系！", id);
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }

                #endregion
            }
            return jsonmsg;
        }

        public string GetIsUseName(string isuse)
        {
            //1未入厂，2空厂，5发卡，7入厂，8装车，3重车，6出厂
            string name = "";
            if (isuse == "1")
            {
                name = "未入厂";
            }
            else if (isuse == "5")
            {
                name = "发卡";
            }
            else if (isuse == "4")
            {
                name = "作废";
            }
            else if (isuse == "7")
            {
                name = "入厂";
            }
            else if (isuse == "2")
            {
                name = "空车";
            }
            else if (isuse == "8")
            {
                name = "装车";
            }
            else if (isuse == "3")
            {
                name = "重车";
            }
            else if (isuse == "6")
            {
                name = "出厂";
            }
            return name;
        }

        public string GetDHIsUseName(string isuse)
        {
            //1未入厂，2空厂，5发卡，7入厂，8装车，3重车，6出厂
            string name = "";
            if (isuse == "1")
            {
                name = "未入厂";
            }
            else if (isuse == "5")
            {
                name = "发卡";
            }
            else if (isuse == "4")
            {
                name = "作废";
            }
            else if (isuse == "7")
            {
                name = "入厂";
            }
            else if (isuse == "2")
            {
                name = "重车";
            }
            else if (isuse == "8")
            {
                name = "装车";
            }
            else if (isuse == "3")
            {
                name = "空车";
            }
            else if (isuse == "6")
            {
                name = "出厂";
            }
            return name;
        }

        public string GetDBIsUseName(string isuse)
        {
            //1启用，4停用
            string name = "";
            if (isuse == "1")
            {
                name = "启用";
            }
            else if (isuse == "5")
            {
                name = "发卡";
            }
            else if (isuse == "4")
            {
                name = "作废";
            }
            else if (isuse == "7")
            {
                name = "入厂";
            }
            else if (isuse == "2")
            {
                name = "空车";
            }
            else if (isuse == "8")
            {
                name = "装车";
            }
            else if (isuse == "3")
            {
                name = "重车";
            }
            else if (isuse == "6")
            {
                name = "出厂";
            }
            return name;
        }

        //客户验证
        public JsonMessage CHECK_CUSTOMER_EXISTS(string customer)
        {
            JsonMessage jsonmsg = null;
            DataTable dt = new DataTable();
            string sql = "", Message = "";
            if (!string.IsNullOrEmpty(customer))
            {
                #region //销售委托

                sql = String.Format("select count(1) num from DP_SOCARSORDER where  (CCUSTOMERID='{0}') ", customer.Trim());
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    Message = String.Format("该客户在销售委托中已使用！");
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }

                #endregion

                #region  //过磅单

                sql = String.Format("select count(1) num from vwb_weightbs where type in(3,8) and (PK_RECEIVEORG='{0}') ", customer.Trim());
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    Message = String.Format("该客户在销售磅单中已使用！");
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }
                #endregion
            }
            return jsonmsg;
        }
        #endregion


        #region 基础数据使用验证
        //车辆验证
        public JsonMessage CHECK_CARS_EXISTS(string Cars)
        {
            JsonMessage jsonmsg = null;
            DataTable dt = new DataTable();
            string sql = "", Message = "";
            if (!string.IsNullOrEmpty(Cars))
            {

                #region 工程车辆
                int res = 0;
                //sql = String.Format("select count(1) num from DP_GCCARSORDER where (Cars='{0}')", Cars.Trim());

                //res = DataFactory.Database().FindCountBySql(sql);
                //if (res > 0)
                //{
                //    Message = String.Format("该车辆在工程车辆通知单中已使用！");
                //    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                //}
                #endregion

                #region//销售提货通知单

                sql = String.Format("select count(1) num from DP_SOCARSORDER where  (Cars='{0}')", Cars.Trim());

                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    Message = String.Format("该车辆在销售发货单中已使用！");
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }
                #endregion

                #region//采购到货通知单

                sql = String.Format("select count(1) num from DP_POCARSORDER where Cars='{0}' ", Cars.Trim());

                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    Message = String.Format("该车辆在采购到货中已使用！");
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }

                #endregion

                #region //其它入库通知单


                #endregion

                #region//其它出库通知单

                #endregion

                #region//厂内倒运单

                sql = String.Format("select count(1) num from DP_DBCARSORDER where (Cars='{0}')  ", Cars.Trim());
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    Message = String.Format("该车辆在厂际倒运单中已使用！");
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }

                #endregion

            }
            return jsonmsg;
        }

        //身份证验证
        public JsonMessage CHECK_DRIVERS_EXISTS(string drivers)
        {
            JsonMessage jsonmsg = null;
            DataTable dt = new DataTable();
            string sql = "", Message = "";
            if (!string.IsNullOrEmpty(drivers))
            {
                #region //销售提货单

                sql = String.Format("select count(1) num from DP_SOCARSORDER where  (drivers='{0}') ", drivers.Trim());
                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    Message = String.Format("该司机在销售发货单中已使用！");
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }

                #endregion

                #region //采购到货单

                sql = String.Format("select count(1) num from DP_POCARSORDER where  (drivers='{0}') ", drivers.Trim());
                res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    Message = String.Format("该司机在采购到货单中已使用！");
                    jsonmsg = new JsonMessage { Success = false, Code = "-1", Message = Message };
                }

                #endregion

                #region  //其它入库通知单


                #endregion
            }
            return jsonmsg;
        }
        #endregion



        /// <summary>
        /// 炼钢成品流水
        /// </summary>
        /// <param name="PK_CASTERNO">炉机号</param>
        /// <param name="GRADE_NAME">牌号</param>
        /// <param name="PK_MATERIAL">物料PK</param>
        /// <param name="MATERIALNAME">物料</param>
        /// <param name="AMOUNT">数量</param>
        /// <param name="AMOUNTWEIGHT">重量</param>
        /// <param name="PMETHOD">去向：0热送、1落地、2落地再送</param>
        /// <param name="SMETHOD">发送方式：0未发送1汽车2平板车3辊道</param>
        /// <param name="database"></param>
        /// <param name="isOpenTrans"></param>
        public void ADD_SM_STORE_DTL(string PK_CASTERNO, string GRADE_NAME, string PK_MATERIAL, string MATERIALNAME, decimal AMOUNT, decimal AMOUNTWEIGHT, string PMETHOD, string SMETHOD, string PK_STORE, string STORENAME, string STAREA, string STSTACK, string STFLOOR, IDatabase database, DbTransaction isOpenTrans)
        {
            QC_SM_STORE_DTL store_dtl = new QC_SM_STORE_DTL();
            store_dtl.Create();
            store_dtl.PK_CASTERNO = PK_CASTERNO;
            store_dtl.PK_CASTERNOID = "";
            store_dtl.GRADE_NAME = GRADE_NAME;
            store_dtl.PK_MATERIAL = PK_MATERIAL;
            store_dtl.MATERIALNAME = MATERIALNAME;

            store_dtl.PMETHOD = PMETHOD;
            store_dtl.SMETHOD = SMETHOD;
            //if (PMETHOD == "1")
            //{
            store_dtl.AMOUNT = AMOUNT;
            store_dtl.AMOUNTWEIGHT = AMOUNTWEIGHT;
            //}
            //else
            //{
            //    store_dtl.AMOUNT = 0-AMOUNT;
            //    store_dtl.AMOUNTWEIGHT = 0 - AMOUNTWEIGHT;
            //}
            store_dtl.PK_STORE = PK_STORE;
            store_dtl.STORENAME = STORENAME;
            store_dtl.STFLOOR = STAREA;
            store_dtl.STFLOOR = STSTACK;

            store_dtl.STFLOOR = STFLOOR;
            database.Insert<QC_SM_STORE_DTL>(store_dtl, isOpenTrans);
        }


        //连铸工序流水
        public void ADD_QC_CCRC_DTL(string PK_CASTERNOID, string PK_CASTERNO, string GRADE_NAME, string PK_MATERIAL, string MATERIALNAME, decimal AMOUNT, decimal AMOUNTWEIGHT, string PMETHOD, string SMETHOD, string PK_STORE, string STORENAME, string STAREA, string STSTACK, string STFLOOR, IDatabase database, DbTransaction isOpenTrans)
        {
            QC_CCRC_DTL store_dtl = new QC_CCRC_DTL();
            store_dtl.Create();
            store_dtl.PK_CASTERNO = PK_CASTERNO;
            store_dtl.PK_CASTERNOID = PK_CASTERNOID;
            store_dtl.GRADE_NAME = GRADE_NAME;
            store_dtl.PK_MATERIAL = PK_MATERIAL;
            store_dtl.MATERIALNAME = MATERIALNAME;

            store_dtl.PMETHOD = PMETHOD;
            store_dtl.SMETHOD = SMETHOD;

            store_dtl.AMOUNT = AMOUNT;
            store_dtl.AMOUNTWEIGHT = AMOUNTWEIGHT;

            store_dtl.PK_STORE = PK_STORE;
            store_dtl.STORENAME = STORENAME;
            store_dtl.STFLOOR = STAREA;
            store_dtl.STFLOOR = STSTACK;

            store_dtl.STFLOOR = STFLOOR;
            database.Insert<QC_CCRC_DTL>(store_dtl, isOpenTrans);
        }


        public void ADD_OL_STORE_DTL(string PK_OL_STORE_DTL, string PK_CASTERNO, string GRADE_NAME, string PK_MATERIAL, string MATERIALNAME, decimal AMOUNT, decimal AMOUNTWEIGHT, string PMETHOD, string PK_STORE, string STORENAME, string STAREA, string STSTACK, string STFLOOR, IDatabase database, DbTransaction isOpenTrans)
        {
            QC_OL_STORE_DTL store_dtl = new QC_OL_STORE_DTL();
            store_dtl.Create();
            store_dtl.PK_OL_STORE_DTL = PK_OL_STORE_DTL;
            store_dtl.PK_CASTERNO = PK_CASTERNO;
            store_dtl.GRADE_NAME = GRADE_NAME;
            store_dtl.PK_MATERIAL = PK_MATERIAL;
            store_dtl.MATERIALNAME = MATERIALNAME;

            store_dtl.PMETHOD = PMETHOD;
            store_dtl.SMETHOD = "0";

            store_dtl.AMOUNT = AMOUNT;
            store_dtl.AMOUNTWEIGHT = AMOUNTWEIGHT;

            store_dtl.PK_STORE = PK_STORE;
            store_dtl.STORENAME = STORENAME;
            store_dtl.STFLOOR = STAREA;
            store_dtl.STFLOOR = STSTACK;

            store_dtl.STFLOOR = STFLOOR;
            database.Insert<QC_OL_STORE_DTL>(store_dtl, isOpenTrans);
        }


        #endregion
    }
}
