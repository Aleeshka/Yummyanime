using Yummyanime.Domain.Repositories.Abstract;

namespace Yummyanime.Domain
{
    public class DataManager
    {
        public IGenresRepository Genres { get; set; }
        public IAnimeRepository Anime { get; set; }
        public IUserAnimeFavoritesRepository UserAnimeFavorites { get; set; }
        public ICommentsRepository Comments { get; set; }

        public DataManager(
            IGenresRepository genresRepository,
            IAnimeRepository animeRepository,
            IUserAnimeFavoritesRepository userAnimeFavoritesRepository,
            ICommentsRepository commentsRepository)
        {
            Genres = genresRepository;
            Anime = animeRepository;
            UserAnimeFavorites = userAnimeFavoritesRepository;
            Comments = commentsRepository;
        }
    }
}
