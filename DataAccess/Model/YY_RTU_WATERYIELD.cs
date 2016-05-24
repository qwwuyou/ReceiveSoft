using System;
namespace Service.Model
{
	/// <summary>
	/// YY_RTU_WATERYIELD:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_RTU_WATERYIELD
	{
		public YY_RTU_WATERYIELD()
		{}
		#region Model
		private string _stcd;
		private string _itemid;
		private decimal _stream;
		private decimal? _str_pad;
		private DateTime? _tm;
		private int? _type;
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
		public decimal STREAM
		{
			set{ _stream=value;}
			get{return _stream;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? STR_PAD
		{
			set{ _str_pad=value;}
			get{return _str_pad;}
		}
		/// <summary>
		/// 
		/// </summary>
		public DateTime? TM
		{
			set{ _tm=value;}
			get{return _tm;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? TYPE
		{
			set{ _type=value;}
			get{return _type;}
		}
		#endregion Model

	}
}

