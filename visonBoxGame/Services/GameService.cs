using System;
using System.Collections.Generic;
using System.Linq;
using visonBoxGame.MockDB;
using visonBoxGame.Models;

namespace visonBoxGame.Services
{
    public interface IGameService
    {
        List<GetAllGamesModel> GetGames();
        GameModel GetGameById(Guid id);
        GameModel AddGame(GameBindingModel model);
        GameResultModel GetResult(Guid gameId);
        bool Validate(Guid gameId);
        bool TitleExists(string title);

    }
    public class GameService : IGameService
    {
        private readonly IDictionary<Guid, GameModel> _games;
        public GameService(IDictionary<Guid, GameModel> games)
        {
            _games = games;
        }
        public GameModel AddGame(GameBindingModel model)
        {
            var game = new GameModel
            {
                Id = Guid.NewGuid(),
                Title = model.Title,
                State = GameState.Created,
            };
            _games.Add(game.Id, game);
            return game;
        }
        public GameModel GetGameById(Guid id)
        {
            return _games.Values.AsParallel().Where(gm=> gm.Id==id && gm.State == GameState.Created).Single();
        }
        public List<GetAllGamesModel> GetGames()
        {
            var result = from game in _games.Values.AsParallel()
                         where game.State == GameState.Created
                         select new GetAllGamesModel
                         {
                             GameId = game.Id.ToString(),
                             Title = game.Title
                         };
            return result.ToList();
        }
        public GameResultModel GetResult(Guid gameId)
        {
            var game = _games.Values.AsParallel().Where(gm => gm.Id == gameId).Single();
            var model = new GameResultModel();
            model.LastCardPlay = game.LastCardPlay;
            if(game.NextPlayer != null)
            {
                model.NextPlayerId = game.NextPlayer.Id;
                model.NextPlayerName = game.NextPlayer.Name;
            }
            model.Result = game.Result;
            model.GameEnded = game.IsGameEnded;
            model.RoundComplete = game.IsRoundComplete;
            return model;
        }

        public bool TitleExists(string title)
        {
            var result = GetGames().Where(gm => gm.Title.ToUpper() == title.ToUpper());
            if (result.Any())
                return true;
            return false;
        }

        public bool Validate(Guid gameId)
        {
            var result = _games.Values.AsParallel().Where(gm => gm.Id == gameId
                                            && !(gm.State == GameState.Created || gm.State == GameState.Ended)).Any();
            return result;
        }
    }
}
