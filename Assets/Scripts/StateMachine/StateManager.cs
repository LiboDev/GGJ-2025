using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private State currentState;
    private Vector2 spawnPosition = new Vector2(7.57f, -3.25f);

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
