using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{    
    public int damage = 10;
    public int range = 20;

    public float fireRate = 4f;
    public float impactForce = 30f;
    private float nextTimetoFire = 0f;
    float throwForce = 25f;
    float donutForce = 100f;

    float reloadTime = 1f;
    public float weaponType = 1;
    int reloadGrenade, gunsReload, reloadAmount;
    int maxRifleAmmo, grenadesAmount, maxDonuts, maxAmmo;
    int minDamage, maxDamage;
    public int currentRifleAmmo, currentGrenades, currentDonuts, currentAmmo;

    public GameObject impact;
    public GameObject grenadePrefab, donutPrefab;
    public GameObject donut, pan, toaster, grenade;
    public Text ammoText;
    public Camera fpscam;
    public GameObject playerObject;
    public NPC npc;
    public GameObject[] windows;
    public AudioManager audioManager;
    Player player;
    public PauseMenu pause;

    private void Start()
    {
        player = playerObject.GetComponent<Player>();
        maxRifleAmmo = 80;
        gunsReload = 3;
        grenadesAmount = 3;
        maxDonuts = 5;
        minDamage = 20;
        maxDamage = 40;
        currentDonuts = maxDonuts;
        currentGrenades = grenadesAmount;
        currentRifleAmmo = maxRifleAmmo;
    }

    void Update()
    {
        UpdateAmmoCount();
        bool isPaused = pause.gameIsPaused;
        //Switch/Reload and Unlock weapons by level
        if (Input.GetKeyDown("1"))//frying pan
        {
            weaponType = 1;
            fireRate = 4f;
            minDamage = 20;
            maxDamage = 40;
            range = 20;
            pan.SetActive(true);
            donut.SetActive(false);
            toaster.SetActive(false);
            grenade.SetActive(false);
        }
        
        if (Input.GetKeyDown("2"))//Donut
        {
            weaponType = 2;
            currentAmmo = currentDonuts;
            maxAmmo = maxDonuts;
            reloadAmount = 0;
            pan.SetActive(false);
            donut.SetActive(true);
            toaster.SetActive(false);
            grenade.SetActive(false);
        }

        if (player.level >= 3)//toaster gun
        {
            if (Input.GetKeyDown("3"))
            {
                weaponType = 3;
                reloadTime = 1f;
                fireRate = 10f;
                minDamage = 5;
                maxDamage = 15;
                range = 35;
                pan.SetActive(false);
                donut.SetActive(false);
                toaster.SetActive(true);
                grenade.SetActive(false);
            }         
        }
        if (player.level >= 4)
        {
            if (Input.GetKeyDown("4"))//PineApple Grenade
            {
                weaponType = 4;
                fireRate = 0.5f;
                pan.SetActive(false);
                donut.SetActive(false);
                toaster.SetActive(false);
                grenade.SetActive(true);
            }
        }

        if (Input.GetKeyDown("r") && reloadAmount > 0 && weaponType != 2 && weaponType != 5 && weaponType != 1 && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        //Firing and Intercating
        if ((Input.GetButton("Fire1") && Time.time >= nextTimetoFire) && !isPaused)
        {
            nextTimetoFire = Time.time + 1f/ fireRate;
            if((currentAmmo > 0 && weaponType == 3) || weaponType == 1)
                Shoot(); 

            if (weaponType == 4 && currentGrenades > 0)
                StartCoroutine(ThrowGrenade());

            if (weaponType == 2 && currentDonuts > 0)
                StartCoroutine(SavingDonut());
        }

        if (Input.GetButtonDown("Fire2"))
        {
            Interact();
        }

        if (Input.GetKeyDown("f") && npc.givenRecipeWindow.activeInHierarchy)
        {
            npc.AcceptRecipe();
        }

        if (Input.GetKeyDown("escape"))
                CloseWindow();      
    }


    void UpdateAmmoCount()
    {
        ammoText.text = currentAmmo + " / " + maxAmmo + " x " + reloadAmount;

        if (weaponType == 1)//pan
        {
            currentAmmo = 0;
            maxAmmo = 0;
            reloadAmount = 0;
        }
        else if (weaponType == 2)//donut
        {
            currentAmmo = currentDonuts;
            maxAmmo = maxDonuts;
            reloadAmount = 0;
        }
        else if(weaponType == 3)//rifle
        {
            currentAmmo = currentRifleAmmo;
            maxAmmo = maxRifleAmmo;
            reloadAmount = gunsReload;
        }
        else if(weaponType == 4)//grenade
        {
            currentAmmo = currentGrenades;
            maxAmmo = grenadesAmount;
            reloadAmount = reloadGrenade;
        }
    }

    void CloseWindow()
    {
        for (int i = 0; windows.Length > 0; i++)
        {
            windows[i].SetActive(false);  
            Time.timeScale = 1;
        }
    }

    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, 20f))
        {
            
            Interactable item = hit.transform.GetComponent<Interactable>();
            npc = hit.transform.GetComponent<NPC>();
            Door door = hit.transform.GetComponent<Door>();
            if (hit.transform.gameObject.CompareTag("Ammunition")) 
            {
                RechargeAmmo();
            }

            if (item != null)
            {            
                item.player = player;
                item.Collect(item.item);
                
            }

            if(npc != null)
            {
                npc.InteractWithPlayer();
            }
            if (door != null)
            {
                door.OpenDoor();
            }
        }
    }

    IEnumerator Reload()
    {
        Debug.Log("Reloading....");

        yield return new WaitForSeconds(reloadTime);

        if (weaponType == 3)
        {
            currentAmmo = maxAmmo;
            currentRifleAmmo = maxAmmo;
        }
        gunsReload--;
        reloadAmount--;
    }

    void RechargeAmmo()
    {
        gunsReload = 3;
        reloadAmount = gunsReload;
        grenadesAmount = 3;
        currentGrenades = grenadesAmount;
        currentRifleAmmo = maxRifleAmmo;
        currentDonuts = maxDonuts;
    }

    public void Shoot()
    {
        damage = Random.Range(minDamage, maxDamage + 1);
        
        RaycastHit hit;
        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, range))
        {
            if (weaponType == 3)
            {
                currentAmmo--;
                currentRifleAmmo--;
                audioManager.Play("Toaster");
                GameObject effect = Instantiate(impact, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(effect, 1f);
            }

            EnemyAI target = hit.transform.GetComponent<EnemyAI>();
            if (target != null)
            {
                Vector3 lookPosition = new Vector3(hit.point.x, player.gameObject.transform.position.y, hit.point.z);
                player.gameObject.transform.LookAt(lookPosition, Vector3.up);
                if (weaponType == 1)
                    audioManager.Play("Pan");

                target.TakeDamage(damage);
            }

            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
        }   
    }

    IEnumerator ThrowGrenade()
    {
        yield return new WaitForSeconds(0.5f);

        FindObjectOfType<AudioManager>().Play("Throw");
        currentGrenades--;
        currentAmmo--;
        GameObject grenade = Instantiate(grenadePrefab, transform.position, fpscam.transform.rotation);
        Rigidbody rb = grenade.GetComponent<Rigidbody>();
        rb.AddForce(fpscam.transform.forward * throwForce, ForceMode.VelocityChange);
    }

    IEnumerator SavingDonut()
    {
        yield return new WaitForSeconds(0.5f);

        currentDonuts--;
        currentAmmo--;
        FindObjectOfType<AudioManager>().Play("Donut");
        GameObject donut = Instantiate(donutPrefab, transform.position, fpscam.transform.rotation);
        Rigidbody rb = donut.GetComponent<Rigidbody>();
        rb.AddForce(fpscam.transform.forward * donutForce, ForceMode.VelocityChange);
    }
}
