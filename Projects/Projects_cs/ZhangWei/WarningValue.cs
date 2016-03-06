using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldW
{
    namespace Modeling
    {
        /// <summary>
        /// 警戒值设定类
        /// </summary>
        [Serializable()]
        public class WarningValue
        {
            /// <summary>
            /// 警戒值设定的名称
            /// </summary>
            public String name;
            /// <summary>
            /// 警戒值设定的等级
            /// </summary>
            public String rate;
            /// <summary>
            /// 轴力警戒值
            /// </summary>
            public WarningForce warningForc;
            /// <summary>
            /// 地表沉降警戒值
            /// </summary>
            public WarningGSetle warningGSetle;
            /// <summary>
            /// 测斜警戒值
            /// </summary>
            public WarningIncli warningIncli;

            public WarningValue(String name,String rate,WarningIncli warningIncli, WarningGSetle warningGSetle, WarningForce warningForc)
            {
                this.name = name;
                this.rate = rate;
                this.warningIncli = warningIncli;
                this.warningGSetle = warningGSetle;
                this.warningForc = warningForc;
            }

            /// <summary>
            /// 返回该警戒值设定的ID
            /// </summary>
            /// <returns></returns>
            public String getId()
            {
                String warningId = this.name + "-" + this.rate;
                return warningId;
            }
        }
    }
}
