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
            var card = Deck.GetRandomCard(ref lstCard);

            //Set card values
            game.LastCardValue = card.Value;
            game.LastCardPlay = card.Name;

            //Increase Score
            if (GetOutCome(player.Guess, card.Value, game.LastCardValue))
            {
                player.Score++;
                game.Result = Constants.Correct;
            }
            else
                game.Result = Constants.InCorrect; 

            //Set Turn Count
            game.TurnCount++;

            //Change Statemachine Transition
            if (!game.IsGameEnded)
            {
                //Get Next player 
                game.NextPlayer = game.Players.Find(player)
                                      .NextOrFirst().Value;

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
