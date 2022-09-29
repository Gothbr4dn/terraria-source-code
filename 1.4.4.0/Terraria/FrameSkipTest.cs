using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;

namespace Terraria
{
	public class FrameSkipTest
	{
		private static int LastRecordedSecondNumber;

		private static float CallsThisSecond;

		private static float DeltasThisSecond;

		private static List<float> DeltaSamples = new List<float>();

		private const int SamplesCount = 5;

		private static MultiTimer serverFramerateTest = new MultiTimer(60);

		public static void Update(GameTime gameTime)
		{
			float num = 60f;
			float num2 = 1f / num;
			float num3 = (float)gameTime.ElapsedGameTime.TotalSeconds;
			Thread.Sleep((int)MathHelper.Clamp((num2 - num3) * 1000f + 1f, 0f, 1000f));
		}

		public static void CheckReset(GameTime gameTime)
		{
			if (LastRecordedSecondNumber != gameTime.TotalGameTime.Seconds)
			{
				DeltaSamples.Add(DeltasThisSecond / CallsThisSecond);
				if (DeltaSamples.Count > 5)
				{
					DeltaSamples.RemoveAt(0);
				}
				CallsThisSecond = 0f;
				DeltasThisSecond = 0f;
				LastRecordedSecondNumber = gameTime.TotalGameTime.Seconds;
			}
		}

		public static void UpdateServerTest()
		{
			serverFramerateTest.Record("frame time");
			serverFramerateTest.StopAndPrint();
			serverFramerateTest.Start();
		}
	}
}
