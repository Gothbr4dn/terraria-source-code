using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.GameInput;

namespace Terraria.UI
{
	public class AchievementAdvisor
	{
		private List<AchievementAdvisorCard> _cards = new List<AchievementAdvisorCard>();

		private Asset<Texture2D> _achievementsTexture;

		private Asset<Texture2D> _achievementsBorderTexture;

		private Asset<Texture2D> _achievementsBorderMouseHoverFatTexture;

		private Asset<Texture2D> _achievementsBorderMouseHoverThinTexture;

		private AchievementAdvisorCard _hoveredCard;

		public bool CanDrawAboveCoins
		{
			get
			{
				if (Main.screenWidth >= 1000 && !PlayerInput.UsingGamepad)
				{
					return !PlayerInput.SteamDeckIsUsed;
				}
				return false;
			}
		}

		public void LoadContent()
		{
			_achievementsTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievements", (AssetRequestMode)1);
			_achievementsBorderTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders", (AssetRequestMode)1);
			_achievementsBorderMouseHoverFatTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders_MouseHover", (AssetRequestMode)1);
			_achievementsBorderMouseHoverThinTexture = Main.Assets.Request<Texture2D>("Images/UI/Achievement_Borders_MouseHoverThin", (AssetRequestMode)1);
		}

		public void Draw(SpriteBatch spriteBatch)
		{
		}

		public void DrawOneAchievement(SpriteBatch spriteBatch, Vector2 position, bool large)
		{
			List<AchievementAdvisorCard> bestCards = GetBestCards(1);
			if (bestCards.Count < 1)
			{
				return;
			}
			AchievementAdvisorCard hoveredCard = bestCards[0];
			float num = 0.35f;
			if (large)
			{
				num = 0.75f;
			}
			_hoveredCard = null;
			DrawCard(bestCards[0], spriteBatch, position + new Vector2(8f) * num, num, out var hovered);
			if (!hovered)
			{
				return;
			}
			_hoveredCard = hoveredCard;
			if (!PlayerInput.IgnoreMouseInterface)
			{
				Main.player[Main.myPlayer].mouseInterface = true;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Main.ingameOptionsWindow = false;
					IngameFancyUI.OpenAchievementsAndGoto(_hoveredCard.achievement);
				}
			}
		}

		public void Update()
		{
			_hoveredCard = null;
		}

		public void DrawOptionsPanel(SpriteBatch spriteBatch, Vector2 leftPosition, Vector2 rightPosition)
		{
			List<AchievementAdvisorCard> bestCards = GetBestCards();
			_hoveredCard = null;
			int num = bestCards.Count;
			if (num > 5)
			{
				num = 5;
			}
			bool hovered;
			for (int i = 0; i < num; i++)
			{
				DrawCard(bestCards[i], spriteBatch, leftPosition + new Vector2(42 * i, 0f), 0.5f, out hovered);
				if (hovered)
				{
					_hoveredCard = bestCards[i];
				}
			}
			for (int j = 5; j < bestCards.Count; j++)
			{
				DrawCard(bestCards[j], spriteBatch, rightPosition + new Vector2(42 * j, 0f), 0.5f, out hovered);
				if (hovered)
				{
					_hoveredCard = bestCards[j];
				}
			}
			if (_hoveredCard == null)
			{
				return;
			}
			if (_hoveredCard.achievement.IsCompleted)
			{
				_hoveredCard = null;
			}
			else if (!PlayerInput.IgnoreMouseInterface)
			{
				Main.player[Main.myPlayer].mouseInterface = true;
				if (Main.mouseLeft && Main.mouseLeftRelease)
				{
					Main.ingameOptionsWindow = false;
					IngameFancyUI.OpenAchievementsAndGoto(_hoveredCard.achievement);
				}
			}
		}

