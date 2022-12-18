using visonBoxGame.DeckCards;
using visonBoxGame.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace visonBoxGame.Services
{
    public interface ICardService
    {
        Task<DeckModel> GetDeck(Guid gameId);        
        Task<bool> GuessNextCard(GuessCardModel model);
    }
    public class CardService : ICardService
    {
        private readonly IDictionary<Guid, GameModel> _games;
        private readonly IDictionary<Guid, PlayerModel> _players;
        private readonly IDictionary<Guid, DeckModel> _decks;
        
        public CardService(IDictionary<Guid, GameModel> games,
                              IDictionary<Guid, PlayerModel> players,
                              IDictionary<Guid, DeckModel> decks)
        {
            _games = games;
            _players = players;
            _decks = decks;
        }
        public async Task<DeckModel> GetDeck(Guid gameId)
        {            
            DeckModel model = null;

            await Task.Run(() =>
            {
                model = _decks[gameId];
            });

            return model;
        }
        public async Task<bool> GuessNextCard(GuessCardModel model)
        {
            //Check if Guess is correct
            if(model.Guess != "H" && model.Guess != "L")
                throw new ApplicationException($"The guess should of char H or L.");

            Guid gameId = Guid.Parse(model.GameId);
            Guid playerId = Guid.Parse(model.PlayerId);

            if (!_games.ContainsKey(gameId))
                throw new ApplicationException($"Game Id not exists");

            if (!_players.ContainsKey(playerId))
                throw new ApplicationException($"Player Id not exists"); 

            if (_players[playerId].GameId != gameId 
                &&_games[gameId].State != GameState.Started)
                throw new ApplicationException($"The game is not started yet.");

            if (_players[playerId].State != PlayerState.Playing)
                throw new ApplicationException($"Player can not guess the card.");

            await Task.Run(() =>
            {
                ProcessGuessCard(model, gameId, playerId);
            });
            
            return true;
        }
        private void ProcessGuessCard(GuessCardModel model, Guid gameId, Guid playerId)
        {
            //Get random card and set new deck in the list with game id
            var deck = _decks[gameId].Deck;
            var card = Deck.GetRandomCard(ref deck);
            _decks[gameId].Deck = deck;

            //Increase Score
            string outcome = "InCorrect";
            if ((model.Guess == "H" && card.Value > _games[gameId].LastCardValue)
                || (model.Guess == "L" && card.Value < _games[gameId].LastCardValue))
            {
                _players[playerId].Score++;
                outcome = "Correct";
            }

            //Add game Outcomes
            _games[gameId].LastCardValue = card.Value;
            _games[gameId].LastCardPlay = card.Name;
            _games[gameId].OutCome = outcome;

            //Add Game turn count
            _games[gameId].TurnCount++;

            if (deck.Count > 0)
            {
                //Set pointer for next player
                if ((_players[playerId].PlayerIndex > 1
                        && _players[playerId].PlayerIndex < _games[gameId].PlayersJoin)
                        || _players[playerId].PlayerIndex == 1)
                {
                    _games[gameId].NextPlayerIndex = _players[playerId].PlayerIndex;
                    _games[gameId].NextPlayerIndex++;
                }
                else
                    _games[gameId].NextPlayerIndex = 1;

                //Change state of next player
                var nextPlayerId = _players.Values.Where(pl => pl.GameId == gameId 
                                                      && pl.PlayerIndex == _games[gameId].NextPlayerIndex).Single().Id;
                _players[nextPlayerId].State = PlayerState.Playing;                

                //Check for Round complete
                if (_games[gameId].TurnCount == _games[gameId].PlayersJoin)
                {
                    _games[gameId].State = GameState.RoundCompleted;
                    _games[gameId].TurnCount = 0;
                }

                //Change state of current player
                _players[playerId].State = PlayerState.Waiting;
            }
            else
            {
                //End Game
                _games[gameId].State = GameState.Ended;

                var nPlayer = _players.Values.Where(pl => pl.GameId == gameId).ToList();
                foreach (var player in nPlayer)
                    player.State = PlayerState.Free;

                _decks.Remove(gameId);
            }
        }
    }
}
