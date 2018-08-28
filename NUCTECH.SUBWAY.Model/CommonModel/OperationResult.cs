using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NUCTECH.SUBWAY.Model.CommonModel
{
    public class OperationResult
    {
        /// <summary>
        /// 操作结果
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 结果描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string RetureUrl { get; set; }
    }
}
