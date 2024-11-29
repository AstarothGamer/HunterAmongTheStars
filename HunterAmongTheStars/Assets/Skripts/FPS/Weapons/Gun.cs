using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] Transform Gunpoint;
    [SerializeField] CinemachineCamera Cam;
    public Animator anim;
    public int bulletsPerShot = 1;
    public float attackTime = 0.5f;
    public float range = 70f;
    public float damage = 5f;
    public float spread = 0f;

    [Header("Aiming Settings")]

    [Header("Ammo Info")]
    public float reloadTime = .7f;
    public int maxAmmo = 15;

    [Header("Sound Effects")]
    public bool RifleSound = true;
    public bool PistolSound = false;
    public bool ShotgunSound = false;

    [Header("Visual Effects")]
    public ParticleSystem GunSmoke;
    public ParticleSystem MuzleFlash;
    public Light muzzleFlashLight;
    public float flashDuration = 0.05f;
    public float shakeDuration = 2.5f;
    public float shakeAmplitude = 1f;
    [SerializeField] Transform shellEjectionPoint;
    [SerializeField] GameObject shellPrefab;
    [SerializeField] GameObject Impact;
    [SerializeField] CinemachineBasicMultiChannelPerlin noise;

    [Header("Debug")]
    //these are shown in the inspector, but cannot be modified while the game is not running
    [SerializeField] protected int currentAmmo;
    [SerializeField] protected float nextShotMinTime = 0; //when can the next attack be fired
    [SerializeField] protected bool isReloading;


    protected void Awake()
    {
        currentAmmo = maxAmmo;
        if (muzzleFlashLight != null )
        muzzleFlashLight.enabled = false;
        if (noise == null)
        noise = Cam.GetComponent<CinemachineBasicMultiChannelPerlin>();
        if (noise)
        noise.FrequencyGain = 0f;
    }

    public virtual void Reload()
    {
        if (currentAmmo == maxAmmo)
            return;

        Debug.Log("Reloaing");
        //TODO: add animation/sound
        StartCoroutine(DoReload());
    }


    protected IEnumerator DoReload()
    {
        isReloading = true;
        anim.SetBool("Reloading", true);
        AudioManager.PlaySound(SoundType.Reloading, 0.5f);

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        isReloading = false;
        anim.SetBool("Reloading", false);
    }

    public virtual void Update()
    {
        if (Input.GetKeyDown(KeyCode.R) && !isReloading)
        {
            Reload();
        }
        /*
        if (Input.GetMouseButton(1) && !isReloading)
        {
            Aim();
        }
        else
        {
            StopAiming();
        }
        */

        if (Input.GetButtonDown("Fire1") && !isReloading)
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
            AudioManager.PlaySound(SoundType.RifleShot, 0.5f);
        if (ShotgunSound)
            AudioManager.PlaySound(SoundType.ShotgunShot, 0.6f);
        if (PistolSound)
            AudioManager.PlaySound(SoundType.PistolShot, 0.5f);
        for (int i = 0; i < bulletsPerShot; i++)
        {
            //Spread
            float x = Random.Range(-spread, spread);
            float y = Random.Range(-spread, spread);
            float z = Random.Range(-spread, spread);

            // Calculate the direction considering spread
            Vector3 spreadDirection = Cam.transform.forward + new Vector3(x, y, z);
            RaycastHit hit;
            if (Physics.Raycast(Cam.transform.position, spreadDirection, out hit, range))
            {
                Debug.Log(hit.transform.name);

                Damageable target = hit.transform.GetComponent<Damageable>();
                if (target != null)
                {
                    Debug.Log("Damaging");
                    target.Damage(damage);
                }
                
                Instantiate(Impact, hit.point, Quaternion.identity);
            }

        }
        if (anim != null)
            anim.SetTrigger("Shot");
        if (GunSmoke != null)
            GunSmoke.Play();
        if (MuzleFlash != null)
            MuzleFlash.Play();
        // Trigger the flash
        if (muzzleFlashLight != null)
            StartCoroutine(Flash());
        //if (shakeDuration != 0 && shakeAmplitude != 0)
            //CamShake.Instance.Shake(shakeDuration, shakeAmplitude);

        nextShotMinTime = Time.time + attackTime;
        currentAmmo--;

        if (shellEjectionPoint != null && shellPrefab != null)
            EjectShell();
    }
    private IEnumerator Flash()
    {
        if (muzzleFlashLight)
        muzzleFlashLight.enabled = true;

        float normalFOV = Cam.Lens.FieldOfView;
        Cam.Lens.FieldOfView = Cam.Lens.FieldOfView + 3f;

        if (noise)
        noise.FrequencyGain = shakeAmplitude;  // Shake

        // Wait for the flash duration
        yield return new WaitForSeconds(flashDuration);

        if (muzzleFlashLight)
        muzzleFlashLight.enabled = false;

        Cam.Lens.FieldOfView = normalFOV;

        // Wait for the flash duration
        yield return new WaitForSeconds(shakeDuration);

        if (noise)
        noise.FrequencyGain = 0f;  // Shake
    }
    void EjectShell()
    {
        GameObject shell = Instantiate(shellPrefab, shellEjectionPoint.position, shellEjectionPoint.rotation);
        Rigidbody shellRigidbody = shell.GetComponent<Rigidbody>();

        // Apply a small force to eject the shell
        Vector3 ejectionForce = shellEjectionPoint.up * Random.Range(0.8f, 1.0f); // Randomize force slightly for variation
        shellRigidbody.AddForce(ejectionForce, ForceMode.Impulse);

        // Apply some spin
        Vector3 randomTorque = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        shellRigidbody.AddTorque(randomTorque * 2f, ForceMode.Impulse);
    }
}