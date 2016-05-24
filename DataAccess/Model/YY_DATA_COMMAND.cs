using System;
namespace Service.Model
{
	/// <summary>
	/// YY_DATA_COMMAND:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_DATA_COMMAND
	{
		public YY_DATA_COMMAND()
		{}
		#region Model
		private string _commandid;
		private int? _state;
		private DateTime _tm;
		private DateTime? _downdate;
		private string _command;
		private int _nfoindex;
		private string _stcd;
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
		public int? STATE
		{
			set{ _state=value;}
			get{return _state;}
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
		public DateTime? DOWNDATE
		{
			set{ _downdate=value;}
			get{return _downdate;}
		}
		/// <summary>
		/// 
		/// </summary>
        public string Command
		{
            set { _command = value; }
            get { return _command; }
		}
		/// <summary>
		/// 
		/// </summary>
		public int NFOINDEX
		{
            set { _nfoindex = value; }
            get { return _nfoindex; }
		}
		/// <summary>
		/// 
		/// </summary>
		public string STCD
		{
			set{ _stcd=value;}
			get{return _stcd;}
		}
		#endregion Model

	}
}

