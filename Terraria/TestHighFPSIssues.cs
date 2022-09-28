using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.GameInput;

namespace Terraria
{
	public class TestHighFPSIssues
	{
		private static List<double> _tapUpdates = new List<double>();

		private static List<double> _tapUpdateEnds = new List<double>();

		private static List<double> _tapDraws = new List<double>();

		private static int conU;

		private static int conUH;

		private static int conD;

		private static int conDH;

		private static int race;

		public static void TapUpdate(GameTime gt)
		{
			_tapUpdates.Add(gt.TotalGameTime.TotalMilliseconds);
			conD = 0;
			race--;
			if (++conU > conUH)
			{
				conUH = conU;
			}
		}

		public static void TapUpdateEnd(GameTime gt)
		{
			_tapUpdateEnds.Add(gt.TotalGameTime.TotalMilliseconds);
		}

		public static void TapDraw(GameTime gt)
		{
			_tapDraws.Add(gt.TotalGameTime.TotalMilliseconds);
			conU = 0;
			race++;
			if (++conD > conDH)
			{
				conDH = conD;
			}
		}

		public static void Update(GameTime gt)
		{
			if (PlayerInput.Triggers.Current.Down)
			{
				race = (conUH = (conDH = 0));
			}
			double num = gt.TotalGameTime.TotalMilliseconds - 5000.0;
			while (_tapUpdates.Count > 0 && _tapUpdates[0] < num)
			{
				_tapUpdates.RemoveAt(0);
			}
			while (_tapDraws.Count > 0 && _tapDraws[0] < num)
			{
				_tapDraws.RemoveAt(0);
			}
			while (_tapUpdateEnds.Count > 0 && _tapUpdateEnds[0] < num)
			{
				_tapUpdateEnds.RemoveAt(0);
			}
			Main.versionNumber = "total (u/d)   " + _tapUpdates.Count + " " + _tapUpdateEnds.Count + "  " + race + " " + conUH + " " + conDH;
			Main.NewText(Main.versionNumber);
		}
	}
}
