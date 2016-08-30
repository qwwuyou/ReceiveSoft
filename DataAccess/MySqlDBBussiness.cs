using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service.Model;
using System.Data;

namespace Service
{
    public class MySqlDBBussiness : DBBussiness
    {
        public MySqlData dt = null;
        public  MySqlDBBussiness()
        {
            dt = new MySqlData();
        }
        public MySqlDBBussiness(string Path)
        { 
            dt = new MySqlData(Path);
        }

        object DBBussiness.dt
        {
            get { return dt; }
        }

        #region 测站信息操作
        /// <summary>
        /// 得到RTU列表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<YY_RTU_Basic> GetRTUList(string Where)
        {
            return dt.Select<YY_RTU_Basic>("YY_RTU_Basic", new string[] { "*" }, Where);
        }

        /// <summary>
        /// 添加测站信息
        /// </summary>
        /// <param name="model">测站实体</param>
        /// <returns></returns>
        public bool AddRTU(YY_RTU_Basic model)
        {
            return dt.Insert<YY_RTU_Basic>("YY_RTU_Basic", model);
        }

        /// <summary>
        /// 更新RTU信息
        /// </summary>
        /// <param name="model">测站实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool UpdRTU(YY_RTU_Basic model, string Where)
        {
            return dt.Update<YY_RTU_Basic>("YY_RTU_Basic", model, Where);
        }

        /// <summary>
        /// 删除RTU信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelRTU(string Where)
        {
            return dt.Delete("delete from YY_RTU_Basic " + Where);
        }
        #endregion

        #region 监测项操作
        /// <summary>
        /// 得到监测项列表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<YY_RTU_ITEM> GetItemList(string Where)
        {
            return dt.Select<YY_RTU_ITEM>("YY_RTU_ITEM", new string[] { "*" }, Where);
        }

        /// <summary>
        /// 更新监测项信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool UdpItem(YY_RTU_ITEM model, string Where)
        {
            return dt.Update<YY_RTU_ITEM>("YY_RTU_ITEM", model, Where);
        }

        /// <summary>
        /// 添加监测项信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddItem(YY_RTU_ITEM model)
        {
            return dt.Insert<YY_RTU_ITEM>("YY_RTU_ITEM", model);
        }

        /// <summary>
        /// 删除监测项信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelItem(string Where)
        {
            return dt.Delete("delete from YY_RTU_ITEM " + Where);
        }
        #endregion

        #region 操作中心站信息配置表
        /// <summary>
        /// 更新中心站信息配置
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool UpdRTU_WRES(YY_RTU_WRES model, string Where)
        {
            return dt.Update<YY_RTU_WRES>("YY_RTU_WRES", model, Where);
        }

        /// <summary>
        /// 添加中心站信息配置
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddRTU_WRES(YY_RTU_WRES model)
        {
            return dt.Insert<YY_RTU_WRES>("YY_RTU_WRES", model);
        }

        /// <summary>
        /// 根据条件得到中心站信息配置表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<YY_RTU_WRES> GetRTU_WRESList(string Where)
        {
            return dt.Select<YY_RTU_WRES>("YY_RTU_WRES", new string[] { "*" }, Where);
        }

        /// <summary>
        /// 删除中心站信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelRTU_WRES(string Where)
        {
            return dt.Delete("delete from YY_RTU_WRES " + Where);
        }
        #endregion

        #region 操作测站工作状态信息表YY_RTU_WORK
        /// <summary>
        /// 查询测站工作状态信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<YY_RTU_WORK> GetRTU_WORKList(string Where)
        {
            return dt.Select<YY_RTU_WORK>("YY_RTU_WORK", new string[] { "*" }, Where);
        }
        /// <summary>
        /// 添加测站工作状态信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddRTU_Work(YY_RTU_WORK model)
        {
            return dt.Insert<YY_RTU_WORK>("YY_RTU_WORK", model);
        }

        /// <summary>
        /// 更新测站工作状态信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool UdpRTU_Work(YY_RTU_WORK model, string Where)
        {
            return dt.Update<YY_RTU_WORK>("YY_RTU_WORK", model, Where);
        }

        /// <summary>
        /// 删除测站工作状态信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelRTU_Work(string Where) 
        {
            return dt.Delete("delete from YY_RTU_WORK " + Where);
        }
        #endregion

        #region 操作测站监测项配置信息表
        /// <summary>
        /// 得到配置项数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public DataTable GetRTU_CONFIGDATA(string Where)
        {
            string[] fields = new string[] { "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemName", "YY_RTU_CONFIGITEM.ConfigItem", "YY_RTU_CONFIGDATA.STCD", "YY_RTU_CONFIGDATA.ItemID", "YY_RTU_CONFIGDATA.ConfigID", "YY_RTU_CONFIGDATA.ConfigVal" };
            string tablename = "YY_RTU_Basic RIGHT OUTER JOIN    YY_RTU_CONFIGDATA ON YY_RTU_Basic.STCD = YY_RTU_CONFIGDATA.STCD LEFT OUTER JOIN YY_RTU_CONFIGITEM ON YY_RTU_CONFIGDATA.ConfigID = YY_RTU_CONFIGITEM.ConfigID LEFT OUTER JOIN YY_RTU_ITEM ON YY_RTU_CONFIGDATA.ItemID = YY_RTU_ITEM.ItemID";
            return dt.Select(tablename, fields, Where);
        }

        /// <summary>
        /// 得到配置项数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<YY_RTU_CONFIGDATA> GetRTU_CONFIGDATAList(string Where)
        {
            return dt.Select<YY_RTU_CONFIGDATA>("YY_RTU_CONFIGDATA", new string[] { "*" }, Where);
        }

        /// <summary>
        /// 删除测站监测项配置信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelRTU_ConfigData(string Where)
        {
            return dt.Delete("delete from YY_RTU_CONFIGDATA " + Where);
        }

        /// <summary>
        /// 添加测站监测项配置信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddRTU_ConfigData(YY_RTU_CONFIGDATA model)
        {
            return dt.Insert<YY_RTU_CONFIGDATA>("YY_RTU_CONFIGDATA", model);
        }
        #endregion

