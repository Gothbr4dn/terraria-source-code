using System;
using System.IO;
using System.Text;

namespace Terraria.Localization
{
	public class NetworkText
	{
		private enum Mode : byte
		{
			Literal,
			Formattable,
			LocalizationKey
		}

		public static readonly NetworkText Empty = FromLiteral("");

		private NetworkText[] _substitutions;

		private string _text;

		private Mode _mode;

		private NetworkText(string text, Mode mode)
		{
			_text = text;
			_mode = mode;
		}

		private static NetworkText[] ConvertSubstitutionsToNetworkText(object[] substitutions)
		{
			NetworkText[] array = new NetworkText[substitutions.Length];
			for (int i = 0; i < substitutions.Length; i++)
			{
				NetworkText networkText = substitutions[i] as NetworkText;
				if (networkText == null)
				{
					networkText = FromLiteral(substitutions[i].ToString());
				}
				array[i] = networkText;
			}
			return array;
		}

		public static NetworkText FromFormattable(string text, params object[] substitutions)
		{
			return new NetworkText(text, Mode.Formattable)
			{
				_substitutions = ConvertSubstitutionsToNetworkText(substitutions)
			};
		}

		public static NetworkText FromLiteral(string text)
		{
			return new NetworkText(text, Mode.Literal);
		}

		public static NetworkText FromKey(string key, params object[] substitutions)
		{
			return new NetworkText(key, Mode.LocalizationKey)
			{
				_substitutions = ConvertSubstitutionsToNetworkText(substitutions)
			};
		}

		public int GetMaxSerializedSize()
		{
			int num = 0;
			num++;
			num += 4 + Encoding.UTF8.GetByteCount(_text);
			if (_mode != 0)
			{
				num++;
				for (int i = 0; i < _substitutions.Length; i++)
				{
					num += _substitutions[i].GetMaxSerializedSize();
				}
			}
			return num;
		}

		public void Serialize(BinaryWriter writer)
		{
			writer.Write((byte)_mode);
			writer.Write(_text);
			SerializeSubstitutionList(writer);
		}

		private void SerializeSubstitutionList(BinaryWriter writer)
		{
			if (_mode != 0)
			{
				writer.Write((byte)_substitutions.Length);
				for (int i = 0; i < (_substitutions.Length & 0xFF); i++)
				{
					_substitutions[i].Serialize(writer);
				}
			}
		}

		public static NetworkText Deserialize(BinaryReader reader)
		{
			Mode mode = (Mode)reader.ReadByte();
			NetworkText networkText = new NetworkText(reader.ReadString(), mode);
			networkText.DeserializeSubstitutionList(reader);
			return networkText;
		}

		public static NetworkText DeserializeLiteral(BinaryReader reader)
		{
			Mode mode = (Mode)reader.ReadByte();
			NetworkText networkText = new NetworkText(reader.ReadString(), mode);
			networkText.DeserializeSubstitutionList(reader);
			if (mode != 0)
			{
				networkText.SetToEmptyLiteral();
			}
			return networkText;
		}

		private void DeserializeSubstitutionList(BinaryReader reader)
		{
			if (_mode != 0)
			{
				_substitutions = new NetworkText[reader.ReadByte()];
				for (int i = 0; i < _substitutions.Length; i++)
				{
					_substitutions[i] = Deserialize(reader);
				}
			}
		}

		private void SetToEmptyLiteral()
		{
			_mode = Mode.Literal;
			_text = string.Empty;
			_substitutions = null;
		}

		public override string ToString()
		{
			try
			{
				switch (_mode)
				{
				case Mode.Literal:
					return _text;
				case Mode.Formattable:
				{
					string text2 = _text;
					object[] substitutions = _substitutions;
					return string.Format(text2, substitutions);
				}
				case Mode.LocalizationKey:
				{
					string text = _text;
					object[] substitutions = _substitutions;
					return Language.GetTextValue(text, substitutions);
				}
				default:
					return _text;
				}
			}
			catch (Exception ex)
			{
				string.Concat(string.Concat("NetworkText.ToString() threw an exception.\n" + ToDebugInfoString(), "\n"), "Exception: ", ex);
				SetToEmptyLiteral();
			}
			return _text;
		}

		private string ToDebugInfoString(string linePrefix = "")
		{
			string text = string.Format("{0}Mode: {1}\n{0}Text: {2}\n", linePrefix, _mode, _text);
			if (_mode == Mode.LocalizationKey)
			{
				text += $"{linePrefix}Localized Text: {Language.GetTextValue(_text)}\n";
			}
			if (_mode != 0)
			{
				for (int i = 0; i < _substitutions.Length; i++)
				{
					text += $"{linePrefix}Substitution {i}:\n";
					text += _substitutions[i].ToDebugInfoString(linePrefix + "\t");
				}
			}
			return text;
		}
	}
}
