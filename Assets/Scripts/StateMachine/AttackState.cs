using Pathfinding;
using System.Collections;
using UnityEngine;

public class AttackState : State
{
    public ChaseState chaseState;

    private bool attackDone = false;

    private Coroutine attackCoroutine;

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
        transform.GetComponentInParent<Transform>().GetComponentInParent<AIPath>().canMove = false;

        Debug.Log("I have Attacked!");

        yield return new WaitForSeconds(0.8f);

        transform.GetComponentInParent<Transform>().GetComponentInParent<AIPath>().canMove = true;

        attackDone = true;
    }
}
