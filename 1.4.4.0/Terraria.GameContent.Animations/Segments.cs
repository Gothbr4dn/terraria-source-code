using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using Terraria.Graphics.Shaders;
using Terraria.Localization;
using Terraria.UI.Chat;

namespace Terraria.GameContent.Animations
{
	public class Segments
	{
		public class LocalizedTextSegment : IAnimationSegment
		{
			private const int PixelsForALine = 120;

			private LocalizedText _text;

			private float _timeToShowPeak;

			private Vector2 _anchorOffset;

			public float DedicatedTimeNeeded => 240f;

			public LocalizedTextSegment(float timeInAnimation, string textKey)
			{
				_text = Language.GetText(textKey);
				_timeToShowPeak = timeInAnimation;
			}

			public LocalizedTextSegment(float timeInAnimation, LocalizedText textObject, Vector2 anchorOffset)
			{
				_text = textObject;
				_timeToShowPeak = timeInAnimation;
				_anchorOffset = anchorOffset;
			}

			public void Draw(ref GameAnimationSegment info)
			{
				float num = 250f;
				float num2 = 250f;
				int timeInAnimation = info.TimeInAnimation;
				float num3 = Utils.GetLerpValue(_timeToShowPeak - num, _timeToShowPeak, timeInAnimation, clamped: true) * Utils.GetLerpValue(_timeToShowPeak + num2, _timeToShowPeak, timeInAnimation, clamped: true);
				if (!(num3 <= 0f))
				{
					float num4 = _timeToShowPeak - (float)timeInAnimation;
					Vector2 position = info.AnchorPositionOnScreen + new Vector2(0f, num4 * 0.5f);
					position += _anchorOffset;
					Vector2 baseScale = new Vector2(0.7f);
					float num5 = Main.GlobalTimeWrappedHourly * 0.02f % 1f;
					if (num5 < 0f)
					{
						num5 += 1f;
					}
					Color color = Main.hslToRgb(num5, 1f, 0.5f);
					string value = _text.Value;
					Vector2 origin = FontAssets.DeathText.get_Value().MeasureString(value);
					origin *= 0.5f;
					float num6 = 1f - (1f - num3) * (1f - num3);
					ChatManager.DrawColorCodedStringShadow(info.SpriteBatch, FontAssets.DeathText.get_Value(), value, position, color * num6 * num6 * 0.25f * info.DisplayOpacity, 0f, origin, baseScale);
					ChatManager.DrawColorCodedString(info.SpriteBatch, FontAssets.DeathText.get_Value(), value, position, Color.White * num6 * info.DisplayOpacity, 0f, origin, baseScale);
				}
			}
		}

		public abstract class AnimationSegmentWithActions<T> : IAnimationSegment
		{
			private int _dedicatedTimeNeeded;

			private int _lastDedicatedTimeNeeded;

			protected int _targetTime;

			private List<IAnimationSegmentAction<T>> _actions = new List<IAnimationSegmentAction<T>>();

			public float DedicatedTimeNeeded => _dedicatedTimeNeeded;

			public AnimationSegmentWithActions(int targetTime)
			{
				_targetTime = targetTime;
				_dedicatedTimeNeeded = 0;
			}

			protected void ProcessActions(T obj, float localTimeForObject)
			{
				for (int i = 0; i < _actions.Count; i++)
				{
					_actions[i].ApplyTo(obj, localTimeForObject);
				}
			}

			public AnimationSegmentWithActions<T> Then(IAnimationSegmentAction<T> act)
			{
				Bind(act);
				act.SetDelay(_dedicatedTimeNeeded);
				_actions.Add(act);
				_lastDedicatedTimeNeeded = _dedicatedTimeNeeded;
				_dedicatedTimeNeeded += act.ExpectedLengthOfActionInFrames;
				return this;
			}

			public AnimationSegmentWithActions<T> With(IAnimationSegmentAction<T> act)
			{
				Bind(act);
				act.SetDelay(_lastDedicatedTimeNeeded);
				_actions.Add(act);
				return this;
			}

			protected abstract void Bind(IAnimationSegmentAction<T> act);

			public abstract void Draw(ref GameAnimationSegment info);
		}

		public class PlayerSegment : AnimationSegmentWithActions<Player>
		{
			public interface IShaderEffect
			{
				void BeforeDrawing(ref GameAnimationSegment info);

