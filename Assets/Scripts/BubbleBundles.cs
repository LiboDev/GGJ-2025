using System.Collections;
using UnityEngine;

public class BubbleBundles : MonoBehaviour
{
    public GameObject bubble;

    void Awake()
    {
        var rand = Random.Range(1, 4);

        for (int i = 0; i < rand; i++)
        {
            SpawnBubble();
        }
    }

    private void SpawnBubble()
    {
        GameObject bubbleObject = Instantiate(bubble, transform.position, Quaternion.Euler(new Vector3(0,0,Random.Range(-180f,180f))));
        Vector2 rand = Random.insideUnitCircle * 3;
        bubbleObject.GetComponent<Rigidbody2D>().linearVelocity = rand;
    }
}
