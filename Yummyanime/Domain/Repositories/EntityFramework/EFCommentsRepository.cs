using Microsoft.EntityFrameworkCore;
using Yummyanime.Domain.Entities;
using Yummyanime.Domain.Repositories.Abstract;

namespace Yummyanime.Domain.Repositories.EntityFramework
{
    public class EFCommentsRepository : ICommentsRepository
    {
        private readonly AppDbContext _context;

        public EFCommentsRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAnimeCommentsAsync(int animeId)
        {
            return await _context.Set<Comment>()
                .Include(x => x.User)
                .Where(x => x.AnimeId == animeId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        public async Task AddCommentAsync(Comment comment)
        {
            _context.Set<Comment>().Add(comment);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int id, string userId)
        {
            Comment? comment = await _context.Set<Comment>().FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
            if (comment != null)
            {
                _context.Set<Comment>().Remove(comment);
                await _context.SaveChangesAsync();
            }
        }
    }
}