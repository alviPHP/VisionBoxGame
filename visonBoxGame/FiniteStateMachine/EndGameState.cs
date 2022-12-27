namespace visonBoxGame.FiniteStateMachine
{
    /// <summary>
    /// A 'EndGameState' class
    /// </summary>
    public class EndGameState : State
    {
        public override void Handle(StateMachine context)
        {
            context.Game.NextPlayer = null;
            context.Game.State = GameState.Ended;
        }
    }
}
