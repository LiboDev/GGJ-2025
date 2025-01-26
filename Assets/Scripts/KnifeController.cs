using UnityEngine;

public class KnifeController : MonoBehaviour
{
    private bool hit = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Invoke("SelfDestruct", 0.25f);
    }

    private void SelfDestruct()
    {
        if (hit == false)
        {
            AudioManager.Instance.PlaySFX("WeaponMiss" + Random.Range(1, 5), 0.2f);
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Debug.Log("knife damage");
            AudioManager.Instance.PlaySFX("KnifeHitEnemy" + Random.Range(1, 4), 0.3f);



            CameraShake.Instance.ShakeCamera(10f, 0.1f);
            //deal damage and knockback relative to pos
            //other.GetComponent<EnemyController>().Damage(1,transform.position);

            hit = true;
        }
        else if (other.gameObject.tag == "Terrain")
        {
            AudioManager.Instance.PlaySFX("KnifeHitWall" + Random.Range(1, 4), 0.4f);

            hit = true;
        }
    }
}
