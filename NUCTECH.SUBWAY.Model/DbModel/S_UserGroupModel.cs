using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Model.DbModel
{
    public class S_UserGroupModel
    {
        public S_UserGroupModel()
        { }
        #region Model
        private int _usergroupid;
        private string _usergroupcode;
        private string _usergroupname;
        private DateTime? _createtime;
        private string _usergroupdesc;
        /// <summary>
        /// 用户组ID
        /// </summary>
        public int UserGroupID
        {
            set { _usergroupid = value; }
            get { return _usergroupid; }
        }
        /// <summary>
        /// 用户组编号
        /// </summary>
        public string UserGroupCode
        {
            set { _usergroupcode = value; }
            get { return _usergroupcode; }
        }
        /// <summary>
        /// 用户组名称
        /// </summary>
        public string UserGroupName
        {
            set { _usergroupname = value; }
            get { return _usergroupname; }
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
        /// 用户组描述
        /// </summary>
        public string UserGroupDesc
        {
            set { _usergroupdesc = value; }
            get { return _usergroupdesc; }
        }
        #endregion Model
    }
}
