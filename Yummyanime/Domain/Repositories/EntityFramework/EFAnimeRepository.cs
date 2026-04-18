using Microsoft.EntityFrameworkCore;
using Yummyanime.Domain.Entities;
using Yummyanime.Domain.Repositories.Abstract;

namespace Yummyanime.Domain.Repositories.EntityFramework
{
    public class EFAnimeRepository : IAnimeRepository
    {
        private readonly AppDbContext _context;

        public EFAnimeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Anime>> GetAnimeAsync()
        {
            return await _context.Animes.Include(x => x.Genre).ToListAsync();
        }

        public async Task<Anime?> GetAnimeByIdAsync(int id)
        {
            return await _context.Animes.Include(x => x.Genre).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveAnimeAsync(Anime entity)
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAnimeAsync(int id)
        {
            _context.Entry(new Anime { Id = id }).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
