using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service
{
    public class OracleDBBussiness : DBBussiness
    { 
        public OracleData dt = null;
        public  OracleDBBussiness()
        {
            dt = new OracleData();
        }
        public OracleDBBussiness(string Path)
        {
            dt = new OracleData(Path);
        }

        object DBBussiness.dt
        {
            get { return dt; }
        }


        public IList<Model.YY_RTU_Basic> GetRTUList(string Where)
        {
            throw new NotImplementedException();
        }

        public bool AddRTU(Model.YY_RTU_Basic model)
        {
            throw new NotImplementedException();
        }

        public bool UpdRTU(Model.YY_RTU_Basic model, string Where)
        {
            throw new NotImplementedException();
        }

        public bool DelRTU(string Where)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_RTU_ITEM> GetItemList(string Where)
        {
            throw new NotImplementedException();
        }

        public bool UdpItem(Model.YY_RTU_ITEM model, string Where)
        {
            throw new NotImplementedException();
        }

        public bool AddItem(Model.YY_RTU_ITEM model)
        {
            throw new NotImplementedException();
        }

        public bool DelItem(string Where)
        {
            throw new NotImplementedException();
        }

        public bool UpdRTU_WRES(Model.YY_RTU_WRES model, string Where)
        {
            throw new NotImplementedException();
        }

        public bool AddRTU_WRES(Model.YY_RTU_WRES model)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_RTU_WRES> GetRTU_WRESList(string Where)
        {
            throw new NotImplementedException();
        }

        public bool DelRTU_WRES(string Where)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_RTU_WORK> GetRTU_WORKList(string Where)
        {
            throw new NotImplementedException();
        }

        public bool AddRTU_Work(Model.YY_RTU_WORK model)
        {
            throw new NotImplementedException();
        }

        public bool UdpRTU_Work(Model.YY_RTU_WORK model, string Where)
        {
            throw new NotImplementedException();
        }

        public bool DelRTU_Work(string Where)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRTU_CONFIGDATA(string Where)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_RTU_CONFIGDATA> GetRTU_CONFIGDATAList(string Where)
        {
            throw new NotImplementedException();
        }

        public bool DelRTU_ConfigData(string Where)
        {
            throw new NotImplementedException();
        }

        public bool AddRTU_ConfigData(Model.YY_RTU_CONFIGDATA model)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_RTU_TIME> GetRTU_TIMEList(string Where)
        {
            throw new NotImplementedException();
        }

        public bool UpdRTU_TIME(Model.YY_RTU_TIME model, string Where)
        {
            throw new NotImplementedException();
        }

        public bool AddRTU_TIME(Model.YY_RTU_TIME model)
        {
            throw new NotImplementedException();
        }

        public bool DelRTU_TIME(string Where)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_RTU_ITEMCONFIG> GetRTU_ItemConfig(string where)
        {
            throw new NotImplementedException();
        }

        public bool AddRTU_ItemConfig(Model.YY_RTU_ITEMCONFIG model)
        {
            throw new NotImplementedException();
        }

        public bool DelRTU_ItemConfig(string where)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRTU_ConfigItem(string STCD)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_RTU_CONFIGITEM> GetRTU_ConfigItemList(string Where)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_RTU_BI> GetRTU_BIList(string Where)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_RTU_COMMAND> GetRTUCommandList()
        {
            throw new NotImplementedException();
        }

        public bool AddRealTimeData(string STCD, string ItemID, DateTime TM, DateTime RTM, int NFOINDEX, decimal? Value)
        {
            throw new NotImplementedException();
        }

        public bool AddRealTimeData(Model.YY_DATA_AUTO model)
        {
            throw new NotImplementedException();
        }

        public bool UdpRealTimeData(Model.YY_DATA_AUTO model, string Where)
        {
            throw new NotImplementedException();
        }

        public bool DelRealTimeData(string Where)
        {
            throw new NotImplementedException();
        }

        public bool AddRemData(string STCD, string ItemID, DateTime TM, DateTime RTM, int NFOINDEX, decimal? Value)
        {
            throw new NotImplementedException();
        }

        public bool AddRemData(Model.YY_DATA_REM model)
        {
            throw new NotImplementedException();
        }

        public bool UdpRemData(Model.YY_DATA_REM model, string Where)
        {
            throw new NotImplementedException();
        }

        public bool DelRemData(string Where)
        {
            throw new NotImplementedException();
        }

        public bool AddDataLog(Model.YY_DATA_LOG model)
        {
            throw new NotImplementedException();
        }

        public bool DelDataLog(string Where)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_LOG> GetLog()
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_COMMAND_TEMP> GetCommandTempList()
        {
            throw new NotImplementedException();
        }

        public bool AddCommandTemp(Model.YY_COMMAND_TEMP model)
        {
            throw new NotImplementedException();
        }

        public bool AddCommandTemp(string STCD, int NFOINDEX, string CommandID, string Data, DateTime TM, int State)
        {
            throw new NotImplementedException();
        }

        public bool DelCommandTemp(string Where)
        {
            throw new NotImplementedException();
        }

        public bool DelCommandTemp(string STCD, int NFOINDEX, string CommandID)
        {
            throw new NotImplementedException();
        }

        public bool AddManualData(Model.YY_DATA_MANUAL model)
        {
            throw new NotImplementedException();
        }

        public bool UdpManualData(Model.YY_DATA_MANUAL model, string Where)
        {
            throw new NotImplementedException();
        }

        public bool DelManualData(string Where)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetManualDataForWhere(string Where)
        {
            throw new NotImplementedException();
        }

        public bool AddImg(Model.YY_DATA_IMG model)
        {
            throw new NotImplementedException();
        }

        public bool DelImg(string Where)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetImgForWhere(string Where)
        {
            throw new NotImplementedException();
        }

        public IList<Model.YY_DATA_IMG> GetImg(string STCD, DateTime TM)
        {
            throw new NotImplementedException();
        }

        public bool AddRTUState(string STCD, DateTime TM, DateTime RTM, int NFOINDEX, string AlarmsStr)
        {
            throw new NotImplementedException();
        }

        public bool AddDataCommand(string STCD, string CommandID, DateTime TM, DateTime? DOWNDATE, string Command, int NFOINDEX, int STATE)
        {
            throw new NotImplementedException();
        }

        public bool DelRTU_Item(string Where)
        {
            throw new NotImplementedException();
        }

        public bool AddRTU_Item(Model.YY_RTU_BI model)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRealTimeData()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRealTimeDataForWhere(string Where)
        {
            throw new NotImplementedException();
        }

        public int GetRealTimeDataCount(string Where)
        {
            throw new NotImplementedException();
        }

        public int GetRemDataCount(string Where)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRemDataForWhere(string Where)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRealTimeData(string STCD)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRealTimeData(string STCD, string[] ItemIDs)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRealTimeNewData()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRealTimeNewData(string[] ItemIDs)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRealTimeNewData(string[] ItemIDs,string STTYPE)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetCommandState(string Where)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRTUState(string STCD)
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable GetRTUNewState()
        {
            throw new NotImplementedException();
        }

        public System.Data.DataTable CreateRTUStateDataTable(System.Data.DataTable datatable)
        {
            throw new NotImplementedException();
        }
    }
}
