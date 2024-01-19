using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jump; 
    private Rigidbody2D rb;
    private bool isGrounded; 

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }


    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        // Using default unity jump function
        if (Input.GetButtonDown("Jump")&& isGrounded)
        {
            // add force onto the rigid body
            rb.AddForce(Vector2.up * jump);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Ground collision
        // Check if the collision that's being colided with has the "Ground" tag in the inspector
        if(other.gameObject.CompareTag("Ground"))
        {
            //if it does? bool is true
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        // Ground collision
        // Copy and pasted from above, just the opposite this time
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
