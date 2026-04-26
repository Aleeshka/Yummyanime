using Yummyanime.Domain.Entities;

namespace Yummyanime.Domain.Repositories.Abstract
{
    public interface IUserAnimeFavoritesRepository
    {
        Task<bool> IsInFavoritesAsync(string userId, int animeId);
        Task AddAsync(string userId, int animeId);
        Task RemoveAsync(string userId, int animeId);

        Task SetStatusAsync(string userId, int animeId, UserAnimeListStatus status);
        Task<UserAnimeListStatus?> GetStatusAsync(string userId, int animeId);

        Task<IReadOnlyCollection<Anime>> GetFavoriteAnimeByUserIdAsync(string userId);
        Task<IReadOnlyCollection<Anime>> GetAnimeByUserAndStatusAsync(string userId, UserAnimeListStatus status);
    }
}