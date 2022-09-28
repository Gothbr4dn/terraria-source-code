using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.ID;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class UICharacter : UIElement
	{
		private Player _player;

		private Projectile[] _petProjectiles;

		private Asset<Texture2D> _texture;

		private static Item _blankItem = new Item();

		private bool _animated;

		private bool _drawsBackPanel;

		private float _characterScale = 1f;

		private int _animationCounter;

		private static readonly Projectile[] NoPets = new Projectile[0];

		public bool IsAnimated => _animated;

		public UICharacter(Player player, bool animated = false, bool hasBackPanel = true, float characterScale = 1f, bool useAClone = false)
		{
			_player = player;
			if (useAClone)
			{
				_player = player.SerializedClone();
				_player.PlayerFrame();
			}
			Width.Set(59f, 0f);
			Height.Set(58f, 0f);
			_texture = Main.Assets.Request<Texture2D>("Images/UI/PlayerBackground", (AssetRequestMode)1);
			UseImmediateMode = true;
			_animated = animated;
			_drawsBackPanel = hasBackPanel;
			_characterScale = characterScale;
			OverrideSamplerState = SamplerState.PointClamp;
			_petProjectiles = NoPets;
			PreparePetProjectiles();
		}

		private void PreparePetProjectiles()
		{
			if (!_player.hideMisc[0])
			{
				Item item = _player.miscEquips[0];
				if (!item.IsAir)
				{
					int shoot = item.shoot;
					_petProjectiles = new Projectile[1] { PreparePetProjectiles_CreatePetProjectileDummy(shoot) };
				}
			}
		}

		private Projectile PreparePetProjectiles_CreatePetProjectileDummy(int projectileId)
		{
			Projectile projectile = new Projectile();
			projectile.SetDefaults(projectileId);
			projectile.isAPreviewDummy = true;
			return projectile;
		}

		public override void Update(GameTime gameTime)
		{
			_player.ResetEffects();
			_player.ResetVisibleAccessories();
			_player.UpdateMiscCounter();
			_player.UpdateDyes();
			_player.PlayerFrame();
			if (_animated)
			{
				_animationCounter++;
			}
			base.Update(gameTime);
		}

		private void UpdateAnim()
		{
			if (!_animated)
			{
				_player.bodyFrame.Y = (_player.legFrame.Y = (_player.headFrame.Y = 0));
				return;
			}
			int num = (int)(Main.GlobalTimeWrappedHourly / 0.07f) % 14 + 6;
			_player.bodyFrame.Y = (_player.legFrame.Y = (_player.headFrame.Y = num * 56));
			_player.WingFrame(wingFlap: false);
		}

		protected override void DrawSelf(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			if (_drawsBackPanel)
			{
				spriteBatch.Draw(_texture.get_Value(), dimensions.Position(), Color.White);
			}
			UpdateAnim();
			DrawPets(spriteBatch);
			Vector2 playerPosition = GetPlayerPosition(ref dimensions);
			Item item = _player.inventory[_player.selectedItem];
			_player.inventory[_player.selectedItem] = _blankItem;
			Main.PlayerRenderer.DrawPlayer(Main.Camera, _player, playerPosition + Main.screenPosition, 0f, Vector2.Zero, 0f, _characterScale);
			_player.inventory[_player.selectedItem] = item;
		}

		private Vector2 GetPlayerPosition(ref CalculatedStyle dimensions)
		{
			Vector2 result = dimensions.Position() + new Vector2(dimensions.Width * 0.5f - (float)(_player.width >> 1), dimensions.Height * 0.5f - (float)(_player.height >> 1));
			if (_petProjectiles.Length != 0)
			{
				result.X -= 10f;
			}
			return result;
		}

		public void DrawPets(SpriteBatch spriteBatch)
		{
			CalculatedStyle dimensions = GetDimensions();
			Vector2 playerPosition = GetPlayerPosition(ref dimensions);
			for (int i = 0; i < _petProjectiles.Length; i++)
			{
				Projectile projectile = _petProjectiles[i];
				Vector2 vector = playerPosition + new Vector2(0f, _player.height) + new Vector2(20f, 0f) + new Vector2(0f, -projectile.height);
				projectile.position = vector + Main.screenPosition;
				projectile.velocity = new Vector2(0.1f, 0f);
				projectile.direction = 1;
				projectile.owner = Main.myPlayer;
				ProjectileID.Sets.CharacterPreviewAnimations[projectile.type].ApplyTo(projectile, _animated);
				Player player = Main.player[Main.myPlayer];
				Main.player[Main.myPlayer] = _player;
				Main.instance.DrawProjDirect(projectile);
				Main.player[Main.myPlayer] = player;
			}
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Immediate, spriteBatch.GraphicsDevice.BlendState, spriteBatch.GraphicsDevice.SamplerStates[0], spriteBatch.GraphicsDevice.DepthStencilState, spriteBatch.GraphicsDevice.RasterizerState, null, Main.UIScaleMatrix);
		}

		public void SetAnimated(bool animated)
		{
			_animated = animated;
		}
	}
}
