using System;
using Microsoft.Xna.Framework;

namespace Terraria.Graphics.CameraModifiers
{
	public class PunchCameraModifier : ICameraModifier
	{
		private int _framesToLast;

		private Vector2 _startPosition;

		private Vector2 _direction;

		private float _distanceFalloff;

		private float _strength;

		private float _vibrationCyclesPerSecond;

		private int _framesLasted;

		public string UniqueIdentity { get; private set; }

		public bool Finished { get; private set; }

		public PunchCameraModifier(Vector2 startPosition, Vector2 direction, float strength, float vibrationCyclesPerSecond, int frames, float distanceFalloff = -1f, string uniqueIdentity = null)
		{
			_startPosition = startPosition;
			_direction = direction;
			_strength = strength;
			_vibrationCyclesPerSecond = vibrationCyclesPerSecond;
			_framesToLast = frames;
			_distanceFalloff = distanceFalloff;
			UniqueIdentity = uniqueIdentity;
		}

		public void Update(ref CameraInfo cameraInfo)
		{
			float num = (float)Math.Cos((float)_framesLasted / 60f * _vibrationCyclesPerSecond * (MathF.PI * 2f));
			float num2 = Utils.Remap(_framesLasted, 0f, _framesToLast, 1f, 0f);
			float num3 = Utils.Remap(Vector2.Distance(_startPosition, cameraInfo.OriginalCameraCenter), 0f, _distanceFalloff, 1f, 0f);
			if (_distanceFalloff == -1f)
			{
				num3 = 1f;
			}
			cameraInfo.CameraPosition += _direction * num * _strength * num2 * num3;
			_framesLasted++;
			if (_framesLasted >= _framesToLast)
			{
				Finished = true;
			}
		}
	}
}
