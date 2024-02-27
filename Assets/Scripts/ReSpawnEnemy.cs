using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawnEnemy : MonoBehaviour
{
    public bool Death;
    float Timer;
    public float Cooldown;
    public GameObject Enemy;
    GameObject LastEnemy;
    public Transform[] enemyWaypoints;

    void Start()
    {
        Death = false;
    }

    void Update()
    {
        if (Death == true)
        {
            Timer += Time.deltaTime;
        }
        //If the timer is bigger than cooldown.
        if (Timer >= Cooldown)
        {
            LastEnemy = Instantiate(Enemy, transform.position, Quaternion.identity);
            LastEnemy.name = Enemy.name;
            SetWaypoints(enemyWaypoints);
            
            //My enemy won't be dead anymore.
            Death = false;
            //Timer will restart.
            Timer = 0;
        }
    }

    public void SetWaypoints(Transform[] enemyWaypoints)
    {
       // Debug.Log(enemyWaypoints.Length);
        LastEnemy.GetComponent<EnemyAI>().waypoints = new Transform[enemyWaypoints.Length];

        for(int i = 0; i < enemyWaypoints.Length; i++)
        {
            LastEnemy.GetComponent<EnemyAI>().waypoints[i] = enemyWaypoints[i];
        }
        Death = false;
    }
}
