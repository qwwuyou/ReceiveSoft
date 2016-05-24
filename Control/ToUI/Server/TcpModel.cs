using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace ToUI
{
    //socket对象
   public class TcpSocket
    {
        private Socket _tcpsocket;
        private DateTime _connecttime;
        private DateTime _datatime;
       
        /// <summary>
        /// tcp的socket对象
        /// </summary>
        public Socket TCPSOCKET 
        {
            get { return _tcpsocket; }
            set { _tcpsocket = value; }
        }

        /// <summary>
        /// 上线时间
        /// </summary>
        public DateTime CONNECTTIME
        {
            get { return _connecttime; }
            set { _connecttime = value; }
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
   public class TcpReceivedData 
   {
       private byte[] _data;
       private Socket _socket;

       /// <summary>
       /// 接收到的数据
       /// </summary>
       public byte[] Data 
       {
           get { return _data; }
           set { _data = value; }
       }

       /// <summary>
       /// socket对象
       /// </summary>
       public Socket SOCKET 
       {
           get { return _socket; }
           set { _socket = value; }
       }
   }

    //回复的数据
   public class TcpSendData 
   {
       private string _stcd;
       private byte[] _data;

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
   }

}
