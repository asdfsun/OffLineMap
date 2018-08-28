using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Model.ViewModel
{
    public class S_UserInfoViewModel
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string UserPhone { get; set; }

        /// <summary>
        /// 联系方式
        /// </summary>
        public string UserEmail { get; set; }
        
        /// <summary>
        /// 用户角色
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public string DepartMent{get;set;}
        /// <summary>
        /// 用户图标
        /// </summary>
        public string UserIcon { get; set; }
        /// <summary>
        /// 用户状态
        /// </summary>
        public string UserStatus { get; set; }
        /// <summary>
        /// 是否登录
        /// </summary>
        public int UserLogin { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string UserGroupName { get; set; }
        /// <summary>
        /// 最后一次登录时间
        /// </summary>
        public string LoginTime { get; set; }

        public string UserAccount { get; set; }
        public string UserPwd { get; set; }
        public string UserSalt { get; set; }
        public string UserNick { get; set; }
        public int RoleID { get; set; }
        public string UserImg { get; set; }
        public int DepartmentID { get; set; }
        public string UserClass { get; set; }
        public int UserGroupID { get; set; }
        public string SendEmailTime { get; set; }
    }
}
