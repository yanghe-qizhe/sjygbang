using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using EOS.Utilities;
using EOS.Entity;
using EOS.Business;
using EOS.DataAccess;
using System.Data.Common;
using EOS.Repository;
using System.Threading;
using System.IO;

namespace EOS.WebApp.Areas.BaseSet.Controllers
{
    public class SUPPLY_PRICE1_IMPORT
    {
        public string PK_SUPPLIER { get; set; }
        public string SUPPLIERNAME { get; set; }
        public string PRICE { get; set; }
        public string IMPLEMENTDATE { get; set; }


    }
    public class SUPPLYPRICE1Controller : PublicController<BD_SUPPLY_PRICE1>
    {
        string ModuleId = "";

        Base_CodeRuleBll base_coderulebll = new Base_CodeRuleBll();
        BD_SUPPLY_PRICE1Bll bd_bll = new BD_SUPPLY_PRICE1Bll();

        //列表
        [LoginAuthorize]
        public override ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取自动单据内码
        /// </summary>
        /// <returns></returns>
        private string BillCode()
        {
            string UserId = ManageProvider.Provider.Current().UserId;
            return base_coderulebll.GetBillCode(UserId, ModuleId);
        }
        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public override ActionResult Form()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.Code = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }

        /// <summary>
        /// 表单
        /// </summary>
        /// <returns></returns>
        [LoginAuthorize]
        public override ActionResult Detail()
        {
            string KeyValue = Request["KeyValue"];
            if (string.IsNullOrEmpty(KeyValue))
            {
                ModuleId = Request["ModuleId"];
                ViewBag.Code = this.BillCode();
                ViewBag.CreateUserName = ManageProvider.Provider.Current().UserName;
            }
            return View();
        }

