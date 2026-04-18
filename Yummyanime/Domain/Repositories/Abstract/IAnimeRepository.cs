using Yummyanime.Domain.Entities;

namespace Yummyanime.Domain.Repositories.Abstract
{
    public interface IAnimeRepository
    {
        Task<IEnumerable<Anime>> GetAnimeAsync();
        Task<Anime?> GetAnimeByIdAsync(int id);
        Task SaveAnimeAsync(Anime entity);
        Task DeleteAnimeAsync(int id);
    }
}
