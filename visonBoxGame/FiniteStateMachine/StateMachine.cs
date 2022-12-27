using System;
using System.Collections.Generic;
using visonBoxGame.DeckCards;
using visonBoxGame.MockDB;
using visonBoxGame.Models;
using visonBoxGame.FiniteStateMachine;

namespace visonBoxGame
{
    public class StateMachine
    {
        private State _state;
        private GameModel _game;
        private PlayerModel _player;

        // Constructor
        public StateMachine(State state, GameModel game)
        {
            this.State = state;
            this.Game = game;
        }
        // Gets or sets the state
        public State State
        {
            get { return _state; }
            set
            {
                _state = value;
            }
        }

        // Gets or sets the games
        public GameModel Game
        {
            get { return _game; }
            set
            {
                _game = value;
            }
        }

        // Gets or sets the players
        public PlayerModel Player
        {
            get { return _player; }
            set
            {
                _player = value;
            }
        }
        public void Request()
        {
            _state.Handle(this);
        }
    }
}
