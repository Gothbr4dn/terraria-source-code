using Terraria.Utilities;

namespace Terraria.IO
{
	public abstract class FileData
	{
		protected string _path;

		protected bool _isCloudSave;

		public FileMetadata Metadata;

		public string Name;

		public readonly string Type;

		protected bool _isFavorite;

		public string Path => _path;

		public bool IsCloudSave => _isCloudSave;

		public bool IsFavorite => _isFavorite;

		protected FileData(string type)
		{
			Type = type;
		}

		protected FileData(string type, string path, bool isCloud)
		{
			Type = type;
			_path = path;
			_isCloudSave = isCloud;
			_isFavorite = (isCloud ? Main.CloudFavoritesData : Main.LocalFavoriteData).IsFavorite(this);
		}

		public void ToggleFavorite()
		{
			SetFavorite(!IsFavorite);
		}

		public string GetFileName(bool includeExtension = true)
		{
			return FileUtilities.GetFileName(Path, includeExtension);
		}

		public void SetFavorite(bool favorite, bool saveChanges = true)
		{
			_isFavorite = favorite;
			if (saveChanges)
			{
				(IsCloudSave ? Main.CloudFavoritesData : Main.LocalFavoriteData).SaveFavorite(this);
			}
		}

		public abstract void SetAsActive();

		public abstract void MoveToCloud();

		public abstract void MoveToLocal();
	}
}
