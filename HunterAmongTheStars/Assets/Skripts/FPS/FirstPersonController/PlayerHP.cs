using System.Collections;
using System.Collections.Generic;
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

    protected void Start()
    {
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
        else if (hideBar && useHealthBar)// to show the bar
        {
            StartCoroutine(ShowHealthBar()); 
        }
    }
    private IEnumerator ShowHealthBar()
    {
        healtBarCG.alpha = 1;
        showBar = true;
   
        yield return new WaitForSeconds(healthBarDuration);

        showBar = false;
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
        isDead = true;
        gameObject.SetActive(false);
    }
}
