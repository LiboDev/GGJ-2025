using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<GameObject> enemies = new List<GameObject>();
    public GameObject bubbleBundleObject;

    //enemyspawner
    public void Spawn()
    {
        var rand = Random.Range(0, enemies.Count+1);

        if (rand == enemies.Count)
        {
            GameObject bubbleBundle = Instantiate(bubbleBundleObject, transform.position, Quaternion.identity);
        }
        else
        {
            GameObject enemy = Instantiate(enemies[rand], transform.position, Quaternion.identity);
            var player = GameObject.FindGameObjectWithTag("Player");
            if (enemy.GetComponent<AIAgent>() != null)
            {
                enemy.GetComponent<AIAgent>().target = player.transform;
            }
            else if (enemy.GetComponent<JellyfishAstar>() != null)
            {
                enemy.GetComponent<JellyfishAstar>().targetPosition = player.transform;
            }
/*
            Vector2 force = Random.insideUnitCircle * 3;
            enemy.GetComponent<Rigidbody2D>().linearVelocity = force;*/
        }
    }
}
