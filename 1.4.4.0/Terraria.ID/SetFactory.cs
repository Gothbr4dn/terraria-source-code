using System;
using System.Collections.Generic;

namespace Terraria.ID
{
	public class SetFactory
	{
		protected int _size;

		private readonly Queue<int[]> _intBufferCache = new Queue<int[]>();

		private readonly Queue<ushort[]> _ushortBufferCache = new Queue<ushort[]>();

		private readonly Queue<bool[]> _boolBufferCache = new Queue<bool[]>();

		private readonly Queue<float[]> _floatBufferCache = new Queue<float[]>();

		private object _queueLock = new object();

		public SetFactory(int size)
		{
			_size = size;
		}

		protected bool[] GetBoolBuffer()
		{
			lock (_queueLock)
			{
				if (_boolBufferCache.Count == 0)
				{
					return new bool[_size];
				}
				return _boolBufferCache.Dequeue();
			}
		}

		protected int[] GetIntBuffer()
		{
			lock (_queueLock)
			{
				if (_intBufferCache.Count == 0)
				{
					return new int[_size];
				}
				return _intBufferCache.Dequeue();
			}
		}

		protected ushort[] GetUshortBuffer()
		{
			lock (_queueLock)
			{
				if (_ushortBufferCache.Count == 0)
				{
					return new ushort[_size];
				}
				return _ushortBufferCache.Dequeue();
			}
		}

		protected float[] GetFloatBuffer()
		{
			lock (_queueLock)
			{
				if (_floatBufferCache.Count == 0)
				{
					return new float[_size];
				}
				return _floatBufferCache.Dequeue();
			}
		}

		public void Recycle<T>(T[] buffer)
		{
			lock (_queueLock)
			{
				if (typeof(T).Equals(typeof(bool)))
				{
					_boolBufferCache.Enqueue((bool[])(object)buffer);
				}
				else if (typeof(T).Equals(typeof(int)))
				{
					_intBufferCache.Enqueue((int[])(object)buffer);
				}
			}
		}

		public bool[] CreateBoolSet(params int[] types)
		{
			return CreateBoolSet(defaultState: false, types);
		}

		public bool[] CreateBoolSet(bool defaultState, params int[] types)
		{
			bool[] boolBuffer = GetBoolBuffer();
			for (int i = 0; i < boolBuffer.Length; i++)
			{
				boolBuffer[i] = defaultState;
			}
			for (int j = 0; j < types.Length; j++)
			{
				boolBuffer[types[j]] = !defaultState;
			}
			return boolBuffer;
		}

		public int[] CreateIntSet(params int[] types)
		{
			return CreateIntSet(-1, types);
		}

		public int[] CreateIntSet(int defaultState, params int[] inputs)
		{
			if (inputs.Length % 2 != 0)
			{
				throw new Exception("You have a bad length for inputs on CreateArraySet");
			}
			int[] intBuffer = GetIntBuffer();
			for (int i = 0; i < intBuffer.Length; i++)
			{
				intBuffer[i] = defaultState;
			}
			for (int j = 0; j < inputs.Length; j += 2)
			{
				intBuffer[inputs[j]] = inputs[j + 1];
			}
			return intBuffer;
		}

		public ushort[] CreateUshortSet(ushort defaultState, params ushort[] inputs)
		{
			if (inputs.Length % 2 != 0)
			{
				throw new Exception("You have a bad length for inputs on CreateArraySet");
			}
			ushort[] ushortBuffer = GetUshortBuffer();
			for (int i = 0; i < ushortBuffer.Length; i++)
			{
				ushortBuffer[i] = defaultState;
			}
			for (int j = 0; j < inputs.Length; j += 2)
			{
				ushortBuffer[inputs[j]] = inputs[j + 1];
			}
			return ushortBuffer;
		}

		public float[] CreateFloatSet(float defaultState, params float[] inputs)
		{
			if (inputs.Length % 2 != 0)
			{
				throw new Exception("You have a bad length for inputs on CreateArraySet");
			}
			float[] floatBuffer = GetFloatBuffer();
			for (int i = 0; i < floatBuffer.Length; i++)
			{
				floatBuffer[i] = defaultState;
			}
			for (int j = 0; j < inputs.Length; j += 2)
			{
				floatBuffer[(int)inputs[j]] = inputs[j + 1];
			}
			return floatBuffer;
		}

		public T[] CreateCustomSet<T>(T defaultState, params object[] inputs)
		{
			if (inputs.Length % 2 != 0)
			{
				throw new Exception("You have a bad length for inputs on CreateCustomSet");
			}
			T[] array = new T[_size];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = defaultState;
			}
			if (inputs != null)
			{
				for (int j = 0; j < inputs.Length; j += 2)
				{
					T val = (typeof(T).IsPrimitive ? ((T)inputs[j + 1]) : ((typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(Nullable<>)) ? ((T)inputs[j + 1]) : ((!typeof(T).IsClass) ? ((T)Convert.ChangeType(inputs[j + 1], typeof(T))) : ((T)inputs[j + 1]))));
					if (inputs[j] is ushort)
					{
						array[(ushort)inputs[j]] = val;
					}
					else if (inputs[j] is int)
					{
						array[(int)inputs[j]] = val;
					}
					else
					{
						array[(short)inputs[j]] = val;
					}
				}
			}
			return array;
		}
	}
}
