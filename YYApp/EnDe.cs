using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Management;

namespace YYApp
{
    class EnDe
    {
        /// <summary>
        /// 使用DES加密指定字符串
        /// </summary>
        /// <param name="encryptStr">待加密的字符串</param>
        /// <param name="key">密钥(最大长度8)</param>
        /// <param name="IV">初始化向量(最大长度8)</param>
        /// <returns>加密后的字符串</returns>
        public static string DESEncrypt(string encryptStr, string key, string IV)
        {
            //将key和IV处理成8个字符
            key += "12345678";
            IV += "12345678";
            key = key.Substring(0, 8);
            IV = IV.Substring(0, 8);

            SymmetricAlgorithm sa;
            ICryptoTransform ict;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            sa = new DESCryptoServiceProvider();
            sa.Key = Encoding.UTF8.GetBytes(key);
            sa.IV = Encoding.UTF8.GetBytes(IV);
            ict = sa.CreateEncryptor();

            byt = Encoding.UTF8.GetBytes(encryptStr);

            ms = new MemoryStream();
            cs = new CryptoStream(ms, ict, CryptoStreamMode.Write);
            cs.Write(byt, 0, byt.Length);
            cs.FlushFinalBlock();

            cs.Close();

            //加上一些干扰字符
            string retVal = Convert.ToBase64String(ms.ToArray());
            System.Random ra = new Random();

            for (int i = 0; i < 8; i++)
            {
                int radNum = ra.Next(36);
                char radChr = Convert.ToChar(radNum + 65);//生成一个随机字符

                retVal = retVal.Substring(0, 2 * i + 1) + radChr.ToString() + retVal.Substring(2 * i + 1);
            }

            return retVal;
        }

        /// <summary>
        /// 使用DES解密指定字符串
        /// </summary>
        /// <param name="encryptedValue">待解密的字符串</param>
        /// <param name="key">密钥(最大长度8)</param>
        /// <param name="IV">初始化向量(最大长度8)</param>
        /// <returns>解密后的字符串</returns>
        public static string DESDecrypt(string encryptedValue, string key, string IV)
        {
            //去掉干扰字符
            string tmp = encryptedValue;
            if (tmp.Length < 16)
            {
                return "";
            }

            for (int i = 0; i < 8; i++)
            {
                tmp = tmp.Substring(0, i + 1) + tmp.Substring(i + 2);
            }
            encryptedValue = tmp;

            //将key和IV处理成8个字符
            key += "12345678";
            IV += "12345678";
            key = key.Substring(0, 8);
            IV = IV.Substring(0, 8);

            SymmetricAlgorithm sa;
            ICryptoTransform ict;
            MemoryStream ms;
            CryptoStream cs;
            byte[] byt;

            try
            {
                sa = new DESCryptoServiceProvider();
                sa.Key = Encoding.UTF8.GetBytes(key);
                sa.IV = Encoding.UTF8.GetBytes(IV);
                ict = sa.CreateDecryptor();

                byt = Convert.FromBase64String(encryptedValue);

                ms = new MemoryStream();
                cs = new CryptoStream(ms, ict, CryptoStreamMode.Write);
                cs.Write(byt, 0, byt.Length);
                cs.FlushFinalBlock();

                cs.Close();

                return Encoding.UTF8.GetString(ms.ToArray());
            }
            catch (System.Exception)
            {
                return "";
            }

        }



        public static void WriteAk(string text)
        {
            string file_name = System.Windows.Forms.Application.StartupPath + "/HYXT.ak";
            try
            {
                if (!File.Exists(file_name))
                {
                    FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
                StreamWriter sr = File.AppendText(file_name);
                sr.WriteLine(text);
                sr.Close();
            }
            catch { }
        }

        public static string ReadAk()
        {
            string file_name = System.Windows.Forms.Application.StartupPath + "/HYXT.ak";
            try
            {
                if (!File.Exists(file_name))
                {
                    FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    fs.Close();
                }
                string[] texts = File.ReadAllLines(file_name);
                if (texts.Length > 0)
                {
                    return texts[0];
                }
                return null;
            }
            catch { return null; }
        }

        public static string GetCPU()
        {
            string cpuInfo = null;
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }

            return cpuInfo.Trim();
        }
    }
}
