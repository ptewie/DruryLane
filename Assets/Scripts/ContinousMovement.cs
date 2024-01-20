using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContinousMovement : MonoBehaviour
{
    private GameManager Manager;
    public float speed = 10.0f; // defualt value
    public float resetThreshold = 100.0f;
    private float currentX;
    public bool shouldUpdateX = true; // To control whether or not X should be updated 

    // Start is called before the first frame update
    void Start()
    {
        Manager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        currentX = transform.position.x;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (shouldUpdateX)
        {
            // increase the x postion constantly based on speed
            currentX += speed * Time.deltaTime;



            // Check if X exceedes the reset threshold 
            // (Something tells me constnatly icnreasing X with no fail safe is a bad idea.)
            if (currentX > resetThreshold)
            {
                // Reset x to 0
                //currentX = 14.0f; //note: commenting out for now. looks really bad when i do this.

            }
            //Update camera position
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
        }
        if (Manager.wasHit == true)
        {
            // if wasHit is true, reduce the speed to 3
            speed = 3.0f;
            // start Courotuine
            StartCoroutine(SpeedDelay());
        }
        
    }

    IEnumerator SpeedDelay()
    {
        //wait for half a second
        yield return new WaitForSeconds(0.5f);
        // Okay, now set back the previous values 
        speed = 10.0f;
        Manager.wasHit = false;
    }
}
