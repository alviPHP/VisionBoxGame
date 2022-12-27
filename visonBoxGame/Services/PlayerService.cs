using System;
using System.Collections.Generic;
using System.Linq;
using visonBoxGame.MockDB;

namespace visonBoxGame.Services
{
    public interface IPlayerService
    {
        PlayerModel GetPlayer(Guid playerId, Guid gameId);
        Guid AddPlayer(string name, Guid gameId);
        bool NameExists(string name, Guid gameId);
    }
    public class PlayerService : IPlayerService
    {
        private readonly IDictionary<Guid, GameModel> _games;
        public PlayerService(IDictionary<Guid, GameModel> games)
        {
            _games = games;
        }
        public Guid AddPlayer(string name, Guid gameId)
        {
            GameModel game = GetGame(gameId);
            var player = new PlayerModel
            {
                Id = Guid.NewGuid(),
                Name = name
            };
            game.Players.AddLast(player);
            return player.Id;
        }
        public PlayerModel GetPlayer(Guid playerId, Guid gameId)
        {
            return GetGame(gameId).Players.Where(pl=> pl.Id == playerId).Single();
        }

        public bool NameExists(string playerName, Guid gameId)
        {
            return GetGame(gameId).Players.Where(pl => pl.Name.ToUpper() == playerName.ToUpper()).Any();
                          
        }
        private GameModel GetGame(Guid gameId)
        {
            return _games.Values.AsParallel().Where(gm => gm.Id == gameId).Single();
        }
    }
}
