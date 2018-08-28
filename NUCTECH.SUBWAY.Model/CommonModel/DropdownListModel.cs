using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Model.CommonModel
{
    public class DropdownListModel
    {
        /// <summary>
        /// 显示内容
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 实际内容
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool selected { get; set; }
    }
}
