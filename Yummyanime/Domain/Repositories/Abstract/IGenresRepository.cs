using Yummyanime.Domain.Entities;

namespace Yummyanime.Domain.Repositories.Abstract
{
    public interface IGenresRepository
    {
        Task<IEnumerable<Genre>> GetGenresAsync();
        Task<Genre?> GetGenreByIdAsync(int id);
        Task SaveGenreAsync(Genre entity);
        Task DeleteGenreAsync(int id);
    }
}
