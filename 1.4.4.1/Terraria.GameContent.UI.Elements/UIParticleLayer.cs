using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UIParticleLayer : UIElement
	{
		public ParticleRenderer ParticleSystem;

		public Vector2 AnchorPositionOffsetByPercents;

		public Vector2 AnchorPositionOffsetByPixels;

		public UIParticleLayer()
		{
			IgnoresMouseInteraction = true;
			ParticleSystem = new ParticleRenderer();
			base.OnUpdate += ParticleSystemUpdate;
		}

		private void ParticleSystemUpdate(UIElement affectedElement)
		{
			ParticleSystem.Update();
		}

		public override void Recalculate()
		{
			base.Recalculate();
			Rectangle r = GetDimensions().ToRectangle();
			ParticleSystem.Settings.AnchorPosition = r.TopLeft() + AnchorPositionOffsetByPercents * r.Size() + AnchorPositionOffsetByPixels;
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			ParticleSystem.Draw(spriteBatch);
		}

		public void AddParticle(IParticle particle)
		{
			ParticleSystem.Add(particle);
		}

		public void ClearParticles()
		{
			ParticleSystem.Clear();
		}
	}
}
