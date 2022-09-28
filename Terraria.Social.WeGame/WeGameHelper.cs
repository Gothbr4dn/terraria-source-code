using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Terraria.Social.WeGame
{
	public class WeGameHelper
	{
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern void OutputDebugString(string message);

		public static void WriteDebugString(string format, params object[] args)
		{
			_ = "[WeGame] - " + format;
		}

		public static string Serialize<T>(T data)
		{
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			using MemoryStream memoryStream = new MemoryStream();
			dataContractJsonSerializer.WriteObject((Stream)memoryStream, (object)data);
			memoryStream.Position = 0L;
			using StreamReader streamReader = new StreamReader(memoryStream, Encoding.UTF8);
			return streamReader.ReadToEnd();
		}

		public static void UnSerialize<T>(string str, out T data)
		{
			using MemoryStream stream = new MemoryStream(Encoding.Unicode.GetBytes(str));
			DataContractJsonSerializer dataContractJsonSerializer = new DataContractJsonSerializer(typeof(T));
			data = (T)dataContractJsonSerializer.ReadObject((Stream)stream);
		}
	}
}
