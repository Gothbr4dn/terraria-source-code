using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.Localization;

namespace Terraria.Social.Base
{
	public class WorkshopIssueReporter : IProvideReports
	{
		private int _maxReports = 1000;

		private List<IssueReport> _reports = new List<IssueReport>();

		public event Action OnNeedToOpenUI;

		public event Action OnNeedToNotifyUI;

		private void AddReport(string reportText)
		{
			IssueReport item = new IssueReport(reportText);
			_reports.Add(item);
			while (_reports.Count > _maxReports)
			{
				_reports.RemoveAt(0);
			}
		}

		public List<IssueReport> GetReports()
		{
			return _reports;
		}

		private void OpenReportsScreen()
		{
			if (this.OnNeedToOpenUI != null)
			{
				this.OnNeedToOpenUI();
			}
		}

		private void NotifyReportsScreen()
		{
			if (this.OnNeedToNotifyUI != null)
			{
				this.OnNeedToNotifyUI();
			}
		}

		public void ReportInstantUploadProblem(string textKey)
		{
			string textValue = Language.GetTextValue(textKey);
			AddReport(textValue);
			OpenReportsScreen();
		}

		public void ReportInstantUploadProblemFromValue(string text)
		{
			AddReport(text);
			OpenReportsScreen();
		}

		public void ReportDelayedUploadProblem(string textKey)
		{
			string textValue = Language.GetTextValue(textKey);
			AddReport(textValue);
			NotifyReportsScreen();
		}

		public void ReportDelayedUploadProblemWithoutKnownReason(string textKey, string reasonValue)
		{
			object obj = new
			{
				Reason = reasonValue
			};
			string textValueWith = Language.GetTextValueWith(textKey, obj);
			AddReport(textValueWith);
			NotifyReportsScreen();
		}

		public void ReportDownloadProblem(string textKey, string path, Exception exception)
		{
			object obj = new
			{
				FilePath = path,
				Reason = exception.ToString()
			};
			string textValueWith = Language.GetTextValueWith(textKey, obj);
			AddReport(textValueWith);
			NotifyReportsScreen();
		}

		public void ReportManifestCreationProblem(string textKey, Exception exception)
		{
			object obj = new
			{
				Reason = exception.ToString()
			};
			string textValueWith = Language.GetTextValueWith(textKey, obj);
			AddReport(textValueWith);
			NotifyReportsScreen();
		}
	}
}
