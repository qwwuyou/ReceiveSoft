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
		private int? _com_b1;
		private string _adr_b1;
        private int? _com_b2;
        private string _adr_b2;
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
		
		/// <summary>
		/// 
		/// </summary>
		public int? COM_B1
		{
			set{ _com_b1=value;}
			get{return _com_b1;}
		}
		/// <summary>
		/// 
		/// </summary>
		public string ADR_B1
		{
			set{ _adr_b1=value;}
			get{return _adr_b1;}
		}
        /// <summary>
        /// 
        /// </summary>
        public int? COM_B2
        {
            set { _com_b2 = value; }
            get { return _com_b2; }
        }
        /// <summary>
        /// 
        /// </summary>
        public string ADR_B2
        {
            set { _adr_b2 = value; }
            get { return _adr_b2; }
        }
		#endregion Model

	}
}

