using Yummyanime.Domain.Entities;

namespace Yummyanime.Domain.Repositories.Abstract
{
    public interface ICommentsRepository
    {
        Task<IEnumerable<Comment>> GetAnimeCommentsAsync(int animeId);
        Task AddCommentAsync(Comment comment);
        Task DeleteCommentAsync(int id, string userId);
    }
}