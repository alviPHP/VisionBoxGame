using visonBoxGame.DeckCards;
using visonBoxGame.Models;
using visonBoxGame.Models.Response;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace visonBoxGame.Services
{
    public interface IGameService
    {
        IEnumerable<GetAllGamesModel> GetAllGames();
        Task<GameIdModel> CreateGame(GameBindingModel model);
        bool JoinGame(GameIdModel model, short maxPlayerVal);
        Task<bool> StartGame(GameIdModel model, short minPalyersVal);
    }
    public class GameService : IGameService
    {
        private readonly IDictionary<Guid, GameModel> _games;
        private readonly IDictionary<Guid, PlayerModel> _players;
        private readonly IDictionary<Guid, DeckModel> _decks;
        public GameService(IDictionary<Guid, GameModel> games,
                              IDictionary<Guid, PlayerModel> players,
                              IDictionary<Guid, DeckModel> decks)
        {
            _games = games;
            _players = players;
            _decks = decks;
        }
        public IEnumerable<GetAllGamesModel> GetAllGames()
        {
            var result = from game in _games.Values
                         where game.State == GameState.Created
                         select new GetAllGamesModel
                         {
                             GameId = game.Id.ToString(),
                             Title = game.Title
                         };
            return result.ToList();
        }
        public async Task<GameIdModel> CreateGame(GameBindingModel model)
        {
            GameIdModel gameModel = null;

            await Task.Run(() =>
            {
                gameModel = ProcessCreateGame(model);
            });                

            return gameModel;
        }
        public bool JoinGame(GameIdModel model, short maxPlayerVal)
        {
            Guid gameId = Guid.Parse(model.GameId);            

            //Check the limit of players
            if (maxPlayerVal == _games[gameId].PlayersJoin)
                throw new ApplicationException($"The maximum limit of players is {maxPlayerVal}.");

            //Check if game not exists
            if (!_games.ContainsKey(gameId))
                throw new ApplicationException($"Game Id not exists.");

            Guid playerId = Guid.Parse(model.PlayerId);

            //Check if player not exists
            if (!_players.ContainsKey(playerId))
                throw new ApplicationException($"Player Id not exists.");

            //Check if this player already exists in same or other game
            if (_players[playerId].State != PlayerState.Free)
                throw new ApplicationException($"Player can not participate in another game.");

            // Join game
            _players[playerId].GameId = gameId;
            _players[playerId].State = PlayerState.Joined;
            _players[playerId].PlayerIndex = ++_games[gameId].PlayersJoin;

            return true;
        }       
        public async Task<bool> StartGame(GameIdModel model, short minPalyersVal)
        {
            Guid gameId = Guid.Parse(model.GameId);
            Guid playerId = Guid.Parse(model.PlayerId);

            if(!_games.ContainsKey(gameId))
                throw new ApplicationException($"Game Id not exists");

            if (!_players.ContainsKey(playerId))
                throw new ApplicationException($"Player Id not exists");

            if (_games[gameId].PlayersJoin < minPalyersVal)
                throw new ApplicationException($"The minimum players limit is {minPalyersVal} to start the game.");            

            if (_players[playerId].GameId != gameId 
                && _players[playerId].State != PlayerState.Joined)
                throw new ApplicationException($"Player has not joined the game.");

            if (_games[gameId].State != GameState.Created)
                throw new ApplicationException($"The player can not start the game.");

            await Task.Run(() =>
            {
                StartGameProcess(gameId, playerId);
            });          

            return true;
        }
        private void StartGameProcess(Guid gameId, Guid playerId)
        {   
            //Get random card
            var deck = _decks[gameId].Deck;
            var card = Deck.GetRandomCard(ref deck);
            _decks[gameId].Deck = deck;

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

            //Set outcomes for game
            _games[gameId].LastCardValue = card.Value;
            _games[gameId].LastCardPlay = card.Name;
            _games[gameId].OutCome = "Game Started";

            //Change state of game
            _games[gameId].State = GameState.Started;

            //Change state of current player
            _players[playerId].State = PlayerState.Waiting;
        }
        private GameIdModel ProcessCreateGame(GameBindingModel model)
        {
            //Check if game title already exists
            var result = _games.Values.Where(gm => gm.State != GameState.Ended && gm.Title.ToUpper() == model.Title.ToUpper()).Any();
            if (result)
                throw new ApplicationException($"Game title : {model.Title.ToUpper()} already exists.");

            //Create new game
            var gameId = Guid.NewGuid();
            _games.Add(gameId, new GameModel
            {
                Id = gameId,
                Title = model.Title,
                State = GameState.Created,

            });

            // Join game
            var playerId = Guid.NewGuid();
            _players.Add(playerId, new PlayerModel
            {
                GameId = gameId,
                Id = playerId,
                Name = model.PlayerName,
                State = PlayerState.Joined,
                PlayerIndex = ++_games[gameId].PlayersJoin
            });

            //Add new deck of cards
            var deck = new List<Card>();
            Deck.FillDeck(ref deck);
            var deckModel = new DeckModel
            {
                GameId = gameId,
                Deck = deck
            };
            _decks.Add(gameId, deckModel);

            var gameModel = new GameIdModel
            {
                GameId = gameId.ToString(),
                PlayerId = playerId.ToString()
            };

            return gameModel;
        }        
    }
}
