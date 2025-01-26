using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private State currentState;
    [SerializeField] private State hitState;
    private Vector2 spawnPosition = new Vector2(7.57f, -3.25f);

    [SerializeField] private float health = 3;
    public int damage = 10;
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

    public void takeDamage(int damageTaken)
    {
        health -= damageTaken;

        currentState = hitState;

        if (health <= 0)
        {
            ScoreManager.Instance.Kill();
            CameraShake.Instance.FreezeFrame(0.1f);
            Destroy(this.gameObject, 0.1f);
            return;
        }
    }
}
