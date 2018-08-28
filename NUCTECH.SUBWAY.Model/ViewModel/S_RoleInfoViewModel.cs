using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Model.ViewModel
{
    public class S_RoleInfoViewModel
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public DateTime CreateTime { get; set; }
        public List<S_TreeInfoViewModel> TreeList { get; set; }
    }
}
