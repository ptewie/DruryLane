using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spawnObject;
    public GameObject[] spawnPoints;
    public bool wasHit = false;
    public float spawntimer;
    public float timeBetweenSpawns;
    public float speedMultiplier;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Incremntaily increase hazard speeds by 0.1 every frame draw 
        // Yes, this will eventually make the obsticles so fast it's impossible to dodge. i'll cross that bridge when i get to it

        // Make sure wasHit is false
        if (wasHit == false)
        {
            speedMultiplier += Time.deltaTime * 0.1f;
        }
        // Spawn hazards
        SpawnHazards();

        // Check if player has been hit




    }

    public void SpawnHazards()
    {
        // NOTE: turn this into it's own function maybe? Seems unorganized to keep it in Update(); 
        spawntimer += Time.deltaTime;

        if (spawntimer > timeBetweenSpawns)
        {
            // reset spawn timer
            spawntimer = 0f;

            // set random and then choose a spawnPoint to spawn the hazard from
            int random = Random.Range(0, 3);

            // Spawn em'
            Instantiate(spawnObject, spawnPoints[random].transform.position, Quaternion.identity);

        }
    }

    public void HitCheck()
    {
        if(wasHit == true)
        {
            // decrement the speed multiplyer
            speedMultiplier = 0f;

            //TO DO: Punish player in some way? Health, lose score, i dunno monsters?
              
        }
    }
}
