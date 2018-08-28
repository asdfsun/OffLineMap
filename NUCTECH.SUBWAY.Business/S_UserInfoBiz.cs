using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

using System.Web;
using System.Data;
using NUCTECH.SUBWAY.Model.ViewModel;
using NUCTECH.SUBWAY.Model.CommonModel;
using NUCTECH.SUBWAY.Common;
using NUCTECH.SUBWAY.Model.DbModel;

namespace NUCTECH.SUBWAY.Business
{
    public class S_UserInfoBiz : BaseBiz
    {
        DAL.S_UserInfoDal S_UserInfoDal = new DAL.S_UserInfoDal();
        DAL.S_UserLoginInfoDal S_UserLoginInfoDal = new DAL.S_UserLoginInfoDal();
        S_TreeInfoBiz treeInfoBiz = new S_TreeInfoBiz();
        public List<S_UserInfoViewModel> GetViewModelList(string userName, int userClass, int roleID, string userGroupId, string userStatus, string orderByField, string orderByDescending, int pageIndex, int pageSize, out int total)
        {
            return S_UserInfoDal.GetViewModelList(userName, userClass, roleID, userGroupId, userStatus, orderByField, orderByDescending, pageIndex, pageSize, out total);
        }
        /// <summary>
        /// 根据用户id获取用户所属角色
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public List<S_RoleInfoViewModel> GetRolesByUserId(string userID)
        {
            return S_UserInfoDal.GetRolesByUserId(userID);
        }
        public ResultModel Login(string userName, string password, bool isRememberLogin)
        {
            ResultModel rm = new ResultModel() { Status = false };
            try
            {
                S_UserInfoViewModel u = S_UserInfoDal.GetUserByUserAccount(userName);
                if (u == null)
                {
                    rm.ReMsg = "1|此账号不存在,请重新输入!";
                    return rm;
                }
                password = Utility.Encrypt(password + u.UserSalt);
                if (u.UserPwd != password)
                {
                    rm.ReMsg = "2|密码不正确,请重新输入!";
                    return rm;
                }

                if (u.UserStatus == "0")
                {
                    rm.ReMsg = "1|此账户已禁用,请联系管理员!";
                    return rm;
                }

                if (u.UserClass == "" && u.RoleID == 0)
                {
                    rm.ReMsg = "1|此账户无权登陆,请联系管理员!";
                    return rm;
                }


                if (u.UserClass.Contains("1"))
                {
                    u.RoleID = -1;   // -1 为超级管理员
                }
                List<S_TreeInfoViewModel> treeList = new List<S_TreeInfoViewModel>();
                treeList = treeInfoBiz.GetListByRoleId(u.RoleID);


                if (treeList.Count == 0)
                {
                    rm.ReMsg = "1|此账户无权登陆,请重新输入!";
                    return rm;
                }

                var tree = treeList.Where(i => i.ParentID != 0).OrderBy(i => i.TreeSort).FirstOrDefault();

                rm.ReturnUrl = tree.TreeUrl + "?treeId=" + treeList.FirstOrDefault(i => i.TreeId == tree.ParentID).TreeId;

                SetUserID(u.UserId);

                SetRoleMenuInfo(new LoginUserViewModel
                {
                    UserAccount = u.UserAccount,
                    UserName = u.UserName,
                    UserId = u.UserId,
                    RoleName = u.RoleName,
                    TreeInfo = treeList
                });


                rm.Status = true;
            }
            catch (SqlException sqlException)
            {
                rm.ReMsg = sqlException.Message;
                Logger.Error(string.Format("[User]:Login:{0}", " Login Failed,userName:" + userName));
            }
            catch (Exception ex)
            {
                rm.ReMsg = ex.Message;
            }
            return rm;
        }

        public bool CheckResetPassWord(string account, string pwd, DateTime sendEmailTime)
        {
            var userInfo = S_UserInfoDal.GetUserByUserAccount(account);
            bool result = (userInfo.UserPwd == pwd && DateTime.Parse(userInfo.SendEmailTime) == sendEmailTime);
            //if (result)
            //    UpdateSendEmailTime(account, DateTime.Parse("1900-01-01"));
            return result;
        }

