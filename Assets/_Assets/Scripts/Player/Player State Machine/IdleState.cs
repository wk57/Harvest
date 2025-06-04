using System;

public class IdleState : IPlayerState
{
    public event Action OnAction;

    public void EnterState(PlayerStateMachine player) { }
    public void UpdateState(PlayerStateMachine player) { }
    public void ExitState(PlayerStateMachine player) { }
}
