using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    //transform
    private Rigidbody2D rb;
    public Transform hand;
    public Transform hand1;

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

    private int oxygen = 10;
    [SerializeField] private int health = 10;

    //scene
    private Vector3 mousePos;

    public GameObject spear;
    public GameObject knife;
    public SpearController spearController;

    public Material newMaterial;

    public Slider slider;

    public SpriteRenderer spriteRenderer;

    private Animator animator;

void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

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
            animator.speed = 0;

            RotateTowardsCursor();

            if (Input.GetMouseButton(1) && grappling == false)
            {
                animator.speed = 1;

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
                    GameObject knifeObject = Instantiate(knife, hand1.position, transform.rotation);
                    knifeObject.transform.parent = hand1;

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

        if(Vector2.Distance(transform.position, spear.transform.position) > 2)
        {
            AudioManager.Instance.PlaySFX("SpearReel" + Random.Range(1, 3), 0.25f);

            grappling = true;

            while (Vector2.Distance(transform.position, spear.transform.position) > 1)
            {
                Vector2 direction = spear.transform.position - transform.position;
                direction.Normalize();

                rb.linearVelocity = direction * grappleSpeed;
                yield return null;
            }
        }

        grappling = false;

        StartCoroutine(spearController.Return());
    }

    public void ChangeOxygen(int amount)
    {
        int pot = oxygen + amount;

        if(pot >= 10)
        {
            oxygen = 10;
        }
        else if(pot < 0)
        {
            oxygen = 0;
            GameOver();
        }
        else
        {
            oxygen = pot;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Terrain")
        {
            if (rb.linearVelocity.magnitude > moveSpeed)
            {
                AudioManager.Instance.PlaySFX("PlayerBump" + Random.Range(1, 3), 0.5f);
            }
            else
            {
                AudioManager.Instance.PlaySFX("PlayerBump" + Random.Range(1, 3), 0.1f);
            }
        }
        else if (other.gameObject.tag == "Enemy")
        {
            ChangeHealth(-other.gameObject.GetComponent<StateManager>().damage);
        }
    }

/*    //player flashes white when hit
    private IEnumerator Flash()
    {
        Material normalMaterial = spriteRenderer.material;
        spriteRenderer.material = newMaterial;

        yield return new WaitForSeconds(0.01f);

        spriteRenderer.material = normalMaterial;
    }*/

    //player health
    public void ChangeHealth(int change)
    {
        int pot = change + health;

        if(change < 0)
        {
            //play sound for player taking damage

            AudioManager.Instance.PlaySFX("PlayerDamage" + Random.Range(1, 4), 0.5f);
/*            StartCoroutine(Flash());*/
        }

        if (pot > 10)
        {
            health = 10;
        }
        else if (pot < 1)
        {
            //game over
            ScoreManager.Instance.GameOver();
            
            AudioManager.Instance.PlaySFX("Death" + Random.Range(1, 3), 0.5f);
            AudioManager.Instance.PlaySFX("GameOver", 0.5f);

            health = 0;

            enabled = false;
        }
        else
        {
            health = pot;
        }

        if (slider != null)
        {
            slider.value = health;
        }
    }

    //when spawnpoint enters radius, enable
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "EnemySpawner")
        {
            other.gameObject.GetComponent<EnemySpawner>().Spawn();
        }
    }
/*
    //when spawnpoint exits radius, disable
    private void OnTriggerExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "EnemySpawner")
        {
            
        }
    }*/

    private void GameOver()
    {
        //game over
        Debug.Log("GAME OVER");
    }
}