        public ResultModel FindPwd(string account)
        {
            ResultModel result = new ResultModel() { Status = false };
            try
            {
                TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
                var timeStamp = Convert.ToInt64(ts.TotalMilliseconds); //当前时间戳
                                                                       //重设密码链接地址和页面标签
                string email, uName;
                string password;

                var userInfo = S_UserInfoDal.GetUserByUserAccount(account);
                if (userInfo == null)
                {
                    result.Status = false;
                    result.ReMsg = "账户不存在！";
                    return result;
                }
                email = userInfo.UserEmail;
                uName = userInfo.UserName;
                password = userInfo.UserPwd;
                DateTime sendTime = DateTime.Now;



                int timeliness = int.Parse(Utility.GetConfigurationValue("Timeliness"));
                var param = Utility.Encrypt(account + "|" + password + "|" + sendTime.AddMinutes(timeliness)).Replace("+", "%2B");

                var resetPwd = string.Format("<a href='http://{0}/Account/ResetPassWord?u={1}'>立即重设></a>", HttpContext.Current.Request.Url.Authority, param);
                string msg = "";
                //发送修改密码的邮件
                var isSuccess = Utility.SendMail(
                    email,
                 ref msg,
                 Utility.GetConfigurationValue("ForgetPwdTitle"),
                    Utility.GetConfigurationValue("ForgetPwdContent")
                    .Replace("name", uName)
                    .Replace("p", "<p/>")
                    .Replace("Reset", resetPwd)
                    .Replace("timeliness", timeliness.ToString())
                   );

                if (isSuccess)
                {
                    UpdateSendEmailTime(account, sendTime);
                    result.Status = true;
                }
                else
                    result.ReMsg = "邮件发送失败:" + msg;
            }
            catch (Exception ex)
            {
                result.Status = false;
                result.ReMsg = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 获取当天在线时长
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public string GetCurDayLoginTime(string userId)
        {
            //var userInfo = this.GetViewModel(userId);
            //int ss = S_UserLoginInfoDal.GetCurDayLoginTime(userInfo.UserAccount);
            string startTime = DateTime.Now.ToString("yyyy-MM-dd") + " 00:00:00";
            string endTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            int ss = Convert.ToInt32(S_UserLoginInfoDal.GetCurDayLoginSecond(userId, startTime, endTime));
            int hour = ss / (60 * 60);
            int min = (ss - hour * 60 * 60) / 60;
            return string.Format("{0}:{1}", hour, min > 9 ? min.ToString() : "0" + min.ToString());
        }

        private bool UpdateSendEmailTime(string account, DateTime sendEmailTime)
        {
            return S_UserInfoDal.UpdateSendEmailTime(account, sendEmailTime);
        }

        public List<S_UserInfoViewModel> GetEnableUserList()
        {
            return S_UserInfoDal.GetEnableUserList();
        }

        public void LoginOut()
        {
            var currUserId = GetUserID();
            if (!string.IsNullOrEmpty(GetUserID()))
            {
                S_UserInfoDal.UpdateUserLoginState(false, currUserId);
            }
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
        public ResultModel SaveUser(string userName, string password, string roles, string fileselect, string email, string userid)
        {
            string userSalf = Guid.NewGuid().ToString();
            password = Utility.Encrypt(password + userSalf);
            return S_UserInfoDal.SaveUser(userName, password, userSalf, roles, fileselect, email, userid);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public bool Delete(Guid UserID)
        {
            return S_UserInfoDal.Delete(UserID);
        }
        /// <summary>
        /// 用户启用、禁用
        /// </summary>
        public bool EnableUser(Guid UserID, int status)
        {
            return S_UserInfoDal.EnableUser(UserID, status);
        }

        public S_UserInfoModel GetModel(Guid UserID)
        {
            return S_UserInfoDal.GetModel(UserID);
        }
        public DataSet GetList(string strWhere)
        {
            return S_UserInfoDal.GetList(strWhere);
        }
        public ResultModel ResetPwd(string newPassword, string oldPassword)
        {
            ResultModel resultValue = new ResultModel() { Status = false };

            S_UserInfoViewModel u = S_UserInfoDal.GetUserByUserAccount(GetRoleMenuInfo(GetUserID()).UserAccount);
            string password = Utility.Encrypt(oldPassword + u.UserSalt);
            if (u.UserPwd == password)
            {
                password = Utility.Encrypt(newPassword + u.UserSalt);
                bool res = S_UserInfoDal.UpdatePassword(password, u.UserId);
                resultValue.Status = res;
            }
            else
                resultValue.ReMsg = "旧密码错误！";

            return resultValue;
        }

        public bool UpdatePassword(string password, string userId)
        {
            var m = S_UserInfoDal.GetModel(new Guid(userId));
            if (m != null)
            {
                var enPwd = Utility.Encrypt(password + m.UserSalt);
                return S_UserInfoDal.UpdatePassword(enPwd, userId);
            }

            return false;
        }
        public ResultModel ResetPassowrd(string account, string password)
        {
            ResultModel result = new ResultModel() { Status = false };
            S_UserInfoViewModel u = S_UserInfoDal.GetUserByUserAccount(account);

            if (u != null)
            {
                var enPwd = Utility.Encrypt(password + u.UserSalt);
                result.Status = S_UserInfoDal.UpdatePassword(enPwd, u.UserId);
            }
            return result;
        }

        /// <summary>
        /// 获取判图员列表
        /// </summary>
        /// <returns></returns>
        public List<S_UserInfoViewModel> GetUsersByUserP()
        {
            return S_UserInfoDal.GetUsersByUserP();
        }

        /// <summary>
        /// 验证账户是否唯一
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public bool AccountValid(string account, string userid)
        {
            return S_UserInfoDal.AccountValid(account, userid);
        }

        /// <summary>
        /// 验证email是否唯一
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool EmailValid(string email, string userid)
        {
            return S_UserInfoDal.EmailValid(email, userid); ;
        }

        /// <summary>
        /// 用户调整分组
        /// </summary>
        public bool ChangeUserGroup(Guid UserID, int usergroupId)
        {
            return S_UserInfoDal.ChangeUserGroup(UserID, usergroupId);
        }
        /// <summary>
        /// 判图员强制下线
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool ForcedOffline(string userId)
        {
            return S_UserInfoDal.ForcedOffline(userId);
        }
    }
}
