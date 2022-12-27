using visonBoxGame.MockDB;

namespace visonBoxGame.Models
{
    public class GameContextModel
    {
        private GameModel _game;
        private PlayerModel _player;
        private string _guess;

        public GameModel Game { get => _game; set => _game = value; }
        public PlayerModel Player { get => _player; set => _player = value; }
        public string Guess { get => _guess; set => _guess = value; }
    }
}
