using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;
using Terraria.DataStructures;
using Terraria.GameContent;
using Terraria.UI;

namespace Terraria.Map
{
	public class PingMapLayer : IMapLayer
	{
		private struct Ping
		{
			public readonly Vector2 Position;

			public readonly DateTime Time;

			public Ping(Vector2 position)
			{
				Position = position;
				Time = DateTime.Now;
			}
		}

		private const double PING_DURATION_IN_SECONDS = 15.0;

		private const double PING_FRAME_RATE = 10.0;

		private readonly SlotVector<Ping> _pings = new SlotVector<Ping>(100);

		public void Draw(ref MapOverlayDrawContext context, ref string text)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			SpriteFrame frame = new SpriteFrame(1, 5);
			DateTime now = DateTime.Now;
			foreach (ItemPair<Ping> item in (IEnumerable<ItemPair<Ping>>)_pings)
			{
				Ping value = item.Value;
				double totalSeconds = (now - value.Time).TotalSeconds;
				int num = (int)(totalSeconds * 10.0);
				frame.CurrentRow = (byte)(num % (int)frame.RowCount);
				context.Draw(TextureAssets.MapPing.get_Value(), value.Position, frame, Alignment.Center);
				if (totalSeconds > 15.0)
				{
					_pings.Remove(item.Id);
				}
			}
		}

		public void Add(Vector2 position)
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			if (_pings.get_Count() != _pings.get_Capacity())
			{
				_pings.Add(new Ping(position));
			}
		}
	}
}