				void AfterDrawing(ref GameAnimationSegment info);
			}

			public class ImmediateSpritebatchForPlayerDyesEffect : IShaderEffect
			{
				public void BeforeDrawing(ref GameAnimationSegment info)
				{
					info.SpriteBatch.End();
					info.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.CurrentFrameFlags.Hacks.CurrentBackgroundMatrixForCreditsRoll);
				}

				public void AfterDrawing(ref GameAnimationSegment info)
				{
					info.SpriteBatch.End();
					info.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, Main.CurrentFrameFlags.Hacks.CurrentBackgroundMatrixForCreditsRoll);
				}
			}

			private Player _player;

			private Vector2 _anchorOffset;

			private Vector2 _normalizedOriginForHitbox;

			private IShaderEffect _shaderEffect;

			private static Item _blankItem = new Item();

			public PlayerSegment(int targetTime, Vector2 anchorOffset, Vector2 normalizedHitboxOrigin)
				: base(targetTime)
			{
				_player = new Player();
				_anchorOffset = anchorOffset;
				_normalizedOriginForHitbox = normalizedHitboxOrigin;
			}

			public PlayerSegment UseShaderEffect(IShaderEffect shaderEffect)
			{
				_shaderEffect = shaderEffect;
				return this;
			}

			protected override void Bind(IAnimationSegmentAction<Player> act)
			{
				act.BindTo(_player);
			}

			public override void Draw(ref GameAnimationSegment info)
			{
				if ((float)info.TimeInAnimation > (float)_targetTime + base.DedicatedTimeNeeded || info.TimeInAnimation < _targetTime)
				{
					return;
				}
				ResetPlayerAnimation(ref info);
				float localTimeForObject = info.TimeInAnimation - _targetTime;
				ProcessActions(_player, localTimeForObject);
				if (info.DisplayOpacity != 0f)
				{
					_player.ResetEffects();
					_player.ResetVisibleAccessories();
					_player.UpdateMiscCounter();
					_player.UpdateDyes();
					_player.PlayerFrame();
					_player.socialIgnoreLight = true;
					_player.position += Main.screenPosition;
					_player.position -= new Vector2(_player.width / 2, _player.height);
					_player.opacityForAnimation *= info.DisplayOpacity;
					Item item = _player.inventory[_player.selectedItem];
					_player.inventory[_player.selectedItem] = _blankItem;
					float num = 1f - _player.opacityForAnimation;
					num = 0f;
					if (_shaderEffect != null)
					{
						_shaderEffect.BeforeDrawing(ref info);
					}
					Main.PlayerRenderer.DrawPlayer(Main.Camera, _player, _player.position, 0f, _player.fullRotationOrigin, num);
					if (_shaderEffect != null)
					{
						_shaderEffect.AfterDrawing(ref info);
					}
					_player.inventory[_player.selectedItem] = item;
				}
			}

