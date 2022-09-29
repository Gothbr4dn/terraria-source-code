using System;

namespace Terraria.Graphics.Effects
{
	public class MissingEffectException : Exception
	{
		public MissingEffectException(string text)
			: base(text)
		{
		}
	}
}
