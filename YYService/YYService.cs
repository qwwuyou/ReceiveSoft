using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Service;
using System.IO;
using System.Runtime.InteropServices;

namespace YYService
{
    public partial class YYService : ServiceBase
    {
        ServiceControl sc;
        DateTime dt;
        public YYService()
        {
            InitializeComponent();
        }


        private Win32.ServiceControlHandlerEx myCallback;
        private int ServiceControlHandler(int control, int eventType, IntPtr eventData, IntPtr context)
        {

            //SERVICE_CONTROL_STOP //要服务停止
            //SERVICE_CONTROL_PAUSE //要服务暂停
            //SERVICE_CONTROL_CONTINUE//要服务继续
            //SERVICE_CONTROL_INTERROGATE//要服务马上报告它的状态
            //SERVICE_CONTROL_SHUTDOWN//告诉服务即将关机 
            if (control == Win32.SERVICE_CONTROL_STOP ) 
            {
                base.Stop();
            }
            else if (control == Win32.SERVICE_CONTROL_SHUTDOWN)
            {
                TimeSpan ts1 = new TimeSpan(ServiceControl.StartTime.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
                ServiceControl.log.Warn ( "系统运行时长：" + dateDiff + "服务停止，原因:系统关机！" );
                ServiceControl.log.Warn("*****************************************************************************");
            }
            else if (control == Win32.SERVICE_CONTROL_DEVICEEVENT)
            {
            }

            return 0;
        }
        protected override void OnStart(string[] args)
        {
            System.Threading.Thread.Sleep(10000);
            //AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            myCallback = new Win32.ServiceControlHandlerEx(ServiceControlHandler);
            Win32.RegisterServiceCtrlHandlerEx(this.ServiceName, myCallback, IntPtr.Zero);

            if (this.ServiceHandle == IntPtr.Zero)
            {
                // TODO handle error
            }

            dt = DateTime.Now;
            sc = new ServiceControl();
            try
            {
                sc.start();
            }
            catch (Exception e)
            {
                try
                {
                    Service.ServiceBussiness.SendMail();
                }
                catch (Exception ex)
                {
                    TimeSpan ts1 = new TimeSpan(ServiceControl.StartTime.Ticks);
                    TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                    TimeSpan ts = ts1.Subtract(ts2).Duration();
                    string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";

                    ServiceControl.log.Error(DateTime.Now + "系统运行时长：" + dateDiff + "服务启动，发送异常邮件时异常，异常原因:" + ex.ToString());
                }
                throw e;
            }
        }

        
        protected override void OnStop()
        {
            try
            {
                Service.ServiceBussiness.SendMail();
            }
            catch (Exception ex)
            {
                TimeSpan ts1 = new TimeSpan(ServiceControl.StartTime.Ticks);
                TimeSpan ts2 = new TimeSpan(DateTime.Now.Ticks);
                TimeSpan ts = ts1.Subtract(ts2).Duration();
                string dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";

                ServiceControl.log.Error(DateTime.Now + "系统运行时长：" + dateDiff +"服务停止，发送异常邮件时异常，异常原因:" + ex.ToString());
            }
            sc.stop();
        }

        //系统崩溃出现的异常此处捕捉到发送到指定邮箱
        //void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        //{

        //    SendMail();
        //}

        
    }


    public class Win32
    {
        public const int DEVICE_NOTIFY_SERVICE_HANDLE = 1;
        public const int DEVICE_NOTIFY_ALL_INTERFACE_CLASSES = 4;

        public const int SERVICE_CONTROL_STOP = 1;
        public const int SERVICE_CONTROL_DEVICEEVENT = 11;
        public const int SERVICE_CONTROL_SHUTDOWN = 5;

        public const uint GENERIC_READ = 0x80000000;
        public const uint OPEN_EXISTING = 3;
        public const uint FILE_SHARE_READ = 1;
        public const uint FILE_SHARE_WRITE = 2;
        public const uint FILE_SHARE_DELETE = 4;
        public const uint FILE_ATTRIBUTE_NORMAL = 128;
        public const uint FILE_FLAG_BACKUP_SEMANTICS = 0x02000000;
        public static readonly IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);

        public const int DBT_DEVTYP_DEVICEINTERFACE = 5;
        public const int DBT_DEVTYP_HANDLE = 6;

        public const int DBT_DEVICEARRIVAL = 0x8000;
        public const int DBT_DEVICEQUERYREMOVE = 0x8001;
        public const int DBT_DEVICEREMOVECOMPLETE = 0x8004;

        public const int WM_DEVICECHANGE = 0x219;

        public delegate int ServiceControlHandlerEx(int control, int eventType, IntPtr eventData, IntPtr context);

        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern IntPtr RegisterServiceCtrlHandlerEx(string lpServiceName, ServiceControlHandlerEx cbex, IntPtr context);

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool GetVolumePathNamesForVolumeNameW(
                [MarshalAs(UnmanagedType.LPWStr)]
					string lpszVolumeName,
                [MarshalAs(UnmanagedType.LPWStr)]
					string lpszVolumePathNames,
                uint cchBuferLength,
                ref UInt32 lpcchReturnLength);

        [DllImport("kernel32.dll")]
        public static extern bool GetVolumeNameForVolumeMountPoint(string
           lpszVolumeMountPoint, [Out] StringBuilder lpszVolumeName,
           uint cchBufferLength);

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr RegisterDeviceNotification(IntPtr IntPtr, IntPtr NotificationFilter, Int32 Flags);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern uint UnregisterDeviceNotification(IntPtr hHandle);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateFile(
              string FileName,                    // file name
              uint DesiredAccess,                 // access mode
              uint ShareMode,                     // share mode
              uint SecurityAttributes,            // Security Attributes
              uint CreationDisposition,           // how to create
              uint FlagsAndAttributes,            // file attributes
              int hTemplateFile                   // handle to template file
              );

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool CloseHandle(IntPtr hObject);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public struct DEV_BROADCAST_DEVICEINTERFACE
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 16)]
            public byte[] dbcc_classguid;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public char[] dbcc_name;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HDR
        {
            public int dbcc_size;
            public int dbcc_devicetype;
            public int dbcc_reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct DEV_BROADCAST_HANDLE
        {
            public int dbch_size;
            public int dbch_devicetype;
            public int dbch_reserved;
            public IntPtr dbch_handle;
            public IntPtr dbch_hdevnotify;
            public Guid dbch_eventguid;
            public long dbch_nameoffset;
            public byte dbch_data;
            public byte dbch_data1;
        }
    }
}