        #region 操作测站自报固态属性表YY_RTU_TIME
        /// <summary>
        /// 查询测站自报固态属性信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<YY_RTU_TIME> GetRTU_TIMEList(string Where)
        {
            return dt.Select<YY_RTU_TIME>("YY_RTU_TIME", new string[] { "*" }, Where);
        }

        /// <summary>
        /// 更新测站自报固态属性信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool UpdRTU_TIME(YY_RTU_TIME model, string Where)
        {
            return dt.Update<YY_RTU_TIME>("YY_RTU_TIME", model, Where);
        }
        /// <summary>
        /// 添加测站自报固态属性信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddRTU_TIME(YY_RTU_TIME model)
        {
            return dt.Insert<YY_RTU_TIME>("YY_RTU_TIME", model);
        }

        /// <summary>
        /// 删除测站自报固态属性信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelRTU_TIME(string Where)
        {
            return dt.Delete("delete from YY_RTU_TIME " + Where);
        }
        #endregion


        #region 配置监测项与配置项关系（工具功能）YY_RTU_ITEMCONFIG
        /// <summary>
        /// 根据条件得到监测项与配置项的关系
        /// </summary>
        /// <param name="where">条件</param>
        /// <returns></returns>
        public IList<YY_RTU_ITEMCONFIG> GetRTU_ItemConfig(string where)
        {
            return dt.Select<YY_RTU_ITEMCONFIG>("YY_RTU_ITEMCONFIG", new string[] { "*" }, where);
        }

        /// <summary>
        /// 添加监测项与配置项的关系
        /// </summary>
        /// <param name="model">关系实体</param>
        /// <returns></returns>
        public bool AddRTU_ItemConfig(YY_RTU_ITEMCONFIG model)
        {
            return dt.Insert<YY_RTU_ITEMCONFIG>("YY_RTU_ITEMCONFIG", model);
        }

        /// <summary>
        ///  根据条件删除监测项与配置项的关系
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public bool DelRTU_ItemConfig(string where)
        {
            return dt.Delete("delete from YY_RTU_ITEMCONFIG " + where);
        }
        #endregion

        /// <summary>
        /// 根据站号得到关联的配置项
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <returns></returns>
        public DataTable GetRTU_ConfigItem(string STCD)
        {
            string[] Fields = new string[] { "YY_RTU_ITEM.ItemName, YY_RTU_CONFIGITEM.ConfigItem, YY_RTU_BI.STCD, YY_RTU_BI.ItemID, YY_RTU_ITEMCONFIG.ConfigID" };
            string TableName = " YY_RTU_ITEMCONFIG RIGHT OUTER JOIN YY_RTU_BI ON YY_RTU_ITEMCONFIG.ItemID =  YY_RTU_BI.ItemID LEFT OUTER JOIN" +
                               " YY_RTU_CONFIGITEM ON YY_RTU_ITEMCONFIG.ConfigID = YY_RTU_CONFIGITEM.ConfigID LEFT OUTER JOIN YY_RTU_ITEM ON YY_RTU_ITEMCONFIG.ItemID = YY_RTU_ITEM.ItemID";
            string Where = " Where YY_RTU_ITEMCONFIG.ConfigID is not null and YY_RTU_BI.STCD='" + STCD + "'" ;
            return dt.Select(TableName, Fields, Where);
        }

        /// <summary>
        /// 得到监测项配置项列表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<YY_RTU_CONFIGITEM> GetRTU_ConfigItemList(string Where) 
        {
            return dt.Select<YY_RTU_CONFIGITEM>("YY_RTU_CONFIGITEM",new string[]{"*"},Where);
        }

        /// <summary>
        /// 得到测站和监测项关系列表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<YY_RTU_BI> GetRTU_BIList(string Where)
        {
            return dt.Select<YY_RTU_BI>("YY_RTU_BI", new string[] { "*" }, Where);
        }

        /// <summary>
        /// 得到RTU召测列表
        /// </summary>
        /// <returns></returns>
        public IList<YY_RTU_COMMAND> GetRTUCommandList()
        {
            return dt.Select<YY_RTU_COMMAND>("YY_RTU_COMMAND", new string[] { "*" }, "where state=1");
        }


        #region 操作实时数据表
        /// <summary>
        /// 添加实时数据
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="ItemID">监测项号</param>
        /// <param name="TM">监测时间</param>
        /// <param name="RTM">接收时间</param>
        /// <param name="NFOINDEX">信道类型</param>
        /// <param name="Value">数据值</param>
        /// <returns></returns>
        public bool AddRealTimeData(string STCD, string ItemID, DateTime TM, DateTime RTM, int NFOINDEX, decimal? Value)
        {
            YY_DATA_AUTO model = new YY_DATA_AUTO();
            model.STCD = STCD;
            model.ItemID = ItemID;
            model.TM = TM;
            model.DOWNDATE = RTM;
            model.NFOINDEX = NFOINDEX;
            model.DATAVALUE = Value;
            model.CorrectionVALUE = Value;
            //model.DATATYPE = null;
            return dt.Insert<YY_DATA_AUTO>("YY_DATA_AUTO", model);
        }

        /// <summary>
        /// 添加实时数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddRealTimeData(YY_DATA_AUTO model) 
        {
            return dt.Insert<YY_DATA_AUTO>("YY_DATA_AUTO", model);
        }

        /// <summary>
        /// 更新实时数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool UdpRealTimeData(YY_DATA_AUTO model,string Where) 
        {
            return dt.Update<YY_DATA_AUTO>("YY_DATA_AUTO",model,Where);
        }

        /// <summary>
        /// 删除实时数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelRealTimeData(string Where)
        {
            return dt.Update("YY_DATA_AUTO", new string[] { "CorrectionVALUE", "NFOINDEX" }, new object[] { DBNull.Value, 5 }, Where);
            //return dt.Delete("delete from YY_DATA_AUTO " + Where);
        }
        #endregion


