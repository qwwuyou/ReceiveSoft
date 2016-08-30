using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace CenterApp
{
     static  class DataAccess
    {
        public static _51Data dt= new _51Data(WriteReadXML.Source, WriteReadXML.DataBase, WriteReadXML.UserName, WriteReadXML.PassWord);
        public static void   ReInit() 
        {
            dt = new _51Data(WriteReadXML.Source, WriteReadXML.DataBase, WriteReadXML.UserName, WriteReadXML.PassWord);
        }



        public static bool ConnectDBstate = false;
        public static string LogIn(string username,string password)
        {
            dt.Open();
            DataTable DT= dt.Select("ECO_User", new string[] { "User_Id" }, " where User_Name='" + username + "' and UserPwd='" + password + "'");
            if (DT != null && DT.Rows.Count > 0) 
            {
                return DT.Rows[0][0].ToString();
            }
            return "";
        }
    }

    
}