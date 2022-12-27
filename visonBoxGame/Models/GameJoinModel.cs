using System.ComponentModel.DataAnnotations;

namespace visonBoxGame.Models
{
    public class GameJoinModel
    {
        [MaxLength(50)]
        [Required]
        public string GameId { get; set; }

        [MaxLength(50)]
        [Required]
        public string PlayerName { get; set; }
    }
}