        #region 操作固态数据表
        /// <summary>
        /// 添加固态数据
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="ItemID">监测项号</param>
        /// <param name="TM">监测时间</param>
        /// <param name="RTM">接收时间</param>
        /// <param name="NFOINDEX">信道类型</param>
        /// <param name="Value">数据值</param>
        /// <returns></returns>
        public bool AddRemData(string STCD, string ItemID, DateTime TM, DateTime RTM, int NFOINDEX, decimal? Value) 
        {
            YY_DATA_REM model = new YY_DATA_REM();
            model.STCD = STCD;
            model.ItemID = ItemID;
            model.TM = TM;
            model.DOWNDATE = RTM;
            model.NFOINDEX = NFOINDEX;
            model.DATAVALUE = Value;
            return dt.Insert<YY_DATA_REM>("YY_DATA_REM", model);
        }

        /// <summary>
        /// 添加固态数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddRemData(YY_DATA_REM model) 
        {
            return dt.Insert<YY_DATA_REM>("YY_DATA_REM", model);
        }

        /// <summary>
        /// 更新固态数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool UdpRemData(YY_DATA_REM model, string Where)
        {
            return dt.Update<YY_DATA_REM>("YY_DATA_REM", model, Where);
        }

        /// <summary>
        /// 删除固态数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelRemData(string Where)
        {
            return dt.Delete("delete from YY_DATA_REM " + Where);
        }
        #endregion

        #region 操作事件记录表YY_DATA_LOG
        /// <summary>
        /// 添加事件记录
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddDataLog(YY_DATA_LOG model)
        {
            return dt.Insert<YY_DATA_LOG>("YY_DATA_LOG", model);
        }

        /// <summary>
        /// 删除事件记录
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelDataLog(string Where)
        {
            return dt.Delete("delete from YY_DATA_LOG " + Where);
        }
        #endregion

        #region 得到事件描述
        /// <summary>
        /// 得到事件描述
        /// </summary>
        /// <returns></returns>
        public IList<YY_LOG> GetLog() 
        {
            return  dt.Select<YY_LOG>("YY_LOG", new string[] { "*" }, "");
        }
        #endregion

        #region 操作命令临时表YY_COMMAND_TEMP
        /// <summary>
        /// 得到命令临时表中的记录
        /// </summary>
        /// <returns></returns>
        public IList<YY_COMMAND_TEMP> GetCommandTempList() 
        {
            return dt.Select<YY_COMMAND_TEMP>("YY_COMMAND_TEMP", new string[] { "*" }, "");
        }

        /// <summary>
        /// 添加命令状态临时数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddCommandTemp(YY_COMMAND_TEMP model) 
        { 
            return dt.Insert<YY_COMMAND_TEMP>("YY_COMMAND_TEMP", model);
        }

        /// <summary>
        /// 添加命令状态临时数据
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="NFOINDEX">信道</param>
        /// <param name="CommandID">命令码</param>
        /// <param name="Data">命令内容</param>
        /// <param name="TM">时间</param>
        /// <param name="State">状态</param>
        /// <returns></returns>
        public bool AddCommandTemp(string STCD, int NFOINDEX, string CommandID,string Data,DateTime TM,int State)
        {
            YY_COMMAND_TEMP model = new YY_COMMAND_TEMP();
            model.STCD=STCD;
            model.NFOINDEX =NFOINDEX ;
            model.CommandID=CommandID;
            model.Data = Data;
            model.TM = TM;
            model.State = State;
            return AddCommandTemp( model);
        } 

        /// <summary>
        /// 删除命令状态临时数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelCommandTemp(string Where)
        {
            return dt.Delete ("delete from YY_COMMAND_TEMP "+Where );
        }

        /// <summary>
        /// 删除命令状态临时数据
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="NFOINDEX">信道</param>
        /// <param name="CommandID">命令码</param>
        /// <returns></returns>
        public bool DelCommandTemp(string STCD, int NFOINDEX, string CommandID)
        {
            string Where = " Where STCD='" + STCD + "' and NFOINDEX=" + NFOINDEX + " and CommandID='" + CommandID + "'";
            return DelCommandTemp(Where);
        }
        #endregion


        #region 人工置数表
        /// <summary>
        /// 添加人工置数数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddManualData(YY_DATA_MANUAL model) 
        { 
            return dt.Insert<YY_DATA_MANUAL>("YY_DATA_MANUAL", model);
        }

        /// <summary>
        /// 更新人工置数数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool UdpManualData(YY_DATA_MANUAL model, string Where)
        {
            return dt.Update<YY_DATA_MANUAL>("YY_DATA_MANUAL", model, Where);
        }

        /// <summary>
        /// 删除人工置数数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelManualData(string Where)
        {
            return dt.Delete("delete from YY_DATA_MANUAL " + Where);
        }

        /// <summary>
        /// 得到实时前100条数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public System.Data.DataTable GetManualDataForWhere(string Where)
        {
            string name = " YY_DATA_MANUAL RIGHT OUTER JOIN   YY_RTU_Basic ON YY_DATA_MANUAL.STCD = YY_RTU_Basic.STCD ";
            string[] fields = new string[] { " YY_RTU_Basic.STCD, YY_RTU_Basic.NiceName, YY_DATA_MANUAL.DOWNDATE, YY_DATA_MANUAL.NFOINDEX,YY_DATA_MANUAL.DATAVALUE, YY_DATA_MANUAL.DATATYPE,YY_DATA_MANUAL.TM" };
            return dt.Select(name, fields, Where + " limit 100");      
        }
        #endregion
        
        #region 操作图片
        /// <summary>
        /// 添加图片
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddImg(YY_DATA_IMG model)
        {
            return dt.Insert<YY_DATA_IMG>("YY_DATA_IMG", model);
        }

        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelImg(string Where)
        {
            return dt.Delete("delete from YY_DATA_IMG " + Where);
        }

        /// <summary>
        /// 得到前100幅图片
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public System.Data.DataTable GetImgForWhere(string Where)
        {
            string name = " YY_DATA_IMG RIGHT OUTER JOIN   YY_RTU_Basic ON YY_DATA_IMG.STCD = YY_RTU_Basic.STCD ";
            string[] fields = new string[] { " YY_RTU_Basic.STCD, YY_RTU_Basic.NiceName, YY_DATA_IMG.DOWNDATE, YY_DATA_IMG.NFOINDEX, YY_DATA_IMG.DATATYPE,YY_DATA_IMG.TM,YY_DATA_IMG.INFO" };
            return dt.Select(name, fields, Where + " limit 100");
        }

