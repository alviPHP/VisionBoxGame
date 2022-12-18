using System;
using System.Collections.Generic;

namespace visonBoxGame.DeckCards
{
    public static class Deck
    {
        public static void FillDeck(ref List<Card> deck)
        {
            foreach (string suit in Card.SuitsArray)
            {
                for (int value = 2; value <= 14; value++)
                {
                    Card card = new Card(value, suit);
                    deck.Add(card);
                }
            }
        }
        public static Card GetRandomCard(ref List<Card> _deck)
        {
            Random random = new Random();
            int index = random.Next(0, _deck.Count);
            Card card = _deck[index];
            _deck.RemoveAt(index);
            return card;
        }    
    }
}
