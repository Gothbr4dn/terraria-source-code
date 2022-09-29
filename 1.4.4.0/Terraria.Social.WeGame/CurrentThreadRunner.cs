using System;
using System.Windows.Threading;

namespace Terraria.Social.WeGame
{
	public class CurrentThreadRunner
	{
		private Dispatcher _dsipatcher;

		public CurrentThreadRunner()
		{
			_dsipatcher = Dispatcher.CurrentDispatcher;
		}

		public void Run(Action f)
		{
			_dsipatcher.BeginInvoke(f);
		}
	}
}