        [LoginAuthorize]
        public ActionResult CheckOrder(string KeyValue)
        {
            //验证
            BD_SUPPLY_PRICE1 entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ENABLESTATE == "1")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该供应商服务费信息是启用状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "启用成功。";
                entity.ENABLESTATE = "1";
                database.Update(entity, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        //反审
        [LoginAuthorize]
        public ActionResult UnCheckOrder(string KeyValue)
        {
            //验证
            BD_SUPPLY_PRICE1 entity = repositoryfactory.Repository().FindEntity(KeyValue);
            if (entity.ENABLESTATE == "0")
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "该供应商服务费信息是停用状态！" }.ToString());
            }

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "停用成功。";
                entity.ENABLESTATE = "0";
                database.Update(entity, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderList(string SUPPLIER, string CUSTOMER, string MATERIAL, string MATSPEC, string MATERIALSPEC, string ISTYPE, string StartTime, string EndTime, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<BD_SUPPLY_PRICE1> ListData = bd_bll.GetOrderList(SUPPLIER, CUSTOMER, MATERIAL, MATSPEC, MATERIALSPEC, ISTYPE, StartTime, EndTime, jqgridparam);
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


        /// <summary>
        /// 订单明细列表（返回Json）
        /// </summary>
        /// <param name="ID">订单主键</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetOrderEntryList(string KeyValue)
        {
            try
            {
                var JsonData = new
                {
                    rows = bd_bll.GetOrderEntryList(KeyValue),
                };
                return Content(JsonData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 提交表单
        /// </summary>
        /// <param name="entity">实体信息</param>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitOrderForm(BD_SUPPLY_PRICE1 entity, string ModuleId, string KeyValue)
        {

            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                if (!string.IsNullOrEmpty(entity.PK_MATERIALSPEC))
                {
                    if (entity.PK_MATERIALSPEC == "&nbsp;")
                    {
                        entity.PK_MATERIALSPEC = "";
                    }
                }
                //验证
                string sql = String.Format("select count(1) from BD_SUPPLY_PRICE1 where PK_SUPPLIER ='{0}' and IMPLEMENTDATE= '{1}' ",
                                       string.IsNullOrEmpty(entity.PK_SUPPLIER) ? "" : entity.PK_SUPPLIER,
                                         string.IsNullOrEmpty(entity.IMPLEMENTDATE) ? "" : entity.IMPLEMENTDATE);
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    sql += " and ID!= '" + KeyValue + "' ";
                }

                int res = DataFactory.Database().FindCountBySql(sql);
                if (res > 0)
                {
                    return Content(new JsonMessage { Success = false, Code = "-1", Message = "服务费信息已存在" }.ToString());
                }
                //验证
                string Message = KeyValue == "" ? "新增成功。" : "编辑成功。";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    entity.Modify(KeyValue);
                    database.Update(entity, isOpenTrans);
                }
                else
                {
                    entity.Create();
                    database.Insert(entity, isOpenTrans);
                }
                //价格执行明细
                InsertMaterialPriceDtl(entity, KeyValue, database, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }

        /// <summary>
        /// 物料价格执行明细表
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="KeyValue"></param>
        /// <param name="database"></param>
        /// <param name="isOpenTrans"></param>
        /// <returns></returns>
        public int InsertMaterialPriceDtl(BD_SUPPLY_PRICE1 entity, string KeyValue, IDatabase database, DbTransaction isOpenTrans)
        {
            int res = 0;

            //验证
            string sql = String.Format("select count(1) from BD_SUPPLY_PRICE1_DTL where PK_SUPPLIER='{0}' and IMPLEMENTDATE='{1}' ",
                string.IsNullOrEmpty(entity.PK_SUPPLIER) ? "" : entity.PK_SUPPLIER,
                string.IsNullOrEmpty(entity.IMPLEMENTDATE) ? "" : entity.IMPLEMENTDATE);
            //if (!string.IsNullOrEmpty(KeyValue))
            //{
            //    sql += " and ID!= '" + KeyValue + "' ";
            //}
            res = DataFactory.Database().FindCountBySql(sql);
            if (entity.ENABLESTATE == "1" && res == 0)
            {
                BD_SUPPLY_PRICE1_DTL entity_dtl = new BD_SUPPLY_PRICE1_DTL();
                entity_dtl.Create();
                entity_dtl.MAINID = entity.ID;
                entity_dtl.PK_SUPPLIER = entity.PK_SUPPLIER;
                entity_dtl.SUPPLIERNAME = entity.SUPPLIERNAME;
                entity_dtl.MATCODE = entity.MATCODE;
                entity_dtl.PK_MATERIAL = entity.PK_MATERIAL;
                entity_dtl.MATERIALNAME = entity.MATERIALNAME;
                entity_dtl.MATSPEC = entity.MATSPEC;
                entity_dtl.PK_MATERIALSPEC = entity.PK_MATERIALSPEC;
                entity_dtl.MATSPECCODE = entity.MATSPECCODE;
                entity_dtl.MATERIALSPECNAME = entity.MATERIALSPECNAME;
                entity_dtl.GRADE = entity.GRADE;
                entity_dtl.VBDEF1 = entity.VBDEF1;
                entity_dtl.PRICE = entity.PRICE;
                entity_dtl.IMPLEMENTDATE = entity.IMPLEMENTDATE;
                entity_dtl.ENABLESTATE = entity.ENABLESTATE;
                entity_dtl.VMEMO = entity.VMEMO;
                database.Insert(entity_dtl, isOpenTrans);
            }

            return res;
        }

        //删除
        [LoginAuthorize]
        public ActionResult DeleteOrder(string KeyValue)
        {
            //验证
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = "删除成功。";
                database.Delete("BD_SUPPLY_PRICE1", "ID", KeyValue);
                database.Delete("BD_SUPPLY_PRICE1_DTL", "MAINID", KeyValue);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "操作失败：" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// 【客户管理】返回对象JSON
        /// </summary>
        /// <param name="KeyValue">主键值</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult SetFormControl(string KeyValue)
        {
            BD_SUPPLY_PRICE1 entity = repositoryfactory.Repository().FindEntity(KeyValue);
            string JsonData = entity.ToJson();
            return Content(JsonData);
        }

        /// <summary>
        /// 列表（返回Json）
        /// </summary>
        /// <param name="NAME">名称</param>
        /// <param name="jqgridparam">分页参数</param>
        /// <returns></returns>
        [LoginAuthorize]
        public ActionResult GetNewPriceList(string PK_MATERIAL, string PK_MATERIALSPEC)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                List<BD_SUPPLY_PRICE1> ListData = bd_bll.GetNewPriceList(PK_MATERIAL, PK_MATERIALSPEC);
                return Content(ListData.ToJson());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "异常错误：" + ex.Message);
                return null;
            }
        }



        #region 导入
        //导入
        [LoginAuthorize]
        public ActionResult Import()
        {
            return View();
        }

        private Dictionary<int, string> GetMapping()
        {
            Dictionary<int, string> importBOPMapping = new Dictionary<int, string>();
            importBOPMapping.Add(0, "PK_SUPPLIER");
            importBOPMapping.Add(1, "SUPPLIERNAME");
            importBOPMapping.Add(2, "PRICE");
            importBOPMapping.Add(3, "IMPLEMENTDATE");
            return importBOPMapping;
        }

        public ActionResult SubmitUploadify(HttpPostedFileBase Filedata, string ModuleId)
        {
            try
            {
                Thread.Sleep(1000);////延迟500毫秒
                string IsOk = "";
                //没有文件上传，直接返回
                if (Filedata == null || string.IsNullOrEmpty(Filedata.FileName) || Filedata.ContentLength == 0)
                {
                    return HttpNotFound();
                }
                //获取文件完整文件名(包含绝对路径)

                string fileGuid = CommonHelper.GetGuid;
                fileGuid = "LLJ" + DateTime.Now.ToString("yyMMddHHmmssffff");
                long filesize = Filedata.ContentLength;
                string FileEextension = Path.GetExtension(Filedata.FileName);
                string uploadDate = DateTime.Now.ToString("yyyyMMdd");
                string virtualPath = string.Format("~/Resource/Document/UPLOADFile/{0}/{1}{2}", uploadDate, fileGuid, FileEextension);
                string fullFileName = this.Server.MapPath(virtualPath);
                //创建文件夹，保存文件
                string path = Path.GetDirectoryName(fullFileName);
                Directory.CreateDirectory(path);
                if (!System.IO.File.Exists(fullFileName))
                {
                    Filedata.SaveAs(fullFileName);
                    try
                    {
                        Dictionary<int, string> mapping = GetMapping();
                        List<SUPPLY_PRICE1_IMPORT> list = EOS.Utilities.NPOIClass.ExcelToList<SUPPLY_PRICE1_IMPORT>(fullFileName, mapping);
                        if (list.Count > 0)
                        {
                            int res = Import_SupplyPrice(list, ModuleId);
                            System.IO.File.Delete(fullFileName);
                        }
                    }
                    catch (Exception ex)
                    {
                        IsOk = ex.Message;
                        System.IO.File.Delete(fullFileName);
                    }
                }
                return Content(IsOk);
            }
            catch (Exception ex)
            {
                return Content(ex.Message);
            }
        }

        public int Import_SupplyPrice(List<SUPPLY_PRICE1_IMPORT> list, string ModuleId)
        {
            int res = 0;
            System.Text.StringBuilder strSql = new System.Text.StringBuilder();
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                #region 实例对象
                //保存对象
                BD_SUPPLY_PRICE1 entity = null;
                //基础对象

                BD_SUPPLY sup = null;
                #endregion
                if (list.Count > 0)
                {
                    decimal price = 0;
                    foreach (SUPPLY_PRICE1_IMPORT imp in list)
                    {
                        //物料 
                        if (!string.IsNullOrEmpty(imp.PK_SUPPLIER))
                        {
                            sup = database.FindEntityByWhere<BD_SUPPLY>(string.Format(" AND ID='{0}'", imp.PK_SUPPLIER));
                            #region 供应商服务费价格主表
                            entity = database.FindEntityBySql<BD_SUPPLY_PRICE1>($"select * from BD_SUPPLY_PRICE1 where rownum<2 and ENABLESTATE=1 and PK_SUPPLIER='{imp.PK_SUPPLIER}' order by CREATIONTIME desc");

                            if (string.IsNullOrEmpty(entity.ID))
                            {
                                entity = new BD_SUPPLY_PRICE1();
                                entity.Create();
                                entity.ENABLESTATE = "1";
                                entity.PK_SUPPLIER = imp.PK_SUPPLIER;
                                entity.SUPPLIERNAME = imp.SUPPLIERNAME;
                                if (!string.IsNullOrEmpty(imp.PRICE))
                                {
                                    price = Convert.ToDecimal(imp.PRICE);
                                }
                                entity.PRICE = price;
                                //entity.IMPLEMENTDATE = imp.IMPLEMENTDATE;
                                if (!string.IsNullOrEmpty(imp.IMPLEMENTDATE))
                                {
                                    try
                                    {
                                        entity.IMPLEMENTDATE = Convert.ToDateTime(imp.IMPLEMENTDATE).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    catch
                                    {
                                        entity.IMPLEMENTDATE = imp.IMPLEMENTDATE;
                                    }
                                }
                                entity.CREATIONTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                entity.CREATOR = ManageProvider.Provider.Current().UserName;
                                res += database.Insert(entity, isOpenTrans);
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(imp.PRICE))
                                {
                                    price = Convert.ToDecimal(imp.PRICE);
                                }
                                entity.PRICE = price;
                                //entity.IMPLEMENTDATE = imp.IMPLEMENTDATE;
                                if (!string.IsNullOrEmpty(imp.IMPLEMENTDATE))
                                {
                                    try
                                    {
                                        entity.IMPLEMENTDATE = Convert.ToDateTime(imp.IMPLEMENTDATE).ToString("yyyy-MM-dd HH:mm:ss");
                                    }
                                    catch
                                    {
                                        entity.IMPLEMENTDATE = imp.IMPLEMENTDATE;
                                    }
                                }
                                entity.MODIFIEDTIME = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                                entity.MODIFIER = ManageProvider.Provider.Current().UserName;
                                res += database.Update(entity, isOpenTrans);
                            }
                            InsertMaterialPriceDtl(entity, entity.ID, database, isOpenTrans);
                            database.Commit();
                            #endregion
                        }
                    }
                }
                database.Commit();
                return res;
            }
            catch (Exception ex)
            {
                database.Rollback();
                return -1;
            }
        }
        #endregion
    }
}
