using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Terraria
{
	public static class TimeLogger
	{
		private struct TimeLogData
		{
			public float time;

			public float timeMax;

			public int resetMaxTime;

			public bool usedLastDraw;

			public string logText;
		}

		private static StreamWriter logWriter;

		private static StringBuilder logBuilder;

		private static int framesToLog;

		private static int currentFrame;

		private static bool startLoggingNextFrame;

		private static bool endLoggingThisFrame;

		private static bool currentlyLogging;

		private static Stopwatch detailedDrawTimer;

		private static double lastDetailedDrawTime;

		private static TimeLogData[] renderTimes;

		private static TimeLogData[] drawTimes;

		private static TimeLogData[] lightingTimes;

		private static TimeLogData[] detailedDrawTimes;

		private const int maxTimeDelay = 100;

		public static void Initialize()
		{
			currentFrame = 0;
			framesToLog = -1;
			detailedDrawTimer = new Stopwatch();
			renderTimes = new TimeLogData[10];
			drawTimes = new TimeLogData[10];
			lightingTimes = new TimeLogData[5];
			detailedDrawTimes = new TimeLogData[40];
			for (int i = 0; i < renderTimes.Length; i++)
			{
				renderTimes[i].logText = $"Render #{i}";
			}
			drawTimes[0].logText = "Drawing Solid Tiles";
			drawTimes[1].logText = "Drawing Non-Solid Tiles";
			drawTimes[2].logText = "Drawing Wall Tiles";
			drawTimes[3].logText = "Drawing Underground Background";
			drawTimes[4].logText = "Drawing Water Tiles";
			drawTimes[5].logText = "Drawing Black Tiles";
			lightingTimes[0].logText = "Lighting Initialization";
			for (int j = 1; j < lightingTimes.Length; j++)
			{
				lightingTimes[j].logText = $"Lighting Pass #{j}";
			}
			detailedDrawTimes[0].logText = "Finding color tiles";
			detailedDrawTimes[1].logText = "Initial Map Update";
			detailedDrawTimes[2].logText = "Finding Waterfalls";
			detailedDrawTimes[3].logText = "Map Section Update";
			detailedDrawTimes[4].logText = "Map Update";
			detailedDrawTimes[5].logText = "Section Framing";
			detailedDrawTimes[6].logText = "Sky Background";
			detailedDrawTimes[7].logText = "Sun, Moon & Stars";
			detailedDrawTimes[8].logText = "Surface Background";
			detailedDrawTimes[9].logText = "Map";
			detailedDrawTimes[10].logText = "Player Chat";
			detailedDrawTimes[11].logText = "Water Target";
			detailedDrawTimes[12].logText = "Background Target";
			detailedDrawTimes[13].logText = "Black Tile Target";
			detailedDrawTimes[14].logText = "Wall Target";
			detailedDrawTimes[15].logText = "Non Solid Tile Target";
			detailedDrawTimes[16].logText = "Waterfalls";
			detailedDrawTimes[17].logText = "Solid Tile Target";
			detailedDrawTimes[18].logText = "NPCs (Behind Tiles)";
			detailedDrawTimes[19].logText = "NPC";
			detailedDrawTimes[20].logText = "Projectiles";
			detailedDrawTimes[21].logText = "Players";
			detailedDrawTimes[22].logText = "Items";
			detailedDrawTimes[23].logText = "Rain";
			detailedDrawTimes[24].logText = "Gore";
			detailedDrawTimes[25].logText = "Dust";
			detailedDrawTimes[26].logText = "Water Target";
			detailedDrawTimes[27].logText = "Interface";
			detailedDrawTimes[28].logText = "Render Solid Tiles";
			detailedDrawTimes[29].logText = "Render Non Solid Tiles";
			detailedDrawTimes[30].logText = "Render Black Tiles";
			detailedDrawTimes[31].logText = "Render Water/Wires";
			detailedDrawTimes[32].logText = "Render Walls";
			detailedDrawTimes[33].logText = "Render Backgrounds";
			detailedDrawTimes[34].logText = "Drawing Wires";
			detailedDrawTimes[35].logText = "Render layers up to Players";
			detailedDrawTimes[36].logText = "Render Items/Rain/Gore/Dust/Water/Map";
			detailedDrawTimes[37].logText = "Render Interface";
			for (int k = 0; k < detailedDrawTimes.Length; k++)
			{
				if (string.IsNullOrEmpty(detailedDrawTimes[k].logText))
				{
					detailedDrawTimes[k].logText = $"Unnamed detailed draw #{k}";
				}
			}
		}

		public static void Start()
		{
			if (currentlyLogging)
			{
				endLoggingThisFrame = true;
				startLoggingNextFrame = false;
			}
			else
			{
				startLoggingNextFrame = true;
				endLoggingThisFrame = false;
				Main.NewText("Detailed logging started", 250, 250, 0);
			}
		}

		public static void NewDrawFrame()
		{
			for (int i = 0; i < renderTimes.Length; i++)
			{
				renderTimes[i].usedLastDraw = false;
			}
			for (int j = 0; j < drawTimes.Length; j++)
			{
				drawTimes[j].usedLastDraw = false;
			}
			for (int k = 0; k < lightingTimes.Length; k++)
			{
				lightingTimes[k].usedLastDraw = false;
			}
			if (startLoggingNextFrame)
			{
				startLoggingNextFrame = false;
				_ = DateTime.Now;
				string savePath = Main.SavePath;
				char directorySeparatorChar = Path.DirectorySeparatorChar;
				string path = savePath + directorySeparatorChar + "TerrariaDrawLog.7z";
				try
				{
					logWriter = new StreamWriter(new GZipStream(new FileStream(path, FileMode.Create), CompressionMode.Compress));
					logBuilder = new StringBuilder(5000);
					framesToLog = 600;
					currentFrame = 1;
					currentlyLogging = true;
				}
				catch
				{
					Main.NewText("Detailed logging could not be started.", 250, 250, 0);
				}
			}
			if (currentlyLogging)
			{
				logBuilder.AppendLine($"Start of Frame #{currentFrame}");
			}
			detailedDrawTimer.Restart();
			lastDetailedDrawTime = detailedDrawTimer.Elapsed.TotalMilliseconds;
		}

		public static void EndDrawFrame()
		{
			if (currentFrame <= framesToLog)
			{
				logBuilder.AppendLine($"End of Frame #{currentFrame}");
				logBuilder.AppendLine();
				if (endLoggingThisFrame)
				{
					endLoggingThisFrame = false;
					logBuilder.AppendLine("Logging ended early");
					currentFrame = framesToLog;
				}
				if (logBuilder.Length > 4000)
				{
					logWriter.Write(logBuilder.ToString());
					logBuilder.Clear();
				}
				currentFrame++;
				if (currentFrame > framesToLog)
				{
					Main.NewText("Detailed logging ended.", 250, 250, 0);
					logWriter.Write(logBuilder.ToString());
					logBuilder.Clear();
					logBuilder = null;
					logWriter.Flush();
					logWriter.Close();
					logWriter = null;
					framesToLog = -1;
					currentFrame = 0;
					currentlyLogging = false;
				}
			}
			detailedDrawTimer.Stop();
		}

		private static void UpdateTime(TimeLogData[] times, int type, double time)
		{
			bool flag = false;
			if (times[type].resetMaxTime > 0)
			{
				times[type].resetMaxTime--;
			}
			else
			{
				times[type].timeMax = 0f;
			}
			times[type].time = (float)time;
			if ((double)times[type].timeMax < time)
			{
				flag = true;
				times[type].timeMax = (float)time;
				times[type].resetMaxTime = 100;
			}
			times[type].usedLastDraw = true;
			if (currentFrame != 0)
			{
				logBuilder.AppendLine(string.Format("    {0} : {1:F4}ms {2}", times[type].logText, time, flag ? " - New Maximum" : string.Empty));
			}
		}

		public static void RenderTime(int renderType, double timeElapsed)
		{
			if (renderType >= 0 && renderType < renderTimes.Length)
			{
				UpdateTime(renderTimes, renderType, timeElapsed);
			}
		}

		public static float GetRenderTime(int renderType)
		{
			return renderTimes[renderType].time;
		}

		public static float GetRenderMax(int renderType)
		{
			return renderTimes[renderType].timeMax;
		}

		public static void DrawTime(int drawType, double timeElapsed)
		{
			if (drawType >= 0 && drawType < drawTimes.Length)
			{
				UpdateTime(drawTimes, drawType, timeElapsed);
			}
		}

		public static float GetDrawTime(int drawType)
		{
			return drawTimes[drawType].time;
		}

		public static float GetDrawTotal()
		{
			float num = 0f;
			for (int i = 0; i < drawTimes.Length; i++)
			{
				num += drawTimes[i].time;
			}
			return num;
		}

		public static void LightingTime(int lightingType, double timeElapsed)
		{
			if (lightingType >= 0 && lightingType < lightingTimes.Length)
			{
				UpdateTime(lightingTimes, lightingType, timeElapsed);
			}
		}

		public static float GetLightingTime(int lightingType)
		{
			return lightingTimes[lightingType].time;
		}

		public static float GetLightingTotal()
		{
			float num = 0f;
			for (int i = 0; i < lightingTimes.Length; i++)
			{
				num += lightingTimes[i].time;
			}
			return num;
		}

		public static void DetailedDrawReset()
		{
			lastDetailedDrawTime = detailedDrawTimer.Elapsed.TotalMilliseconds;
		}

		public static void DetailedDrawTime(int detailedDrawType)
		{
			if (detailedDrawType >= 0 && detailedDrawType < detailedDrawTimes.Length)
			{
				double totalMilliseconds = detailedDrawTimer.Elapsed.TotalMilliseconds;
				double time = totalMilliseconds - lastDetailedDrawTime;
				lastDetailedDrawTime = totalMilliseconds;
				UpdateTime(detailedDrawTimes, detailedDrawType, time);
			}
		}

		public static float GetDetailedDrawTime(int detailedDrawType)
		{
			return detailedDrawTimes[detailedDrawType].time;
		}

		public static float GetDetailedDrawTotal()
		{
			float num = 0f;
			for (int i = 0; i < detailedDrawTimes.Length; i++)
			{
				if (detailedDrawTimes[i].usedLastDraw)
				{
					num += detailedDrawTimes[i].time;
				}
			}
			return num;
		}

		public static void MenuDrawTime(double timeElapsed)
		{
			if (currentlyLogging)
			{
				logBuilder.AppendLine($"Menu Render Time : {timeElapsed:F4}");
			}
		}

		public static void SplashDrawTime(double timeElapsed)
		{
			if (currentlyLogging)
			{
				logBuilder.AppendLine($"Splash Render Time : {timeElapsed:F4}");
			}
		}

		public static void MapDrawTime(double timeElapsed)
		{
			if (currentlyLogging)
			{
				logBuilder.AppendLine($"Full Screen Map Render Time : {timeElapsed:F4}");
			}
		}

		public static void DrawException(Exception e)
		{
			if (currentlyLogging)
			{
				logBuilder.AppendLine(e.ToString());
			}
		}
	}
}
