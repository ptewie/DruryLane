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
        rb.velocity = Vector2.left * (speed + Manager.speedMultiplier);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // if you end up colliding with the player
        if (other.gameObject.CompareTag("Player"))
        {
            //Destroy the game object
            Destroy(this);

            //Let the game manager know that the hit happened
            // TO DO: NOT FUCKING THIS. OH MY GOD. 
            Manager.wasHit = true;
            
            
        }
    }
}
