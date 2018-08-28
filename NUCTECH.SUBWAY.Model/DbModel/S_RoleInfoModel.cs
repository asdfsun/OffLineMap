using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Model.DbModel
{
    public class S_RoleInfoModel
    {
        public S_RoleInfoModel()
        { }
        #region Model
        private int _roleid;
        private string _rolename;
        private string _roledesc;
        private DateTime? _createtime;
        /// <summary>
        /// 角色ID
        /// </summary>
        public int RoleID
        {
            set { _roleid = value; }
            get { return _roleid; }
        }
        /// <summary>
        /// 角色名称
        /// </summary>
        public string RoleName
        {
            set { _rolename = value; }
            get { return _rolename; }
        }
        /// <summary>
        /// 角色描述
        /// </summary>
        public string RoleDesc
        {
            set { _roledesc = value; }
            get { return _roledesc; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime
        {
            set { _createtime = value; }
            get { return _createtime; }
        }
        #endregion Model

    }
}
