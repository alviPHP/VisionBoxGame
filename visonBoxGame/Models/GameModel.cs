using System;
using System.Collections.Generic;
using visonBoxGame.DeckCards;

namespace visonBoxGame.MockDB
{
    public class GameModel
    {
        private int turnCount;
        private bool isRoundComplete;
        private LinkedList<PlayerModel> players;
        private List<Card> cards;

        public GameModel()
        {
            players = new LinkedList<PlayerModel>();
            cards = new List<Card>();
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public GameState State
        {
            get;
            set;
        }
        public int TurnCount
        {
            get
            {
                return turnCount;
            }
            set
            {
                isRoundComplete = false;
                turnCount = value;

                if (Players.Count == turnCount)
                {
                    turnCount = 0;
                    isRoundComplete = true;
                }
            }
        }
        public int PlayersJoined
        {
            get => players.Count;
        }
        public LinkedList<PlayerModel> Players { get => players; set => players = value; }
        public List<Card> Cards { get => cards; set => cards = value; }
        public bool IsGameEnded
        {
            get
            {
                if (Cards == null)
                    return false;
                return Cards.Count == 0;
            }
        }
        public bool IsRoundComplete
        {
            get { return isRoundComplete; }
        }
        public PlayerModel NextPlayer
        {
            get; set;
        }
        public string Result { get; set; }
        public string LastCardPlay { get; set; }
        public int LastCardValue { get; set; }
    }
}
