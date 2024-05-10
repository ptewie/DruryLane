using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    private GameManager Manager;
    // Start is called before the first frame update
    void Start()
    {
        // set rigidbody and game manager
        Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        rb = GetComponent<Rigidbody2D>();   
    }

    // Update is called once per frame
    void Update()
    {
        // set velocity of rigid body (for throwing, at you)
        rb.velocity = Vector2.left * (speed + Manager.spawnSpeedMultiplier);
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // if you end up colliding with the player
        if (other.gameObject.CompareTag("Player"))
        {
            //Let the game manager know that the hit happened
            // TO DO: NOT FUCKING THIS. THIS GOES BETWEEN LIKE...... 3 SCRIPTS!!!!!
            Manager.wasHit = true;

            // 
            Destroy(this.gameObject);
            
            
        }

        //TO DO: Kill plain object, check for tag, if overlaps disable. 

        if (other.gameObject.CompareTag("KillPlane"))
        {
            //disable game object
            this.gameObject.SetActive(false);
            Debug.Log("bye bye silly hazard");
        }
    }
}
