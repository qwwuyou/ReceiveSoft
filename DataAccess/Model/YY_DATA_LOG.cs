using System;
namespace Service.Model
{
	/// <summary>
	/// YY_DATA_LOG:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_DATA_LOG
	{
		public YY_DATA_LOG()
		{}
		#region Model
		private string _stcd;
		private DateTime _tm;
		private int? _logid;
		private DateTime? _downdate;
		private int? _nfoindex;
		private int? _count;
		private int? _erc;
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
		public int? LOGID
		{
			set{ _logid=value;}
			get{return _logid;}
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
		public int? COUNT
		{
			set{ _count=value;}
			get{return _count;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ERC
		{
			set{ _erc=value;}
			get{return _erc;}
		}
		#endregion Model

	}
}

