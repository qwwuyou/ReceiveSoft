using System;
namespace Service.Model
{
	/// <summary>
	/// YY_RTU_Basic:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_RTU_Basic
	{
		public YY_RTU_Basic()
		{}
		#region Model
		private string _stcd;
		private string _password;
		private string _nicename;
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
		public string PassWord
		{
			set{ _password=value;}
			get{return _password;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string NiceName
		{
			set{ _nicename=value;}
			get{return _nicename;}
		}
		#endregion Model

	}
}

