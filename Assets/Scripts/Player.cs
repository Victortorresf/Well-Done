using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    //Player
    public int level = 1;
    public int currentHealth;
    public int maxHealth = 100;
    public int speed = 6;

    //Camera Movement
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;

    public Vector3 spawnPoint;
    
    //UI Recipe Status
    public Text ingredients;
    int amount;
    public Image[] iconsProgress;
    
    public HealthBar healthBar;
    public Recipe quest;

   
    public GameObject recipeStatusWindow;
    public GameObject deathWindow;
  
    public CharacterController controller;
    public Transform cam;

    public Vector3 direction;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        healthBar.SetHealth(currentHealth);

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        
         direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (Input.GetKeyDown("m"))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown("h")) currentHealth += 10;

        if (Input.GetKeyDown("l"))
        {
            level++;
        }

        if (Input.GetKey("left shift"))
        {
            speed = 25;
        }
        else speed = 10;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
            
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            controller.SimpleMove(Vector3.down);
        }
        else
        {
            controller.SimpleMove(Vector3.down);
        }

        if (currentHealth <= 0 || transform.position.y <= -10)
        {
            Time.timeScale = 0;
            deathWindow.SetActive(true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        Debug.Log(damage);
    }

    public void UpdateRecipeStatus()
    {
        ingredients.text = "";
        for (int i = 0; i < quest.Ingredients.Length; i++)
        {
            amount = quest.Ingredients[i].amount;
            if(quest.Ingredients[i].currentAmount < amount)
            {
                ingredients.text += quest.Ingredients[i].currentAmount + " / " + amount + "\n";
            }
            else
            {
                ingredients.text += "Done" + "\n";
            }
        }
        IsAllCollected();
    }

    public void IsAllCollected()
    {
        for (int i = 0; i < quest.Ingredients.Length; i++)
        {
            if(quest.Ingredients[i].currentAmount != quest.Ingredients[i].amount)
            {
                quest.ingredientsComplete = false;
                break;
            }
            else
            {
                quest.ingredientsComplete = true;
            }
        }
    }

    public void DisplayRecipeStatus()
    {
        recipeStatusWindow.SetActive(true);
        ingredients.text = "";
        for (int i = 0; i < quest.Ingredients.Length; i++)
        {
            for (int y = 0; y < iconsProgress.Length; y++)
            {
                iconsProgress[i].sprite = quest.Ingredients[i].icon;
                if (iconsProgress[y].sprite != null)
                {
                    iconsProgress[y].gameObject.SetActive(true);
                }
            
            }
                amount = quest.Ingredients[i].amount;
                ingredients.text += quest.Ingredients[i].currentAmount + " / " + amount + "\n";
        }
    }

    public void ReSpawn()
    {
        deathWindow.SetActive(false);
        Time.timeScale = 1;
        transform.position = (spawnPoint);
        if(quest != null)
        {
            quest.deaths++;
            for (int i = 0; i < quest.Ingredients.Length; i++)
            {
               quest.Ingredients[i].currentAmount = 0;
            }
        }
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Inventory.instance.Clear();
        currentHealth = maxHealth;
        healthBar.SetHealth(maxHealth);
        UpdateRecipeStatus();
    }
}
