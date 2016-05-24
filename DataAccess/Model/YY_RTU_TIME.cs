using System;
namespace Service.Model
{
	/// <summary>
	/// YY_RTU_TIME:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_RTU_TIME
	{
		public YY_RTU_TIME()
		{}
        #region Model
        private string _stcd;
        private string _itemid;
        private bool _isautoneedquery;
        private int? _interval;
        private decimal? _sillvalue;
        private string _commandtype;
        /// <summary>
        /// 
        /// </summary>
        public string STCD
        {
            set { _stcd = value; }
            get { return _stcd; }
        }
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
        public bool IsAutoNeedQuery
        {
            set { _isautoneedquery = value; }
            get { return _isautoneedquery; }
        }
        /// <summary>
        /// 
        /// </summary>
        public int? Interval
        {
            set { _interval = value; }
            get { return _interval; }
        }
        /// <summary>
        /// 
        /// </summary>
        public decimal? SillValue
        {
            set { _sillvalue = value; }
            get { return _sillvalue; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string CommandType
        {
            set { _commandtype = value; }
            get { return _commandtype; }
        }
        #endregion Model

	}
}

