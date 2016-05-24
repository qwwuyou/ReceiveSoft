using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComService
{
    public class ComSatellite
    {
        private string _stcd;
        private string _satellite;
        private DateTime _datatime;

        /// <summary>
        /// 测站编码
        /// </summary>		
        public string STCD
        {
            get { return _stcd; }
            set { _stcd = value; }
        }

        /// <summary>
        /// 卫星标识
        /// </summary>
        public string SATELLITE
        {
            get { return _satellite; }
            set { _satellite = value; }
        }

        /// <summary>
        /// 数据接收时间(最新)
        /// </summary>
        public DateTime DATATIME
        {
            get { return _datatime; }
            set { _datatime = value; }
        }
    }

    //收到的数据
    public class ComReceivedData
    {
        private byte[] _data;
        private string _satellite;
        /// <summary>
        /// 接收到的数据
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }
        /// <summary>
        /// 卫星标识
        /// </summary>
        public string SATELLITE
        {
            get { return _satellite; }
            set { _satellite = value; }
        }
    }

    //原版本卫星协议的卫星状态
    public class ComState
    {
          
        private int _power1;
        private int _power2;
        private int _beam1;
        private int _beam2;
        private int _response;
        private int _inhibition;
        private int _powersupply;
        private DateTime _datatime;
        /// <summary>
        /// 通道1信号功率
        /// </summary>
        public int Power1
        {
            get { return _power1; }
            set { _power1 = value; }
        }

        /// <summary>
        /// 通道2信号功率
        /// </summary>
        public int Power2
        {
            get { return _power2; }
            set { _power2 = value; }
        }

        /// <summary>
        /// 通道1卫星波束
        /// </summary>
        public int Beam1
        {
            get { return _beam1; }
            set { _beam1 = value; }
        }

        /// <summary>
        /// 通道2卫星波束
        /// </summary>
        public int Beam2
        {
            get { return _beam2; }
            set { _beam2 = value; }
        }

        /// <summary>
        /// 响应波束
        /// </summary>
        public int Response
        {
            get { return _response; }
            set { _response = value; }
        }

        /// <summary>
        /// 信号抑制
        /// </summary>
        public int Inhibition
        {
            get { return _inhibition; }
            set { _inhibition = value; }
        }

        /// <summary>
        /// 供电状态
        /// </summary>
        public int PowerSupply
        {
            get { return _powersupply; }
            set { _powersupply = value; }
        }

        /// <summary>
        /// 状态数据接收时间(最新)
        /// </summary>
        public DateTime DATATIME
        {
            get { return _datatime; }
            set { _datatime = value; }
        }
    }
    //新版本卫星协议4.0的卫星状态
    public class ComStateFor4 
    {
        private string _icstate;
        private string _hardwarestate;
        private string _electricity;
        private string _power;
        private DateTime _datatime;

        /// <summary>
        /// IC卡状态
        /// </summary>
        public string ICState 
        {
            get { return _icstate; }
            set { _icstate = value; }
        }

        /// <summary>
        /// 硬件状态
        /// </summary>
        public string HardwareState
        {
            get { return _hardwarestate; }
            set { _hardwarestate = value; }
        }

        /// <summary>
        /// 电量百分比
        /// </summary>
        public string Electricity
        {
            get { return _electricity; }
            set { _electricity = value; }
        }

        /// <summary>
        /// 1#~6#波束功率
        /// </summary>
        public string Power
        {
            get { return _power; }
            set { _power = value; }
        }

        /// <summary>
        /// 状态数据接收时间(最新)
        /// </summary>
        public DateTime DATATIME
        {
            get { return _datatime; }
            set { _datatime = value; }
        }
    }

    //回复的数据
    public class ComSendData
    {
        private string _stcd;
        private byte[] _data;
        private int _state;
        private string _commandcode;
        /// <summary>
        /// 测站编码
        /// </summary>		
        public string STCD
        {
            get { return _stcd; }
            set { _stcd = value; }
        }


        /// <summary>
        /// 要发送的数据
        /// </summary>
        public byte[] Data
        {
            get { return _data; }
            set { _data = value; }
        }

        /// <summary>
        /// 召测命令状态  0新产生未激活发送  1第一次发送完成  2第二次发送完成
        /// </summary>
        public int STATE
        {
            get { return _state; }
            set { _state = value; }
        }

        /// <summary>
        /// 命令码
        /// </summary>
        public string COMMANDCODE
        {
            get { return _commandcode; }
            set { _commandcode = value; }
        }
    }
}
