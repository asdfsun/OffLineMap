using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Model.DbModel
{
    public class S_UserInfoModel
    {
        public S_UserInfoModel()
        { }
        #region Model
        private Guid _userid;
        private string _username;
        private string _useraccount;
        private string _userpwd;
        private string _usersalt;
        private string _userphone;
        private string _useremail;
        private DateTime? _sendemailtime;
        private DateTime? _createtime;
        private string _usernick;
        private int? _userstatus;
        private int? _userlogin;
        private DateTime? _updatetime;
        private int? _roleid;
        private string _userimg;
        private string _usernote;
        private int? _departmentid;
        private string _userclass;
        private int? _usergroupid;
        /// <summary>
        /// 
        /// </summary>
        public Guid UserID
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserAccount
        {
            set { _useraccount = value; }
            get { return _useraccount; }
        }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string UserPwd
        {
            set { _userpwd = value; }
            get { return _userpwd; }
        }
        /// <summary>
        /// 混淆码
        /// </summary>
        public string UserSalt
        {
            set { _usersalt = value; }
            get { return _usersalt; }
        }
        /// <summary>
        /// 用户手机
        /// </summary>
        public string UserPhone
        {
            set { _userphone = value; }
            get { return _userphone; }
        }
        /// <summary>
        /// 用户邮箱
        /// </summary>
        public string UserEmail
        {
            set { _useremail = value; }
            get { return _useremail; }
        }
        /// <summary>
        /// 发送邮件时间
        /// </summary>
        public DateTime? SendEmailTime
        {
            set { _sendemailtime = value; }
            get { return _sendemailtime; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        /// <summary>
        /// 用户昵称
        /// </summary>
        public string UserNick
        {
            set { _usernick = value; }
            get { return _usernick; }
        }
        /// <summary>
        /// 用户状态
        /// </summary>
        public int? UserStatus
        {
            set { _userstatus = value; }
            get { return _userstatus; }
        }
        /// <summary>
        /// 登录状态
        /// </summary>
        public int? UserLogin
        {
            set { _userlogin = value; }
            get { return _userlogin; }
        }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime? UpdateTime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }
        /// <summary>
        /// 所属角色
        /// </summary>
        public int? RoleID
        {
            set { _roleid = value; }
            get { return _roleid; }
        }
        /// <summary>
        /// 用户头像
        /// </summary>
        public string UserImg
        {
            set { _userimg = value; }
            get { return _userimg; }
        }
        /// <summary>
        /// 用户备注
        /// </summary>
        public string UserNote
        {
            set { _usernote = value; }
            get { return _usernote; }
        }
        /// <summary>
        /// 所属部门
        /// </summary>
        public int? DepartmentID
        {
            set { _departmentid = value; }
            get { return _departmentid; }
        }
        /// <summary>
        /// 用户类型(1,2,3)
        /// </summary>
        public string UserClass
        {
            set { _userclass = value; }
            get { return _userclass; }
        }
        /// <summary>
        /// 所属用户组
        /// </summary>
        public int? UserGroupID
        {
            set { _usergroupid = value; }
            get { return _usergroupid; }
        }
        #endregion Model
    }
}
