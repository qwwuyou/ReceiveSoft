using System;
namespace Service.Model
{
    /// <summary>
    /// YY_RTU_ITEMCONFIG:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [Serializable]
    public partial class YY_RTU_ITEMCONFIG
    {
        public YY_RTU_ITEMCONFIG()
        { }
        #region Model
        private string _itemid;
        private string _configid;
        /// <summary>
        /// 
        /// </summary>
        public string ItemID
        {
            set { _itemid = value; }
            get { return _itemid; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ConfigID
        {
            set { _configid = value; }
            get { return _configid; }
        }
        #endregion Model

    }
}
