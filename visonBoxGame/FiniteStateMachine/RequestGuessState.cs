using System.Collections.Generic;
using visonBoxGame.DeckCards;
using visonBoxGame.MockDB;

namespace visonBoxGame.FiniteStateMachine
{
    /// <summary>
    /// A 'RequestGuessState' class
    /// </summary>
    public class RequestGuessState : State
    {
        public override void Handle(StateMachine context)
        {
            
            var game = context.Game;
            var player = context.Player;

            //Generate Random Card
            var lstCard = game.Cards;
            
            for (int i = 0; i < lstCard.Count; i++)
                Deck.GetRandomCard(ref lstCard);

            var card = Deck.GetRandomCard(ref lstCard);

            //Set card values
            game.LastCardValue = card.Value;
            game.LastCardPlay = card.Name;

            string outcome = string.Empty;
            //Increase Score
            if (GetOutCome(player.Guess, card.Value, game.LastCardValue))
            {
                player.Score++;
                outcome = Constants.InCorrect;
            }
            else
                outcome = Constants.InCorrect; 

            //Set outcomes for game                
            game.Result = outcome;
            //Set Turn Count
            game.TurnCount++;

            //Change Statemachine Transition
            if (!game.IsGameEnded)
            {
                //Get Next player 
                var currentNode = game.Players.Find(player);
                var nextPlyerNode = currentNode.NextOrFirst();
                game.NextPlayer = nextPlyerNode.Value;

                game.State = GameState.RequestGuess;
                context.State = new RequestGuessState();
            }
            else
            {
                context.State = new EndGameState();
                context.Request();
            }
        }
        private bool GetOutCome(string Guess, int cardVal1, int cardVal2)
        {
            return ((Guess == Constants.High && cardVal1 > cardVal2) 
                    || (Guess == Constants.Low && cardVal1 < cardVal2));
        }
    }
}
