using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Terraria.Social.WeGame
{
	public class IPCMessage
	{
		private IPCMessageType _cmd;

		private string _jsonData;

		public void Build<T>(IPCMessageType cmd, T t)
		{
			_jsonData = WeGameHelper.Serialize(t);
			_cmd = cmd;
		}

		public void BuildFrom(byte[] data)
		{
			byte[] value = data.Take(4).ToArray();
			byte[] bytes = data.Skip(4).ToArray();
			_cmd = (IPCMessageType)BitConverter.ToInt32(value, 0);
			_jsonData = Encoding.UTF8.GetString(bytes);
		}

		public void Parse<T>(out T value)
		{
			WeGameHelper.UnSerialize<T>(_jsonData, out value);
		}

		public byte[] GetBytes()
		{
			List<byte> list = new List<byte>();
			byte[] bytes = BitConverter.GetBytes((int)_cmd);
			list.AddRange(bytes);
			list.AddRange(Encoding.UTF8.GetBytes(_jsonData));
			return list.ToArray();
		}

		public IPCMessageType GetCmd()
		{
			return _cmd;
		}
	}
}
