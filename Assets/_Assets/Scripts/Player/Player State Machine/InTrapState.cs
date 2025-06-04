using System;
using System.Collections;
using UnityEngine;

public class InTrapState : IPlayerState
{
    public event Action OnAction;

    private Coroutine coroutine;

    public void EnterState(PlayerStateMachine player)
    {
        coroutine = player.StartCoroutine(TrapDuration());
    }

    public void ExitState(PlayerStateMachine player) 
    {
        if (coroutine != null)
        {
            player.StopCoroutine(coroutine);
            coroutine = null;
        }
    }

    public void UpdateState(PlayerStateMachine player) { }

    private IEnumerator TrapDuration()
    {
        yield return new WaitForSeconds(1);
        OnAction?.Invoke();
    }
}
