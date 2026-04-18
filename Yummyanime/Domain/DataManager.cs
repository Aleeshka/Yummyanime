using Yummyanime.Domain.Repositories.Abstract;

namespace Yummyanime.Domain
{
    public class DataManager
    {
        public IGenresRepository Genres { get; set; }
        public IAnimeRepository Anime { get; set; }

        public DataManager(IGenresRepository genresRepository, IAnimeRepository animeRepository)
        {
            Genres = genresRepository;
            Anime = animeRepository;
        }
    }
}
