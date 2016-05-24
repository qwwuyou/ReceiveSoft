using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public interface  DataProcess
    {
        /// <summary>
        /// 发送召测命令
        /// </summary>
        /// <param name="US">udp服务</param>
        void SendCommand(UdpService.UdpServer US);
        /// <summary>
        /// 发送召测命令
        /// </summary>
        /// <param name="TS">tcp服务</param>
        void SendCommand(TcpService.TcpServer TS);
        /// <summary>
        /// 发送召测命令
        /// </summary>
        /// <param name="GS">短信服务</param>
        void SendCommand(GsmService.GsmServer GS);
        /// <summary>
        /// 发送召测命令
        /// </summary>
        /// <param name="CS">卫星服务</param>
        void SendCommand(ComService.ComServer CS);


        /// <summary>
        /// 处理数据报
        /// </summary>
        /// <param name="US">udp服务</param>
        void PacketArrived(UdpService.UdpServer US);
        /// <summary>
        /// 处理数据报
        /// </summary>
        /// <param name="TS">tcp服务</param>
        void PacketArrived(TcpService.TcpServer TS);
        /// <summary>
        /// 处理数据报
        /// </summary>
        /// <param name="GS">短信服务</param>
        void PacketArrived(GsmService.GsmServer GS);
        /// <summary>
        /// 处理数据报
        /// </summary>
        /// <param name="CS">卫星服务</param>
        void PacketArrived(ComService.ComServer CS);
    }
}
