using System.Collections;
using UnityEngine;

public class JellyfishHitState : State
{
    public AttackState attackState;

    private Coroutine hitRecovery;

    private bool recovered = false;

    [SerializeField] private StateManager stateManager;

    [SerializeField] private JellyfishAstar jellyfishPathfinding;

    public override State RunCurrentState()
    {
        if (recovered == false && hitRecovery == null)
        {
            hitRecovery = StartCoroutine(recoverFromHit());
        }
        else if (recovered == true)
        {
            recovered = false;
            return attackState;
        }

        return this;
    }

    private IEnumerator recoverFromHit()
    {
        if (stateManager != null)
        {
            jellyfishPathfinding.canMove = false;

            yield return new WaitForSeconds(stateManager.hitRecoveryTime);

            jellyfishPathfinding.canMove = true;
        }
        else
        {
            Debug.LogError("Error: No AIAgent Found!");
        }

        recovered = true;
    }
}
