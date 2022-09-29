using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.CameraModifiers
{
	public class CameraModifierStack
	{
		private List<ICameraModifier> _modifiers = new List<ICameraModifier>();

		public void Add(ICameraModifier modifier)
		{
			RemoveIdenticalModifiers(modifier);
			_modifiers.Add(modifier);
		}

		private void RemoveIdenticalModifiers(ICameraModifier modifier)
		{
			string uniqueIdentity = modifier.UniqueIdentity;
			if (uniqueIdentity == null)
			{
				return;
			}
			for (int num = _modifiers.Count - 1; num >= 0; num--)
			{
				if (_modifiers[num].UniqueIdentity == uniqueIdentity)
				{
					_modifiers.RemoveAt(num);
				}
			}
		}

		public void ApplyTo(ref Vector2 cameraPosition)
		{
			CameraInfo cameraPosition2 = new CameraInfo(cameraPosition);
			ClearFinishedModifiers();
			for (int i = 0; i < _modifiers.Count; i++)
			{
				_modifiers[i].Update(ref cameraPosition2);
			}
			cameraPosition = cameraPosition2.CameraPosition;
		}

		private void ClearFinishedModifiers()
		{
			for (int num = _modifiers.Count - 1; num >= 0; num--)
			{
				if (_modifiers[num].Finished)
				{
					_modifiers.RemoveAt(num);
				}
			}
		}
	}
}
