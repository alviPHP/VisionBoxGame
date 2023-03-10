using visonBoxGame.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using visonBoxGame.MockDB;

namespace visonBoxGame.Services
{
    public interface IScoreService
    {
        List<ScoreModel> GetScoreTable(Guid gameId);
    }
    public class ScoreService : IScoreService
    {   
        private readonly IList<GameModel> _games;
        public ScoreService(IList<GameModel> games)
        {
            _games = games;
        }
        public List<ScoreModel> GetScoreTable(Guid gameId)
        {
            var game = GetGame(gameId);
            List<ScoreModel> lstScoreTable = null;

            if (game.IsRoundComplete || game.State==GameState.Ended)
            {
                lstScoreTable = new List<ScoreModel>();
                foreach (var player in game.Players)
                {
                    lstScoreTable.Add(new ScoreModel
                    {
                        PlayerName = player.Name,
                        Score = player.Score,
                    });
                }
            }
            return lstScoreTable;
        }
        private GameModel GetGame(Guid gameId)
        {
            return _games.AsParallel().Where(gm => gm.Id == gameId).Single();
        }
    }
}