        /// <summary>
        /// 根据站号时间得到图片
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="TM">时间</param>
        /// <returns></returns>
        public IList<YY_DATA_IMG> GetImg(string STCD, DateTime TM)
        {
            return dt.Select<YY_DATA_IMG>("YY_DATA_IMG", new string[] { "*" }, " where STCD='" + STCD + "' and TM ='" + TM + "'");
        }
        #endregion


        #region 操作状态表
        /// <summary>
        /// 得到状态列表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<YY_STATE> GetStateList(string Where)
        {
            return dt.Select<YY_STATE>("YY_STATE", new string[] { "*" }, Where);
        }

        /// <summary>
        /// 添加终端状态和报警状态
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="TM">监测时间</param>
        /// <param name="RTM">接收时间</param>
        /// <param name="NFOINDEX">信道类型</param>
        /// <param name="Alarms">报警数据</param>
        /// <returns></returns>
        public bool AddRTUState(string STCD, DateTime TM, DateTime RTM, int NFOINDEX, string AlarmsStr)
        {
            YY_DATA_STATE model = new YY_DATA_STATE();
            model.STCD = STCD;
            model.TM = TM;
            model.DOWNDATE = RTM;
            model.NFOINDEX = NFOINDEX;
            model.STATEDATA = AlarmsStr;
            return dt.Insert<YY_DATA_STATE>("YY_DATA_STATE", model);
        }

        /// <summary>
        /// 添加终端状态和报警状态
        /// </summary>
        /// <param name="model">状态实体</param>
        /// <returns></returns>
        public bool AddRTUState(YY_DATA_STATE model)
        {
            return dt.Insert<YY_DATA_STATE>("YY_DATA_STATE", model);
        }
        #endregion

        /// <summary>
        /// 添加召测命令记录
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="CommandID">命令码</param>
        /// <param name="TM">发送时间</param>
        /// <param name="DOWNDATE">接收时间</param>
        /// <param name="Command">命令数据</param>
        /// <param name="NFOINDEX">信道</param>
        /// <param name="STATE">状态 -1超时  -2回复</param>
        /// <returns></returns>
        public bool AddDataCommand(string STCD, string CommandID, DateTime TM, DateTime? DOWNDATE, string Command, int NFOINDEX, int STATE)
        {
            YY_DATA_COMMAND model = new YY_DATA_COMMAND();
            model.STCD = STCD;
            model.CommandID = CommandID;
            model.TM = TM;
            model.DOWNDATE = DOWNDATE;
            model.NFOINDEX = NFOINDEX;
            model.STATE = STATE;
            model.Command = Command;
            return dt.Insert<YY_DATA_COMMAND>("YY_DATA_COMMAND", model);
        }

        /// <summary>
        /// 删除测站与监测项关系
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelRTU_Item(string Where)
        {
            return dt.Delete("delete from YY_RTU_BI " + Where);
        }

        /// <summary>
        /// 添加测站与监测项关系
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns></returns>
        public bool AddRTU_Item(YY_RTU_BI model)
        {
            return dt.Insert<YY_RTU_BI>("YY_RTU_BI", model);
        }

        #region 得到数据
        /// <summary>
        /// 得到实时前100条数据
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable GetRealTimeData()   //**********
        {
            string name = "(SELECT STCD, ItemID, TM, DOWNDATE, NFOINDEX, DATAVALUE,CorrectionVALUE, DATATYPE FROM  YY_DATA_AUTO ORDER BY TM DESC limit 100) AS yda LEFT OUTER JOIN YY_RTU_Basic ON yda.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN YY_RTU_ITEM ON yda.ItemID = YY_RTU_ITEM.ItemID";
            string[] fields = new string[] { "yda.TM, YY_RTU_Basic.STCD,yda.CorrectionVALUE, YY_RTU_Basic.NiceName, YY_RTU_ITEM.ItemID, YY_RTU_ITEM.ItemName, YY_RTU_ITEM.ItemDecimal,YY_RTU_ITEM.PlusOrMinus, yda.DOWNDATE, yda.NFOINDEX, yda.DATAVALUE, yda.DATATYPE" };
            string Where = "order by yda.TM DESC";
            return dt.Select(name, fields, Where);

            //string name = "YY_DATA_AUTO RIGHT OUTER JOIN YY_RTU_Basic ON YY_DATA_AUTO.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN  YY_RTU_ITEM ON YY_DATA_AUTO.ItemID = YY_RTU_ITEM.ItemID  ";
            //string[] fields = new string[] { " YY_RTU_Basic.STCD", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemID", "YY_RTU_ITEM.ItemName", "YY_DATA_AUTO.TM", "YY_DATA_AUTO.DOWNDATE", "YY_DATA_AUTO.NFOINDEX", "YY_DATA_AUTO.CorrectionVALUE", "YY_DATA_AUTO.DATATYPE", "YY_RTU_ITEM.ItemInteger", "YY_RTU_ITEM.ItemDecimal", "YY_RTU_ITEM.PlusOrMinus" };
            //string Where = " where YY_DATA_AUTO.CorrectionVALUE is not NULL order by YY_DATA_AUTO.TM desc  limit 100";
            //return dt.Select(name, fields,Where);
        }
        /// <summary>
        /// 得到实时前100条数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public System.Data.DataTable GetRealTimeDataForWhere(string Where) //**********
        {
            string name = "(SELECT STCD, ItemID, TM, DOWNDATE, NFOINDEX, DATAVALUE,CorrectionVALUE, DATATYPE FROM  YY_DATA_AUTO  " + Where + " ORDER BY TM DESC limit 100) AS yda LEFT OUTER JOIN YY_RTU_Basic ON yda.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN YY_RTU_ITEM ON yda.ItemID = YY_RTU_ITEM.ItemID";
            string[] fields = new string[] { "yda.TM, YY_RTU_Basic.STCD,yda.CorrectionVALUE, YY_RTU_Basic.NiceName, YY_RTU_ITEM.ItemID, YY_RTU_ITEM.ItemName, YY_RTU_ITEM.ItemDecimal,YY_RTU_ITEM.PlusOrMinus, yda.DOWNDATE, yda.NFOINDEX, yda.DATAVALUE, yda.DATATYPE" };
            string where = "order by yda.TM DESC";
            return dt.Select(name, fields, where);

            //string name = "YY_DATA_AUTO RIGHT OUTER JOIN YY_RTU_Basic ON YY_DATA_AUTO.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN  YY_RTU_ITEM ON YY_DATA_AUTO.ItemID = YY_RTU_ITEM.ItemID ";
            //string[] fields = new string[] { "  YY_RTU_Basic.STCD", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemID", "YY_RTU_ITEM.ItemName", "YY_DATA_AUTO.TM", "YY_DATA_AUTO.CorrectionVALUE", "YY_DATA_AUTO.DOWNDATE", "YY_DATA_AUTO.NFOINDEX", "YY_DATA_AUTO.DATATYPE", "YY_RTU_ITEM.ItemDecimal" };
            //return dt.Select(name, fields, Where + " limit 100");
        }

