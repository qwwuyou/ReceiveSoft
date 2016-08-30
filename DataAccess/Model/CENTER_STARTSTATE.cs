
using System;
namespace Service.Model
{
	/// <summary>
    /// CENTER_STARTSTATE:实体类(属性说明自动提取数据库字段的描述信息)
	/// </summary>
	[Serializable]
	public partial class CENTER_STARTSTATE
	{
        public CENTER_STARTSTATE()
		{}

		#region Model
        private string _projectname;
        private string _publicip;
        private string _runtime;
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

        public string RunTime
        {
            get { return _runtime; }
            set { _runtime = value; }
        }

        public DateTime DTime
        {
            get { return _dtime; }
            set { _dtime = value; }
        }
		#endregion Model

	}
}