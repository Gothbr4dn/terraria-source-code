namespace Terraria.Utilities.FileBrowser
{
	public interface IFileBrowser
	{
		string OpenFilePanel(string title, ExtensionFilter[] extensions);
	}
}
