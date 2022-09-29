using System.Collections.Generic;
using Terraria.Localization;

namespace Terraria.UI
{
	public class ItemTooltip
	{
		public static readonly ItemTooltip None = new ItemTooltip();

		private static readonly List<TooltipProcessor> _globalProcessors = new List<TooltipProcessor>();

		private static ulong _globalValidatorKey = 1uL;

		private string[] _tooltipLines;

		private ulong _validatorKey;

		private readonly LocalizedText _text;

		private string _processedText;

		public int Lines
		{
			get
			{
				ValidateTooltip();
				if (_tooltipLines == null)
				{
					return 0;
				}
				return _tooltipLines.Length;
			}
		}

		private ItemTooltip()
		{
		}

		private ItemTooltip(string key)
		{
			_text = Language.GetText(key);
		}

		public static ItemTooltip FromLanguageKey(string key)
		{
			if (!Language.Exists(key))
			{
				return None;
			}
			return new ItemTooltip(key);
		}

		public string GetLine(int line)
		{
			ValidateTooltip();
			return _tooltipLines[line];
		}

		private void ValidateTooltip()
		{
			if (_validatorKey == _globalValidatorKey)
			{
				return;
			}
			_validatorKey = _globalValidatorKey;
			if (_text == null)
			{
				_tooltipLines = null;
				_processedText = string.Empty;
				return;
			}
			string text = _text.Value;
			foreach (TooltipProcessor globalProcessor in _globalProcessors)
			{
				text = globalProcessor(text);
			}
			_tooltipLines = text.Split(new char[1] { '\n' });
			_processedText = text;
		}

		public static void AddGlobalProcessor(TooltipProcessor processor)
		{
			_globalProcessors.Add(processor);
		}

		public static void RemoveGlobalProcessor(TooltipProcessor processor)
		{
			_globalProcessors.Remove(processor);
		}

		public static void ClearGlobalProcessors()
		{
			_globalProcessors.Clear();
		}

		public static void InvalidateTooltips()
		{
			_globalValidatorKey++;
			if (_globalValidatorKey == ulong.MaxValue)
			{
				_globalValidatorKey = 0uL;
			}
		}
	}
}
