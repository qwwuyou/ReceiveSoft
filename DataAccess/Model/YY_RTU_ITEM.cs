using System;
namespace Service.Model
{
	/// <summary>
	/// YY_RTU_ITEM:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
    public partial class YY_RTU_ITEM
    {
        public YY_RTU_ITEM()
        { }
        #region Model
        private string _itemid;
        private string _itemname;
        private string _itemcode;
        private int _iteminteger;
        private int _itemdecimal;
        private bool _plusorminus;
        private string _units;
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
        public string ItemName
        {
            set { _itemname = value; }
            get { return _itemname; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ItemCode
        {
            set { _itemcode = value; }
            get { return _itemcode; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int ItemInteger
        {
            set { _iteminteger = value; }
            get { return _iteminteger; }
        } 
        /// <summary>
        /// 
        /// </summary>
        public int ItemDecimal
        {
            set { _itemdecimal = value; }
            get { return _itemdecimal; }
        } 
        /// <summary>
        /// 
        /// </summary>
        public bool PlusOrMinus
        {
            set { _plusorminus = value; }
            get { return _plusorminus; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string Units
        {
            set { _units = value; }
            get { return _units; }
        }
        #endregion Model

    }
}

