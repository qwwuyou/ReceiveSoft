using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Service
{
    public class Center:DataProcess
    {
        public void SendCommand(UdpService.UdpServer US)
        {
            throw new NotImplementedException();
        }

        public void SendCommand(TcpService.TcpServer TS)
        {
            throw new NotImplementedException();
        }

        public void SendCommand(GsmService.GsmServer GS)
        {
            throw new NotImplementedException();
        }

        public void SendCommand(ComService.ComServer CS)
        {
            throw new NotImplementedException();
        }

        public void PacketArrived(UdpService.UdpServer US)
        {
            throw new NotImplementedException();
        }

        public void PacketArrived(TcpService.TcpServer TS)
        {
            //服务运行数据:运行时长[8天22小时11分钟49秒],RTU数量[10],项目[桃山区桃山区桃山区],注册信息[系统还可试用172天！]
            throw new NotImplementedException();
        }

        private void FormatString(string data) 
        {
            string[] strs=data.Split(new string[] { "服务运行数据:" }, StringSplitOptions.None);
            foreach (var str in strs)
            {
                if (str.Length > 40) 
                {
                    YY_Service ys = new YY_Service();
                    string[] Strs = str.Split(new string[] { "," }, StringSplitOptions.None); 

                }
            }
        }

        public void PacketArrived(GsmService.GsmServer GS)
        {
            throw new NotImplementedException();
        }

        public void PacketArrived(ComService.ComServer CS)
        {
            throw new NotImplementedException();
        }
    }

    public class YY_Service
    {
        public YY_Service()
        { }
        #region Model
        private string _project;
        private int _seconds;
        private string _time;
        private int _rtucount;
        private int _register;

        
        /// <summary>
        /// 
        /// </summary>
        public string Project
        {
            set { _project = value; }
            get { return _project; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Seconds
        {
            set { _seconds = value; }
            get { return _seconds; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Time
        {
            set { _time = value; }
            get { return _time; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int RtuCount
        {
            set { _rtucount = value; }
            get { return _rtucount; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int Register
        {
            set { _register = value; }
            get { return _register; }
        }
       
        #endregion Model

    }
}
