using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace Terraria.GameContent
{
	public class TilePaintSystemV2
	{
		public abstract class ARenderTargetHolder
		{
			public RenderTarget2D Target;

			protected bool _wasPrepared;

			public bool IsReady => _wasPrepared;

			public abstract void Prepare();

			public abstract void PrepareShader();

			public void Clear()
			{
				if (Target != null && !Target.IsDisposed)
				{
					Target.Dispose();
				}
			}

			protected void PrepareTextureIfNecessary(Texture2D originalTexture, Rectangle? sourceRect = null)
			{
				if (Target == null || Target.IsContentLost)
				{
					Main instance = Main.instance;
					if (!sourceRect.HasValue)
					{
						sourceRect = originalTexture.Frame();
					}
					Target = new RenderTarget2D(instance.GraphicsDevice, sourceRect.Value.Width, sourceRect.Value.Height, mipMap: false, instance.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
					Target.ContentLost += Target_ContentLost;
					Target.Disposing += Target_Disposing;
					instance.GraphicsDevice.SetRenderTarget(Target);
					instance.GraphicsDevice.Clear(Color.Transparent);
					Main.spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
					PrepareShader();
					Rectangle value = sourceRect.Value;
					value.X = 0;
					value.Y = 0;
					Main.spriteBatch.Draw(originalTexture, value, Color.White);
					Main.spriteBatch.End();
					instance.GraphicsDevice.SetRenderTarget(null);
					_wasPrepared = true;
				}
			}

			private void Target_Disposing(object sender, EventArgs e)
			{
				_wasPrepared = false;
				Target = null;
			}

			private void Target_ContentLost(object sender, EventArgs e)
			{
				_wasPrepared = false;
			}

			protected void PrepareShader(int paintColor, TreePaintingSettings settings)
			{
				Effect tileShader = Main.tileShader;
				tileShader.Parameters["leafHueTestOffset"].SetValue(settings.HueTestOffset);
				tileShader.Parameters["leafMinHue"].SetValue(settings.SpecialGroupMinimalHueValue);
				tileShader.Parameters["leafMaxHue"].SetValue(settings.SpecialGroupMaximumHueValue);
				tileShader.Parameters["leafMinSat"].SetValue(settings.SpecialGroupMinimumSaturationValue);
				tileShader.Parameters["leafMaxSat"].SetValue(settings.SpecialGroupMaximumSaturationValue);
				tileShader.Parameters["invertSpecialGroupResult"].SetValue(settings.InvertSpecialGroupResult);
				int index = Main.ConvertPaintIdToTileShaderIndex(paintColor, settings.UseSpecialGroups, settings.UseWallShaderHacks);
				tileShader.CurrentTechnique.Passes[index].Apply();
			}
		}

		public class TreeTopRenderTargetHolder : ARenderTargetHolder
		{
			public TreeFoliageVariantKey Key;

			public override void Prepare()
			{
				Asset<Texture2D> val = Main.Assets.Request<Texture2D>(TextureAssets.TreeTop[Key.TextureIndex].get_Name(), (AssetRequestMode)1);
				PrepareTextureIfNecessary(val.get_Value());
			}

			public override void PrepareShader()
			{
				PrepareShader(Key.PaintColor, TreePaintSystemData.GetTreeFoliageSettings(Key.TextureIndex, Key.TextureStyle));
			}
		}

		public class TreeBranchTargetHolder : ARenderTargetHolder
		{
			public TreeFoliageVariantKey Key;

			public override void Prepare()
			{
				Asset<Texture2D> val = Main.Assets.Request<Texture2D>(TextureAssets.TreeBranch[Key.TextureIndex].get_Name(), (AssetRequestMode)1);
				PrepareTextureIfNecessary(val.get_Value());
			}

			public override void PrepareShader()
			{
				PrepareShader(Key.PaintColor, TreePaintSystemData.GetTreeFoliageSettings(Key.TextureIndex, Key.TextureStyle));
			}
		}

		public class TileRenderTargetHolder : ARenderTargetHolder
		{
			public TileVariationkey Key;

			public override void Prepare()
			{
				Asset<Texture2D> val = Main.Assets.Request<Texture2D>(TextureAssets.Tile[Key.TileType].get_Name(), (AssetRequestMode)1);
				PrepareTextureIfNecessary(val.get_Value());
			}

			public override void PrepareShader()
			{
				PrepareShader(Key.PaintColor, TreePaintSystemData.GetTileSettings(Key.TileType, Key.TileStyle));
			}
		}

		public class WallRenderTargetHolder : ARenderTargetHolder
		{
			public WallVariationKey Key;

			public override void Prepare()
			{
				Asset<Texture2D> val = Main.Assets.Request<Texture2D>(TextureAssets.Wall[Key.WallType].get_Name(), (AssetRequestMode)1);
				PrepareTextureIfNecessary(val.get_Value());
			}

			public override void PrepareShader()
			{
				PrepareShader(Key.PaintColor, TreePaintSystemData.GetWallSettings(Key.WallType));
			}
		}

		public struct TileVariationkey
		{
			public int TileType;

			public int TileStyle;

			public int PaintColor;

			public bool Equals(TileVariationkey other)
			{
				if (TileType == other.TileType && TileStyle == other.TileStyle)
				{
					return PaintColor == other.PaintColor;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj is TileVariationkey)
				{
					return Equals((TileVariationkey)obj);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (((TileType * 397) ^ TileStyle) * 397) ^ PaintColor;
			}

			public static bool operator ==(TileVariationkey left, TileVariationkey right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(TileVariationkey left, TileVariationkey right)
			{
				return !left.Equals(right);
			}
		}

		public struct WallVariationKey
		{
			public int WallType;

			public int PaintColor;

			public bool Equals(WallVariationKey other)
			{
				if (WallType == other.WallType)
				{
					return PaintColor == other.PaintColor;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj is WallVariationKey)
				{
					return Equals((WallVariationKey)obj);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (WallType * 397) ^ PaintColor;
			}

			public static bool operator ==(WallVariationKey left, WallVariationKey right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(WallVariationKey left, WallVariationKey right)
			{
				return !left.Equals(right);
			}
		}

		public struct TreeFoliageVariantKey
		{
			public int TextureIndex;

			public int TextureStyle;

			public int PaintColor;

			public bool Equals(TreeFoliageVariantKey other)
			{
				if (TextureIndex == other.TextureIndex && TextureStyle == other.TextureStyle)
				{
					return PaintColor == other.PaintColor;
				}
				return false;
			}

			public override bool Equals(object obj)
			{
				if (obj is TreeFoliageVariantKey)
				{
					return Equals((TreeFoliageVariantKey)obj);
				}
				return false;
			}

			public override int GetHashCode()
			{
				return (((TextureIndex * 397) ^ TextureStyle) * 397) ^ PaintColor;
			}

			public static bool operator ==(TreeFoliageVariantKey left, TreeFoliageVariantKey right)
			{
				return left.Equals(right);
			}

			public static bool operator !=(TreeFoliageVariantKey left, TreeFoliageVariantKey right)
			{
				return !left.Equals(right);
			}
		}

		private Dictionary<TileVariationkey, TileRenderTargetHolder> _tilesRenders = new Dictionary<TileVariationkey, TileRenderTargetHolder>();

		private Dictionary<WallVariationKey, WallRenderTargetHolder> _wallsRenders = new Dictionary<WallVariationKey, WallRenderTargetHolder>();

		private Dictionary<TreeFoliageVariantKey, TreeTopRenderTargetHolder> _treeTopRenders = new Dictionary<TreeFoliageVariantKey, TreeTopRenderTargetHolder>();

		private Dictionary<TreeFoliageVariantKey, TreeBranchTargetHolder> _treeBranchRenders = new Dictionary<TreeFoliageVariantKey, TreeBranchTargetHolder>();

		private List<ARenderTargetHolder> _requests = new List<ARenderTargetHolder>();

		public void Reset()
		{
			foreach (TileRenderTargetHolder value in _tilesRenders.Values)
			{
				value.Clear();
			}
			_tilesRenders.Clear();
			foreach (WallRenderTargetHolder value2 in _wallsRenders.Values)
			{
				value2.Clear();
			}
			_wallsRenders.Clear();
			foreach (TreeTopRenderTargetHolder value3 in _treeTopRenders.Values)
			{
				value3.Clear();
			}
			_treeTopRenders.Clear();
			foreach (TreeBranchTargetHolder value4 in _treeBranchRenders.Values)
			{
				value4.Clear();
			}
			_treeBranchRenders.Clear();
			foreach (ARenderTargetHolder request in _requests)
			{
				request.Clear();
			}
			_requests.Clear();
		}

		public void RequestTile(ref TileVariationkey lookupKey)
		{
			if (!_tilesRenders.TryGetValue(lookupKey, out var value))
			{
				value = new TileRenderTargetHolder
				{
					Key = lookupKey
				};
				_tilesRenders.Add(lookupKey, value);
			}
			if (!value.IsReady)
			{
				_requests.Add(value);
			}
		}

		private void RequestTile_CheckForRelatedTileRequests(ref TileVariationkey lookupKey)
		{
			if (lookupKey.TileType == 83)
			{
				TileVariationkey tileVariationkey = default(TileVariationkey);
				tileVariationkey.TileType = 84;
				tileVariationkey.TileStyle = lookupKey.TileStyle;
				tileVariationkey.PaintColor = lookupKey.PaintColor;
				TileVariationkey lookupKey2 = tileVariationkey;
				RequestTile(ref lookupKey2);
			}
		}

		public void RequestWall(ref WallVariationKey lookupKey)
		{
			if (!_wallsRenders.TryGetValue(lookupKey, out var value))
			{
				value = new WallRenderTargetHolder
				{
					Key = lookupKey
				};
				_wallsRenders.Add(lookupKey, value);
			}
			if (!value.IsReady)
			{
				_requests.Add(value);
			}
		}

		public void RequestTreeTop(ref TreeFoliageVariantKey lookupKey)
		{
			if (!_treeTopRenders.TryGetValue(lookupKey, out var value))
			{
				value = new TreeTopRenderTargetHolder
				{
					Key = lookupKey
				};
				_treeTopRenders.Add(lookupKey, value);
			}
			if (!value.IsReady)
			{
				_requests.Add(value);
			}
		}

		public void RequestTreeBranch(ref TreeFoliageVariantKey lookupKey)
		{
			if (!_treeBranchRenders.TryGetValue(lookupKey, out var value))
			{
				value = new TreeBranchTargetHolder
				{
					Key = lookupKey
				};
				_treeBranchRenders.Add(lookupKey, value);
			}
			if (!value.IsReady)
			{
				_requests.Add(value);
			}
		}

		public Texture2D TryGetTileAndRequestIfNotReady(int tileType, int tileStyle, int paintColor)
		{
			TileVariationkey tileVariationkey = default(TileVariationkey);
			tileVariationkey.TileType = tileType;
			tileVariationkey.TileStyle = tileStyle;
			tileVariationkey.PaintColor = paintColor;
			TileVariationkey lookupKey = tileVariationkey;
			if (_tilesRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
			{
				return value.Target;
			}
			RequestTile(ref lookupKey);
			return null;
		}

		public Texture2D TryGetWallAndRequestIfNotReady(int wallType, int paintColor)
		{
			WallVariationKey wallVariationKey = default(WallVariationKey);
			wallVariationKey.WallType = wallType;
			wallVariationKey.PaintColor = paintColor;
			WallVariationKey lookupKey = wallVariationKey;
			if (_wallsRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
			{
				return value.Target;
			}
			RequestWall(ref lookupKey);
			return null;
		}

		public Texture2D TryGetTreeTopAndRequestIfNotReady(int treeTopIndex, int treeTopStyle, int paintColor)
		{
			TreeFoliageVariantKey treeFoliageVariantKey = default(TreeFoliageVariantKey);
			treeFoliageVariantKey.TextureIndex = treeTopIndex;
			treeFoliageVariantKey.TextureStyle = treeTopStyle;
			treeFoliageVariantKey.PaintColor = paintColor;
			TreeFoliageVariantKey lookupKey = treeFoliageVariantKey;
			if (_treeTopRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
			{
				return value.Target;
			}
			RequestTreeTop(ref lookupKey);
			return null;
		}

		public Texture2D TryGetTreeBranchAndRequestIfNotReady(int treeTopIndex, int treeTopStyle, int paintColor)
		{
			TreeFoliageVariantKey treeFoliageVariantKey = default(TreeFoliageVariantKey);
			treeFoliageVariantKey.TextureIndex = treeTopIndex;
			treeFoliageVariantKey.TextureStyle = treeTopStyle;
			treeFoliageVariantKey.PaintColor = paintColor;
			TreeFoliageVariantKey lookupKey = treeFoliageVariantKey;
			if (_treeBranchRenders.TryGetValue(lookupKey, out var value) && value.IsReady)
			{
				return value.Target;
			}
			RequestTreeBranch(ref lookupKey);
			return null;
		}

		public void PrepareAllRequests()
		{
			if (_requests.Count != 0)
			{
				for (int i = 0; i < _requests.Count; i++)
				{
					_requests[i].Prepare();
				}
				_requests.Clear();
			}
		}
	}
}
