using UnityEngine;
using Pathfinding;

public class AIAgent : MonoBehaviour
{

    private AIPath path;
    public float moveSpeed;
    public float currentMoveSpeed;
    public Transform target;

    [SerializeField] private float stopDistanceThreshold;
    private float distanceToTarget;

    public float lungeDistance = 5;
    public float lungeWindup = 5;
    public float lungeRecovery = 5;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = GetComponent<AIPath>();
        currentMoveSpeed = moveSpeed;
        path.maxSpeed = moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        path.maxSpeed = currentMoveSpeed;

        // Stop the enemy a set distance away from the player
        distanceToTarget = Vector2.Distance(transform.position, target.position);
        if (distanceToTarget < stopDistanceThreshold)
        {
            path.destination = transform.position;
        }
        else
        {
            path.destination = target.position;
        }
    }
}
