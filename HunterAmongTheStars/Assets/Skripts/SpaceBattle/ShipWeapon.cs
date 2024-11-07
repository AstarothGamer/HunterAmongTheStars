using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipWeapon : MonoBehaviour
{
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] Transform projectileSpawnpoint;


    [Header("General")]
    public float projectileSpeed = 50;
    public int projectilesPerShot = 1;
    public float projectileLifetime = 3;
    public float projectileSpeedVariation = 1;
    public float projectileSpread = 0.5f;
    public float attackSpeed = 0.5f;
    public float damage = 25f;

    [Header("Ammo Info")]
    public float reloadSpeed = .7f;
    public int maxAmmo = 15;

    [Header("Sound Effects")]
    public bool RifleSound = true;
    public bool PistolSound = false;
    public bool ShotGunSound = false;

    [Header("Visual Effects")]
    public ParticleSystem MuzleFlash;
    public Light muzzleFlashLight;
    public float flashDuration = 0.05f;
    [SerializeField] Transform shellEjectionPoint;
    [SerializeField] GameObject shellPrefab;

    [Header("Debug")]
    //these are shown in the inspector, but cannot be modified while the game is not running
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected float nextShotMinTime = 0; //when can the next attack be fired
    [SerializeField] protected bool isReloading;

    protected void Awake()
    {
        currentAmmo = maxAmmo;
        //muzzleFlashLight.enabled = false;
    }

    public virtual void Reload()
    {
        if (currentAmmo == maxAmmo)
            return;

        Debug.Log("Reloaing");
        int reloadAmount = maxAmmo - currentAmmo;

        //TODO: add animation/sound
        StartCoroutine(DoReload(reloadAmount));
    }


    protected IEnumerator DoReload(int reloadAmount)
    {
        isReloading = true;
        yield return new WaitForSeconds(reloadSpeed);

        currentAmmo += reloadAmount;
        isReloading = false;
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            Reload();
        }
        if (Input.GetButton("Fire1") && !isReloading)
        {
            if (currentAmmo <= 0)
            {
                Reload();
                return;
            }

            if (nextShotMinTime > Time.time)
                return;

            Attack();
        }

    }



    public virtual void Attack()
    {
        if (RifleSound)
            AudioManager.PlaySoundAtPoint(SoundType.RifleShot, projectileSpawnpoint.position, 0.9f);
        if (ShotGunSound)
            AudioManager.PlaySoundAtPoint(SoundType.ShotgunShot, projectileSpawnpoint.position, 0.8f);
        if (PistolSound)
            AudioManager.PlaySoundAtPoint(SoundType.PistolShot, projectileSpawnpoint.position, 0.8f);
        for (int i = 0; i < projectilesPerShot; i++)
        {
            var go = Instantiate(projectilePrefab, projectileSpawnpoint.position, GetProjectileDirection());
            var proj = go.GetComponent<Projectile>();
            InitializeProjectile(proj);
        }
        if (MuzleFlash != null)
            MuzleFlash.Play();
        // Trigger the flash
        //StartCoroutine(Flash());
        nextShotMinTime = Time.time + attackSpeed;
        currentAmmo--;
        EjectShell();
    }

    //inheriting classes can override this to make it easier to have different types of projectiles
    protected virtual void InitializeProjectile(Projectile projectile)
    {
        projectile.Initialize(damage, GetProjectileSpeed(), projectileLifetime);
    }

    protected float GetProjectileSpeed()
    {
        return projectileSpeed + Random.Range(0, projectileSpeedVariation) - projectileSpeedVariation / 2;
    }

    protected Quaternion GetProjectileDirection()
    {

        var variation = Random.Range(-projectileSpread, projectileSpread);
        return Quaternion.Euler(projectileSpawnpoint.rotation.eulerAngles + Vector3.forward * variation);
    }
    private IEnumerator Flash()
    {
        muzzleFlashLight.enabled = true;

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        muzzleFlashLight.enabled = false;
    }
    void EjectShell()
    {
        GameObject shell = Instantiate(shellPrefab, shellEjectionPoint.position, shellEjectionPoint.rotation);
        Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();

        // Apply a small force to eject the shell
        Vector3 ejectionForce = shellEjectionPoint.up * Random.Range(0.5f, 1.0f); // Randomize force slightly for variation
        shellRigidbody.AddForce(ejectionForce, ForceMode.Impulse);

        // Apply some spin
        Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        shellRigidbody.AddTorque(randomTorque * 2f, ForceMode.Impulse);
    }
}