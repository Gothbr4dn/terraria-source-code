using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Terraria.Cinematics
{
	public class CinematicManager
	{
		public static CinematicManager Instance = new CinematicManager();

		private List<Film> _films = new List<Film>();

		public void Update(GameTime gameTime)
		{
			if (_films.Count > 0)
			{
				if (!_films[0].IsActive)
				{
					_films[0].OnBegin();
				}
				if (Main.hasFocus && !Main.gamePaused && !_films[0].OnUpdate(gameTime))
				{
					_films[0].OnEnd();
					_films.RemoveAt(0);
				}
			}
		}

		public void PlayFilm(Film film)
		{
			_films.Add(film);
		}

		public void StopAll()
		{
		}
	}
}
