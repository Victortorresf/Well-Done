using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 1f;
    public float blastRadius = 5f;
    public float force = 7000f;
    public GameObject explosionEffect;
    int damage;
    float countDown;
    bool hasExploded = false;
    int minDamage = 99;
    int maxDamage = 100;
    void Start()
    {
        countDown = delay;
        minDamage = 99;
        maxDamage = 100;
        damage = Random.Range(minDamage, maxDamage + 1);
    }

    // Update is called once per frame
    void Update()
    {
        countDown -= Time.deltaTime;
        if (countDown <= 0 && !hasExploded)
        {
            Explode();
            hasExploded = true;
        }
    }

    void Explode()
    {
        GameObject effect = Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);
        foreach(Collider nearbyObject in colliders)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            EnemyAI enemy = nearbyObject.GetComponent<EnemyAI>();
            if(rb != null)
            {
                rb.AddExplosionForce(force, transform.position, blastRadius);
            }
            if(enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            /// aplicar forca
            /// dano
        }
        FindObjectOfType<AudioManager>().Play("Explosion");
        //Destroi granada
        Destroy(gameObject);
        Destroy(effect, 3f);

    }
}
