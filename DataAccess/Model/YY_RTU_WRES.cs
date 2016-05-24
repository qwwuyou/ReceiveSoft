using System;
namespace Service.Model
{
	/// <summary>
	/// YY_RTU_WRES:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class YY_RTU_WRES
	{
		public YY_RTU_WRES()
		{}
		#region Model
		private string _stcd;
		private int _code;
		private int? _adr_zx;
		private int? _com_m;
		private string _adr_m;
        private int? _port_m;
		private int? _com_b;
		private string _adr_b;
        private int? _port_b;
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
		public int CODE
		{
			set{ _code=value;}
			get{return _code;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? ADR_ZX
		{
			set{ _adr_zx=value;}
			get{return _adr_zx;}
		}
		/// <summary>
		/// 
		/// </summary>
		public int? COM_M
		{
			set{ _com_m=value;}
			get{return _com_m;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ADR_M
		{
			set{ _adr_m=value;}
			get{return _adr_m;}
		}
        public int?  PORT_M
        {
            set { _port_m = value; }
            get { return _port_m; }
        }
		/// <summary>
		/// 
		/// </summary>
		public int? COM_B
		{
			set{ _com_b=value;}
			get{return _com_b;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ADR_B
		{
			set{ _adr_b=value;}
			get{return _adr_b;}
		}

        public int? PORT_B
        {
            set { _port_b = value; }
            get { return _port_b; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string PhoneNum
        {
            set { _phonenum = value; }
            get { return _phonenum; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string SatelliteNum
        {
            set { _satellitenum = value; }
            get { return _satellitenum; }
        }
		#endregion Model

	}
}

