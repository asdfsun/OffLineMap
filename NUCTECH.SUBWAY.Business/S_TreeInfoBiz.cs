using NUCTECH.SUBWAY.Model.ViewModel;
using System.Collections.Generic;

namespace NUCTECH.SUBWAY.Business
{
    public class S_TreeInfoBiz : BaseBiz
    {
        private readonly DAL.S_TreeInfoDal treeInfoDal = new DAL.S_TreeInfoDal();
        internal List<S_TreeInfoViewModel> GetListByRoleId(int roleID)
        {
            return treeInfoDal.GetListByRoleId(roleID);
        }
    }
}
