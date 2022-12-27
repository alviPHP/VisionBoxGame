using System;
using System.Collections.Generic;
using System.Linq;
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
            var game = GetGame(gameId);
            Deck.FillDeck(out var deck);
            game.Cards = deck;
        }
        public List<Card> GetDeck(Guid gameId)
        {
            return GetGame(gameId).Cards;
        }
        private GameModel GetGame(Guid gameId)
        {
            return _games.Values.AsParallel().Where(gm => gm.Id == gameId).Single();
        }
    }
}
