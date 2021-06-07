using ParkyAPI.Enums;
using System.ComponentModel.DataAnnotations;

namespace ParkyAPI.Models.Dtos
{
    public class TrailDto {
        [Required]
        public int Id { get; set; }
        [Required]
        public int NationalParkId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Distance { get; set; }
        public DifficultyType Difficulty { get; set; }
        public NationalParkDto NationalPark { get; set; }
    }
}
