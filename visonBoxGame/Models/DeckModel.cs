using visonBoxGame.DeckCards;
using System;
using System.Collections.Generic;

namespace visonBoxGame.Models
{
    public class DeckModel
    {
        public Guid GameId { get; set; }
        public List<Card> Deck { get; set; }
    }
}
