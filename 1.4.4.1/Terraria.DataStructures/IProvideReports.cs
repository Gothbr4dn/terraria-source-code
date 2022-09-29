using System.Collections.Generic;

namespace Terraria.DataStructures
{
	public interface IProvideReports
	{
		List<IssueReport> GetReports();
	}
}
