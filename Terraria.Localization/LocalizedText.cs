using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Terraria.Localization
{
	public class LocalizedText
	{
		public static readonly LocalizedText Empty = new LocalizedText("", "");

		private static Regex _substitutionRegex = new Regex("{(\\?(?:!)?)?([a-zA-Z][\\w\\.]*)}", RegexOptions.Compiled);

		public readonly string Key;

		public string Value { get; private set; }

		internal LocalizedText(string key, string text)
		{
			Key = key;
			Value = text;
		}

		internal void SetValue(string text)
		{
			Value = text;
		}

		public string FormatWith(object obj)
		{
			string value = Value;
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
			return _substitutionRegex.Replace(value, delegate(Match match)
			{
				if (match.Groups[1].Length != 0)
				{
					return "";
				}
				string name = match.Groups[2].ToString();
				PropertyDescriptor propertyDescriptor = properties.Find(name, ignoreCase: false);
				return (propertyDescriptor == null) ? "" : (propertyDescriptor.GetValue(obj) ?? "").ToString();
			});
		}

		public bool CanFormatWith(object obj)
		{
			PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(obj);
			foreach (Match item in _substitutionRegex.Matches(Value))
			{
				string name = item.Groups[2].ToString();
				PropertyDescriptor propertyDescriptor = properties.Find(name, ignoreCase: false);
				if (propertyDescriptor == null)
				{
					return false;
				}
				object value = propertyDescriptor.GetValue(obj);
				if (value == null)
				{
					return false;
				}
				if (item.Groups[1].Length != 0 && (((value as bool?) ?? false) ^ (item.Groups[1].Length == 1)))
				{
					return false;
				}
			}
			return true;
		}

		public NetworkText ToNetworkText()
		{
			return NetworkText.FromKey(Key);
		}

		public NetworkText ToNetworkText(params object[] substitutions)
		{
			return NetworkText.FromKey(Key, substitutions);
		}

		public static explicit operator string(LocalizedText text)
		{
			return text.Value;
		}

		public string Format(object arg0)
		{
			return string.Format(Value, arg0);
		}

		public string Format(object arg0, object arg1)
		{
			return string.Format(Value, arg0, arg1);
		}

		public string Format(object arg0, object arg1, object arg2)
		{
			return string.Format(Value, arg0, arg1, arg2);
		}

		public string Format(params object[] args)
		{
			return string.Format(Value, args);
		}

		public override string ToString()
		{
			return Value;
		}
	}
}
