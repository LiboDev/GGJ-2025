using System.Collections;
using UnityEngine;

public class HitState : State
{
    public ChaseState chaseState;

    private Coroutine hitRecovery;

    private bool recovered = false;

    [SerializeField] private AIAgent aiAgent;
    [SerializeField] private StateManager stateManager;

    public override State RunCurrentState()
    {
        if (recovered == false && hitRecovery == null)
        {
            hitRecovery = StartCoroutine(recoverFromHit());
        }
        else if (recovered == true)
        {
            recovered = false;
            return chaseState;
        }

        return this;
    }

    private IEnumerator recoverFromHit()
    {
        if (aiAgent != null)
        {
            aiAgent.currentMoveSpeed = 0f;

            yield return new WaitForSeconds(stateManager.hitRecoveryTime);

            aiAgent.currentMoveSpeed = aiAgent.moveSpeed;
        }
        else
        {
            Debug.LogError("Error: No AIAgent Found!");
        }

        recovered = true;
    }
}
