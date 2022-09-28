using System;
using System.Collections.Generic;
using System.Linq;

namespace Terraria.Utilities
{
	public class WeightedRandom<T>
	{
		public readonly List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

		public readonly UnifiedRandom random;

		public bool needsRefresh = true;

		private double _totalWeight;

		public WeightedRandom()
		{
			random = new UnifiedRandom();
		}

		public WeightedRandom(int seed)
		{
			random = new UnifiedRandom(seed);
		}

		public WeightedRandom(UnifiedRandom random)
		{
			this.random = random;
		}

		public WeightedRandom(params Tuple<T, double>[] theElements)
		{
			random = new UnifiedRandom();
			elements = theElements.ToList();
		}

		public WeightedRandom(int seed, params Tuple<T, double>[] theElements)
		{
			random = new UnifiedRandom(seed);
			elements = theElements.ToList();
		}

		public WeightedRandom(UnifiedRandom random, params Tuple<T, double>[] theElements)
		{
			this.random = random;
			elements = theElements.ToList();
		}

		public void Add(T element, double weight = 1.0)
		{
			elements.Add(new Tuple<T, double>(element, weight));
			needsRefresh = true;
		}

		public T Get()
		{
			if (needsRefresh)
			{
				CalculateTotalWeight();
			}
			double num = random.NextDouble();
			num *= _totalWeight;
			foreach (Tuple<T, double> element in elements)
			{
				if (num > element.Item2)
				{
					num -= element.Item2;
					continue;
				}
				return element.Item1;
			}
			return default(T);
		}

		public void CalculateTotalWeight()
		{
			_totalWeight = 0.0;
			foreach (Tuple<T, double> element in elements)
			{
				_totalWeight += element.Item2;
			}
			needsRefresh = false;
		}

		public void Clear()
		{
			elements.Clear();
		}

		public static implicit operator T(WeightedRandom<T> weightedRandom)
		{
			return weightedRandom.Get();
		}
	}
}
