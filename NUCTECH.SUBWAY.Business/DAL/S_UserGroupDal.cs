using NUCTECH.SUBWAY.Model.DbModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace NUCTECH.SUBWAY.Business.DAL
{
    public class S_UserGroupDal : BaseDal
    {
        public S_UserGroupDal()
        { }
        #region  BasicMethod

        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(S_UserGroupModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into S_UserGroup(");
            strSql.Append("UserGroupCode,UserGroupName,CreateTime,UserGroupDesc)");
            strSql.Append(" values (");
            strSql.Append("@UserGroupCode,@UserGroupName,@CreateTime,@UserGroupDesc)");
            strSql.Append(";select @@IDENTITY");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserGroupCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserGroupName", SqlDbType.NVarChar,50),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@UserGroupDesc", SqlDbType.NVarChar,500)};
            parameters[0].Value = model.UserGroupCode;
            parameters[1].Value = model.UserGroupName;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.UserGroupDesc;

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
        public bool Update(S_UserGroupModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update S_UserGroup set ");
            strSql.Append("UserGroupCode=@UserGroupCode,");
            strSql.Append("UserGroupName=@UserGroupName,");
            strSql.Append("CreateTime=@CreateTime,");
            strSql.Append("UserGroupDesc=@UserGroupDesc");
            strSql.Append(" where UserGroupID=@UserGroupID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserGroupCode", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserGroupName", SqlDbType.NVarChar,50),
                    new SqlParameter("@CreateTime", SqlDbType.DateTime),
                    new SqlParameter("@UserGroupDesc", SqlDbType.NVarChar,500),
                    new SqlParameter("@UserGroupID", SqlDbType.Int,4)};
            parameters[0].Value = model.UserGroupCode;
            parameters[1].Value = model.UserGroupName;
            parameters[2].Value = model.CreateTime;
            parameters[3].Value = model.UserGroupDesc;
            parameters[4].Value = model.UserGroupID;

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
        public bool Delete(int UserGroupID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("UPDATE S_UserInfo SET UserGroupID=0 ");
            strSql.Append(" where UserGroupID=@UserGroupID ;");
            strSql.Append(" delete from S_UserGroup ");
            strSql.Append(" where UserGroupID=@UserGroupID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserGroupID", SqlDbType.Int,4)
            };
            parameters[0].Value = UserGroupID;
            int rows = db.ExecuteSql(strSql.ToString(), parameters);
            return rows > 0;
        }

        internal bool NameExists(string groupId, string groupName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from S_UserGroup");
            strSql.Append(" where UserGroupId<>@UserGroupId AND UserGroupName=@UserGroupName");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserGroupName", SqlDbType.NVarChar,50),
                    new SqlParameter("@UserGroupId", SqlDbType.Int)
            };
            parameters[0].Value = groupName;
            parameters[1].Value = groupId;
            return db.Exists(strSql.ToString(), parameters);
        }

        internal bool NameExists(string groupName)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from S_UserGroup");
            strSql.Append(" where UserGroupName=@UserGroupName");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserGroupName", SqlDbType.NVarChar,50)
            };
            parameters[0].Value = groupName;
            return db.Exists(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public S_UserGroupModel GetModel(int UserGroupID)
        {

            StringBuilder strSql = new StringBuilder();
            strSql.Append("select  top 1 UserGroupID,UserGroupCode,UserGroupName,CreateTime,UserGroupDesc from S_UserGroup ");
            strSql.Append(" where UserGroupID=@UserGroupID");
            SqlParameter[] parameters = {
                    new SqlParameter("@UserGroupID", SqlDbType.Int,4)
            };
            parameters[0].Value = UserGroupID;

            S_UserGroupModel model = new S_UserGroupModel();
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
        /// 得到一个对象集合
        /// </summary>
        public List<S_UserGroupModel> GetModelList(string strWhere)
        {
            var result = new List<S_UserGroupModel>();
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select UserGroupID,UserGroupCode,UserGroupName,CreateTime,UserGroupDesc from S_UserGroup ");
            if (!string.IsNullOrEmpty(strWhere))
            {
                strSql.Append(" where " + strWhere);
            }

            DataSet ds = db.Query(strSql.ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    result.Add(DataRowToModel(ds.Tables[0].Rows[i]));
                }
            }
            else
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public S_UserGroupModel DataRowToModel(DataRow row)
        {
            S_UserGroupModel model = new S_UserGroupModel();
            if (row != null)
            {
                if (row["UserGroupID"] != null && row["UserGroupID"].ToString() != "")
                {
                    model.UserGroupID = int.Parse(row["UserGroupID"].ToString());
                }
                if (row["UserGroupCode"] != null && row["UserGroupCode"].ToString() != "")
                {
                    model.UserGroupCode = row["UserGroupCode"].ToString();
                }
                if (row["UserGroupName"] != null && row["UserGroupName"].ToString() != "")
                {
                    model.UserGroupName = row["UserGroupName"].ToString();
                }
                if (row["CreateTime"] != null && row["CreateTime"].ToString() != "")
                {
                    model.CreateTime = DateTime.Parse(row["CreateTime"].ToString());
                }
                if (row["UserGroupDesc"] != null && row["UserGroupDesc"].ToString() != "")
                {
                    model.UserGroupDesc = row["UserGroupDesc"].ToString();
                }
            }
            return model;
        }

        #endregion  BasicMethod
        #region  ExtensionMethod

        #endregion  ExtensionMethod
    }
}
