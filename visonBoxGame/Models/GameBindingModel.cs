using System.ComponentModel.DataAnnotations;

namespace visonBoxGame.Models
{
    public class GameBindingModel
    {
        [MaxLength(50)]
        [Required]
        public string Title { get; set; }

        [MaxLength(50)]
        [Required]
        public string PlayerName { get; set; }
    }
}
