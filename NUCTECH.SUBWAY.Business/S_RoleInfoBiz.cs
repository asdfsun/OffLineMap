using NUCTECH.SUBWAY.Model.CommonModel;
using NUCTECH.SUBWAY.Model.DbModel;
using NUCTECH.SUBWAY.Model.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Business
{
    public class S_RoleInfoBiz : BaseBiz
    {
        private readonly DAL.S_RoleInfoDal roleInfoDal = new DAL.S_RoleInfoDal();
        private readonly DAL.S_UserInfoDal userInfoDal = new DAL.S_UserInfoDal();
        S_TreeInfoBiz treeInfoBiz = new S_TreeInfoBiz();

        public S_RoleInfoBiz()
        { }
        /// <summary>
        /// 给角色的下拉列表提供数据源
        /// </summary>
        /// <returns></returns>
        public List<S_RoleInfoViewModel> GetRoleForDropdownList(bool isDefault)
        {
            var resultList = new List<S_RoleInfoViewModel>();
            var strWhere = string.Empty;
            if(isDefault)
            {
                strWhere = " RoleDesc='default' ";
            }
            else
            {
                strWhere = " RoleDesc <> 'default' or RoleDesc is null ";
            }
            var modelList = GetModelList(strWhere);

            if (modelList != null)
            {
                foreach (var item in modelList)
                {
                    resultList.Add(new S_RoleInfoViewModel
                    {
                        RoleId = item.RoleID.ToString(),
                        RoleName = item.RoleName
                    });
                }
            }
            return resultList;
        }

        /// <summary>
        /// 角色权限列表查询
        /// </summary>
        /// <returns></returns>
        public List<S_RoleInfoViewModel> GetRoleInfoList(string roleName)
        {
            var resultList = new List<S_RoleInfoViewModel>();

            var modelList = GetModelList(" RoleName like '%" + roleName + "%'");
            if (modelList != null)
            {
                var treeBiz = new S_TreeInfoBiz();
                foreach (var item in modelList)
                {
                    var model = new S_RoleInfoViewModel
                    {
                        RoleId = item.RoleID.ToString(),
                        RoleName = item.RoleName,
                        CreateTime = (DateTime)item.CreateTime
                    };
                    model.TreeList = GetRoleTreeList(item.RoleID);

                    resultList.Add(model);
                }
            }
            return resultList;
        }
        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="roleId">角色ID</param>
        /// <param name="roleName">角色名</param>
        /// <param name="treeIds">功能ID</param>
        /// <returns></returns>
        public ResultModel Edit(int roleId, string roleName, string treeIds)
        {
            var result = new ResultModel { Status = false };

            try
            {
                if (roleName == null || roleName.Length == 0)
                {
                    result.ReMsg = "角色名不允许为空";
                    return result;
                }
                if (roleInfoDal.RoleNameExists(roleId, roleName))
                {
                    result.ReMsg = "该角色名已被使用";
                    return result;
                }
                if (Update(new S_RoleInfoModel() { RoleID = roleId, RoleName = roleName }))
                {
                    ResultModel r = roleInfoDal.AddTreeRole(roleId, treeIds);
                    if (!r.Status)
                        result.ReMsg = r.ReMsg;
                    else
                    {
                        result.Status = true;

                    }
                }
                else
                    result.ReMsg = "修改角色信息失败";
            }
            catch (Exception ex)
            {
                result.ReMsg = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 根据角色id获取菜单
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<S_TreeInfoViewModel> GetRoleTreeList(int roleID)
        {
            return roleInfoDal.GetRoleTreeList(roleID);
        }
        /// <summary>
        /// 获取所有菜单
        /// </summary>
        /// <param name="roleID"></param>
        /// <returns></returns>
        public List<S_TreeInfoViewModel> GetTreeList()
        {
            return roleInfoDal.GetTreeList();
        }
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public ResultModel Add(string roleName, string treeIds)
        {
            var result = new ResultModel { Status = false };
            try
            {
                if (roleName == null || roleName.Length == 0)
                {
                    result.ReMsg = "角色名不允许为空";
                    return result;
                }
                if (roleInfoDal.RoleNameExists(roleName))
                {
                    result.ReMsg = "该角色名已被使用";
                    return result;
                }

                S_RoleInfoModel model = new S_RoleInfoModel();
                model.RoleName = roleName;
                model.CreateTime = DateTime.Now;
                int newId = roleInfoDal.Add(model);
                if (newId == 0)
                    result.ReMsg = "角色信息保存失败";
                else
                {
                    ResultModel r = roleInfoDal.AddTreeRole(newId, treeIds);
                    if (!r.Status)
                        result.ReMsg = r.ReMsg;
                    else
                    {
                        result.Status = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.ReMsg = ex.ToString();
            }
            return result;
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public bool Update(S_RoleInfoModel model)
        {
            return roleInfoDal.Update(model);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public ResultModel Delete(int RoleID)
        {
            var result = new ResultModel { Status = false };
            try
            {
                int userCount = userInfoDal.GetUserCountByRoleId(RoleID);
                if (userCount > 0)
                {
                    result.ReMsg = "删除失败,该角色已被用户使用";
                }
                else
                {
                    if (roleInfoDal.Delete(RoleID))
                    {
                        result.Status = true; 
                    }
                    else
                        result.ReMsg = "数据已变更，请刷新后重新操作!";
                }
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
            return roleInfoDal.GetModel(RoleID);
        }

        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<S_RoleInfoModel> GetModelList(string strWhere)
        {
            DataSet ds = roleInfoDal.GetList(strWhere);
            return DataTableToList(ds.Tables[0]);
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<S_RoleInfoModel> DataTableToList(DataTable dt)
        {
            List<S_RoleInfoModel> modelList = new List<S_RoleInfoModel>();

            int rowsCount = dt.Rows.Count;
            if (rowsCount > 0)
            {
                S_RoleInfoModel model;
                for (int n = 0; n < rowsCount; n++)
                {
                    model = roleInfoDal.DataRowToModel(dt.Rows[n]);
                    if (model != null)
                    {
                        modelList.Add(model);
                    }
                }
            }
            return modelList;
        }
    }
}
