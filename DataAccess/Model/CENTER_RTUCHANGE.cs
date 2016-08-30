using System;
namespace Service.Model
{
	/// <summary>
    /// CENTER_RTUCHANGE:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CENTER_RTUCHANGE
	{
        public CENTER_RTUCHANGE()
		{}

		#region Model
        private string _projectname;
        private string _publicip;
        private int _rtucount;
        private DateTime _dtime;

        public string ProjectName
        {
            get { return _projectname; }
            set { _projectname = value; }
        }

        public string PublicIP
        {
            get { return _publicip; }
            set { _publicip = value; }
        }

        public int RTUCount
        {
            get { return _rtucount; }
            set { _rtucount = value; }
        }

        public DateTime DTime
        {
            get { return _dtime; }
            set { _dtime = value; }
        }
        #endregion Model

	}
}