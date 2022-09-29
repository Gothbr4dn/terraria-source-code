using System.Collections.Generic;
using Terraria.DataStructures;

namespace Terraria
{
	public class Animation
	{
		private static List<Animation> _animations;

		private static Dictionary<Point16, Animation> _temporaryAnimations;

		private static List<Point16> _awaitingRemoval;

		private static List<Animation> _awaitingAddition;

		private bool _temporary;

		private Point16 _coordinates;

		private ushort _tileType;

		private int _frame;

		private int _frameMax;

		private int _frameCounter;

		private int _frameCounterMax;

		private int[] _frameData;

		public static void Initialize()
		{
			_animations = new List<Animation>();
			_temporaryAnimations = new Dictionary<Point16, Animation>();
			_awaitingRemoval = new List<Point16>();
			_awaitingAddition = new List<Animation>();
		}

		private void SetDefaults(int type)
		{
			_tileType = 0;
			_frame = 0;
			_frameMax = 0;
			_frameCounter = 0;
			_frameCounterMax = 0;
			_temporary = false;
			switch (type)
			{
			case 0:
			{
				_frameMax = 5;
				_frameCounterMax = 12;
				_frameData = new int[_frameMax];
				for (int l = 0; l < _frameMax; l++)
				{
					_frameData[l] = l + 1;
				}
				break;
			}
			case 1:
			{
				_frameMax = 5;
				_frameCounterMax = 12;
				_frameData = new int[_frameMax];
				for (int j = 0; j < _frameMax; j++)
				{
					_frameData[j] = 5 - j;
				}
				break;
			}
			case 2:
				_frameCounterMax = 6;
				_frameData = new int[5] { 1, 2, 2, 2, 1 };
				_frameMax = _frameData.Length;
				break;
			case 3:
			{
				_frameMax = 5;
				_frameCounterMax = 5;
				_frameData = new int[_frameMax];
				for (int k = 0; k < _frameMax; k++)
				{
					_frameData[k] = k;
				}
				break;
			}
			case 4:
			{
				_frameMax = 3;
				_frameCounterMax = 5;
				_frameData = new int[_frameMax];
				for (int i = 0; i < _frameMax; i++)
				{
					_frameData[i] = 9 + i;
				}
				break;
			}
			}
		}

		public static void NewTemporaryAnimation(int type, ushort tileType, int x, int y)
		{
			Point16 coordinates = new Point16(x, y);
			if (x >= 0 && x < Main.maxTilesX && y >= 0 && y < Main.maxTilesY)
			{
				Animation animation = new Animation();
				animation.SetDefaults(type);
				animation._tileType = tileType;
				animation._coordinates = coordinates;
				animation._temporary = true;
				_awaitingAddition.Add(animation);
				if (Main.netMode == 2)
				{
					NetMessage.SendTemporaryAnimation(-1, type, tileType, x, y);
				}
			}
		}

		private static void RemoveTemporaryAnimation(short x, short y)
		{
			Point16 point = new Point16(x, y);
			if (_temporaryAnimations.ContainsKey(point))
			{
				_awaitingRemoval.Add(point);
			}
		}

		public static void UpdateAll()
		{
			for (int i = 0; i < _animations.Count; i++)
			{
				_animations[i].Update();
			}
			if (_awaitingAddition.Count > 0)
			{
				for (int j = 0; j < _awaitingAddition.Count; j++)
				{
					Animation animation = _awaitingAddition[j];
					_temporaryAnimations[animation._coordinates] = animation;
				}
				_awaitingAddition.Clear();
			}
			foreach (KeyValuePair<Point16, Animation> temporaryAnimation in _temporaryAnimations)
			{
				temporaryAnimation.Value.Update();
			}
			if (_awaitingRemoval.Count > 0)
			{
				for (int k = 0; k < _awaitingRemoval.Count; k++)
				{
					_temporaryAnimations.Remove(_awaitingRemoval[k]);
				}
				_awaitingRemoval.Clear();
			}
		}

		public void Update()
		{
			if (_temporary)
			{
				Tile tile = Main.tile[_coordinates.X, _coordinates.Y];
				if (tile != null && tile.type != _tileType)
				{
					RemoveTemporaryAnimation(_coordinates.X, _coordinates.Y);
					return;
				}
			}
			_frameCounter++;
			if (_frameCounter < _frameCounterMax)
			{
				return;
			}
			_frameCounter = 0;
			_frame++;
			if (_frame >= _frameMax)
			{
				_frame = 0;
				if (_temporary)
				{
					RemoveTemporaryAnimation(_coordinates.X, _coordinates.Y);
				}
			}
		}

		public static bool GetTemporaryFrame(int x, int y, out int frameData)
		{
			Point16 key = new Point16(x, y);
			if (!_temporaryAnimations.TryGetValue(key, out var value))
			{
				frameData = 0;
				return false;
			}
			frameData = value._frameData[value._frame];
			return true;
		}
	}
}
