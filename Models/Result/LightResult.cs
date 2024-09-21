using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace newblinkinglight
{
    public class LightResult
    {
        /// <summary>
        /// trạng thái trả về cho client là thành công hoặc không
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// giá trị trả về nếu cần
        /// </summary>
        public object Data { get; set; }
        /// <summary>
        /// nội dung trả về
        /// </summary>
        public string Messagse { get; set; }
    }
}
