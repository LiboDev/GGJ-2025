using UnityEngine;
using System.Collections;

public class SpearController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D rb;

    private PlayerController playerController;
    private Transform playerTransform;
    private BoxCollider2D bc;

    private bool hit = false;
    private bool returning = false;
    private bool thrown = false;

    private float speed;

    private Quaternion rot;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        bc = GetComponent<BoxCollider2D>();

        playerController = transform.parent.GetComponent<PlayerController>();
        playerTransform = transform.parent;
    }

    public IEnumerator Throw(float speed)
    {
        

        thrown = true;

        this.speed = speed;

        transform.parent = null;
        rb.bodyType = RigidbodyType2D.Dynamic;

        transform.rotation = playerTransform.rotation;

        rb.linearVelocity = transform.right * speed * 2f;

        yield return new WaitForSeconds(0.1f);
        bc.enabled = true;

        yield return new WaitUntil(() => rb.linearVelocity.magnitude < 0.1f);

        Debug.Log("return automatically");

        if(hit == false)
        {
            StartCoroutine(Return());
            returning = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(returning == false && thrown == true)
        {
            if (other.gameObject.tag == "Terrain")
            {
                Debug.Log("grapple");
                StartCoroutine(playerController.Grapple());
                hit = true;
                returning = true;
                rb.linearVelocity = Vector3.zero;
            }
            else if(other.gameObject.tag == "Enemy")
            {
                Debug.Log("hook");
                StartCoroutine(Hook(other.transform));
                hit = true;
                returning = true;
                rb.linearVelocity = Vector3.zero;
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (returning == false && thrown == true)
        {
            if (other.gameObject.tag == "Terrain")
            {
                Debug.Log("grapple");
                StartCoroutine(playerController.Grapple());
                hit = true;
                returning = true;
                rb.linearVelocity = Vector3.zero;
            }
            else if (other.gameObject.tag == "Enemy")
            {
                Debug.Log("hook");
                StartCoroutine(Hook(other.transform));
                hit = true;
                returning = true;
                rb.linearVelocity = Vector3.zero;
            }
        }

    }

    private IEnumerator Hook(Transform enemy)
    {
        bc.enabled = false;

        yield return new WaitForSeconds(0.5f);

        enemy.position = transform.position;
        enemy.parent = transform;

        while(Vector2.Distance(transform.position, playerTransform.position) > 3)
        {
            Vector2 direction = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0,0,angle));

            transform.position = Vector2.Lerp(transform.position, playerTransform.position, Time.deltaTime);
            yield return null;
        }

        if(enemy != null)
        {
            enemy.parent = null;
        }

        StartCoroutine(Return());
    }

    public IEnumerator Return()
    {
        bc.enabled = false;
        returning = true;

        while (Vector2.Distance(transform.position, playerTransform.position) > 0.1f)
        {
            Vector2 direction = playerTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, speed * Time.deltaTime);
            yield return null;
        }



        hit = false;
        returning = false;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.rotation = playerTransform.rotation;
        playerController.spearThrown = false;
        thrown = false;

        transform.position = playerTransform.position;
        transform.parent = playerTransform;
    }
}
