using System;
namespace Service.Model
{
	/// <summary>
	/// YY_RTU_CONFIGITEM:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_RTU_CONFIGITEM
	{
		public YY_RTU_CONFIGITEM()
		{}
		#region Model
		private string _configid;
		private string _configitem;
		private string _datatype;
		private string _units;
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
		public string ConfigItem
		{
			set{ _configitem=value;}
			get{return _configitem;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string DataType
		{
			set{ _datatype=value;}
			get{return _datatype;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Units
		{
			set{ _units=value;}
			get{return _units;}
		}
		#endregion Model

	}
}

