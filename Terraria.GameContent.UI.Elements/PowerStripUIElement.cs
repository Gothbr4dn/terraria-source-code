using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.UI;

namespace Terraria.GameContent.UI.Elements
{
	public class PowerStripUIElement : UIElement
	{
		private List<UIElement> _buttonsBySorting;

		private string _gamepadPointGroupname;

		public PowerStripUIElement(string gamepadGroupName, List<UIElement> buttons)
		{
			_buttonsBySorting = new List<UIElement>(buttons);
			_gamepadPointGroupname = gamepadGroupName;
			int count = buttons.Count;
			int num = 4;
			int num2 = 40;
			int num3 = 40;
			int num4 = num3 + num;
			UIPanel uIPanel = new UIPanel
			{
				Width = new StyleDimension(num2 + num * 2, 0f),
				Height = new StyleDimension(num3 * count + num * (1 + count), 0f)
			};
			SetPadding(0f);
			Width = uIPanel.Width;
			Height = uIPanel.Height;
			uIPanel.BorderColor = new Color(89, 116, 213, 255) * 0.9f;
			uIPanel.BackgroundColor = new Color(73, 94, 171) * 0.9f;
			uIPanel.SetPadding(0f);
			Append(uIPanel);
			for (int i = 0; i < count; i++)
			{
				UIElement uIElement = buttons[i];
				uIElement.HAlign = 0.5f;
				uIElement.Top = new StyleDimension(num + num4 * i, 0f);
				uIElement.SetSnapPoint(_gamepadPointGroupname, i);
				uIPanel.Append(uIElement);
				_buttonsBySorting.Add(uIElement);
			}
		}
	}
}
