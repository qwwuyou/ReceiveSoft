using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Service.Model;
using System.Data;

namespace Service
{
    public interface DBBussiness
    {
        object dt
        {
            get;
        }

        #region 测站信息操作
        /// <summary>
        /// 得到RTU列表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         IList<YY_RTU_Basic> GetRTUList(string Where);
       

        /// <summary>
        /// 添加测站信息
        /// </summary>
        /// <param name="model">测站实体</param>
        /// <returns></returns>
         bool AddRTU(YY_RTU_Basic model);

        /// <summary>
        /// 更新RTU信息
        /// </summary>
        /// <param name="model">测站实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool UpdRTU(YY_RTU_Basic model, string Where);

        /// <summary>
        /// 删除RTU信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelRTU(string Where);
        #endregion

        #region 监测项操作
        /// <summary>
        /// 得到监测项列表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         IList<YY_RTU_ITEM> GetItemList(string Where);

        /// <summary>
        /// 更新监测项信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool UdpItem(YY_RTU_ITEM model, string Where);

        /// <summary>
        /// 添加监测项信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
         bool AddItem(YY_RTU_ITEM model);

        /// <summary>
        /// 删除监测项信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelItem(string Where);
        #endregion

        #region 操作中心站信息配置表
        /// <summary>
        /// 更新中心站信息配置
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool UpdRTU_WRES(YY_RTU_WRES model, string Where);

        /// <summary>
        /// 添加中心站信息配置
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
         bool AddRTU_WRES(YY_RTU_WRES model);

        /// <summary>
        /// 根据条件得到中心站信息配置表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         IList<YY_RTU_WRES> GetRTU_WRESList(string Where);

        /// <summary>
        /// 删除中心站信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelRTU_WRES(string Where);
        #endregion

        #region 操作测站工作状态信息表YY_RTU_WORK
        /// <summary>
        /// 查询测站工作状态信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         IList<YY_RTU_WORK> GetRTU_WORKList(string Where);
        /// <summary>
        /// 添加测站工作状态信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
         bool AddRTU_Work(YY_RTU_WORK model);

        /// <summary>
        /// 更新测站工作状态信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool UdpRTU_Work(YY_RTU_WORK model, string Where);

        /// <summary>
        /// 删除测站工作状态信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelRTU_Work(string Where);
        #endregion

        #region 操作测站监测项配置信息表
        /// <summary>
        /// 得到配置项数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         DataTable GetRTU_CONFIGDATA(string Where);

        /// <summary>
        /// 得到配置项数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         IList<YY_RTU_CONFIGDATA> GetRTU_CONFIGDATAList(string Where);

        /// <summary>
        /// 删除测站监测项配置信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelRTU_ConfigData(string Where);

        /// <summary>
        /// 添加测站监测项配置信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
         bool AddRTU_ConfigData(YY_RTU_CONFIGDATA model);
        #endregion

        #region 操作测站自报固态属性表YY_RTU_TIME
        /// <summary>
        /// 查询测站自报固态属性信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         IList<YY_RTU_TIME> GetRTU_TIMEList(string Where);

        /// <summary>
        /// 更新测站自报固态属性信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool UpdRTU_TIME(YY_RTU_TIME model, string Where);
        /// <summary>
        /// 添加测站自报固态属性信息
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
         bool AddRTU_TIME(YY_RTU_TIME model);

        /// <summary>
        /// 删除测站自报固态属性信息
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelRTU_TIME(string Where);
        #endregion

        #region 配置监测项与配置项关系（工具功能）YY_RTU_ITEMCONFIG
        /// <summary>
        /// 根据条件得到监测项与配置项的关系
        /// </summary>
         /// <param name="where">条件</param>
        /// <returns></returns>
         IList<YY_RTU_ITEMCONFIG> GetRTU_ItemConfig(string where);

        /// <summary>
        /// 添加监测项与配置项的关系
        /// </summary>
        /// <param name="model">关系实体</param>
        /// <returns></returns>
         bool AddRTU_ItemConfig(YY_RTU_ITEMCONFIG model);

