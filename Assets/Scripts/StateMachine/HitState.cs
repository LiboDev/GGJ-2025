using UnityEngine;

public class HitState : State
{
    public ChaseState chaseState;

    public override State RunCurrentState()
    {
        return chaseState;
    }
}
