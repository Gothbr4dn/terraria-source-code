namespace Terraria
{
	public class Sign
	{
		public const int maxSigns = 1000;

		public int x;

		public int y;

		public string text;

		public static void KillSign(int x, int y)
		{
			for (int i = 0; i < 1000; i++)
			{
				if (Main.sign[i] != null && Main.sign[i].x == x && Main.sign[i].y == y)
				{
					Main.sign[i] = null;
				}
			}
		}

		public static int ReadSign(int i, int j, bool CreateIfMissing = true)
		{
			int num = Main.tile[i, j].frameX / 18;
			int num2 = Main.tile[i, j].frameY / 18;
			num %= 2;
			int num3 = i - num;
			int num4 = j - num2;
			if (!Main.tileSign[Main.tile[num3, num4].type])
			{
				KillSign(num3, num4);
				return -1;
			}
			int num5 = -1;
			for (int k = 0; k < 1000; k++)
			{
				if (Main.sign[k] != null && Main.sign[k].x == num3 && Main.sign[k].y == num4)
				{
					num5 = k;
					break;
				}
			}
			if (num5 < 0 && CreateIfMissing)
			{
				for (int l = 0; l < 1000; l++)
				{
					if (Main.sign[l] == null)
					{
						num5 = l;
						Main.sign[l] = new Sign();
						Main.sign[l].x = num3;
						Main.sign[l].y = num4;
						Main.sign[l].text = "";
						break;
					}
				}
			}
			return num5;
		}

		public static void TextSign(int i, string text)
		{
			if (Main.tile[Main.sign[i].x, Main.sign[i].y] == null || !Main.tile[Main.sign[i].x, Main.sign[i].y].active() || !Main.tileSign[Main.tile[Main.sign[i].x, Main.sign[i].y].type])
			{
				Main.sign[i] = null;
			}
			else
			{
				Main.sign[i].text = text;
			}
		}

		public override string ToString()
		{
			return "x" + x + "\ty" + y + "\t" + text;
		}
	}
}
