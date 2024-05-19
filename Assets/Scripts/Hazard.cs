using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour, IKillable
{
    private Rigidbody2D rb;
    public float speed;
    private GameManager Manager;
    public string hazardType;

    // Start is called before the first frame update
    void Start()
    {
        // set rigidbody and game manager
        Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        Manager.hazardsPool.Add(this);
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        // set velocity of rigid body (for throwing, at you)
        rb.velocity = Vector2.left * (speed + Manager.spawnSpeedMultiplier);
        
    }

    public void Die()
    {
        gameObject.SetActive(false); 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if you end up colliding with the player
        if (other.gameObject.CompareTag("Player"))
        {
            //Let the game manager know that the hit happened
            // TO DO: NOT FUCKING THIS. THIS GOES BETWEEN LIKE...... 3 SCRIPTS!!!!!
            Manager.wasHit = true;

            //disable game object
            Die();
            
            
        }

        //TO DO: Kill plain object, check for tag, if overlaps disable. 

        if (other.gameObject.CompareTag("KillPlane"))
        {
            //disable game object
            Die();
            Debug.Log("bye bye silly hazard");
        }
    }

}