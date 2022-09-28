using System;

namespace Terraria.DataStructures
{
	public class IssueReport
	{
		public DateTime timeReported;

		public string reportText;

		public IssueReport(string reportText)
		{
			timeReported = DateTime.Now;
			this.reportText = reportText;
		}
	}
}
