using UnityEngine;
using System.Collections;

public class KnifeController : MonoBehaviour
{
    public GameObject enemyParticles;
    public GameObject wallParticles;

    public Material newMaterial;

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
            var enemyManager = other.gameObject.GetComponent<StateManager>();

            if (enemyManager != null)
            {
                enemyManager.takeDamage(1);
                /*StartCoroutine(Flash(other.gameObject.GetComponent<SpriteRenderer>()));*/
            }


            AudioManager.Instance.PlaySFX("KnifeHitEnemy" + Random.Range(1, 4), 0.3f);

            CameraShake.Instance.ShakeCamera(10f, 0.1f);
            CameraShake.Instance.FreezeFrame(0.1f);

            Instantiate(enemyParticles, other.transform.position, Quaternion.identity);

            //deal damage and knockback relative to pos
            //other.GetComponent<EnemyController>().Damage(1,transform.position);

            hit = true;
        }
        else if (other.gameObject.tag == "Terrain")
        {
            AudioManager.Instance.PlaySFX("KnifeHitWall" + Random.Range(1, 4), 0.4f);

            Instantiate(wallParticles, transform.position + transform.right, Quaternion.identity);

            hit = true;
        }
    }

    //make enemies flash white when hit
/*    private IEnumerator Flash(SpriteRenderer other)
    {
        Material normalMaterial = other.material;
        other.material = newMaterial;

        yield return new WaitForSeconds(0.01f);

        other.material = normalMaterial;
    }*/
}
