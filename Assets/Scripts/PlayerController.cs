using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //transform
    private Rigidbody2D rb;
    public Transform hand;

    //stats
    public float moveSpeed = 5f;
    public float rotateDelay = 2;

    public float spearAttackTime;
    public float spearSpeed;
    public float grappleSpeed;

    public float knifeAttackTime;

    //tracking
    public bool spearThrown = false;
    public bool grappling = false;

    private float timeElapsed = 0f;

    //scene
    private Vector3 mousePos;

    public GameObject spear;
    public GameObject knife;
    public SpearController spearController;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        StartCoroutine(Action());
    }

    void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f;
    }

    private IEnumerator Action()
    {
        while (true)
        {
            RotateTowardsCursor();

            if (Input.GetMouseButton(1) && grappling == false)
            {
                if(rb.linearVelocity.magnitude > moveSpeed)
                {
                    Vector2 vel = Vector2.Lerp(rb.linearVelocity, transform.right * moveSpeed, timeElapsed / rotateDelay);
                    rb.linearVelocity = vel;

                    timeElapsed += Time.deltaTime;

                    if (timeElapsed > rotateDelay)
                    {
                        timeElapsed = 0;
                    }
                }
                else
                {
                    rb.linearVelocity = transform.right * moveSpeed;
                }

            }

            if(Input.GetMouseButton(0))
            {
                //attack
                if(spearThrown == false && grappling == false)
                {
                    StartCoroutine(spearController.Throw(spearSpeed));


                    spearThrown = true;

                    yield return new WaitForSeconds(spearAttackTime);
                }
                else
                {
                    GameObject knifeObject = Instantiate(knife, hand.position, transform.rotation);
                    knifeObject.transform.parent = hand;

                    yield return new WaitForSeconds(knifeAttackTime);
                }


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

    public IEnumerator Grapple()
    {
        yield return new WaitForSeconds(0.25f);

        grappling = true;

        while (Vector2.Distance(transform.position, spear.transform.position) > 1)
        {
            Vector2 direction = spear.transform.position - transform.position;
            direction.Normalize();

            rb.linearVelocity = direction * grappleSpeed;
            yield return null;
        }

        grappling = false;
        spearThrown = false;

        StartCoroutine(spearController.Return());
    }
}
