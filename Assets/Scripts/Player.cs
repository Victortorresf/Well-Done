using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Player Stats
    public int Level { get; private set; } = 1;
    public int CurrentHealth { get; private set; }
    public int MaxHealth = 100;
    public int Speed { get; private set; } = 10;

    // Camera Movement
    public float TurnSmoothTime = 0.1f;

    // UI Elements
    public Text IngredientsText;
    public Image[] IconsProgress;
    public HealthBar HealthBar;
    public GameObject RecipeStatusWindow;
    public GameObject DeathWindow;

    // Other Components
    public CharacterController Controller;
    public Transform Cam;

    private Vector3 _spawnPoint;
    private float _turnSmoothVelocity;
    private Vector3 _direction;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        HandleInput();

        HealthBar.SetHealth(CurrentHealth);

        MovePlayer();
        
        CheckDeath();
    }

    private void HandleInput() // This function checks the keys pressed to move the player or the use of the protoype cheat codes.
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        _direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (Input.GetKeyDown(KeyCode.M))
        {
            TakeDamage(10);
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            CurrentHealth += 10;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Level++;
        }

        Speed = Input.GetKey(KeyCode.LeftShift) ? 25 : 10;
    }

    private void MovePlayer()
    {
        if (_direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg + Cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            Controller.Move(moveDir.normalized * Speed * Time.deltaTime);
            Controller.SimpleMove(Vector3.down);
        }
        else
        {
            Controller.SimpleMove(Vector3.down);
        }
    }

    private void CheckDeath()
    {
        if (CurrentHealth <= 0 || transform.position.y <= -10)
        {
            Time.timeScale = 0;
            DeathWindow.SetActive(true);
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
