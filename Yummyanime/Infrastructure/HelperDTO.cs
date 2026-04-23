using Yummyanime.Domain.Entities;
using Yummyanime.Models;

namespace Yummyanime.Infrastructure
{
    public static class HelperDTO
    {
        public static AnimeDTO TransformAnime(Anime entity)
        {
            return new AnimeDTO
            {
                Id = entity.Id,
                GenreName = entity.Genre?.Title,
                Title = entity.Title,
                DescriptionShort = entity.DescriptionShort,
                Description = entity.Description,
                PhotoFileName = entity.Photo,
                Year = entity.Year,
                Rating = entity.Rating,
                VideoUrl = entity.VideoUrl
            };
        }

        public static IEnumerable<AnimeDTO> TransformAnime(IEnumerable<Anime> entities)
        {
            List<AnimeDTO> entitiesDTO = new List<AnimeDTO>();

            foreach (Anime entity in entities)
            {
                entitiesDTO.Add(TransformAnime(entity));
            }

            return entitiesDTO;
        }
    }
}