        /// <summary>
         ///  根据条件删除监测项与配置项的关系
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
         bool DelRTU_ItemConfig(string where);
         #endregion

        /// <summary>
        /// 根据站号得到关联的配置项
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <returns></returns>
        DataTable GetRTU_ConfigItem(string STCD);

         /// <summary>
        /// 得到监测项配置项列表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         IList<YY_RTU_CONFIGITEM> GetRTU_ConfigItemList(string Where);

        /// <summary>
        /// 得到测站和监测项关系列表
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         IList<YY_RTU_BI> GetRTU_BIList(string Where);

        /// <summary>
        /// 得到RTU召测列表
        /// </summary>
        /// <returns></returns>
         IList<YY_RTU_COMMAND> GetRTUCommandList();


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
         bool AddRealTimeData(string STCD, string ItemID, DateTime TM, DateTime RTM, int NFOINDEX, decimal? Value);

        /// <summary>
        /// 添加实时数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
         bool AddRealTimeData(YY_DATA_AUTO model);

        /// <summary>
        /// 更新实时数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool UdpRealTimeData(YY_DATA_AUTO model, string Where);

        /// <summary>
        /// 删除实时数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelRealTimeData(string Where);
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
         bool AddRemData(string STCD, string ItemID, DateTime TM, DateTime RTM, int NFOINDEX, decimal? Value);

        /// <summary>
        /// 添加固态数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
         bool AddRemData(YY_DATA_REM model);

        /// <summary>
        /// 更新固态数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool UdpRemData(YY_DATA_REM model, string Where);

        /// <summary>
        /// 删除固态数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelRemData(string Where);
        #endregion


        #region 操作事件记录表YY_DATA_LOG
         /// <summary>
         /// 添加事件记录
         /// </summary>
         /// <param name="model">实体</param>
         /// <returns></returns>
         bool AddDataLog(YY_DATA_LOG model);
        
        /// <summary>
        /// 删除事件记录
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
        bool DelDataLog(string Where);
        #endregion

        #region 得到事件描述
        /// <summary>
        /// 得到事件描述
        /// </summary>
        /// <returns></returns>
        IList<YY_LOG> GetLog();
        #endregion

        #region 操作命令临时表YY_COMMAND_TEMP
        /// <summary>
        /// 得到命令临时表中的记录
        /// </summary>
        /// <returns></returns>
         IList<YY_COMMAND_TEMP> GetCommandTempList();

        /// <summary>
        /// 添加命令状态临时数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
         bool AddCommandTemp(YY_COMMAND_TEMP model);

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
         bool AddCommandTemp(string STCD, int NFOINDEX, string CommandID, string Data, DateTime TM, int State);

        /// <summary>
        /// 删除命令状态临时数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelCommandTemp(string Where);

        /// <summary>
        /// 删除命令状态临时数据
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="NFOINDEX">信道</param>
        /// <param name="CommandID">命令码</param>
        /// <returns></returns>
         bool DelCommandTemp(string STCD, int NFOINDEX, string CommandID);
        #endregion


        #region 人工置数表
        /// <summary>
        /// 添加人工置数数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns></returns>
         bool AddManualData(YY_DATA_MANUAL model);

        /// <summary>
        /// 更新人工置数数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool UdpManualData(YY_DATA_MANUAL model, string Where);

        /// <summary>
        /// 删除人工置数数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelManualData(string Where);

        /// <summary>
        /// 得到实时前100条数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         System.Data.DataTable GetManualDataForWhere(string Where);
        #endregion


         #region 操作图片
         /// <summary>
         /// 添加图片
         /// </summary>
         /// <param name="model">实体</param>
         /// <returns></returns>
         bool AddImg(YY_DATA_IMG model);

         /// <summary>
         /// 删除图片
         /// </summary>
         /// <param name="Where">条件</param>
         /// <returns></returns>
         bool DelImg(string Where);

         /// <summary>
         /// 得到前100幅图片
         /// </summary>
         /// <param name="Where">条件</param>
         /// <returns></returns>
         System.Data.DataTable GetImgForWhere(string Where);

