using Pathfinding;
using System.Collections;
using UnityEngine;

public class AttackState : State
{
    public ChaseState chaseState;

    public bool attackDone = false;

    private Coroutine attackCoroutine;

    [SerializeField] private AIAgent aiAgent;

    public override State RunCurrentState()
    {
        // Check whether enemy is finished attacking or not; if it is, return to chase state
        if (attackDone == true)
        {
            attackDone = false;
            attackCoroutine = null;
            return chaseState;
        }

        // If the enemy is not currently attacking, attack
        if (attackCoroutine == null)
        {
            attackCoroutine = StartCoroutine(Attack());
        }

        return this;
    }

    private IEnumerator Attack()
    {
        // Stop the enemy while it winds up
        aiAgent.currentMoveSpeed = 0f;

        yield return new WaitForSeconds(aiAgent.lungeWindup);

        var t = 0f;

        // Quickly burst it forward before coming to a halt
        while (t < 1.0f)
        {
            aiAgent.currentMoveSpeed = Mathf.Lerp(0, aiAgent.lungeDistance, t);

            // How fast the enemy should accelerate
            t += 1f * Time.deltaTime;

            yield return null;
        }

        Debug.Log("I have Attacked!");

        aiAgent.currentMoveSpeed = 0;

        // Wait till the enemy is recovered then reset move speed
        yield return new WaitForSeconds(aiAgent.lungeRecovery);

        aiAgent.currentMoveSpeed = aiAgent.moveSpeed;

        attackDone = true;
    }
}
