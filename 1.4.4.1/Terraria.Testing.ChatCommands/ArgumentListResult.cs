using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.Testing.ChatCommands
{
	public class ArgumentListResult : IEnumerable<string>, IEnumerable
	{
		public static readonly ArgumentListResult Empty = new ArgumentListResult(isValid: true);

		public static readonly ArgumentListResult Invalid = new ArgumentListResult(isValid: false);

		public readonly bool IsValid;

		private readonly List<string> _results;

		public int Count => _results.Count;

		public string this[int index] => _results[index];

		public ArgumentListResult(IEnumerable<string> results)
		{
			_results = results.ToList();
			IsValid = true;
		}

		private ArgumentListResult(bool isValid)
		{
			_results = new List<string>();
			IsValid = isValid;
		}

		public IEnumerator<string> GetEnumerator()
		{
			return _results.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
