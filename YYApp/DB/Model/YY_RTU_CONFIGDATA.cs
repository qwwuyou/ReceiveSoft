using System;
namespace Service.Model
{
	/// <summary>
	/// YY_RTU_CONFIGDATA:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_RTU_CONFIGDATA
	{
		public YY_RTU_CONFIGDATA()
		{}
		#region Model
		private string _stcd;
		private string _itemid;
		private string _configid;
		private string _configval;
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
		public string ConfigID
		{
			set{ _configid=value;}
			get{return _configid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ConfigVal
		{
			set{ _configval=value;}
			get{return _configval;}
		}
		#endregion Model

	}
}

