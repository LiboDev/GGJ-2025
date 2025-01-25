using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //transform
    private Rigidbody2D rb;

    //stats
    public float moveSpeed = 5f;

    //scene
    private Vector3 mousePos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        rb.linearDamping = 3f;

        StartCoroutine(Move());
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
    }

    private IEnumerator Move()
    {
        while (true)
        {
            RotateTowardsCursor();

            if (Input.GetMouseButton(0))
            {
                rb.linearVelocity = transform.right * moveSpeed;
            }
            else
            {

            }

            yield return null;
        }
    }

    void RotateTowardsCursor()
    {
        Vector3 direction = mousePos - transform.position;

        // Rotate the player to face the direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
    }
}
