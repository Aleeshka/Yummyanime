using Microsoft.EntityFrameworkCore;
using Yummyanime.Domain.Entities;
using Yummyanime.Domain.Repositories.Abstract;

namespace Yummyanime.Domain.Repositories.EntityFramework
{
    public class EFGenresRepository : IGenresRepository
    {
        private readonly AppDbContext _context;

        public EFGenresRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Genre>> GetGenresAsync()
        {
            return await _context.Genres.Include(x => x.Animes).ToListAsync();
        }

        public async Task<Genre?> GetGenreByIdAsync(int id)
        {
            return await _context.Genres.Include(x => x.Animes).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task SaveGenreAsync(Genre entity)
        {
            _context.Entry(entity).State = entity.Id == default ? EntityState.Added : EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteGenreAsync(int id)
        {
            _context.Entry(new Genre { Id = id }).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}
