using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class EnCoder
    {
        /// <summary>
        /// byte[] 转 Hex字符串
        /// </summary>
        /// <param name="Data">byte数组</param>
        /// <returns></returns>
        public static string ByteArrayToHexStr(byte[] Data) 
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < Data.Length; i++)
            {
                sb.Append(Data[i].ToString("X2"));
            }

            return sb.ToString();
        }

        /// <summary>
        /// 字符串转16进制字节数组
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexStrToByteArray(string HexString)
        {
            HexString = HexString.Replace(" ", "");
            if ((HexString.Length % 2) != 0)
                HexString += " ";
            byte[] returnBytes = new byte[HexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(HexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

      
    }
}
