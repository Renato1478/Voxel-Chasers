using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{
    private Transform player; //the enemy's target
    public GameObject[] enemies;

    public float timeBetweenEnemies;
    private float enemyGenCounter;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform; //target the player
    }

    // Start is called before the first frame update
    void Start()
    {
        enemyGenCounter = timeBetweenEnemies;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager._canMove)
        {
            enemyGenCounter -= Time.deltaTime;

            if (enemyGenCounter <= 0)
            {
                int chosenEnemy = Random.Range(0, enemies.Length);
                float distanceX = Random.Range(-20f, 20f);
                float distanceZ = Random.Range(-20f, 20f);
                Instantiate(enemies[chosenEnemy], player.transform.position + new Vector3(distanceX, 40f, distanceZ), Quaternion.Euler(0f, Random.Range(-45f, 45f), 0f));

                enemyGenCounter = Random.Range(timeBetweenEnemies * 0.75f, timeBetweenEnemies * 1.25f);
            }
        }
    }
}
