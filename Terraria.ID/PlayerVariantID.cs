namespace Terraria.ID
{
	public static class PlayerVariantID
	{
		public class Sets
		{
			public static bool[] Male = Factory.CreateBoolSet(0, 1, 2, 3, 8, 10);

			public static int[] AltGenderReference = Factory.CreateIntSet(0, 0, 4, 4, 0, 1, 5, 5, 1, 2, 6, 6, 2, 3, 7, 7, 3, 8, 9, 9, 8, 10, 11, 11, 10);

			public static int[] VariantOrderMale = new int[6] { 0, 1, 2, 3, 8, 10 };

			public static int[] VariantOrderFemale = new int[6] { 4, 5, 6, 7, 9, 11 };
		}

		public static SetFactory Factory = new SetFactory(12);

		public const int MaleStarter = 0;

		public const int MaleSticker = 1;

		public const int MaleGangster = 2;

		public const int MaleCoat = 3;

		public const int FemaleStarter = 4;

		public const int FemaleSticker = 5;

		public const int FemaleGangster = 6;

		public const int FemaleCoat = 7;

		public const int MaleDress = 8;

		public const int FemaleDress = 9;

		public const int MaleDisplayDoll = 10;

		public const int FemaleDisplayDoll = 11;

		public const int Count = 12;
	}
}
