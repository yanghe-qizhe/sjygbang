using EOS.Business;
using EOS.DataAccess;
using EOS.Entity;
using EOS.Repository;
using EOS.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace EOS.WebApp.Areas.CommonModule.Controllers
{
    /// <summary>
    /// �û����������
    /// </summary>
    public class UserController : PublicController<Base_User>
    {
        Base_UserBll base_userbll = new Base_UserBll();
        Base_CompanyBll base_companybll = new Base_CompanyBll();
        Base_ObjectUserRelationBll base_objectuserrelationbll = new Base_ObjectUserRelationBll();

        #region �û�����
        public ActionResult TreeJson()
        {
            DataTable DataList = base_userbll.TreeUserList();
            List<TreeJsonEntity> TreeList = new List<TreeJsonEntity>();
            if (DataHelper.IsExistRows(DataList))
            {
                foreach (DataRow row in DataList.Rows)
                {
                    TreeJsonEntity tree = new TreeJsonEntity();
                    string id = row["id"].ToString();
                    bool hasChildren = false;
                    DataTable childnode = DataHelper.GetNewDataTable(DataList, "parentid='" + id + "'");
                    if (childnode.Rows.Count > 0)
                    {
                        hasChildren = true;
                    }
                    tree.Attribute = "Type";
                    tree.AttributeValue = row["sort"].ToString();
                    tree.id = id;
                    tree.text = row["fullname"].ToString();
                    tree.value = id;
                    tree.parentId = row["parentid"].ToString();
                    tree.title = row["code"].ToString();
                    tree.isexpand = true;
                    tree.complete = true;
                    tree.hasChildren = hasChildren;
                    if (row["parentid"].ToString() == "0")
                    {
                        tree.img = "/Content/Images/Icon16/molecule.png";
                    }
                    else if (row["sort"].ToString() == "Company")
                    {
                        tree.img = "/Content/Images/Icon16/hostname.png";
                    }
                    else if (row["sort"].ToString() == "Department")
                    {
                        tree.img = "/Content/Images/Icon16/chart_organisation.png";
                    }
                    else
                    {
                        string Genderimg = "user_female.png";
                        if (row["gender"].ToString() == "��")
                        {
                            Genderimg = "user_green.png";
                        }
                        tree.img = "/Content/Images/Icon16/" + Genderimg;
                    }
                    TreeList.Add(tree);
                }
            }
            return Content(TreeList.TreeToJson());
        }
        /// <summary>
        /// ��ѯǰ��50���û���Ϣ������JSON��
        /// </summary>
        /// <param name="keywords">��ѯ�ؼ���</param>
        /// <returns></returns>
        public ActionResult Autocomplete(string keywords)
        {
            DataTable ListData = base_userbll.OptionUserList(keywords);
            return Content(ListData.ToJson());
        }
        /// <summary>
        /// ���û����������û��б�JSON
        /// </summary>
        /// <param name="keywords">��ѯ�ؼ���</param>
        /// <param name="CompanyId">��˾ID</param>
        /// <param name="DepartmentId">����ID</param>
        /// <param name="ParameterJson">�߼���ѯ����JSON</param>
        /// <param name="jqgridparam">������</param>
        /// <returns></returns>
        public ActionResult GridPageListJson(string keywords, string Enabled, string CompanyId, string DepartmentId, string ParameterJson, JqGridParam jqgridparam)
        {
            try
            {
                Stopwatch watch = CommonHelper.TimerStart();
                DataTable ListData = base_userbll.GetPageList(keywords, Enabled, CompanyId, DepartmentId, ParameterJson, ref jqgridparam);
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
                Base_SysLogBll.Instance.WriteLog("", OperationType.Query, "-1", "�쳣����" + ex.Message);
                return null;
            }
        }
        /// <summary>
        /// ���û������ύ��
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <param name="base_user">�û���Ϣ</param>
        /// <param name="base_employee">Ա����Ϣ</param>
        /// <param name="BuildFormJson">�Զ����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SubmitUserForm(string KeyValue, Base_User base_user, Base_Employee base_employee, string BuildFormJson)
        {
            string ModuleId = DESEncrypt.Decrypt(CookieHelper.GetCookie("ModuleId"));
            IDatabase database = DataFactory.Database();
            DbTransaction isOpenTrans = database.BeginTrans();
            try
            {
                string Message = KeyValue == "" ? "�����ɹ���" : "�༭�ɹ���";
                if (!string.IsNullOrEmpty(KeyValue))
                {
                    //if (KeyValue == ManageProvider.Provider.Current().UserId)
                    //{
                    //    throw new Exception("��Ȩ�ޱ༭������Ϣ");
                    //}
                    base_user.Modify(KeyValue);
                    base_employee.Modify(KeyValue);
                    database.Update(base_user, isOpenTrans);

                    //System.Text.StringBuilder strSql = new System.Text.StringBuilder();
                    //strSql.Clear();
                    //strSql.Append(string.Format("update Base_User set Code='{0}',RealName='{1}',Account='{2}',Gender='{3}',Birthday=to_date('{4}','yyyy-MM-dd hh24:mi:ss'),Mobile='{5}',Telephone='{6}',Email='{7}',OICQ='{8}',CompanyId='{9}',DepartmentId='{10}',ISCHECKDATA={11},InnerUser={12},ISCY={13},ISZY={14},Enabled={15},ZCUser={16},ZCAmount='{17}',Remark='{18}' where UserID='{19}'", base_user.Code, base_user.RealName, base_user.Account, base_user.Gender, base_user.Birthday, base_user.Mobile, base_user.Telephone, base_user.Email, base_user.OICQ, base_user.CompanyId, base_user.DepartmentId, base_user.ISCHECKDATA, base_user.InnerUser, base_user.ISCY, base_user.ISZY, base_user.Enabled, base_user.ZCUser, base_user.ZCAmount, base_user.Remark, KeyValue));
                    //database.ExecuteBySql(strSql, isOpenTrans);
                    database.Update(base_employee, isOpenTrans);
                }
                else
                {
                    base_user.Create();
                    base_user.SortCode = CommonHelper.GetInt(BaseFactory.BaseHelper().GetSortCode<Base_User>("SortCode"));
                    base_employee.Create();
                    base_employee.EmployeeId = base_user.UserId;
                    base_employee.UserId = base_user.UserId;
                    if (base_user.Birthday != null)
                    {
                        base_user.Birthday = base_user.Birthday;
                    }
                    else
                    {
                        base_user.Birthday = DateTime.Now;
                    }

                    database.Insert(base_user, isOpenTrans);
                    database.Insert(base_employee, isOpenTrans);
                    Base_DataScopePermissionBll.Instance.AddScopeDefault(ModuleId, ManageProvider.Provider.Current().UserId, base_user.UserId, "5", isOpenTrans);
                }
                Base_FormAttributeBll.Instance.SaveBuildForm(BuildFormJson, base_user.UserId, ModuleId, isOpenTrans);
                database.Commit();
                return Content(new JsonMessage { Success = true, Code = "1", Message = Message }.ToString());
            }
            catch (Exception ex)
            {
                database.Rollback();
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
        /// <summary>
        /// ��ȡ�û�ְԱ��Ϣ���󷵻�JSON
        /// </summary>
        /// <param name="KeyValue">����ֵ</param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SetUserForm(string KeyValue)
        {
            Base_User base_user = DataFactory.Database().FindEntity<Base_User>(KeyValue);
            if (base_user == null)
            {
                return Content("");
            }
            Base_Employee base_employee = DataFactory.Database().FindEntity<Base_Employee>(KeyValue);
            Base_Company base_company = DataFactory.Database().FindEntity<Base_Company>(base_user.CompanyId);
            string strJson = base_user.ToJson();
            //��˾
            strJson = strJson.Insert(1, "\"CompanyName\":\"" + base_company.FullName + "\",");
            //Ա����Ϣ
            strJson = strJson.Insert(1, base_employee.ToJson().Replace("{", "").Replace("}", "") + ",");
            //�Զ���
            strJson = strJson.Insert(1, Base_FormAttributeBll.Instance.GetBuildForm(KeyValue));
            return Content(strJson);
        }
        #endregion

        #region �޸ĵ�¼����
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public ActionResult ResetPassword()
        {
            ViewBag.Account = Request["Account"];
            return View();
        }
        /// <summary>
        /// �޸�����
        /// </summary>
        /// <param name="KeyValue">����</param>
        /// <param name="Password">������</param>
        /// <returns></returns>
        public ActionResult ResetPasswordSubmit(string KeyValue, string Password)
        {
            try
            {
                int IsOk = 0;
                Base_User base_user = DataFactory.Database().FindEntity<Base_User>(KeyValue);
                base_user.UserId = KeyValue;
                base_user.ModifyDate = DateTime.Now;
                base_user.ModifyUserId = ManageProvider.Provider.Current().UserId;
                base_user.ModifyUserName = ManageProvider.Provider.Current().UserName;
                base_user.Secretkey = Md5Helper.MD5(CommonHelper.CreateNo(), 16).ToLower();
                base_user.Password = Md5Helper.MD5(DESEncrypt.Encrypt(Password, base_user.Secretkey).ToLower(), 32).ToLower();
                base_user.Enabled = 1;
                IsOk = repositoryfactory.Repository().Update(base_user);
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, IsOk.ToString(), "�޸�����");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "�����޸ĳɹ���" }.ToString());
            }
            catch (Exception ex)
            {
                Base_SysLogBll.Instance.WriteLog(KeyValue, OperationType.Update, "-1", "�����޸�ʧ�ܣ�" + ex.Message);
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "�����޸�ʧ�ܣ�" + ex.Message }.ToString());
            }
        }
        #endregion

        #region �û���ɫ
        /// <summary>
        /// �û���ɫ
        /// </summary>
        /// <returns></returns>
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult UserRole()
        {
            return View();
        }
        /// <summary>
        /// �����û���ɫ
        /// </summary>
        /// <param name="CompanyId">��˾ID</param>
        /// <param name="UserId">�û�Id</param>
        /// <returns></returns>
        public ActionResult UserRoleList(string CompanyId, string UserId)
        {
            StringBuilder sb = new StringBuilder();
            DataTable dt = base_userbll.UserRoleList(CompanyId, UserId);
            foreach (DataRow dr in dt.Rows)
            {
                string strchecked = "";
                if (!string.IsNullOrEmpty(dr["objectid"].ToString()))//�ж��Ƿ�ѡ��
                {
                    strchecked = "selected";
                }
                sb.Append("<li title=\"" + dr["fullname"] + "(" + dr["code"] + ")" + "\" class=\"" + strchecked + "\">");
                sb.Append("<a id=\"" + dr["RoleId"] + "\"><img src=\"../../Content/Images/Icon16/role.png \">" + dr["fullname"] + "</a><i></i>");
                sb.Append("</li>");
            }
            return Content(sb.ToString());
        }
        /// <summary>
        /// �û���ɫ - �ύ����
        /// </summary>
        /// <param name="UserId">�û�ID</param>
        /// <param name="ObjectId">��ɫid:1,2,3,4,5,6</param>
        /// <returns></returns>
        public ActionResult UserRoleSubmit(string CompanyId, string UserId, string ObjectId)
        {
            try
            {
                string[] array = ObjectId.Split(',');
                int IsOk = base_objectuserrelationbll.BatchAddObject(CompanyId, UserId, array, "2");
                return Content(new JsonMessage { Success = true, Code = IsOk.ToString(), Message = "�����ɹ���" }.ToString());
            }
            catch (Exception ex)
            {
                return Content(new JsonMessage { Success = false, Code = "-1", Message = "����ʧ�ܣ�����" + ex.Message }.ToString());
            }
        }
        #endregion

        //�б�
        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult UserIndex()
        {
            return View();
        }

        [ManagerPermission(PermissionMode.Enforce)]
        public ActionResult UserForm()
        {
            return View();
        }

        #region ��������
        /// <summary>
        /// ��������
        /// </summary>
        /// <returns></returns>
        public ActionResult PersonCenter()
        {
            if (ManageProvider.Provider.Current().IsSystem)
            {
                throw new Exception("ϵͳ��������Աӵ������Ȩ��");
            }
            if (ManageProvider.Provider.Current().Gender == "��")
            {
                ViewBag.imgGender = "man.png";
            }
            else
            {
                ViewBag.imgGender = "woman.png";
            }
            ViewBag.strUserInfo = ManageProvider.Provider.Current().UserName + "��" + ManageProvider.Provider.Current().Account + "��";
            return View();
        }
        /// <summary>
        /// ��֤������
        /// </summary>
        /// <param name="OldPassword"></param>
        /// <returns></returns>
        public ActionResult ValidationOldPassword(string OldPassword)
        {
            if (ManageProvider.Provider.Current().Account == "System" || ManageProvider.Provider.Current().Account == "guest")
            {
                return Content(new JsonMessage { Success = true, Code = "0", Message = "��ǰ�˻������޸�����" }.ToString());
            }
            OldPassword = Md5Helper.MD5(DESEncrypt.Encrypt(Md5Helper.MD5(OldPassword, 32).ToLower(), ManageProvider.Provider.Current().Secretkey).ToLower(), 32).ToLower();
            if (OldPassword != ManageProvider.Provider.Current().Password)
            {
                return Content(new JsonMessage { Success = true, Code = "0", Message = "ԭ�����������������" }.ToString());
            }
            else
            {
                return Content(new JsonMessage { Success = true, Code = "1", Message = "ͨ����Ϣ��֤" }.ToString());
            }
        }
        #endregion
    }
}