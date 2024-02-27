using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BlueMonster : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    EnemyAI enemy;

    //Attack variables
    public float impactForce = 30f;
    public float timeBetweenAttacks = 2f;
    public float walkPointRange, attackRange;
    int maxDamage = 21;
    int minDamage = 10;
    bool alreadyAttacked = false;
    float playerInSightRange, playerInAttackRange;
    public float sightRange = 20f;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyAI>();
    }

    void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Vector3.Distance(transform.position, player.position);
        playerInAttackRange = Vector3.Distance(transform.position, player.position);

        if (playerInSightRange > sightRange && playerInAttackRange > attackRange && enemy.IsAtDestination()) enemy.GotoNextWaypoint();
        if (playerInSightRange <= sightRange && playerInAttackRange > attackRange) ChasePlayer();
        if (playerInSightRange < sightRange && playerInAttackRange <= attackRange) AttackPlayer();
    }


    private void ChasePlayer()
    {
        agent.speed = 10;
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            //Attack Code
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, attackRange))
            {
                Player target = hit.transform.GetComponent<Player>();
                if (target != null)
                {
                    int randomDamage = Random.Range(minDamage, maxDamage);
                    target.TakeDamage(randomDamage);
                }
                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactForce);
                }
            }
            alreadyAttacked = true;
            /////   
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }
}
