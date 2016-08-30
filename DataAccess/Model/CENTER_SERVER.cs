using System;
namespace Service.Model
{
	/// <summary>
    /// CENTER_SERVER:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CENTER_SERVER
	{
		public CENTER_SERVER()
		{}

		#region Model
        private string _projectname;
        private string _publicip;
        private string _runtime;
        private int _registertime;
        private int _rtucount;
        private DateTime _dtime;
        private string _runstate;
        private DateTime _srarttime;


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

        public string RunTime
        {
            get { return _runtime; }
            set { _runtime = value; }
        }

        public int RegisterTime
        {
            get { return _registertime; }
            set { _registertime = value; }
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

        public string RunState
        {
            get { return _runstate; }
            set { _runstate = value; }
        }

        public DateTime SrartTime
        {
            get { return _srarttime; }
            set { _srarttime = value; }
        }
		#endregion Model

	}
}