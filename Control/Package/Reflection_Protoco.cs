using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ComService;
using GsmService;
using TcpService;
using UdpService;

namespace Service
{
    public class Reflection_Protoco
    {

        public Reflection_Protoco() 
        {
            A = System.Reflection.Assembly.LoadFrom(System.Windows.Forms.Application.StartupPath + "/" + ServiceControl.wrx.XMLObj.dllfile);
            T = A.GetType(ServiceControl.wrx.XMLObj.dllclass);
            O = Activator.CreateInstance(T);
        }
        /// <summary>
        /// 数据处理接口（根据不同协议）
        /// </summary>
        static System.Reflection.Assembly A = null;
        static System.Type T = null;
        static object O = null;


        public static void PacketArrived(ComServer CS)
        {
            System.Reflection.MethodInfo mi = T.GetMethod("PacketArrived", new Type[] { typeof(ComServer) });
            mi.Invoke(O, new object[] { CS }); 
        }

        public static void PacketArrived(GsmServer GS)
        {
            System.Reflection.MethodInfo mi = T.GetMethod("PacketArrived", new Type[] { typeof(GsmServer) });
            mi.Invoke(O, new object[] { GS });
        }

        public static void PacketArrived(TcpServer TS)
        {
            System.Reflection.MethodInfo mi = T.GetMethod("PacketArrived", new Type[] { typeof(TcpServer) });
            mi.Invoke(O, new object[] { TS });
        }

        public static void PacketArrived(UdpServer US)
        {
            System.Reflection.MethodInfo mi = T.GetMethod("PacketArrived", new Type[] { typeof(UdpServer) });
            mi.Invoke(O, new object[] { US });
        }

        public static void SendCommand(ComServer CS)
        {
            System.Reflection.MethodInfo mi =T.GetMethod("SendCommand", new Type[] { typeof(ComServer) });
            mi.Invoke(O, new object[] { CS });
        }

        public static void SendCommand(GsmServer GS)
        {
            System.Reflection.MethodInfo mi = T.GetMethod("SendCommand", new Type[] { typeof(GsmServer) });
            mi.Invoke(O, new object[] { GS });
        }

        public static void SendCommand(TcpServer TS)
        {
            System.Reflection.MethodInfo mi = T.GetMethod("SendCommand", new Type[] { typeof(TcpServer) });
            mi.Invoke(O, new object[] { TS });
        }

        public static void SendCommand(UdpServer US)
        {
            System.Reflection.MethodInfo mi = T.GetMethod("SendCommand", new Type[] { typeof(UdpServer) });
            mi.Invoke(O, new object[] { US });
        }
    }
}
