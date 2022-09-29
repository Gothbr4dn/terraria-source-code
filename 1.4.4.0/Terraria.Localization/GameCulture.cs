using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Terraria.Localization
{
	public class GameCulture
	{
		public enum CultureName
		{
			English = 1,
			German = 2,
			Italian = 3,
			French = 4,
			Spanish = 5,
			Russian = 6,
			Chinese = 7,
			Portuguese = 8,
			Polish = 9,
			Unknown = 9999
		}

		private static Dictionary<CultureName, GameCulture> _NamedCultures;

		private static Dictionary<int, GameCulture> _legacyCultures;

		public readonly CultureInfo CultureInfo;

		public readonly int LegacyId;

		public static GameCulture DefaultCulture { get; set; }

		public bool IsActive => Language.ActiveCulture == this;

		public string Name => CultureInfo.Name;

		public static GameCulture FromCultureName(CultureName name)
		{
			if (!_NamedCultures.ContainsKey(name))
			{
				return DefaultCulture;
			}
			return _NamedCultures[name];
		}

		public static GameCulture FromLegacyId(int id)
		{
			if (id < 1)
			{
				id = 1;
			}
			if (!_legacyCultures.ContainsKey(id))
			{
				return DefaultCulture;
			}
			return _legacyCultures[id];
		}

		public static GameCulture FromName(string name)
		{
			return _legacyCultures.Values.SingleOrDefault((GameCulture culture) => culture.Name == name) ?? DefaultCulture;
		}

		static GameCulture()
		{
			_NamedCultures = new Dictionary<CultureName, GameCulture>
			{
				{
					CultureName.English,
					new GameCulture("en-US", 1)
				},
				{
					CultureName.German,
					new GameCulture("de-DE", 2)
				},
				{
					CultureName.Italian,
					new GameCulture("it-IT", 3)
				},
				{
					CultureName.French,
					new GameCulture("fr-FR", 4)
				},
				{
					CultureName.Spanish,
					new GameCulture("es-ES", 5)
				},
				{
					CultureName.Russian,
					new GameCulture("ru-RU", 6)
				},
				{
					CultureName.Chinese,
					new GameCulture("zh-Hans", 7)
				},
				{
					CultureName.Portuguese,
					new GameCulture("pt-BR", 8)
				},
				{
					CultureName.Polish,
					new GameCulture("pl-PL", 9)
				}
			};
			DefaultCulture = _NamedCultures[CultureName.English];
		}

		public GameCulture(string name, int legacyId)
		{
			CultureInfo = new CultureInfo(name);
			LegacyId = legacyId;
			RegisterLegacyCulture(this, legacyId);
		}

		private static void RegisterLegacyCulture(GameCulture culture, int legacyId)
		{
			if (_legacyCultures == null)
			{
				_legacyCultures = new Dictionary<int, GameCulture>();
			}
			_legacyCultures.Add(legacyId, culture);
		}
	}
}
