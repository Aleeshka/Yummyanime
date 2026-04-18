using System.ComponentModel.DataAnnotations;

namespace Yummyanime.Domain.Entities
{
    public class Anime : EntityBase
    {
        [Display(Name = "Жанр")]
        [Required(ErrorMessage = "Выберите жанр")]
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }

        [Display(Name = "Краткое описание")]
        [MaxLength(3_000)]
        public string? DescriptionShort { get; set; }

        [Display(Name = "Описание")]
        [MaxLength(100_000)]
        public string? Description { get; set; }

        [Display(Name = "Постер")]
        [MaxLength(300)]
        public string? Photo { get; set; }

        [Display(Name = "Год")]
        [Range(1900, 2100, ErrorMessage = "Введите корректный год")]
        public int Year { get; set; }

        [Display(Name = "Рейтинг")]
        [Range(typeof(decimal), "0", "10", ErrorMessage = "Рейтинг должен быть от 0 до 10")]
        public decimal Rating { get; set; }
    }
}
