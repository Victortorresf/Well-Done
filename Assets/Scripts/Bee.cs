using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Bee : MonoBehaviour
{
    NavMeshAgent agent;
    Transform player;
    EnemyAI enemy;
    public GameObject beeHive;

    //Wander
    public float walkPointRange;
    float playerInSightRange, playerInAttackRange;

    GameObject hive;
    int maxDamage = 16;
    int minDamage = 8;
    bool alreadyAttacked = false;
    public float sightRange = 20;
    public float attackRange;
    public float impactForce = 30f;
    public float timeBetweenAttacks = 2f;

    void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        enemy = GetComponent<EnemyAI>();
        if (hive == null)
            hive = GameObject.Find("BeeHive" + Random.Range(1, 5));
    }

    void Update()
    {
        //Check for sight range
        playerInSightRange = Vector3.Distance(transform.position, player.position);
        playerInAttackRange = Vector3.Distance(transform.position, player.position);
        bool nearHive = hive.GetComponent<BeeHive>().playerNear;

        if ((!nearHive || playerInSightRange > sightRange) && enemy.IsAtDestination()) enemy.GotoNextWaypoint();
        if (nearHive) agent.SetDestination(player.position);
        if (nearHive && playerInAttackRange <= attackRange) StingPlayer();

    }
    private void StingPlayer()
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
