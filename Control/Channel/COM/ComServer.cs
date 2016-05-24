using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;

namespace ComService
{
    public class ComServer
    {
        #region [变量]
        public SerialPort sp=null;
        string PortName;
        public string Satellite;
        int BaudRate;
        public string ServiceID;
        public List<ComSatellite> Cs;
        public ComQueue CQ;
        ComThread CT;
        public ComState CState;
        public ComStateFor4 CStateFor4;
        #endregion

        #region [事件]
        /// <summary>
        /// 接收数据
        /// </summary>
        public event EventHandler<ReceivedDataEventArgs> OnReceivedData;

        /// <summary>
        /// 发送数据(未使用)
        /// </summary>
        //public event EventHandler<SendDataEventArgs> OnSendData;

        /// <summary>
        /// 有一个客户端连接上来
        /// </summary>
        //public event EventHandler<ConnectedEventArgs> OnConnected;

        /// <summary>
        /// 断开释放
        /// </summary>
        //public event EventHandler<DisconnectedEventArgs> OnDisconnected;
        #endregion

        public ComServer(string Portname, int Baudrate, string satellite, string serviceID)
        {
            PortName = Portname;
            BaudRate = Baudrate;
            ServiceID = serviceID;
            Satellite = satellite;
            Cs = ComBussiness.GetComSatelliteList();
            CQ = new ComQueue();
            CT = new ComThread(this);

            //原版本卫星协议的卫星状态
            CState = new ComState();
            //新版本卫星协议4.0的卫星状态
            CStateFor4 = new ComStateFor4();
        }
       
        public void Start()
        {
            if (sp == null)
            {
                sp = new SerialPort();
                sp.PortName = PortName;
                sp.BaudRate = BaudRate;

                sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                sp.Open();
               
                ComBussiness.ToQcsd();
            }
                //重启
            else if (sp != null && !sp.IsOpen)
            {
                try
                {
                    sp = new SerialPort();
                    sp.PortName = PortName;
                    sp.BaudRate = BaudRate;

                    sp.DataReceived += new SerialDataReceivedEventHandler(sp_DataReceived);
                    sp.Open();

                    ComBussiness.ToQcsd();
                }
                catch (Exception ex)
                {
                    Service.ServiceControl.log .Warn(DateTime.Now + "com服务(" + sp.PortName + ":" + sp.BaudRate + ")重启失败！", ex);
                }
            }

            ////卫星发送启动后测试
            //byte[] b = Service.EnCoder.HexStrToByteArray("24434F55542C302C312C302C3232373730382C302C32352C34362C7E7E0100000000010451EF001D020001140801093511F1F1000000000150F0F0140801093539230003730003DBAC2C020D");
            //byte[] b = Service.EnCoder.HexStrToByteArray("24545354412C302C302C322C332C322C302C312C3232373730382C31313136333035302C36302C332C302C302C382C2C");
            //byte[] b = Service.EnCoder.HexStrToByteArray("24545354412C342C342C322C332C322C302C312C3232373730382C31313136333035302C36302C332C302C302C380D0A");
            //byte[] b = Service.EnCoder.HexStrToByteArray("24434F55542C302C312C302C3232373730382C302C39362C35332C7E7E010000000002045132002402001D140819154708F1F1000000000250F0F01407300700261900005038121230FF03080403EA0B2C060D0A");
            //byte[] b = Service.EnCoder.HexStrToByteArray("2454585351004E022AC446022AC301E0007E7E000015000001123430002B020019150430111014F1F100150000014BF0F00000000000201900009726190000973C23000000003812000003B0CDD0");
            
            //string str="24 49 43 4A 43 00 0C 00 00 00 00 2B 24 49 43 58 58 00 16 02 2A C4 00 1D 77 14 06 00 3C 04 00 00 00 94 24 58 54 5A 4A 00 0D 02 2A C4 00 00 D9 24 5A 4A 58 58 00 15 02 2A C4 01 00 64 02 02 04 02 00 04 02 A8 24 54 58 53 51 00 4E 02 2A C4 46 02 2A C3 01 E0 00 7E 7E 00 00 15 00 00 01 12 34 30 00 2B 02 00 19 15 04 30 11 10 14 F1 F1 00 15 00 00 01 4B F0 F0 00 00 00 00 00 20 19 00 00 97 26 19 00 00 97 3C 23 00 00 00 00 38 12 00 00 03 B0 CD D0 24 46 4B 58 58 00 10 02 2A C4 00 54 58 53 51 DB ";
            //string str1 = "24 54 58 53 51 00 4E 02 2A C4 46 02 2A C3 01 E0 00 7E 7E 00 00 15 00 00 01 12 34 30 00 2B 02 00 19 15 04 30 11 10 14 F1 F1 00 15 00 00 01 4B F0 F0 00 00 00 00 00 20 19 00 00 97 26 19 00 00 97 3C 23 00 00 00 00 38 12 00 00 03 B0 CD D0";
            //string str1 = "24 54 58 58 58  00 1F   02 2A C3  62      02 2A C3    00 00   00 58  A4 C4 E3 BA C3 D6 D0 B9 FA C8 CB  00         B1";
            //str1 = "24545858580050022AC363022AC4000001E07E7E000015000001123430002B020013150504155214F1F100150000014BF0F00000000000201900017226190001723C230000000038120000038A2E0049";
            //str1 = "24 5A 4A 58 58  00 15  02 2A C3  01   00   64   02  01 04 01 00 01 00  A8 ";

            //byte[] b = Service.EnCoder.HexStrToByteArray(str1.Replace(" ", ""));

            //sp.Write(b, 0, b.Length);
        }

        public void Stop()
        {
            if (sp.IsOpen)
            {
                sp.Close();
                sp.Dispose();
            }
        }

        void sp_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (sp.IsOpen)
            {
                try
                {
                    System.Threading.Thread.Sleep(300);
                    byte[] data=new byte[sp.BytesToRead];
                    sp.Read(data, 0,data.Length );


                    if (data.Length > 0)
                    {
                        //byte[] data = Encoding.ASCII.GetBytes(str);
                        if (this.OnReceivedData != null)
                            this.OnReceivedData(this, new ReceivedDataEventArgs(sp, data));



                        Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", data,"ASC");
                        Service.ServiceControl.LogInfoToTxt(Service.ServiceEnum.NFOINDEX.COM, "", data);
                    }
                }
                catch (Exception ex)
                { Service.ServiceControl.log.Error(DateTime.Now + ex.ToString()); }
            }
        }


        public void SendData(string satellite, byte[] msg)
        {
            if (sp.IsOpen)
            {
                //sp.WriteLine(Encoding.ASCII.GetString(cs.Data));
            }
        }
    }

    //接收事件
    public class ReceivedDataEventArgs : EventArgs
    {
        #region
        public readonly SerialPort serialPort = null;
        public readonly byte[]  Data = null;
        public ReceivedDataEventArgs(SerialPort serialport, byte[] data)
        {
            serialPort = serialport;
            Data = data;
        }
        #endregion
    }
}