		public void DrawMouseHover()
		{
			if (_hoveredCard != null)
			{
				Main.spriteBatch.End();
				Main.spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, Main.UIScaleMatrix);
				PlayerInput.SetZoom_UI();
				Item item = new Item();
				item.SetDefaults(0, noMatCheck: true);
				item.SetNameOverride(_hoveredCard.achievement.FriendlyName.Value);
				item.ToolTip = ItemTooltip.FromLanguageKey(_hoveredCard.achievement.Description.Key);
				item.type = 1;
				item.scale = 0f;
				item.rare = 10;
				item.value = -1;
				Main.HoverItem = item;
				Main.instance.MouseText("", 0, 0);
				Main.mouseText = true;
			}
		}

		private void DrawCard(AchievementAdvisorCard card, SpriteBatch spriteBatch, Vector2 position, float scale, out bool hovered)
		{
			hovered = false;
			if (Main.MouseScreen.Between(position, position + card.frame.Size() * scale))
			{
				Main.LocalPlayer.mouseInterface = true;
				hovered = true;
			}
			Color color = Color.White;
			if (!hovered)
			{
				color = new Color(220, 220, 220, 220);
			}
			Vector2 vector = new Vector2(-4f) * scale;
			Vector2 vector2 = new Vector2(-8f) * scale;
			Texture2D value = _achievementsBorderMouseHoverFatTexture.get_Value();
			if (scale > 0.5f)
			{
				value = _achievementsBorderMouseHoverThinTexture.get_Value();
				vector2 = new Vector2(-5f) * scale;
			}
			Rectangle frame = card.frame;
			frame.X += 528;
			spriteBatch.Draw(_achievementsTexture.get_Value(), position, frame, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			spriteBatch.Draw(_achievementsBorderTexture.get_Value(), position + vector, null, color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			if (hovered)
			{
				spriteBatch.Draw(value, position + vector2, null, Main.OurFavoriteColor, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
			}
		}

		private List<AchievementAdvisorCard> GetBestCards(int cardsAmount = 10)
		{
			List<AchievementAdvisorCard> list = new List<AchievementAdvisorCard>();
			for (int i = 0; i < _cards.Count; i++)
			{
				AchievementAdvisorCard achievementAdvisorCard = _cards[i];
				if (!achievementAdvisorCard.achievement.IsCompleted && achievementAdvisorCard.IsAchievableInWorld())
				{
					list.Add(achievementAdvisorCard);
					if (list.Count >= cardsAmount)
					{
						break;
					}
				}
			}
			return list;
		}

		public void Initialize()
		{
			float num = 1f;
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("TIMBER"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("BENCHED"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("OBTAIN_HAMMER"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("NO_HOBO"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("YOU_CAN_DO_IT"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("OOO_SHINY"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("HEAVY_METAL"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("MATCHING_ATTIRE"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("HEART_BREAKER"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("I_AM_LOOT"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("HOLD_ON_TIGHT"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("STAR_POWER"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("EYE_ON_YOU"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("SMASHING_POPPET"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("WHERES_MY_HONEY"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("STING_OPERATION"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("BONED"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("DUNGEON_HEIST"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("ITS_GETTING_HOT_IN_HERE"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("MINER_FOR_FIRE"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("STILL_HUNGRY"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("ITS_HARD"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("BEGONE_EVIL"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("EXTRA_SHINY"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("HEAD_IN_THE_CLOUDS"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("BUCKETS_OF_BOLTS"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("DRAX_ATTAX"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("PHOTOSYNTHESIS"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("GET_A_LIFE"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("THE_GREAT_SOUTHERN_PLANTKILL"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("TEMPLE_RAIDER"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("LIHZAHRDIAN_IDOL"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("ROBBING_THE_GRAVE"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("OBSESSIVE_DEVOTION"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("STAR_DESTROYER"), num++));
			_cards.Add(new AchievementAdvisorCard(Main.Achievements.GetAchievement("CHAMPION_OF_TERRARIA"), num++));
			_cards.OrderBy((AchievementAdvisorCard x) => x.order);
		}
	}
}
