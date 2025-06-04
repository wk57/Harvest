using System;

public interface IPlayerState
{
    public event Action OnAction;

    void EnterState(PlayerStateMachine player);
    void UpdateState(PlayerStateMachine player);
    void ExitState(PlayerStateMachine player);
}
