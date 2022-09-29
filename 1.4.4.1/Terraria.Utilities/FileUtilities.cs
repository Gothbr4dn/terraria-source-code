using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using ReLogic.OS;
using Terraria.Social;

namespace Terraria.Utilities
{
	public static class FileUtilities
	{
		private static Regex FileNameRegex = new Regex("^(?<path>.*[\\\\\\/])?(?:$|(?<fileName>.+?)(?:(?<extension>\\.[^.]*$)|$))", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public static bool Exists(string path, bool cloud)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				return SocialAPI.Cloud.HasFile(path);
			}
			return File.Exists(path);
		}

		public static void Delete(string path, bool cloud, bool forceDeleteFile = false)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				SocialAPI.Cloud.Delete(path);
			}
			else if (forceDeleteFile)
			{
				File.Delete(path);
			}
			else
			{
				Platform.Get<IPathService>().MoveToRecycleBin(path);
			}
		}

		public static string GetFullPath(string path, bool cloud)
		{
			if (!cloud)
			{
				return Path.GetFullPath(path);
			}
			return path;
		}

		public static void Copy(string source, string destination, bool cloud, bool overwrite = true)
		{
			if (!cloud)
			{
				File.Copy(source, destination, overwrite);
			}
			else if (SocialAPI.Cloud != null && (overwrite || !SocialAPI.Cloud.HasFile(destination)))
			{
				SocialAPI.Cloud.Write(destination, SocialAPI.Cloud.Read(source));
			}
		}

		public static void Move(string source, string destination, bool cloud, bool overwrite = true, bool forceDeleteSourceFile = false)
		{
			Copy(source, destination, cloud, overwrite);
			Delete(source, cloud, forceDeleteSourceFile);
		}

		public static int GetFileSize(string path, bool cloud)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				return SocialAPI.Cloud.GetFileSize(path);
			}
			return (int)new FileInfo(path).Length;
		}

		public static void Read(string path, byte[] buffer, int length, bool cloud)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				SocialAPI.Cloud.Read(path, buffer, length);
				return;
			}
			using FileStream fileStream = File.OpenRead(path);
			fileStream.Read(buffer, 0, length);
		}

		public static byte[] ReadAllBytes(string path, bool cloud)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				return SocialAPI.Cloud.Read(path);
			}
			return File.ReadAllBytes(path);
		}

		public static void WriteAllBytes(string path, byte[] data, bool cloud)
		{
			Write(path, data, data.Length, cloud);
		}

		public static void Write(string path, byte[] data, int length, bool cloud)
		{
			if (cloud && SocialAPI.Cloud != null)
			{
				SocialAPI.Cloud.Write(path, data, length);
				return;
			}
			string parentFolderPath = GetParentFolderPath(path);
			if (parentFolderPath != "")
			{
				Utils.TryCreatingDirectory(parentFolderPath);
			}
			RemoveReadOnlyAttribute(path);
			using FileStream fileStream = File.Open(path, FileMode.Create);
			while (fileStream.Position < length)
			{
				fileStream.Write(data, (int)fileStream.Position, Math.Min(length - (int)fileStream.Position, 2048));
			}
		}

		public static void RemoveReadOnlyAttribute(string path)
		{
			if (!File.Exists(path))
			{
				return;
			}
			try
			{
				FileAttributes attributes = File.GetAttributes(path);
				if ((attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
				{
					attributes &= ~FileAttributes.ReadOnly;
					File.SetAttributes(path, attributes);
				}
			}
			catch (Exception)
			{
			}
		}

		public static bool MoveToCloud(string localPath, string cloudPath)
		{
			if (SocialAPI.Cloud == null)
			{
				return false;
			}
			WriteAllBytes(cloudPath, ReadAllBytes(localPath, cloud: false), cloud: true);
			Delete(localPath, cloud: false);
			return true;
		}

		public static bool MoveToLocal(string cloudPath, string localPath)
		{
			if (SocialAPI.Cloud == null)
			{
				return false;
			}
			WriteAllBytes(localPath, ReadAllBytes(cloudPath, cloud: true), cloud: false);
			Delete(cloudPath, cloud: true);
			return true;
		}

		public static bool CopyToLocal(string cloudPath, string localPath)
		{
			if (SocialAPI.Cloud == null)
			{
				return false;
			}
			WriteAllBytes(localPath, ReadAllBytes(cloudPath, cloud: true), cloud: false);
			return true;
		}

		public static string GetFileName(string path, bool includeExtension = true)
		{
			Match match = FileNameRegex.Match(path);
			if (match == null || match.Groups["fileName"] == null)
			{
				return "";
			}
			includeExtension &= match.Groups["extension"] != null;
			return match.Groups["fileName"].Value + (includeExtension ? match.Groups["extension"].Value : "");
		}

		public static string GetParentFolderPath(string path, bool includeExtension = true)
		{
			Match match = FileNameRegex.Match(path);
			if (match == null || match.Groups["path"] == null)
			{
				return "";
			}
			return match.Groups["path"].Value;
		}

		public static void CopyFolder(string sourcePath, string destinationPath)
		{
			Directory.CreateDirectory(destinationPath);
			string[] directories = Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories);
			for (int i = 0; i < directories.Length; i++)
			{
				Directory.CreateDirectory(directories[i].Replace(sourcePath, destinationPath));
			}
			directories = Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories);
			foreach (string obj in directories)
			{
				File.Copy(obj, obj.Replace(sourcePath, destinationPath), overwrite: true);
			}
		}

		public static void ProtectedInvoke(Action action)
		{
			bool isBackground = Thread.CurrentThread.IsBackground;
			try
			{
				Thread.CurrentThread.IsBackground = false;
				action();
			}
			finally
			{
				Thread.CurrentThread.IsBackground = isBackground;
			}
		}
	}
}
