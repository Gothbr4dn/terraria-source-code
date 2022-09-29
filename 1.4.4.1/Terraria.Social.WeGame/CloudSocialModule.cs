using System.Collections.Generic;
using rail;
using Terraria.Social.Base;

namespace Terraria.Social.WeGame
{
	public class CloudSocialModule : Terraria.Social.Base.CloudSocialModule
	{
		private object ioLock = new object();

		public override void Initialize()
		{
			base.Initialize();
		}

		public override void Shutdown()
		{
		}

		public override IEnumerable<string> GetFiles()
		{
			//IL_0041: Unknown result type (might be due to invalid IL or missing references)
			lock (ioLock)
			{
				uint fileCount = rail_api.RailFactory().RailStorageHelper().GetFileCount();
				List<string> list = new List<string>((int)fileCount);
				ulong num = 0uL;
				string item = default(string);
				for (uint num2 = 0u; num2 < fileCount; num2++)
				{
					rail_api.RailFactory().RailStorageHelper().GetFileNameAndSize(num2, ref item, ref num);
					list.Add(item);
				}
				return list;
			}
		}

		public override bool Write(string path, byte[] data, int length)
		{
			lock (ioLock)
			{
				bool result = true;
				IRailFile val = null;
				val = ((!rail_api.RailFactory().RailStorageHelper().IsFileExist(path)) ? rail_api.RailFactory().RailStorageHelper().CreateFile(path) : rail_api.RailFactory().RailStorageHelper().OpenFile(path));
				if (val != null)
				{
					val.Write(data, (uint)length);
					val.Close();
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		public override int GetFileSize(string path)
		{
			lock (ioLock)
			{
				IRailFile val = rail_api.RailFactory().RailStorageHelper().OpenFile(path);
				if (val != null)
				{
					int size = (int)val.GetSize();
					val.Close();
					return size;
				}
				return 0;
			}
		}

		public override void Read(string path, byte[] buffer, int size)
		{
			lock (ioLock)
			{
				IRailFile val = rail_api.RailFactory().RailStorageHelper().OpenFile(path);
				if (val != null)
				{
					val.Read(buffer, (uint)size);
					val.Close();
				}
			}
		}

		public override bool HasFile(string path)
		{
			lock (ioLock)
			{
				return rail_api.RailFactory().RailStorageHelper().IsFileExist(path);
			}
		}

		public override bool Delete(string path)
		{
			//IL_001c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Invalid comparison between Unknown and I4
			lock (ioLock)
			{
				RailResult val = rail_api.RailFactory().RailStorageHelper().RemoveFile(path);
				return (int)val == 0;
			}
		}

		public override bool Forget(string path)
		{
			return Delete(path);
		}
	}
}
