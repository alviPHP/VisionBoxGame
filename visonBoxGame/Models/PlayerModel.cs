using System;

namespace visonBoxGame.MockDB
{
    public class PlayerModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Score { get; set; }
        public string Guess { get; set; }
    }
}
