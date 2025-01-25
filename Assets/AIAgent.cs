using UnityEngine;
using Pathfinding;

public class AIAgent : MonoBehaviour
{

    private AIPath path;
    [SerializeField] private float moveSpeed;
    [SerializeField] private Transform target;

    [SerializeField] private float stopDistanceThreshold;
    private float distanceToTarget;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        path = GetComponent<AIPath>();
    }

    // Update is called once per frame
    void Update()
    {
        path.maxSpeed = moveSpeed;

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
