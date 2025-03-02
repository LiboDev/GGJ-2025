using System.Collections;
using UnityEngine;

public class BubbleController : MonoBehaviour
{
    private float oxygen;

    public GameObject pop;

    void Start()
    {
        oxygen = Random.Range(1, 4);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerController>().ChangeOxygen(1);
            AudioManager.Instance.PlaySFX("BubblePickup" + Random.Range(1, 4), 0.5f);
            ScoreManager.Instance.Bubble();

            SelfDestruct();
        }
        else if (other.gameObject.tag == "Weapon")
        {
            SelfDestruct();
        }
    }

    private void SelfDestruct()
    {
        AudioManager.Instance.PlaySFX("Pop"+Random.Range(1,3),0.5f);
        Instantiate(pop, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
