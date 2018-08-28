using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Model.ViewModel
{
    public class LoginUserViewModel
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
        public string UserEmail { get; set; }

        /// <summary>
        /// 用户角色
        /// </summary>
        public string RoleName { get; set; }
        /// <summary>
        /// 所属部门
        /// </summary>
        public string DepartMent { get; set; }
        /// <summary>
        /// 用户图标
        /// </summary>
        public string UserIcon { get; set; }
        /// <summary>
        /// 最后登录时间
        /// </summary>
        public DateTime UpdateTime { get; set; }
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string UserGroupName { get; set; }
        /// <summary>
        /// 用户帐号
        /// </summary>
        public string UserAccount { get; set; }
        /// <summary>
        /// 所属权限集合
        /// </summary>
        public List<S_TreeInfoViewModel> TreeInfo { get; set; }
    }
}
