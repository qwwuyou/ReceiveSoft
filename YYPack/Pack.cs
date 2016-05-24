using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace YYPack
{


    public class Pack
    {

        static void Main()
        {
            string ss = (new Pack()).GetLength("0065000D0005001C0009002A00290001000A000100010001FFFF0001FFFFFFFFFFFFFFFFFFFFFFFFFFFF");
            
            //Console.WriteLine((new Pack()).GetBeginOrEndTime(DateTime.Now));
            ////
            //string str = "F2F2002400050002000B3231333435";
            //byte[] buffer = new byte[str.Length / 2];// Encoding.ASCII.GetBytes(str);

            //for (int i = 0; i < str.Length / 2; i++)
            //{
            //    buffer[i] = Convert.ToByte(str.Substring(i * 2, 1), 16); //这里有可能会出错误（祝20133-22）
            //}

            ////byte[] tempBuffer = new byte[2];
            ////Array.Copy(buffer, tempBuffer, 2);
            ////for (int i = 0; i < buffer.Length / 2; i++)
            ////{
            ////    byte[] temp2Buffer=new byte[2];
            ////    Console.WriteLine(Encoding.ASCII.GetString(tempBuffer));
            ////    Console.Write(tempBuffer[0].ToString("X2"));
            ////    Console.Write(tempBuffer[1].ToString("X2"));
            ////    Array.Copy(buffer, (i * 2) + 2, temp2Buffer, 0, 2);

            ////    tempBuffer[0] = (byte)(tempBuffer[0] ^ temp2Buffer[0]);
            ////    tempBuffer[1] = (byte)(tempBuffer[1] ^ temp2Buffer[1]);

            ////}
            //byte b = 0;
            //b = buffer[0];
            //for (int i = 1; i < buffer.Length; i++)
            //{
            //    b = (byte)(b ^ buffer[i]);
            //    Console.Write(b.ToString("X2"));
            //}
            //Console.Write(b.ToString("X2"));
            //Console.Read();
        }

        //打包当前时间
        private string GetDxTime()
        {
            DateTime dt = DateTime.Now;
            StringBuilder result = new StringBuilder();
            int year = dt.Year % 100;
            int moth = dt.Month;
            int day = dt.Day;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int second = dt.Second;

            result.Append(NotLengAdd(From10ToX(year, 16), 4).ToUpper());
            result.Append(NotLengAdd(From10ToX(moth, 16), 4).ToUpper());
            result.Append(NotLengAdd(From10ToX(day, 16), 4).ToUpper());
            result.Append(NotLengAdd(From10ToX(hour, 16), 4).ToUpper());
            result.Append(NotLengAdd(From10ToX(minute, 16), 4).ToUpper());
            result.Append(NotLengAdd(From10ToX(second, 16), 4).ToUpper());

            return result.ToString();
        }

        //打包起始或截止时间（固态提取时用）
        public string GetBeginOrEndTime(DateTime dt)
        {
            StringBuilder result = new StringBuilder();
            int year = DateTime.Now.Year - dt.Year;
            int moth = dt.Month;
            int day = dt.Day;
            int hour = dt.Hour;
            int minute = dt.Minute;
            int second = dt.Second;
            result.Append(NotLengAdd(From10ToX(moth, 2), 4));
            result.Append(NotLengAdd(From10ToX(day, 2), 5));
            result.Append(NotLengAdd(From10ToX(hour, 2), 5));
            result.Append(NotLengAdd(From10ToX(year, 2), 2));
            return From2To16(result.ToString()).ToUpper();

        }

        //将数据区传入，返回数据长度L
        private string GetLength(string val)
        {
            return NotLengAdd(From10ToX(val.Length / 4, 16), 4);
        }

        private string GetXOR()
        {
            return "";
        }

        //用 "0"补位
        private string NotLengAdd(string val, int len)
        {
            StringBuilder result = new StringBuilder();
            result.Append(val);
            if (val.Length < len)
            {
                int dl = len - val.Length;
                for (int i = 0; i < dl; i++)
                    result.Insert(0, "0");
            }
            return result.ToString();
        }

        //10进制转X进制
        public string From10ToX(int val, int x)
        {
            return Convert.ToString(val, x);
        }

        //2进制转16进制
        public string From2To16(string val)
        {
            return Convert.ToString(Convert.ToInt32(val, 2), 16);
        }


        public string GetAnswer(string N, string C, string D7, string D8, string D10, string D11, string D13, string D15, string D16, string D17) 
        {
            try
            {
                string XOR = GetXOR();
                string L = GetLength(C + GetDxTime() + D7 + D8 + "0001" + D10 + D11 + "FFFF" + D13 + "FFFF" + D15 + D16 + D17 + "FFFFFFFFFFFF");
                return "F2F2" + N + L + C + GetDxTime() + D7 + D8 + "0001" + D10 + D11 + "FFFF" + D13 + "FFFF" + D15 + D16 + D17 + "FFFFFFFFFFFF" + XOR;
            }
            catch { return null; }
        }

        public string GetAnswer(string N, string C, string D7, string D8, string D10, string D11, string D13, string D15, string D16, string D17,string GprsOrGsm)
        {
            try
            {
                string XOR = GetXOR();
                string L = GetLength(C + GetDxTime() + D7 + D8 + "0001" + D10 + D11 + "FFFF" + D13 + "FFFF" + D15 + D16 + D17 + "FFFFFFFFFFFF");
                if (GprsOrGsm.ToLower()=="gprs")
                return "F2F2" + N + L + C + GetDxTime() + D7 + D8 + "0001" + D10 + D11 + "FFFF" + D13 + "FFFF" + D15 + D16 + D17 + "FFFFFFFF0002" + XOR;
                else
                    return "F2F2" + N + L + C + GetDxTime() + D7 + D8 + "0001" + D10 + D11 + "FFFF" + D13 + "FFFF" + D15 + D16 + D17 + "FFFFFFFF0001" + XOR;
           
            }
            catch { return null; }
        }
     
    }
}
