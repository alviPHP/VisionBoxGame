using System;

namespace visonBoxGame.Models
{
    public class PlayerModel
    {        
        public Guid Id { get; set; }
        public Guid GameId { get; set; }
        public int PlayerIndex { get; set; }
        public string Name { get; set; } 
        public int Score { get; set; }
        public PlayerState State { get; set; }
    }
}
