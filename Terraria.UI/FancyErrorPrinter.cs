using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ReLogic.Content;

namespace Terraria.UI
{
	public class FancyErrorPrinter
	{
		public static void ShowFailedToLoadAssetError(Exception exception, string filePath)
		{
			bool flag = false;
			if (exception is UnauthorizedAccessException)
			{
				flag = true;
			}
			if (exception is FileNotFoundException)
			{
				flag = true;
			}
			if (exception is DirectoryNotFoundException)
			{
				flag = true;
			}
			if (exception is AssetLoadException)
			{
				flag = true;
			}
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Failed to load asset: \"" + filePath.Replace("/", "\\") + "\"!");
				List<string> suggestions = new List<string> { "Try verifying integrity of game files via Steam, the asset may be missing.", "If you are using an Anti-virus, please make sure it does not block Terraria in any way." };
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Suggestions:");
				AppendSuggestions(stringBuilder, suggestions);
				stringBuilder.AppendLine();
				IncludeOriginalMessage(stringBuilder, exception);
				ShowTheBox(stringBuilder.ToString());
				Console.WriteLine(stringBuilder.ToString());
			}
		}

		public static void ShowFileSavingFailError(Exception exception, string filePath)
		{
			bool flag = false;
			if (exception is UnauthorizedAccessException)
			{
				flag = true;
			}
			if (exception is FileNotFoundException)
			{
				flag = true;
			}
			if (exception is DirectoryNotFoundException)
			{
				flag = true;
			}
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Failed to create the file: \"" + filePath.Replace("/", "\\") + "\"!");
				List<string> list = new List<string> { "If you are using an Anti-virus, please make sure it does not block Terraria in any way.", "Try making sure your `Documents/My Games/Terraria` folder is not set to 'read-only'.", "Try verifying integrity of game files via Steam." };
				if (filePath.ToLower().Contains("onedrive"))
				{
					list.Add("Try updating OneDrive.");
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Suggestions:");
				AppendSuggestions(stringBuilder, list);
				stringBuilder.AppendLine();
				IncludeOriginalMessage(stringBuilder, exception);
				ShowTheBox(stringBuilder.ToString());
				Console.WriteLine(stringBuilder.ToString());
			}
		}

		public static void ShowDirectoryCreationFailError(Exception exception, string folderPath)
		{
			bool flag = false;
			if (exception is UnauthorizedAccessException)
			{
				flag = true;
			}
			if (exception is FileNotFoundException)
			{
				flag = true;
			}
			if (exception is DirectoryNotFoundException)
			{
				flag = true;
			}
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Failed to create the folder: \"" + folderPath.Replace("/", "\\") + "\"!");
				List<string> list = new List<string> { "If you are using an Anti-virus, please make sure it does not block Terraria in any way.", "Try making sure your `Documents/My Games/Terraria` folder is not set to 'read-only'.", "Try verifying integrity of game files via Steam." };
				if (folderPath.ToLower().Contains("onedrive"))
				{
					list.Add("Try updating OneDrive.");
				}
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("Suggestions:");
				AppendSuggestions(stringBuilder, list);
				stringBuilder.AppendLine();
				IncludeOriginalMessage(stringBuilder, exception);
				ShowTheBox(stringBuilder.ToString());
				Console.WriteLine(exception);
			}
		}

		private static void IncludeOriginalMessage(StringBuilder text, Exception exception)
		{
			text.AppendLine("The original Error below");
			text.Append(exception);
		}

		private static void AppendSuggestions(StringBuilder text, List<string> suggestions)
		{
			for (int i = 0; i < suggestions.Count; i++)
			{
				string text2 = suggestions[i];
				text.AppendLine(i + 1 + ". " + text2);
			}
		}

		private static void ShowTheBox(string preparedMessage)
		{
		}
	}
}
