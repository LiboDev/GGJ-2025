using UnityEngine;
using System.Collections;

public class SpearController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D rb;

    private PlayerController playerController;
    public Transform playerTransform;
    public Transform handTransform;
    private BoxCollider2D bc;
    private LineRenderer lineRenderer;

    private bool hit = false;
    private bool returning = false;
    private bool thrown = false;

    private float speed;

    private Quaternion rot;

    private float targetAngle;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;

        bc = GetComponent<BoxCollider2D>();
        lineRenderer = GetComponent<LineRenderer>();

        playerController = transform.parent.GetComponent<PlayerController>();
        //playerTransform = transform.parent;
    }

    void Update()
    {
        lineRenderer.SetPosition(0, handTransform.position); // Start of the line
        lineRenderer.SetPosition(1, transform.position); // End of the line

        /*        var currentAngle = SmoothAngleTransition(transform.rotation.z, targetAngle, 2f);
                transform.rotation = Quaternion.Euler(0f, 0f, currentAngle); // Example for 2D rotation*/
    }

    public IEnumerator Throw(float speed)
    {
        AudioManager.Instance.PlaySFX("SpearThrow" + Random.Range(1, 4), 0.3f);

        hit = false;
        returning = false;
        thrown = true;

        this.speed = speed;

        transform.parent = null;
        rb.bodyType = RigidbodyType2D.Dynamic;

        transform.rotation = playerTransform.rotation;

        rb.linearVelocity = transform.right * speed * 2f;

        bc.enabled = true;

        yield return new WaitUntil(() => rb.linearVelocity.magnitude < 0.1f);
        AudioManager.Instance.PlaySFX("SpearReel" + Random.Range(1, 3), 0.25f);

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
                AudioManager.Instance.PlaySFX("SpearHitWall" + Random.Range(1, 4), 1f);
            }
            else if(other.gameObject.tag == "Enemy")
            {
                AudioManager.Instance.PlaySFX("SpearHitEnemy" + Random.Range(1, 4), 1f);
                //other.gameObject.GetComponent<EnemyController>().Damage();
            }
        }

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (returning == false && thrown == true)
        {
            if (other.gameObject.tag == "Terrain")
            {
                rb.linearVelocity = Vector2.zero;

                Debug.Log("grapple");
                StartCoroutine(playerController.Grapple());
                hit = true;
                returning = true;
                rb.linearVelocity = Vector3.zero;
            }
            else if (other.gameObject.tag == "Enemy")
            {
                rb.linearVelocity = Vector2.zero;

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

        if(Vector2.Distance(transform.position, handTransform.position) > 2)
        {
            AudioManager.Instance.PlaySFX("SpearReel" + Random.Range(1, 3), 0.25f);

            //enemy.position = transform.position;
            enemy.parent = transform;

            while(Vector2.Distance(enemy.position, handTransform.position) > 2)
            {
                Vector2 direction = handTransform.position - transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 180f + angle));

                transform.position = Vector2.MoveTowards(transform.position, handTransform.position, speed * Time.deltaTime); ;
                yield return null;
            }

            if(enemy != null)
            {
                enemy.parent = null;
            }
        }

        StartCoroutine(Return());
    }

    public IEnumerator Return()
    {
        bc.enabled = false;
        returning = true;

        while (Vector2.Distance(transform.position, handTransform.position) > 0.1f)
        {
            Vector2 direction = handTransform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0,0,180f+angle));

            transform.position = Vector2.MoveTowards(transform.position, handTransform.position, speed * Time.deltaTime);
            yield return null;
        }


        hit = false;
        returning = false;
        rb.bodyType = RigidbodyType2D.Kinematic;

        transform.rotation = playerTransform.rotation;
        thrown = false;

        transform.position = handTransform.position;
        transform.parent = handTransform;

        AudioManager.Instance.PlaySFX("SpearDetach" + Random.Range(1, 3), 0.5f);
        yield return new WaitForSeconds(0.25f);
        playerController.spearThrown = false;
        
    }

/*    float SmoothAngleTransition(float fromAngle, float toAngle, float speed)
    {
        // Smoothly interpolate the angle

        Debug.Log("target angle " + targetAngle);
        return Mathf.Lerp(fromAngle, toAngle, speed * Time.deltaTime);
    }*/
}
