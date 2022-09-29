namespace Terraria.Social.Base
{
	public class WorkshopTagOption
	{
		public readonly string NameKey;

		public readonly string InternalNameForAPIs;

		public WorkshopTagOption(string nameKey, string internalName)
		{
			NameKey = nameKey;
			InternalNameForAPIs = internalName;
		}
	}
}
