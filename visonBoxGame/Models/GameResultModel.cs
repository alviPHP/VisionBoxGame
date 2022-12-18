using System;

namespace visonBoxGame.Models
{
    public class GameResultModel
    {
        public string Result { get; set; }
        public string LastCardPlay { get; set; }
        public Guid? NextPlayerId { get; set; }
        public string NextPlayerName { get; set; }        
    }
}
