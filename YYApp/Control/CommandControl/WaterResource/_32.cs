﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service;

namespace YYApp.CommandControl
{
    public partial class _32 : UserControl, ICommandControl
    {
        public _32(string[] Stcds)
        {
            InitializeComponent();
            this.Dock = DockStyle.Fill;

            Init(Stcds);
        }

        public void Init(string[] Stcds)
        {
            //string Where = "where stcd='"+Stcds[0]+"'";
            //IList<Service.Model.YY_RTU_WORK> WORKList = PublicBD.db.GetRTU_WORKList(Where);
             IList<Service.Model.YY_RTU_CONFIGDATA> CONFIGDATAList = PublicBD.db.GetRTU_CONFIGDATAList("where STCD='" + Stcds[0] + "' and ItemID='0000000000' and ConfigID = '120000000032'");
             if (CONFIGDATAList.Count > 0)
             {
                 int FixValueStatus = 0;
                 if (int.TryParse(CONFIGDATAList.First().ConfigVal, out FixValueStatus))
                 {
                     if (FixValueStatus == 1) 
                     { cb1.Checked = true; }
                 }
             }
        }

        public string[] GetCommand(string[] Stcds, string NFOINDEX, out string CommandCode)
        {
            YanYu.WRIMR.Protocol.Pack P = new YanYu.WRIMR.Protocol.Pack();
            string sjy ="";
           
            int gnm = 0;
            if (cb1.Checked)
            { gnm = 0x32; CommandCode = "32"; }
            else
            { gnm = 0x33; CommandCode = "33"; }
            string[] commands = new string[Stcds.Length];
            for (int i = 0; i < Stcds.Length; i++)
            {
                var RTU = from rtu in ExecRTUList.Lrdm where rtu.STCD == Stcds[i] select rtu;

                byte[] b = P.pack(Stcds[i], 0, 0, gnm, sjy, int.Parse(RTU.First().PWD));

                commands[i] = YanYu.WRIMR.Protocol.PackageHelper.ByteToHexStr(b);
            }

            return commands;
        }
    }
}
