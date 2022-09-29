using System.Collections.Generic;
using Microsoft.Xna.Framework;
using ReLogic.Utilities;

namespace Terraria.Audio
{
	public class SoundPlayer
	{
		private readonly SlotVector<ActiveSound> _trackedSounds = new SlotVector<ActiveSound>(4096);

		public SlotId Play(SoundStyle style, Vector2 position)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_0059: Unknown result type (might be due to invalid IL or missing references)
			if (Main.dedServ || style == null || !style.IsTrackable)
			{
				return SlotId.Invalid;
			}
			if (Vector2.DistanceSquared(Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), position) > 100000000f)
			{
				return SlotId.Invalid;
			}
			ActiveSound activeSound = new ActiveSound(style, position);
			return _trackedSounds.Add(activeSound);
		}

		public SlotId PlayLooped(SoundStyle style, Vector2 position, ActiveSound.LoopedPlayCondition loopingCondition)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0044: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			if (Main.dedServ || style == null || !style.IsTrackable)
			{
				return SlotId.Invalid;
			}
			if (Vector2.DistanceSquared(Main.screenPosition + new Vector2(Main.screenWidth / 2, Main.screenHeight / 2), position) > 100000000f)
			{
				return SlotId.Invalid;
			}
			ActiveSound activeSound = new ActiveSound(style, position, loopingCondition);
			return _trackedSounds.Add(activeSound);
		}

		public void Reload()
		{
			StopAll();
		}

		public SlotId Play(SoundStyle style)
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			if (Main.dedServ || style == null || !style.IsTrackable)
			{
				return SlotId.Invalid;
			}
			ActiveSound activeSound = new ActiveSound(style);
			return _trackedSounds.Add(activeSound);
		}

		public ActiveSound GetActiveSound(SlotId id)
		{
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			if (!_trackedSounds.Has(id))
			{
				return null;
			}
			return _trackedSounds.get_Item(id);
		}

		public void PauseAll()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			foreach (ItemPair<ActiveSound> item in (IEnumerable<ItemPair<ActiveSound>>)_trackedSounds)
			{
				item.Value.Pause();
			}
		}

		public void ResumeAll()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			foreach (ItemPair<ActiveSound> item in (IEnumerable<ItemPair<ActiveSound>>)_trackedSounds)
			{
				item.Value.Resume();
			}
		}

		public void StopAll()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			foreach (ItemPair<ActiveSound> item in (IEnumerable<ItemPair<ActiveSound>>)_trackedSounds)
			{
				item.Value.Stop();
			}
			_trackedSounds.Clear();
		}

		public void Update()
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0020: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0034: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_0049: Unknown result type (might be due to invalid IL or missing references)
			foreach (ItemPair<ActiveSound> item in (IEnumerable<ItemPair<ActiveSound>>)_trackedSounds)
			{
				try
				{
					item.Value.Update();
					if (!item.Value.IsPlaying)
					{
						_trackedSounds.Remove(item.Id);
					}
				}
				catch
				{
					_trackedSounds.Remove(item.Id);
				}
			}
		}

		public ActiveSound FindActiveSound(SoundStyle style)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			foreach (ItemPair<ActiveSound> item in (IEnumerable<ItemPair<ActiveSound>>)_trackedSounds)
			{
				if (item.Value.Style == style)
				{
					return item.Value;
				}
			}
			return null;
		}
	}
}
