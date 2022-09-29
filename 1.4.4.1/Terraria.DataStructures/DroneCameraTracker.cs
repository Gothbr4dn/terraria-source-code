using Microsoft.Xna.Framework;

namespace Terraria.DataStructures
{
	public class DroneCameraTracker
	{
		private Projectile _trackedProjectile;

		private int _lastTrackedType;

		private bool _inUse;

		public void Track(Projectile proj)
		{
			_trackedProjectile = proj;
			_lastTrackedType = proj.type;
		}

		public void Clear()
		{
			_trackedProjectile = null;
		}

		private void ValidateTrackedProjectile()
		{
			if (_trackedProjectile == null || !_trackedProjectile.active || _trackedProjectile.type != _lastTrackedType || _trackedProjectile.owner != Main.myPlayer || !Main.LocalPlayer.remoteVisionForDrone)
			{
				Clear();
			}
		}

		public bool IsInUse()
		{
			return _inUse;
		}

		public bool TryTracking(out Vector2 cameraPosition)
		{
			ValidateTrackedProjectile();
			cameraPosition = default(Vector2);
			if (_trackedProjectile == null)
			{
				_inUse = false;
				return false;
			}
			cameraPosition = _trackedProjectile.Center;
			_inUse = true;
			return true;
		}
	}
}
