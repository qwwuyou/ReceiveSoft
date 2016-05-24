using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class ServiceEnum
    {
        /// <summary>
        /// 发送界面数据类型枚举
        /// </summary>
        public enum DataType
        {
            /// <summary>
            /// 明文
            /// </summary>
            Text = 1,
            /// <summary>
            /// 状态 
            /// </summary>
            State = 2
        };

        /// <summary>
        /// 发送界面数据编码类型枚举
        /// </summary>
        public enum EnCoderType
        {
            /// <summary>
            /// HEX格式
            /// </summary>
            HEX = 1,
            /// <summary>
            /// ASCII格式
            /// </summary>
            ASCII = 2
        };

        /// <summary>
        /// 信道类型
        /// </summary>
        public enum NFOINDEX 
        {
            TCP=1,
            UDP=2,
            GSM=3,
            COM=4,
        }
    }
}
