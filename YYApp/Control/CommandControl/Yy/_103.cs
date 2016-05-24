using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service.Model;
using Service;

namespace YYApp.CommandControl
{
    public partial class _103 : UserControl, ICommandControl
    {
        public _103()
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList("").ToList<YY_RTU_CONFIGDATA>();
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YYPack.Pack pack = new YYPack.Pack();
            string[] commands = null;

            CommandCode = "0067";
            commands = new string[Stcds.Length];
            for (int i = 0; i < Stcds.Length; i++)
            {
                if (NFOINDEX.ToLower() != "短信") //短信
                {
                    //打召测包
                    commands[i] = pack.GetAnswer("0001", CommandCode, GetAuto(Stcds[i]), GetWaterLevelOrder(Stcds[i]), GetRainfallOrder(Stcds[i]), GetPowerMode(Stcds[i]), GetCenterAddress(Stcds[i]), "FFFF", "FFFF", "FFFF");
                }
                else
                {
                    if(radTX_A.Checked)
                    commands[i] = pack.GetAnswer("0001", CommandCode, GetAuto(Stcds[i]), GetWaterLevelOrder(Stcds[i]), GetRainfallOrder(Stcds[i]), GetPowerMode(Stcds[i]), GetCenterAddress(Stcds[i]), "FFFF", "FFFF", "FFFF","gsm");
                    else
                        commands[i] = pack.GetAnswer("0001", CommandCode, GetAuto(Stcds[i]), GetWaterLevelOrder(Stcds[i]), GetRainfallOrder(Stcds[i]), GetPowerMode(Stcds[i]), GetCenterAddress(Stcds[i]), "FFFF", "FFFF", "FFFF", "gprs"); 
                }
            }

            return commands;
        }

        #region  从数据库里得到回复用的数据
        
        public List<YY_RTU_CONFIGDATA> CONFIGDATAList = null;
        /// <summary>
        /// 得到设备自报间隔D7  (默认值1 =0001)
        /// </summary>
        /// <param name="STCD"></param>
        private string GetAuto(string STCD)
        {
            var rtu = from r in CONFIGDATAList where r.STCD == STCD && r.ConfigID == "100000000007" select r;
            if (rtu.Count() > 0)
            {
                return Convert.ToInt32(rtu.First().ConfigVal,16).ToString().PadLeft(4, '0');
            }

            return "0001";
        }

        /// <summary>
        /// 得到雨量量级D10（取12、180、181中第一个保存的量级值，否则返回默认值1 =0001）
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        private string GetRainfallOrder(string STCD)
        {
            if (CONFIGDATAList != null && CONFIGDATAList.Count() > 0)
            {
                var config = from c in CONFIGDATAList where c.STCD == STCD && (c.ItemID == "12" || c.ItemID == "180" || c.ItemID == "181" || c.ConfigID == "10") && c.ConfigVal != null && c.ConfigVal != "" select c;
                if (config.Count() > 0)
                {
                    int Val = 0;
                    if (int.TryParse(config.First().ConfigVal, out Val))
                    {
                        return Convert.ToInt32(Val.ToString(),16).ToString().PadLeft(4, '0');
                    }
                }
            }

            return "0001";
        }

        /// <summary>
        /// 得到水位量级D8（取15、16、182~185中第一个保存的量级值，否则返回默认值1 =0001）
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        private string GetWaterLevelOrder(string STCD)
        {
            if (CONFIGDATAList != null && CONFIGDATAList.Count() > 0)
            {
                var config = from c in CONFIGDATAList where c.STCD == STCD && (c.ItemID == "15" || c.ItemID == "16" || c.ItemID == "182" && c.ConfigID == "183" && c.ConfigID == "184" && c.ConfigID == "185") && c.ConfigVal != null && c.ConfigVal != "" select c;
                if (config.Count() > 0)
                {
                    int Val = 0;
                    if (int.TryParse(config.First().ConfigVal, out Val))
                    {
                        return Convert.ToInt32(Val.ToString(),16).ToString().PadLeft(4, '0');
                    }
                }
            }

            return "0001";
        }

        /// <summary>
        /// 得到电源供电模式D11（默认值0 =0000自动掉电  1=0001保持长供电）
        /// </summary>
        /// <param name="STCD"></param>
        /// <returns></returns>
        private string GetPowerMode(string STCD)
        {
            var rtu = from r in CONFIGDATAList where r.STCD == STCD && r.ConfigID == "100000000011" select r;
            if (rtu.Count() > 0)
            {
                return Convert.ToInt32(rtu.First().ConfigVal,16).ToString().PadLeft(4, '0');
            }
            return "0000";
        }

        /// <summary>
        ///得到能够远程提取历史数据的中心站号D13(默认值1 =0001)
        /// </summary>
        /// <param name="STCD"></param>
        ///// <returns></returns>
        private string GetCenterAddress(string STCD)
        {
            var rtu = from r in CONFIGDATAList where r.STCD == STCD && r.ConfigID == "100000000013" select r;
            if (rtu.Count() > 0)
            {
                return Convert.ToInt32(rtu.First().ConfigVal,16).ToString().PadLeft(4, '0');
            }
            
            return "0001";

        }
        #endregion

       
    }
}
