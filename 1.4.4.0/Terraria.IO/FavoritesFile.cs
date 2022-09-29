using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Terraria.UI;
using Terraria.Utilities;

namespace Terraria.IO
{
	public class FavoritesFile
	{
		public readonly string Path;

		public readonly bool IsCloudSave;

		private Dictionary<string, Dictionary<string, bool>> _data = new Dictionary<string, Dictionary<string, bool>>();

		private UTF8Encoding _ourEncoder = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true, throwOnInvalidBytes: true);

		public FavoritesFile(string path, bool isCloud)
		{
			Path = path;
			IsCloudSave = isCloud;
		}

		public void SaveFavorite(FileData fileData)
		{
			if (!_data.ContainsKey(fileData.Type))
			{
				_data.Add(fileData.Type, new Dictionary<string, bool>());
			}
			_data[fileData.Type][fileData.GetFileName()] = fileData.IsFavorite;
			Save();
		}

		public void ClearEntry(FileData fileData)
		{
			if (_data.ContainsKey(fileData.Type))
			{
				_data[fileData.Type].Remove(fileData.GetFileName());
				Save();
			}
		}

		public bool IsFavorite(FileData fileData)
		{
			if (!_data.ContainsKey(fileData.Type))
			{
				return false;
			}
			string fileName = fileData.GetFileName();
			if (_data[fileData.Type].TryGetValue(fileName, out var value))
			{
				return value;
			}
			return false;
		}

		public void Save()
		{
			try
			{
				string s = JsonConvert.SerializeObject((object)_data, (Formatting)1);
				byte[] bytes = _ourEncoder.GetBytes(s);
				FileUtilities.WriteAllBytes(Path, bytes, IsCloudSave);
			}
			catch (Exception exception)
			{
				FancyErrorPrinter.ShowFileSavingFailError(exception, Path);
				throw;
			}
		}

		public void Load()
		{
			if (!FileUtilities.Exists(Path, IsCloudSave))
			{
				_data.Clear();
				return;
			}
			try
			{
				byte[] bytes = FileUtilities.ReadAllBytes(Path, IsCloudSave);
				string @string;
				try
				{
					@string = _ourEncoder.GetString(bytes);
				}
				catch
				{
					@string = Encoding.ASCII.GetString(bytes);
				}
				_data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, bool>>>(@string);
				if (_data == null)
				{
					_data = new Dictionary<string, Dictionary<string, bool>>();
				}
			}
			catch (Exception)
			{
				Console.WriteLine("Unable to load favorites.json file ({0} : {1})", Path, IsCloudSave ? "Cloud Save" : "Local Save");
			}
		}
	}
}
