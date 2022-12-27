using visonBoxGame.DeckCards;

namespace visonBoxGame.FiniteStateMachine
{
    /// <summary>
    /// A 'StartGameState' class
    /// </summary>
    public class StartGameState : State
    {
        public override void Handle(StateMachine context)
        {
            var game = context.Game;
            var player = context.Player;

            //Generate Random Card.
            var lstCard = game.Cards;
            var card = Deck.GetRandomCard(ref lstCard);

            //Set outcomes for game.
            game.LastCardValue = card.Value;
            game.LastCardPlay = card.Name;
            game.Result = Constants.GameStarted;

            //Get Next player 
            var currentNode = game.Players.Find(player);
            var nextPlyerNode = currentNode.NextOrFirst();
            game.NextPlayer = nextPlyerNode.Value;

            //Set game state.
            game.State = GameState.Started;

            //Change Statemachine Transition.
            context.State = new RequestGuessState();
        }
    }
}
