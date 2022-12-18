using visonBoxGame.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace visonBoxGame.Services
{
    public interface IScoreService
    {
        Task<ScoreTableModel> GetScoreTable(Guid playerId);
        Task<GameResultModel> GetResult(Guid gameId);
    }
    public class ScoreService : IScoreService
    {
        private readonly IDictionary<Guid, GameModel> _games;
        private readonly IDictionary<Guid, PlayerModel> _players;
        public ScoreService(IDictionary<Guid, GameModel> games,
                              IDictionary<Guid, PlayerModel> players)
        {
            _games = games;
            _players = players;
        }
        public async Task<ScoreTableModel> GetScoreTable(Guid gameId)
        {
            ScoreTableModel scoreTable = null;

            await Task.Run(() =>
            {
                if (_games[gameId].State == GameState.RoundCompleted
                     || _games[gameId].State == GameState.Ended)
                {
                    var lstPlayers = _players.Values.Where(pl => pl.GameId == gameId).ToList();
                    
                    scoreTable = new ScoreTableModel();
                    scoreTable.Table = new List<ScoreModel>();

                    foreach (var player in lstPlayers)
                    {
                        scoreTable.Table.Add(
                                new ScoreModel()
                                {
                                    PlayerName = player.Name,
                                    Score = player.Score,
                                });
                    }
                }
            });            

            return scoreTable;
        }
        public async Task<GameResultModel> GetResult(Guid gameId)
        {
            GameResultModel result = null;
            Guid nextPlayerId = Guid.Empty;
            string nextPlayerName = null;

            await Task.Run(() =>
            {
                if(_games[gameId].State != GameState.Ended)
                {
                    var nPlayer = _players.Values.Where(pl => pl.GameId == gameId && pl.State == PlayerState.Playing).Single();
                    nextPlayerId = nPlayer.Id;
                    nextPlayerName = nPlayer.Name;
                }

                result = new GameResultModel
                {
                    NextPlayerId = nextPlayerId,
                    NextPlayerName = nextPlayerName,
                    LastCardPlay = _games[gameId].LastCardPlay,
                    Result = _games[gameId].OutCome
                };

                return result;
            });
                
            return result;
        }
    }
}
