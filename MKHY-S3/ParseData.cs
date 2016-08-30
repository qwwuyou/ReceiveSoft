using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MKHY_S3
{
    public class ParseData
    {
        /// <summary>
        /// 命令码
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public string CommandCode(string data)
        {
            string PacketType = "";
            if (data.Length > 12)
            {
                PacketType = data.Substring(10, 2);
            }
            return PacketType;
        }

        /// <summary>
        /// 数据报类型，定时报01，加报是02，手动召测是03
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private string PacketType(string data)
        {
            string PacketType = "";
            if (data.Length > 24)
            {
                PacketType = data.Substring(22, 2);
            }
            return PacketType;
        }

        /// <summary>
        /// 得到时间  14个F取当前系统时间
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private DateTime GetDateTime(string data)
        {
            DateTime dt = DateTime.Now;
            string DateStr = "";
            if (data.Length > 22) 
            {
                string dateString = data.Substring(12, 10);
                DateStr = (2000 + Convert.ToInt32(dateString.Substring(0, 2), 16)).ToString() + "-" +
                           Convert.ToInt32(dateString.Substring(2, 2), 16) + "-" +
                           Convert.ToInt32(dateString.Substring(4, 2), 16) + " " +
                           Convert.ToInt32(dateString.Substring(6, 2), 16) + ":" +
                           Convert.ToInt32(dateString.Substring(8, 2), 16) + ":00";
                
            }
            DateTime.TryParse(DateStr, out dt);
            return dt;
        }

        /// <summary>
        /// 得到报文头标识
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private int GetTitleCode(string data,out string Newdata)
        {
            Newdata = data;
            string[] temps = data.Split(new char[] { '(', ')' }, StringSplitOptions.None);
            if (temps.Length >= 3) 
            {
                Newdata =temps[2];
                return Convert.ToInt16(temps[1]);
            }
            return -1;
        }

        /// <summary>
        /// 得到RTU地址
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public string GetCode(string data) 
        {
            string Code = "";
            if (data.Length > 10) 
            {
                Code = Convert.ToInt32(data.Substring(8, 2), 16).ToString();
            }
            return Code;
        }

        /// <summary>
        /// 数据长度（本协议无意义）
        /// </summary>
        /// <returns></returns>
        private int DataLength(string data) 
        {
            int Len =0;
            if (data.Length > 8)
            {
                Len = Convert.ToInt32(data.Substring(4, 4), 16);
            }
            return Len;
        }

        /// <summary>
        /// 得到数据状态(01 为数据有效，00 为数据无效，02 为设备故障)
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public string GetDataState(string data) 
        {
            string DataState="";
            if (data.Length > 44) 
            {
                DataState = data.Substring(42, 2);
            }
            return DataState;
        }


        #region 信息类的数据
        /// <summary>
        /// 得到算法
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private string GetAlgorithm(string data)
        {
            string Algorithm = "";
            if (data.Length > 46)
            {
                Algorithm = data.Substring(44, 2);
            }
            return Algorithm;
        }

        /// <summary>
        /// 得到测流历时(秒)
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private int GetTake(string data) 
        {
            int Take = 0;
            if (data.Length > 48)
            {
                Take = Convert.ToInt32(data.Substring(46, 2),16);
            }
            return Take;
        }

        /// <summary>
        /// 得到左岸系数
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private double GetLeftBank(string data)
        {
            double LeftBank = 0;
            if (data.Length > 50)
            {
                LeftBank =  Convert.ToInt32(data.Substring(48, 2),16)/100d;
            }
            return LeftBank;
        }

        /// <summary>
        /// 得到右岸系数
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private double GetRightBank(string data)
        {
            double RightBank =0;
            if (data.Length > 52)
            {
                RightBank = Convert.ToInt32(data.Substring(50, 2),16)/100d;
            }
            return RightBank;
        }

        /// <summary>
        /// 得到起算水位
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private double GetStartStage(string data) 
        {
            double StartStage = 0;
            if (data.Length > 64)
            {
                StartStage = Convert.ToInt32(data.Substring(56, 8), 16)/100d;
            }
            return StartStage;
        }

        /// <summary>
        /// 得到断面面积
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private double GetSection(string data)
        {
            double Section = 0;
            if (data.Length > 72)
            {
                Section = Convert.ToInt32(data.Substring(64, 8), 16) / 100;
            }
            return Section;
        }

        /// <summary>
        /// 得到探头数量
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private int GetSensorCount(string data)
        {
            int SensorCount = 0;
            if (data.Length > 86)
            {
                SensorCount = Convert.ToInt32(data.Substring(84, 2), 16);
            }
            return SensorCount;
        }

        /// <summary>
        /// 得到参与计算的探头数量
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        private int GetComputeSensorCount(string data, int SensorCount) 
        {
            int ComputeSensorCount = 0;
            if (data.Length > (84 + SensorCount * 8 + 2)) 
            {
                ComputeSensorCount = Convert.ToInt32(data.Substring(84 + SensorCount * 8 , 2), 16);
            }
            return ComputeSensorCount;
        }
        #endregion

        #region 数据
        private List<Item_Data> GetData(string data, int SensorCount) 
        {
            List<Item_Data> Lid = new List<Item_Data>();
            Item_Data id=null;
            if (data.Length > 26)
            {
                id = new Item_Data();
                id.Item = "S3.Voltage";                                                 //电压
                id.Data = Convert.ToInt32(data.Substring(24, 2), 16) / 10m;
                Lid.Add(id);
            }
            if (data.Length > 34)
            {
                id = new Item_Data();
                id.Item = "S3.Stage";                                                   //水位
                id.Data = Convert.ToInt32(data.Substring(26, 8), 16) / 100m;
                Lid.Add(id);
            }
            if (data.Length > 74)
            {
                id = new Item_Data();
                id.Item = "S3.Speed";                                                   //平均流速
                id.Data = Convert.ToInt32(data.Substring(72,4), 16) / 100m;
                Lid.Add(id);
            }


            if (data.Length > (86 + SensorCount * 8))
            for (int i = 0; i < SensorCount; i++)
            {
                if (data.Substring(86 +(8* i), 2) == "01") 
                {
                    id = new Item_Data();
                    id.Item = "S3." + (1 + i) + "#Speed";                                //?#传感器平均流速
                    id.Data = Convert.ToInt32(data.Substring(86 + (8 * i) + 2, 4), 16) / 100m;
                    Lid.Add(id);


                    id = new Item_Data();
                    id.Item = "S3." + (1 + i) + "#SNR";                                //?#传感器信噪比
                    id.Data = Convert.ToInt32(data.Substring(86 + (8 * i) + 6, 2), 16) / 100m;
                    Lid.Add(id);
                }
            }

            return Lid;
        } 
        #endregion



        /// <summary>
        /// 解包方法
        /// </summary>
        /// <param name="data">数据报</param>
        /// <returns></returns>
        public DataModel UnPack(string data)
        {
            DataModel DM = new DataModel();
            DM.TiltleCode= GetTitleCode(data ,out data);
            DM.Code = GetCode(data);
            DM.DataLength = DataLength(data);
            DM.Datetime = GetDateTime(data);
            DM.PacketType =PacketType(data);
            DM.CommandCode = CommandCode(data);
            DM.DataState = GetDataState(data);

            DM.Algorithm = GetAlgorithm(data);
            DM.Take = GetTake(data);
            DM.LeftBank = GetLeftBank(data);
            DM.RightBank = GetRightBank(data);
            DM.StartStage = GetStartStage(data);
            DM.Section = GetSection(data);
            DM.SensorCount = GetSensorCount(data);
            DM.ComputeSensorCount = GetComputeSensorCount(data, DM.SensorCount);

            DM.Item_data = GetData(data, DM.SensorCount);
                       
            return DM;
        }
    }


    #region 数据报实体类
    public class DataModel
    {
        private int _tiltlecode;
        private string _code;
        private string _packettype;
        private string _commandcode;
        private int _datalength;
        private DateTime _datetime;
        private string _datastate;


        private string _algorithm;
        private int _take;
        private double _leftbank;
        private double _rightbank;
        private double _startstage;
        private double _section;
        private int _sensorcount;
        private int _computesensorcount;

        private List<Item_Data> _item_data;

        /// <summary>
        /// 报文头标识
        /// </summary>
        public int TiltleCode
        {
            get { return _tiltlecode; }
            set { _tiltlecode = value; }
        }    


        /// <summary>
        /// 站（模块）号
        /// </summary>
        public string Code
        {
            set { _code = value; }
            get { return _code; }
        }

        /// <summary>
        /// 报类型
        /// </summary>
        public string PacketType
        {
            set { _packettype = value; }
            get { return _packettype; }
        }

        /// <summary>
        /// 命令码
        /// </summary>
        public string CommandCode
        {
            set { _commandcode = value; }
            get { return _commandcode; }
        }

        /// <summary>
        /// 数据区长度
        /// </summary>
        public int DataLength
        {
            set { _datalength = value; }
            get { return _datalength; }
        }

        /// <summary>
        /// 上报时间
        /// </summary>
        public DateTime Datetime
        {
            set { _datetime = value; }
            get { return _datetime; }
        }

        /// <summary>
        /// 数据状态(01 为数据有效，00 为数据无效，02 为设备故障)
        /// </summary>
        public string DataState 
        {
            set { _datastate = value; }
            get { return _datastate; }
        }

        /// <summary>
        /// 算法
        /// </summary>
        public string Algorithm 
        {
            set { _algorithm = value; }
            get { return _algorithm; }
        }

        /// <summary>
        /// 测流历时(秒)
        /// </summary>
        public int Take 
        {
            set { _take = value; }
            get { return _take; }
        }

        /// <summary>
        /// 左岸系数
        /// </summary>
        public double LeftBank
        {
            set { _leftbank = value; }
            get { return _leftbank; }
        }

        /// <summary>
        /// 右岸系数
        /// </summary>
        public double RightBank
        {
            set { _rightbank = value; }
            get { return _rightbank; }
        }

        /// <summary>
        /// 起算水位
        /// </summary>
        public double StartStage
        {
            set { _startstage = value; }
            get { return _startstage; }
        }

        /// <summary>
        /// 断面面积
        /// </summary>
        public double Section
        {
            set { _section = value; }
            get { return _section; }
        }

        /// <summary>
        /// 探头数量
        /// </summary>
        public int SensorCount
        {
            set { _sensorcount = value; }
            get { return _sensorcount; }
        }

        /// <summary>
        /// 参与计算的探头数量
        /// </summary>
        public int ComputeSensorCount
        {
            set { _computesensorcount = value; }
            get { return _computesensorcount; }
        }


        /// <summary>
        /// 数据区 监测项目--数据
        /// </summary>
        public List<Item_Data> Item_data
        {
            set { _item_data = value; }
            get { return _item_data; }
        }

    }

    public class Item_Data
    {
        private string _item;
        private decimal _data;

        /// <summary>
        /// 项目（元素）
        /// </summary>
        public string Item
        {
            set { _item = value; }
            get { return _item; }
        }

        /// <summary>
        /// 数据值
        /// </summary>
        public decimal Data
        {
            set { _data = value; }
            get { return _data; }
        }
    }
    #endregion
}
