using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Shooting : MonoBehaviour
{    
    public const int FryingPan = 1;
    public const int Donut = 2;
    public const int ToasterGun = 3;
    public const int PineappleGrenade = 4;

    public int damage = 10;
    public int range = 20;

    public float fireRate = 4f;
    public float impactForce = 30f;
    private float nextTimetoFire = 0f;
    float throwForce = 25f;
    float donutForce = 100f;

    float reloadTime = 1f;
    public int weaponType = FryingPan;
    int maxRifleAmmo = 80, grenadesAmount = 3, maxDonuts = 5, maxAmmo;
    int minDamage = 20, maxDamage = 40;
    public int currentRifleAmmo, currentGrenades, currentDonuts, currentAmmo;
    public GameObject impact, grenadePrefab, donutPrefab;
    public Text ammoText;
    public Camera fpscam;
    public GameObject playerObject;
    public GameObject[] weapons;
    public GameObject[] windows;
    public AudioManager audioManager;
    Player player;
    public PauseMenu pause;

    private void Start()
    {
        player = playerObject.GetComponent<Player>();
        currentDonuts = maxDonuts;
        currentGrenades = grenadesAmount;
        currentRifleAmmo = maxRifleAmmo;
    }

    void Update()
    {
        UpdateAmmoCount();
        bool isPaused = pause.gameIsPaused;

        if (Input.GetKeyDown("1"))
            SelectWeapon(FryingPan);
        else if (Input.GetKeyDown("2"))
            SelectWeapon(Donut);
        else if (player.level >= 3 && Input.GetKeyDown("3"))
            SelectWeapon(ToasterGun);
        else if (player.level >= 4 && Input.GetKeyDown("4"))
            SelectWeapon(PineappleGrenade);

        if (Input.GetKeyDown("r") && currentAmmo < maxAmmo && weaponType != Donut && weaponType != PineappleGrenade && weaponType != FryingPan)
            StartCoroutine(Reload());

        if ((Input.GetButton("Fire1") && Time.time >= nextTimetoFire) && !isPaused)
        {
            nextTimetoFire = Time.time + 1f / fireRate;
            if ((currentAmmo > 0 && weaponType == ToasterGun) || weaponType == FryingPan)
                Shoot();

            if (weaponType == PineappleGrenade && currentGrenades > 0)
                StartCoroutine(ThrowGrenade());

            if (weaponType == Donut && currentDonuts > 0)
                StartCoroutine(SavingDonut());
        }

        if (Input.GetButtonDown("Fire2"))
            Interact();

        if (Input.GetKeyDown("escape"))
            CloseWindow();   
    }

    void UpdateAmmoCount()
    {
        switch (weaponType)
        {
            case FryingPan:
                currentAmmo = 0;
                maxAmmo = 0;
                break;
            case Donut:
                currentAmmo = currentDonuts;
                maxAmmo = maxDonuts;
                break;
            case ToasterGun:
                currentAmmo = currentRifleAmmo;
                maxAmmo = maxRifleAmmo;
                break;
            case PineappleGrenade:
                currentAmmo = currentGrenades;
                maxAmmo = grenadesAmount;
                break;
        }
    }

    void CloseWindow()
    {
        foreach (GameObject window in windows)
        {
            window.SetActive(false);
        }
        Time.timeScale = 1;
    }

    void SelectWeapon(int type)
    {
        weaponType = type;
        
        // Deactivate all weapons
        foreach (GameObject weapon in weapons)
        {
            weapon.SetActive(false);
        }

        // Activate selected weapon
        weapons[type - 1].SetActive(true); 
        switch (type)
        {
            case FryingPan:
                fireRate = 4f;
                minDamage = 20;
                maxDamage = 40;
                range = 20;
                break;
            case Donut:
                reloadTime = 0.5f;
                break;
            case ToasterGun:
                fireRate = 10f;
                minDamage = 5;
                maxDamage = 15;
                range = 35;
                break;
            case PineappleGrenade:
                fireRate = 0.5f;
                break;
        }
    }
        
    private void Interact()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, 20f))
        {
            Interactable item = hit.transform.GetComponent<Interactable>();
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

        currentAmmo = maxAmmo;
    }

    void RechargeAmmo()
    {
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
            if (weaponType == ToasterGun)
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
                if (weaponType == FryingPan)
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
