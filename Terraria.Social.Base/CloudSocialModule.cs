using System.Collections.Generic;
using Terraria.IO;

namespace Terraria.Social.Base
{
	public abstract class CloudSocialModule : ISocialModule
	{
		public bool EnabledByDefault;

		public virtual void Initialize()
		{
			Main.Configuration.OnLoad += delegate(Preferences preferences)
			{
				EnabledByDefault = preferences.Get("CloudSavingDefault", defaultValue: false);
			};
			Main.Configuration.OnSave += delegate(Preferences preferences)
			{
				preferences.Put("CloudSavingDefault", EnabledByDefault);
			};
		}

		public abstract void Shutdown();

		public abstract IEnumerable<string> GetFiles();

		public abstract bool Write(string path, byte[] data, int length);

		public abstract void Read(string path, byte[] buffer, int length);

		public abstract bool HasFile(string path);

		public abstract int GetFileSize(string path);

		public abstract bool Delete(string path);

		public abstract bool Forget(string path);

		public byte[] Read(string path)
		{
			byte[] array = new byte[GetFileSize(path)];
			Read(path, array, array.Length);
			return array;
		}

		public void Read(string path, byte[] buffer)
		{
			Read(path, buffer, buffer.Length);
		}

		public bool Write(string path, byte[] data)
		{
			return Write(path, data, data.Length);
		}
	}
}
