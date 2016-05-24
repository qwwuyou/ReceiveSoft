using System;
namespace Service.Model
{
	/// <summary>
	/// YY_RTU_BI:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_RTU_BI
	{
		public YY_RTU_BI()
		{}
		#region Model
		private string _stcd;
		private string _itemid;
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
		#endregion Model

	}
}

