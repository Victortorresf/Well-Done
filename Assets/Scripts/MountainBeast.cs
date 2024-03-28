using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MountainBeast : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    EnemyAI enemy;

    //Attack variables
    public int sightRange = 20;
    float playerInSightRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyAI>();
    }

    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Vector3.Distance(transform.position, player.position);
        
        if (playerInSightRange > sightRange && enemy.IsAtDestination()) enemy.GotoNextWaypoint(); //------------------------ try deleting this
        
        if (playerInSightRange <= sightRange)
        {
            ChasePlayer();
        }

        if (enemy.health <= 0)
            FindObjectOfType<AudioManager>().Play("Mountain");
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }
}
