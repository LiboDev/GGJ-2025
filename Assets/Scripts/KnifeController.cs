using UnityEngine;

public class KnifeController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("SelfDestruct", 0.25f);
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("knife damage");

            //deal damage and knockback relative to pos
            //other.GetComponent<EnemyController>().Damage(1,transform.position);
        }
    }
}
