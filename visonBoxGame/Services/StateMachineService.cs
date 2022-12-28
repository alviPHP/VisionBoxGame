using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using visonBoxGame.FiniteStateMachine;
using visonBoxGame.MockDB;

namespace visonBoxGame.Services
{
    public interface IStateMachineService
    {
        void Create(Guid gameId);
        void PlayTurn(Guid gameId,PlayerModel player);
    }
    public class StateMachineService : IStateMachineService
    {
        
        public readonly IList<StateMachine> _stateMachines;
        public readonly IList<GameModel> _games;
        public StateMachineService(IList<StateMachine> stateMachine,
                                   IList<GameModel> games)
        {
            _stateMachines = stateMachine;
            _games = games;
        }
        public void Create(Guid gameId)
        {
            var game = GetGame(gameId);
            _stateMachines.Add(new StateMachine(new StartGameState(),game));
        }
        public void PlayTurn(Guid gameId,PlayerModel player)
        {
            var statemachine = GetStateMachine(gameId);
            statemachine.Player = player;
            statemachine.Request();            
        }

        private GameModel GetGame(Guid gameId)
        {
            return _games.AsParallel().Where(gm => gm.Id == gameId).Single();
        }
        private StateMachine GetStateMachine(Guid gameId)
        {
            return _stateMachines.AsParallel().Where(stmac => stmac.Game.Id == gameId).Single();
        }
    }
}
