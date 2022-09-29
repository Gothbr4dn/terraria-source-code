using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.IO;

namespace Terraria.GameContent.UI.Minimap
{
	public class MinimapFrameManager : SelectionHolder<MinimapFrame>
	{
		protected override void Configuration_OnLoad(Preferences obj)
		{
			ActiveSelectionConfigKey = Main.Configuration.Get("MinimapFrame", "Default");
		}

		protected override void Configuration_Save(Preferences obj)
		{
			obj.Put("MinimapFrame", ActiveSelectionConfigKey);
		}

		protected override void PopulateOptionsAndLoadContent(AssetRequestMode mode)
		{
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
			//IL_0132: Unknown result type (might be due to invalid IL or missing references)
			//IL_017a: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c2: Unknown result type (might be due to invalid IL or missing references)
			//IL_020a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0252: Unknown result type (might be due to invalid IL or missing references)
			//IL_029a: Unknown result type (might be due to invalid IL or missing references)
			float num = 2f;
			float num2 = 6f;
			CreateAndAdd("Default", new Vector2(-8f, -15f), new Vector2(148f + num, 234f + num2), new Vector2(200f + num, 234f + num2), new Vector2(174f + num, 234f + num2), mode);
			CreateAndAdd("Golden", new Vector2(-10f, -10f), new Vector2(136f, 248f), new Vector2(96f, 248f), new Vector2(116f, 248f), mode);
			CreateAndAdd("Remix", new Vector2(-10f, -10f), new Vector2(200f, 234f), new Vector2(148f, 234f), new Vector2(174f, 234f), mode);
			CreateAndAdd("Sticks", new Vector2(-10f, -10f), new Vector2(148f, 234f), new Vector2(200f, 234f), new Vector2(174f, 234f), mode);
			CreateAndAdd("StoneGold", new Vector2(-15f, -15f), new Vector2(220f, 244f), new Vector2(244f, 188f), new Vector2(244f, 216f), mode);
			CreateAndAdd("TwigLeaf", new Vector2(-20f, -20f), new Vector2(206f, 242f), new Vector2(162f, 242f), new Vector2(184f, 242f), mode);
			CreateAndAdd("Leaf", new Vector2(-20f, -20f), new Vector2(212f, 244f), new Vector2(168f, 246f), new Vector2(190f, 246f), mode);
			CreateAndAdd("Retro", new Vector2(-10f, -10f), new Vector2(150f, 236f), new Vector2(202f, 236f), new Vector2(176f, 236f), mode);
			CreateAndAdd("Valkyrie", new Vector2(-10f, -10f), new Vector2(154f, 242f), new Vector2(206f, 240f), new Vector2(180f, 244f), mode);
		}

		private void CreateAndAdd(string name, Vector2 frameOffset, Vector2 resetPosition, Vector2 zoomInPosition, Vector2 zoomOutPosition, AssetRequestMode mode)
		{
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			MinimapFrameTemplate minimapFrameTemplate = new MinimapFrameTemplate(name, frameOffset, resetPosition, zoomInPosition, zoomOutPosition);
			Options.Add(name, minimapFrameTemplate.CreateInstance(mode));
		}

		public void DrawTo(SpriteBatch spriteBatch, Vector2 position)
		{
			ActiveSelection.MinimapPosition = position;
			ActiveSelection.Update();
			ActiveSelection.DrawBackground(spriteBatch);
		}

		public void DrawForeground(SpriteBatch spriteBatch)
		{
			ActiveSelection.DrawForeground(spriteBatch);
		}
	}
}