        /// <summary>
        /// 得到符合条件的记录数量(实时数据)
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public int GetRealTimeDataCount(string Where)
        {
            return dt.Select<YY_DATA_AUTO>("YY_DATA_AUTO", new string[] { "*" }, Where).Count();
        }

        /// <summary>
        /// 得到符合条件的记录数量(固态数据)
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public int GetRemDataCount(string Where)
        {
            return dt.Select<YY_DATA_REM>("YY_DATA_REM", new string[] { "*" }, Where).Count();
        }

        /// <summary>
        /// 得到固态前100条数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public System.Data.DataTable GetRemDataForWhere(string Where) //**********
        {
            string name = "(SELECT STCD, ItemID, TM, DOWNDATE, NFOINDEX, DATAVALUE, DATATYPE FROM YY_DATA_REM " + Where + " ORDER BY TM DESC limit 100) AS ydr LEFT OUTER JOIN YY_RTU_ITEM ON ydr.ItemID = YY_RTU_ITEM.ItemID LEFT OUTER JOIN YY_RTU_Basic ON ydr.STCD = YY_RTU_Basic.STCD ";
            string[] fields = new string[] { " ydr.TM, ydr.DOWNDATE, ydr.NFOINDEX, ydr.DATAVALUE, YY_RTU_Basic.STCD, YY_RTU_Basic.NiceName, YY_RTU_ITEM.ItemID, YY_RTU_ITEM.ItemName, YY_RTU_ITEM.ItemDecimal " };
            string where = "ORDER BY ydr.TM DESC";
            return dt.Select(name, fields, where);

            //string name = "YY_DATA_REM RIGHT OUTER JOIN YY_RTU_Basic ON YY_DATA_REM.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN  YY_RTU_ITEM ON YY_DATA_REM.ItemID = YY_RTU_ITEM.ItemID ";
            //string[] fields = new string[] { "  YY_RTU_Basic.STCD", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemID", "YY_RTU_ITEM.ItemName", "YY_DATA_REM.TM", "YY_DATA_REM.DATAVALUE", "YY_DATA_REM.DOWNDATE", "YY_DATA_REM.NFOINDEX", "YY_RTU_ITEM.ItemDecimal" };
            //return dt.Select(name, fields, Where + " limit 100");
        }
        // <summary>
        /// 根据测站站号得到前100条数据
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <returns></returns>
        public System.Data.DataTable GetRealTimeData(string STCD)  //**********
        {
            string name = "(SELECT STCD, ItemID, TM, DOWNDATE, NFOINDEX, DATAVALUE,CorrectionVALUE, DATATYPE FROM  YY_DATA_AUTO  where stcd='" + STCD + "' ORDER BY TM DESC limit 100) AS yda LEFT OUTER JOIN YY_RTU_Basic ON yda.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN YY_RTU_ITEM ON yda.ItemID = YY_RTU_ITEM.ItemID";
            string[] fields = new string[] { "yda.TM, YY_RTU_Basic.STCD,yda.CorrectionVALUE, YY_RTU_Basic.NiceName, YY_RTU_ITEM.ItemID, YY_RTU_ITEM.ItemName, YY_RTU_ITEM.ItemDecimal,YY_RTU_ITEM.PlusOrMinus, yda.DOWNDATE, yda.NFOINDEX, yda.DATAVALUE, yda.DATATYPE" };
            string where = "order by yda.TM DESC";
            return dt.Select(name, fields, where);

            //string name = "YY_DATA_AUTO RIGHT OUTER JOIN YY_RTU_Basic ON YY_DATA_AUTO.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN  YY_RTU_ITEM ON YY_DATA_AUTO.ItemID = YY_RTU_ITEM.ItemID";
            //string[] fields = new string[] { "  YY_RTU_Basic.STCD", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemID", "YY_RTU_ITEM.ItemName", "YY_DATA_AUTO.TM", "YY_DATA_AUTO.DOWNDATE", "YY_DATA_AUTO.NFOINDEX", "YY_DATA_AUTO.CorrectionVALUE", "YY_DATA_AUTO.DATATYPE", "YY_RTU_ITEM.ItemDecimal" };
            //string where = "where  YY_DATA_AUTO.CorrectionVALUE is not NULL and  YY_RTU_Basic.STCD='" + STCD + "'  order by YY_DATA_AUTO.TM desc";
            //return dt.Select(name, fields, where + " limit 100");
        }
        // <summary>
        /// 根据测站站号得到前100条数据YY_DATA_AUTO
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <param name="ItemIDs">监测项ID号</param>
        /// <returns></returns>
        public System.Data.DataTable GetRealTimeData(string STCD, string[] ItemIDs) //**********
        {
            string itemids = null;
            foreach (var item in ItemIDs)
            {
                itemids += "'" + item + "',";
            }
            if (itemids.Length > 1)
            {
                itemids = itemids.Substring(0, itemids.Length - 1);
            }

            string name = "(SELECT STCD, ItemID, TM, DOWNDATE, NFOINDEX, DATAVALUE,CorrectionVALUE, DATATYPE FROM  YY_DATA_AUTO  where stcd='" + STCD + "' and ItemID in(" + itemids + ") ORDER BY TM DESC limit 100) AS yda LEFT OUTER JOIN YY_RTU_Basic ON yda.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN YY_RTU_ITEM ON yda.ItemID = YY_RTU_ITEM.ItemID";
            string[] fields = new string[] { "yda.TM, YY_RTU_Basic.STCD,yda.CorrectionVALUE, YY_RTU_Basic.NiceName, YY_RTU_ITEM.ItemID, YY_RTU_ITEM.ItemName, YY_RTU_ITEM.ItemDecimal,YY_RTU_ITEM.PlusOrMinus, yda.DOWNDATE, yda.NFOINDEX, yda.DATAVALUE, yda.DATATYPE" };
            string where = "order by yda.TM DESC";
            return dt.Select(name, fields, where);

            //string name = "YY_DATA_AUTO RIGHT OUTER JOIN YY_RTU_Basic ON YY_DATA_AUTO.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN  YY_RTU_ITEM ON YY_DATA_AUTO.ItemID = YY_RTU_ITEM.ItemID";
            //string[] fields = new string[] { " YY_RTU_Basic.STCD", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemID", "YY_RTU_ITEM.ItemName", "YY_DATA_AUTO.TM", "YY_DATA_AUTO.DOWNDATE", "YY_DATA_AUTO.NFOINDEX", "YY_DATA_AUTO.CorrectionVALUE", "YY_DATA_AUTO.DATATYPE", "YY_RTU_ITEM.ItemDecimal" };
            //string where = "where YY_DATA_AUTO.CorrectionVALUE is not NULL and YY_RTU_Basic.STCD='" + STCD + "' and YY_DATA_AUTO.ItemID in (" + itemids + ")  order by YY_DATA_AUTO.TM desc";
            //return dt.Select(name, fields, where + " limit 100");
        }