			private void ResetPlayerAnimation(ref GameAnimationSegment info)
			{
				_player.CopyVisuals(Main.LocalPlayer);
				_player.position = info.AnchorPositionOnScreen + _anchorOffset;
				_player.opacityForAnimation = 1f;
			}
		}

		public class NPCSegment : AnimationSegmentWithActions<NPC>
		{
			private NPC _npc;

			private Vector2 _anchorOffset;

			private Vector2 _normalizedOriginForHitbox;

			public NPCSegment(int targetTime, int npcId, Vector2 anchorOffset, Vector2 normalizedNPCHitboxOrigin)
				: base(targetTime)
			{
				_npc = new NPC();
				_npc.SetDefaults(npcId, new NPCSpawnParams
				{
					gameModeData = Main.RegisteredGameModes[0],
					playerCountForMultiplayerDifficultyOverride = 1,
					sizeScaleOverride = null,
					strengthMultiplierOverride = 1f
				});
				_npc.IsABestiaryIconDummy = true;
				_anchorOffset = anchorOffset;
				_normalizedOriginForHitbox = normalizedNPCHitboxOrigin;
			}

			protected override void Bind(IAnimationSegmentAction<NPC> act)
			{
				act.BindTo(_npc);
			}

			public override void Draw(ref GameAnimationSegment info)
			{
				if ((float)info.TimeInAnimation > (float)_targetTime + base.DedicatedTimeNeeded || info.TimeInAnimation < _targetTime)
				{
					return;
				}
				ResetNPCAnimation(ref info);
				float localTimeForObject = info.TimeInAnimation - _targetTime;
				ProcessActions(_npc, localTimeForObject);
				if (_npc.alpha < 255)
				{
					_npc.FindFrame();
					if (_npc.townNPC && TownNPCProfiles.Instance.GetProfile(_npc.type, out var profile))
					{
						TextureAssets.Npc[_npc.type] = profile.GetTextureNPCShouldUse(_npc);
					}
					_npc.Opacity *= info.DisplayOpacity;
					Main.instance.DrawNPCDirect(info.SpriteBatch, _npc, _npc.behindTiles, Vector2.Zero);
				}
			}

			private void ResetNPCAnimation(ref GameAnimationSegment info)
			{
				_npc.position = info.AnchorPositionOnScreen + _anchorOffset - _npc.Size * _normalizedOriginForHitbox;
				_npc.alpha = 0;
				_npc.velocity = Vector2.Zero;
			}
		}

		public class LooseSprite
		{
			private DrawData _originalDrawData;

			private Asset<Texture2D> _asset;

			public DrawData CurrentDrawData;

			public float CurrentOpacity;

			public LooseSprite(DrawData data, Asset<Texture2D> asset)
			{
				_originalDrawData = data;
				_asset = asset;
				Reset();
			}

			public void Reset()
			{
				_originalDrawData.texture = _asset.get_Value();
				CurrentDrawData = _originalDrawData;
				CurrentOpacity = 1f;
			}
		}

		public class SpriteSegment : AnimationSegmentWithActions<LooseSprite>
		{
			public interface IShaderEffect
			{
				void BeforeDrawing(ref GameAnimationSegment info, ref DrawData drawData);

				void AfterDrawing(ref GameAnimationSegment info, ref DrawData drawData);
			}

			public class MaskedFadeEffect : IShaderEffect
			{
				public delegate Matrix GetMatrixAction();

				private readonly string _shaderKey;

				private readonly int _verticalFrameCount;

				private readonly int _verticalFrameWait;

				private Panning _panX;

				private Panning _panY;

				private GetMatrixAction _fetchMatrix;

				public MaskedFadeEffect(GetMatrixAction fetchMatrixMethod = null, string shaderKey = "MaskedFade", int verticalFrameCount = 1, int verticalFrameWait = 1)
				{
					_fetchMatrix = fetchMatrixMethod;
					_shaderKey = shaderKey;
					_verticalFrameCount = verticalFrameCount;
					if (verticalFrameWait < 1)
					{
						verticalFrameWait = 1;
					}
					_verticalFrameWait = verticalFrameWait;
					if (_fetchMatrix == null)
					{
						_fetchMatrix = DefaultFetchMatrix;
					}
				}

				private Matrix DefaultFetchMatrix()
				{
					return Main.CurrentFrameFlags.Hacks.CurrentBackgroundMatrixForCreditsRoll;
				}

				public void BeforeDrawing(ref GameAnimationSegment info, ref DrawData drawData)
				{
					info.SpriteBatch.End();
					info.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, Main.Rasterizer, null, _fetchMatrix());
					MiscShaderData miscShaderData = GameShaders.Misc[_shaderKey];
					miscShaderData.UseShaderSpecificData(new Vector4(y: (float)(info.TimeInAnimation / _verticalFrameWait % _verticalFrameCount) / (float)_verticalFrameCount, x: 1f / (float)_verticalFrameCount, z: _panX.GetPanAmount(info.TimeInAnimation), w: _panY.GetPanAmount(info.TimeInAnimation)));
					miscShaderData.Apply(drawData);
				}

				public MaskedFadeEffect WithPanX(Panning panning)
				{
					_panX = panning;
					return this;
				}

				public MaskedFadeEffect WithPanY(Panning panning)
				{
					_panY = panning;
					return this;
				}

				public void AfterDrawing(ref GameAnimationSegment info, ref DrawData drawData)
				{
					Main.pixelShader.CurrentTechnique.Passes[0].Apply();
					info.SpriteBatch.End();
					info.SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, Main.DefaultSamplerState, DepthStencilState.None, Main.Rasterizer, null, _fetchMatrix());
				}
			}

			private LooseSprite _sprite;

			private Vector2 _anchorOffset;

			private IShaderEffect _shaderEffect;

			public SpriteSegment(Asset<Texture2D> asset, int targetTime, DrawData data, Vector2 anchorOffset)
				: base(targetTime)
			{
				_sprite = new LooseSprite(data, asset);
				_anchorOffset = anchorOffset;
			}

			protected override void Bind(IAnimationSegmentAction<LooseSprite> act)
			{
				act.BindTo(_sprite);
			}

			public SpriteSegment UseShaderEffect(IShaderEffect shaderEffect)
			{
				_shaderEffect = shaderEffect;
				return this;
			}

			public override void Draw(ref GameAnimationSegment info)
			{
				if (!((float)info.TimeInAnimation > (float)_targetTime + base.DedicatedTimeNeeded) && info.TimeInAnimation >= _targetTime)
				{
					ResetSpriteAnimation(ref info);
					float localTimeForObject = info.TimeInAnimation - _targetTime;
					ProcessActions(_sprite, localTimeForObject);
					DrawData drawData = _sprite.CurrentDrawData;
					drawData.position += info.AnchorPositionOnScreen + _anchorOffset;
					drawData.color *= _sprite.CurrentOpacity * info.DisplayOpacity;
					if (_shaderEffect != null)
					{
						_shaderEffect.BeforeDrawing(ref info, ref drawData);
					}
					drawData.Draw(info.SpriteBatch);
					if (_shaderEffect != null)
					{
						_shaderEffect.AfterDrawing(ref info, ref drawData);
					}
				}
			}

			private void ResetSpriteAnimation(ref GameAnimationSegment info)
			{
				_sprite.Reset();
			}
		}

		public struct Panning
		{
			public float AmountOverTime;

			public float StartAmount;

			public float Delay;

			public float Duration;

			public float GetPanAmount(float time)
			{
				float num = MathHelper.Clamp((time - Delay) / Duration, 0f, 1f);
				return StartAmount + num * AmountOverTime;
			}
		}

		public class EmoteSegment : IAnimationSegment
		{
			private int _targetTime;

			private Vector2 _offset;

			private SpriteEffects _effect;

			private int _emoteId;

			private Vector2 _velocity;

			public float DedicatedTimeNeeded { get; private set; }

			public EmoteSegment(int emoteId, int targetTime, int timeToPlay, Vector2 position, SpriteEffects drawEffect, Vector2 velocity = default(Vector2))
			{
				_emoteId = emoteId;
				_targetTime = targetTime;
				_effect = drawEffect;
				_offset = position;
				_velocity = velocity;
				DedicatedTimeNeeded = timeToPlay;
			}

			public void Draw(ref GameAnimationSegment info)
			{
				int num = info.TimeInAnimation - _targetTime;
				if (num < 0 || (float)num >= DedicatedTimeNeeded)
				{
					return;
				}
				Vector2 vec = info.AnchorPositionOnScreen + _offset + _velocity * num;
				vec = vec.Floor();
				bool flag = num < 6 || (float)num >= DedicatedTimeNeeded - 6f;
				Texture2D value = TextureAssets.Extra[48].get_Value();
				Rectangle value2 = value.Frame(8, 39, (!flag) ? 1 : 0);
				Vector2 origin = new Vector2(value2.Width / 2, value2.Height);
				SpriteEffects spriteEffects = _effect;
				info.SpriteBatch.Draw(value, vec, value2, Color.White * info.DisplayOpacity, 0f, origin, 1f, spriteEffects, 0f);
				if (!flag)
				{
					int emoteId = _emoteId;
					if ((emoteId == 87 || emoteId == 89) && spriteEffects.HasFlag(SpriteEffects.FlipHorizontally))
					{
						spriteEffects &= ~SpriteEffects.FlipHorizontally;
						vec.X += 4f;
					}
					info.SpriteBatch.Draw(value, vec, GetFrame(num % 20), Color.White, 0f, origin, 1f, spriteEffects, 0f);
				}
			}

			private Rectangle GetFrame(int wrappedTime)
			{
				int num = ((wrappedTime >= 10) ? 1 : 0);
				return TextureAssets.Extra[48].get_Value().Frame(8, 39, _emoteId % 4 * 2 + num, _emoteId / 4 + 1);
			}
		}

		private const float PixelsToRollUpPerFrame = 0.5f;
	}
}
