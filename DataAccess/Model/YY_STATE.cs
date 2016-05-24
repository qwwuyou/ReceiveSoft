using System;
namespace Service.Model
{
	/// <summary>
	/// YY_STATE:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_STATE
	{
		public YY_STATE()
		{}
		#region Model
		private int _stateid;
		private string _rtustate;
		/// <summary>
		/// 
		/// </summary>
		public int STATEID
		{
			set{ _stateid=value;}
			get{return _stateid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RTUSTATE
		{
			set{ _rtustate=value;}
			get{return _rtustate;}
		}
		#endregion Model

	}
}

