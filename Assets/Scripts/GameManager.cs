using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject spawnObject;
    public GameObject[] spawnPoints;
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
        speedMultiplier += Time.deltaTime * 0.1f;

        // NOTE: turn this into it's own function maybe?
        spawntimer += Time.deltaTime;

        if(spawntimer > timeBetweenSpawns)
        {
            spawntimer = 0;
            int random = Random.Range(0, 3);
            Instantiate(spawnObject, spawnPoints[random].transform.position, Quaternion.identity);

        }

    }
}
