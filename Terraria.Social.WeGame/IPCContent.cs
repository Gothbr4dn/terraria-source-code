using System.Threading;

namespace Terraria.Social.WeGame
{
	public class IPCContent
	{
		public byte[] data;

		public CancellationToken CancelToken { get; set; }
	}
}
