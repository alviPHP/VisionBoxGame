using System;
using System.ComponentModel.DataAnnotations;

namespace visonBoxGame.Models
{
    public class GameIdModel
    {
        [Required]
        public string GameId { get; set; }

        [Required]
        public string PlayerId { get; set; }

    }
}
