using visonBoxGame.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace visonBoxGame.Services
{
    public interface IPlayerService
    {
       Task<string> AddPlayer(string playerName);        
    }
    public class PlayerService : IPlayerService
    {
        private readonly IDictionary<Guid, GameModel> _games;
        private readonly IDictionary<Guid, PlayerModel> _players;
        public PlayerService(IDictionary<Guid, GameModel> games, 
                             IDictionary<Guid, PlayerModel> players)
        {
            _games = games;
            _players = players;
        }
        public async Task<string> AddPlayer(string playerName)
        {            
            var playerId = Guid.NewGuid();

            // Add Player
            await Task.Run(() =>
            {
                //Check if player name already exists in the game that are need to join
                foreach (var game in _games.Values)
                {
                    if (game.State == GameState.Created)
                    {
                        var result = _players.Values.Where(pl => pl.GameId == game.Id && pl.Name.ToUpper() == playerName.ToUpper()).Any();

                        if (result)
                            throw new ApplicationException($"Player name {playerName} already exists.");
                    }
                }
                
                _players.Add(playerId, new PlayerModel
                {
                    Id = playerId,
                    Name = playerName,
                    State = PlayerState.Free
                });
            });

            return playerId.ToString();
        }
    }
}
