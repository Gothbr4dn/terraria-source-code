namespace Terraria.Utilities.FileBrowser
{
	public class FileBrowser
	{
		private static IFileBrowser _platformWrapper;

		static FileBrowser()
		{
			_platformWrapper = new NativeFileDialog();
		}

		public static string OpenFilePanel(string title, string extension)
		{
			ExtensionFilter[] extensions = (string.IsNullOrEmpty(extension) ? null : new ExtensionFilter[1]
			{
				new ExtensionFilter("", extension)
			});
			return OpenFilePanel(title, extensions);
		}

		public static string OpenFilePanel(string title, ExtensionFilter[] extensions)
		{
			return _platformWrapper.OpenFilePanel(title, extensions);
		}
	}
}
