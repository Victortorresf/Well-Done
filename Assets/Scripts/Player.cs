using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // Player Stats
    public int level { get; private set; } = 1;
    public int currentHealth;
    public int maxHealth = 100;
    public int Speed { get; private set; } = 10;

    // Camera Movement
    public float TurnSmoothTime = 0.1f;

    // UI Elements
    public HealthBar healthBar;
    public GameObject deathWindow;

    // Other Components
    public CharacterController Controller;
    public Transform Cam;

    private Vector3 _spawnPoint;
    private float _turnSmoothVelocity;
    public Vector3 _direction;

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

        healthBar.SetHealth(currentHealth);

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
            currentHealth += 10;
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            level++;
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

    public void ReSpawn()
    {
        deathWindow.SetActive(false);
        Time.timeScale = 1;
        transform.position = (_spawnPoint);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked; 
        currentHealth = maxHealth;
        healthBar.SetHealth(maxHealth);   
    }
}
