using System;
namespace Service.Model
{
	/// <summary>
	/// YY_LOG:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_LOG
	{
		public YY_LOG()
		{}
		#region Model
		private int _logid;
		private string _rtulog;
		/// <summary>
		/// 
		/// </summary>
		public int LOGID
		{
			set{ _logid=value;}
			get{return _logid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RTULOG
		{
			set{ _rtulog=value;}
			get{return _rtulog;}
		}
		#endregion Model

	}
}

