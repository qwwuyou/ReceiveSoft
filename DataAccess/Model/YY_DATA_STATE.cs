using System;
namespace Service.Model
{
	/// <summary>
	/// YY_DATA_STATE:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_DATA_STATE
	{
		public YY_DATA_STATE()
		{}
		#region Model
		private string _stcd;
		private DateTime _tm;
		private DateTime? _downdate;
		private int? _nfoindex;
        private string _statedata;
        private decimal? _datavalue;
		/// <summary>
		/// 
		/// </summary>
		public string STCD
		{
			set{ _stcd=value;}
			get{return _stcd;}
		}
		
		/// <summary>
		/// 
		/// </summary>
		public DateTime TM
		{
			set{ _tm=value;}
			get{return _tm;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? DOWNDATE
		{
			set{ _downdate=value;}
			get{return _downdate;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? NFOINDEX
		{
			set{ _nfoindex=value;}
			get{return _nfoindex;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string STATEDATA
		{
			set{ _statedata=value;}
            get { return _statedata; }
		}
        /// <summary>
        /// 
        /// </summary>
        public decimal? DATAVALUE
        {
            set { _datavalue = value; }
            get { return _datavalue; }
        }
		#endregion Model

	}
}

