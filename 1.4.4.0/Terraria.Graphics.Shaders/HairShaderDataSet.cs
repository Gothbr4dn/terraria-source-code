using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace Terraria.Graphics.Shaders
{
	public class HairShaderDataSet
	{
		protected List<HairShaderData> _shaderData = new List<HairShaderData>();

		protected Dictionary<int, short> _shaderLookupDictionary = new Dictionary<int, short>();

		protected byte _shaderDataCount;

		public T BindShader<T>(int itemId, T shaderData) where T : HairShaderData
		{
			if (_shaderDataCount == byte.MaxValue)
			{
				throw new Exception("Too many shaders bound.");
			}
			_shaderLookupDictionary[itemId] = ++_shaderDataCount;
			_shaderData.Add(shaderData);
			return shaderData;
		}

		public void Apply(short shaderId, Player player, DrawData? drawData = null)
		{
			if (shaderId != 0 && shaderId <= _shaderDataCount)
			{
				_shaderData[shaderId - 1].Apply(player, drawData);
			}
			else
			{
				Main.pixelShader.CurrentTechnique.Passes[0].Apply();
			}
		}

		public Color GetColor(short shaderId, Player player, Color lightColor)
		{
			if (shaderId != 0 && shaderId <= _shaderDataCount)
			{
				return _shaderData[shaderId - 1].GetColor(player, lightColor);
			}
			return new Color(lightColor.ToVector4() * player.hairColor.ToVector4());
		}

		public HairShaderData GetShaderFromItemId(int type)
		{
			if (_shaderLookupDictionary.ContainsKey(type))
			{
				return _shaderData[_shaderLookupDictionary[type] - 1];
			}
			return null;
		}

		public short GetShaderIdFromItemId(int type)
		{
			if (_shaderLookupDictionary.ContainsKey(type))
			{
				return _shaderLookupDictionary[type];
			}
			return -1;
		}
	}
}
