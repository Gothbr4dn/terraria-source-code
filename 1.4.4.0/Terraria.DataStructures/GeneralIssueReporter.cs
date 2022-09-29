using System.Collections.Generic;

namespace Terraria.DataStructures
{
	public class GeneralIssueReporter : IProvideReports
	{
		private List<IssueReport> _reports = new List<IssueReport>();

		public void AddReport(string textToShow)
		{
			_reports.Add(new IssueReport(textToShow));
		}

		public List<IssueReport> GetReports()
		{
			return _reports;
		}
	}
}
