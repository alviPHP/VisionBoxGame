using System.ComponentModel.DataAnnotations;

namespace visonBoxGame.Models
{
    public class GuessCardModel
    {
        [Required]
        public string GameId { get; set; }

        [Required]
        public string PlayerId { get; set; }

        [Required]
        [RegularExpression(@"[H|L]")]
        public string Guess { get; set; }

    }
}
