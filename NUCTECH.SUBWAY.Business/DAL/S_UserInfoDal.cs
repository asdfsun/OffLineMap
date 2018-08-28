using NUCTECH.SUBWAY.Model.CommonModel;
using NUCTECH.SUBWAY.Model.DbModel;
using NUCTECH.SUBWAY.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Business.DAL
{
    public class S_UserInfoDal : BaseDal
    {

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public bool Add(S_UserInfoModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into S_UserInfo(");
            strSql.Append("UserID,UserName,UserAccount,UserPwd,UserSalt,UserPhone,UserEmail,SendEmailTime,CreateTime,UserNick,UserStatus,UserLogin,UpdateTime,RoleID,UserImg,UserNote,DepartmentID,UserClass,UserGroupID)");
            strSql.Append(" values (");
            strSql.Append("@UserID,@UserName,@UserAccount,@UserPwd,@UserSalt,@UserPhone,@UserEmail,@SendEmailTime,@CreateTime,@UserNick,@UserStatus,@UserLogin,@UpdateTime,@RoleID,@UserImg,@UserNote,@DepartmentID,@UserClass,@UserGroupID)");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.UniqueIdentifier,16),
                    new SqlParameter("@UserName", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserAccount", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserPwd", SqlDbType.NVarChar,200),
                    new SqlParameter("@UserSalt", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserPhone", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserEmail", SqlDbType.NVarChar,50),
                    new SqlParameter("@SendEmailTime", SqlDbType.DateTime),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@UserNick", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserStatus", SqlDbType.Int,4),
                    new SqlParameter("@UserLogin", SqlDbType.Int,4),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@RoleID", SqlDbType.Int,4),
                    new SqlParameter("@UserImg", SqlDbType.NVarChar,int.MaxValue),
                    new SqlParameter("@UserNote", SqlDbType.NVarChar,500),
                    new SqlParameter("@DepartmentID", SqlDbType.Int,4),
                    new SqlParameter("@UserClass", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserGroupID", SqlDbType.Int,4)};
            parameters[0].Value = Guid.NewGuid();
            parameters[1].Value = model.UserName;
            parameters[2].Value = model.UserAccount;
            parameters[3].Value = model.UserPwd;
            parameters[4].Value = model.UserSalt;
            parameters[5].Value = model.UserPhone;
            parameters[6].Value = model.UserEmail;
            parameters[7].Value = model.SendEmailTime;
            parameters[8].Value = model.CreateTime;
            parameters[9].Value = model.UserNick;
            parameters[10].Value = model.UserStatus;
            parameters[11].Value = model.UserLogin;
            parameters[12].Value = model.UpdateTime;
            parameters[13].Value = model.RoleID;
            parameters[14].Value = model.UserImg;
            parameters[15].Value = model.UserNote;
            parameters[16].Value = model.DepartmentID;
            parameters[17].Value = model.UserClass;
            parameters[18].Value = 0; //model.UserGroupID;

            int rows = db.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal S_UserInfoViewModel GetUserByUserAccount(string userAccount)
        {
            S_UserInfoViewModel user = new S_UserInfoViewModel();
            StringBuilder cmdText = new StringBuilder();
            cmdText.AppendFormat("SELECT * FROM S_UserInfo WHERE UserAccount='{0}'", userAccount);
            DataSet data = db.Query(cmdText.ToString());
            if (data != null && data.Tables[0].Rows.Count > 0)
            {
                DataRowCollection rows = data.Tables[0].Rows;
                foreach (DataRow row in rows)
                {
                    user.UserId = Convert.IsDBNull(row["UserId"]) ? "" : Convert.ToString(row["UserId"]);
                    user.UserName = Convert.IsDBNull(row["UserName"]) ? "" : Convert.ToString(row["UserName"]);
                    user.UserAccount = Convert.IsDBNull(row["UserAccount"]) ? "" : Convert.ToString(row["UserAccount"]);
                    user.UserPwd = Convert.IsDBNull(row["UserPwd"]) ? "" : Convert.ToString(row["UserPwd"]);
                    user.UserEmail = Convert.IsDBNull(row["UserEmail"]) ? "" : Convert.ToString(row["UserEmail"]);
                    user.SendEmailTime = Convert.IsDBNull(row["SendEmailTime"]) ? "1900-01-01" : Convert.ToString(row["SendEmailTime"]);
                    user.UserSalt = Convert.IsDBNull(row["UserSalt"]) ? "" : Convert.ToString(row["UserSalt"]);
                    user.UserNick = Convert.IsDBNull(row["UserNick"]) ? "" : Convert.ToString(row["UserNick"]);
                    user.UserStatus = Convert.IsDBNull(row["UserStatus"]) ? "" : Convert.ToString(row["UserStatus"]);
                    user.UserLogin = Convert.IsDBNull(row["UserLogin"]) ? 0 : Convert.ToInt32(row["UserLogin"]);
                    user.RoleID = Convert.IsDBNull(row["RoleID"]) ? 0 : Convert.ToInt32(row["RoleID"]);
                    user.UserImg = Convert.IsDBNull(row["UserImg"]) ? "" : Convert.ToString(row["UserImg"]);
                    user.DepartmentID = Convert.IsDBNull(row["DepartmentID"]) ? 0 : Convert.ToInt32(row["DepartmentID"]);
                    user.UserClass = Convert.IsDBNull(row["UserClass"]) ? "" : Convert.ToString(row["UserClass"]);
                    user.UserGroupID = Convert.IsDBNull(row["UserGroupID"]) ? 0 : Convert.ToInt32(row["UserGroupID"]);
                }
                return user;
            }
            else
                return null;

        }

        internal List<S_UserInfoViewModel> GetEnableUserList()
        {
            List<S_UserInfoViewModel> resultList = new List<S_UserInfoViewModel>();
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("SELECT UserID,UserName,UserGroupID FROM S_UserInfo WHERE UserStatus=1 ");
            DataSet data = db.Query(cmdText.ToString());
            if (data != null && data.Tables[0].Rows.Count > 0)
            {
                DataRowCollection rows = data.Tables[0].Rows;
                foreach (DataRow row in rows)
                {
                    S_UserInfoViewModel user = new S_UserInfoViewModel();
                    user.UserId = Convert.IsDBNull(row["UserId"]) ? "" : Convert.ToString(row["UserId"]);
                    user.UserName = Convert.IsDBNull(row["UserName"]) ? "" : Convert.ToString(row["UserName"]);
                    user.UserGroupID = Convert.IsDBNull(row["UserGroupID"]) ? 0 : Convert.ToInt32(row["UserGroupID"]);
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        internal bool UpdateUserLoginState(bool state, string userId)
        {
            StringBuilder strSql = new StringBuilder();
            
            strSql.Append("update S_UserInfo set ");
            strSql.Append("UserLogin=@UserLogin,");
            strSql.Append("UpdateTime=@UpdateTime ");
            strSql.Append(" where userId=@userId ");
            SqlParameter[] parameters = {
                   new SqlParameter("@UserLogin", SqlDbType.Int),
                   new SqlParameter("@UpdateTime", SqlDbType.DateTime ),
                   new SqlParameter("@userId", SqlDbType.NVarChar)};

            parameters[0].Value = state ? 1 : 0;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = userId;
            return db.ExecuteSql(strSql.ToString(), parameters) > 0;
        }

        internal bool UpdateSendEmailTime(string userAccount, DateTime sendEmailTime)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_UserInfo set ");
            strSql.Append("SendEmailTime=@SendEmailTime,");
            strSql.Append("UpdateTime=@UpdateTime ");
            strSql.Append(" where UserAccount=@UserAccount ");
            SqlParameter[] parameters = {
                   new SqlParameter("@SendEmailTime", SqlDbType.DateTime),
                   new SqlParameter("@UpdateTime", SqlDbType.DateTime ),
                   new SqlParameter("@UserAccount", SqlDbType.NVarChar,50)};
            parameters[0].Value = sendEmailTime;
            parameters[1].Value = DateTime.Now;
            parameters[2].Value = userAccount;
            return db.ExecuteSql(strSql.ToString(), parameters) > 0;

        }

        internal List<S_UserInfoViewModel> GetUsersByUserP()
        {
            List<S_UserInfoViewModel> resultList = new List<S_UserInfoViewModel>();
            StringBuilder cmdText = new StringBuilder();
            cmdText.Append("SELECT UserID,UserName FROM S_UserInfo WHERE UserClass like '%2%' ");
            DataSet data = db.Query(cmdText.ToString());
            if (data != null && data.Tables[0].Rows.Count > 0)
            {
                DataRowCollection rows = data.Tables[0].Rows;
                foreach (DataRow row in rows)
                {
                    S_UserInfoViewModel user = new S_UserInfoViewModel();
                    user.UserId = Convert.IsDBNull(row["UserId"]) ? "" : Convert.ToString(row["UserId"]);
                    user.UserName = Convert.IsDBNull(row["UserName"]) ? "" : Convert.ToString(row["UserName"]);
                    resultList.Add(user);
                }
            }
            return resultList;
        }

        internal bool UpdatePassword(string newPassword, string userId)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_UserInfo set ");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("UserPwd=@UserPwd");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@UserPwd", SqlDbType.NVarChar),
                    new SqlParameter("@UserID", SqlDbType.NVarChar)};
            parameters[0].Value = DateTime.Now;
            parameters[1].Value = newPassword;
            parameters[2].Value = userId;
            return db.ExecuteSql(strSql.ToString(), parameters) > 0;
        }

        internal int GetUserCountByRoleId(int roleID)
        {
            string cmdText = "SELECT COUNT(0) FROM S_UserInfo WHERE RoleID=" + roleID;
            var obj = db.GetSingle(cmdText);
            return Convert.ToInt32(obj);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(Guid UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from S_UserInfo ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.UniqueIdentifier,16)          };
            parameters[0].Value = UserID;

            int rows = db.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取用户列表信息
        /// </summary>
        /// <param name="userName">用户名称</param>
        /// <param name="userClass">用户类型</param>
        ///  <param name="roleID">角色id</param>
        /// <param name="userGroupId">用户组id</param>
        /// <param name="orderByField">排序字段</param>
        /// <param name="orderByDescending">排序方式</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">每个显示个数</param>
        /// <param name="total">总条数</param>
        /// <returns></returns>
        public List<S_UserInfoViewModel> GetViewModelList(string userName, int userClass, int roleID, string userGroupId, string userStatus, string orderByField, string orderByDescending, int pageIndex, int pageSize, out int total)
        {
            var list = new List<S_UserInfoViewModel>();
            StringBuilder strSql = new StringBuilder();
            //StringBuilder strSql2 = new StringBuilder();
            //strSql.Append("select * from (");
            //strSql.Append("select row_number() over (ORDER BY " + orderByField + " " + orderByDescending + ") as nums ,* from (");
            strSql.Append("select {func} from (");
            strSql.Append("select  case UserClass when '1' then'超级管理员' when '2' then '判图员' when '3' then '开包员' else '' end as roles1 ,(select RoleName from S_RoleInfo r where r.RoleID = u.RoleID) as roles2, UserClass, RoleID, u.UserID, UserName, UserPhone,UserEmail, u.CreateTime, UserStatus, UserLogin, UpdateTime, UserImg,d.DepartName, g.UserGroupName,u.UserGroupID,(select top 1 logintime from S_UserLoginInfo s1 where s1.userid=u.userid ORDER BY logintime desc) LoginTime from S_UserInfo u  LEFT JOIN S_UserGroup g on u.usergroupId = g.UserGroupID   LEFT JOIN S_DepartmentInfo d  on u.departmentId = d.DepartID  where u.UserID is not null ");

            if (!string.IsNullOrEmpty(userName))
            {
                strSql.Append(" and  u.UserName like '%" + userName + "%' ");
            }
            if (!string.IsNullOrEmpty(userGroupId))
            {
                if (userGroupId == "-1")
                {
                    strSql.Append(" and  (u.UserGroupID <=0 or u.UserGroupID is null) ");
                }
                else if (userGroupId == "0")
                {

                }
                else
                {
                    strSql.Append(" and  u.UserGroupID = '" + userGroupId + "' ");
                }

            }
            if (!string.IsNullOrEmpty(userStatus))
            {
                //禁用
                if (userStatus == "2")
                {
                    strSql.Append(" and  u.UserStatus = 0 ");
                }//在线
                else if (userStatus == "1")
                {
                    strSql.Append(" and  u.UserStatus = 1 and UserLogin=1");
                }//离线
                else if (userStatus == "0")
                {
                    strSql.Append(" and  (u.UserStatus = 1 and (userLogin=0 or userlogin is null) or u.UserStatus=0)");
                }
            }

            if (userClass > 0)
            {
                strSql.Append(" and  u.UserClass like '%" + userClass + "%' ");
            }
            else
            {
                if (roleID > 0)
                {
                    strSql.Append(" and  u.RoleID = " + roleID + " ");
                }

            }

            strSql.Append(" ) T ");
            var str01 = strSql.ToString().Replace("{func}", " count(1) ");
            var obj = db.GetSingle(str01);
            if (obj == null)
            {
                total = 0;
            }
            else
            {
                total = Convert.ToInt32(obj);
            }
            strSql.Insert(0, "select * from ( ");
            strSql.AppendFormat("  ) TT where nums between {0} and {1}", ((pageIndex - 1) * pageSize) + 1, pageIndex * pageSize);
            var str02 = strSql.ToString().Replace("{func}", "  row_number() over (ORDER BY " + orderByField + " " + orderByDescending + ") as nums ,*  ");
            DataSet ds = db.Query(str02);
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (var item in ds.Tables[0].Rows)
                {
                    S_UserInfoViewModel model = new S_UserInfoViewModel();
                    var row = (DataRow)item;
                    if (row != null)
                    {
                        if (row["UserID"] != null && row["UserID"].ToString() != "")
                        {
                            model.UserId = row["UserID"].ToString();
                        }
                        if (row["UserName"] != null)
                        {
                            model.UserName = row["UserName"].ToString();
                        }
                        if (row["UserPhone"] != null)
                        {
                            model.UserPhone = row["UserPhone"].ToString();
                        }
                        if (row["UserEmail"] != null)
                        {
                            model.UserEmail = row["UserEmail"].ToString();
                        }
                        if (row["UserClass"] != null && !string.IsNullOrEmpty(row["UserClass"].ToString()))
                        {

                            var names = new StringBuilder();
                            var ucs = row["UserClass"].ToString().Split(',');
                            foreach (var uc in ucs)
                            {
                                switch (uc)
                                {
                                    case "1":
                                        names.Append("超级管理员");
                                        names.Append(",");
                                        break;
                                    case "2":
                                        names.Append("判图员");
                                        names.Append(",");
                                        break;
                                    case "3":
                                        names.Append("开包员");
                                        names.Append(",");
                                        break;
                                    default:
                                        break;
                                }
                            }

                            if (row["roles2"] != null && !string.IsNullOrEmpty(row["roles2"].ToString()))
                            {
                                names.Append(row["roles2"].ToString());
                            }
                            model.RoleName = names.ToString().TrimEnd(',');


                        }
                        else
                        {
                            if (row["roles2"] != null && !string.IsNullOrEmpty(row["roles2"].ToString()))
                            {
                                model.RoleName = row["roles2"].ToString();
                            }
                        }


                        if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                        {
                            model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                        }

                        if (row["UserStatus"] != null && row["UserStatus"].ToString() != "")
                        {
                            model.UserStatus = row["UserStatus"].ToString();
                            //if (row["UserStatus"].ToString() == "0" || string.IsNullOrEmpty(row["UserStatus"].ToString()))
                            //{
                            //    model.UserStatus = "禁用";
                            //}
                            //else
                            //{
                            //    if (row["UserLogin"].ToString() == "0" || string.IsNullOrEmpty(row["UserLogin"].ToString()))
                            //    {
                            //        model.UserStatus = "离线";
                            //    }
                            //    else
                            //    {
                            //        model.UserStatus = "在线";
                            //    }
                            //}

                        }
                        else
                        {
                            model.UserStatus = "";
                        }

                        if (row["UserLogin"] != null && !string.IsNullOrEmpty(row["UserLogin"].ToString()))
                        {
                            var tmp = 0;
                            int.TryParse(row["UserLogin"].ToString(), out tmp);
                            model.UserLogin = tmp;
                        }
                        else
                        {
                            model.UserLogin = 0;
                        }

                        if (row["UpdateTime"] != null && row["UpdateTime"].ToString() != "")
                        {
                            model.UpdateTime = DateTime.Parse(row["UpdateTime"].ToString());
                        }

                        if (row["UserImg"] != null)
                        {
                            model.UserIcon = row["UserImg"].ToString();
                        }
                        //UserGroupID
                        if (row["DepartName"] != null && row["DepartName"].ToString() != "")
                        {
                            model.DepartMent = row["DepartName"].ToString();
                        }
                        if (row["UserGroupName"] != null && row["UserGroupName"].ToString() != "")
                        {
                            model.UserGroupName = row["UserGroupName"].ToString();
                        }
                        else
                            model.UserGroupName = "未分组";

                        if (row["UserGroupID"] != null && row["UserGroupID"].ToString() != "")
                        {
                            model.UserGroupID = Convert.ToInt32(row["UserGroupID"].ToString());
                        }
                        if (row["LoginTime"] != null && row["LoginTime"].ToString() != "")
                        {
                            model.LoginTime =row["LoginTime"].ToString();
                        }
                        else
                        {
                            model.LoginTime = "--";
                        }
                        list.Add(model);
                    }
                }

            }
            return list;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public S_UserInfoModel GetModel(Guid UserID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserID,UserName,UserAccount,UserPwd,UserSalt,UserPhone,UserEmail,SendEmailTime,CreateTime,UserNick,UserStatus,UserLogin,UpdateTime,RoleID,UserImg,UserNote,DepartmentID,UserClass,UserGroupID from S_UserInfo ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.UniqueIdentifier,16)          };
            parameters[0].Value = UserID;

            S_UserInfoModel model = new S_UserInfoModel();
            DataSet ds = db.Query(strSql.ToString(), parameters);
            if (ds.Tables[0].Rows.Count > 0)
            {
                return DataRowToModel(ds.Tables[0].Rows[0]);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public S_UserInfoModel DataRowToModel(DataRow row)
        {
            S_UserInfoModel model = new S_UserInfoModel();
            if (row != null)
            {
                if (row["UserID"] != null && row["UserID"].ToString() != "")
                {
                    model.UserID = new Guid(row["UserID"].ToString());
                }
                if (row["UserName"] != null)
                {
                    model.UserName = row["UserName"].ToString();
                }
                if (row["UserAccount"] != null)
                {
                    model.UserAccount = row["UserAccount"].ToString();
                }
                if (row["UserPwd"] != null)
                {
                    model.UserPwd = row["UserPwd"].ToString();
                }
                if (row["UserSalt"] != null)
                {
                    model.UserSalt = row["UserSalt"].ToString();
                }
                if (row["UserPhone"] != null)
                {
                    model.UserPhone = row["UserPhone"].ToString();
                }
                if (row["UserEmail"] != null)
                {
                    model.UserEmail = row["UserEmail"].ToString();
                }
                if (row["SendEmailTime"] != null && row["SendEmailTime"].ToString() != "")
                {
                    model.SendEmailTime = DateTime.Parse(row["SendEmailTime"].ToString());
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["UserNick"] != null)
                {
                    model.UserNick = row["UserNick"].ToString();
                }
                if (row["UserStatus"] != null && row["UserStatus"].ToString() != "")
                {
                    model.UserStatus = int.Parse(row["UserStatus"].ToString());
                }
                if (row["UserLogin"] != null && row["UserLogin"].ToString() != "")
                {
                    model.UserLogin = int.Parse(row["UserLogin"].ToString());
                }
                if (row["UpdateTime"] != null && row["UpdateTime"].ToString() != "")
                {
                    model.UpdateTime = DateTime.Parse(row["UpdateTime"].ToString());
                }
                if (row["RoleID"] != null && row["RoleID"].ToString() != "")
                {
                    model.RoleID = int.Parse(row["RoleID"].ToString());
                }
                if (row["UserImg"] != null)
                {
                    model.UserImg = row["UserImg"].ToString();
                }
                if (row["UserNote"] != null)
                {
                    model.UserNote = row["UserNote"].ToString();
                }
                if (row["DepartmentID"] != null && row["DepartmentID"].ToString() != "")
                {
                    model.DepartmentID = int.Parse(row["DepartmentID"].ToString());
                }
                if (row["UserClass"] != null)
                {
                    model.UserClass = row["UserClass"].ToString();
                }
                if (row["UserGroupID"] != null && row["UserGroupID"].ToString() != "")
                {
                    model.UserGroupID = int.Parse(row["UserGroupID"].ToString());
                }
            }
            return model;
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public DataSet GetList(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            
            strSql.Append("select UserAccount as 帐号,r.RoleName as 角色名称,ug.UserGroupName as 所属组,UserClass ,UserEmail 联系方式, case UserStatus when 0 then '禁用' when 1 then '启用' else '' end 用户状态 from S_UserInfo s LEFT JOIN S_RoleInfo r on s.RoleID = r.RoleID LEFT JOIN S_UserGroup ug on s.UserGroupID = ug.UserGroupID  ");

            if (strWhere.Trim() != "")
            {
                strSql.Append(" where 1=1 " + strWhere);
            }
            return db.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据用户id获取用户所属角色
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<S_RoleInfoViewModel> GetRolesByUserId(string userID)
        {
            var result = new List<S_RoleInfoViewModel>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserClass,u.RoleID,r.RoleName,u.UserID from S_UserInfo u left join S_RoleInfo r on u.RoleID=r.RoleID where UserID = '" + userID + "'");

            var ds = db.Query(strSql.ToString());
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {


                    var row = ds.Tables[0].Rows[i];
                    if (row != null)
                    {
                        if (row["UserClass"] != null || !string.IsNullOrEmpty(row["UserClass"].ToString()))
                        {
                            var uc = row["UserClass"].ToString().TrimEnd(',').Split(',');
                            if (uc.Length > 0)
                            {
                                foreach (var item in uc)
                                {
                                    var model2 = new S_RoleInfoViewModel();
                                    switch (item.Trim())
                                    {
                                        case "1":
                                            model2.RoleName = "超级管理员";
                                            break;
                                        case "2":
                                            model2.RoleName = "判图员";
                                            break;
                                        case "3":
                                            model2.RoleName = "开包员";
                                            break;
                                        default:
                                            break;
                                    }
                                    if (!string.IsNullOrEmpty(model2.RoleName))
                                    {
                                        result.Add(model2);
                                    }

                                }
                            }
                        }


                        if (row["RoleName"] != null && !string.IsNullOrEmpty(row["RoleName"].ToString()))
                        {
                            var model = new S_RoleInfoViewModel();

                            model.RoleName = row["RoleName"].ToString();

                            if (row["RoleID"] != null && !string.IsNullOrEmpty(row["RoleID"].ToString()))
                            {
                                model.RoleId = row["RoleID"].ToString();
                            }

                            result.Add(model);

                        }



                    }


                }
            }

            return result;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="roles"></param>
        /// <param name="fileselect"></param>
        /// <param name="email"></param>
        /// <returns></returns>
        public ResultModel SaveUser(string userName, string password, string userSalf, string roles, string fileselect, string email, string userid)
        {
            var result = new ResultModel { Status = false };

            try
            {
                var userClass = new StringBuilder();
                int roleId = 0;
                if (!string.IsNullOrEmpty(roles))
                {
                    var ros = roles.Split(',');

                    for (int i = 0; i < ros.Length; i++)
                    {
                        if (ros[i].Trim() == "超级管理员")
                        {
                            userClass.Append("1");
                            userClass.Append(",");
                        }
                        else if (ros[i].Trim() == "判图员")
                        {
                            userClass.Append("2");
                            userClass.Append(",");
                        }
                        else if (ros[i].Trim() == "开包员")
                        {
                            userClass.Append("3");
                            userClass.Append(",");
                        }
                        else
                        {
                            roleId = Convert.ToInt32(ros[i]);
                        }
                    }

                }
                var b = false;
                if (string.IsNullOrEmpty(userid))
                {
                    b = Add(new S_UserInfoModel { UserID = new Guid(), UserAccount = userName, UserName = userName, UserSalt = userSalf, UserPwd = password, RoleID = roleId, UserClass = userClass.ToString().TrimEnd(','), UserImg = fileselect, UserEmail = email, CreateTime = DateTime.Now, UpdateTime = DateTime.Now, UserStatus = 1 });
                }
                else
                {
                    b = Update(userName, roleId, userClass.ToString().TrimEnd(','), fileselect, email, userid);
                }

                if (b)
                {
                    result.Status = true;
                }
                else
                {
                    result.Status = false;
                }
            }
            catch (Exception ex)
            {
                result.ReMsg = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 用户启用、禁用
        /// </summary>
        public bool EnableUser(Guid UserID, int status)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_UserInfo set UserStatus=@UserStatus");
            strSql.Append(" where UserID=@UserID  AND UserStatus<>@UserStatus");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.UniqueIdentifier,16) ,
                    new SqlParameter("@UserStatus", SqlDbType.Int) };
            parameters[0].Value = UserID;
            parameters[1].Value = status;

            int rows = db.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 验证账户是否唯一
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool AccountValid(string account, string userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select userid from S_UserInfo where userAccount=@userAccount");
            if (!string.IsNullOrEmpty(userid))
            {
                strSql.Append(" and userid !=@userid");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@userAccount", SqlDbType.VarChar) ,
                    new SqlParameter("@userid", SqlDbType.VarChar)
                   };
            parameters[0].Value = account;
            parameters[1].Value = userid;

            var ds = db.Query(strSql.ToString(), parameters);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 验证email是否唯一
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool EmailValid(string email, string userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select userid from S_UserInfo where userEmail=@userEmail");
            if (!string.IsNullOrEmpty(userid))
            {
                strSql.Append(" and userid !=@userid");
            }
            SqlParameter[] parameters = {
                    new SqlParameter("@userEmail", SqlDbType.VarChar) ,
                    new SqlParameter("@userid", SqlDbType.VarChar)
                   };
            parameters[0].Value = email;
            parameters[1].Value = userid;

            var ds = db.Query(strSql.ToString(), parameters);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(string userName, int roleId, string userClass, string fileselect, string email, string userid)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_UserInfo set ");
            strSql.Append("UserName=@UserName,");
            strSql.Append("UserAccount=@UserName,");
            strSql.Append("UserEmail=@UserEmail,");
            strSql.Append("UpdateTime=@UpdateTime,");
            strSql.Append("RoleID=@RoleID,");
            strSql.Append("UserImg=@UserImg,");
            strSql.Append("UserClass=@UserClass ");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserName", SqlDbType.NVarChar,50),

                    new SqlParameter("@UserEmail", SqlDbType.NVarChar,50),
                    new SqlParameter("@UpdateTime", SqlDbType.DateTime),
                    new SqlParameter("@RoleID", SqlDbType.Int,4),
                    new SqlParameter("@UserImg", SqlDbType.NVarChar,int.MaxValue),
                    new SqlParameter("@UserClass", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserID", SqlDbType.UniqueIdentifier,16)};
            parameters[0].Value = userName;
            parameters[1].Value = email;
            parameters[2].Value = DateTime.Now;
            parameters[3].Value = roleId;
            parameters[4].Value = fileselect;
            parameters[5].Value = userClass;
            parameters[6].Value = new Guid(userid);

            int rows = db.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
       
        /// <summary>
        /// 用户调整分组
        /// </summary>
        public bool ChangeUserGroup(Guid UserID, int usergroupId)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_UserInfo set UserGroupID=@UserGroupID");
            strSql.Append(" where UserID=@UserID ");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserID", SqlDbType.UniqueIdentifier,16) ,
                    new SqlParameter("@UserGroupID", SqlDbType.Int) };
            parameters[0].Value = UserID;
            parameters[1].Value = usergroupId;

            int rows = db.ExecuteSql(strSql.ToString(), parameters);
            if (rows > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public bool ForcedOffline(string userId)
        {
            try
            {
                var sqlstr = "update s_userinfo set userlogin=0 where userid='" + userId + "'";
                var count= db.ExecuteSql(sqlstr);
                if(count>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
