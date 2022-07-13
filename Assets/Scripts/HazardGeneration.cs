using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardGeneration : MonoBehaviour
{
    private Transform player; //the enemy's target
    public GameObject[] hazards;

    public float timeBetweenHazards;
    private float hazardGenCounter;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform; //target the player
    }

    // Start is called before the first frame update
    void Start()
    {
        hazardGenCounter = timeBetweenHazards;
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager._canMove)
        {
            hazardGenCounter -= Time.deltaTime;

            if (hazardGenCounter <= 0)
            {
                int chosenHazard = Random.Range(0, hazards.Length);
                float distanceX = Random.Range(-10f, 10f);
                float distanceZ = Random.Range(-10f, 10f);
                Instantiate(hazards[chosenHazard], player.transform.position + new Vector3(distanceX, 40f, distanceZ), Quaternion.Euler(0f, Random.Range(-45f, 45f), 0f));

                hazardGenCounter = Random.Range(timeBetweenHazards * 0.75f, timeBetweenHazards * 1.25f);
            }
        }
    }
}
