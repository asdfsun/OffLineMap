using NUCTECH.SUBWAY.Model.CommonModel;
using NUCTECH.SUBWAY.Model.DbModel;
using NUCTECH.SUBWAY.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace NUCTECH.SUBWAY.Business.DAL
{
    public class S_RoleInfoDal : BaseDal
    {

        /// <summary>
        /// 角色名是否存在
        /// </summary>
        public bool RoleNameExists(string RoleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from S_RoleInfo");
            strSql.Append(" where RoleName=@RoleName");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleName", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = RoleName;

            return db.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 角色名是否存在
        /// </summary>
        public bool RoleNameExists(int roleId, string RoleName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from S_RoleInfo");
            strSql.Append(" where RoleId<>@RoleId AND RoleName=@RoleName");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleName", SqlDbType.NVarChar,50),
                    new SqlParameter("@RoleId", SqlDbType.Int)
            };
            parameters[0].Value = RoleName;
            parameters[1].Value = roleId;
            return db.Exists(strSql.ToString(), parameters);
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(S_RoleInfoModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into S_RoleInfo(");
            strSql.Append("RoleName,RoleDesc,CreateTime)");
            strSql.Append(" values (");
            strSql.Append("@RoleName,@RoleDesc,@CreateTime)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleName", SqlDbType.NVarChar,50),
                    new SqlParameter("@RoleDesc", SqlDbType.NVarChar,500),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime)};
            parameters[0].Value = model.RoleName;
            parameters[1].Value = model.RoleDesc;
            parameters[2].Value = model.CreateTime;

            object obj = db.GetSingle(strSql.ToString(), parameters);
            if (obj == null)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(obj);
            }
        }
        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(S_RoleInfoModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_RoleInfo set ");
            strSql.Append("RoleName=@RoleName,");
            strSql.Append("RoleDesc=@RoleDesc");
            strSql.Append(" where RoleID=@RoleID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleName", SqlDbType.NVarChar,50),
                    new SqlParameter("@RoleDesc", SqlDbType.NVarChar,500),

                    new SqlParameter("@RoleID", SqlDbType.Int,4)};
            parameters[0].Value = model.RoleName;
            parameters[1].Value = model.RoleDesc;
            parameters[2].Value = model.RoleID;

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
        /// 删除一条数据
        /// </summary>
        public bool Delete(int RoleID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append(" DELETE FROM S_TreeRole WHERE RoleID=@RoleID ; ");
            strSql.Append(" delete from S_RoleInfo WHERE RoleID=@RoleID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleID", SqlDbType.Int,4)
            };
            parameters[0].Value = RoleID;

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
        /// 添加权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <param name="treeIds"></param>
        /// <returns></returns>
        internal ResultModel AddTreeRole(int roleId, string treeIds)
        {

            var result = new ResultModel { Status = false };

            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(" DELETE FROM S_TreeRole WHERE RoleID={0} ;", roleId);
                if (treeIds != null && treeIds.Length > 0)
                {
                    strSql.AppendFormat(@"
INSERT INTO S_TreeRole(RoleID, TreeID)
SELECT {0} AS RoleID, TreeID FROM S_TreeInfo
WHERE TreeID IN(
    SELECT ParentID FROM S_TreeInfo
    WHERE TreeID IN({1})
)
UNION
SELECT {0} AS RoleID, TreeID FROM S_TreeInfo
WHERE TreeID IN({1})", roleId, treeIds.TrimEnd(','));
                }
                int rows = db.ExecuteSql(strSql.ToString());
                if (rows > 0)
                    result.Status = true;
                else
                    result.ReMsg = "Not Save Role Tree Data";
            }
            catch (Exception ex)
            {
                result.ReMsg = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public S_RoleInfoModel GetModel(int RoleID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 RoleID,RoleName,RoleDesc,CreateTime from S_RoleInfo ");
            strSql.Append(" where RoleID=@RoleID");
            SqlParameter[] parameters = {
                    new SqlParameter("@RoleID", SqlDbType.Int,4)
            };
            parameters[0].Value = RoleID;

            S_RoleInfoModel model = new S_RoleInfoModel();
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
        public S_RoleInfoModel DataRowToModel(DataRow row)
        {
            S_RoleInfoModel model = new S_RoleInfoModel();
            if (row != null)
            {
                if (row["RoleID"] != null && row["RoleID"].ToString() != "")
                {
                    model.RoleID = int.Parse(row["RoleID"].ToString());
                }
                if (row["RoleName"] != null)
                {
                    model.RoleName = row["RoleName"].ToString();
                }
                if (row["RoleDesc"] != null)
                {
                    model.RoleDesc = row["RoleDesc"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
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
            strSql.Append("select RoleID,RoleName,RoleDesc,CreateTime ");
            strSql.Append(" FROM S_RoleInfo ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }
            return db.Query(strSql.ToString());
        }

        /// <summary>
        /// 根据角色id获取菜单
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<S_TreeInfoViewModel> GetRoleTreeList(int roleID)
        {
            var resultList = new List<S_TreeInfoViewModel>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  t.TreeID,t.TreeName,t.ParentID,t.TreeSort from S_TreeInfo t, S_TreeRole tr  where  t.TreeID = tr.TreeID and tr.RoleID = " + roleID + " ORDER BY TreeSort");

            var ds = db.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var model = new S_TreeInfoViewModel();
                    var row = ds.Tables[0].Rows[i];
                    if (row["TreeID"] != null && row["TreeID"].ToString() != "")
                    {
                        model.TreeId = int.Parse(row["TreeID"].ToString());
                    }
                    if (row["TreeName"] != null)
                    {
                        model.TreeName = row["TreeName"].ToString();
                    }
                    if (row["ParentID"] != null)
                    {
                        model.ParentID =Convert.ToInt32( row["ParentID"]);
                    }
                    if (row["TreeSort"] != null && row["TreeSort"].ToString() != "")
                    {
                        model.TreeSort = Convert.ToDecimal(row["TreeSort"].ToString());
                    }
                    resultList.Add(model);
                }

            }
            else
            {
                return null;
            }
            return resultList;
        }
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<S_TreeInfoViewModel> GetTreeList()
        {
            var resultList = new List<S_TreeInfoViewModel>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  t.TreeID,t.TreeName,t.ParentID,t.TreeSort from S_TreeInfo t WHERE ISNULL(T.TreeUrl,'')!='' ORDER BY t.TreeSort");

            var ds = db.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var model = new S_TreeInfoViewModel();
                    var row = ds.Tables[0].Rows[i];
                    if (row["TreeID"] != null && row["TreeID"].ToString() != "")
                    {
                        model.TreeId = int.Parse(row["TreeID"].ToString());
                    }
                    if (row["TreeName"] != null)
                    {
                        model.TreeName = row["TreeName"].ToString();
                    }
                    if (row["ParentID"] != null)
                    {
                        model.ParentID =Convert.ToInt32( row["ParentID"]);
                    }
                    if (row["TreeSort"] != null && row["TreeSort"].ToString() != "")
                    {
                        model.TreeSort = Convert.ToDecimal(row["TreeSort"].ToString());
                    }
                    resultList.Add(model);
                }
            }
            else
            {
                return null;
            }
            return resultList;
        }

    }
}

