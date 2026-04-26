using Yummyanime.Domain.Entities;

namespace Yummyanime.Domain.Repositories.Abstract
{
    public interface IUserAnimeFavoritesRepository
    {
        Task<bool> IsInFavoritesAsync(string userId, int animeId);
        Task AddAsync(string userId, int animeId);
        Task RemoveAsync(string userId, int animeId);
        Task<IReadOnlyCollection<Anime>> GetFavoriteAnimeByUserIdAsync(string userId);
    }
}