using System;
namespace Service.Model
{
	/// <summary>
	/// YY_RTU_WORK:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_RTU_WORK
	{
		public YY_RTU_WORK()
		{}
		#region Model
		private string _stcd;
		private int _mode;
		private bool? _autoswitch;
		private bool? _relaying;
		private bool? _powerreport;
		private bool? _switchreport;
		private bool? _faultreport;
		private bool? _fixvaluestatus;
		private decimal? _fixvalue;
		private string _relayaddress;
		private int? _relaylength;
		private bool? _icstatus;
		private bool? _workm;
		private bool? _communicationm;
		private int? _pwmode;
		private string _elem;
		private string _phonenum;
		private string _satellitenum;
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
		public int MODE
		{
			set{ _mode=value;}
			get{return _mode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? AutoSwitch
		{
			set{ _autoswitch=value;}
			get{return _autoswitch;}
		}
		/// <summary>
		/// 
		/// </summary>
        public bool? Relaying
		{
			set{ _relaying=value;}
			get{return _relaying;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? PowerReport
		{
			set{ _powerreport=value;}
			get{return _powerreport;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? SwitchReport
		{
			set{ _switchreport=value;}
			get{return _switchreport;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? FaultReport
		{
			set{ _faultreport=value;}
			get{return _faultreport;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? FixValueStatus
		{
			set{ _fixvaluestatus=value;}
			get{return _fixvaluestatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public decimal? FixValue
		{
			set{ _fixvalue=value;}
			get{return _fixvalue;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string RelayAddress
		{
			set{ _relayaddress=value;}
			get{return _relayaddress;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? RelayLength
		{
			set{ _relaylength=value;}
			get{return _relaylength;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? ICStatus
		{
			set{ _icstatus=value;}
			get{return _icstatus;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? WorkM
		{
			set{ _workm=value;}
			get{return _workm;}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool? CommunicationM
		{
			set{ _communicationm=value;}
			get{return _communicationm;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? PWMODE
		{
			set{ _pwmode=value;}
			get{return _pwmode;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ELEM
		{
			set{ _elem=value;}
			get{return _elem;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PhoneNum
		{
			set{ _phonenum=value;}
			get{return _phonenum;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string SatelliteNum
		{
			set{ _satellitenum=value;}
			get{return _satellitenum;}
		}
		#endregion Model

	}
}

