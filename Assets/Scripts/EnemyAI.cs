using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    GameObject item;
    public GameObject dropItem1;
    public GameObject dropItem2;
    public Ingredient ingredient1;
    public Ingredient ingredient2;
    private NavMeshAgent agent;
    public int health = 120;
    public Transform[] waypoints;
    private int currentWaypoint;
    ReSpawnEnemy spawn;


    private void Start()
    {
        spawn = GameObject.Find(gameObject.name + (" SpawnPoint")).GetComponent<ReSpawnEnemy>();
        agent = GetComponent<NavMeshAgent>();
        GotoNextWaypoint();
    }

    public bool IsAtDestination()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if ((!agent.hasPath) || (agent.velocity.sqrMagnitude == 0))
                {
                    return true;
                }
            }
        }
        return false;
    }

    public void GotoNextWaypoint()
    {
        currentWaypoint = UnityEngine.Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[currentWaypoint].position);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0) DestroyEnemy();
    }

    private void DestroyEnemy()
    {
        int dropChance = UnityEngine.Random.Range(1, 4);
        float chosen = UnityEngine.Random.Range(1, 3);
        if (dropItem1 != null)
        {
            if (chosen == 1)
            {
                item = dropItem1;
                dropItem1.GetComponent<Interactable>().item = ingredient1;
            }
            if (chosen == 2)
            {
                item = dropItem2;
                dropItem2.GetComponent<Interactable>().item = ingredient2;
            }
        }
        if (dropChance == 2 && item != null)
        {
            Instantiate(item, transform.position, Quaternion.identity); 
        }
        spawn.Death = true;
        spawn.enemyWaypoints = waypoints;
        FindObjectOfType<AudioManager>().Play(gameObject.name);
        
        Destroy(gameObject);
    }
}
