using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JK
{
    static class PackageProcess
    {
        static log4net.ILog log = log4net.LogManager.GetLogger("Logger");

        static ParseData pd = new ParseData();

        /// <summary>
        ///  ADCP上传监测数据报
        /// </summary>
        /// <param name="data"></param>
        /// <param name="NFOINDEX"></param>
        /// <param name="Server"></param>
        internal static void Process_39(string data, Service.ServiceEnum.NFOINDEX NFOINDEX, object Server)
        {
            DataModel DM = pd.UnPack(data);

        }
    }
}