        /// <summary>
        /// 得到各站的最新一条数据
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable GetRealTimeNewData()
        {
            //也可以尝试用以下语句联合生成试图 select a.* from YY_DATA_AUTO a where tm = (select max(TM) from YY_DATA_AUTO where STCD = a.STCD) order by tm,a.STCD

            //string name = "YY_DATA_AUTO AS t LEFT OUTER JOIN YY_RTU_ITEM ON t.ItemID = YY_RTU_ITEM.ItemID RIGHT OUTER JOIN  YY_RTU_Basic ON t.STCD = YY_RTU_Basic.STCD";
            //string[] fields = new string[] { "YY_RTU_Basic.STCD", "t.ItemID", "t.TM", "t.DOWNDATE", "t.NFOINDEX", "t.CorrectionVALUE", "t.DATATYPE", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemName", "YY_RTU_ITEM.ItemDecimal" };
            //string where = "WHERE (NOT EXISTS (SELECT     1 AS Expr1 FROM    YY_DATA_AUTO  WHERE      (STCD = t.STCD) AND (TM > t.TM)))";
            string name = "(SELECT   STCD, ItemID, MAX(TM) AS TM    FROM      YY_DATA_AUTO  GROUP BY STCD, ItemID) AS ttt LEFT OUTER JOIN" +
           " YY_DATA_AUTO AS YY_DATA_AUTO_1 ON ttt.STCD = YY_DATA_AUTO_1.STCD AND  ttt.ItemID = YY_DATA_AUTO_1.ItemID AND ttt.TM = YY_DATA_AUTO_1.TM" +
           " SELECT   YY_RTU_Basic.STCD, t.ItemID, t.TM, t.DOWNDATE, t.NFOINDEX,t.CorrectionVALUE, t.DATATYPE,YY_RTU_Basic.NiceName, YY_RTU_ITEM.ItemName, YY_RTU_ITEM.ItemDecimal" +
           " FROM #t AS t LEFT OUTER JOIN YY_RTU_ITEM ON t.ItemID = YY_RTU_ITEM.ItemID RIGHT OUTER JOIN YY_RTU_Basic ON t.STCD = YY_RTU_Basic.STCD";
            string[] fields = new string[] {"ttt.STCD, ttt.ItemID, ttt.TM, YY_DATA_AUTO_1.DOWNDATE, YY_DATA_AUTO_1.NFOINDEX, YY_DATA_AUTO_1.CorrectionVALUE, YY_DATA_AUTO_1.DATAVALUE, YY_DATA_AUTO_1.DATATYPE "+
            " into #t"};
            string where = " order by YY_RTU_Basic.STCD,t.ItemID  drop table #t";
            
            return dt.Select(name, fields, where);
        }

