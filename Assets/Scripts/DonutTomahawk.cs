using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutTomahawk : MonoBehaviour
{
    int minDamage = 40;
    int maxDamage = 45;
    int damage;
    private void Awake()
    {
        minDamage = 40;
        maxDamage = 45;
        damage = Random.Range(minDamage, maxDamage + 1);
    }
    private void OnCollisionEnter(Collision collision)
    {
        EnemyAI enemy = collision.gameObject.GetComponent<EnemyAI>();
        FindObjectOfType<AudioManager>().Play("Donut Hit");
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject, 1f);
        }
    }
    
}