         /// <summary>
         /// 根据站号时间得到图片
         /// </summary>
         /// <param name="STCD">站号</param>
         /// <param name="TM">时间</param>
         /// <returns></returns>
         IList<YY_DATA_IMG> GetImg(string STCD, DateTime TM);
         #endregion
         
        
        /// <summary>
        /// 添加终端状态和报警状态
        /// </summary>
        /// <param name="STCD">站号</param>
        /// <param name="TM">监测时间</param>
        /// <param name="RTM">接收时间</param>
        /// <param name="NFOINDEX">信道类型</param>
        /// <param name="Alarms">报警数据</param>
        /// <returns></returns>
         bool AddRTUState(string STCD, DateTime TM, DateTime RTM, int NFOINDEX, string AlarmsStr);

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
         bool AddDataCommand(string STCD, string CommandID, DateTime TM, DateTime? DOWNDATE, string Command, int NFOINDEX, int STATE);

        /// <summary>
        /// 删除测站与监测项关系
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         bool DelRTU_Item(string Where);

        /// <summary>
        /// 添加测站与监测项关系
        /// </summary>
        /// <param name="model">数据实体</param>
        /// <returns></returns>
         bool AddRTU_Item(YY_RTU_BI model);

        #region 得到数据
        /// <summary>
        /// 得到实时前100条数据
        /// </summary>
        /// <returns></returns>
         System.Data.DataTable GetRealTimeData();
        /// <summary>
        /// 得到实时前100条数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         System.Data.DataTable GetRealTimeDataForWhere(string Where);

        /// <summary>
        /// 得到符合条件的记录数量(实时数据)
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         int GetRealTimeDataCount(string Where);

         /// <summary>
         /// 得到符合条件的记录数量(固态数据)
         /// </summary>
         /// <param name="Where">条件</param>
         /// <returns></returns>
         int GetRemDataCount(string Where);

        /// <summary>
        /// 得到固态前100条数据
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         System.Data.DataTable GetRemDataForWhere(string Where);
        // <summary>
        /// 根据测站站号得到前100条数据
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <returns></returns>
         System.Data.DataTable GetRealTimeData(string STCD);

        // <summary>
        /// 根据测站站号得到前100条数据YY_DATA_AUTO
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <param name="ItemIDs">监测项ID号</param>
        /// <returns></returns>
         System.Data.DataTable GetRealTimeData(string STCD, string[] ItemIDs);

        /// <summary>
        /// 得到各站的最新一条数据
        /// </summary>
        /// <returns></returns>
         System.Data.DataTable GetRealTimeNewData();

        /// <summary>
        /// 得到各站的最新一条数据YY_DATA_AUTO，指定监测项ID
        /// </summary>
        /// <param name="ItemIDs">监测项ID</param>
        /// <returns></returns>
         System.Data.DataTable GetRealTimeNewData(string[] ItemIDs);

         /// <summary>
         /// 得到各站的最新一条数据YY_DATA_AUTO，指定监测项ID
         /// </summary>
         /// <param name="ItemIDs">监测项ID</param>
         /// <returns></returns>
         System.Data.DataTable GetRealTimeNewData(string[] ItemIDs,string STTYPE);

        /// <summary>
        /// 查询召测结果状态列表YY_DATA_COMMAND
        /// </summary>
        /// <param name="Where">条件</param>
        /// <returns></returns>
         System.Data.DataTable GetCommandState(string Where);
        #endregion


        #region 设备状态查询相关方法
        /// <summary>
        /// 根据测站站号得到前100条状态数据
        /// </summary>
        /// <param name="STCD">测站站号</param>
        /// <returns></returns>
         System.Data.DataTable GetRTUState(string STCD);

        /// <summary>
        /// 得到各测站最新一条状态数据
        /// </summary>
        /// <returns></returns>
         System.Data.DataTable GetRTUNewState();

        /// <summary>
        /// 创建符合格式的状态列表
        /// </summary>
        /// <param name="datatable">终端状态数据表</param>
        /// <returns></returns>
         System.Data.DataTable CreateRTUStateDataTable(System.Data.DataTable datatable);
        #endregion
    }
}
