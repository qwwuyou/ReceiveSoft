using System;
namespace Service.Model
{
	/// <summary>
	/// YY_RTU_COMMAND:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_RTU_COMMAND
	{
		public YY_RTU_COMMAND()
		{}
		#region Model
		private string _commandid;
		private string _remark;
		private bool _state;
		/// <summary>
		/// 
		/// </summary>
		public string CommandID
		{
			set{ _commandid=value;}
			get{return _commandid;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string Remark
		{
			set{ _remark=value;}
			get{return _remark;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool State
		{
			set{ _state=value;}
			get{return _state;}
		}
		#endregion Model

	}
}

