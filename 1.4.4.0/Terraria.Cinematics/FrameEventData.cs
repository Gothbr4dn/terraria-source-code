namespace Terraria.Cinematics
{
	public struct FrameEventData
	{
		private int _absoluteFrame;

		private int _start;

		private int _duration;

		public int AbsoluteFrame => _absoluteFrame;

		public int Start => _start;

		public int Duration => _duration;

		public int Frame => _absoluteFrame - _start;

		public bool IsFirstFrame => _start == _absoluteFrame;

		public bool IsLastFrame => Remaining == 0;

		public int Remaining => _start + _duration - _absoluteFrame - 1;

		public FrameEventData(int absoluteFrame, int start, int duration)
		{
			_absoluteFrame = absoluteFrame;
			_start = start;
			_duration = duration;
		}
	}
}
