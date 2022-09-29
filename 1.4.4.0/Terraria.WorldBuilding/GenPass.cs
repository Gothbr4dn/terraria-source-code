using Terraria.IO;

namespace Terraria.WorldBuilding
{
	public abstract class GenPass : GenBase
	{
		public string Name;

		public double Weight;

		public GenPass(string name, double loadWeight)
		{
			Name = name;
			Weight = loadWeight;
		}

		protected abstract void ApplyPass(GenerationProgress progress, GameConfiguration configuration);

		public void Apply(GenerationProgress progress, GameConfiguration configuration)
		{
			ApplyPass(progress, configuration);
		}
	}
}
