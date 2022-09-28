using Microsoft.Xna.Framework;
using Terraria.Enums;
using Terraria.Graphics.Shaders;

namespace Terraria.GameContent.Shaders
{
	public class SepiaScreenShaderData : ScreenShaderData
	{
		public SepiaScreenShaderData(string passName)
			: base(passName)
		{
		}

		public override void Update(GameTime gameTime)
		{
			float x = (Main.screenPosition.Y + (float)(Main.screenHeight / 2)) / 16f;
			float num = 1f - Utils.SmoothStep((float)Main.worldSurface, (float)Main.worldSurface + 30f, x);
			Vector3 value;
			Vector3 vector = (value = new Vector3(0.191f, -0.054f, -0.221f));
			Vector3 value2 = vector * 0.5f;
			Vector3 value3 = new Vector3(0f, -0.03f, 0.15f);
			Vector3 value4 = new Vector3(-0.11f, 0.01f, 0.16f);
			float cloudAlpha = Main.cloudAlpha;
			GetDaylightPowers(out var nightlightPower, out var daylightPower, out var moonPower, out var dawnPower);
			float num2 = nightlightPower * 0.13f;
			if (Main.starGame)
			{
				float num3 = (float)Main.starGameMath() - 1f;
				nightlightPower = num3;
				daylightPower = 1f - num3;
				moonPower = num3;
				dawnPower = 1f - num3;
				num2 = nightlightPower * 0.13f;
			}
			else if (!Main.dayTime)
			{
				if (Main.GetMoonPhase() == MoonPhase.Full)
				{
					value = new Vector3(-0.19f, 0.01f, 0.22f);
					num2 += 0.07f * moonPower;
				}
				if (Main.bloodMoon)
				{
					value = new Vector3(0.2f, -0.1f, -0.221f);
					num2 = 0.2f;
				}
			}
			nightlightPower *= num;
			daylightPower *= num;
			moonPower *= num;
			dawnPower *= num;
			UseOpacity(1f);
			UseIntensity(1.4f - daylightPower * 0.2f);
			float value5 = 0.3f - num2 * nightlightPower;
			value5 = MathHelper.Lerp(value5, 0.1f, cloudAlpha);
			float value6 = 0.2f;
			value5 = MathHelper.Lerp(value5, value6, 1f - num);
			UseProgress(value5);
			Vector3 value7 = Vector3.Lerp(vector, value, moonPower);
			value7 = Vector3.Lerp(value7, value3, dawnPower);
			value7 = Vector3.Lerp(value7, value4, cloudAlpha);
			value7 = Vector3.Lerp(value7, value2, 1f - num);
			UseColor(value7);
		}

		private static void GetDaylightPowers(out float nightlightPower, out float daylightPower, out float moonPower, out float dawnPower)
		{
			nightlightPower = 0f;
			daylightPower = 0f;
			moonPower = 0f;
			Vector2 dayTimeAsDirectionIn24HClock = Utils.GetDayTimeAsDirectionIn24HClock();
			Vector2 dayTimeAsDirectionIn24HClock2 = Utils.GetDayTimeAsDirectionIn24HClock(4.5f);
			Vector2 dayTimeAsDirectionIn24HClock3 = Utils.GetDayTimeAsDirectionIn24HClock(0f);
			float fromValue = Vector2.Dot(dayTimeAsDirectionIn24HClock, dayTimeAsDirectionIn24HClock3);
			float fromValue2 = Vector2.Dot(dayTimeAsDirectionIn24HClock, dayTimeAsDirectionIn24HClock2);
			nightlightPower = Utils.Remap(fromValue, -0.2f, 0.1f, 0f, 1f);
			daylightPower = Utils.Remap(fromValue, 0.1f, -1f, 0f, 1f);
			dawnPower = Utils.Remap(fromValue2, 0.66f, 1f, 0f, 1f);
			if (!Main.dayTime)
			{
				float num = (float)(Main.time / 32400.0) * 2f;
				if (num > 1f)
				{
					num = 2f - num;
				}
				moonPower = Utils.Remap(num, 0f, 0.25f, 0f, 1f);
			}
		}
	}
}
