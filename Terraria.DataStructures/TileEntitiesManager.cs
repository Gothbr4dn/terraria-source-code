using System.Collections.Generic;
using Terraria.GameContent.Tile_Entities;

namespace Terraria.DataStructures
{
	public class TileEntitiesManager
	{
		private int _nextEntityID;

		private Dictionary<int, TileEntity> _types = new Dictionary<int, TileEntity>();

		private int AssignNewID()
		{
			return _nextEntityID++;
		}

		private bool InvalidEntityID(int id)
		{
			if (id >= 0)
			{
				return id >= _nextEntityID;
			}
			return true;
		}

		public void RegisterAll()
		{
			Register(new TETrainingDummy());
			Register(new TEItemFrame());
			Register(new TELogicSensor());
			Register(new TEDisplayDoll());
			Register(new TEWeaponsRack());
			Register(new TEHatRack());
			Register(new TEFoodPlatter());
			Register(new TETeleportationPylon());
		}

		public void Register(TileEntity entity)
		{
			int num = AssignNewID();
			_types[num] = entity;
			entity.RegisterTileEntityID(num);
		}

		public bool CheckValidTile(int id, int x, int y)
		{
			if (InvalidEntityID(id))
			{
				return false;
			}
			return _types[id].IsTileValidForEntity(x, y);
		}

		public void NetPlaceEntity(int id, int x, int y)
		{
			if (!InvalidEntityID(id) && _types[id].IsTileValidForEntity(x, y))
			{
				_types[id].NetPlaceEntityAttempt(x, y);
			}
		}

		public TileEntity GenerateInstance(int id)
		{
			if (InvalidEntityID(id))
			{
				return null;
			}
			return _types[id].GenerateInstance();
		}
	}
}
