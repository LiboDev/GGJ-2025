using Pathfinding;
using UnityEngine;

public class IdleState : State
{
    public ChaseState chaseState;
    public bool canSeeThePlayer;

    [SerializeField] private Collider2D visionCollider;

    public override State RunCurrentState()
    {
        transform.GetComponentInParent<Transform>().GetComponentInParent<AIPath>().canMove = false;

        if (canSeeThePlayer)
        {
            transform.GetComponentInParent<Transform>().GetComponentInParent<AIPath>().canMove = true;
            return chaseState;
        }
        else
        {
            return this;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Make sure there are no walls between the enemy and the player
            Vector2 playerDirection = collision.transform.position - transform.position;
            LayerMask mask = LayerMask.GetMask("Terrain", "Player");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, Mathf.Infinity, mask);
            if (hit && hit.transform.tag == "Player")
            {
                canSeeThePlayer = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // Make sure there are no walls between the enemy and the player
            Vector2 playerDirection = collision.transform.position - transform.position;
            LayerMask mask = LayerMask.GetMask("Terrain", "Player");
            RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDirection, Mathf.Infinity, mask);
            if (hit && hit.transform.tag == "Player")
            {
                canSeeThePlayer = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            canSeeThePlayer = false;
        }
    }
}
