using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bird : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    EnemyAI enemy;  

    //Attack variables
    public float walkPointRange, attackRange, sightRange;
    public float impactForce = 30f;
    public float timeBetweenAttacks = 2f;
    int maxDamage = 26;
    int minDamage = 15;
    bool alreadyAttacked = false;
    float playerInSightRange, playerInAttackRange;

    //Guard
    bool stillthere;
    public GameObject egg;

    void Awake()
    {   
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyAI>();
        if (egg == null)
            egg = GameObject.Find("Egg" + Random.Range(1, 4));
    }

    void Update()
    {
        //Check for sight range
        playerInSightRange = Vector3.Distance(transform.position, player.position);
        playerInAttackRange = Vector3.Distance(transform.position, player.position);

        stillthere = egg.GetComponent<Interactable>().stillThere;

        if ((stillthere && enemy.IsAtDestination()) || (playerInSightRange > sightRange && enemy.IsAtDestination())) enemy.GotoNextWaypoint();
        
        if (!stillthere && playerInSightRange <= sightRange)
        {
            ChasePlayer();
            if (playerInSightRange < sightRange && playerInAttackRange <= attackRange)
                AttackPlayer();
        }

        if (enemy.health <= 0)
            FindObjectOfType<AudioManager>().Play("Bird");
    }

    private void ChasePlayer()
    {
        agent.speed = 15;
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
                Vector3 lookPosition = new Vector3(hit.point.x, gameObject.transform.position.y, hit.point.z);
                gameObject.transform.LookAt(lookPosition, Vector3.up);

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
