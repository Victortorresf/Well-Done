using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Worm : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    EnemyAI enemy;

    //Wander
    public float walkPointRange;
    float sightRange = 20;
    float playerInSightRange;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyAI>();
    }

    void Update()
    {
        //Check for sight range
        playerInSightRange = Vector3.Distance(transform.position, player.position);

        if (playerInSightRange <= sightRange) Flee();
        if (playerInSightRange > sightRange && enemy.IsAtDestination()) enemy.GotoNextWaypoint();
    }

    private void Flee()
    {
        agent.speed = 20;
        Vector3 distance = transform.position - player.transform.position;
        Vector3 fleePoint = transform.position + distance;
        agent.SetDestination(fleePoint);
    }
}
