
public class StateMachine
{
    public GameState currentState { get; private set; }
    
    public void Initialize(GameState startState)
    {
        currentState = startState;
        currentState.EnterState();
    }
    
    public void ChangeState(GameState newState)
    {
        currentState.ExitState();
        currentState = newState;
        currentState.EnterState();
    }
}
