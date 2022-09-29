using System;

namespace Terraria.DataStructures
{
	public class DoubleStack<T1>
	{
		private T1[][] _segmentList;

		private readonly int _segmentSize;

		private int _segmentCount;

		private readonly int _segmentShiftPosition;

		private int _start;

		private int _end;

		private int _size;

		private int _last;

		public int Count => _size;

		public DoubleStack(int segmentSize = 1024, int initialSize = 0)
		{
			if (segmentSize < 16)
			{
				segmentSize = 16;
			}
			_start = segmentSize / 2;
			_end = _start;
			_size = 0;
			_segmentShiftPosition = segmentSize + _start;
			initialSize += _start;
			int num = initialSize / segmentSize + 1;
			_segmentList = new T1[num][];
			for (int i = 0; i < num; i++)
			{
				_segmentList[i] = new T1[segmentSize];
			}
			_segmentSize = segmentSize;
			_segmentCount = num;
			_last = _segmentSize * _segmentCount - 1;
		}

		public void PushFront(T1 front)
		{
			if (_start == 0)
			{
				T1[][] array = new T1[_segmentCount + 1][];
				for (int i = 0; i < _segmentCount; i++)
				{
					array[i + 1] = _segmentList[i];
				}
				array[0] = new T1[_segmentSize];
				_segmentList = array;
				_segmentCount++;
				_start += _segmentSize;
				_end += _segmentSize;
				_last += _segmentSize;
			}
			_start--;
			T1[] obj = _segmentList[_start / _segmentSize];
			int num = _start % _segmentSize;
			obj[num] = front;
			_size++;
		}

		public T1 PopFront()
		{
			if (_size == 0)
			{
				throw new InvalidOperationException("The DoubleStack is empty.");
			}
			T1[] obj = _segmentList[_start / _segmentSize];
			int num = _start % _segmentSize;
			T1 result = obj[num];
			obj[num] = default(T1);
			_start++;
			_size--;
			if (_start >= _segmentShiftPosition)
			{
				T1[] array = _segmentList[0];
				for (int i = 0; i < _segmentCount - 1; i++)
				{
					_segmentList[i] = _segmentList[i + 1];
				}
				_segmentList[_segmentCount - 1] = array;
				_start -= _segmentSize;
				_end -= _segmentSize;
			}
			if (_size == 0)
			{
				_start = _segmentSize / 2;
				_end = _start;
			}
			return result;
		}

		public T1 PeekFront()
		{
			if (_size == 0)
			{
				throw new InvalidOperationException("The DoubleStack is empty.");
			}
			T1[] obj = _segmentList[_start / _segmentSize];
			int num = _start % _segmentSize;
			return obj[num];
		}

		public void PushBack(T1 back)
		{
			if (_end == _last)
			{
				T1[][] array = new T1[_segmentCount + 1][];
				for (int i = 0; i < _segmentCount; i++)
				{
					array[i] = _segmentList[i];
				}
				array[_segmentCount] = new T1[_segmentSize];
				_segmentCount++;
				_segmentList = array;
				_last += _segmentSize;
			}
			T1[] obj = _segmentList[_end / _segmentSize];
			int num = _end % _segmentSize;
			obj[num] = back;
			_end++;
			_size++;
		}

		public T1 PopBack()
		{
			if (_size == 0)
			{
				throw new InvalidOperationException("The DoubleStack is empty.");
			}
			T1[] obj = _segmentList[_end / _segmentSize];
			int num = _end % _segmentSize;
			T1 result = obj[num];
			obj[num] = default(T1);
			_end--;
			_size--;
			if (_size == 0)
			{
				_start = _segmentSize / 2;
				_end = _start;
			}
			return result;
		}

		public T1 PeekBack()
		{
			if (_size == 0)
			{
				throw new InvalidOperationException("The DoubleStack is empty.");
			}
			T1[] obj = _segmentList[_end / _segmentSize];
			int num = _end % _segmentSize;
			return obj[num];
		}

		public void Clear(bool quickClear = false)
		{
			if (!quickClear)
			{
				for (int i = 0; i < _segmentCount; i++)
				{
					Array.Clear(_segmentList[i], 0, _segmentSize);
				}
			}
			_start = _segmentSize / 2;
			_end = _start;
			_size = 0;
		}
	}
}
