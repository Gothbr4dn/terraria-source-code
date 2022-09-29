using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Cinematics
{
	public class Film
	{
		private class Sequence
		{
			private FrameEvent _frameEvent;

			private int _duration;

			private int _start;

			public FrameEvent Event => _frameEvent;

			public int Duration => _duration;

			public int Start => _start;

			public Sequence(FrameEvent frameEvent, int start, int duration)
			{
				_frameEvent = frameEvent;
				_start = start;
				_duration = duration;
			}
		}

		private int _frame;

		private int _frameCount;

		private int _nextSequenceAppendTime;

		private bool _isActive;

		private List<Sequence> _sequences = new List<Sequence>();

		public int Frame => _frame;

		public int FrameCount => _frameCount;

		public int AppendPoint => _nextSequenceAppendTime;

		public bool IsActive => _isActive;

		public void AddSequence(int start, int duration, FrameEvent frameEvent)
		{
			_sequences.Add(new Sequence(frameEvent, start, duration));
			_nextSequenceAppendTime = Math.Max(_nextSequenceAppendTime, start + duration);
			_frameCount = Math.Max(_frameCount, start + duration);
		}

		public void AppendSequence(int duration, FrameEvent frameEvent)
		{
			AddSequence(_nextSequenceAppendTime, duration, frameEvent);
		}

		public void AddSequences(int start, int duration, params FrameEvent[] frameEvents)
		{
			foreach (FrameEvent frameEvent in frameEvents)
			{
				AddSequence(start, duration, frameEvent);
			}
		}

		public void AppendSequences(int duration, params FrameEvent[] frameEvents)
		{
			int nextSequenceAppendTime = _nextSequenceAppendTime;
			foreach (FrameEvent frameEvent in frameEvents)
			{
				_sequences.Add(new Sequence(frameEvent, nextSequenceAppendTime, duration));
				_nextSequenceAppendTime = Math.Max(_nextSequenceAppendTime, nextSequenceAppendTime + duration);
				_frameCount = Math.Max(_frameCount, nextSequenceAppendTime + duration);
			}
		}

		public void AppendEmptySequence(int duration)
		{
			AddSequence(_nextSequenceAppendTime, duration, EmptyFrameEvent);
		}

		public void AppendKeyFrame(FrameEvent frameEvent)
		{
			AddKeyFrame(_nextSequenceAppendTime, frameEvent);
		}

		public void AppendKeyFrames(params FrameEvent[] frameEvents)
		{
			int nextSequenceAppendTime = _nextSequenceAppendTime;
			foreach (FrameEvent frameEvent in frameEvents)
			{
				_sequences.Add(new Sequence(frameEvent, nextSequenceAppendTime, 1));
			}
			_frameCount = Math.Max(_frameCount, nextSequenceAppendTime + 1);
		}

		public void AddKeyFrame(int frame, FrameEvent frameEvent)
		{
			_sequences.Add(new Sequence(frameEvent, frame, 1));
			_frameCount = Math.Max(_frameCount, frame + 1);
		}

		public void AddKeyFrames(int frame, params FrameEvent[] frameEvents)
		{
			foreach (FrameEvent frameEvent in frameEvents)
			{
				AddKeyFrame(frame, frameEvent);
			}
		}

		public bool OnUpdate(GameTime gameTime)
		{
			if (_sequences.Count == 0)
			{
				return false;
			}
			foreach (Sequence sequence in _sequences)
			{
				int num = _frame - sequence.Start;
				if (num >= 0 && num < sequence.Duration)
				{
					sequence.Event(new FrameEventData(_frame, sequence.Start, sequence.Duration));
				}
			}
			return ++_frame != _frameCount;
		}

		public virtual void OnBegin()
		{
			_isActive = true;
		}

		public virtual void OnEnd()
		{
			_isActive = false;
		}

		private static void EmptyFrameEvent(FrameEventData evt)
		{
		}
	}
}
