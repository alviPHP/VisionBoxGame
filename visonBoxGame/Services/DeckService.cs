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
        private readonly IList<GameModel> _games;
        public DeckService(IList<GameModel> games)
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
            return _games.AsParallel().Where(gm => gm.Id == gameId).Single();
        }
    }
}
