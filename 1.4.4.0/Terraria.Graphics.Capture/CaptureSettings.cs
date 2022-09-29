using System;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.Capture
{
	public class CaptureSettings
	{
		public Rectangle Area;

		public bool UseScaling = true;

		public string OutputName;

		public bool CaptureEntities = true;

		public CaptureBiome Biome = CaptureBiome.DefaultPurity;

		public bool CaptureMech;

		public bool CaptureBackground;

		public CaptureSettings()
		{
			DateTime dateTime = DateTime.Now.ToLocalTime();
			OutputName = "Capture " + dateTime.Year.ToString("D4") + "-" + dateTime.Month.ToString("D2") + "-" + dateTime.Day.ToString("D2") + " " + dateTime.Hour.ToString("D2") + "_" + dateTime.Minute.ToString("D2") + "_" + dateTime.Second.ToString("D2");
		}
	}
}
