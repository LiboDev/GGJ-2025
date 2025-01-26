using System.Collections;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    private float oxygen;

    void Start()
    {
        oxygen = Random.Range(1, 4);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().ChangeOxygen(1);
            SelfDestruct();
        }
        else if (other.gameObject.tag == "Weapon")
        {
            SelfDestruct();
        }
    }

    private void SelfDestruct()
    {
        if (Vector2.Distance(transform.position, GameObject.Find("Player").transform.position) > 10f)
        {
            //play sfx
            //play vfx
        }

        Destroy(gameObject);
    }
}
