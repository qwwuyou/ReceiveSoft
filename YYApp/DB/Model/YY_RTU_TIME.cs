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
		private bool _isauto;
		private int? _auto;
		private bool _needquery;
		private int? _rem;
		private decimal? _sillvalue;
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
		public string ItemID
		{
			set{ _itemid=value;}
			get{return _itemid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool IsAuto
		{
			set{ _isauto=value;}
			get{return _isauto;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Auto
		{
			set{ _auto=value;}
			get{return _auto;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool NeedQuery
		{
			set{ _needquery=value;}
			get{return _needquery;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? Rem
		{
			set{ _rem=value;}
			get{return _rem;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? SillValue
		{
			set{ _sillvalue=value;}
			get{return _sillvalue;}
		}
		#endregion Model

	}
}

