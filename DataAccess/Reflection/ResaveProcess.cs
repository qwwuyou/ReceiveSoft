using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service.Model;

namespace Service
{
    public interface ResaveProcess
    {
        /// <summary>
        /// 初始化转存信息  
        /// [aq读xml配置信息，hlj获取最新雨量、水位（河道、库）数据...]
        /// </summary>
        void InitInfo();

        /// <summary>
        /// 转存的具体实现
        /// </summary>
        void Resave(YY_DATA_AUTO model);
    }
}
