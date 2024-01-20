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
        // Check if player has been hit by anything
        HitCheck();

        if (wasHit == false)
        {
            // Increase hazard speed by 0.1 every frame draw. 
            // Yes, this will eventually make them so fast it's impossible to dodge. I'll deal with that later.
            speedMultiplier += Time.deltaTime * 0.1f;
        }

        // Spawn hazards
        SpawnHazards();




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
            Debug.Log("Hit check true!");
            // When the player is hit, decrement the speed variable. 
            speedMultiplier = 0f;

            //TO DO: Punish player in some way? Health, lose score, i dunno monsters?
              
        }
    }
}
