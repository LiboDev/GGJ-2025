using Pathfinding;
using System.Collections;
using UnityEngine;

public class JellyfishAstar : MonoBehaviour
{
    public Transform targetPosition;
    private Vector2 lastCalculatedTargetPosition;

    private Seeker seeker;
    private Rigidbody2D jellyfishRigidbody;

    public Path path;

    public float speed = 2;

    // How much should the jellyfish slow down before it moves again?
    public float jellfishMoveThreshold = 0.2f;

    public float nextWaypointDistance = 3;

    private int currentWaypoint = 0;

    public bool reachedEndOfPath;

    private float rotateTimer = 0f;

    public float savedRotation = 0;

    private bool rotating = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        seeker = GetComponent<Seeker>();
        jellyfishRigidbody = GetComponent<Rigidbody2D>();

        // Start a new path to the targetPosition, call the OnPathComplete function
        // when the path has been calculated (which may take a few frames depending on the complexity)
        seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
        lastCalculatedTargetPosition = targetPosition.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        // Recalculate the path whenever the player moves
        if (Mathf.Abs(targetPosition.position.x - lastCalculatedTargetPosition.x) > 0.1f || Mathf.Abs(targetPosition.position.y - lastCalculatedTargetPosition.y) > 0.1f) {
            seeker.StartPath(transform.position, targetPosition.position, OnPathComplete);
            lastCalculatedTargetPosition = targetPosition.position;
        }

        if (path == null)
        {
            // We have no path to follow yet, so don't do anything
            return;
        }

        // Check in a loop if we are close enough to the currrent waypoint to switch to the next one.
        // We do this in a loop because many waypoints might be close to each other and we may reach
        // several of them in the same frame.
        reachedEndOfPath = false;
        // The distance to the next waypoint in the path
        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, path.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < nextWaypointDistance)
            {
                // Check if there is another waypoint or if we have reached the end of the path
                if (currentWaypoint + 1 < path.vectorPath.Count)
                {
                    currentWaypoint++;
                } else
                {
                    // Set a status variable to indicate that the agent has reached the end of the path.
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        // Slow down smoothly upon approaching the end of the path
        // This value will smoothly go from 1 to 0 as the agent approaches the last waypoint in the path.
        var speedFactor = reachedEndOfPath ? Mathf.Sqrt(distanceToWaypoint / nextWaypointDistance) : 1f;

        // Direction to the next waypoint
        // Normalize it so that it has a length of 1 world unit
        Vector2 dir = (path.vectorPath[currentWaypoint] - transform.position).normalized;
        // Multiply the direction by our desired speed to get a velocity
        Vector2 velocity = dir * speed * 100 * speedFactor;

        Debug.DrawRay(transform.position, velocity);

        // If the jellyfish has slowed down, push again
        if (Mathf.Abs(jellyfishRigidbody.linearVelocityX) < jellfishMoveThreshold && Mathf.Abs(jellyfishRigidbody.linearVelocityY) < jellfishMoveThreshold)
        {
            RotateThenMove(dir, velocity);
        }
    }

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            // Reset the waypoint counter so that we start to move towards the first point in the path
            currentWaypoint = 0;
        }
    }

    private void RotateThenMove(Vector2 targetDir, Vector2 pushVelocity)
    {
        var targetAngle = (Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90) + 360;
        var rot = Mathf.MoveTowardsAngle(transform.rotation.eulerAngles.z, targetAngle, 80 * Time.deltaTime);

        jellyfishRigidbody.MoveRotation(rot);

        if (Mathf.Abs(transform.rotation.eulerAngles.z - targetAngle) < 1f)
        {
            rotateTimer = 0f;
            rotating = false;
            jellyfishRigidbody.AddForce(pushVelocity);
        }
    }
}
