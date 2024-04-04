using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animations : MonoBehaviour
{
    Animator animator;
    Player player;
    int weapon;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        weapon = (int) GetComponentInChildren<Shooting>().weaponType;
        Vector3 moveDirection = player._direction;
        bool runPressed = Input.GetKey("left shift");
        animator.SetInteger("Weapon Type", weapon);

        if(player.currentHealth <= 0)
        {
            animator.SetBool("Dye", true);
        }
        else
        {
            animator.SetBool("Dye", false);
        }

        //Movement
        if (moveDirection != Vector3.zero && !runPressed)
        {
            Walk();
        }
        if (moveDirection != Vector3.zero && runPressed)
        {
            Run();
        }
        else if(moveDirection == Vector3.zero)
        {
            Idle();
        }

        //Toaster
        if ((moveDirection != Vector3.zero && !runPressed) && weapon == 3)
        {
            PistolWalk();
        }
        if(moveDirection == Vector3.zero && weapon == 3)
        {
            PistolIdle();
        }

        //Attack
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            MelleeAttack();
        }
    }

    //Movement
    private void Idle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
    private void Walk()
    {   
        animator.SetFloat("Speed", 2, 0.1f, Time.deltaTime);
    }
    private void Run()
    {
        animator.SetFloat("Speed", 4, 0.1f, Time.deltaTime);
    }

    //Pistol
    private void PistolIdle()
    {
        animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
    }
    private void PistolWalk()
    {
        animator.SetFloat("Speed", 1, 0.1f, Time.deltaTime);
    }


    //Attack
    private void MelleeAttack()
    {
        animator.SetTrigger("Attack");
        animator.SetInteger("Weapon Type", weapon);
    }
}
