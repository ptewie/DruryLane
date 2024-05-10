using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    // Floats
    public float maxHealth = 3f;
    public float currentHealth  = 3f; 
    UnityEvent OnTakeDamage;
    UnityEvent OnDeath;



    public void TakeDamage(float damage)
    {
        //When hit, subtract health (duh)
        currentHealth -= damage;

        //Active OnTakeDamage event
        OnTakeDamage.Invoke();

        //Death check
        if (currentHealth <= 0)
        {
            //If player is out of health activate OnDeath
            OnDeath.Invoke();
        } 




    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
