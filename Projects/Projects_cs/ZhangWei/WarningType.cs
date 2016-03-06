using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OldW
{
    namespace Modeling
    {
        [Serializable()]
        public class Warning
        {
        }

        [Serializable()]
        public class WarningSumVelo:Warning
        {
            /// <summary>
            /// 累计值
            /// </summary>
            public double sum;
            /// <summary>
            /// 速率
            /// </summary>
            public double velo;

            public WarningSumVelo(double sum, double velo)
            {
                this.sum = sum;
                this.velo = velo;
            }
        }

        /// <summary>
        /// 测斜警戒值
        /// </summary>
        [Serializable()]
        public class WarningIncli : WarningSumVelo 
        {
            public WarningIncli(double sum, double velo)
                : base(sum, velo)
            {
            }
        }

        /// <summary>
        /// 地表沉降警戒值
        /// </summary>
        [Serializable()]
        public class WarningGSetle : WarningSumVelo
        {
            public WarningGSetle(double sum, double velo)
                : base(sum, velo)
            {
            }
        }

        /// <summary>
        /// 轴力警戒值
        /// </summary>
        [Serializable()]
        public class WarningForce : Warning
        {
            /// <summary>
            /// 系数
            /// </summary>
            public double ratio;

            public WarningForce(double ratio)
            {
                this.ratio = ratio;
            }
        }
    }
}