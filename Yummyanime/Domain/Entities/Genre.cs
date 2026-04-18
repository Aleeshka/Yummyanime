namespace Yummyanime.Domain.Entities
{
    public class Genre : EntityBase
    {
        public ICollection<Anime>? Animes { get; set; }
    }
}
