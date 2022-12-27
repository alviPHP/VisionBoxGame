namespace visonBoxGame.FiniteStateMachine
{
    /// <summary>
    /// The 'State' abstract class
    /// </summary>
    public abstract class State
    {
        public abstract void Handle(StateMachine context);
    }
}
