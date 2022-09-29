using System;
using System.Threading.Tasks;

namespace Terraria.Social.WeGame
{
	public class AsyncTaskHelper
	{
		private CurrentThreadRunner _currentThreadRunner;

		private AsyncTaskHelper()
		{
			_currentThreadRunner = new CurrentThreadRunner();
		}

		public void RunAsyncTaskAndReply(Action task, Action replay)
		{
			Task.Factory.StartNew(delegate
			{
				task();
				_currentThreadRunner.Run(replay);
			});
		}

		public void RunAsyncTask(Action task)
		{
			Task.Factory.StartNew(task);
		}
	}
}
