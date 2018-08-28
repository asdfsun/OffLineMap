
using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using NUCTECH.SUBWAY.Model.ViewModel;

namespace NUCTECH.SUBWAY.Business.DAL
{
    /// <summary>
    /// 数据访问类:S_TreeInfo
    /// </summary>
    public partial class S_TreeInfoDal:BaseDal
    {
		public S_TreeInfoDal()
		{}
		#region  BasicMethod

        internal List<S_TreeInfoViewModel> GetListByRoleId(int roleID)
        {
            List<S_TreeInfoViewModel> resultList = new List<S_TreeInfoViewModel>();
            StringBuilder cmdText = new StringBuilder();
            if (roleID == -1)
                cmdText.AppendFormat("SELECT * FROM S_TreeInfo WHERE ISNULL(TreeUrl,'')!=''  ORDER BY TreeSort ASC", roleID);
            else
                cmdText.AppendFormat("SELECT * FROM S_TreeInfo WHERE TreeID IN ( SELECT TreeID FROM S_TreeRole WHERE RoleID={0} ) ORDER BY TreeSort ASC", roleID);
            DataSet data = db.Query(cmdText.ToString());
            if (data != null && data.Tables[0].Rows.Count > 0)
            {
                DataRowCollection rows = data.Tables[0].Rows;
                foreach (DataRow row in rows)
                {
                    S_TreeInfoViewModel tree = new S_TreeInfoViewModel();
                    tree.TreeId = Convert.IsDBNull(row["TreeId"]) ? 0 : Convert.ToInt32(row["TreeId"]);
                    tree.TreeName = Convert.IsDBNull(row["TreeName"]) ? "" : Convert.ToString(row["TreeName"]);
                    tree.ParentID = Convert.IsDBNull(row["ParentID"]) ? 0 : Convert.ToInt32(row["ParentID"]);
                    tree.TreeSort = Convert.IsDBNull(row["TreeSort"]) ? 0 : Convert.ToInt32(row["TreeSort"]);
                    tree.TreeUrl = Convert.IsDBNull(row["TreeUrl"]) ? "" : Convert.ToString(row["TreeUrl"]);
                    tree.TreeNote = Convert.IsDBNull(row["TreeNote"]) ? "" : Convert.ToString(row["TreeNote"]);
                    tree.TreeCode = Convert.IsDBNull(row["TreeCode"]) ? "" : Convert.ToString(row["TreeCode"]);
                    tree.TreePageCode = Convert.IsDBNull(row["TreePageCode"]) ? "" : Convert.ToString(row["TreePageCode"]);
                    tree.TreeDesc = Convert.IsDBNull(row["TreeDesc"]) ? "" : Convert.ToString(row["TreeDesc"]);
                    resultList.Add(tree);
                }
            }
            return resultList;
        }

		#endregion  BasicMethod

	}
}

