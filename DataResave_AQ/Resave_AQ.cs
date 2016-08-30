using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service;
using System.Xml;
using System.Collections.Concurrent;

namespace DataResave_AQ
{
    public class Resave_AQ : ResaveProcess
    {
        string FieldPath = System.Windows.Forms.Application.StartupPath + "/Log/DataResaveAQ";

        ServiceReference_AQ.AQAcquisitionServiceClient client = null;
        string authToken = "";
        static List<TimeSeries> TimeSeriesList;
        IList<Service.Model.YY_RTU_ITEM> list = null;
        ConcurrentQueue<Service.Model.YY_DATA_AUTO> ToAqDataQueue = null;
        
        public Resave_AQ()
        { 
            string FileName = "Warn" + DateTime.Now.ToString("yyyy-MM-dd");
            try
            {
                client = new ServiceReference_AQ.AQAcquisitionServiceClient();
            }
            catch
            { Service._51Data.SystemError.SystemLog(FieldPath, FileName, "实例化web服务失败"); }

            ToAqDataQueue = new ConcurrentQueue<Service.Model.YY_DATA_AUTO>();
            try
            {
                TimeSeriesList = ReadTimeSeriesXml();
            }
            catch
            { Service._51Data.SystemError.SystemLog(FieldPath, FileName, "读取AQxml配置文件失败"); }
            list = Service.PublicBD.db.GetItemList("");


            System.Threading.Thread autoToAq_th = new System.Threading.Thread(autoToAq);
            autoToAq_th.Start();
            System.Threading.Thread GetauthToken_th = new System.Threading.Thread(GetauthToken);
            GetauthToken_th.Start();
        }

        private void autoToAq()
        {
            string FileName = "";
            string message = "";
                
            string append = "";
            Service.Model.YY_DATA_AUTO item;
            while (true)
            {
                FileName = "Info" + DateTime.Now.ToString("yyyy-MM-dd") ;
                System.Threading.Thread.Sleep(5 * 1000);

                lock (ToAqDataQueue)
                    if (ToAqDataQueue.TryDequeue(out item))
                    {
                        append = AddDataToAQ(item);
                        if (append == "")
                        {
                            if (item.NFOINDEX < 5)
                            {
                                item.NFOINDEX++;
                                ToAqDataQueue.Enqueue(item);
                                
                                message = "站号：" + item.STCD + "   监测项：" + item.ItemID + "   监测时间：" + item.TM + "   监测值：" + item.DATAVALUE + "-----失败";
                                Service._51Data.SystemError.SystemLog(FieldPath, FileName, message);
                            }
                            else 
                            {
                                message = "站号：" + item.STCD + "   监测项：" + item.ItemID + "   监测时间：" + item.TM + "   监测值：" + item.DATAVALUE + "-----放弃";
                                Service._51Data.SystemError.SystemLog(FieldPath, FileName, message);
                            }
                        }
                        else
                        {
                            message ="站号：" + item.STCD + "   监测项：" + item.ItemID + "   监测时间：" + item.TM + "   监测值：" + item.DATAVALUE + "-----成功";
                            Service._51Data.SystemError.SystemLog(FieldPath, FileName, message);
                        }
                    }
                System.Threading.Thread.Sleep(5 * 1000);
            }
        }

        private void GetauthToken()
        {
            string FileName = "";
            while (true)
            {
                lock (authToken) 
                {
                    try
                    {
                        authToken = client.GetAuthToken("admin", "admin");
                    }
                    catch
                    {
                        FileName = "Warn" + DateTime.Now.ToString("yyyy-MM-dd") ;
                        Service._51Data.SystemError.SystemLog(FieldPath, FileName, "获得Token失败"); 
                    }
                }
                System.Threading.Thread.Sleep(5 *60* 1000);
            }
        }

        public void InitInfo()
        {
            lock (TimeSeriesList)
            TimeSeriesList = ReadTimeSeriesXml();
        }

        public void Resave(Service.Model.YY_DATA_AUTO model)
        {
            AddDataToAQ(model);
        }



        private string AddDataToAQ(Service.Model.YY_DATA_AUTO model)
        {
            string message = "";
            string FileName = "Info" + DateTime.Now.ToString("yyyy-MM-dd");

            string append = "";
            try
            {
                if (client != null)
                {
                    byte[] b = (new System.Text.ASCIIEncoding()).GetBytes(model.TM.ToString("yyyy-MM-ddTHH:mm:ss.fffzzz") + "," + model.DATAVALUE);
                    var Item = from il in list where il.ItemID == model.ItemID select il;
                    string QW_ts = Item.First().ItemName + "@" + model.STCD;

                    if (TimeSeriesList != null && TimeSeriesList.Count > 0)
                    {
                        if (model.DATATYPE == 2011)
                        {
                            var TS = from Ts in TimeSeriesList where Ts.QWService == QW_ts && (Ts.DataType == ""||Ts.DataType == "2011") select Ts;
                            if (TS.Count() > 0)
                            {
                                append = client.AppendTimeSeriesAsync(authToken, Guid.Parse(TS.First().AQ), b); //NGAQ上数
                            }
                        }
                        else
                        { 
                            var TS = from Ts in TimeSeriesList where Ts.QWService == QW_ts && Ts.DataType == model.DATATYPE.ToString()  select Ts;
                            if (TS.Count() > 0)
                            {
                                append = client.AppendTimeSeriesAsync(authToken, Guid.Parse(TS.First().AQ), b); //NGAQ上数
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                message = "站号：" + model.STCD + "   监测项：" + model.ItemID + "   监测时间：" + model.TM + "   监测值：" + model.DATAVALUE + "-----失败";
                Service._51Data.SystemError.SystemLog(FieldPath, FileName, message);

                Console.Write(e.ToString());
                append = "";
                lock (ToAqDataQueue)
                {
                    ToAqDataQueue.Enqueue(model);
                }
            }
            return append;
        }
        /// <summary>
        /// 读取时间序列列表
        /// </summary>
        /// <returns></returns>
        public static List<TimeSeries> ReadTimeSeriesXml()
        {
            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(System.Windows.Forms.Application.StartupPath + "/TimeSeries.xml");
                XmlNodeList root = xmlDoc.SelectSingleNode("TimeSeries").ChildNodes;
                List<TimeSeries> list = new List<TimeSeries>();
                TimeSeries ts;
                foreach (XmlNode item in root)
                {
                    ts = new TimeSeries();
                    ts.ID = item.SelectSingleNode("ID").InnerText;
                    ts.DataType = item.SelectSingleNode("QWService").Attributes["DataType"].Value;
                    ts.QWService = item.SelectSingleNode("QWService").InnerText;
                    ts.AQ = item.SelectSingleNode("AQ").InnerText;
                    list.Add(ts);
                }
                return list;
            }
            catch { }
            return null;
        }
    }

    /// <summary>
    /// 时间序列列表
    /// </summary>
    public class TimeSeries
    {
        public TimeSeries()
        { }
        #region Model
        private string _id;
        private string _datatype;
        private string _qwservice;
        private string _aq;

        /// <summary>
        /// 
        /// </summary>
        public string ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string DataType
        {
            set { _datatype = value; }
            get { return _datatype; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string QWService
        {
            set { _qwservice = value; }
            get { return _qwservice; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string AQ
        {
            set { _aq = value; }
            get { return _aq; }
        }
        #endregion Model

    }
}
