using Microsoft.EntityFrameworkCore;
using Yummyanime.Domain.Entities;
using Yummyanime.Domain.Repositories.Abstract;

namespace Yummyanime.Domain.Repositories.EntityFramework
{
    public class EFUserAnimeFavoritesRepository : IUserAnimeFavoritesRepository
    {
        private readonly AppDbContext _context;

        public EFUserAnimeFavoritesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsInFavoritesAsync(string userId, int animeId)
        {
            return await _context.UserAnimeFavorites
                .AnyAsync(x => x.UserId == userId
                               && x.AnimeId == animeId
                               && x.Status == UserAnimeListStatus.Favorite);
        }

        public async Task AddAsync(string userId, int animeId)
        {
            await SetStatusAsync(userId, animeId, UserAnimeListStatus.Favorite);
        }

        public async Task RemoveAsync(string userId, int animeId)
        {
            UserAnimeFavorite? entity = await _context.UserAnimeFavorites
                .FirstOrDefaultAsync(x => x.UserId == userId && x.AnimeId == animeId);

            if (entity is null)
            {
                return;
            }

            _context.UserAnimeFavorites.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task SetStatusAsync(string userId, int animeId, UserAnimeListStatus status)
        {
            UserAnimeFavorite? entity = await _context.UserAnimeFavorites
                .FirstOrDefaultAsync(x => x.UserId == userId && x.AnimeId == animeId);

            if (entity is null)
            {
                entity = new UserAnimeFavorite
                {
                    UserId = userId,
                    AnimeId = animeId,
                    Status = status
                };
                _context.UserAnimeFavorites.Add(entity);
            }
            else
            {
                entity.Status = status;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<UserAnimeListStatus?> GetStatusAsync(string userId, int animeId)
        {
            return await _context.UserAnimeFavorites
                .Where(x => x.UserId == userId && x.AnimeId == animeId)
                .Select(x => (UserAnimeListStatus?)x.Status)
                .FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyCollection<Anime>> GetFavoriteAnimeByUserIdAsync(string userId)
        {
            return await GetAnimeByUserAndStatusAsync(userId, UserAnimeListStatus.Favorite);
        }

        public async Task<IReadOnlyCollection<Anime>> GetAnimeByUserAndStatusAsync(string userId, UserAnimeListStatus status)
        {
            return await _context.UserAnimeFavorites
                .Where(x => x.UserId == userId && x.Status == status)
                .Include(x => x.Anime)
                    .ThenInclude(a => a!.Genre)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.Anime!)
                .ToListAsync();
        }
    }
}