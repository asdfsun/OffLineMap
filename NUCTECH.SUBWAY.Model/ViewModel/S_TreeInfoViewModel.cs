using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Model.ViewModel
{
    public class S_TreeInfoViewModel
    {
        public int TreeId { get; set; }
        public string TreeName { get; set; }
        public int ParentID { get; set; }
        public decimal TreeSort { get; set; }
        public string TreeUrl { get; set; }
        public string TreeNote { get; set; }
        public string TreeCode { get; set; }
        public string TreePageCode { get; set; }
        public string TreeDesc { get; set; }
    }
}
