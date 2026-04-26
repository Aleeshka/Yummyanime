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
                .AnyAsync(x => x.UserId == userId && x.AnimeId == animeId);
        }

        public async Task AddAsync(string userId, int animeId)
        {
            bool exists = await IsInFavoritesAsync(userId, animeId);
            if (exists)
            {
                return;
            }

            _context.UserAnimeFavorites.Add(new UserAnimeFavorite
            {
                UserId = userId,
                AnimeId = animeId
            });

            await _context.SaveChangesAsync();
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

        public async Task<IReadOnlyCollection<Anime>> GetFavoriteAnimeByUserIdAsync(string userId)
        {
            return await _context.UserAnimeFavorites
                .Where(x => x.UserId == userId)
                .Include(x => x.Anime)
                    .ThenInclude(a => a!.Genre)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => x.Anime!)
                .ToListAsync();
        }
    }
}