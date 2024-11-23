using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHP : MonoBehaviour
{
    [SerializeField] private float currentHealth;
    public float maxHealth = 100;
    public bool isDead;
    public bool isVulnerable = true;

    [Header("Health Bar")]
    public bool useHealthBar = true;
    public bool hideBar = true;
    private bool showBar;
    public Image healthBarBG;
    public Image healthBar;
    public float healthBarDuration = 1f;
    public float healthBarWidthPercent = .3f;
    public float healthBarHeightPercent = .015f;
    public CanvasGroup healtBarCG;
    private float healthBarWidth;
    private float healthBarHeight;

    [Header("Damage Overlay")]
    public bool useDamageOverlay = true;
    private bool showOverlay;
    public float DamageOverlayDuration = 1f;
    public CanvasGroup DamageOverlayCG;

    [Header("Death screen")]
    public GameObject DeathScreen;

    protected void Start()
    {
        #region Damage Overlay

        if (useDamageOverlay)
        {
            DamageOverlayCG.gameObject.SetActive(true);

            DamageOverlayCG.alpha = 0;
        }
        else
        {
            DamageOverlayCG.gameObject.SetActive(false);
        }

        #endregion

        #region Health Bar

        if (useHealthBar)
        {
            healtBarCG.gameObject.SetActive(true);
            healthBar.gameObject.SetActive(true);

            float screenWidth = Screen.width;
            float screenHeight = Screen.height;

            healthBarWidth = screenWidth * healthBarWidthPercent;
            healthBarHeight = screenHeight * healthBarHeightPercent;

            healthBarBG.rectTransform.sizeDelta = new Vector3(healthBarWidth, healthBarHeight, 0f);
            healthBar.rectTransform.sizeDelta = new Vector3(healthBarWidth - 2, healthBarHeight - 2, 0f);

            if (hideBar)
            {
                healtBarCG.alpha = 0;
            }
        }
        else
        {
            healthBarBG.gameObject.SetActive(false);
            healthBar.gameObject.SetActive(false);
        }

        #endregion

        if (DeathScreen != null)
        DeathScreen.SetActive(false);
    }
    protected void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        currentHealth = maxHealth;

        if (hideBar)
        showBar = false;
        showOverlay = false;
    }

    public virtual void Update()
    {
        // Handles HealthBar 
        if (useHealthBar && healthBar != null && !isDead)
        {
            float healthPercent = currentHealth / maxHealth;
            healthBar.transform.localScale = new Vector3(healthPercent, 1f, 1f);
        }

        if (hideBar && useHealthBar && !showBar)// to hide the bar
        {
            healtBarCG.alpha -= 3 * Time.deltaTime;
        }
        if (useDamageOverlay && !showOverlay)// to hide the bar
        {
            DamageOverlayCG.alpha -= 3 * Time.deltaTime;
        }
    }

    // Damage the player and trigger the health bar visability
    public virtual void DamagePlayer(float damage)
    {
        if (isDead || !isVulnerable || damage <= 0)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Death();
        }
        else
        {
            if (hideBar && useHealthBar)// to show the bar
            {
                StartCoroutine(ShowHealthBar());
            }
            if (useDamageOverlay)// to show the bar
            {
                StartCoroutine(ShowDamageOverlay());
            }
        }
    }
    private IEnumerator ShowHealthBar()
    {
        healtBarCG.alpha = 1;
        showBar = true;
   
        yield return new WaitForSeconds(healthBarDuration);

        showBar = false;
    }
    private IEnumerator ShowDamageOverlay()
    {
        DamageOverlayCG.alpha = 1;
        showOverlay = true;

        yield return new WaitForSeconds(DamageOverlayDuration);

        showOverlay = false;
    }

    public void HealPlayer(float amount)
    {
        if (isDead || amount <= 0)
            return;

        if (currentHealth + amount > maxHealth)
            currentHealth = maxHealth;
        else
            currentHealth += amount;

        //play SFX
    }

    public virtual void Death()
    {
        if (isDead) return; // Prevent multiple death calls

        isDead = true;

        // Detach the camera from the player
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            mainCamera.transform.parent = null; // Detach from player
        }

        Cursor.lockState = CursorLockMode.None;
        //DisablePlayerControls();

        // Show death screen
        if (DeathScreen != null)
        {
            DeathScreen.SetActive(true);
        }

        // Disable player object
        gameObject.SetActive(false);
    }
}
