using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCloud : MonoBehaviour
{
    ParticleSystem cloud;
    ParticleSystem.ShapeModule gas;
    float timeBetweenExpansion = 1.5f;
    bool alreadyAttacked = false;
    int minDamage =  15;
    int maxDamage = 31;
    int randomDamage;

    void Start()
    {
        cloud = GetComponentInChildren<ParticleSystem>();
        cloud.Play();
    }

    void Update()
    {  
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            Erupt(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
            Erupt(other.gameObject);
       
    }

    private void Erupt(GameObject other)
    {
        if (!alreadyAttacked)
        {
            //Attack Code
            randomDamage = Random.Range(minDamage, maxDamage);
            other.GetComponent<Player>().TakeDamage(randomDamage);
            /////   
            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenExpansion);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
