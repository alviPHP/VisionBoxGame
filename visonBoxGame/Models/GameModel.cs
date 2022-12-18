using System;

namespace visonBoxGame.Models
{
    public class GameModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public GameState State { get; set; }
        public int PlayersJoin { get; set; }
        public int NextPlayerIndex { get; set; }
        public int TurnCount { get; set; }
        public string OutCome { get; set; }
        public string LastCardPlay { get; set; }
        public int LastCardValue { get; set; }
    }
}