        /// <summary>
        /// 得到各站的最新一条数据YY_DATA_AUTO，指定监测项ID
        /// </summary>
        /// <param name="ItemIDs">监测项ID</param>
        /// <returns></returns>
        public System.Data.DataTable GetRealTimeNewData(string[] ItemIDs)
        {
            string itemids = null;
            foreach (var item in ItemIDs)
            {
                itemids += "'" + item + "',";
            }
            if (itemids.Length > 1)
            {
                itemids = itemids.Substring(0, itemids.Length - 1);
            }
            //string name = "YY_DATA_AUTO AS t LEFT OUTER JOIN YY_RTU_ITEM ON t.ItemID = YY_RTU_ITEM.ItemID RIGHT OUTER JOIN  YY_RTU_Basic ON t.STCD = YY_RTU_Basic.STCD";
            //string[] fields = new string[] { "YY_RTU_Basic.STCD", "t.ItemID", "t.TM", "t.DOWNDATE", "t.NFOINDEX", "t.CorrectionVALUE", "t.DATATYPE", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemName", "YY_RTU_ITEM.ItemDecimal" };
            //string where = "WHERE (NOT EXISTS (SELECT     1 AS Expr1 FROM    YY_DATA_AUTO  WHERE     (STCD = t.STCD) AND (TM > t.TM) and ItemID in (" + itemids + "))) and t.ItemID in (" + itemids + ")";
            string name = "(SELECT   STCD, ItemID, MAX(TM) AS TM    FROM      YY_DATA_AUTO  GROUP BY STCD, ItemID) AS ttt LEFT OUTER JOIN" +
           " YY_DATA_AUTO AS YY_DATA_AUTO_1 ON ttt.STCD = YY_DATA_AUTO_1.STCD AND  ttt.ItemID = YY_DATA_AUTO_1.ItemID AND ttt.TM = YY_DATA_AUTO_1.TM" +
           " SELECT   YY_RTU_Basic.STCD, t.ItemID, t.TM, t.DOWNDATE,t.STTYPE ,t.NFOINDEX,t.CorrectionVALUE, t.DATATYPE,t.STTYPE,YY_RTU_Basic.NiceName, YY_RTU_ITEM.ItemName, YY_RTU_ITEM.ItemDecimal" +
           " FROM #t AS t LEFT OUTER JOIN YY_RTU_ITEM ON t.ItemID = YY_RTU_ITEM.ItemID RIGHT OUTER JOIN YY_RTU_Basic ON t.STCD = YY_RTU_Basic.STCD";
            string[] fields = new string[] {"ttt.STCD, ttt.ItemID, ttt.TM,YY_DATA_AUTO_1.STTYPE, YY_DATA_AUTO_1.DOWNDATE, YY_DATA_AUTO_1.NFOINDEX, YY_DATA_AUTO_1.CorrectionVALUE, YY_DATA_AUTO_1.DATAVALUE, YY_DATA_AUTO_1.DATATYPE, YY_DATA_AUTO_1.STTYPE "+
            " into #t"};
            string where = " where  #t.ItemID in (" + itemids + ") order by YY_RTU_Basic.STCD,t.ItemID  drop table #t";
            return dt.Select(name, fields, where);
        }

        public System.Data.DataTable GetRealTimeNewData(string[] ItemIDs,string STTYPE)
        {
            string itemids = null;
            foreach (var item in ItemIDs)
            {
                itemids += "'" + item + "',";
            }
            if (itemids.Length > 1)
            {
                itemids = itemids.Substring(0, itemids.Length - 1);
            }
            //string name = "YY_DATA_AUTO AS t LEFT OUTER JOIN YY_RTU_ITEM ON t.ItemID = YY_RTU_ITEM.ItemID RIGHT OUTER JOIN  YY_RTU_Basic ON t.STCD = YY_RTU_Basic.STCD";
            //string[] fields = new string[] { "YY_RTU_Basic.STCD", "t.ItemID", "t.TM", "t.DOWNDATE", "t.NFOINDEX", "t.CorrectionVALUE", "t.DATATYPE", "YY_RTU_Basic.NiceName", "YY_RTU_ITEM.ItemName", "YY_RTU_ITEM.ItemDecimal" };
            //string where = "WHERE (NOT EXISTS (SELECT     1 AS Expr1 FROM    YY_DATA_AUTO  WHERE     (STCD = t.STCD) AND (TM > t.TM) and ItemID in (" + itemids + "))) and t.ItemID in (" + itemids + ")";
            string name = "(SELECT   STCD, ItemID, MAX(TM) AS TM    FROM      YY_DATA_AUTO  GROUP BY STCD, ItemID) AS ttt LEFT OUTER JOIN" +
           " YY_DATA_AUTO AS YY_DATA_AUTO_1 ON ttt.STCD = YY_DATA_AUTO_1.STCD AND  ttt.ItemID = YY_DATA_AUTO_1.ItemID AND ttt.TM = YY_DATA_AUTO_1.TM" +
           " SELECT   YY_RTU_Basic.STCD, t.ItemID, t.TM, t.DOWNDATE,t.STTYPE ,t.NFOINDEX,t.CorrectionVALUE, t.DATATYPE,YY_RTU_Basic.NiceName, YY_RTU_ITEM.ItemName, YY_RTU_ITEM.ItemDecimal" +
           " FROM #t AS t LEFT OUTER JOIN YY_RTU_ITEM ON t.ItemID = YY_RTU_ITEM.ItemID RIGHT OUTER JOIN YY_RTU_Basic ON t.STCD = YY_RTU_Basic.STCD";
            string[] fields = new string[] {"ttt.STCD, ttt.ItemID, ttt.TM, YY_DATA_AUTO_1.DOWNDATE, YY_DATA_AUTO_1.NFOINDEX, YY_DATA_AUTO_1.CorrectionVALUE, YY_DATA_AUTO_1.DATAVALUE, YY_DATA_AUTO_1.DATATYPE, YY_DATA_AUTO_1.STTYPE "+
            " into #t"};
            string where = " where  #t.ItemID in (" + itemids + ") and  #t.STTYPE=" + STTYPE + " order by YY_RTU_Basic.STCD,t.ItemID  drop table #t";
            return dt.Select(name, fields, where);
        }


        /// <summary>
        /// 查询召测结果状态列表YY_DATA_COMMAND
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public System.Data.DataTable GetCommandState(string Where) 
        {
            string name = "YY_DATA_COMMAND LEFT OUTER JOIN YY_RTU_Basic ON YY_DATA_COMMAND.STCD = YY_RTU_Basic.STCD LEFT OUTER JOIN YY_RTU_COMMAND ON YY_DATA_COMMAND.CommandID = YY_RTU_COMMAND.CommandID";
            string[] fields = new string[]{"  YY_DATA_COMMAND.*", "YY_RTU_COMMAND.Remark", "YY_RTU_Basic.NiceName"};
            return dt.Select(name, fields, Where + " limit 100");
        }
        #endregion


        #region 设备状态查询相关方法
        /// <summary>
        /// 根据测站站号得到前100条状态数据
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <returns></returns>
        public System.Data.DataTable GetRTUState(string STCD)
        {
            string name = "YY_DATA_STATE RIGHT OUTER JOIN YY_RTU_Basic ON YY_DATA_STATE.STCD = YY_RTU_Basic.STCD ";
            string[] fields = new string[] { " YY_RTU_Basic.STCD", "YY_RTU_Basic.NiceName", "YY_DATA_STATE.TM", "YY_DATA_STATE.DOWNDATE", "YY_DATA_STATE.NFOINDEX", "YY_DATA_STATE.STATEDATA" };
            string where = "where YY_RTU_Basic.STCD='" + STCD + "' ORDER BY YY_DATA_STATE.TM DESC";
            return dt.Select(name, fields, where + " limit 100");
        }

