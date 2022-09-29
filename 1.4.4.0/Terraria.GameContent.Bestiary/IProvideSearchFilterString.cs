namespace Terraria.GameContent.Bestiary
{
	public interface IProvideSearchFilterString
	{
		string GetSearchString(ref BestiaryUICollectionInfo info);
	}
}
