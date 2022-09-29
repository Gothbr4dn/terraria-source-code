using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;

namespace Terraria.ID
{
	public static class TorchID
	{
		private interface ITorchLightProvider
		{
			void GetRGB(out float r, out float g, out float b);
		}

		private struct ConstantTorchLight : ITorchLightProvider
		{
			public float R;

			public float G;

			public float B;

			public ConstantTorchLight(float Red, float Green, float Blue)
			{
				R = Red;
				G = Green;
				B = Blue;
			}

			public void GetRGB(out float r, out float g, out float b)
			{
				r = R;
				g = G;
				b = B;
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct DemonTorchLight : ITorchLightProvider
		{
			public void GetRGB(out float r, out float g, out float b)
			{
				r = 0.5f * Main.demonTorch + (1f - Main.demonTorch);
				g = 0.3f;
				b = Main.demonTorch + 0.5f * (1f - Main.demonTorch);
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct ShimmerTorchLight : ITorchLightProvider
		{
			public void GetRGB(out float r, out float g, out float b)
			{
				float num = 0.9f;
				float num2 = 0.9f;
				num += (float)(270 - Main.mouseTextColor) / 900f;
				num2 += (float)(270 - Main.mouseTextColor) / 125f;
				num = MathHelper.Clamp(num, 0f, 1f);
				num2 = MathHelper.Clamp(num2, 0f, 1f);
				r = num * 0.9f;
				g = num2 * 0.55f;
				b = num * 1.2f;
			}
		}

		[StructLayout(LayoutKind.Sequential, Size = 1)]
		private struct DiscoTorchLight : ITorchLightProvider
		{
			public void GetRGB(out float r, out float g, out float b)
			{
				r = (float)Main.DiscoR / 255f;
				g = (float)Main.DiscoG / 255f;
				b = (float)Main.DiscoB / 255f;
			}
		}

		public static int[] Dust = new int[24]
		{
			6, 59, 60, 61, 62, 63, 64, 65, 75, 135,
			158, 169, 156, 234, 66, 242, 293, 294, 295, 296,
			297, 298, 307, 310
		};

		private static ITorchLightProvider[] _lights;

		public const short Torch = 0;

		public const short Blue = 1;

		public const short Red = 2;

		public const short Green = 3;

		public const short Purple = 4;

		public const short White = 5;

		public const short Yellow = 6;

		public const short Demon = 7;

		public const short Cursed = 8;

		public const short Ice = 9;

		public const short Orange = 10;

		public const short Ichor = 11;

		public const short UltraBright = 12;

		public const short Bone = 13;

		public const short Rainbow = 14;

		public const short Pink = 15;

		public const short Desert = 16;

		public const short Coral = 17;

		public const short Corrupt = 18;

		public const short Crimson = 19;

		public const short Hallowed = 20;

		public const short Jungle = 21;

		public const short Mushroom = 22;

		public const short Shimmer = 23;

		public const short Count = 24;

		public static void Initialize()
		{
			_lights = new ITorchLightProvider[24]
			{
				new ConstantTorchLight(1f, 0.95f, 0.8f),
				new ConstantTorchLight(0f, 0.1f, 1.3f),
				new ConstantTorchLight(1f, 0.1f, 0.1f),
				new ConstantTorchLight(0f, 1f, 0.1f),
				new ConstantTorchLight(0.9f, 0f, 0.9f),
				new ConstantTorchLight(1.4f, 1.4f, 1.4f),
				new ConstantTorchLight(0.9f, 0.9f, 0f),
				default(DemonTorchLight),
				new ConstantTorchLight(1f, 1.6f, 0.5f),
				new ConstantTorchLight(0.75f, 0.85f, 1.4f),
				new ConstantTorchLight(1f, 0.5f, 0f),
				new ConstantTorchLight(1.4f, 1.4f, 0.7f),
				new ConstantTorchLight(0.75f, 1.3499999f, 1.5f),
				new ConstantTorchLight(0.95f, 0.75f, 1.3f),
				default(DiscoTorchLight),
				new ConstantTorchLight(1f, 0f, 1f),
				new ConstantTorchLight(1.4f, 0.85f, 0.55f),
				new ConstantTorchLight(0.25f, 1.3f, 0.8f),
				new ConstantTorchLight(0.95f, 0.4f, 1.4f),
				new ConstantTorchLight(1.4f, 0.7f, 0.5f),
				new ConstantTorchLight(1.25f, 0.6f, 1.2f),
				new ConstantTorchLight(0.75f, 1.45f, 0.9f),
				new ConstantTorchLight(0.3f, 0.78f, 1.2f),
				default(ShimmerTorchLight)
			};
		}

		public static void TorchColor(int torchID, out float R, out float G, out float B)
		{
			if (torchID < 0 || torchID >= _lights.Length)
			{
				R = (G = (B = 0f));
			}
			else
			{
				_lights[torchID].GetRGB(out R, out G, out B);
			}
		}
	}
}