        /// <summary>
        /// 得到各测站最新一条状态数据
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable GetRTUNewState()
        {
            string name = "YY_DATA_STATE AS t right OUTER JOIN YY_RTU_Basic ON t.STCD = YY_RTU_Basic.STCD";
            string[] fields = new string[] { "t.TM", "t.DOWNDATE", "t.NFOINDEX", "t.STATEDATA", "YY_RTU_Basic.NiceName", "YY_RTU_Basic.STCD" };
            string where = "WHERE     (NOT EXISTS (SELECT     1 AS Expr1  FROM  YY_DATA_STATE WHERE      (STCD = t.STCD) AND (TM > t.TM)))";
            return dt.Select(name, fields, where);
        }

        /// <summary>
        /// 创建符合格式的状态列表
        /// </summary>
        /// <param name="datatable">终端状态数据表</param>
        /// <returns></returns>
        public System.Data.DataTable CreateRTUStateDataTable(System.Data.DataTable datatable)
        {
            IList<YY_STATE> list = dt.Select<YY_STATE>("YY_STATE", new string[] { "*" }, "order by STATEID asc");

            System.Data.DataTable DT = new System.Data.DataTable();
            DT.Columns.Add("站号");
            DT.Columns.Add("站名");
            DT.Columns.Add("接收时间");
            DT.Columns.Add("采集时间");
            DT.Columns.Add("信道");
            if(list !=null)
            for (int i = 0; i < list.Count; i++)
            {
                DT.Columns.Add(list[i].RTUSTATE);
            }
            if (datatable != null)
            for (int i = 0; i < datatable.Rows.Count; i++)
            {
                System.Data.DataRow dr = DT.NewRow();
                dr["站号"] = datatable.Rows[i]["STCD"].ToString();
                dr["站名"] = datatable.Rows[i]["NiceName"].ToString();
                dr["接收时间"] = datatable.Rows[i]["TM"].ToString();
                dr["采集时间"] = datatable.Rows[i]["DOWNDATE"].ToString();
                dr["信道"] = datatable.Rows[i]["NFOINDEX"].ToString();

                string statedata = datatable.Rows[i]["STATEDATA"].ToString();
                for (int j = 0; j < statedata.Length; j++)
                {
                    dr[5 + j] = statedata[j].ToString();
                }
                DT.Rows.Add(dr);
            }

            return DT;

        }
        #endregion



        #region 中心表相关方法
        /// <summary>
        /// 得到所有中心站的
        /// </summary>
        /// <returns></returns>
        public IList<CENTER_SERVER> GetServers()
        {
            return dt.Select<CENTER_SERVER>("center_server", new string[] { "*" }, " order by desc DTime");
        }

        /// <summary>
        /// 根据条件获得各中心站服务的启动状态
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<CENTER_STARTSTATE> GetStartState(string Where)
        {
            return dt.Select<CENTER_STARTSTATE>("CENTER_STARTSTATE", new string[] { "*" }, Where);
        }

        /// <summary>
        /// 更新服务列表状态
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool UpdCENTER_SERVER(CENTER_SERVER model, string Where)
        {
            return dt.Update<CENTER_SERVER>("CENTER_SERVER", model, Where);
        }

        /// <summary>
        /// 添加新服务到服务列表
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddCENTER_SERVER(CENTER_SERVER model)
        {
            return dt.Insert<CENTER_SERVER>("CENTER_SERVER", model);
        }

        /// <summary>
        /// 得到启动状态表中各服务最新的数据
        /// </summary>
        /// <returns></returns>
        public IList<CENTER_STARTSTATE> GetStartStateServers()
        {
            string name = " Center_StartState";
            string[] fields = new string[] { "*" };
            string Where = "where exists (" +
                         "select 1 from " +
                         "(select ProjectName,PublicIP , max(DTime) as DTime from Center_StartState  group by ProjectName,PublicIP) x " +
                         "where x.ProjectName = Center_StartState.ProjectName  and x.PublicIP =Center_StartState.PublicIP and x.DTime = Center_StartState.DTime" +
                         ")";

            return dt.Select<CENTER_STARTSTATE>(name, fields, Where);
        }

        /// <summary>
        /// 添加服务启动信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddCENTER_STARTSTATE(CENTER_STARTSTATE model)
        {
            return dt.Insert<CENTER_STARTSTATE>("CENTER_STARTSTATE", model);
        }

        /// <summary>
        /// 更新服务启动信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool UdpCENTER_STARTSTATE(CENTER_STARTSTATE model, string Where)
        {
            return dt.Update<CENTER_STARTSTATE>("CENTER_STARTSTATE", model, Where);
        }

        /// <summary>
        /// 添加RTU数量变化信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
        public bool AddCENTER_RTUCHANGE(CENTER_RTUCHANGE model)
        {
            return dt.Insert<CENTER_RTUCHANGE>("CENTER_RTUCHANGE", model);
        }

        /// <summary>
        /// 得到所有RTU数量变化信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public IList<CENTER_RTUCHANGE> GetCenterRTUChange(string Where)
        {
            return dt.Select<CENTER_RTUCHANGE>("CENTER_RTUCHANGE", new string[] { "*" }, Where);
        }

        /// <summary>
        /// 删除RTU数量变化信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelCENTER_RTUCHANGE(string Where)
        {
            return dt.Delete("delete from CENTER_RTUCHANGE " + Where);
        }

        /// <summary>
        /// 删除服务启动信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelCENTER_STARTSTATE(string Where)
        {
            return dt.Delete("delete from CENTER_STARTSTATE " + Where);
        }

        /// <summary>
        /// 删除服务
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        public bool DelCENTER_SERVER(string Where)
        {
            return dt.Delete("delete from CENTER_SERVER " + Where);
        }
        #endregion

    }
}
