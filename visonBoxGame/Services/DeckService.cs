using System;
using System.Collections.Generic;
using visonBoxGame.DeckCards;
using visonBoxGame.MockDB;

namespace visonBoxGame.Services
{
    public interface IDeckService
    {
        void AddNewDeck(Guid gameId);
        List<Card> GetDeck(Guid gameId);
    }
    public class DeckService : IDeckService
    {
        private readonly IDictionary<Guid, GameModel> _games;
        public DeckService(IDictionary<Guid, GameModel> games)
        {
            _games = games;
        }
        public void AddNewDeck(Guid gameId)
        {
            var game = _games[gameId];
            Deck.FillDeck(out var deck);
            game.Cards= deck;
        }
        public List<Card> GetDeck(Guid gameId)
        {
            if(!_games.ContainsKey(gameId))
                return null;
            var game = _games[gameId];
            return game.Cards;
        }
    }
}
