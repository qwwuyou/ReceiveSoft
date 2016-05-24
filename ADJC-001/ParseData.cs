using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service;

namespace ADJC_001
{
    class ParseData
    {

        public byte[] GetDateTime() 
        {
            string strTime = DateTime.Now.ToString("yyMMddHHmmss");
            byte[] tmpBody = EnCoder.HexStrToByteArray(strTime);
            Array.Copy(EnCoder.HexStrToByteArray(strTime), 0, tmpBody, 2, 6);
            return tmpBody;
        }
    }
}
