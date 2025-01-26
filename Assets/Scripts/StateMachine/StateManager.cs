using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private State currentState;
    private Vector2 spawnPosition = new Vector2(7.57f, -3.25f);

    public float health = 10;
    public float damage = 10;
    public float hitRecoveryTime = 5;

    // Update is called once per frame
    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        State nextState = currentState?.RunCurrentState();

        if (nextState != null)
        {
            SwitchToTheNextState(nextState);
        }
    }

    private void SwitchToTheNextState(State nextState)
    {
        currentState = nextState;
    }
}
