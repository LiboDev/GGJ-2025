using UnityEngine;

public class JellyfishIdleState : IdleState
{
    [SerializeField] private AttackState attackState;

    public override State RunCurrentState()
    {
        if (canSeeThePlayer)
        {
            return attackState;
        }
        else
        {
            return this;
        }
    }
}
